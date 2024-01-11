<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SendingScreen
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SendingScreen))
        Me.GetHelpManual = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Disconnect = New System.Windows.Forms.Button()
        Me.StatusLbl = New System.Windows.Forms.Label()
        Me.RecipientAddressTxt = New System.Windows.Forms.RichTextBox()
        Me.SendingAmountTxt = New System.Windows.Forms.RichTextBox()
        Me.CheckConfirm = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BackBtn = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GetHelpManual
        '
        Me.GetHelpManual.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GetHelpManual.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.GetHelpManual.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetHelpManual.Location = New System.Drawing.Point(25, 196)
        Me.GetHelpManual.Name = "GetHelpManual"
        Me.GetHelpManual.Size = New System.Drawing.Size(173, 51)
        Me.GetHelpManual.TabIndex = 3
        Me.GetHelpManual.Text = "ABOUT THE WAYFARER SYSTEM"
        Me.GetHelpManual.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Bahnschrift Light", 14.0!)
        Me.Label1.Location = New System.Drawing.Point(204, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(419, 55)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "SENDING CRYPTOCURRENCY ACROSS THE WAYFARER NETWORK"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Disconnect
        '
        Me.Disconnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Disconnect.Font = New System.Drawing.Font("Bahnschrift", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Disconnect.ForeColor = System.Drawing.Color.Goldenrod
        Me.Disconnect.Location = New System.Drawing.Point(204, 257)
        Me.Disconnect.Name = "Disconnect"
        Me.Disconnect.Size = New System.Drawing.Size(314, 66)
        Me.Disconnect.TabIndex = 17
        Me.Disconnect.Text = "DISCONNECT FROM WAYFARER SYSTEM"
        Me.Disconnect.UseVisualStyleBackColor = False
        '
        'StatusLbl
        '
        Me.StatusLbl.BackColor = System.Drawing.Color.Ivory
        Me.StatusLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StatusLbl.Font = New System.Drawing.Font("Bahnschrift", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLbl.Location = New System.Drawing.Point(25, 257)
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(173, 66)
        Me.StatusLbl.TabIndex = 18
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RecipientAddressTxt
        '
        Me.RecipientAddressTxt.BackColor = System.Drawing.Color.Ivory
        Me.RecipientAddressTxt.Font = New System.Drawing.Font("Bahnschrift Light", 10.0!)
        Me.RecipientAddressTxt.ForeColor = System.Drawing.Color.Red
        Me.RecipientAddressTxt.Location = New System.Drawing.Point(350, 75)
        Me.RecipientAddressTxt.Name = "RecipientAddressTxt"
        Me.RecipientAddressTxt.Size = New System.Drawing.Size(273, 53)
        Me.RecipientAddressTxt.TabIndex = 22
        Me.RecipientAddressTxt.Text = "[User enters recipient address here]"
        '
        'SendingAmountTxt
        '
        Me.SendingAmountTxt.BackColor = System.Drawing.Color.Ivory
        Me.SendingAmountTxt.Font = New System.Drawing.Font("Bahnschrift Light", 10.0!)
        Me.SendingAmountTxt.ForeColor = System.Drawing.Color.Red
        Me.SendingAmountTxt.Location = New System.Drawing.Point(350, 133)
        Me.SendingAmountTxt.Name = "SendingAmountTxt"
        Me.SendingAmountTxt.Size = New System.Drawing.Size(273, 66)
        Me.SendingAmountTxt.TabIndex = 23
        Me.SendingAmountTxt.Text = "[Enter an amount to send - must be less than or equal to balance.]"
        '
        'CheckConfirm
        '
        Me.CheckConfirm.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.CheckConfirm.Font = New System.Drawing.Font("Bahnschrift", 10.0!, System.Drawing.FontStyle.Bold)
        Me.CheckConfirm.ForeColor = System.Drawing.Color.Honeydew
        Me.CheckConfirm.Location = New System.Drawing.Point(204, 206)
        Me.CheckConfirm.Name = "CheckConfirm"
        Me.CheckConfirm.Size = New System.Drawing.Size(419, 35)
        Me.CheckConfirm.TabIndex = 28
        Me.CheckConfirm.Text = "CHECK DETAILS AND SEND TRANSACTION TO THE POOL"
        Me.CheckConfirm.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(139, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(206, 75)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(138, 53)
        Me.Label2.TabIndex = 30
        Me.Label2.Text = "ENTER RECIPIENT'S PUBLIC ADDRESS"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(139, Byte), Integer), CType(CType(43, Byte), Integer), CType(CType(110, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(206, 133)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(138, 66)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "ENTER AMOUNT TO SEND HERE - MUST BE LESS THAN BALANCE"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BackBtn
        '
        Me.BackBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.BackBtn.Font = New System.Drawing.Font("Bahnschrift", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackBtn.ForeColor = System.Drawing.Color.Goldenrod
        Me.BackBtn.Location = New System.Drawing.Point(524, 257)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(99, 66)
        Me.BackBtn.TabIndex = 32
        Me.BackBtn.Text = "BACK TO MENU"
        Me.BackBtn.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ProjectBlockchain.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(25, 14)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(173, 172)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 29
        Me.PictureBox1.TabStop = False
        '
        'SendingScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(648, 343)
        Me.Controls.Add(Me.BackBtn)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.CheckConfirm)
        Me.Controls.Add(Me.SendingAmountTxt)
        Me.Controls.Add(Me.RecipientAddressTxt)
        Me.Controls.Add(Me.StatusLbl)
        Me.Controls.Add(Me.Disconnect)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GetHelpManual)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "SendingScreen"
        Me.Text = "SENDING CRYPTOCURRENCY"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GetHelpManual As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Disconnect As Button
    Friend WithEvents StatusLbl As Label
    Friend WithEvents RecipientAddressTxt As RichTextBox
    Friend WithEvents SendingAmountTxt As RichTextBox
    Friend WithEvents CheckConfirm As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents BackBtn As Button
End Class
