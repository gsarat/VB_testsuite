
' SimpleSourceMeasure Source Code                                 

' This sample demonstrates how to perform a simple source measure
' operation. You can control the source and measure functions used, as well
' as the source delay and the measure reading count.  The results are presented
' in a listbox.  Auto-ranging or a manually programmed range can be
' used.


Imports Keithley.Ke26XXA.Interop
Imports System.Runtime.InteropServices
Imports Ivi.Driver.Interop

Public Class frmSimpleKeithleyControl
    Inherits System.Windows.Forms.Form

    Private _driver As IKe26XXA
    Private OutputstateChA As Boolean = False
    Private OutputstateChB As Boolean = False
    Dim bufferNameChA As String = "MyBuf"
    Dim bufferNameChB As String = "MyBufB"
    Dim bMeasuringChA As Boolean = False
    Dim bMeasuringChB As Boolean = False

    Private Sub frmSimpleKeithleyControl_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        ' Close the driver if it's already been initialized
        If (_driver.Initialized) Then
            _driver.Close()
        End If
    End Sub


    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Instantiate the driver on load.  Note that Initialize
        ' must still be called in order to call functions on the driver.
        Try

            _driver = New Ke26XXA

            ' Set the source function to a default value
            cboSourceFunctionChA.SelectedIndex = 0
            cboSourceFunctionChB.SelectedIndex = 0

            ' Set the measure function to a default value
            cboMeasureFunctionChA.SelectedIndex = 1
            cboMeasureFunctionChB.SelectedIndex = 1

            statusBar.Text = "Instrument not initialized."

            UpdateUI()

            OutputstateChA = False
            OutputstateChB = False
            btnOutputStateChA.Text = "Output is OFF"
            btnOutputStateChA.BackColor = Color.DarkRed
            btnOutputStateChB.Text = "Output is OFF"
            btnOutputStateChB.BackColor = Color.DarkRed

        Catch ex As Exception
            MessageBox.Show("Failed to create driver class:  " + ex.Message, "Ke26XX Simple Source Measure", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub
    Private Sub UpdateUI()
        txtSourcRangeChA.Enabled = Not chkSourceAutoRangeChA.Checked
        txtMeasureRangeChA.Enabled = Not chkMeasureAutoRangeChA.Checked

        txtSourcRangeChB.Enabled = Not chkSourceAutoRangeChB.Checked
        txtMeasureRangeChB.Enabled = Not chkMeasureAutoRangeChB.Checked

    End Sub

    Private Sub HandleInstrumentError(ByVal ex As COMException)

        If ex.ErrorCode = IviDriver_ErrorCodes.E_IVI_INSTRUMENT_STATUS Then
            Dim errorCode As Integer = -1
            Dim errorMessage As String = ""
            _driver.Utility.ErrorQuery(errorCode, errorMessage)

            MessageBox.Show("Instrument Error: " + errorMessage)

            'Clear the error queue		
            While Not (errorCode = 0)
                _driver.Utility.ErrorQuery(errorCode, errorMessage)
            End While
        Else
            MessageBox.Show(String.Format("Error {0}: {1}", ex.ErrorCode, ex.Message), "Ke26XX Simple Source Measure", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        btnOutputStateChA.Text = "Output is OFF"
        btnOutputStateChA.BackColor = Color.DarkRed
        btnOutputStateChB.Text = "Output is OFF"
        btnOutputStateChB.BackColor = Color.DarkRed

        statusBar.Text = "Ready."

    End Sub

    Private Sub initializeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles initializeButton.Click

        Try
            ' Disable the Initialize button to provide visual feedback
            initializeButton.Enabled = False

            ' Close the driver if it's already been initialized
            If (_driver.Initialized) Then
                _driver.Close()
            End If

            statusBar.Text = "Initializing..."

            Dim options As String = "QueryInstrStatus=True"

            _driver.Initialize(resourceDescTextBox.Text, True, True, options)

            statusBar.Text = "Ready."
        Catch ex As Exception
            MessageBox.Show("Error:  " + ex.Message, "Ke26XX Simple Source Measure", MessageBoxButtons.OK, MessageBoxIcon.Error)

            statusBar.Text = "Initialize failed."
        Finally
            ' Re-enable the Initialize button
            initializeButton.Enabled = True
        End Try

    End Sub

    Private Sub Turn_OutputA_On()
        Dim sSMUchannel As String = "A"
        Dim delaytime As Double = 100

        If Not _driver.Initialized Then
            MessageBox.Show("Instrument not initialized.", "Ke26XX Simple Source Measure", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try

            _driver.Utility.Reset()

            ' Configure the source
            Dim sourceFunction As String = cboSourceFunctionChA.SelectedItem.ToString()

            Select Case sourceFunction
                Case "Current"
                    _driver.Source.Function(sSMUchannel) = Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCAmps
                    _driver.Source.Voltage.Limit(sSMUchannel) = Double.Parse(txtSourceLimitChA.Text)

                    If (chkSourceAutoRangeChA.Checked) Then
                        _driver.Source.Current.AutoRangeEnabled(sSMUchannel) = True
                    Else
                        _driver.Source.Current.Range(sSMUchannel) = Double.Parse(txtSourcRangeChA.Text)
                    End If

                    _driver.Source.Current.Level(sSMUchannel) = Double.Parse(sourceLevelValueChA.Value)

                Case "Voltage"
                    _driver.Source.Function(sSMUchannel) = Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCVolts
                    _driver.Source.Current.Limit(sSMUchannel) = Double.Parse(txtSourceLimitChA.Text)

                    If (chkSourceAutoRangeChA.Checked) Then
                        _driver.Source.Voltage.AutoRangeEnabled(sSMUchannel) = True
                    Else
                        _driver.Source.Voltage.Range(sSMUchannel) = Double.Parse(txtSourcRangeChA.Text)
                    End If

                    _driver.Source.Voltage.Level(sSMUchannel) = Double.Parse(sourceLevelValueChA.Value)
            End Select

            _driver.Source.Delay(sSMUchannel) = 0

            ' Configure the measure
            Dim measureFunction As String = cboMeasureFunctionChA.SelectedItem.ToString()

            Select Case measureFunction
                Case "Current"
                    If (chkMeasureAutoRangeChA.Checked) Then
                        _driver.Measurement.Current.AutoRangeEnabled(sSMUchannel) = True
                    Else
                        _driver.Measurement.Current.Range(sSMUchannel) = Double.Parse(txtMeasureRangeChA.Text)
                    End If
                Case "Voltage"
                    If (chkMeasureAutoRangeChA.Checked) Then
                        _driver.Measurement.Voltage.AutoRangeEnabled(sSMUchannel) = True
                    Else
                        _driver.Measurement.Voltage.Range(sSMUchannel) = Double.Parse(txtMeasureRangeChA.Text)
                    End If
            End Select


            ' Configure the reading count, delay, and integration time
            _driver.Measurement.Count(sSMUchannel) = 1
            _driver.Measurement.Delay(sSMUchannel) = 0
            _driver.Measurement.NPLC(sSMUchannel) = Double.Parse(txtNPLCchA.Text)

            ' Create a measurement buffer to store the readings

            _driver.Measurement.Buffer.Create(sSMUchannel, bufferNameChA, 1)

            ' Turn the output on
            _driver.Source.OutputEnabled(sSMUchannel) = True

            ' Retrieve all of the readings from the buffer
            Application.DoEvents()

            While bMeasuringChA
                ' Perform the measurement and store the readings in the buffer

                statusBar.Text = "Measuring..."

                Select Case measureFunction
                    Case "Current"
                        _driver.Measurement.Current.MeasureMultiple(sSMUchannel, bufferNameChA)
                    Case "Voltage"
                        _driver.Measurement.Voltage.MeasureMultiple(sSMUchannel, bufferNameChA)
                    Case "Power"
                        _driver.Measurement.Power.MeasureMultiple(sSMUchannel, bufferNameChA)
                    Case "Resistance"
                        _driver.Measurement.Resistance.MeasureMultiple(sSMUchannel, bufferNameChA)
                End Select

                ' Retrieve all of the readings from the buffer
                Dim readings() As Double = _driver.Measurement.Buffer.MeasureData.GetReadings(bufferNameChA, 1, 1)

                For Each reading As Double In readings
                    'resultsListBox.Items.Add(reading.ToString("E"))
                    txtResultValueChA.Text = reading.ToString("000.000E+0")
                Next

                ' Clear the buffer
                Threading.Thread.Sleep(50)
                Application.DoEvents()
                _driver.Measurement.Buffer.Clear("MyBuf")

            End While

            Application.DoEvents()

            ' Turn the output OFF
            _driver.Source.OutputEnabled(sSMUchannel) = False

            statusBar.Text = "Ready."

        Catch ex As COMException
            HandleInstrumentError(ex)

        Finally
            btnOutputStateChA.Text = "Output is OFF"
            btnOutputStateChA.BackColor = Color.DarkRed
            bMeasuringChA = False
        End Try

    End Sub


    Private Sub Turn_OutputB_On()
        Dim sSMUchannel As String = "B"
        Dim delaytime As Double = 100

        If Not _driver.Initialized Then
            MessageBox.Show("Instrument not initialized.", "Ke26XX Simple Source Measure", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try

            _driver.Utility.Reset()

            ' Configure the source
            Dim sourceFunction As String = cboSourceFunctionChB.SelectedItem.ToString()

            Select Case sourceFunction
                Case "Current"
                    _driver.Source.Function(sSMUchannel) = Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCAmps
                    _driver.Source.Voltage.Limit(sSMUchannel) = Double.Parse(txtSourceLimitChB.Text)

                    If (chkSourceAutoRangeChB.Checked) Then
                        _driver.Source.Current.AutoRangeEnabled(sSMUchannel) = True
                    Else
                        _driver.Source.Current.Range(sSMUchannel) = Double.Parse(txtSourcRangeChB.Text)
                    End If

                    _driver.Source.Current.Level(sSMUchannel) = Double.Parse(sourceLevelValueChB.Value)

                Case "Voltage"
                    _driver.Source.Function(sSMUchannel) = Ke26XXASourceFunctionEnum.Ke26XXASourceFunctionDCVolts
                    _driver.Source.Current.Limit(sSMUchannel) = Double.Parse(txtSourceLimitChB.Text)

                    If (chkSourceAutoRangeChB.Checked) Then
                        _driver.Source.Voltage.AutoRangeEnabled(sSMUchannel) = True
                    Else
                        _driver.Source.Voltage.Range(sSMUchannel) = Double.Parse(txtSourcRangeChB.Text)
                    End If

                    _driver.Source.Voltage.Level(sSMUchannel) = Double.Parse(sourceLevelValueChB.Value)
            End Select

            _driver.Source.Delay(sSMUchannel) = 0

            ' Configure the measure
            Dim measureFunction As String = cboMeasureFunctionChB.SelectedItem.ToString()

            Select Case measureFunction
                Case "Current"
                    If (chkMeasureAutoRangeChB.Checked) Then
                        _driver.Measurement.Current.AutoRangeEnabled(sSMUchannel) = True
                    Else
                        _driver.Measurement.Current.Range(sSMUchannel) = Double.Parse(txtMeasureRangeChB.Text)
                    End If
                Case "Voltage"
                    If (chkMeasureAutoRangeChB.Checked) Then
                        _driver.Measurement.Voltage.AutoRangeEnabled(sSMUchannel) = True
                    Else
                        _driver.Measurement.Voltage.Range(sSMUchannel) = Double.Parse(txtMeasureRangeChB.Text)
                    End If
            End Select


            ' Configure the reading count, delay, and integration time
            _driver.Measurement.Count(sSMUchannel) = 1
            _driver.Measurement.Delay(sSMUchannel) = 0
            _driver.Measurement.NPLC(sSMUchannel) = Double.Parse(txtNPLCchB.Text)

            ' Create a measurement buffer to store the readings

            _driver.Measurement.Buffer.Create(sSMUchannel, bufferNameChB, 1)

            ' Turn the output on
            _driver.Source.OutputEnabled(sSMUchannel) = True

            ' Retrieve all of the readings from the buffer
            Application.DoEvents()

            While bMeasuringChB
                ' Perform the measurement and store the readings in the buffer

                statusBar.Text = "Measuring..."

                Select Case measureFunction
                    Case "Current"
                        _driver.Measurement.Current.MeasureMultiple(sSMUchannel, bufferNameChB)
                    Case "Voltage"
                        _driver.Measurement.Voltage.MeasureMultiple(sSMUchannel, bufferNameChB)
                    Case "Power"
                        _driver.Measurement.Power.MeasureMultiple(sSMUchannel, bufferNameChB)
                    Case "Resistance"
                        _driver.Measurement.Resistance.MeasureMultiple(sSMUchannel, bufferNameChB)
                End Select

                ' Retrieve all of the readings from the buffer
                Dim readings() As Double = _driver.Measurement.Buffer.MeasureData.GetReadings(bufferNameChB, 1, 1)

                For Each reading As Double In readings
                    'resultsListBox.Items.Add(reading.ToString("E"))
                    txtResultValueChB.Text = reading.ToString("000.000E+0")
                Next

                ' Clear the buffer
                Threading.Thread.Sleep(50)
                Application.DoEvents()
                _driver.Measurement.Buffer.Clear("MyBufB")

            End While

            Application.DoEvents()

            ' Turn the output OFF
            _driver.Source.OutputEnabled(sSMUchannel) = False

            statusBar.Text = "Ready."

        Catch ex As COMException
            HandleInstrumentError(ex)

        Finally
            btnOutputStateChB.Text = "Output is OFF"
            btnOutputStateChB.BackColor = Color.DarkRed
            bMeasuringChB = False
        End Try

    End Sub

    Private Sub chkSourceAutoRangeChA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSourceAutoRangeChA.CheckedChanged

        txtSourcRangeChA.Enabled = Not chkSourceAutoRangeChA.Checked

    End Sub

    Private Sub chkMeasureAutoRangeChA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMeasureAutoRangeChA.CheckedChanged

        txtMeasureRangeChA.Enabled = Not chkMeasureAutoRangeChA.Checked

    End Sub

    Private Sub chkSourceAutoRangeChB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSourceAutoRangeChB.CheckedChanged

        txtSourcRangeChB.Enabled = Not chkSourceAutoRangeChB.Checked

    End Sub

    Private Sub chkMeasureAutoRangeChB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMeasureAutoRangeChB.CheckedChanged

        txtMeasureRangeChB.Enabled = Not chkMeasureAutoRangeChB.Checked

    End Sub

    Private Sub btnOutputStateChA_Click(sender As Object, e As EventArgs) Handles btnOutputStateChA.Click

        Try

            OutputstateChA = Not (OutputstateChA)

            If OutputstateChA = False Then
                btnOutputStateChA.Text = "Output is OFF"
                btnOutputStateChA.BackColor = Color.DarkRed
                bMeasuringChA = False
            End If

            If OutputstateChA = True Then
                btnOutputStateChA.Text = "Output is ON"
                btnOutputStateChA.BackColor = Color.Green
                bMeasuringChA = True
                Call Turn_OutputA_On()
            End If

        Catch ex As Exception
            MessageBox.Show("Error:" + vbCrLf + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub btnOutputStateChB_Click(sender As Object, e As EventArgs) Handles btnOutputStateChB.Click

        Try

            OutputstateChB = Not (OutputstateChB)

            If OutputstateChB = False Then
                btnOutputStateChB.Text = "Output is OFF"
                btnOutputStateChB.BackColor = Color.DarkRed
                bMeasuringChB = False
            End If

            If OutputstateChB = True Then
                btnOutputStateChB.Text = "Output is ON"
                btnOutputStateChB.BackColor = Color.Green
                bMeasuringChB = True
                Call Turn_OutputB_On()
            End If

        Catch ex As Exception
            MessageBox.Show("Error:" + vbCrLf + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

    Private Sub sourceLevelValueChA_AfterChangeValue(sender As Object, e As AfterChangeNumericValueEventArgs) Handles sourceLevelValueChA.AfterChangeValue
        If bMeasuringChA Then
            ' Configure the source
            Dim sourceFunction As String = cboSourceFunctionChA.SelectedItem.ToString()
            Dim sSMUchannel As String = "A"

            Try
                Select Case sourceFunction
                    Case "Current"

                        _driver.Source.Current.Level(sSMUchannel) = Double.Parse(sourceLevelValueChA.Value)

                    Case "Voltage"

                        _driver.Source.Voltage.Level(sSMUchannel) = Double.Parse(sourceLevelValueChA.Value)

                End Select

            Catch ex As COMException
                HandleInstrumentError(ex)

            Finally

            End Try
        End If
    End Sub

    Private Sub sourceLevelValueChA_KeyDown(sender As Object, e As KeyEventArgs) Handles sourceLevelValueChA.KeyDown
        If e.KeyCode = Keys.Enter Then

            ' Configure the source
            Dim sourceFunction As String = cboSourceFunctionChA.SelectedItem.ToString()
            Dim sSMUchannel As String = "A"

            Try
                Select Case sourceFunction
                    Case "Current"

                        _driver.Source.Current.Level(sSMUchannel) = Double.Parse(sourceLevelValueChA.Value)

                    Case "Voltage"

                        _driver.Source.Voltage.Level(sSMUchannel) = Double.Parse(sourceLevelValueChA.Value)

                End Select

            Catch ex As COMException
                HandleInstrumentError(ex)

            Finally

            End Try
        End If
    End Sub


    Private Sub sourceLevelValueChB_AfterChangeValue(sender As Object, e As AfterChangeNumericValueEventArgs) Handles sourceLevelValueChB.AfterChangeValue
        If bMeasuringChB Then
            ' Configure the source
            Dim sourceFunction As String = cboSourceFunctionChB.SelectedItem.ToString()
            Dim sSMUchannel As String = "B"

            Try
                Select Case sourceFunction
                    Case "Current"

                        _driver.Source.Current.Level(sSMUchannel) = Double.Parse(sourceLevelValueChB.Value)

                    Case "Voltage"

                        _driver.Source.Voltage.Level(sSMUchannel) = Double.Parse(sourceLevelValueChB.Value)

                End Select

            Catch ex As COMException
                HandleInstrumentError(ex)

            Finally

            End Try
        End If
    End Sub


    Private Sub sourceLevelValueChB_KeyDown(sender As Object, e As KeyEventArgs) Handles sourceLevelValueChB.KeyDown
        If e.KeyCode = Keys.Enter Then

            ' Configure the source
            Dim sourceFunction As String = cboSourceFunctionChB.SelectedItem.ToString()
            Dim sSMUchannel As String = "B"

            Try
                Select Case sourceFunction
                    Case "Current"

                        _driver.Source.Current.Level(sSMUchannel) = Double.Parse(sourceLevelValueChB.Value)

                    Case "Voltage"

                        _driver.Source.Voltage.Level(sSMUchannel) = Double.Parse(sourceLevelValueChB.Value)

                End Select

            Catch ex As COMException
                HandleInstrumentError(ex)

            Finally

            End Try
        End If
    End Sub

End Class
