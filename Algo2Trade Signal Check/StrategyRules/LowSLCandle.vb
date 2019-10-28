Imports Algo2TradeBLL
Imports System.Threading
Public Class LowSLCandle
    Inherits Rule

    Private ReadOnly _MaxSLAmount As Decimal = -1000
    Private ReadOnly _MinimumCapitalPerStock As Decimal = 15000
    Private ReadOnly _ATRMultiplier As Decimal = 1 / 3

    Public Sub New(ByVal canceller As CancellationTokenSource, ByVal stockCategory As String, ByVal stockName As String, ByVal timeFrame As Integer, ByVal useHA As Boolean)
        MyBase.New(canceller, stockCategory, stockName, timeFrame, useHA)
    End Sub
    Public Overrides Async Function RunAsync(startDate As Date, endDate As Date) As Task(Of DataTable)
        Await Task.Delay(0).ConfigureAwait(False)
        Dim ret As New DataTable
        ret.Columns.Add("Date")
        ret.Columns.Add("Instrument")
        ret.Columns.Add("Candle Range")
        ret.Columns.Add("ATR")
        ret.Columns.Add("Quantity")
        ret.Columns.Add("Stoploss")

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
                            Dim tradingSymbolToken As Tuple(Of String, String) = _common.GetCurrentTradingSymbolWithInstrumentToken(Common.DataBaseTable.Intraday_Cash, chkDate, stock)
                            If tradingSymbolToken IsNot Nothing Then
                                stockPayload = _common.GetRawPayloadForSpecificTradingSymbol(Common.DataBaseTable.Intraday_Cash, tradingSymbolToken.Item2, chkDate.AddDays(-7), chkDate)
                            End If
                        Case "Currency"
                            Dim tradingSymbolToken As Tuple(Of String, String) = _common.GetCurrentTradingSymbolWithInstrumentToken(Common.DataBaseTable.Intraday_Currency, chkDate, stock)
                            If tradingSymbolToken IsNot Nothing Then
                                stockPayload = _common.GetRawPayloadForSpecificTradingSymbol(Common.DataBaseTable.Intraday_Currency, tradingSymbolToken.Item2, chkDate.AddDays(-7), chkDate)
                            End If
                        Case "Commodity"
                            Dim tradingSymbolToken As Tuple(Of String, String) = _common.GetCurrentTradingSymbolWithInstrumentToken(Common.DataBaseTable.Intraday_Commodity, chkDate, stock)
                            If tradingSymbolToken IsNot Nothing Then
                                stockPayload = _common.GetRawPayloadForSpecificTradingSymbol(Common.DataBaseTable.Intraday_Commodity, tradingSymbolToken.Item2, chkDate.AddDays(-7), chkDate)
                            End If
                        Case "Future"
                            Dim tradingSymbolToken As Tuple(Of String, String) = _common.GetCurrentTradingSymbolWithInstrumentToken(Common.DataBaseTable.Intraday_Futures, chkDate, stock)
                            If tradingSymbolToken IsNot Nothing Then
                                stockPayload = _common.GetRawPayloadForSpecificTradingSymbol(Common.DataBaseTable.Intraday_Futures, tradingSymbolToken.Item2, chkDate.AddDays(-7), chkDate)
                            End If
                        Case Else
                            Throw New NotImplementedException
                    End Select
                    _canceller.Token.ThrowIfCancellationRequested()
                    If stockPayload IsNot Nothing AndAlso stockPayload.Count > 0 Then
                        OnHeartbeat("Processing Data")
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
                            Dim lotSize As Integer = _common.GetLotSize(Common.DataBaseTable.Intraday_Futures, currentDayPayload.FirstOrDefault.Value.TradingSymbol, currentDayPayload.FirstOrDefault.Key.Date)
                            Dim quantity As Integer = CalculateQuantityFromInvestment(lotSize, _MinimumCapitalPerStock, currentDayPayload.FirstOrDefault.Value.Open, True)
                            Dim buffer As Decimal = CalculateBuffer(currentDayPayload.FirstOrDefault.Value.Open, Utilities.Numbers.NumberManipulation.RoundOfType.Floor)
                            For Each runningPayload In currentDayPayload.Keys
                                _canceller.Token.ThrowIfCancellationRequested()
                                If currentDayPayload(runningPayload).Volume >= currentDayPayload(runningPayload).PreviousCandlePayload.Volume * 90 / 100 Then
                                    If currentDayPayload(runningPayload).CandleRange > ATRPayload(runningPayload) * _ATRMultiplier Then
                                        Dim pl As Decimal = CalculatePL(currentDayPayload(runningPayload).TradingSymbol, currentDayPayload(runningPayload).High + buffer, currentDayPayload(runningPayload).Low - buffer, quantity, lotSize)
                                        If pl >= _MaxSLAmount Then
                                            Dim row As DataRow = ret.NewRow
                                            row("Date") = runningPayload
                                            row("Instrument") = currentDayPayload(runningPayload).TradingSymbol
                                            row("Candle Range") = currentDayPayload(runningPayload).CandleRange
                                            row("ATR") = Math.Round(ATRPayload(runningPayload), 2)
                                            row("Quantity") = quantity
                                            row("Stoploss") = pl
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
