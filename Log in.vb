Imports MySql.Data.MySqlClient
Public Class Form1

    Private Sub TextUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextUsername.TextChanged


    End Sub

    Private Sub LinkSignUp_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkSignUp.LinkClicked
        Form2.id.Text = ""
        Form2.TextName.Text = ""
        Form2.TextUsername.Text = ""
        Form2.TextPassword.Text = ""
        Form2.TextConfirmPassword.Text = ""
        Form2.Show()
        Me.Hide()
    End Sub

    Private Sub ButtonLogIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLogIn.Click

        If TextUsername.Text = "" Then
            MsgBox("Username Required", MsgBoxStyle.Exclamation, "Warning!")
            TextUsername.Focus()
        ElseIf TextPassword.Text = "" Then
            MsgBox("Password Required", MsgBoxStyle.Exclamation, "Warning!")
            TextPassword.Focus()

        Else
            connstring = "select * from sign_up where Username = '" & TextUsername.Text & "' and Password = '" & TextPassword.Text & "'"
            sqlconnection.Open()
            Dim cmd As New MySqlCommand(connstring, sqlconnection)
            dr = cmd.ExecuteReader
            If dr.Read = True Then
                MsgBox("Welcome", MsgBoxStyle.Information, "Log In Message")
                Me.Hide()
                Form5.Show()
            Else

                MsgBox("Invalid User Credentials", MsgBoxStyle.Exclamation, "Error")

                TextUsername.Text = ""

                TextPassword.Text = ""
            End If
            sqlconnection.Close()
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextPassword.PasswordChar = "*"

    End Sub


    Private Sub CheckBoxShowPassword_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxShowPassword.CheckedChanged
        If CheckBoxShowPassword.Checked Then
            TextPassword.PasswordChar = ""
        Else
            TextPassword.PasswordChar = "*" ' or any other character you prefer
        End If
    End Sub


    Private Sub TextPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextPassword.TextChanged

    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click

    End Sub


    Private Sub PictureBoxClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxClose.Click
        Dim response As Integer

        response = MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If response = vbYes Then

            Application.ExitThread()

        End If

    End Sub

    Private Sub PictureBoxRestor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxRestor.Click
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub PictureBoxMini_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBoxMini.Click
        Me.WindowState = FormWindowState.Normal
    End Sub

   

    Private Sub ButtonLogIn_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonLogIn.Disposed

    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub
End Class