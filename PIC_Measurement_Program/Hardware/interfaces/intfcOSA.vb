Public Interface intfcOSA
    Inherits intfcMeasurementInstrument

    Enum enumSweepMode
        sweepSingle
        sweepContinuous
    End Enum

    Property CenterWavelength() As Double
    Property WavelengthSpan() As Double

    Property ReferenceLevel() As Double
    Property dbPerDivision() As Double

    Property StartWavelength() As Double
    Property StopWavelength() As Double

    Property Resolution() As Double
    Property VBW() As Double
    Property Averaging() As Integer

    Property SweepMode() As enumSweepMode

    ReadOnly Property readData() As Double()

End Interface
