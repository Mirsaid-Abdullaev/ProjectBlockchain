Imports System.Drawing.Drawing2D
Imports System.IO
Module Design
    Public Class CustomMsgBox
        Inherits Form
        Protected Friend Property ButtonOK As Button
        Protected Friend Property ButtonCancel As Button
        Protected Friend Property MessageTxt As TextBox
        Protected Friend Property MessageHeadTxt As RichTextBox
        Protected Friend Overloads Property DialogResult As Boolean = False

        Public Sub New(Optional Title As String = Nothing, Optional ShowCancel As Boolean = True)
            If Not Title = Nothing Then
                Me.Text = Title 'allows for custom user title to be input
            Else
                Me.Text = "Message Box" 'otherwise set as default title
            End If
            Me.MaximizeBox = False 'removes maximise option for user, UI safety
            Me.Height = 300
            Me.Width = 400
            'initialising form size
            Me.StartPosition = FormStartPosition.CenterScreen
            'specifies form will initialise in the middle of the screen
            If ShowCancel Then 'user can specify whether to show cancel button or not
                Me.ButtonCancel = New Button With {.Location = New Point(8, 206), .Height = 43, .Width = 179, .Text = "CANCEL", .BackColor = Color.FromArgb(245, 0, 50), .Font = New Font("Bahnschrift Light", 20, FontStyle.Bold)}
                Me.ButtonOK = New Button With {.Location = New Point(193, 206), .Height = 43, .Width = 179, .Text = "OK", .BackColor = Color.Lime, .Font = New Font("Bahnschrift Light", 20, FontStyle.Bold)}
                AddHandler ButtonCancel.Click, AddressOf CancelButton_Click
                Me.Controls.Add(ButtonCancel)
                'creates a cancel button and an ok button appropriately placed and sized
            Else
                Me.ButtonOK = New Button With {.Location = New Point(8, 206), .Height = 43, .Width = 362, .Text = "OK", .BackColor = Color.Lime, .Font = New Font("Bahnschrift Light", 20, FontStyle.Bold)}
                'just a large OK button created
            End If
            Me.MessageTxt = New TextBox With {.Location = New Point(8, 12), .BackColor = Color.Ivory, .Width = 363, .Height = 182, .[ReadOnly] = True, .ScrollBars = ScrollBars.Vertical, .Multiline = True, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110)}
            'this is the textbox for the data itself to be displayed
            Me.Controls.Add(MessageTxt)
            Me.Controls.Add(ButtonOK)
            'add controls to the form
            Me.Icon = New Icon("C:\Users\abdul\Downloads\ProjectBlockchain\My Project\AppIcon.ico")
            'set the icon
            AddHandler ButtonOK.Click, AddressOf OKButton_Click 'make a handler for the OK button
            AddHandler MessageTxt.KeyDown, AddressOf MessageTextBox_KeyDown 'make a handler for user clicking Enter on the messagebox
            Me.Focus() 'give focus to the message box
            MsgBoxDesign(Me) 'set the colour gradients for the message box form
        End Sub

        Protected Friend Sub OKButton_Click(sender As Object, e As EventArgs)
            DialogResult = True 'returns the dialog result that the ok button has been pressed
            Me.Close() 'closes the form
        End Sub

        Private Sub CancelButton_Click(sender As Object, e As EventArgs)
            DialogResult = False 'returns false for the dialog result = cancel clicked
            Me.Close() 'closes the form
        End Sub

        Private Sub MessageTextBox_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                OKButton_Click(ButtonOK, EventArgs.Empty) 'enter has same effect as OK button
            End If
        End Sub

        Public Shared Function ShowBox(Data As String, Optional Title As String = Nothing, Optional ShowCancel As Boolean = True) As Boolean
            Dim msgBoxInstance As CustomMsgBox 'local instance
            msgBoxInstance = New CustomMsgBox(Title, ShowCancel) 'uses user input
            Application.EnableVisualStyles() 'boilerplate code
            msgBoxInstance.MessageTxt.Text = Data 'sets the message txt box text
            msgBoxInstance.MessageTxt.SelectionStart = msgBoxInstance.MessageTxt.Text.Length 'sets cursor to the end of the text

            msgBoxInstance.ShowDialog() 'opens the form dialog box 
            Return msgBoxInstance.DialogResult 'returns the button clicked
        End Function
    End Class


    Public Class CustomListBoxInputBox
        Inherits CustomMsgBox
        'this class is purely for displaying wallets in my application
        Public InputListBox As ListBox

        Public Sub New(Heading As String, Optional ShowCancel As Boolean = True, Optional Title As String = Nothing)
            MyBase.New(Title, ShowCancel)

            Me.Controls.Remove(MessageTxt)  ' Remove the original textbox

            Me.InputListBox = New ListBox With {.Location = New Point(8, 88), .BackColor = Color.Ivory, .Width = 363, .Height = 106, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110)}
            'sets up the list boxes with location and size
            Me.MessageHeadTxt = New RichTextBox With {.Location = New Point(8, 9), .Width = 363, .Height = 73, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110), .[ReadOnly] = True, .Text = Heading}
            'heading message box with data description
            Me.Controls.Add(MessageHeadTxt)
            Me.Controls.Add(InputListBox)
            'add controls to the form instance
            Try
                For Each Wallet As String In Directory.EnumerateFiles(DirectoryList(0))
                    InputListBox.Items.Add(Path.GetFileName(Wallet))
                Next
            Catch ex As Exception
                ShowBox("Empty or missing file directory. No wallets to open.", "ERROR", False)
            End Try
            'shows all the wallet filenames to the user in the listbox or throws an error messagebox and exits
            ' Set the default button to OK
            Me.AcceptButton = ButtonOK
            Me.ButtonOK.Focus() 'set focus to ok button
        End Sub

        ' Method to display the input box with ListBox
        Public Shared Function ShowInputBox(Heading As String, Optional ShowCancel As Boolean = True, Optional Title As String = Nothing) As Object() 'returns {selecteditem, dialogresult} for capturing a potential cancelclicked
            Dim CustomLBInstance As New CustomListBoxInputBox(Heading, ShowCancel, Title)
            Application.EnableVisualStyles()
            CustomLBInstance.ShowDialog()
            Return {If(CustomLBInstance.DialogResult AndAlso CustomLBInstance.InputListBox.SelectedItem IsNot Nothing, CustomLBInstance.InputListBox.SelectedItem.ToString(), String.Empty), CustomLBInstance.DialogResult}
            'if ok clicked and something selected, return that item as a string, otherwise return an empty string
        End Function
    End Class

    Public Class CustomInputBox
        'versatile inputbox class for entering data
        Inherits CustomMsgBox
        Protected Friend Property NewTxt As TextBox 'textbox for user input
        Public Sub New(Prompt As String, Optional Title As String = Nothing, Optional ShowCancel As Boolean = True)
            MyBase.New(Title, ShowCancel)
            Me.NewTxt = New TextBox() With {.Location = New Point(8, 88), .BackColor = Color.Ivory, .Width = 363, .Height = 106, .[ReadOnly] = False, .ScrollBars = ScrollBars.Vertical, .Multiline = True, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110)}
            Me.Controls.Remove(MessageTxt)
            'adds the new textbox as the old one is a different size and location
            Me.MessageHeadTxt = New RichTextBox With {.Location = New Point(8, 9), .Width = 363, .Height = 73, .Font = New Font("Bahnschrift Light", 12, FontStyle.Regular), .ForeColor = Color.FromArgb(139, 43, 110), .[ReadOnly] = True, .Text = Prompt}
            'sets message heading textbox properties
            Me.Controls.Add(NewTxt)
            Me.Controls.Add(MessageHeadTxt)
            'adding the controls to the form
            AddHandler NewTxt.KeyDown, AddressOf NewTxt_KeyDown 'adding a handler for user clicking Enter
        End Sub

        Public Shared Function ShowInputBox(Prompt As String, Optional Title As String = Nothing, Optional ShowCancel As Boolean = True) As Object()
            Dim CustomIBInstance As New CustomInputBox(Prompt, Title, ShowCancel)
            Application.EnableVisualStyles()
            CustomIBInstance.MessageTxt.SelectionStart = CustomIBInstance.MessageTxt.Text.Length
            CustomIBInstance.ShowDialog()
            Return {CustomIBInstance.NewTxt.Text, CustomIBInstance.DialogResult} 'returns an object(1) where 0 is the text entered, and 1 is the button clicked
        End Function

        Private Sub NewTxt_KeyDown(sender As Object, e As KeyEventArgs)
            If e.KeyCode = Keys.Enter Then
                OKButton_Click(ButtonOK, EventArgs.Empty) 'Enter while in the input text box is same effect as OK
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
    Color.FromArgb(2, 170, 176),
    Color.FromArgb(1, 181, 175),
    Color.FromArgb(0, 189, 174),
    Color.FromArgb(0, 195, 173),
    Color.FromArgb(0, 205, 172)
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
    Color.FromArgb(255, 216, 155),
    Color.FromArgb(25, 84, 123)
}
    Public BlockchainExpColours As Color() = {
    Color.FromArgb(44, 62, 80),
    Color.FromArgb(52, 152, 219)
}
    Public SendingScreenColours As Color() = {
    Color.FromArgb(0, 90, 167),
    Color.FromArgb(255, 253, 228)
}
    'each form has its colour gradient setup, which is used in the form load event to paint the forms


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

    Public Sub DesignLoad(ParentFrm As Form, ColourArray As Color()) 'used in form.load() events to update UI
        For Each ctl As Control In ParentFrm.Controls.OfType(Of Button)
            If Not ctl.Name = "Disconnect" AndAlso Not ctl.Name = "GetHelpManual" AndAlso Not ctl.Name = Nothing AndAlso Not ctl.Name = "BackBtn" Then
                SetControlGradient(ctl, ButtonColours)
                ctl.ForeColor = Color.FromArgb(139, 43, 110)
            End If
        Next
        SetControlGradient(ParentFrm, ColourArray)
        'paints custom form elements in a specific way, some buttons are not touched when painting
    End Sub

    Public Sub MsgBoxDesign(ParentFrm As Form) 'specific type of designload() function but for the custom msgboxes
        For Each ctl As Control In ParentFrm.Controls.OfType(Of Button)
            ctl.ForeColor = Color.Goldenrod
        Next
        SetControlGradient(ParentFrm, MsgBoxColours)
    End Sub


    Public Class ImagePopupForm
        Inherits Form
        'for wallet address QR code generation and showing, new addition to the project as an easter egg feature in WalletBaseView
        Friend WithEvents PictureBox1 As PictureBox

        Public Sub New(Title As String)
            ' Initialize the form and PictureBox
            InitializeComponent(Title)

        End Sub

        ' Method to show the form with an image
        Public Shared Sub ShowBox(ImageInstance As Image, Optional Title As String = "Picture")
            ' Set the image using the PictureBox control
            Dim Instance As New ImagePopupForm(Title)
            Instance.PictureBox1.Image = ImageInstance

            ' Auto-size the form based on the image size
            Instance.ClientSize = New Size(ImageInstance.Width, ImageInstance.Height)
            Instance.ShowDialog()
        End Sub

        Private Sub InitializeComponent(Title As string)
            ' InitializeComponent method to set up the form and controls
            Me.PictureBox1 = New PictureBox()
            Me.SuspendLayout()

            ' Set up PictureBox properties
            Me.PictureBox1.Dock = DockStyle.Fill
            Me.PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize

            ' Add PictureBox to the form
            Me.Controls.Add(Me.PictureBox1)

            ' Set up form properties
            Me.FormBorderStyle = FormBorderStyle.FixedSingle
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Text = Title
            Me.ResumeLayout(False)
        End Sub
    End Class
End Module
