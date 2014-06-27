'implement into all device drivers that source currents
Public Interface intfcCurrentSource
    Inherits intfcMeasurementInstrument

    Sub SetSourceCurrent(ByVal dValue As Double)

    Property VoltageCompliance() As Double 'maximum voltage

End Interface
