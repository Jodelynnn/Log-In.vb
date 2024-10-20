Imports System.Data.DataTable
Imports MySql.Data.MySqlClient

Public Class Form2
    Dim Index As Integer
    Dim newdata As DataSet

    '  Private sqlconnection As New MySqlConnection("Server=localhost;Port=3306;Database=admin_db;Uid=root;Pwd=Cheng_30;")

    Public Sub formid()
        Dim created = ""

        Dim connstring = "SELECT * FROM sign_up WHERE UserID = 'NOID'"
        sqlconnection.Open()
        cmd = New MySqlCommand(connstring, sqlconnection)
        dr = cmd.ExecuteReader()
        If dr.Read() Then
            id.Text = "UserID-00" & dr("id").ToString()
        Else
            created = "create"
        End If
        sqlconnection.Close()

        If created = "create" Then
            makeid()
        End If
    End Sub

    Public Sub makeid()
        Dim connstring = "INSERT INTO sign_up (UserID) VALUES ('NOID')"
        sqlconnection.Open()
        Dim di = New MySqlDataAdapter(connstring, sqlconnection)
        di.Fill(ds)
        sqlconnection.Close()
        formid()
    End Sub

    Private Sub Form2_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        TextPassword.PasswordChar = "*"
        TextConfirmPassword.PasswordChar = "*"
        id.Focus()
        formid()
        Dataview()
    End Sub

    Private Sub Dataview()
        Try
            Dim query As String = "SELECT * FROM sign_up WHERE UserID <> 'NOID'"
            sqlconnection.Open()
            Dim cmd As New MySqlCommand(query, sqlconnection)
            Dim dr As MySqlDataReader = cmd.ExecuteReader()
            DataGridView1.Rows.Clear()

            While dr.Read()
                DataGridView1.Rows.Add(dr("UserID"), dr("Name"), dr("Username"), dr("Password"), dr("Date_Created"), dr("Time_Created"))
            End While
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            sqlconnection.Close()
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSave.Click
        If ValidateInput() Then
            Dim userIdExists As Boolean = CheckIfIdExists(id.Text)

            If userIdExists Then
                Dim rowIndex As Integer = FindRowIndexById(id.Text)
                If rowIndex >= 0 Then
                    UpdateRow(rowIndex)
                Else
                    MsgBox("User ID does not correspond to any record.", MsgBoxStyle.Exclamation, "Update Error")
                End If
            Else
                AddNewRow()
            End If

            ClearFields()
            Dataview()
        Else
            MsgBox("Please fill in all required fields.", MsgBoxStyle.Exclamation, "Input Error")
        End If
    End Sub

    Private Function ValidateInput() As Boolean
        If String.IsNullOrEmpty(TextName.Text) Then
            ShowWarning("Name Required", TextName)
        ElseIf String.IsNullOrEmpty(TextUsername.Text) Then
            ShowWarning("Username Required", TextUsername)
        ElseIf String.IsNullOrEmpty(TextPassword.Text) Then
            ShowWarning("Password Required", TextPassword)
        ElseIf String.IsNullOrEmpty(TextConfirmPassword.Text) Then
            ShowWarning("Confirm Password Required", TextConfirmPassword)
        ElseIf TextConfirmPassword.Text <> TextPassword.Text Then
            ShowWarning("Confirm Password Does Not Match", TextConfirmPassword)
            TextConfirmPassword.Clear()
            TextConfirmPassword.Focus()
        Else
            Return True
        End If
        Return False
    End Function

    Private Function CheckIfIdExists(ByVal UserId As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM sign_up WHERE UserID = @UserID"
        Using cmd As New MySqlCommand(query, sqlconnection)
            cmd.Parameters.AddWithValue("@UserID", UserId)
            sqlconnection.Open()
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            sqlconnection.Close()
            Return count > 0
        End Using
    End Function

    Private Sub ShowWarning(ByVal message As String, ByVal control As Control)
        MsgBox(message, MsgBoxStyle.Exclamation, "Warning!")
        control.Focus()
    End Sub

    Private Function FindRowIndexById(ByVal userId As String) As Integer
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row.Cells(0).Value IsNot Nothing AndAlso row.Cells(0).Value.ToString() = userId Then
                Return row.Index
            End If
        Next
        Return -1
    End Function

    Private Sub UpdateRow(ByVal rowIndex As Integer)
        DataGridView1.Rows(rowIndex).Cells(1).Value = TextName.Text
        DataGridView1.Rows(rowIndex).Cells(2).Value = TextUsername.Text
        DataGridView1.Rows(rowIndex).Cells(3).Value = TextPassword.Text
        DataGridView1.Rows(rowIndex).Cells(4).Value = DateTime.Now.ToString("MMMM dd, yyyy")
        DataGridView1.Rows(rowIndex).Cells(5).Value = DateTime.Now.ToString("t")

        Dim query = "UPDATE sign_up SET Name = @Name, Username = @Username, Password = @Password WHERE UserID = @UserID"
        Using cmd As New MySqlCommand(query, sqlconnection)
            cmd.Parameters.AddWithValue("@UserID", id.Text)
            cmd.Parameters.AddWithValue("@Name", TextName.Text)
            cmd.Parameters.AddWithValue("@Username", TextUsername.Text)
            cmd.Parameters.AddWithValue("@Password", TextPassword.Text)
            sqlconnection.Open()
            cmd.ExecuteNonQuery()
            sqlconnection.Close()
        End Using

        MsgBox("Updated Successfully!", MsgBoxStyle.Information, "Update")
        ClearFields()
    End Sub

    Private Sub AddNewRow()
        Try
            If String.IsNullOrEmpty(TextName.Text) Then
                ShowWarning("Name is required.", TextName)
                Return
            End If

            Dim query As String = "INSERT INTO sign_up (UserID, Name, Username, Password, Date_Created, Time_Created) VALUES (@UserID, @Name, @Username, @Password, @Date_Created, @Time_Created)"
            Using cmd As New MySqlCommand(query, sqlconnection)
                cmd.Parameters.AddWithValue("@UserID", id.Text)
                cmd.Parameters.AddWithValue("@Name", TextName.Text)
                cmd.Parameters.AddWithValue("@Username", TextUsername.Text)
                cmd.Parameters.AddWithValue("@Password", TextPassword.Text)
                cmd.Parameters.AddWithValue("@Date_Created", DateTime.Now.ToString("yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@Time_Created", DateTime.Now.ToString("HH:mm:ss"))

                sqlconnection.Open()
                cmd.ExecuteNonQuery()
            End Using

            DataGridView1.Rows.Add(id.Text, TextName.Text, TextUsername.Text, TextPassword.Text, DateTime.Now.ToString("MMMM dd, yyyy"), DateTime.Now.ToString("t"))
            MsgBox("Saved Successfully!", MsgBoxStyle.Information, "Save")
            ClearFields()
        Catch ex As MySqlException
            MsgBox("An error occurred: " & ex.Message, MsgBoxStyle.Critical, "Error")
        Finally
            sqlconnection.Close()
        End Try
    End Sub

    Private Sub ClearFields()
        TextName.Clear()
        TextUsername.Clear()
        TextPassword.Clear()
        TextConfirmPassword.Clear()
        id.Clear()
        id.Focus()
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        If MsgBox("Are you sure you want to cancel? Any unsaved changes will be lost.", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Cancel") = MsgBoxResult.Yes Then
            ClearFields()
        End If
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            If row.Cells(0).Value IsNot Nothing Then
                id.Text = row.Cells(0).Value.ToString()
                TextName.Text = row.Cells(1).Value.ToString()
                TextUsername.Text = row.Cells(2).Value.ToString()
                TextPassword.Text = row.Cells(3).Value.ToString()
                TextName.Focus()
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Dim passwordChar As Char = If(CheckBox1.Checked, "", "*")
        TextPassword.PasswordChar = passwordChar
        TextConfirmPassword.PasswordChar = passwordChar
    End Sub


    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Form3.Show()
        Me.Hide()
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub PictureBox7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox7.Click
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub PictureBox6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox6.Click
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub PictureBox8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox8.Click
        Dim response As Integer

        response = MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If response = vbYes Then

            Application.ExitThread()

        End If

    End Sub


    Private Sub id_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles id.TextChanged

    End Sub

    Private Sub PictureBox9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox9.Click
        Me.Hide()
        Form1.Show()


        Form1.TextUsername.Text = ""
        Form1.TextPassword.Text = ""


    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
