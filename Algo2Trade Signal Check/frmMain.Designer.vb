<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.dgvSignal = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkbHA = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.nmrcTimeFrame = New System.Windows.Forms.NumericUpDown()
        Me.lblTimeFrame = New System.Windows.Forms.Label()
        Me.dtpckrToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpckrFromDate = New System.Windows.Forms.DateTimePicker()
        Me.lblToDate = New System.Windows.Forms.Label()
        Me.lblFromDate = New System.Windows.Forms.Label()
        Me.btnView = New System.Windows.Forms.Button()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbCategory = New System.Windows.Forms.ComboBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.cmbRule = New System.Windows.Forms.ComboBox()
        Me.lblRule = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.saveFile = New System.Windows.Forms.SaveFileDialog()
        CType(Me.dgvSignal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.nmrcTimeFrame, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvSignal
        '
        Me.dgvSignal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSignal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSignal.Location = New System.Drawing.Point(3, 102)
        Me.dgvSignal.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvSignal.Name = "dgvSignal"
        Me.dgvSignal.RowTemplate.Height = 24
        Me.dgvSignal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSignal.Size = New System.Drawing.Size(1237, 551)
        Me.dgvSignal.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.chkbHA)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.nmrcTimeFrame)
        Me.Panel1.Controls.Add(Me.lblTimeFrame)
        Me.Panel1.Controls.Add(Me.dtpckrToDate)
        Me.Panel1.Controls.Add(Me.dtpckrFromDate)
        Me.Panel1.Controls.Add(Me.lblToDate)
        Me.Panel1.Controls.Add(Me.lblFromDate)
        Me.Panel1.Controls.Add(Me.btnView)
        Me.Panel1.Controls.Add(Me.txtName)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cmbCategory)
        Me.Panel1.Controls.Add(Me.lblCategory)
        Me.Panel1.Controls.Add(Me.cmbRule)
        Me.Panel1.Controls.Add(Me.lblRule)
        Me.Panel1.Location = New System.Drawing.Point(3, 4)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1237, 92)
        Me.Panel1.TabIndex = 1
        '
        'chkbHA
        '
        Me.chkbHA.AutoSize = True
        Me.chkbHA.Location = New System.Drawing.Point(621, 50)
        Me.chkbHA.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkbHA.Name = "chkbHA"
        Me.chkbHA.Size = New System.Drawing.Size(149, 21)
        Me.chkbHA.TabIndex = 30
        Me.chkbHA.Text = "HeikenAshi Candle"
        Me.chkbHA.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnCancel.Location = New System.Drawing.Point(1125, 48)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 28)
        Me.btnCancel.TabIndex = 29
        Me.btnCancel.Text = "Stop"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'nmrcTimeFrame
        '
        Me.nmrcTimeFrame.Location = New System.Drawing.Point(525, 47)
        Me.nmrcTimeFrame.Margin = New System.Windows.Forms.Padding(4)
        Me.nmrcTimeFrame.Name = "nmrcTimeFrame"
        Me.nmrcTimeFrame.Size = New System.Drawing.Size(75, 22)
        Me.nmrcTimeFrame.TabIndex = 28
        '
        'lblTimeFrame
        '
        Me.lblTimeFrame.AutoSize = True
        Me.lblTimeFrame.Location = New System.Drawing.Point(395, 48)
        Me.lblTimeFrame.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTimeFrame.Name = "lblTimeFrame"
        Me.lblTimeFrame.Size = New System.Drawing.Size(126, 17)
        Me.lblTimeFrame.TabIndex = 27
        Me.lblTimeFrame.Text = "Signal TimeFrame:"
        '
        'dtpckrToDate
        '
        Me.dtpckrToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpckrToDate.Location = New System.Drawing.Point(277, 46)
        Me.dtpckrToDate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dtpckrToDate.Name = "dtpckrToDate"
        Me.dtpckrToDate.Size = New System.Drawing.Size(108, 22)
        Me.dtpckrToDate.TabIndex = 26
        '
        'dtpckrFromDate
        '
        Me.dtpckrFromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpckrFromDate.Location = New System.Drawing.Point(93, 44)
        Me.dtpckrFromDate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dtpckrFromDate.Name = "dtpckrFromDate"
        Me.dtpckrFromDate.Size = New System.Drawing.Size(108, 22)
        Me.dtpckrFromDate.TabIndex = 25
        '
        'lblToDate
        '
        Me.lblToDate.AutoSize = True
        Me.lblToDate.Location = New System.Drawing.Point(211, 47)
        Me.lblToDate.Name = "lblToDate"
        Me.lblToDate.Size = New System.Drawing.Size(63, 17)
        Me.lblToDate.TabIndex = 24
        Me.lblToDate.Text = "To Date:"
        '
        'lblFromDate
        '
        Me.lblFromDate.AutoSize = True
        Me.lblFromDate.Location = New System.Drawing.Point(11, 46)
        Me.lblFromDate.Name = "lblFromDate"
        Me.lblFromDate.Size = New System.Drawing.Size(78, 17)
        Me.lblFromDate.TabIndex = 23
        Me.lblFromDate.Text = "From Date:"
        '
        'btnView
        '
        Me.btnView.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnView.Location = New System.Drawing.Point(1125, 7)
        Me.btnView.Margin = New System.Windows.Forms.Padding(4)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 28)
        Me.btnView.TabIndex = 22
        Me.btnView.Text = "View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'txtName
        '
        Me.txtName.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(907, 10)
        Me.txtName.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(205, 22)
        Me.txtName.TabIndex = 21
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(787, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(119, 17)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Instrument Name:"
        '
        'cmbCategory
        '
        Me.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategory.FormattingEnabled = True
        Me.cmbCategory.Items.AddRange(New Object() {"Cash", "Currency", "Commodity", "Future"})
        Me.cmbCategory.Location = New System.Drawing.Point(653, 9)
        Me.cmbCategory.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cmbCategory.Name = "cmbCategory"
        Me.cmbCategory.Size = New System.Drawing.Size(121, 24)
        Me.cmbCategory.TabIndex = 19
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Location = New System.Drawing.Point(512, 11)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(139, 17)
        Me.lblCategory.TabIndex = 18
        Me.lblCategory.Text = "Instrument Category:"
        '
        'cmbRule
        '
        Me.cmbRule.FormattingEnabled = True
        Me.cmbRule.Items.AddRange(New Object() {"Stall Pattern", "Piercing And Dark Cloud", "One Sided Volume", "Constriction At Breakout", "HK Trend Opposing By Volume", "HK Temporary Pause", "HK Reversal", "Get Raw Candle", "Daily Strong HK Opposite Color Volume", "Fractal Cut 2 MA", "Volume Index", "EOD Signal", "Pin Bar Formation", "Bollinger With ATR Bands", "Low Loss High Gain VWAP", "Double Volume EOD", "Fractal Breakout Short Trend", "Donchian Breakout Short Trend", "Pinocchio Bar Formation", "Market Open HA Breakout Screener"})
        Me.cmbRule.Location = New System.Drawing.Point(108, 7)
        Me.cmbRule.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbRule.Name = "cmbRule"
        Me.cmbRule.Size = New System.Drawing.Size(397, 24)
        Me.cmbRule.TabIndex = 17
        '
        'lblRule
        '
        Me.lblRule.AutoSize = True
        Me.lblRule.Location = New System.Drawing.Point(11, 12)
        Me.lblRule.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRule.Name = "lblRule"
        Me.lblRule.Size = New System.Drawing.Size(93, 17)
        Me.lblRule.TabIndex = 16
        Me.lblRule.Text = "Choose Rule:"
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Location = New System.Drawing.Point(1139, 658)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(100, 28)
        Me.btnExport.TabIndex = 20
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'lblProgress
        '
        Me.lblProgress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(3, 665)
        Me.lblProgress.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(128, 17)
        Me.lblProgress.TabIndex = 19
        Me.lblProgress.Text = "Progess Status ....."
        '
        'saveFile
        '
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1244, 690)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.dgvSignal)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Algo2Trade Signal Check"
        CType(Me.dgvSignal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nmrcTimeFrame, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvSignal As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents cmbRule As ComboBox
    Friend WithEvents lblRule As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbCategory As ComboBox
    Friend WithEvents lblCategory As Label
    Friend WithEvents btnView As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents lblProgress As Label
    Friend WithEvents saveFile As SaveFileDialog
    Friend WithEvents dtpckrToDate As DateTimePicker
    Friend WithEvents dtpckrFromDate As DateTimePicker
    Friend WithEvents lblToDate As Label
    Friend WithEvents lblFromDate As Label
    Friend WithEvents nmrcTimeFrame As NumericUpDown
    Friend WithEvents lblTimeFrame As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents chkbHA As CheckBox
End Class
