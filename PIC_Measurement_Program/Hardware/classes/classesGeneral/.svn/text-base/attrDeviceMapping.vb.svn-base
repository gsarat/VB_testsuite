Imports System.Reflection

''' <summary>
''' Mit diesem Attribute (Attribute im Sinne von .NET) kann man angeben welche
''' Properties eines HardwareDevices (d.h. eine Klasse, die intfcHardwareDevice
''' implementiert) zur Konfiguration gemappt werden sollen.
''' <para>Also angenommen die &quot;Wavelength&quot; Property einer Klasse clsLaser
''' soll in <see cref="T:Hardware.frmConfigureDevices">frmConfigureDevices</see>
''' gesetzt werden können. Die Property soll unter dem Knoten &quot;General&quot;
''' auftauchen und &quot;Wellenlänge&quot; heißen. </para>
''' <para>Dann muss dieses Attribute über die Property, die die Wellenlänge setzen
''' soll (diese kann auch anders heißen z.B. LaserWavelength), gesetzt werden. Dabei
''' ist der TreeKey:=&quot;General&quot; und der
''' NodeName:=&quot;Wavelength&quot;.</para>
''' <para></para>
''' <para>In <see cref="T:Hardware.frmConfigureDevices">frmConfigureDevices</see>
''' wird dann unter Laser-&gt;General-&gt;Wavelength ein Item angezeigt, was mit der
''' Property clsLaser.LaserWavelength verknüpft ist.</para>
''' </summary>
<AttributeUsage(AttributeTargets.Property, AllowMultiple:=True, Inherited:=True)> _
Public Class attrDeviceMapping
    Inherits Attribute

    Private m_szTreeKey As String
    Private m_szNodeName As String
    Private m_szPropertyName As String

    Sub New(ByVal szTreeKey As String, ByVal szNodeName As String)
        m_szTreeKey = szTreeKey
        m_szNodeName = szNodeName
    End Sub

    Public ReadOnly Property TreeKey() As String
        Get
            Return m_szTreeKey
        End Get
    End Property

    Public ReadOnly Property NodeName() As String
        Get
            Return m_szNodeName
        End Get
    End Property

    Public Property Tag() As String
        Get
            Return m_szPropertyName
        End Get
        Set(ByVal value As String)
            m_szPropertyName = value
        End Set
    End Property

    Public Shared Function GetAllProperties(ByVal pType As Type) As List(Of attrDeviceMapping)
        Dim Res As New List(Of attrDeviceMapping)
        Dim PropertyInfos As System.Reflection.PropertyInfo() = pType.GetProperties()
        Dim CustAttr As attrDeviceMapping

        For Each aInfo As PropertyInfo In PropertyInfos
            For i As Integer = 0 To Attribute.GetCustomAttributes(pType.GetMember(aInfo.Name)(0), GetType(attrDeviceMapping)).Length - 1

                CustAttr = CType(Attribute.GetCustomAttributes(pType.GetMember(aInfo.Name)(0), GetType(attrDeviceMapping))(i),  _
                            attrDeviceMapping)

                CustAttr.Tag = aInfo.Name

                If CustAttr IsNot Nothing Then
                    Res.Add(CustAttr)
                End If
            Next i

        Next


        Return Res

    End Function
End Class

