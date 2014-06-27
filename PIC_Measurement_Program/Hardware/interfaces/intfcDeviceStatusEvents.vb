Public Interface intfcDeviceStatusEvents
    Event SessionOpening(ByVal aHardware As intfcMeasurementInstrument)
    Event SessionClosing(ByVal aHardware As intfcMeasurementInstrument)
    Event InitHardware(ByVal aHardware As intfcMeasurementInstrument)
    Event ShutdownHardware(ByVal aHardware As intfcMeasurementInstrument)
End Interface
