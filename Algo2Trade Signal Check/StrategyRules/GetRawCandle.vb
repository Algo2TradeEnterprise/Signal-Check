Imports Algo2TradeBLL
Imports System.Threading
Public Class GetRawCandle
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
        ret.Columns.Add("PSAR")
        ret.Columns.Add("Trend")

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

                        Dim psarPayload As Dictionary(Of Date, Decimal) = Nothing
                        Dim trendPayload As Dictionary(Of Date, Color) = Nothing

                        Indicator.ParabolicSAR.CalculatePSAR(0.02, 0.2, inputPayload, psarPayload, trendPayload)
                        If currentDayPayload IsNot Nothing AndAlso currentDayPayload.Count > 0 Then
                            For Each runningPayload In currentDayPayload.Keys
                                _canceller.Token.ThrowIfCancellationRequested()
                                Dim row As DataRow = ret.NewRow
                                row("Date") = inputPayload(runningPayload).PayloadDate
                                row("Trading Symbol") = inputPayload(runningPayload).TradingSymbol
                                row("Open") = inputPayload(runningPayload).Open
                                row("Low") = inputPayload(runningPayload).Low
                                row("High") = inputPayload(runningPayload).High
                                row("Close") = inputPayload(runningPayload).Close
                                row("Volume") = inputPayload(runningPayload).Volume
                                row("PSAR") = psarPayload(runningPayload)
                                row("Trend") = trendPayload(runningPayload).ToString

                                ret.Rows.Add(row)
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
