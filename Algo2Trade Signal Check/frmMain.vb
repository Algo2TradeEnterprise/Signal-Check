﻿Imports Utilities
Imports System.Threading
Public Class frmMain

#Region "Common Delegates"
    Delegate Sub SetObjectEnableDisable_Delegate(ByVal [obj] As Object, ByVal [value] As Boolean)
    Public Sub SetObjectEnableDisable_ThreadSafe(ByVal [obj] As Object, ByVal [value] As Boolean)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [obj].InvokeRequired Then
            Dim MyDelegate As New SetObjectEnableDisable_Delegate(AddressOf SetObjectEnableDisable_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[obj], [value]})
        Else
            [obj].Enabled = [value]
        End If
    End Sub

    Delegate Sub SetLabelText_Delegate(ByVal [label] As Label, ByVal [text] As String)
    Public Sub SetLabelText_ThreadSafe(ByVal [label] As Label, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [label].InvokeRequired Then
            Dim MyDelegate As New SetLabelText_Delegate(AddressOf SetLabelText_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[label], [text]})
        Else
            [label].Text = [text]
        End If
    End Sub

    Delegate Function GetLabelText_Delegate(ByVal [label] As Label) As String
    Public Function GetLabelText_ThreadSafe(ByVal [label] As Label) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [label].InvokeRequired Then
            Dim MyDelegate As New GetLabelText_Delegate(AddressOf GetLabelText_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[label]})
        Else
            Return [label].Text
        End If
    End Function

    Delegate Sub SetLabelTag_Delegate(ByVal [label] As Label, ByVal [tag] As String)
    Public Sub SetLabelTag_ThreadSafe(ByVal [label] As Label, ByVal [tag] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [label].InvokeRequired Then
            Dim MyDelegate As New SetLabelTag_Delegate(AddressOf SetLabelTag_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[label], [tag]})
        Else
            [label].Tag = [tag]
        End If
    End Sub

    Delegate Function GetLabelTag_Delegate(ByVal [label] As Label) As String
    Public Function GetLabelTag_ThreadSafe(ByVal [label] As Label) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [label].InvokeRequired Then
            Dim MyDelegate As New GetLabelTag_Delegate(AddressOf GetLabelTag_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[label]})
        Else
            Return [label].Tag
        End If
    End Function
    Delegate Sub SetToolStripLabel_Delegate(ByVal [toolStrip] As StatusStrip, ByVal [label] As ToolStripStatusLabel, ByVal [text] As String)
    Public Sub SetToolStripLabel_ThreadSafe(ByVal [toolStrip] As StatusStrip, ByVal [label] As ToolStripStatusLabel, ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [toolStrip].InvokeRequired Then
            Dim MyDelegate As New SetToolStripLabel_Delegate(AddressOf SetToolStripLabel_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[toolStrip], [label], [text]})
        Else
            [label].Text = [text]
        End If
    End Sub

    Delegate Function GetToolStripLabel_Delegate(ByVal [toolStrip] As StatusStrip, ByVal [label] As ToolStripLabel) As String
    Public Function GetToolStripLabel_ThreadSafe(ByVal [toolStrip] As StatusStrip, ByVal [label] As ToolStripLabel) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [toolStrip].InvokeRequired Then
            Dim MyDelegate As New GetToolStripLabel_Delegate(AddressOf GetToolStripLabel_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[toolStrip], [label]})
        Else
            Return [label].Text
        End If
    End Function

    Delegate Function GetDateTimePickerValue_Delegate(ByVal [dateTimePicker] As DateTimePicker) As Date
    Public Function GetDateTimePickerValue_ThreadSafe(ByVal [dateTimePicker] As DateTimePicker) As Date
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [dateTimePicker].InvokeRequired Then
            Dim MyDelegate As New GetDateTimePickerValue_Delegate(AddressOf GetDateTimePickerValue_ThreadSafe)
            Return Me.Invoke(MyDelegate, New DateTimePicker() {[dateTimePicker]})
        Else
            Return [dateTimePicker].Value
        End If
    End Function

    Delegate Function GetNumericUpDownValue_Delegate(ByVal [numericUpDown] As NumericUpDown) As Integer
    Public Function GetNumericUpDownValue_ThreadSafe(ByVal [numericUpDown] As NumericUpDown) As Integer
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [numericUpDown].InvokeRequired Then
            Dim MyDelegate As New GetNumericUpDownValue_Delegate(AddressOf GetNumericUpDownValue_ThreadSafe)
            Return Me.Invoke(MyDelegate, New NumericUpDown() {[numericUpDown]})
        Else
            Return [numericUpDown].Value
        End If
    End Function

    Delegate Function GetComboBoxIndex_Delegate(ByVal [combobox] As ComboBox) As Integer
    Public Function GetComboBoxIndex_ThreadSafe(ByVal [combobox] As ComboBox) As Integer
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [combobox].InvokeRequired Then
            Dim MyDelegate As New GetComboBoxIndex_Delegate(AddressOf GetComboBoxIndex_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[combobox]})
        Else
            Return [combobox].SelectedIndex
        End If
    End Function

    Delegate Function GetComboBoxItem_Delegate(ByVal [ComboBox] As ComboBox) As String
    Public Function GetComboBoxItem_ThreadSafe(ByVal [ComboBox] As ComboBox) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [ComboBox].InvokeRequired Then
            Dim MyDelegate As New GetComboBoxItem_Delegate(AddressOf GetComboBoxItem_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[ComboBox]})
        Else
            Return [ComboBox].SelectedItem.ToString
        End If
    End Function

    Delegate Function GetTextBoxText_Delegate(ByVal [textBox] As TextBox) As String
    Public Function GetTextBoxText_ThreadSafe(ByVal [textBox] As TextBox) As String
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [textBox].InvokeRequired Then
            Dim MyDelegate As New GetTextBoxText_Delegate(AddressOf GetTextBoxText_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[textBox]})
        Else
            Return [textBox].Text
        End If
    End Function

    Delegate Function GetCheckBoxChecked_Delegate(ByVal [checkBox] As CheckBox) As Boolean
    Public Function GetCheckBoxChecked_ThreadSafe(ByVal [checkBox] As CheckBox) As Boolean
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [checkBox].InvokeRequired Then
            Dim MyDelegate As New GetCheckBoxChecked_Delegate(AddressOf GetCheckBoxChecked_ThreadSafe)
            Return Me.Invoke(MyDelegate, New Object() {[checkBox]})
        Else
            Return [checkBox].Checked
        End If
    End Function

    Delegate Sub SetDatagridBindDatatable_Delegate(ByVal [datagrid] As DataGridView, ByVal [table] As DataTable)
    Public Sub SetDatagridBindDatatable_ThreadSafe(ByVal [datagrid] As DataGridView, ByVal [table] As DataTable)
        ' InvokeRequired required compares the thread ID of the calling thread to the thread ID of the creating thread.  
        ' If these threads are different, it returns true.  
        If [datagrid].InvokeRequired Then
            Dim MyDelegate As New SetDatagridBindDatatable_Delegate(AddressOf SetDatagridBindDatatable_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[datagrid], [table]})
        Else
            [datagrid].DataSource = [table]
        End If
    End Sub
