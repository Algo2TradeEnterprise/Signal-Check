﻿Imports Algo2TradeBLL
Imports System.Threading
Public Class PinocchioBarFormation
    Inherits Rule
    Public Sub New(ByVal canceller As CancellationTokenSource, ByVal stockCategory As String, ByVal stockName As String, ByVal timeFrame As Integer, ByVal useHA As Boolean)
        MyBase.New(canceller, stockCategory, stockName, timeFrame, useHA)
    End Sub
    Public Overrides Async Function RunAsync(startDate As Date, endDate As Date) As Task(Of DataTable)
        Await Task.Delay(0).ConfigureAwait(False)
        Dim ret As New DataTable
        ret.Columns.Add("Date")
        ret.Columns.Add("Instrument")
        ret.Columns.Add("Signal")
        ret.Columns.Add("VWAP")
        ret.Columns.Add("CurrentDayHighLow")
        ret.Columns.Add("PreviousDayHighLow")
        Dim stockData As StockSelection = New StockSelection(_canceller, _category, _name)
        AddHandler stockData.Heartbeat, AddressOf OnHeartbeat
        AddHandler stockData.WaitingFor, AddressOf OnWaitingFor
        AddHandler stockData.DocumentRetryStatus, AddressOf OnDocumentRetryStatus
        AddHandler stockData.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
        Dim chkDate As Date = startDate
        While chkDate <= endDate
            _canceller.Token.ThrowIfCancellationRequested()
            Dim stockList As List(Of String) = Nothing
            If _name Is Nothing OrElse _name = "" Then
                stockList = Await stockData.GetStockList(chkDate).ConfigureAwait(False)
            Else
                stockList = New List(Of String)
                stockList.Add(_name)
            End If
            _canceller.Token.ThrowIfCancellationRequested()
            If stockList IsNot Nothing AndAlso stockList.Count > 0 Then
                For Each stock In stockList
                    _canceller.Token.ThrowIfCancellationRequested()
                    Dim stockPayload As Dictionary(Of Date, Payload) = Nothing
                    Select Case _category
                        Case "Cash"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.Intraday_Cash, stock, chkDate.AddDays(-7), chkDate)
                        Case "Currency"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.Intraday_Currency, stock, chkDate.AddDays(-7), chkDate)
                        Case "Commodity"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.Intraday_Commodity, stock, chkDate.AddDays(-7), chkDate)
                        Case "Future"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.Intraday_Futures, stock, chkDate.AddDays(-7), chkDate)
                        Case Else
                            Throw New NotImplementedException
                    End Select
                    _canceller.Token.ThrowIfCancellationRequested()
                    If stockPayload IsNot Nothing AndAlso stockPayload.Count > 0 Then
                        Dim XMinutePayload As Dictionary(Of Date, Payload) = Nothing
                        If _timeFrame > 1 Then
                            XMinutePayload = _common.ConvertPayloadsToXMinutes(stockPayload, _timeFrame)
                        Else
                            XMinutePayload = stockPayload
                        End If
                        _canceller.Token.ThrowIfCancellationRequested()
                        Dim inputPayload As Dictionary(Of Date, Payload) = Nothing
                        If _useHA Then
                            Indicator.HeikenAshi.ConvertToHeikenAshi(XMinutePayload, inputPayload)
                        Else
                            inputPayload = XMinutePayload
                        End If
                        _canceller.Token.ThrowIfCancellationRequested()
                        Dim currentDayPayload As Dictionary(Of Date, Payload) = Nothing
                        For Each runningPayload In inputPayload.Keys
                            _canceller.Token.ThrowIfCancellationRequested()
                            If runningPayload.Date = chkDate.Date Then
                                If currentDayPayload Is Nothing Then currentDayPayload = New Dictionary(Of Date, Payload)
                                currentDayPayload.Add(runningPayload, inputPayload(runningPayload))
                            End If
                        Next
                        'Main Logic
                        Dim VWAPPayload As Dictionary(Of Date, Decimal) = Nothing
                        Indicator.VWAP.CalculateVWAP(inputPayload, VWAPPayload)
                        If currentDayPayload IsNot Nothing AndAlso currentDayPayload.Count > 0 Then
                            Dim previousDay As Date = Nothing
                            Dim eodPayload As Dictionary(Of Date, Payload) = Nothing
                            Select Case _category
                                Case "Cash"
                                    previousDay = _common.GetPreviousTradingDay(Common.DataBaseTable.Intraday_Cash, currentDayPayload.LastOrDefault.Key.Date)
                                    eodPayload = _common.GetRawPayload(Common.DataBaseTable.EOD_Cash, stock, previousDay, previousDay)
                                Case "Currency"
                                    previousDay = _common.GetPreviousTradingDay(Common.DataBaseTable.Intraday_Currency, currentDayPayload.LastOrDefault.Key.Date)
                                    eodPayload = _common.GetRawPayload(Common.DataBaseTable.EOD_Currency, stock, previousDay, previousDay)
                                Case "Commodity"
                                    previousDay = _common.GetPreviousTradingDay(Common.DataBaseTable.Intraday_Commodity, currentDayPayload.LastOrDefault.Key.Date)
                                    eodPayload = _common.GetRawPayload(Common.DataBaseTable.EOD_Commodity, stock, previousDay, previousDay)
                                Case "Future"
                                    previousDay = _common.GetPreviousTradingDay(Common.DataBaseTable.Intraday_Futures, currentDayPayload.LastOrDefault.Key.Date)
                                    eodPayload = _common.GetRawPayload(Common.DataBaseTable.EOD_Futures, stock, previousDay, previousDay)
                                Case Else
                                    Throw New NotImplementedException
                            End Select
                            For Each runningPayload In currentDayPayload.Keys
                                _canceller.Token.ThrowIfCancellationRequested()
                                Dim wickEndPoint As Decimal = Nothing
                                If currentDayPayload(runningPayload).High > currentDayPayload(runningPayload).PreviousCandlePayload.High AndAlso
                                    currentDayPayload(runningPayload).Low > currentDayPayload(runningPayload).PreviousCandlePayload.Low Then
                                    If currentDayPayload(runningPayload).Open < currentDayPayload(runningPayload).PreviousCandlePayload.High AndAlso
                                        currentDayPayload(runningPayload).Close < currentDayPayload(runningPayload).PreviousCandlePayload.High Then
                                        If (currentDayPayload(runningPayload).High - currentDayPayload(runningPayload).PreviousCandlePayload.High) >= currentDayPayload(runningPayload).CandleRange * 50 / 100 Then
                                            Dim currentDayHigh As Decimal = currentDayPayload.Max(Function(x)
                                                                                                      If x.Value.PayloadDate < runningPayload Then
                                                                                                          Return x.Value.High
                                                                                                      Else
                                                                                                          Return Decimal.MinValue
                                                                                                      End If
                                                                                                  End Function)
                                            If currentDayPayload(runningPayload).CandleColor = Color.Green Then
                                                wickEndPoint = currentDayPayload(runningPayload).Close
                                            Else
                                                wickEndPoint = currentDayPayload(runningPayload).Open
                                            End If
                                            Dim row As DataRow = ret.NewRow
                                            row("Date") = currentDayPayload(runningPayload).PayloadDate
                                            row("Instrument") = currentDayPayload(runningPayload).TradingSymbol
                                            row("Signal") = -1
                                            row("VWAP") = currentDayPayload(runningPayload).High > VWAPPayload(currentDayPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso wickEndPoint < VWAPPayload(currentDayPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                            row("CurrentDayHighLow") = currentDayPayload(runningPayload).High > currentDayHigh AndAlso wickEndPoint < currentDayHigh
                                            row("PreviousDayHighLow") = currentDayPayload(runningPayload).High > eodPayload.LastOrDefault.Value.High AndAlso wickEndPoint < eodPayload.LastOrDefault.Value.High
                                            ret.Rows.Add(row)
                                        End If
                                    End If
                                ElseIf currentDayPayload(runningPayload).Low < currentDayPayload(runningPayload).PreviousCandlePayload.Low AndAlso
                                    currentDayPayload(runningPayload).High < currentDayPayload(runningPayload).PreviousCandlePayload.High Then
                                    If currentDayPayload(runningPayload).Open > currentDayPayload(runningPayload).PreviousCandlePayload.Low AndAlso
                                        currentDayPayload(runningPayload).Close > currentDayPayload(runningPayload).PreviousCandlePayload.Low Then
                                        If (currentDayPayload(runningPayload).PreviousCandlePayload.Low - currentDayPayload(runningPayload).Low) >= currentDayPayload(runningPayload).CandleRange * 50 / 100 Then
                                            Dim currentDayLow As Decimal = currentDayPayload.Min(Function(x)
                                                                                                     If x.Value.PayloadDate < runningPayload Then
                                                                                                         Return x.Value.Low
                                                                                                     Else
                                                                                                         Return Decimal.MaxValue
                                                                                                     End If
                                                                                                 End Function)
                                            If currentDayPayload(runningPayload).CandleColor = Color.Green Then
                                                wickEndPoint = currentDayPayload(runningPayload).Open
                                            Else
                                                wickEndPoint = currentDayPayload(runningPayload).Close
                                            End If
                                            Dim row As DataRow = ret.NewRow
                                            row("Date") = currentDayPayload(runningPayload).PayloadDate
                                            row("Instrument") = currentDayPayload(runningPayload).TradingSymbol
                                            row("Signal") = 1
                                            row("VWAP") = currentDayPayload(runningPayload).Low < VWAPPayload(currentDayPayload(runningPayload).PreviousCandlePayload.PayloadDate) AndAlso wickEndPoint > VWAPPayload(currentDayPayload(runningPayload).PreviousCandlePayload.PayloadDate)
                                            row("CurrentDayHighLow") = currentDayPayload(runningPayload).Low < currentDayLow AndAlso wickEndPoint > currentDayLow
                                            row("PreviousDayHighLow") = currentDayPayload(runningPayload).Low < eodPayload.LastOrDefault.Value.Low AndAlso wickEndPoint > eodPayload.LastOrDefault.Value.Low
                                            ret.Rows.Add(row)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next
            End If
            chkDate = chkDate.AddDays(1)
        End While
        Return ret
    End Function
End Class
