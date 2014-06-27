'implement into all device drivers that source voltages
Public Interface intfcVoltageSource
    Inherits intfcMeasurementInstrument

    Sub SetSourceVoltage(ByVal dblValue As Double)

    Property CurrentCompliance() As Double 'maximum current

End Interface
