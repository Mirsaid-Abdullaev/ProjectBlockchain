Imports System.Drawing.Drawing2D
Imports System.IO
Module Design 'need to finish custommsgbox
    Public Class CustomMsgBox
        Inherits Form

        Protected Friend Property ButtonOK As Button
        Protected Friend Property ButtonCancel As Button
        Protected Friend Property MessageTxt As TextBox
        Protected Friend Property MessageHeadTxt As RichTextBox
        Protected Friend Overloads Property DialogResult As Boolean = False

        Public Sub New(Optional Title As String = Nothing, Optional ShowCancel As Boolean = True)
            If Not Title = Nothing Then
                Me.Text = Title
            Else
                Me.Text = "Message Box"
            End If
            Me.Height = 300
            Me.Width = 400
            Me.StartPosition = FormStartPosition.CenterScreen
            If ShowCancel Then
                Me.ButtonCancel = New Button With {.Location = New Point(8, 206), .Height = 43, .Width = 179, .Text = "CANCEL", .BackColor = Color.FromArgb(245, 0, 50), .Font = New Font("Bahnschrift Light", 20, FontStyle.Bold)}
                Me.ButtonOK = New Button With {.Location = New Point(193, 206), .Height = 43, .Width = 179, .Text = "OK", .BackColor = Color.Lime, .Font = New Font("Bahnschrift Light", 20, FontStyle.Bold)}
                AddHandler ButtonCancel.Click, AddressOf CancelButton_Click
                Me.Controls.Add(ButtonCancel)
            Else
                Me.ButtonOK = New Button With {.Location = New Point(8, 206), .Height = 43, .Width = 362, .Text = "OK", .BackColor = Color.Lime, .Font = New Font("Bahnschrift Light", 20, FontStyle.Bold)}
            End If
            Me.MessageTxt = New TextBox With {.Location = New Point(8, 12), .BackColor = Color.Ivory, .Width = 363, .Height = 182, .[ReadOnly] = True, .ScrollBars = ScrollBars.Vertical, .Multiline = True, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110)}
            Me.Controls.Add(MessageTxt)
            Me.Controls.Add(ButtonOK)
            Me.Icon = New Icon("C:\Users\abdul\Downloads\ProjectBlockchain\My Project\AppIcon.ico")

            AddHandler ButtonOK.Click, AddressOf OKButton_Click
            AddHandler MessageTxt.KeyDown, AddressOf MessageTextBox_KeyDown
            Me.Focus()
            MsgBoxDesign(Me)
        End Sub

        Protected Friend Sub OKButton_Click(sender As Object, e As EventArgs)
            DialogResult = True
            Me.Close()
        End Sub

        Private Sub CancelButton_Click(sender As Object, e As EventArgs)
            DialogResult = False
            Me.Close()
        End Sub

        Private Sub MessageTextBox_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                OKButton_Click(ButtonOK, EventArgs.Empty)
            End If
        End Sub

        Public Shared Function ShowBox(Data As String, Optional Title As String = Nothing, Optional ShowCancel As Boolean = True) As Boolean
            Dim msgBoxInstance As CustomMsgBox
            msgBoxInstance = New CustomMsgBox(Title, ShowCancel)
            Application.EnableVisualStyles()
            msgBoxInstance.MessageTxt.Text = Data
            msgBoxInstance.MessageTxt.SelectionStart = msgBoxInstance.MessageTxt.Text.Length

            msgBoxInstance.ShowDialog()
            Return msgBoxInstance.DialogResult
        End Function
    End Class


    Public Class CustomListBoxInputBox
        Inherits CustomMsgBox

        Public InputListBox As ListBox

        Public Sub New(Heading As String, Optional ShowCancel As Boolean = True, Optional Title As String = Nothing)
            MyBase.New(Title, ShowCancel)

            Me.Controls.Remove(MessageTxt)  ' Remove the original TextBox

            Me.InputListBox = New ListBox With {.Location = New Point(8, 88), .BackColor = Color.Ivory, .Width = 363, .Height = 106, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110)}
            Me.MessageHeadTxt = New RichTextBox With {.Location = New Point(8, 9), .Width = 363, .Height = 73, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110), .[ReadOnly] = True, .Text = Heading}
            Me.Controls.Add(MessageHeadTxt)
            Me.Controls.Add(InputListBox)
            Try
                For Each Wallet As String In Directory.EnumerateFiles(FileSystem.DirectoryList(0))
                    InputListBox.Items.Add(Path.GetFileName(Wallet))
                Next
            Catch ex As Exception
                CustomMsgBox.ShowBox("Empty or missing file directory. No wallets to open.", "ERROR", False)
            End Try
            ' Set the default button to OK
            Me.AcceptButton = ButtonOK
            Me.ButtonOK.Focus()
        End Sub

        ' Method to display the input box with ListBox
        Public Shared Function ShowInputBox(Heading As String, Optional ShowCancel As Boolean = True, Optional Title As String = Nothing) As Object() 'returns {selecteditem, dialogresult} for capturing a potential cancelclicked
            Dim CustomLBInstance As New CustomListBoxInputBox(Heading, ShowCancel, Title)
            Application.EnableVisualStyles()
            CustomLBInstance.ShowDialog()
            Return {If(CustomLBInstance.DialogResult AndAlso CustomLBInstance.InputListBox.SelectedItem IsNot Nothing, CustomLBInstance.InputListBox.SelectedItem.ToString(), String.Empty), CustomLBInstance.DialogResult}
        End Function
    End Class

    Public Class CustomInputBox
        Inherits CustomMsgBox
        Protected Friend Property NewTxt As TextBox
        Public Sub New(Prompt As String, Optional Title As String = Nothing, Optional ShowCancel As Boolean = True)
            MyBase.New(Title, ShowCancel)
            Me.NewTxt = New TextBox() With {.Location = New Point(8, 88), .BackColor = Color.Ivory, .Width = 363, .Height = 106, .[ReadOnly] = False, .ScrollBars = ScrollBars.Vertical, .Multiline = True, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110)}
            Me.Controls.Remove(MessageTxt)
            Me.MessageHeadTxt = New RichTextBox With {.Location = New Point(8, 9), .Width = 363, .Height = 73, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110), .[ReadOnly] = True, .Text = Prompt}
            Me.Controls.Add(NewTxt)
            Me.Controls.Add(MessageHeadTxt)
            AddHandler NewTxt.KeyDown, AddressOf NewTxt_KeyDown
        End Sub

        Public Shared Function ShowInputBox(Prompt As String, Optional Title As String = Nothing, Optional ShowCancel As Boolean = True) As Object()
            Dim CustomIBInstance As New CustomInputBox(Prompt, Title, ShowCancel)
            Application.EnableVisualStyles()
            CustomIBInstance.MessageTxt.SelectionStart = CustomIBInstance.MessageTxt.Text.Length
            CustomIBInstance.ShowDialog()
            Return {CustomIBInstance.NewTxt.Text, CustomIBInstance.DialogResult}
        End Function

        Private Sub NewTxt_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                OKButton_Click(ButtonOK, EventArgs.Empty)
            End If
        End Sub
    End Class

