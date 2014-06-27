'implement into all device drivers that read voltages
Public Interface intfcVoltageMeter
    Inherits intfcMeasurementInstrument

    Function ReadVoltage() As Double 'read a single voltage
    Function ReadVoltages(ByVal intNumOfSamples As Integer) As Double() 'read multiple voltages

End Interface
