<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSimpleKeithleyControl
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
        Me.sourceLevelValueChA = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.sourceFunctionLabel = New System.Windows.Forms.Label()
        Me.cboSourceFunctionChA = New System.Windows.Forms.ComboBox()
        Me.chkSourceAutoRangeChA = New System.Windows.Forms.CheckBox()
        Me.txtSourceLimitChA = New System.Windows.Forms.TextBox()
        Me.sourceLimitLabel = New System.Windows.Forms.Label()
        Me.txtSourcRangeChA = New System.Windows.Forms.TextBox()
        Me.sourceRangeLabel = New System.Windows.Forms.Label()
        Me.sourceLevelLabel = New System.Windows.Forms.Label()
        Me.initializeButton = New System.Windows.Forms.Button()
        Me.resourceDescTextBox = New System.Windows.Forms.TextBox()
        Me.resourceDescLabel = New System.Windows.Forms.Label()
        Me.connectionGroupBox = New System.Windows.Forms.GroupBox()
        Me.sourceConfigurationGroupBox = New System.Windows.Forms.GroupBox()
        Me.txtResultValueChA = New System.Windows.Forms.TextBox()
        Me.txtNPLCchA = New System.Windows.Forms.TextBox()
        Me.nplcLabel = New System.Windows.Forms.Label()
        Me.chkMeasureAutoRangeChA = New System.Windows.Forms.CheckBox()
        Me.txtMeasureRangeChA = New System.Windows.Forms.TextBox()
        Me.measureRangeLabel = New System.Windows.Forms.Label()
        Me.cboMeasureFunctionChA = New System.Windows.Forms.ComboBox()
        Me.measureFunctionLabel = New System.Windows.Forms.Label()
        Me.btnOutputStateChA = New System.Windows.Forms.Button()
        Me.statusBar = New System.Windows.Forms.StatusBar()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtResultValueChB = New System.Windows.Forms.TextBox()
        Me.txtNPLCchB = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkMeasureAutoRangeChB = New System.Windows.Forms.CheckBox()
        Me.txtMeasureRangeChB = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboMeasureFunctionChB = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnOutputStateChB = New System.Windows.Forms.Button()
        Me.sourceLevelValueChB = New NationalInstruments.UI.WindowsForms.NumericEdit()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboSourceFunctionChB = New System.Windows.Forms.ComboBox()
        Me.chkSourceAutoRangeChB = New System.Windows.Forms.CheckBox()
        Me.txtSourceLimitChB = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtSourcRangeChB = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.sourceLevelValueChA, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.connectionGroupBox.SuspendLayout()
        Me.sourceConfigurationGroupBox.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.sourceLevelValueChB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'sourceLevelValueChA
        '
        Me.sourceLevelValueChA.CausesValidation = False
        Me.sourceLevelValueChA.CoercionInterval = 0.001R
        Me.sourceLevelValueChA.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateGenericMode("000.0000")
        Me.sourceLevelValueChA.Location = New System.Drawing.Point(99, 82)
        Me.sourceLevelValueChA.Name = "sourceLevelValueChA"
        Me.sourceLevelValueChA.Size = New System.Drawing.Size(106, 22)
        Me.sourceLevelValueChA.TabIndex = 8
        Me.sourceLevelValueChA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'sourceFunctionLabel
        '
        Me.sourceFunctionLabel.AutoSize = True
        Me.sourceFunctionLabel.Location = New System.Drawing.Point(6, 23)
        Me.sourceFunctionLabel.Name = "sourceFunctionLabel"
        Me.sourceFunctionLabel.Size = New System.Drawing.Size(111, 17)
        Me.sourceFunctionLabel.TabIndex = 6
        Me.sourceFunctionLabel.Text = "Source function:"
        '
        'cboSourceFunctionChA
        '
        Me.cboSourceFunctionChA.Items.AddRange(New Object() {"Current", "Voltage"})
        Me.cboSourceFunctionChA.Location = New System.Drawing.Point(123, 20)
        Me.cboSourceFunctionChA.Name = "cboSourceFunctionChA"
        Me.cboSourceFunctionChA.Size = New System.Drawing.Size(107, 24)
        Me.cboSourceFunctionChA.TabIndex = 5
        '
        'chkSourceAutoRangeChA
        '
        Me.chkSourceAutoRangeChA.Checked = True
        Me.chkSourceAutoRangeChA.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSourceAutoRangeChA.Location = New System.Drawing.Point(133, 49)
        Me.chkSourceAutoRangeChA.Name = "chkSourceAutoRangeChA"
        Me.chkSourceAutoRangeChA.Size = New System.Drawing.Size(100, 28)
        Me.chkSourceAutoRangeChA.TabIndex = 3
        Me.chkSourceAutoRangeChA.Text = "Autorange"
        '
        'txtSourceLimitChA
        '
        Me.txtSourceLimitChA.Location = New System.Drawing.Point(49, 52)
        Me.txtSourceLimitChA.Name = "txtSourceLimitChA"
        Me.txtSourceLimitChA.Size = New System.Drawing.Size(52, 22)
        Me.txtSourceLimitChA.TabIndex = 1
        Me.txtSourceLimitChA.Text = "0.2"
        '
        'sourceLimitLabel
        '
        Me.sourceLimitLabel.AutoSize = True
        Me.sourceLimitLabel.Location = New System.Drawing.Point(6, 54)
        Me.sourceLimitLabel.Name = "sourceLimitLabel"
        Me.sourceLimitLabel.Size = New System.Drawing.Size(41, 17)
        Me.sourceLimitLabel.TabIndex = 2
        Me.sourceLimitLabel.Text = "Limit:"
        '
        'txtSourcRangeChA
        '
        Me.txtSourcRangeChA.Location = New System.Drawing.Point(289, 52)
        Me.txtSourcRangeChA.Name = "txtSourcRangeChA"
        Me.txtSourcRangeChA.Size = New System.Drawing.Size(61, 22)
        Me.txtSourcRangeChA.TabIndex = 1
        Me.txtSourcRangeChA.Text = "1E-8"
        '
        'sourceRangeLabel
        '
        Me.sourceRangeLabel.AutoSize = True
        Me.sourceRangeLabel.Location = New System.Drawing.Point(239, 54)
        Me.sourceRangeLabel.Name = "sourceRangeLabel"
        Me.sourceRangeLabel.Size = New System.Drawing.Size(54, 17)
        Me.sourceRangeLabel.TabIndex = 2
        Me.sourceRangeLabel.Text = "Range:"
        '
        'sourceLevelLabel
        '
        Me.sourceLevelLabel.AutoSize = True
        Me.sourceLevelLabel.Location = New System.Drawing.Point(6, 83)
        Me.sourceLevelLabel.Name = "sourceLevelLabel"
        Me.sourceLevelLabel.Size = New System.Drawing.Size(95, 17)
        Me.sourceLevelLabel.TabIndex = 2
        Me.sourceLevelLabel.Text = "Source Level:"
        '
        'initializeButton
        '
        Me.initializeButton.Location = New System.Drawing.Point(305, 15)
        Me.initializeButton.Name = "initializeButton"
        Me.initializeButton.Size = New System.Drawing.Size(69, 27)
        Me.initializeButton.TabIndex = 19
        Me.initializeButton.Text = "Initialize"
        '
        'resourceDescTextBox
        '
        Me.resourceDescTextBox.Location = New System.Drawing.Point(74, 18)
        Me.resourceDescTextBox.Name = "resourceDescTextBox"
        Me.resourceDescTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.resourceDescTextBox.Size = New System.Drawing.Size(225, 22)
        Me.resourceDescTextBox.TabIndex = 24
        Me.resourceDescTextBox.Text = "TCPIP::192.168.1.114::inst0::INSTR"
        '
        'resourceDescLabel
        '
        Me.resourceDescLabel.AutoSize = True
        Me.resourceDescLabel.Location = New System.Drawing.Point(6, 21)
        Me.resourceDescLabel.Name = "resourceDescLabel"
        Me.resourceDescLabel.Size = New System.Drawing.Size(73, 17)
        Me.resourceDescLabel.TabIndex = 25
        Me.resourceDescLabel.Text = "Resource:"
        Me.resourceDescLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'connectionGroupBox
        '
        Me.connectionGroupBox.Controls.Add(Me.initializeButton)
        Me.connectionGroupBox.Controls.Add(Me.resourceDescTextBox)
        Me.connectionGroupBox.Controls.Add(Me.resourceDescLabel)
        Me.connectionGroupBox.Location = New System.Drawing.Point(10, 4)
        Me.connectionGroupBox.Name = "connectionGroupBox"
        Me.connectionGroupBox.Size = New System.Drawing.Size(385, 50)
        Me.connectionGroupBox.TabIndex = 26
        Me.connectionGroupBox.TabStop = False
        Me.connectionGroupBox.Text = "Connection"
        '
        'sourceConfigurationGroupBox
        '
        Me.sourceConfigurationGroupBox.Controls.Add(Me.txtResultValueChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.txtNPLCchA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.nplcLabel)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.chkMeasureAutoRangeChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.txtMeasureRangeChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.measureRangeLabel)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.cboMeasureFunctionChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.measureFunctionLabel)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.btnOutputStateChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.sourceLevelValueChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.sourceFunctionLabel)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.cboSourceFunctionChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.chkSourceAutoRangeChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.txtSourceLimitChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.sourceLimitLabel)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.txtSourcRangeChA)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.sourceRangeLabel)
        Me.sourceConfigurationGroupBox.Controls.Add(Me.sourceLevelLabel)
        Me.sourceConfigurationGroupBox.Location = New System.Drawing.Point(10, 61)
        Me.sourceConfigurationGroupBox.Name = "sourceConfigurationGroupBox"
        Me.sourceConfigurationGroupBox.Size = New System.Drawing.Size(385, 217)
        Me.sourceConfigurationGroupBox.TabIndex = 28
        Me.sourceConfigurationGroupBox.TabStop = False
        Me.sourceConfigurationGroupBox.Text = "Channel A"
        '
        'txtResultValueChA
        '
        Me.txtResultValueChA.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtResultValueChA.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtResultValueChA.Enabled = False
        Me.txtResultValueChA.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtResultValueChA.Location = New System.Drawing.Point(175, 181)
        Me.txtResultValueChA.Name = "txtResultValueChA"
        Me.txtResultValueChA.Size = New System.Drawing.Size(197, 27)
        Me.txtResultValueChA.TabIndex = 21
        Me.txtResultValueChA.Text = "000.00000"
        Me.txtResultValueChA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNPLCchA
        '
        Me.txtNPLCchA.Location = New System.Drawing.Point(77, 154)
        Me.txtNPLCchA.Name = "txtNPLCchA"
        Me.txtNPLCchA.Size = New System.Drawing.Size(34, 22)
        Me.txtNPLCchA.TabIndex = 14
        Me.txtNPLCchA.Text = "0.2"
        '
        'nplcLabel
        '
        Me.nplcLabel.AutoSize = True
        Me.nplcLabel.Location = New System.Drawing.Point(6, 156)
        Me.nplcLabel.Name = "nplcLabel"
        Me.nplcLabel.Size = New System.Drawing.Size(69, 17)
        Me.nplcLabel.TabIndex = 16
        Me.nplcLabel.Text = "NPLC (s):"
        '
        'chkMeasureAutoRangeChA
        '
        Me.chkMeasureAutoRangeChA.Checked = True
        Me.chkMeasureAutoRangeChA.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMeasureAutoRangeChA.Location = New System.Drawing.Point(133, 152)
        Me.chkMeasureAutoRangeChA.Name = "chkMeasureAutoRangeChA"
        Me.chkMeasureAutoRangeChA.Size = New System.Drawing.Size(105, 28)
        Me.chkMeasureAutoRangeChA.TabIndex = 18
        Me.chkMeasureAutoRangeChA.Text = "Autorange"
        '
        'txtMeasureRangeChA
        '
        Me.txtMeasureRangeChA.Location = New System.Drawing.Point(291, 154)
        Me.txtMeasureRangeChA.Name = "txtMeasureRangeChA"
        Me.txtMeasureRangeChA.Size = New System.Drawing.Size(55, 22)
        Me.txtMeasureRangeChA.TabIndex = 15
        Me.txtMeasureRangeChA.Text = "20"
        '
        'measureRangeLabel
        '
        Me.measureRangeLabel.AutoSize = True
        Me.measureRangeLabel.Location = New System.Drawing.Point(239, 156)
        Me.measureRangeLabel.Name = "measureRangeLabel"
        Me.measureRangeLabel.Size = New System.Drawing.Size(54, 17)
        Me.measureRangeLabel.TabIndex = 17
        Me.measureRangeLabel.Text = "Range:"
        '
        'cboMeasureFunctionChA
        '
        Me.cboMeasureFunctionChA.Items.AddRange(New Object() {"Current", "Voltage", "Power", "Resistance"})
        Me.cboMeasureFunctionChA.Location = New System.Drawing.Point(123, 124)
        Me.cboMeasureFunctionChA.Name = "cboMeasureFunctionChA"
        Me.cboMeasureFunctionChA.Size = New System.Drawing.Size(107, 24)
        Me.cboMeasureFunctionChA.TabIndex = 19
        '
        'measureFunctionLabel
        '
        Me.measureFunctionLabel.AutoSize = True
        Me.measureFunctionLabel.Location = New System.Drawing.Point(6, 127)
        Me.measureFunctionLabel.Name = "measureFunctionLabel"
        Me.measureFunctionLabel.Size = New System.Drawing.Size(121, 17)
        Me.measureFunctionLabel.TabIndex = 20
        Me.measureFunctionLabel.Text = "Measure function:"
        '
        'btnOutputStateChA
        '
        Me.btnOutputStateChA.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnOutputStateChA.ForeColor = System.Drawing.Color.Black
        Me.btnOutputStateChA.Location = New System.Drawing.Point(258, 11)
        Me.btnOutputStateChA.Name = "btnOutputStateChA"
        Me.btnOutputStateChA.Size = New System.Drawing.Size(114, 35)
        Me.btnOutputStateChA.TabIndex = 9
        Me.btnOutputStateChA.Text = "Output is OFF"
        Me.btnOutputStateChA.UseVisualStyleBackColor = False
        '
        'statusBar
        '
        Me.statusBar.Location = New System.Drawing.Point(0, 509)
        Me.statusBar.Name = "statusBar"
        Me.statusBar.Size = New System.Drawing.Size(404, 27)
        Me.statusBar.TabIndex = 29
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtResultValueChB)
        Me.GroupBox1.Controls.Add(Me.txtNPLCchB)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.chkMeasureAutoRangeChB)
        Me.GroupBox1.Controls.Add(Me.txtMeasureRangeChB)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.cboMeasureFunctionChB)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.btnOutputStateChB)
        Me.GroupBox1.Controls.Add(Me.sourceLevelValueChB)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.cboSourceFunctionChB)
        Me.GroupBox1.Controls.Add(Me.chkSourceAutoRangeChB)
        Me.GroupBox1.Controls.Add(Me.txtSourceLimitChB)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.txtSourcRangeChB)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 286)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(385, 217)
        Me.GroupBox1.TabIndex = 29
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Channel B"
        '
        'txtResultValueChB
        '
        Me.txtResultValueChB.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtResultValueChB.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtResultValueChB.Enabled = False
        Me.txtResultValueChB.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtResultValueChB.Location = New System.Drawing.Point(175, 181)
        Me.txtResultValueChB.Name = "txtResultValueChB"
        Me.txtResultValueChB.Size = New System.Drawing.Size(197, 27)
        Me.txtResultValueChB.TabIndex = 21
        Me.txtResultValueChB.Text = "000.00000"
        Me.txtResultValueChB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtNPLCchB
        '
        Me.txtNPLCchB.Location = New System.Drawing.Point(77, 154)
        Me.txtNPLCchB.Name = "txtNPLCchB"
        Me.txtNPLCchB.Size = New System.Drawing.Size(34, 22)
        Me.txtNPLCchB.TabIndex = 14
        Me.txtNPLCchB.Text = "0.2"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 156)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 17)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "NPLC (s):"
        '
        'chkMeasureAutoRangeChB
        '
        Me.chkMeasureAutoRangeChB.Checked = True
        Me.chkMeasureAutoRangeChB.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMeasureAutoRangeChB.Location = New System.Drawing.Point(133, 152)
        Me.chkMeasureAutoRangeChB.Name = "chkMeasureAutoRangeChB"
        Me.chkMeasureAutoRangeChB.Size = New System.Drawing.Size(105, 28)
        Me.chkMeasureAutoRangeChB.TabIndex = 18
        Me.chkMeasureAutoRangeChB.Text = "Autorange"
        '
        'txtMeasureRangeChB
        '
        Me.txtMeasureRangeChB.Location = New System.Drawing.Point(291, 154)
        Me.txtMeasureRangeChB.Name = "txtMeasureRangeChB"
        Me.txtMeasureRangeChB.Size = New System.Drawing.Size(55, 22)
        Me.txtMeasureRangeChB.TabIndex = 15
        Me.txtMeasureRangeChB.Text = "20"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(239, 156)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 17)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Range:"
        '
        'cboMeasureFunctionChB
        '
        Me.cboMeasureFunctionChB.Items.AddRange(New Object() {"Current", "Voltage", "Power", "Resistance"})
        Me.cboMeasureFunctionChB.Location = New System.Drawing.Point(123, 124)
        Me.cboMeasureFunctionChB.Name = "cboMeasureFunctionChB"
        Me.cboMeasureFunctionChB.Size = New System.Drawing.Size(107, 24)
        Me.cboMeasureFunctionChB.TabIndex = 19
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 127)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 17)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Measure function:"
        '
        'btnOutputStateChB
        '
        Me.btnOutputStateChB.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnOutputStateChB.ForeColor = System.Drawing.Color.Black
        Me.btnOutputStateChB.Location = New System.Drawing.Point(258, 11)
        Me.btnOutputStateChB.Name = "btnOutputStateChB"
        Me.btnOutputStateChB.Size = New System.Drawing.Size(114, 35)
        Me.btnOutputStateChB.TabIndex = 9
        Me.btnOutputStateChB.Text = "Output is OFF"
        Me.btnOutputStateChB.UseVisualStyleBackColor = False
        '
        'sourceLevelValueChB
        '
        Me.sourceLevelValueChB.CausesValidation = False
        Me.sourceLevelValueChB.CoercionInterval = 0.001R
        Me.sourceLevelValueChB.FormatMode = NationalInstruments.UI.NumericFormatMode.CreateGenericMode("0000.0000")
        Me.sourceLevelValueChB.Location = New System.Drawing.Point(99, 82)
        Me.sourceLevelValueChB.Name = "sourceLevelValueChB"
        Me.sourceLevelValueChB.Size = New System.Drawing.Size(106, 22)
        Me.sourceLevelValueChB.TabIndex = 8
        Me.sourceLevelValueChB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(111, 17)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Source function:"
        '
        'cboSourceFunctionChB
        '
        Me.cboSourceFunctionChB.Items.AddRange(New Object() {"Current", "Voltage"})
        Me.cboSourceFunctionChB.Location = New System.Drawing.Point(123, 20)
        Me.cboSourceFunctionChB.Name = "cboSourceFunctionChB"
        Me.cboSourceFunctionChB.Size = New System.Drawing.Size(107, 24)
        Me.cboSourceFunctionChB.TabIndex = 5
        '
        'chkSourceAutoRangeChB
        '
        Me.chkSourceAutoRangeChB.Checked = True
        Me.chkSourceAutoRangeChB.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSourceAutoRangeChB.Location = New System.Drawing.Point(133, 49)
        Me.chkSourceAutoRangeChB.Name = "chkSourceAutoRangeChB"
        Me.chkSourceAutoRangeChB.Size = New System.Drawing.Size(100, 28)
        Me.chkSourceAutoRangeChB.TabIndex = 3
        Me.chkSourceAutoRangeChB.Text = "Autorange"
        '
        'txtSourceLimitChB
        '
        Me.txtSourceLimitChB.Location = New System.Drawing.Point(49, 52)
        Me.txtSourceLimitChB.Name = "txtSourceLimitChB"
        Me.txtSourceLimitChB.Size = New System.Drawing.Size(52, 22)
        Me.txtSourceLimitChB.TabIndex = 1
        Me.txtSourceLimitChB.Text = "0.2"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(41, 17)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Limit:"
        '
        'txtSourcRangeChB
        '
        Me.txtSourcRangeChB.Location = New System.Drawing.Point(289, 52)
        Me.txtSourcRangeChB.Name = "txtSourcRangeChB"
        Me.txtSourcRangeChB.Size = New System.Drawing.Size(61, 22)
        Me.txtSourcRangeChB.TabIndex = 1
        Me.txtSourcRangeChB.Text = "1E-8"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(239, 54)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 17)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Range:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 83)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(95, 17)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "Source Level:"
        '
        'frmSimpleKeithleyControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(404, 536)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.connectionGroupBox)
        Me.Controls.Add(Me.sourceConfigurationGroupBox)
        Me.Controls.Add(Me.statusBar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmSimpleKeithleyControl"
        Me.Text = "Keithley 26xx"
        CType(Me.sourceLevelValueChA, System.ComponentModel.ISupportInitialize).EndInit()
        Me.connectionGroupBox.ResumeLayout(False)
        Me.connectionGroupBox.PerformLayout()
        Me.sourceConfigurationGroupBox.ResumeLayout(False)
        Me.sourceConfigurationGroupBox.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.sourceLevelValueChB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents sourceLevelValueChA As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents sourceFunctionLabel As System.Windows.Forms.Label
    Friend WithEvents cboSourceFunctionChA As System.Windows.Forms.ComboBox
    Friend WithEvents chkSourceAutoRangeChA As System.Windows.Forms.CheckBox
    Friend WithEvents txtSourceLimitChA As System.Windows.Forms.TextBox
    Friend WithEvents sourceLimitLabel As System.Windows.Forms.Label
    Friend WithEvents txtSourcRangeChA As System.Windows.Forms.TextBox
    Friend WithEvents sourceRangeLabel As System.Windows.Forms.Label
    Friend WithEvents sourceLevelLabel As System.Windows.Forms.Label
    Friend WithEvents initializeButton As System.Windows.Forms.Button
    Friend WithEvents resourceDescTextBox As System.Windows.Forms.TextBox
    Friend WithEvents resourceDescLabel As System.Windows.Forms.Label
    Friend WithEvents connectionGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents sourceConfigurationGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents statusBar As System.Windows.Forms.StatusBar
    Friend WithEvents btnOutputStateChA As System.Windows.Forms.Button
    Friend WithEvents txtResultValueChA As System.Windows.Forms.TextBox
    Friend WithEvents txtNPLCchA As System.Windows.Forms.TextBox
    Friend WithEvents nplcLabel As System.Windows.Forms.Label
    Friend WithEvents chkMeasureAutoRangeChA As System.Windows.Forms.CheckBox
    Friend WithEvents txtMeasureRangeChA As System.Windows.Forms.TextBox
    Friend WithEvents measureRangeLabel As System.Windows.Forms.Label
    Friend WithEvents cboMeasureFunctionChA As System.Windows.Forms.ComboBox
    Friend WithEvents measureFunctionLabel As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtResultValueChB As System.Windows.Forms.TextBox
    Friend WithEvents txtNPLCchB As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkMeasureAutoRangeChB As System.Windows.Forms.CheckBox
    Friend WithEvents txtMeasureRangeChB As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboMeasureFunctionChB As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnOutputStateChB As System.Windows.Forms.Button
    Friend WithEvents sourceLevelValueChB As NationalInstruments.UI.WindowsForms.NumericEdit
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cboSourceFunctionChB As System.Windows.Forms.ComboBox
    Friend WithEvents chkSourceAutoRangeChB As System.Windows.Forms.CheckBox
    Friend WithEvents txtSourceLimitChB As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtSourcRangeChB As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
