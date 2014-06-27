Public Interface intfcTemperatureController
    Inherits intfcMeasurementInstrument

    Property SetPointTemperature() As Double
    Property OutputState() As Boolean

    Function ReadTemperature() As Double

End Interface
