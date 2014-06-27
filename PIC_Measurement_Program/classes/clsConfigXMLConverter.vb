'Converts XML-String to clsXMLElement-Object and vice versa
'used to save Hardware-Device Configurations
'is able to detect Enumerations and saves the type of Enumeration
Public Class clsConfigXMLConverter

    Private mElements As New Dictionary(Of String, clsXMLElement)
    Private mSectionName As String

    Public Sub Clear()

        mSectionName = ""
        mElements.Clear()

    End Sub

    Public Property SectionName() As String
        Get
            Return mSectionName
        End Get
        Set(ByVal value As String)
            mSectionName = value
        End Set
    End Property

    Public Function asDictionary() As Dictionary(Of String, clsXMLElement)

        Return mElements

    End Function

    Public Function containsXMLElement(ByVal aKey As String) As Boolean
        Return mElements.ContainsKey(aKey)
    End Function

    Public Property XMLElement(ByVal aKey As String) As clsXMLElement

        Get

            If mElements.ContainsKey(aKey) Then
                Return mElements.Item(aKey)
            Else
                Dim cEx As New ExeptionHandler("Unknown ElementKey: " & aKey)
                Try
                    Throw cEx
                Catch anEx As ExeptionHandler
                    anEx.Log()
                End Try
                'Endstation
                Throw cEx
            End If

        End Get

        Set(ByVal value As clsXMLElement)
            mElements.Add(aKey, value)
        End Set

    End Property

    Public Property XMLEquivalent() As String

        Get

            'Convert given Elements/Attributes to XML-String:
            Dim aStrWriter As New StringWriter
            Dim anXMLWriter As New Xml.XmlTextWriter(aStrWriter)

            anXMLWriter.WriteStartElement(mSectionName)
            anXMLWriter.WriteString(vbCrLf)

            For Each anElement As KeyValuePair(Of String, clsXMLElement) In mElements

                anXMLWriter.WriteStartElement(anElement.Key)

                For Each anAttrib As KeyValuePair(Of String, String) In anElement.Value.asDictionary

                    'To ensure that numeric Values are Language-Independent, Microsofts XmlConvert-Class is used            
                    Dim valueAsString As String

                    If IsNumeric(anAttrib.Value) And Split(anAttrib.Value, ".").Count <= 2 And Split(anAttrib.Value, ",").Count <= 2 Then
                        valueAsString = System.Xml.XmlConvert.ToString(CType(anAttrib.Value, Decimal))
                    Else
                        valueAsString = anAttrib.Value
                    End If

                    anXMLWriter.WriteAttributeString(anAttrib.Key, valueAsString)

                Next

                'Schließt ein Element und löst den entsprechenden Namespacebereich auf.
                anXMLWriter.WriteEndElement()
                anXMLWriter.WriteString(vbCrLf)

            Next

            anXMLWriter.WriteEndElement()

            Return aStrWriter.ToString

        End Get

        Set(ByVal value As String)

            'Set by XML-String:
            Dim aStrReader As New StringReader(value)
            Dim anXMLReader As New Xml.XmlTextReader(aStrReader)

            Me.Clear()

            Try
                anXMLReader.Read()
            Catch ex As Exception
                Exit Property
            End Try

            Me.SectionName = anXMLReader.Name
            anXMLReader.ReadStartElement()

            Do
                'every XMLElement contains one Key of a XML string
                Dim anXMLElement As New clsConfigXMLConverter.clsXMLElement

                anXMLReader.Read()

                Do While anXMLReader.NodeType = Xml.XmlNodeType.Whitespace
                    anXMLReader.Read()
                Loop

                If anXMLReader.NodeType = Xml.XmlNodeType.EndElement Then Exit Do

                'To ensure that numeric Values are Language-Independent, Microsofts XmlConvert-Class is used            
                Dim valueAsString As String

                For i = 0 To anXMLReader.AttributeCount - 1

                    anXMLReader.MoveToAttribute(i)

                    If IsNumeric(anXMLReader.Value) And Split(anXMLReader.Value, ".").Count <= 2 And Split(anXMLReader.Value, ",").Count <= 2 Then
                        valueAsString = System.Xml.XmlConvert.ToDecimal(anXMLReader.Value).ToString
                    Else
                        valueAsString = anXMLReader.Value
                    End If

                    anXMLElement.XMLAttribute(anXMLReader.Name) = valueAsString

                Next

                'Wechselt zu dem Element, das den aktuellen Attributknoten enthält
                anXMLReader.MoveToElement()

                Me.XMLElement(anXMLReader.Name) = anXMLElement

            Loop Until anXMLReader.EOF

        End Set

    End Property

    '------------- clsXMLElement -----------
    Public Class clsXMLElement

        Private mAttributes As New Dictionary(Of String, String)
        Private mElementName As String

        'ändern in asAttributeDictionary
        Public ReadOnly Property asDictionary() As Dictionary(Of String, String)

            Get
                Return mAttributes
            End Get

        End Property

        Public Property XMLAttribute(ByVal aKey As String) As String
            Get

                If mAttributes.ContainsKey(aKey) Then

                    Return mAttributes.Item(aKey)

                Else
                    Dim cEx As New ExeptionHandler("Unknown AttributeKey: " & aKey)
                    Try
                        Throw cEx
                    Catch anEx As ExeptionHandler
                        anEx.Log()
                    End Try
                    'Endstation
                    Throw cEx
                End If

            End Get
            Set(ByVal value As String)

                If mAttributes.ContainsKey(aKey) Then mAttributes.Remove(aKey)

                mAttributes.Add(aKey, value)

            End Set

        End Property

        Public Property XMLAttribute(ByVal aKey As String, ByVal aEnumType As System.Enum) As [Enum]

            Get

                If mAttributes.ContainsKey(aKey) Then

                    Return CType(System.Enum.Parse(aEnumType.GetType, Split(mAttributes.Item(aKey), "_ENUM_")(0)), System.Enum)

                Else
                    Dim cEx As New ExeptionHandler("Unknown AttributeKey: " & aKey)
                    Try
                        Throw cEx
                    Catch anEx As ExeptionHandler
                        anEx.Log()
                    End Try
                    'Endstation
                    Throw cEx
                End If

            End Get

            Set(ByVal anEnum As [Enum])

                If mAttributes.ContainsKey(aKey) Then mAttributes.Remove(aKey)

                mAttributes.Add(aKey, anEnum.ToString & "_ENUM_" & aEnumType.GetType.ToString & ", " & My.Application.Info.AssemblyName)

            End Set

        End Property

        Public Property XMLAttribute(ByVal aKey As String, ByVal aEnumType As System.Type) As [Enum]

            Get

                If mAttributes.ContainsKey(aKey) Then

                    Return CType(System.Enum.Parse(aEnumType, Split(mAttributes.Item(aKey), "_ENUM_")(0)), System.Enum)

                Else
                    Dim cEx As New ExeptionHandler("Unknown AttributeKey: " & aKey)
                    Try
                        Throw cEx
                    Catch anEx As ExeptionHandler
                        anEx.Log()
                    End Try
                    'Endstation
                    Throw cEx
                End If

            End Get

            Set(ByVal anEnum As [Enum])

                If mAttributes.ContainsKey(aKey) Then mAttributes.Remove(aKey)

                mAttributes.Add(aKey, anEnum.ToString & "_ENUM_" & aEnumType.ToString & ", " & My.Application.Info.AssemblyName)

            End Set

        End Property

    End Class
    '------------- clsXMLElement -----------

    Public Sub New()
        '...
    End Sub

    Public Sub New(ByVal anXMLString As String)

        Me.XMLEquivalent = anXMLString

    End Sub

End Class

