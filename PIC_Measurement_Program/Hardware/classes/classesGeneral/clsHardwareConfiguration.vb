'The Class that handles Configurations
'Objects are translated into XML-Strings, which are stored in "_default.config" (a File in the Directory where the Exe is located)
'All new Hardware-Devices must implement a Function "getMapList(ByVal aNewDevice As clsNewDeviceType) As structConfigMapping"!
Public Class clsHardwareConfiguration

    Private mConfigFile As System.Configuration.Configuration

    ''' <summary>
    ''' Findet seine Anwendung beim Mappen von Properties von HardwareKlassen
    ''' </summary>
    Public Structure structConfigMapping

        Public MapList As List(Of structSubConfMapping)

        Public Class structSubConfMapping

            Public strElementName As String
            Public strAttributeName As String
            Public strPropertyName As String

            ''' <summary>
            ''' Initializes a new instance of the <see
            ''' cref="T:Hardware.clsConfig.structConfigMapping.structSubConfMapping">structSubConfMapping</see>
            ''' class.
            ''' </summary>
            ''' <param name="anElementName">Gibt Kategorie an z.B. &quot;General&quot;</param>
            ''' <param name="anAttributeName">Gibt Namen an z.B.
            ''' &quot;Wavelength&quot;,&quot;GPIB Address&quot;</param>
            ''' <param name="aPropertyName">Name der Property, wie er im Code steht</param>
            Public Sub New(ByVal anElementName As String, ByVal anAttributeName As String, ByVal aPropertyName As String)
                With Me
                    .strAttributeName = anAttributeName
                    .strElementName = anElementName
                    .strPropertyName = aPropertyName
                End With
            End Sub

            ''' <summary>
            ''' Bestimmt, ob das angegebene <see cref="T:System.Object">T:System.Object</see>
            ''' und das aktuelle <see cref="T:System.Object">T:System.Object</see> gleich sind.
            ''' </summary>
            ''' <param name="obj">Das <see cref="T:System.Object">T:System.Object</see>, das mit
            ''' dem aktuellen <see cref="T:System.Object">T:System.Object</see> verglichen
            ''' werden soll.</param>
            ''' <returns>
            ''' true, wenn das angegebene <see cref="T:System.Object">T:System.Object</see>
            ''' gleich dem aktuellen <see cref="T:System.Object">T:System.Object</see> ist,
            ''' andernfalls false.
            ''' </returns>
            ''' <exception cref="System.NullReferenceException">Der <paramref
            ''' name="obj"/>-Parameter ist null.</exception>
            ''' <filterpriority>2</filterpriority>
            Public Overrides Function Equals(ByVal obj As Object) As Boolean
                If TypeOf (obj) Is structSubConfMapping Then
                    With CType(obj, structSubConfMapping)
                        If strAttributeName = .strAttributeName And strElementName = .strElementName And strPropertyName = .strPropertyName Then
                            Return True
                        Else
                            Return False
                        End If
                    End With
                End If

                Return False
            End Function

        End Class

        ''' <summary>
        ''' Erzeugt eine <see
        ''' cref="T:Hardware.clsConfig.structConfigMapping.structSubConfMapping">structConfigMapping.structSubConfMapping</see>
        ''' anhand eines <see cref="T:Hardware.attrDeviceMapping">attrDeviceMapping</see>.
        ''' <para>Dieser Weg wurde gewählt, um mit dem alten/ursprünglichen Code verträglich
        ''' zu bleiben.</para>
        ''' </summary>
        ''' <param name="anAttributeDeviceMapping"></param>
        Public Shared Function FromAttributeDeviceMapping(ByVal anAttributeDeviceMapping As attrDeviceMapping) As structSubConfMapping
            Return New structSubConfMapping(anAttributeDeviceMapping.TreeKey, anAttributeDeviceMapping.NodeName, anAttributeDeviceMapping.Tag.ToString)
        End Function

        ''' <summary>
        ''' Gibt an, ob diese Instanz und ein angegebenes Objekt gleich sind.
        ''' </summary>
        ''' <param name="obj">Ein weiteres Objekt für den Vergleich.</param>
        ''' <returns>
        ''' true, wenn <paramref name="obj"/> und diese Instanz denselben Typ aufweisen und
        ''' denselben Wert darstellen, andernfalls false.
        ''' </returns>
        ''' <filterpriority>2</filterpriority>
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Dim bWasEqual As Boolean

            If TypeOf (obj) Is structConfigMapping Then
                With CType(obj, structConfigMapping)
                    If Me.MapList.Count <> .MapList.Count Then
                        Return False
                    End If


                    For iRun = 0 To MapList.Count - 1
                        bWasEqual = False
                        For iSndRun = 0 To MapList.Count - 1
                            If Me.MapList(iRun).Equals(.MapList(iSndRun)) Then
                                bWasEqual = True
                                Exit For
                            End If
                        Next

                        If bWasEqual = False Then
                            Return False
                        End If
                    Next

                    Return True
                End With
            End If

            Return False
        End Function

    End Structure

    Public Sub New()

        Dim strDirName As String = "C:\PIC_MeasuremenTool_Config"
        Dim strConfName As String = "_defaultsystem.config"

        Try
            If Not Directory.Exists(strDirName) Then

                'Create Directory and grant Access for Everybody!
                Directory.CreateDirectory(strDirName)

                Dim accDirSecurity As Security.AccessControl.DirectorySecurity
                Dim accFileSecurity As Security.AccessControl.FileSecurity
                Dim idEveryone As Security.Principal.SecurityIdentifier

                idEveryone = New Security.Principal.SecurityIdentifier(Security.Principal.WellKnownSidType.WorldSid, Nothing)

                accDirSecurity = Directory.GetAccessControl(strDirName)
                accDirSecurity.AddAccessRule(New Security.AccessControl.FileSystemAccessRule(idEveryone, _
                                                                                         Security.AccessControl.FileSystemRights.FullControl, _
                                                                                          Security.AccessControl.InheritanceFlags.ObjectInherit Or Security.AccessControl.InheritanceFlags.ContainerInherit, _
                                                                                          Security.AccessControl.PropagationFlags.None, _
                                                                                         Security.AccessControl.AccessControlType.Allow))

                Directory.SetAccessControl(strDirName, accDirSecurity)

                'use own Config-File as initial File:
                Try
                    File.Copy(strConfName, strDirName & "\" & strConfName)

                    accFileSecurity = File.GetAccessControl(strDirName & "\" & strConfName)
                    accFileSecurity.AddAccessRule(New Security.AccessControl.FileSystemAccessRule(idEveryone, _
                                                                                         Security.AccessControl.FileSystemRights.FullControl, _
                                                                                          Security.AccessControl.InheritanceFlags.ObjectInherit Or Security.AccessControl.InheritanceFlags.ContainerInherit, _
                                                                                          Security.AccessControl.PropagationFlags.None, _
                                                                                         Security.AccessControl.AccessControlType.Allow))
                    File.SetAccessControl(strDirName & "\" & strConfName, accFileSecurity)

                Catch ex As Exception
                    'no problem ...
                End Try

            End If

            'Get the configuration file.
            Dim myConfigFile As New System.Configuration.ExeConfigurationFileMap()
            myConfigFile.ExeConfigFilename = strDirName & "\" & strConfName

            'Open Configuration File
            mConfigFile = ConfigurationManager.OpenMappedExeConfiguration(myConfigFile, ConfigurationUserLevel.None)

        Catch ex As Exception
            MessageBox.Show("Error accessing Configuration File (""" & strDirName & "\" & strConfName & """).", "Configuration File", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Dim cEx As New ExeptionHandler("Configuration File not accessable" & vbLf)
            Try
                Throw cEx
            Catch anEx As ExeptionHandler
                anEx.Log()
            End Try
            'Endstation
            Throw cEx

        End Try

        If mConfigFile.AppSettings.ElementInformation.Source IsNot Nothing Then

            If (File.GetAttributes(mConfigFile.AppSettings.ElementInformation.Source) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                MessageBox.Show("Configuration File (""_default.config"") is write-protected.", "Configuration File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        End If

    End Sub

    Protected Overrides Sub Finalize()

        MyBase.Finalize()

        ' Save the configuration file.
        Try
            mConfigFile.Save()
        Catch ex As Exception
            'too late to log
        End Try

    End Sub

    'reads the XML configuration as string
    Public Function ReadStringFromConfig(ByVal ConfigKey As String, Optional ByVal ExceptionIfNotFound As Boolean = True) As String

        If mConfigFile.AppSettings.Settings.AllKeys.Contains(ConfigKey) Then
            ReadStringFromConfig = mConfigFile.AppSettings.Settings.Item(ConfigKey).Value
        Else
            'Configuration not found ...
            ReadStringFromConfig = ""

            'Log
            Dim cEx As New ExeptionHandler("ConfigurationKey not found")
            Try
                Throw cEx
            Catch anEx As ExeptionHandler
                anEx.Log()
            End Try

            If ExceptionIfNotFound Then Throw cEx

        End If

    End Function


    'wandelt XML-Konfiguration in String um
    Public Sub Hardware_ReadFromConfig(ByRef aHardware As intfcMeasurementInstrument, ByVal strDeviceName As String)

        If aHardware Is Nothing Then Exit Sub

        'strConfig ->  XML-Konfiguration des übergebenen Gerätes als String
        Dim strConfig As String = ReadStringFromConfig(strDeviceName, False) 'ReadStringFromConfig(aHardware.Name.Replace(" ", "_"), False)

        If Not String.IsNullOrEmpty(strConfig) Then

            'Methode unterscheidet zwischen intfcMotorController und anderen Interfaces
            'bei nicht Motor, Aufruf von "Hardware_FromStringByPropertyMap(strConfig, aHardware)"
            Hardware_FromString(strConfig, aHardware)

        Else

            'Config not found - set to default Settings:
            aHardware.SetToDefault()

            Dim myXML As String = Hardware_ToString(aHardware, strDeviceName)

            If MessageBox.Show("Configuration for """ & aHardware.Name & """ not found, default Settings are used." _
                            & vbCrLf & vbCrLf & "Please verify the Settings:" & vbCrLf & vbCrLf & _
                            myXML, _
                            "Configuration not found.", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.OK Then

                'write the Setting               
                WriteStringToConfig(strDeviceName, _
                                    Hardware_ToString(aHardware, strDeviceName))

            Else
                'Log
                Dim cEx As New ExeptionHandler("Configuration not found and default Settings not ok.")
                Try
                    Throw cEx
                Catch anEx As ExeptionHandler
                    anEx.Log()
                End Try
                'Endstation
                Throw cEx

            End If

        End If

    End Sub

    Private Sub Hardware_FromString(ByVal strConfig As String, ByRef aHardware As intfcMeasurementInstrument)

        Hardware_FromStringByPropertyMap(strConfig, aHardware)

    End Sub


    Private Function getMapList(ByVal aKeithley2700 As clsKeithley2700) As structConfigMapping

        getMapList.MapList = New List(Of structConfigMapping.structSubConfMapping)

        With getMapList.MapList

            .Add(New structConfigMapping.structSubConfMapping("General", "CardNr", "CardNr"))
            .Add(New structConfigMapping.structSubConfMapping("General", "Beeper", "Beeper"))
            .Add(New structConfigMapping.structSubConfMapping("General", "GPIBAddressNr", "GPIBAddressNr"))

            .Add(New structConfigMapping.structSubConfMapping("BeforeMeasurement", "Init", "BeforeMeas_Init"))

            .Add(New structConfigMapping.structSubConfMapping("AfterMeasurement", "Init", "AfterMeas_Init"))
            .Add(New structConfigMapping.structSubConfMapping("AfterMeasurement", "GoLocal", "AfterMeas_Local"))

        End With

    End Function

    Public Function Hardware_ToString(ByRef aHardware As intfcMeasurementInstrument, ByVal strConfigType As String) As String

        Return Hardware_ToStringByPropertyMap(aHardware, strConfigType)

    End Function

    Public Sub RemoveConfigSetting(ByVal aKey As String)

        mConfigFile.AppSettings.Settings.Remove(aKey)

        Try
            mConfigFile.Save()
        Catch ex As Exception
            MessageBox.Show("Error writing Configuration File (""_default.config"").", "Configuration File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ExeptionHandler.LogException(ex)
        End Try

    End Sub

    Public Sub WriteStringToConfig(ByVal aKey As String, ByVal aValue As String)

        mConfigFile.AppSettings.Settings.Remove(aKey)
        mConfigFile.AppSettings.Settings.Add(aKey, aValue)

        Try
            mConfigFile.Save()
        Catch ex As Exception
            MessageBox.Show("Error writing Configuration File (""_default.config"").", "Configuration File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ExeptionHandler.LogException(ex)
        End Try

    End Sub

    'erhält xml-konfiguration des übergebenen gerätes als String und das gerät selber
    Private Sub Hardware_FromStringByPropertyMap(ByVal strConfig As String, _
                                                    ByRef aHardware As intfcMeasurementInstrument)

        'anXMLElement besitzt dict(String,String) und elementName als String
        'möglicherweise unnütz angelegt
        Dim anXMLElement As New clsConfigXMLConverter.clsXMLElement
        Dim aMapDict As Dictionary(Of String, Dictionary(Of String, System.Reflection.PropertyInfo))

        'Hier werden die statischen Werte der einzelen Deviceklassen anhand der statischen Maplisten zusammengeführt
        '-> unbedingt anders machen
        aMapDict = getPropertyDict(aHardware)

        With New clsConfigXMLConverter

            'XMLEquivalent ist auch String
            .XMLEquivalent = strConfig

            For Each aMainConfig As KeyValuePair(Of String, Dictionary(Of String, System.Reflection.PropertyInfo)) In aMapDict

                'gibt clsXMLElement des Keys zurück
                With .XMLElement(aMainConfig.Key)
                    'aMainConfig.Value -> Properties des Devices, fest eingecoded
                    'aConfigMapping -> key = mapdict string, value = property des device
                    For Each aConfigMapping As KeyValuePair(Of String, System.Reflection.PropertyInfo) In aMainConfig.Value
                        Try
                            If aConfigMapping.Value.PropertyType.IsEnum Then

                                'device wird in _default.config gespeicherter wert bei entsprechendem property zugewiesen
                                aConfigMapping.Value.SetValue(aHardware, .XMLAttribute(aConfigMapping.Key, _
                                                                                            aConfigMapping.Value.PropertyType), Nothing)

                            Else

                                'device wird in _default.config gespeicherter wert bei entsprechendem property zugewiesen
                                aConfigMapping.Value.SetValue(aHardware, ValueTypeConv(aConfigMapping.Value.PropertyType, _
                                                                                            .XMLAttribute(aConfigMapping.Key)), Nothing)

                            End If
                        Catch e As Exception
                            modLog.logWarning(e)
                        End Try
                    Next

                End With

            Next aMainConfig

        End With

    End Sub

    Private Function getPropertyDict(ByVal aHardware As intfcMeasurementInstrument) As Dictionary(Of String, Dictionary(Of String, System.Reflection.PropertyInfo))

        getPropertyDict = New Dictionary(Of String, Dictionary(Of String, System.Reflection.PropertyInfo))


        'Entfernt GLASS
        '' ''compare both mechanisms
        ' ''If (Not getMapList(aHardware).Equals(GetMapListByAttributes(aHardware))) Then
        ' ''    Dim CE As New CustomException("getMapList and getMapListByAttributes are not equal for " + aHardware.Name)
        ' ''End If

        'getMapList(aHardware).Maplist -> structlist
        'diese Zeile auskommentieren, falls Fehler auftreten
        For Each aMapping As structConfigMapping.structSubConfMapping In GetMapListByAttributes(aHardware).MapList
            ' nächste Zeile "einkommentieren", um auf alte Methode zu wechseln.
            '        For Each aMapping As structConfigMapping.structSubConfMapping In getMapList(aHardware).MapList

            With aMapping

                If Not getPropertyDict.ContainsKey(.strElementName) Then getPropertyDict.Add(.strElementName, New Dictionary(Of String, System.Reflection.PropertyInfo))

                'hier wird anhand von strAttributeName der feste Property des Gerätes zugewiesen
                getPropertyDict.Item(.strElementName).Add(.strAttributeName, aHardware.GetType.GetProperty(.strPropertyName))

            End With

        Next

    End Function

    Public Function returnPropertyDict(ByVal aHardware As intfcMeasurementInstrument) As Dictionary(Of String, Dictionary(Of String, System.Reflection.PropertyInfo))
        Return getPropertyDict(aHardware)
    End Function



    'includes all devices in *.config files
    Public Function getDeviceConfigAsXMLConverters() As Dictionary(Of String, clsConfigXMLConverter)

        Dim anXMLConv As clsConfigXMLConverter

        getDeviceConfigAsXMLConverters = New Dictionary(Of String, clsConfigXMLConverter)

        'mConfigFile: C:\ModuNetConfig\_default.config
        'runs through every String from mConfigFile
        For Each aConfigKey As String In mConfigFile.AppSettings.Settings.AllKeys

            anXMLConv = New clsConfigXMLConverter(ReadStringFromConfig(aConfigKey))

            Try
                If anXMLConv.containsXMLElement("DeviceType") Then getDeviceConfigAsXMLConverters.Add(anXMLConv.SectionName, anXMLConv)
            Catch ex As Exception

            End Try

        Next

    End Function

    Public Function getDevices() As String()
        getDevices = mConfigFile.AppSettings.Settings.AllKeys
    End Function

    Public Sub setConfigByXMLConverter(ByVal anXMLConv As clsConfigXMLConverter)

        WriteStringToConfig(anXMLConv.SectionName, anXMLConv.XMLEquivalent)

    End Sub




    ''' <summary>
    ''' Liefert über Reflection der Attribute der Properties von aHardware alle
    ''' Attribute, die in  <see
    ''' cref="T:Hardware.frmConfigureDevices">frmConfigureDevices</see> zu aHardware
    ''' gemappt werden sollen.
    ''' </summary>
    ''' <param name="aHardware"></param>
    Public Function GetMapListByAttributes(ByVal aHardware As intfcMeasurementInstrument) As structConfigMapping
        GetMapListByAttributes.MapList = New List(Of structConfigMapping.structSubConfMapping)

        With GetMapListByAttributes.MapList
            For Each runAttrMapping As attrDeviceMapping In attrDeviceMapping.GetAllProperties(aHardware.GetType)
                .Add(structConfigMapping.FromAttributeDeviceMapping(runAttrMapping))
            Next
        End With
    End Function


    Public Function getMapList(ByVal aHardware As intfcMeasurementInstrument) As structConfigMapping

        If aHardware.GetType Is GetType(clsKeithley2700) Then

            Return getMapList(CType(aHardware, clsKeithley2700))

        ElseIf aHardware.GetType.BaseType Is GetType(clsKeithley24xx) Then

            Return getMapList(CType(aHardware, clsKeithley24xx))

        ElseIf aHardware.GetType Is GetType(clsKeithley2602) Then

            Return getMapList(CType(aHardware, clsKeithley2602))

        ElseIf aHardware.GetType Is GetType(clsKeithley2510) Then
            Return getMapList(CType(aHardware, clsKeithley2510))

        ElseIf aHardware.GetType Is GetType(clsPro8TED8040) Then

            Return getMapList(CType(aHardware, clsPro8TED8040))

        Else
            Dim cEx As New ExeptionHandler("Unknown Hardware.")
            Throw cEx
        End If

    End Function

    'XML-String der Configuration
    'wird anhand der statischen Properties erzeugt
    Private Function Hardware_ToStringByPropertyMap(ByVal aHardware As intfcMeasurementInstrument, ByVal strConfigType As String) As String

        Dim anXMLElement As New clsConfigXMLConverter.clsXMLElement
        Dim aMapDict As Dictionary(Of String, Dictionary(Of String, System.Reflection.PropertyInfo))

        aMapDict = getPropertyDict(aHardware)

        With New clsConfigXMLConverter

            .SectionName = strConfigType 'aHardware.Name.Replace(" ", "_")

            anXMLElement.XMLAttribute("DeviceType") = aHardware.GetType.ToString
            .XMLElement("DeviceType") = anXMLElement

            For Each aMainConfig As KeyValuePair(Of String, Dictionary(Of String, System.Reflection.PropertyInfo)) In aMapDict

                anXMLElement = New clsConfigXMLConverter.clsXMLElement

                For Each aConfigMapping As KeyValuePair(Of String, System.Reflection.PropertyInfo) In aMainConfig.Value

                    If aConfigMapping.Value.PropertyType.IsEnum Then

                        anXMLElement.XMLAttribute(aConfigMapping.Key, aConfigMapping.Value.PropertyType) = CType(aConfigMapping.Value.GetValue(aHardware, Nothing), System.Enum)

                    Else

                        anXMLElement.XMLAttribute(aConfigMapping.Key) = CStr(aConfigMapping.Value.GetValue(aHardware, Nothing))

                    End If

                Next

                .XMLElement(aMainConfig.Key) = anXMLElement

            Next aMainConfig

            Return .XMLEquivalent

        End With

    End Function


    Private Function getMapList(ByVal aKeithley24xx As clsKeithley24xx) As structConfigMapping

        getMapList.MapList = New List(Of structConfigMapping.structSubConfMapping)

        With getMapList.MapList

            .Add(New structConfigMapping.structSubConfMapping("General", "PanelType", "PanelType"))
            .Add(New structConfigMapping.structSubConfMapping("General", "Beeper", "Beeper"))
            .Add(New structConfigMapping.structSubConfMapping("General", "GPIBAddressNr", "GPIBAddressNr"))

            .Add(New structConfigMapping.structSubConfMapping("Sense", "Type", "SenseType"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "AutoRange", "SenseAutoRange"))
            ' -- in clsDeviceAssignment: --
            '.Add(New structConfigMapping.structSubConfMapping("Sense", "RangeMaximum", "SenseMax"))
            '.Add(New structConfigMapping.structSubConfMapping("Sense", "ResistanceMode", "SenseResistanceMode"))
            ' -----------------------------
            .Add(New structConfigMapping.structSubConfMapping("Sense", "AutoZero", "SenseAutoZero"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseSpeed", "SenseSpeed"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "FourWireRemoteSens", "RemoteSensing"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "GuardType", "GuardType"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "AverageEnabled", "SenseAVGEnabled"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "AverageFilterType", "SenseAVGFilterType"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "AverageCount", "SenseAVGCount"))

            .Add(New structConfigMapping.structSubConfMapping("Source", "Type", "SourceType"))
            ' -- in clsDeviceAssignment: --
            '.Add(New structConfigMapping.structSubConfMapping("Source", "Mode", "SourceMode"))

            .Add(New structConfigMapping.structSubConfMapping("Source", "AutoRange", "SourceAutoRange"))
            '.Add(New structConfigMapping.structSubConfMapping("Source", "SweepStartLevel", "SourceSweepStartLevel"))
            '.Add(New structConfigMapping.structSubConfMapping("Source", "SweepStopLevel", "SourceSweepStopLevel"))
            '.Add(New structConfigMapping.structSubConfMapping("Source", "SweepStepSize", "SourceSweepStepSize"))
            ' -- in clsDeviceAssignment: --
            '.Add(New structConfigMapping.structSubConfMapping("Source", "ProtectionLevelCurrent", "SourceCurrentProtectionLevel"))
            '.Add(New structConfigMapping.structSubConfMapping("Source", "ProtectionLevelVoltage", "SourceVoltageProtectionLevel"))

            .Add(New structConfigMapping.structSubConfMapping("BeforeMeasurement", "Init", "BeforeMeas_Init"))

            .Add(New structConfigMapping.structSubConfMapping("AfterMeasurement", "Init", "AfterMeas_Init"))
            .Add(New structConfigMapping.structSubConfMapping("AfterMeasurement", "GoLocal", "AfterMeas_Local"))
            .Add(New structConfigMapping.structSubConfMapping("AfterMeasurement", "OutputOn", "AfterMeas_On"))
            .Add(New structConfigMapping.structSubConfMapping("AfterMeasurement", "ToZero", "AfterMeas_Zero"))

        End With

    End Function

    Private Function getMapList(ByVal aKeithley26xx As clsKeithley26xx) As structConfigMapping

        getMapList.MapList = New List(Of structConfigMapping.structSubConfMapping)

        With getMapList.MapList

            .Add(New structConfigMapping.structSubConfMapping("General", "Beeper", "Beeper"))
            .Add(New structConfigMapping.structSubConfMapping("General", "GPIBAddressNr", "GPIBAddressNr"))

            .Add(New structConfigMapping.structSubConfMapping("Sense", "Type", "SenseType"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "AutoRange", "SenseAutoRange"))

            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseSpeed", "SenseSpeed"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "FourWireRemoteSens", "RemoteSensing"))

            .Add(New structConfigMapping.structSubConfMapping("Sense", "AverageEnabled", "SenseAVGEnabled"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "AverageFilterType", "SenseAVGFilterType"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "AverageCount", "SenseAVGCount"))

            .Add(New structConfigMapping.structSubConfMapping("Source", "Type", "SourceType"))

            .Add(New structConfigMapping.structSubConfMapping("Source", "AutoRange", "SourceAutoRange"))

            .Add(New structConfigMapping.structSubConfMapping("BeforeMeasurement", "Init", "BeforeMeas_Init"))

        End With

    End Function

    Private Function getMapList(ByVal aKeithley2510 As clsKeithley2510) As structConfigMapping
        getMapList.MapList = New List(Of structConfigMapping.structSubConfMapping)
        With getMapList.MapList
            .Add(New structConfigMapping.structSubConfMapping("General", "GPIBAddressNr", "GPIBAddressNr"))
            .Add(New structConfigMapping.structSubConfMapping("General", "BeforeMeas_Init", "BeforeMeas_Init"))
            .Add(New structConfigMapping.structSubConfMapping("General", "AfterMeas_Init", "AfterMeas_Init"))
            .Add(New structConfigMapping.structSubConfMapping("General", "AfterMeas_On", "AfterMeas_On"))
            .Add(New structConfigMapping.structSubConfMapping("General", "AfterMeas_Local", "AfterMeas_Local"))

            .Add(New structConfigMapping.structSubConfMapping("Display", "DisplayWindowOneTextData", "DisplayWindowOneTextData"))
            .Add(New structConfigMapping.structSubConfMapping("Display", "DisplayWindowTwoTextData", "DisplayWindowTwoTextData"))
            .Add(New structConfigMapping.structSubConfMapping("Display", "DisplayWindowOneTextState", "DisplayWindowOneTextState"))
            .Add(New structConfigMapping.structSubConfMapping("Display", "DisplayWindowTwoTextState", "DisplayWindowTwoTextState"))

            .Add(New structConfigMapping.structSubConfMapping("Format", "FormatData", "FormatData"))
            .Add(New structConfigMapping.structSubConfMapping("Format", "FormatElements", "FormatElements"))
            .Add(New structConfigMapping.structSubConfMapping("Format", "FormatSourceTwo", "FormatSourceTwo"))
            .Add(New structConfigMapping.structSubConfMapping("Format", "FormatByteOrder", "FormatByteOrder"))
            .Add(New structConfigMapping.structSubConfMapping("Format", "FormatStatusRegister", "FormatStatusRegister"))

            .Add(New structConfigMapping.structSubConfMapping("Output", "OutputEnable", "OutputEnable"))

            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseCurrentDCProtectionLimit", "SenseCurrentDCProtectionLimit"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseResistanceCurrent", "SenseResistanceCurrent"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseResistanceCurrentAuto", "SenseResistanceCurrentAuto"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseResistanceTransducer", "SenseResistanceTransducer"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseResistanceRTDRange", "SenseResistanceRTDRange"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseResistanceThermistorRange", "SenseResistanceThermistorRange"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureCurrent", "SenseTemperatureCurrent"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureCurrentAuto", "SenseTemperatureCurrentAuto"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureTransducer", "SenseTemperatureTransducer"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureRTDType", "SenseTemperatureRTDType"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureRTDRange", "SenseTemperatureRTDRange"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureRTDAlpha", "SenseTemperatureRTDAlpha"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureRTDBeta", "SenseTemperatureRTDBeta"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureRTDDelta", "SenseTemperatureRTDDelta"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureThermistorRange", "SenseTemperatureThermistorRange"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureThermistorA", "SenseTemperatureThermistorA"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureThermistorB", "SenseTemperatureThermistorB"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureThermistorC", "SenseTemperatureThermistorC"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureVSSGain", "SenseTemperatureVSSGain"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureISSGain", "SenseTemperatureISSGain"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureVSSOffset", "SenseTemperatureVSSOffset"))
            .Add(New structConfigMapping.structSubConfMapping("Sense", "SenseTemperatureISSOffset", "SenseTemperatureISSOffset"))

            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceFunction", "SourceFunction"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceCurrentAmplitude", "SourceCurrentAmplitude"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceCurrentProportional", "SourceCurrentProportional"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceCurrentDervative", "SourceCurrentDervative"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceCurrentIntegral", "SourceCurrentIntegral"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceResistanceAmplitude", "SourceResistanceAmplitude"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceResistanceProportional", "SourceResistanceProportional"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceResistanceDervative", "SourceResistanceDervative"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceResistanceIntegral", "SourceResistanceIntegral"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceResistanceProtectionHighLimit", "SourceResistanceProtectionHighLimit"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceResistanceProtectionLowLimit", "SourceResistanceProtectionLowLimit"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceResistanceProtectionState", "SourceResistanceProtectionState"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceTemperatureProportional", "SourceTemperatureProportional"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceTemperatureDervative", "SourceTemperatureDervative"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceTemperatureIntegral", "SourceTemperatureIntegral"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceTemperatureProtectionHighLimit", "SourceTemperatureProtectionHighLimit"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceTemperatureProtectionLowLimit", "SourceTemperatureProtectionLowLimit"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceTemperatureProtectionState", "SourceTemperatureProtectionState"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceVoltageAmplitude", "SourceVoltageAmplitude"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceVoltageProportional", "SourceVoltageProportional"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceVoltageDervative", "SourceVoltageDervative"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceVoltageIntegral", "SourceVoltageIntegral"))
            .Add(New structConfigMapping.structSubConfMapping("SourceOne", "SourceVoltageProtectionLimit", "SourceVoltageProtectionLimit"))

            .Add(New structConfigMapping.structSubConfMapping("SourceTwo", "SourceTwoTTL", "SourceTwoTTL"))

            .Add(New structConfigMapping.structSubConfMapping("Status", "StatusMeasurementEnable", "StatusMeasurementEnable"))
            .Add(New structConfigMapping.structSubConfMapping("Status", "StatusQuestionableEnable", "StatusQuestionableEnable"))
            .Add(New structConfigMapping.structSubConfMapping("Status", "StatusOperationEnable", "StatusOperationEnable"))
            .Add(New structConfigMapping.structSubConfMapping("Status", "StatusQueueEnable", "StatusQueueEnable"))
            .Add(New structConfigMapping.structSubConfMapping("Status", "StatusQueueDisable", "StatusQueueDisable"))

            .Add(New structConfigMapping.structSubConfMapping("System", "SystemPOSetup", "SystemPOSetup"))
            .Add(New structConfigMapping.structSubConfMapping("System", "SystemLinefrequency", "SystemLinefrequency"))
            .Add(New structConfigMapping.structSubConfMapping("System", "SystemKey", "SystemKey"))
            .Add(New structConfigMapping.structSubConfMapping("System", "SystemRSense", "SystemRSense"))
            .Add(New structConfigMapping.structSubConfMapping("System", "SystemGroundConnect", "SystemGroundConnect"))

            .Add(New structConfigMapping.structSubConfMapping("Unit", "UnitTemperature", "UnitTemperature"))

            .Add(New structConfigMapping.structSubConfMapping("Function", "K1_PD", "K1_PD"))
            .Add(New structConfigMapping.structSubConfMapping("Function", "K2_PD", "K2_PD"))
            .Add(New structConfigMapping.structSubConfMapping("Function", "K3_PD", "K3_PD"))
            .Add(New structConfigMapping.structSubConfMapping("Function", "K1_PK2", "K1_PK2"))
            .Add(New structConfigMapping.structSubConfMapping("Function", "K2_PK2", "K2_PK2"))
            .Add(New structConfigMapping.structSubConfMapping("Function", "K3_PK2", "K3_PK2"))

        End With
    End Function

    Private Function getMapList(ByVal aTED8040 As clsPro8TED8040) As structConfigMapping

        getMapList.MapList = New List(Of structConfigMapping.structSubConfMapping)

        With getMapList.MapList

            .Add(New structConfigMapping.structSubConfMapping("General", "GPIBAddressNr", "GPIBAddressNr"))
            .Add(New structConfigMapping.structSubConfMapping("General", "SlotNumber", "SlotNumber"))

        End With

    End Function

End Class
