'Contains the Device-Assignments
'Provides the HardwareDevices for the Measurement Classes (in "DeviceAssignments")
'Loads and Saves the Assignments
'"Recognizes" what HardwareDevice-Classes are available (by scanning the Class-Structure of SubClasses of intfcHardwareDevice)
'"Recognizes" what HardwareDevice-Classes can be used for a type of needed Hardware (e.g. what LaserClasses can be used for clsDeviceAssignmentHardware.Laser)

Public Class clsDeviceAssignments

    Public Class clsDeviceAssignmentHardware

        'Phase Electrodes as current source
        Public CurrentSource_PhaseElectrodeP1 As intfcCurrentSource
        Public CurrentSource_PhaseElectrodeP2 As intfcCurrentSource

        'Phase Electrodes for complex Modulators
        Public VoltageSource_PhaseElectrodeP1 As intfcVoltageSource
        Public VoltageSource_PhaseElectrodeP2 As intfcVoltageSource

        'Monitor Diodes for complex Modulators
        Public VoltageSource_MonitorDiodeM1Bias As intfcVoltageSource
        Public VoltageSource_MonitorDiodeM2Bias As intfcVoltageSource
        Public AmMeter_MonitorDiodeM1 As intfcCurrentMeter


        Public Voltmeter_Laser As intfcVoltageMeter

        Public VoltageSource_UElectrode As intfcVoltageSource
        Public VoltageSource_IElectrode As intfcVoltageSource

        'Signal Electrodes for complex Modulators
        Public VoltageSource_SignalElectrodeS1 As intfcVoltageSource
        Public VoltageSource_SignalElectrodeS2 As intfcVoltageSource
        Public VoltageSource_SignalElectrodeS3 As intfcVoltageSource
        Public VoltageSource_SignalElectrodeS4 As intfcVoltageSource

        Public CurrentSource_LaserDiodeDriver As intfcCurrentSource

        Public TemperatureController As intfcTemperatureController

        Public OpticalSpectrumAnalyser As intfcOSA

        Public TemperatureControl_TED8040 As clsPro8TED8040

    End Class

    Public Class clsAssignmentInfo
        Public DeviceObject As intfcMeasurementInstrument
        Public DeviceSetupName As String

        Public Sub New()
            '
        End Sub

        Public Sub New(ByVal objDevice As intfcMeasurementInstrument, ByVal strName As String)
            DeviceObject = objDevice
            DeviceSetupName = strName
        End Sub

    End Class

    'wird mittels ReadFromConfig() zu Startbeginn des Programmes erstellt
    Private Shared mListAssignmentInfo As New Dictionary(Of String, clsAssignmentInfo)

    Public Shared DeviceAssignments As New clsDeviceAssignmentHardware

    'übergibt an gConfig.Hardware_ReadFromConfig das Device welches "assignt" wurde und dessen name als string
    'übergibt in _default.config gespeicherte Wertzuweisungen an die statischen Prperties der devices
    Public Shared Sub ReInitHardware(Optional ByVal FullReLoad As Boolean = False)

        For Each anAssignment As clsAssignmentInfo In mListAssignmentInfo.Values
            'true, wenn bereits Geräte zu DeviceAssignments hinzugefügt wurden, siehe mListAssignmentInfo
            If anAssignment IsNot Nothing Then

                If anAssignment.DeviceObject IsNot Nothing Then

                    'für alle Geräte gültig
                    'methode könnte auch anAssignment übergeben
                    gConfig.Hardware_ReadFromConfig(anAssignment.DeviceObject, anAssignment.DeviceSetupName)


                End If

            End If
        Next

    End Sub

    Public Shared Function AvailableHardwareClasses() As List(Of System.Type)

        AvailableHardwareClasses = New List(Of System.Type)

        'Debug
        Dim aAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
        Dim aTypeList As System.Type() = System.Reflection.Assembly.GetExecutingAssembly.GetTypes

        'find available Hardware-Classes:
        'searches all classes of project to find all classes from type of intfcHardwareDevices
        'and adds them to AvailableHardwareClasses
        For Each aType As System.Type In System.Reflection.Assembly.GetExecutingAssembly.GetTypes

            If aType.IsClass And Not aType.IsAbstract Then
                If GetType(intfcMeasurementInstrument).IsAssignableFrom(aType) Then AvailableHardwareClasses.Add(aType)
            End If

        Next

    End Function

    Public Shared Function getAvailableDevices(ByVal aType As String) As List(Of System.Type)

        getAvailableDevices = New List(Of System.Type)

        Try

            For Each aHardwareClass As System.Type In AvailableHardwareClasses()
                If GetType(clsDeviceAssignmentHardware).GetField(aType).FieldType.IsAssignableFrom(aHardwareClass) Then getAvailableDevices.Add(aHardwareClass)
            Next

        Catch ex As Exception
            ThrowError("Unknown AssignmentType")

        End Try

    End Function

    Public Shared Property DeviceAssignment(ByVal aType As String) As clsAssignmentInfo
        Get

            Try

                If mListAssignmentInfo.ContainsKey(aType) Then
                    Return mListAssignmentInfo.Item(aType)
                Else
                    Return Nothing
                End If

                'Return CType(GetType(clsDeviceAssignmentHardware).GetField(aType).GetValue(DeviceAssignments), intfcHardwareDevice)

            Catch ex As Exception
                ThrowError("Unknown AssignmentType")
                Return Nothing

            End Try

        End Get
        Set(ByVal value As clsAssignmentInfo)

            Try
                If mListAssignmentInfo.ContainsKey(aType) Then mListAssignmentInfo.Remove(aType)
                mListAssignmentInfo.Add(aType, value)

                If value IsNot Nothing Then
                    GetType(clsDeviceAssignmentHardware).GetField(aType).SetValue(DeviceAssignments, value.DeviceObject)
                Else
                    GetType(clsDeviceAssignmentHardware).GetField(aType).SetValue(DeviceAssignments, Nothing)
                End If

            Catch ex As Exception
                ThrowError("Unknown AssignmentType")

            End Try

        End Set

    End Property

    Public Shared Function getSettableProperties(ByVal objTarget As intfcMeasurementInstrument) As List(Of System.Reflection.PropertyInfo)

        Dim propertyList As New List(Of String)

        If objTarget.GetType.BaseType Is GetType(clsKeithley24xx) Then

            propertyList.Add("SenseResistanceMode")
            propertyList.Add("SenseMax")
            propertyList.Add("SourceVoltageProtectionLevel")
            propertyList.Add("SourceCurrentProtectionLevel")
            propertyList.Add("SourceMode")

        ElseIf objTarget.GetType.BaseType Is GetType(clsKeithley26xx) Then

            propertyList.Add("TSPNode")
            propertyList.Add("Channel")

            propertyList.Add("SenseMax")
            propertyList.Add("SourceVoltageProtectionLevel")
            propertyList.Add("SourceCurrentProtectionLevel")

        ElseIf objTarget.GetType Is GetType(clsKeithley2510) Then
            propertyList.Add("SetPointTemperature")
            'PropertyDict.Add("SetPointTolerance")
            'PropertyDict.Add("SetPointToleranceCount")
            'PropertyDict.Add("Output")
        End If

        getSettableProperties = New List(Of System.Reflection.PropertyInfo)

        For Each aPropertyName In propertyList

            getSettableProperties.Add(objTarget.GetType.GetProperty(aPropertyName))

        Next

    End Function

    Private Shared Sub ThrowError(ByVal strReason As String)

        Dim cEx As New ExeptionHandler(strReason)
        Try
            Throw cEx
        Catch anEx As ExeptionHandler
            anEx.Log()
        End Try
        'Endstation
        Throw cEx

    End Sub

    Public Shared Sub WriteToConfig()

        Dim strSave As String = ""
        Dim assignInfoHardware As intfcMeasurementInstrument

        For Each anAssignment As KeyValuePair(Of String, clsAssignmentInfo) In mListAssignmentInfo ' In GetType(clsDeviceAssignmentHardware).GetFields

            strSave &= anAssignment.Key
            assignInfoHardware = anAssignment.Value.DeviceObject

            If assignInfoHardware IsNot Nothing Then

                strSave &= "=DeviceType:" & anAssignment.Value.DeviceSetupName '.GetType.ToString

                Dim strValue As String

                For Each aProperty In getSettableProperties(assignInfoHardware)

                    strValue = aProperty.GetValue(assignInfoHardware, Nothing).ToString()

                    'always use a Point as Decimal-Separator:
                    If IsNumeric(strValue) Then strValue = Trim(Str(CDec(strValue)))

                    strSave &= "," & aProperty.Name & ":" & strValue

                Next

            Else

                strSave &= "=Nothing"

            End If

            strSave &= ";"

        Next

        gConfig.WriteStringToConfig("DeviceAssignments", strSave)

        ReadFromConfig()

    End Sub

    Public Shared Sub ReadFromConfig()

        mListAssignmentInfo = New Dictionary(Of String, clsAssignmentInfo)

        For Each aSubString As String In gConfig.ReadStringFromConfig("DeviceAssignments", False).Split(CChar(";"))

            Dim oType As Object
            Dim oTmp As Object
            Dim anAssignType As String 'enumAssignTypes
            Dim objAssignInfo As New clsAssignmentInfo

            Dim strDeviceInfos As List(Of String) = Split(aSubString, ",").ToList

            anAssignType = Split(aSubString, "=")(0)
            'check if Hardware is known:
            If GetType(clsDeviceAssignmentHardware).GetField(anAssignType) Is Nothing Then Continue For

            If strDeviceInfos(0).ToLower.Contains("devicetype") Then

                Try
                    ' CType(Activator.CreateInstance(System.Type.GetType((Split(strDeviceInfos(0), ":")(1)))), intfcHardwareDevice)
                    objAssignInfo.DeviceSetupName = Split(strDeviceInfos(0), ":")(1)
                    oTmp = New clsConfigXMLConverter(gConfig.ReadStringFromConfig(objAssignInfo.DeviceSetupName)).XMLElement("DeviceType").XMLAttribute("DeviceType")

                    If CStr(oTmp).Contains("Modulator.") Then
                        oTmp = CObj(CStr(oTmp).Replace("Modulator.", "Hardware."))
                    End If
                    If CStr(oTmp).Contains("Laser.") Then
                        oTmp = CObj(CStr(oTmp).Replace("Laser.", "Hardware."))
                    End If
                    If CStr(oTmp).Contains("Optipot.") Then
                        oTmp = CObj(CStr(oTmp).Replace("Optipot.", "Hardware."))
                    End If


                    oType = System.Type.GetType(oTmp)
                    objAssignInfo.DeviceObject = CType(Activator.CreateInstance(oType), intfcMeasurementInstrument)
                Catch ex As Exception
                    Continue For
                End Try

                DeviceAssignment(anAssignType) = objAssignInfo

                strDeviceInfos.RemoveAt(0)
                For Each strProperty In strDeviceInfos

                    Dim aKey As String = strProperty.Split(CChar(":"))(0)
                    Dim aVal As String = strProperty.Split(CChar(":"))(1)

                    'Point was used as Decimal Separator, convert into Language Specific Format:
                    If IsNumeric(aVal) Then aVal = Val(aVal).ToString

                    With objAssignInfo.DeviceObject.GetType.GetProperty(aKey)

                        Try
                            If .PropertyType.IsEnum Then
                                .SetValue(objAssignInfo.DeviceObject, System.Enum.Parse(.PropertyType, aVal), Nothing)
                            Else
                                .SetValue(objAssignInfo.DeviceObject, ValueTypeConv(.PropertyType, aVal), Nothing)
                            End If

                        Catch ex As Exception
                            MessageBox.Show("Unknown PropertyKey """ & aKey & """ for Device """ & objAssignInfo.DeviceSetupName & """ found in DeviceAssignment-Configuration. Please check Settings.", "Unknown Property", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        End Try

                    End With

                Next

            Else
                objAssignInfo = Nothing
            End If

            DeviceAssignment(anAssignType) = objAssignInfo

        Next

    End Sub

End Class