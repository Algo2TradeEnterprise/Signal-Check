﻿Imports Algo2TradeBLL
Imports System.Threading
Public Class EODSignal
    Inherits Rule
    Public Sub New(ByVal canceller As CancellationTokenSource, ByVal stockCategory As String, ByVal stockName As String, ByVal timeFrame As Integer, ByVal useHA As Boolean)
        MyBase.New(canceller, stockCategory, stockName, timeFrame, useHA)
    End Sub
    Public Overrides Async Function RunAsync(ByVal startDate As Date, ByVal endDate As Date) As Task(Of DataTable)
        Await Task.Delay(0).ConfigureAwait(False)
        Dim ret As New DataTable
        ret.Columns.Add("Date")
        ret.Columns.Add("Instrument")
        ret.Columns.Add("CurrentCandle Range")
        ret.Columns.Add("CurrentCandle ATR")
        ret.Columns.Add("Next Candle Range")
        ret.Columns.Add("Next Candle ATR")
        ret.Columns.Add("Next Candle Close")
        ret.Columns.Add("Next Candle Volume")
        ret.Columns.Add("Second Next Candle Range")
        ret.Columns.Add("Second Next Candle ATR")
        ret.Columns.Add("Second Next Candle Close")
        ret.Columns.Add("Second Next Candle Volume")
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
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.EOD_Cash, stock, chkDate.AddDays(-200), chkDate)
                        Case "Currency"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.EOD_Currency, stock, chkDate.AddDays(-200), chkDate)
                        Case "Commodity"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.EOD_Commodity, stock, chkDate.AddDays(-200), chkDate)
                        Case "Future"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.EOD_Futures, stock, chkDate.AddDays(-200), chkDate)
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
                        Dim ATRPayload As Dictionary(Of Date, Decimal) = Nothing
                        Indicator.ATR.CalculateATR(14, inputPayload, ATRPayload)
                        If currentDayPayload IsNot Nothing AndAlso currentDayPayload.Count > 0 Then
                            For Each runningPayload In currentDayPayload.Keys
                                _canceller.Token.ThrowIfCancellationRequested()
                                If currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.CandleColor <> currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.VolumeColor AndAlso
                                    ATRPayload(currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.PayloadDate) >= 2.5 Then
                                    Dim row As DataRow = ret.NewRow
                                    row("Date") = currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate.ToShortDateString
                                    row("Instrument") = currentDayPayload(runningPayload).TradingSymbol
                                    row("CurrentCandle Range") = currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.CandleRange
                                    row("CurrentCandle ATR") = Math.Round(ATRPayload(currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PreviousCandlePayload.PayloadDate), 2)
                                    row("Next Candle Range") = currentDayPayload(runningPayload).PreviousCandlePayload.CandleRange
                                    row("Next Candle ATR") = Math.Round(ATRPayload(currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.PayloadDate), 2)
                                    row("Next Candle Close") = currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Close
                                    row("Next Candle Volume") = currentDayPayload(runningPayload).PreviousCandlePayload.PreviousCandlePayload.Volume
                                    row("Second Next Candle Range") = currentDayPayload(runningPayload).CandleRange
                                    row("Second Next Candle ATR") = Math.Round(ATRPayload(currentDayPayload(runningPayload).PreviousCandlePayload.PayloadDate), 2)
                                    row("Second Next Candle Close") = currentDayPayload(runningPayload).PreviousCandlePayload.Close
                                    row("Second Next Candle Volume") = currentDayPayload(runningPayload).PreviousCandlePayload.Volume
                                    ret.Rows.Add(row)
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
