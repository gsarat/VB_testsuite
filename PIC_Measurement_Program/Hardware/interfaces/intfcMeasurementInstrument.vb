Option Strict On
Option Explicit On

'This is a general hardware interface that every device interface must implement
'All not abstract Classes that inherit this class are automatically available in clsDeviceAssignments
Public Interface intfcMeasurementInstrument

    ''' <summary>
    ''' Checks if the configuration for the communication is valid.
    ''' </summary>
    ''' <returns>
    ''' True if valid, false else.
    ''' </returns>
    ''' <remarks></remarks>
    Function InstrumentIsAvailable() As Boolean

    ''' <summary>
    ''' Opens the communication to the instrument.
    ''' </summary>
    ''' <returns>
    ''' True if successful, false else
    ''' </returns>
    ''' <remarks></remarks>
    Function OpenSession() As Boolean

    ''' <summary>
    ''' Closes the communication to the instrument.
    ''' </summary>
    ''' <remarks></remarks>
    Sub CloseSession()

    ''' <summary>
    ''' Initializes the instrument.
    ''' </summary>
    ''' <remarks></remarks>
    Sub Initialize()

    ''' <summary>
    ''' Sets the instrument to the default settings.
    ''' </summary>
    ''' <remarks></remarks>
    Sub SetToDefault()

    ''' <summary>
    ''' Returns the name of the instrument device.
    ''' </summary>
    ''' <value></value>
    ''' <returns>
    ''' Name of instrument
    ''' </returns>
    ''' <remarks></remarks>
    ReadOnly Property Name() As String


End Interface
