<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSpectrumMeasurement
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TChart1 = New Steema.TeeChart.TChart()
        Me.SuspendLayout()
        '
        'TChart1
        '
        '
        '
        '
        Me.TChart1.Aspect.Height3D = 0
        Me.TChart1.Aspect.View3D = False
        Me.TChart1.Aspect.Width3D = 0
        '
        '
        '
        '
        '
        '
        Me.TChart1.Axes.Bottom.IAxisSize = 471
        Me.TChart1.Axes.Bottom.IEndPos = 493
        Me.TChart1.Axes.Bottom.IGapSize = 0
        Me.TChart1.Axes.Bottom.IsDepthAxis = False
        Me.TChart1.Axes.Bottom.IStartPos = 22
        '
        '
        '
        Me.TChart1.Axes.Depth.IAxisSize = 0
        Me.TChart1.Axes.Depth.IEndPos = 0
        Me.TChart1.Axes.Depth.IGapSize = 0
        Me.TChart1.Axes.Depth.IsDepthAxis = True
        Me.TChart1.Axes.Depth.IStartPos = 0
        '
        '
        '
        Me.TChart1.Axes.DepthTop.IAxisSize = 0
        Me.TChart1.Axes.DepthTop.IEndPos = 0
        Me.TChart1.Axes.DepthTop.IGapSize = 0
        Me.TChart1.Axes.DepthTop.IsDepthAxis = True
        Me.TChart1.Axes.DepthTop.IStartPos = 0
        '
        '
        '
        Me.TChart1.Axes.Left.IAxisSize = 322
        Me.TChart1.Axes.Left.IEndPos = 363
        Me.TChart1.Axes.Left.IGapSize = 0
        Me.TChart1.Axes.Left.IsDepthAxis = False
        Me.TChart1.Axes.Left.IStartPos = 41
        Me.TChart1.Axes.NumFixedAxes = 6
        '
        '
        '
        Me.TChart1.Axes.Right.IAxisSize = 322
        Me.TChart1.Axes.Right.IEndPos = 363
        Me.TChart1.Axes.Right.IGapSize = 0
        Me.TChart1.Axes.Right.IsDepthAxis = False
        Me.TChart1.Axes.Right.IStartPos = 41
        '
        '
        '
        Me.TChart1.Axes.Top.IAxisSize = 471
        Me.TChart1.Axes.Top.IEndPos = 493
        Me.TChart1.Axes.Top.IGapSize = 0
        Me.TChart1.Axes.Top.IsDepthAxis = False
        Me.TChart1.Axes.Top.IStartPos = 22
        '
        '
        '
        Me.TChart1.Header.Visible = False
        Me.TChart1.Location = New System.Drawing.Point(-1, 0)
        Me.TChart1.Name = "TChart1"
        Me.TChart1.Size = New System.Drawing.Size(515, 404)
        Me.TChart1.TabIndex = 0
        '
        'frmSpectrumMeasurement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 613)
        Me.Controls.Add(Me.TChart1)
        Me.Name = "frmSpectrumMeasurement"
        Me.Text = "Spectrum Measurement"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TChart1 As Steema.TeeChart.TChart
End Class
