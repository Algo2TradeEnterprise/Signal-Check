Imports Algo2TradeBLL
Imports System.Threading
Imports MySql.Data.MySqlClient
Public Class StockSelection

#Region "Events/Event handlers"
    Public Event DocumentDownloadComplete()
    Public Event DocumentRetryStatus(ByVal currentTry As Integer, ByVal totalTries As Integer)
    Public Event Heartbeat(ByVal msg As String)
    Public Event WaitingFor(ByVal elapsedSecs As Integer, ByVal totalSecs As Integer, ByVal msg As String)
    'The below functions are needed to allow the derived classes to raise the above two events
    Protected Overridable Sub OnDocumentDownloadComplete()
        RaiseEvent DocumentDownloadComplete()
    End Sub
    Protected Overridable Sub OnDocumentRetryStatus(ByVal currentTry As Integer, ByVal totalTries As Integer)
        RaiseEvent DocumentRetryStatus(currentTry, totalTries)
    End Sub
    Protected Overridable Sub OnHeartbeat(ByVal msg As String)
        RaiseEvent Heartbeat(msg)
    End Sub
    Protected Overridable Sub OnWaitingFor(ByVal elapsedSecs As Integer, ByVal totalSecs As Integer, ByVal msg As String)
        RaiseEvent WaitingFor(elapsedSecs, totalSecs, msg)
    End Sub
#End Region

    Private _canceller As CancellationTokenSource
    Private _category As String
    Private _name As String
    Private _common As Common = New Common(_canceller)
    Public Sub New(ByVal canceller As CancellationTokenSource, ByVal stockCategory As String, ByVal stockName As String)
        _canceller = canceller
        _category = stockCategory
        _name = stockName
        AddHandler _common.Heartbeat, AddressOf OnHeartbeat
        AddHandler _common.WaitingFor, AddressOf OnWaitingFor
        AddHandler _common.DocumentRetryStatus, AddressOf OnDocumentRetryStatus
        AddHandler _common.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
    End Sub
    Public Async Function GetStockList(ByVal tradingDate As Date) As Task(Of List(Of String))
        Await Task.Delay(0).ConfigureAwait(False)
        Dim ret As List(Of String) = Nothing
        _canceller.Token.ThrowIfCancellationRequested()
        Dim conn As MySqlConnection = _common.OpenDBConnection
        _canceller.Token.ThrowIfCancellationRequested()
        Select Case _category
            Case "Cash"
                Dim dt As DataTable = Nothing

                If conn.State = ConnectionState.Open Then
                    OnHeartbeat("Fetching All Stock Data")
                    Dim cmd As New MySqlCommand("GET_STOCK_CASH_DATA_ATR_VOLUME_ALL_DATES", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@startDate", tradingDate)
                    cmd.Parameters.AddWithValue("@endDate", tradingDate)
                    cmd.Parameters.AddWithValue("@numberOfRecords", 0)
                    cmd.Parameters.AddWithValue("@minClose", 100)
                    cmd.Parameters.AddWithValue("@maxClose", 1500)
                    cmd.Parameters.AddWithValue("@atrPercentage", 2.5)
                    cmd.Parameters.AddWithValue("@potentialAmount", 1000000)
                    'OnHeartbeat("Fetching Top Gainer Looser Data")
                    'Dim cmd As New MySqlCommand("GET_TOP_GAINER_LOOSER_DATA_ATR_VOLUME_ALL_DATES", conn)
                    'cmd.CommandType = CommandType.StoredProcedure
                    'cmd.Parameters.AddWithValue("@startDate", tradingDate)
                    'cmd.Parameters.AddWithValue("@endDate", tradingDate)
                    'cmd.Parameters.AddWithValue("@userTime", "09:19:00")
                    'cmd.Parameters.AddWithValue("@numberOfRecords", 5)
                    'cmd.Parameters.AddWithValue("@minClose", 100)
                    'cmd.Parameters.AddWithValue("@maxClose", 1500)
                    'cmd.Parameters.AddWithValue("@atrPercentage", 2.5)
                    'cmd.Parameters.AddWithValue("@potentialAmount", 1000000)

                    Dim adapter As New MySqlDataAdapter(cmd)
                    adapter.SelectCommand.CommandTimeout = 3000
                    dt = New DataTable
                    adapter.Fill(dt)
                End If
                _canceller.Token.ThrowIfCancellationRequested()
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    ret = New List(Of String)
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ret.Add(dt.Rows(i).Item(1))
                    Next
                End If
            Case "Currency"
                Throw New NotImplementedException
            Case "Commodity"
                Throw New NotImplementedException
            Case "Future"
                Dim dt As DataTable = Nothing

                If conn.State = ConnectionState.Open Then
                    OnHeartbeat("Fetching All Stock Data")
                    Dim cmd As New MySqlCommand("GET_STOCK_CASH_DATA_ATR_VOLUME_ALL_DATES", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@startDate", tradingDate)
                    cmd.Parameters.AddWithValue("@endDate", tradingDate)
                    cmd.Parameters.AddWithValue("@numberOfRecords", 0)
                    cmd.Parameters.AddWithValue("@minClose", 100)
                    cmd.Parameters.AddWithValue("@maxClose", 1500)
                    cmd.Parameters.AddWithValue("@atrPercentage", 2.5)
                    cmd.Parameters.AddWithValue("@potentialAmount", 1000000)

                    Dim adapter As New MySqlDataAdapter(cmd)
                    adapter.SelectCommand.CommandTimeout = 3000
                    dt = New DataTable
                    adapter.Fill(dt)
                End If
                _canceller.Token.ThrowIfCancellationRequested()
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    ret = New List(Of String)
                    For i As Integer = 0 To dt.Rows.Count - 1
                        ret.Add(dt.Rows(i).Item(1))
                    Next
                End If
            Case Else
                Throw New NotImplementedException
        End Select
        Return ret
    End Function
End Class