End Module

Module GradientHelper
    Public ButtonColours As Color() = {
    Color.FromArgb(210, 209, 207),
    Color.FromArgb(253, 247, 177),
    Color.FromArgb(241, 228, 158)
    }
    Public MainMenuColours As Color() = {
    Color.FromArgb(2, 170, 176),   ' rgba(2, 170, 176, 1)
    Color.FromArgb(1, 181, 175),    ' rgba(1, 181, 175, 1)
    Color.FromArgb(0, 189, 174),    ' rgba(0, 189, 174, 1)
    Color.FromArgb(0, 195, 173),    ' rgba(0, 195, 173, 1)
    Color.FromArgb(0, 205, 172)     ' rgba(0, 205, 172, 1)
    }
    Public MsgBoxColours As Color() = {
    Color.FromArgb(238, 238, 255),
    Color.FromArgb(173, 255, 183)
    }
    Public WalletColours As Color() = {
        Color.FromArgb(35, 7, 77),
        Color.FromArgb(204, 83, 51)
    }
    Public ViewingMenuColours As Color() = {
        Color.FromArgb(55, 59, 68),
        Color.FromArgb(66, 134, 244)
    }
    Public TransactPoolColours As Color() = {
    Color.FromArgb(255, 216, 155), ' #ffd89b
    Color.FromArgb(25, 84, 123)     ' #19547b
}
    Public BlockchainExpColours As Color() = {
    Color.FromArgb(44, 62, 80),   ' #2c3e50
    Color.FromArgb(52, 152, 219)   ' #3498db
}



    ' Helper method to set a control's background color with a diagonal gradient
    Public Sub SetControlGradient(control As Control, colors As Color())
        Dim gradientBrush As LinearGradientBrush
        ' Set up the color blend
        Dim blend As New ColorBlend With {
            .Colors = colors,
            .Positions = CalculateGradientPositions(colors.Length)
        }

        ' Create a linear gradient brush
        ' Assign the color blend to the brush
        gradientBrush = New LinearGradientBrush(control.ClientRectangle, Color.Black, Color.Black, LinearGradientMode.ForwardDiagonal) With {
            .InterpolationColors = blend
        }

        ' Set the control's background to the gradient brush
        control.BackgroundImage = New Bitmap(1, 1)
        control.BackgroundImage = DrawToBitmap(gradientBrush, control.ClientRectangle.Size)
        control.ForeColor = Color.DodgerBlue
    End Sub

    ' Helper method to calculate gradient positions
    Private Function CalculateGradientPositions(count As Integer) As Single()
        Dim positions(count - 1) As Single
        For i As Integer = 0 To count - 1
            positions(i) = CSng(i) / (count - 1)
        Next
        Return positions
    End Function

    ' Helper method to draw the gradient to a bitmap
    Private Function DrawToBitmap(brush As Brush, size As Size) As Bitmap
        Dim bitmap As New Bitmap(size.Width, size.Height)
        Using g As Graphics = Graphics.FromImage(bitmap)
            g.FillRectangle(brush, New Rectangle(Point.Empty, size))
        End Using
        Return bitmap
    End Function

    Public Sub DesignLoad(ParentFrm As Form, ColourArray As Color()) 'to be used in form.load() events to update UI
        For Each ctl As Control In ParentFrm.Controls.OfType(Of Button)
            If Not ctl.Name = "Disconnect" AndAlso Not ctl.Name = "GetHelpManual" AndAlso Not ctl.Name = Nothing AndAlso Not ctl.Name = "BackBtn" Then
                SetControlGradient(ctl, ButtonColours)
                ctl.ForeColor = Color.FromArgb(139, 43, 110)
            End If
        Next
        SetControlGradient(ParentFrm, ColourArray)
    End Sub

    Public Sub MsgBoxDesign(ParentFrm As Form)
        For Each ctl As Control In ParentFrm.Controls.OfType(Of Button)
            ctl.ForeColor = Color.Goldenrod
        Next
        SetControlGradient(ParentFrm, MsgBoxColours)
    End Sub
End Module
