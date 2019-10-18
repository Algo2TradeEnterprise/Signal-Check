Imports Algo2TradeBLL
Imports System.Threading
Public Class DayHighLow
    Inherits Rule
    Public Sub New(ByVal canceller As CancellationTokenSource, ByVal stockCategory As String, ByVal stockName As String, ByVal timeFrame As Integer, ByVal useHA As Boolean)
        MyBase.New(canceller, stockCategory, stockName, timeFrame, useHA)
    End Sub
    Public Overrides Async Function RunAsync(ByVal startDate As Date, ByVal endDate As Date) As Task(Of DataTable)
        Await Task.Delay(0).ConfigureAwait(False)
        Dim ret As New DataTable
        ret.Columns.Add("Date")
        ret.Columns.Add("Trading Symbol")
        ret.Columns.Add("Open")
        ret.Columns.Add("Low")
        ret.Columns.Add("High")
        ret.Columns.Add("Close")
        ret.Columns.Add("Volume")
        ret.Columns.Add("Candle Change")
        ret.Columns.Add("Volume Change")
        ret.Columns.Add("Direction")

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
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.Intraday_Cash, stock, chkDate.AddDays(-8), chkDate)
                        Case "Currency"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.Intraday_Currency, stock, chkDate.AddDays(-8), chkDate)
                        Case "Commodity"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.Intraday_Commodity, stock, chkDate.AddDays(-8), chkDate)
                        Case "Future"
                            stockPayload = _common.GetRawPayload(Common.DataBaseTable.Intraday_Futures, stock, chkDate.AddDays(-8), chkDate)
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
                            Dim firstCandle As Boolean = True
                            Dim highSignalCandle As Payload = Nothing
                            Dim lowSignalCandle As Payload = Nothing
                            For Each runningPayload In currentDayPayload.Keys
                                _canceller.Token.ThrowIfCancellationRequested()
                                If Not firstCandle Then
                                    If highSignalCandle Is Nothing Then
                                        Dim dayHigh As Decimal = currentDayPayload.Max(Function(x)
                                                                                           If x.Key < runningPayload Then
                                                                                               Return x.Value.High
                                                                                           Else
                                                                                               Return Decimal.MinValue
                                                                                           End If
                                                                                       End Function)

                                        If currentDayPayload(runningPayload).PreviousCandlePayload.High = dayHigh Then
                                            highSignalCandle = currentDayPayload(runningPayload)
                                        End If
                                    Else
                                        If highSignalCandle.CandleRange >= ATRPayload(highSignalCandle.PayloadDate) * 2 Then
                                            If currentDayPayload(runningPayload).PreviousCandlePayload.High > highSignalCandle.High Then
                                                highSignalCandle = currentDayPayload(runningPayload)
                                            End If
                                        End If
                                    End If

                                    If lowSignalCandle Is Nothing Then
                                        Dim dayLow As Decimal = currentDayPayload.Min(Function(x)
                                                                                          If x.Key < runningPayload Then
                                                                                              Return x.Value.Low
                                                                                          Else
                                                                                              Return Decimal.MaxValue
                                                                                          End If
                                                                                      End Function)

                                        If currentDayPayload(runningPayload).PreviousCandlePayload.Low = dayLow Then
                                            lowSignalCandle = currentDayPayload(runningPayload)
                                        End If
                                    Else
                                        If lowSignalCandle.CandleRange >= ATRPayload(lowSignalCandle.PayloadDate) * 2 Then
                                            If currentDayPayload(runningPayload).PreviousCandlePayload.Low < lowSignalCandle.Low Then
                                                lowSignalCandle = currentDayPayload(runningPayload)
                                            End If
                                        End If
                                    End If

                                    If highSignalCandle IsNot Nothing AndAlso
                                        highSignalCandle.CandleRange < ATRPayload(highSignalCandle.PayloadDate) * 2 Then
                                        If currentDayPayload(runningPayload).CandleRange >= highSignalCandle.CandleRange AndAlso
                                            currentDayPayload(runningPayload).Volume < highSignalCandle.Volume Then
                                            Dim row As DataRow = ret.NewRow
                                            row("Date") = inputPayload(runningPayload).PayloadDate
                                            row("Trading Symbol") = inputPayload(runningPayload).TradingSymbol
                                            row("Open") = inputPayload(runningPayload).Open
                                            row("Low") = inputPayload(runningPayload).Low
                                            row("High") = inputPayload(runningPayload).High
                                            row("Close") = inputPayload(runningPayload).Close
                                            row("Volume") = inputPayload(runningPayload).Volume
                                            row("Candle Change") = (inputPayload(runningPayload).CandleRange / highSignalCandle.CandleRange) - 1
                                            row("Volume Change") = (inputPayload(runningPayload).Volume / highSignalCandle.Volume) - 1
                                            row("Direction") = -1

                                            ret.Rows.Add(row)
                                        Else
                                            If Not IsInsideBar(highSignalCandle, currentDayPayload(runningPayload)) Then
                                                highSignalCandle = Nothing
                                            End If
                                        End If
                                    End If

                                    If lowSignalCandle IsNot Nothing AndAlso
                                        lowSignalCandle.CandleRange < ATRPayload(lowSignalCandle.PayloadDate) * 2 Then
                                        If currentDayPayload(runningPayload).CandleRange >= lowSignalCandle.CandleRange AndAlso
                                            currentDayPayload(runningPayload).Volume < lowSignalCandle.Volume Then
                                            Dim row As DataRow = ret.NewRow
                                            row("Date") = inputPayload(runningPayload).PayloadDate
                                            row("Trading Symbol") = inputPayload(runningPayload).TradingSymbol
                                            row("Open") = inputPayload(runningPayload).Open
                                            row("Low") = inputPayload(runningPayload).Low
                                            row("High") = inputPayload(runningPayload).High
                                            row("Close") = inputPayload(runningPayload).Close
                                            row("Volume") = inputPayload(runningPayload).Volume
                                            row("Candle Change") = (inputPayload(runningPayload).CandleRange / lowSignalCandle.CandleRange) - 1
                                            row("Volume Change") = (inputPayload(runningPayload).Volume / lowSignalCandle.Volume) - 1
                                            row("Direction") = 1

                                            ret.Rows.Add(row)
                                        Else
                                            If Not IsInsideBar(lowSignalCandle, currentDayPayload(runningPayload)) Then
                                                lowSignalCandle = Nothing
                                            End If
                                        End If
                                    End If
                                End If

                                firstCandle = False
                            Next
                        End If
                    End If
                Next
            End If
            chkDate = chkDate.AddDays(1)
        End While
        Return ret
    End Function

    Private Function IsInsideBar(ByVal signalCandle As Payload, ByVal currentCandle As Payload) As Boolean
        Dim ret As Boolean = False
        If currentCandle.CandleColor = Color.Green Then
            If currentCandle.Open >= signalCandle.Low AndAlso currentCandle.Close <= signalCandle.High Then
                ret = True
            End If
        Else
            If currentCandle.Open <= signalCandle.High AndAlso currentCandle.Close >= signalCandle.Low Then
                ret = True
            End If
        End If
        Return ret
    End Function
End Class