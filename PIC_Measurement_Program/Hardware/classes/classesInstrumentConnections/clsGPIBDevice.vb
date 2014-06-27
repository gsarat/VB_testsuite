'Super-Class for all GPIB-Devices
Public MustInherit Class clsGPIBDevice
    Implements intfcMeasurementInstrument

    Protected WithEvents mVISASession As NationalInstruments.VisaNS.GpibSession

    Private mGPIBNr As enumGPIBAddressNr
    Private mStrGPIBIdn As String

    Private m_sGPIBIdnQuery As String = "*IDN?"

    Public MustOverride Sub SetToDefault() Implements intfcMeasurementInstrument.SetToDefault
    Public MustOverride ReadOnly Property Name() As String Implements intfcMeasurementInstrument.Name
    Public MustOverride Sub Initialize() Implements intfcMeasurementInstrument.Initialize

    Enum enumGPIBAddressNr
        'as enumeration, since we want to be able to use AUTO-Value and Numbers are limited to 1..30
        AUTO = -1
        Nr1 = 1
        Nr2 = 2
        Nr3 = 3
        Nr4 = 4
        Nr5 = 5
        Nr6 = 6
        Nr7 = 7
        Nr8 = 8
        Nr9 = 9
        Nr10 = 10
        Nr11 = 11
        Nr12 = 12
        Nr13 = 13
        Nr14 = 14
        Nr15 = 15
        Nr16 = 16
        Nr17 = 17
        Nr18 = 18
        Nr19 = 19
        Nr20 = 20
        Nr21 = 21
        Nr22 = 22
        Nr23 = 23
        Nr24 = 24
        Nr25 = 25
        Nr26 = 26
        Nr27 = 27
        Nr28 = 28
        Nr29 = 29
        Nr30 = 30
    End Enum

    Protected Property GPIBIdnString() As String
        Get
            Return mStrGPIBIdn
        End Get
        Set(ByVal value As String)
            mStrGPIBIdn = value
        End Set
    End Property

    <System.Xml.Serialization.XmlIgnore()> _
 <attrDeviceMapping("General", "GPIBAddressNr")> _
    Public Property GPIBAddressNr() As enumGPIBAddressNr
        Get
            Return mGPIBNr
        End Get
        Set(ByVal value As enumGPIBAddressNr)
            mGPIBNr = value
        End Set
    End Property

    Protected Overridable ReadOnly Property getGPIBIdnQuery() As String
        Get
            Return m_sGPIBIdnQuery 'pre init with "*IDN?"
        End Get
    End Property

    Public WriteOnly Property setGPIBIdnQuery() As String
        Set(ByVal value As String)
            m_sGPIBIdnQuery = value
        End Set
    End Property

    Private Sub GoLocal(ByRef aSession As NationalInstruments.VisaNS.GpibSession)

        aSession.ControlRen(RenMode.AddressAndGtl)

    End Sub

    Public Sub GoLocal()

        If mVISASession IsNot Nothing Then
            GoLocal(mVISASession)
        End If

    End Sub

    Public Overridable Function IsAvailable() As Boolean Implements intfcMeasurementInstrument.InstrumentIsAvailable

        'try to find Controller:
        Dim tempSession = FindController()

        If tempSession Is Nothing Then
            Return False
        Else
            Me.GoLocal(tempSession)
            tempSession.Dispose()
            IsAvailable = True
        End If

    End Function

    Public Overridable Function OpenSession() As Boolean Implements intfcMeasurementInstrument.OpenSession

        'try to find Controller:
        mVISASession = FindController()

        Return (mVISASession IsNot Nothing)

    End Function

    Public Overridable Sub CloseSession() Implements intfcMeasurementInstrument.CloseSession

        If mVISASession IsNot Nothing Then
            Me.GoLocal()
            mVISASession.Dispose()
            mVISASession = Nothing
        End If

    End Sub

    Protected Function QuerySecure(ByRef aSession As GpibSession, ByVal strQuery As String) As String

        Threading.Thread.Sleep(20)
        aSession.Write(strQuery)

        'evaluate StatusByte
        Threading.Thread.Sleep(20)
        If (CShort(aSession.ReadStatusByte) And StatusByteFlags.MessageAvailable) = StatusByteFlags.MessageAvailable Then Return aSession.ReadString
        Threading.Thread.Sleep(20)
        If (CShort(aSession.ReadStatusByte) And StatusByteFlags.MessageAvailable) = StatusByteFlags.MessageAvailable Then Return aSession.ReadString
        Threading.Thread.Sleep(20)
        If (CShort(aSession.ReadStatusByte) And StatusByteFlags.MessageAvailable) = StatusByteFlags.MessageAvailable Then Return aSession.ReadString

        Dim waitStart As DateTime = DateTime.Now

        Do

            If (CShort(aSession.ReadStatusByte) And StatusByteFlags.MessageAvailable) = StatusByteFlags.MessageAvailable Then Exit Do
            Threading.Thread.Sleep(20)

        Loop Until (DateTime.Now - waitStart).Ticks > CLng(1 * 10000000)

        Return aSession.ReadString

    End Function

    Private Function FindController() As NationalInstruments.VisaNS.GpibSession

        'possible Filters:
        '"?*", "GPIB?*", "GPIB?*INSTR", "GPIB?*INTFC", "GPIB?*SERVANT", "GPIB-VXI?*", "GPIB-VXI?*INSTR", "GPIB-VXI?*MEMACC", "GPIB-VXI?*BACKPLANE", _
        '"PXI?*INSTR", "ASRL?*INSTR", "VXI?*", "VXI?*INSTR", "VXI?*MEMACC", "VXI?*BACKPLANE", "VXI?*SERVANT", "USB?*", "FIREWIRE?*"

        Dim mbSession As NationalInstruments.VisaNS.GpibSession = Nothing
        Dim strSearch As String

        If Me.GPIBAddressNr = enumGPIBAddressNr.AUTO Then
            strSearch = "GPIB?*INSTR"
        Else
            strSearch = "GPIB0::" & CInt(Me.GPIBAddressNr) & "::INSTR"
        End If

        Try

            For i As Integer = 0 To 5

                Dim wasSuccess As Boolean

                For Each aVISAResource As String In ResourceManager.GetLocalManager().FindResources(strSearch)

                    wasSuccess = False

                    System.Threading.Thread.Sleep(100)

                    Try
                        mbSession = CType(ResourceManager.GetLocalManager().Open(aVISAResource), GpibSession)
                        System.Threading.Thread.Sleep(200)

                        Dim strSessionResponse As String = mbSession.Query(Me.getGPIBIdnQuery & vbLf).Trim(CChar(vbLf))

                        With strSessionResponse
                            'when mbSession.Query was successful, then mbSession.Query is not nothing -> return true
                            'Query not complete
                            If .Length = 0 Then
                                'wait
                                System.Threading.Thread.Sleep(100)
                                'perform another query
                                'GPIBIdnQuery: "*IDN?"
                                'when QuerySecure was successful, then QuerySecure is not nothing -> return true
                                If Not Me.QuerySecure(mbSession, Me.getGPIBIdnQuery & vbLf) Is Nothing Then
                                    wasSuccess = True
                                    Exit For
                                End If

                            ElseIf Not strSessionResponse Is Nothing Then
                                wasSuccess = True
                                Exit For
                            End If

                            'If .Length = 0 Then
                            '    System.Threading.Thread.Sleep(100)
                            '    If Me.QuerySecure(mbSession, Me.GPIBIdnQuery & vbLf).ToLower.Contains(Me.GPIBIdnString.ToLower) Then
                            '        wasSuccess = True
                            '        Exit For
                            '    End If

                            'ElseIf .ToLower.Contains(Me.GPIBIdnString.ToLower) Then
                            '    wasSuccess = True
                            '    Exit For
                            'End If
                        End With

                    Catch ex As VisaException
                        ' Don't do anything
                        'CustomException.LogException(ex)
                    Catch ex As Exception
                        ExeptionHandler.LogException(ex)
                    End Try

                    If mbSession IsNot Nothing Then
                        mbSession.Dispose()
                        mbSession = Nothing
                    End If

                Next

                If wasSuccess Then Exit For

            Next

        Catch ex As Exception
            'No GPIB?*INSTR - Resources
        End Try

        If mbSession Is Nothing Then
            'MessageBox.Show("There was no Tunics-SC-Plus-Laser found on your system.", "Laser not found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        Else
            mbSession.Timeout = 10000
            Return mbSession
        End If

    End Function

End Class
