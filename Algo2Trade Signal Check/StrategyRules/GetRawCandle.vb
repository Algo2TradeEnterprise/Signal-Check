﻿Imports Algo2TradeBLL
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
                        'Dim diPlusPayload As Dictionary(Of Date, Decimal) = Nothing
                        'Dim diMinusPayload As Dictionary(Of Date, Decimal) = Nothing
                        'Dim ADXPayload As Dictionary(Of Date, Decimal) = Nothing

                        'Dim trPayload As Dictionary(Of Date, Decimal) = Nothing
                        'Dim dm1PlusPayload As Dictionary(Of Date, Decimal) = Nothing
                        'Dim dm1MinusPayload As Dictionary(Of Date, Decimal) = Nothing
                        'Dim dxPayload As Dictionary(Of Date, Decimal) = Nothing

                        'Dim tr14Payload As Dictionary(Of Date, Decimal) = Nothing
                        'Dim dm14PlusPayload As Dictionary(Of Date, Decimal) = Nothing
                        'Dim dm14MinusPayload As Dictionary(Of Date, Decimal) = Nothing

                        'Indicator.ADX.CalculateADX(14, 14, inputPayload, ADXPayload, diPlusPayload, diMinusPayload, trPayload, dm1PlusPayload, dm1MinusPayload, dxPayload, tr14Payload, dm14PlusPayload, dm14MinusPayload)
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

                                'row("TR1") = trPayload(runningPayload)
                                'row("DM1+") = dm1PlusPayload(runningPayload)
                                'row("DM1-") = dm1MinusPayload(runningPayload)
                                'row("TR14") = tr14Payload(runningPayload)
                                'row("DM14+") = dm14PlusPayload(runningPayload)
                                'row("DM14-") = dm14MinusPayload(runningPayload)
                                'row("DX") = dxPayload(runningPayload)
                                'row("DI+") = diPlusPayload(runningPayload)
                                'row("DI-") = diMinusPayload(runningPayload)
                                'row("ADX") = ADXPayload(runningPayload)
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
