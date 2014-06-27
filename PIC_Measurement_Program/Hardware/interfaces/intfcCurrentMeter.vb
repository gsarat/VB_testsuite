'implement into all device drivers that measure currents
Public Interface intfcCurrentMeter
    Inherits intfcMeasurementInstrument

    Function ReadCurrent() As Double

End Interface
