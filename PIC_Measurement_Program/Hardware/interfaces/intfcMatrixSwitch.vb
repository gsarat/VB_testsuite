'implement into all device drivers that are a matrix switch
Public Interface intfcMatrixSwitch

    Sub CloseChannel(ByVal channel As Long)
    Sub OpenChannel(ByVal channel As Long)
    Sub ReleaseAllChannels()

End Interface
