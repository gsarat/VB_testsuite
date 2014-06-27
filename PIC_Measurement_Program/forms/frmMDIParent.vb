Imports System.Windows.Forms

Public Class frmMDIParent

    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs)
        ' Create a new instance of the child form.
        Dim ChildForm As New System.Windows.Forms.Form
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        m_ChildFormNumber += 1
        ChildForm.Text = "Window " & m_ChildFormNumber

        ChildForm.Show()
    End Sub


    Private m_ChildFormNumber As Integer


    Private Sub OSASpectrumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OSASpectrumToolStripMenuItem.Click
        With frmSpectrumMeasurement
            .MdiParent = Me
            .Show()
        End With
    End Sub


End Class