#End Region

#Region "Event Handlers"
    Private _canceller As CancellationTokenSource
    Private Sub OnHeartbeat(message As String)
        SetLabelText_ThreadSafe(lblProgress, message)
    End Sub
    Private Sub OnDocumentDownloadComplete()
        'OnHeartbeat("Document download compelete")
    End Sub
    Private Sub OnDocumentRetryStatus(currentTry As Integer, totalTries As Integer)
        OnHeartbeat(String.Format("Try #{0}/{1}: Connecting...", currentTry, totalTries))
    End Sub
    Public Sub OnWaitingFor(ByVal elapsedSecs As Integer, ByVal totalSecs As Integer, ByVal msg As String)
        OnHeartbeat(String.Format("{0}, waiting {1}/{2} secs", msg, elapsedSecs, totalSecs))
    End Sub
#End Region

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dgvSignal IsNot Nothing AndAlso dgvSignal.Rows.Count > 0 Then
            saveFile.AddExtension = True
            saveFile.FileName = String.Format("{0}.csv", GetComboBoxItem_ThreadSafe(cmbRule))
            saveFile.Filter = "CSV (*.csv)|*.csv"
            saveFile.ShowDialog()
        Else
            MessageBox.Show("Empty DataGrid. Nothing to export.", "Signal Check CSV File", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub saveFile_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles saveFile.FileOk
        Using export As New DAL.CSVHelper(saveFile.FileName, ",", _canceller)
            export.GetCSVFromDataGrid(dgvSignal)
        End Using
        If MessageBox.Show("Do you want to open file?", "Signal Check CSV File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Process.Start(saveFile.FileName)
        End If
    End Sub
    Private Sub InitUI(ByVal value As Boolean)
        SetObjectEnableDisable_ThreadSafe(btnView, value)
        SetObjectEnableDisable_ThreadSafe(btnCancel, Not value)
    End Sub
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        _canceller.Cancel()
        InitUI(True)
    End Sub
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitUI(True)
        cmbCategory.SelectedIndex = My.Settings.Category
        cmbRule.SelectedIndex = My.Settings.Rule
        nmrcTimeFrame.Value = My.Settings.TimeFrame
        chkbHA.Checked = My.Settings.UseHA
        txtName.Text = My.Settings.Intrument
        Try
            dtpckrFromDate.Value = My.Settings.FromDate
            dtpckrToDate.Value = My.Settings.ToDate
        Catch ex As Exception
            dtpckrFromDate.Value = Now
            dtpckrToDate.Value = Now
        End Try
    End Sub
    Private Async Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        My.Settings.Category = cmbCategory.SelectedIndex
        My.Settings.Rule = cmbRule.SelectedIndex
        My.Settings.FromDate = dtpckrFromDate.Value
        My.Settings.ToDate = dtpckrToDate.Value
        My.Settings.TimeFrame = nmrcTimeFrame.Value
        My.Settings.UseHA = chkbHA.Checked
        My.Settings.Intrument = txtName.Text
        My.Settings.Save()
        InitUI(False)
        Try
            _canceller = New CancellationTokenSource
            Await Task.Run(AddressOf ViewDataAsync).ConfigureAwait(False)
            SetObjectEnableDisable_ThreadSafe(btnExport, True)
        Catch ox As OperationCanceledException
            MsgBox(String.Format("Error: {0}", ox.Message), MsgBoxStyle.Critical)
        Catch ex As Exception
            MsgBox(String.Format("Error: {0}", ex.ToString), MsgBoxStyle.Critical)
        Finally
            SetLabelText_ThreadSafe(lblProgress, "Process Complete")
            InitUI(True)
        End Try
    End Sub
    Private Async Function ViewDataAsync() As Task
        Dim startDate As Date = GetDateTimePickerValue_ThreadSafe(dtpckrFromDate)
        Dim endDate As Date = GetDateTimePickerValue_ThreadSafe(dtpckrToDate)
        Dim selectedRule As Integer = GetComboBoxIndex_ThreadSafe(cmbRule)
        Dim name As String = GetTextBoxText_ThreadSafe(txtName)
        Dim category As String = GetComboBoxItem_ThreadSafe(cmbCategory)
        Dim timeFrame As Integer = GetNumericUpDownValue_ThreadSafe(nmrcTimeFrame)
        Dim useHA As Boolean = GetCheckBoxChecked_ThreadSafe(chkbHA)
        Dim dt As DataTable = Nothing
        Dim rule As Rule = Nothing

        Select Case selectedRule
            Case 0
                rule = New StallPattern(_canceller, category, name, timeFrame, useHA)
            Case 1
                rule = New PiercingAndDarkCloud(_canceller, category, name, timeFrame, useHA)
            Case 2
                rule = New OneSidedVolume(_canceller, category, name, timeFrame, useHA)
            Case 3
                rule = New ConstrictionAtBreakout(_canceller, category, name, timeFrame, useHA)
            Case 4
                rule = New HKTrendOpposingByVolume(_canceller, category, name, timeFrame, useHA)
            Case 5
                rule = New HKTemporaryPause(_canceller, category, name, timeFrame, useHA)
            Case 6
                rule = New HKReversal(_canceller, category, name, timeFrame, useHA)
            Case 7
                rule = New GetRawCandle(_canceller, category, name, timeFrame, useHA)
            Case 8
                rule = New DailyStrongHKOppositeVolume(_canceller, category, name, timeFrame, useHA)
            Case 9
                rule = New FractalCut2MA(_canceller, category, name, timeFrame, useHA)
            Case 10
                rule = New VolumeIndex(_canceller, category, name, timeFrame, useHA)
            Case 11
                rule = New EODSignal(_canceller, category, name, timeFrame, useHA)
            Case 12
                rule = New PinBarFormation(_canceller, category, name, timeFrame, useHA)
            Case 13
                rule = New BollingerWithATRBands(_canceller, category, name, timeFrame, useHA)
            Case 14
                rule = New LowLossHighGainVWAP(_canceller, category, name, timeFrame, useHA)
            Case 15
                rule = New DoubleVolumeEOD(_canceller, category, name, timeFrame, useHA)
            Case 16
                rule = New FractalBreakoutShortTrend(_canceller, category, name, timeFrame, useHA)
            Case 17
                rule = New DonchianBreakoutShortTrend(_canceller, category, name, timeFrame, useHA)
            Case 18
                rule = New PinocchioBarFormation(_canceller, category, name, timeFrame, useHA)
            Case 19
                rule = New MarketOpenHABreakoutScreener(_canceller, category, name, timeFrame, useHA)
            Case 20
                rule = New VolumeWithCandleRange(_canceller, category, name, timeFrame, useHA)
            Case 21
                rule = New DayHighLow(_canceller, category, name, timeFrame, useHA)
            Case 22
                rule = New LowSLCandle(_canceller, category, name, timeFrame, useHA)
            Case 23
                rule = New InsideBarHighLow(_canceller, category, name, timeFrame, useHA)
            Case 24
                rule = New ReversaHHLLBreakout(_canceller, category, name, timeFrame, useHA)
        End Select
        AddHandler Rule.Heartbeat, AddressOf OnHeartbeat
        AddHandler Rule.WaitingFor, AddressOf OnWaitingFor
        AddHandler Rule.DocumentRetryStatus, AddressOf OnDocumentRetryStatus
        AddHandler Rule.DocumentDownloadComplete, AddressOf OnDocumentDownloadComplete
        dt = Await Rule.RunAsync(startDate, endDate).ConfigureAwait(False)
        SetDatagridBindDatatable_ThreadSafe(dgvSignal, dt)
    End Function
End Class
