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
        Me.CheckConfirm = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BackBtn = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.AddressInput = New System.Windows.Forms.Button()
        Me.AmountInput = New System.Windows.Forms.Button()
        Me.AddressInputTxt = New System.Windows.Forms.TextBox()
        Me.SendingAmountTxt = New System.Windows.Forms.TextBox()
        Me.TransactionStatus = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GetHelpManual
        '
        Me.GetHelpManual.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GetHelpManual.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.GetHelpManual.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetHelpManual.Location = New System.Drawing.Point(8, 205)
        Me.GetHelpManual.Name = "GetHelpManual"
        Me.GetHelpManual.Size = New System.Drawing.Size(208, 51)
        Me.GetHelpManual.TabIndex = 3
        Me.GetHelpManual.Text = "ABOUT THE WAYFARER SYSTEM"
        Me.GetHelpManual.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Bahnschrift Light", 14.0!)
        Me.Label1.Location = New System.Drawing.Point(222, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(647, 55)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "SENDING CRYPTOCURRENCY ACROSS THE WAYFARER NETWORK"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Disconnect
        '
        Me.Disconnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Disconnect.Font = New System.Drawing.Font("Bahnschrift", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Disconnect.ForeColor = System.Drawing.Color.Goldenrod
        Me.Disconnect.Location = New System.Drawing.Point(222, 310)
        Me.Disconnect.Name = "Disconnect"
        Me.Disconnect.Size = New System.Drawing.Size(309, 52)
        Me.Disconnect.TabIndex = 17
        Me.Disconnect.Text = "DISCONNECT FROM WAYFARER SYSTEM"
        Me.Disconnect.UseVisualStyleBackColor = False
        '
        'StatusLbl
        '
        Me.StatusLbl.BackColor = System.Drawing.Color.Ivory
        Me.StatusLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StatusLbl.Font = New System.Drawing.Font("Bahnschrift", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLbl.Location = New System.Drawing.Point(8, 259)
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(208, 106)
        Me.StatusLbl.TabIndex = 18
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CheckConfirm
        '
        Me.CheckConfirm.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.CheckConfirm.Font = New System.Drawing.Font("Bahnschrift", 10.0!, System.Drawing.FontStyle.Bold)
        Me.CheckConfirm.ForeColor = System.Drawing.Color.Honeydew
        Me.CheckConfirm.Location = New System.Drawing.Point(222, 259)
        Me.CheckConfirm.Name = "CheckConfirm"
        Me.CheckConfirm.Size = New System.Drawing.Size(452, 45)
        Me.CheckConfirm.TabIndex = 28
        Me.CheckConfirm.Text = "CHECK DETAILS AND SEND TRANSACTION TO THE POOL"
        Me.CheckConfirm.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.SaddleBrown
        Me.Label2.Location = New System.Drawing.Point(222, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(138, 48)
        Me.Label2.TabIndex = 30
        Me.Label2.Text = "ENTER RECIPIENT'S PUBLIC ADDRESS"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.SaddleBrown
        Me.Label3.Location = New System.Drawing.Point(222, 152)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(138, 47)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "ENTER AMOUNT TO SEND HERE"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BackBtn
        '
        Me.BackBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.BackBtn.Font = New System.Drawing.Font("Bahnschrift", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackBtn.ForeColor = System.Drawing.Color.Goldenrod
        Me.BackBtn.Location = New System.Drawing.Point(537, 310)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(137, 52)
        Me.BackBtn.TabIndex = 32
        Me.BackBtn.Text = "BACK TO MENU"
        Me.BackBtn.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ProjectBlockchain.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(8, 7)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(208, 192)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 29
        Me.PictureBox1.TabStop = False
        '
        'AddressInput
        '
        Me.AddressInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.AddressInput.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddressInput.ForeColor = System.Drawing.Color.Goldenrod
        Me.AddressInput.Location = New System.Drawing.Point(366, 68)
        Me.AddressInput.Name = "AddressInput"
        Me.AddressInput.Size = New System.Drawing.Size(503, 48)
        Me.AddressInput.TabIndex = 35
        Me.AddressInput.Text = "CLICK HERE TO ENTER ADDRESS"
        Me.AddressInput.UseVisualStyleBackColor = False
        '
        'AmountInput
        '
        Me.AmountInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.AmountInput.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AmountInput.ForeColor = System.Drawing.Color.Goldenrod
        Me.AmountInput.Location = New System.Drawing.Point(366, 152)
        Me.AmountInput.Name = "AmountInput"
        Me.AmountInput.Size = New System.Drawing.Size(503, 47)
        Me.AmountInput.TabIndex = 36
        Me.AmountInput.Text = "CLICK HERE TO ENTER THE AMOUNT"
        Me.AmountInput.UseVisualStyleBackColor = False
        '
        'AddressInputTxt
        '
        Me.AddressInputTxt.Font = New System.Drawing.Font("Lucida Console", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AddressInputTxt.ForeColor = System.Drawing.Color.Navy
        Me.AddressInputTxt.Location = New System.Drawing.Point(222, 122)
        Me.AddressInputTxt.Name = "AddressInputTxt"
        Me.AddressInputTxt.ReadOnly = True
        Me.AddressInputTxt.Size = New System.Drawing.Size(647, 23)
        Me.AddressInputTxt.TabIndex = 37
        '
        'SendingAmountTxt
        '
        Me.SendingAmountTxt.Font = New System.Drawing.Font("Lucida Console", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SendingAmountTxt.ForeColor = System.Drawing.Color.Navy
        Me.SendingAmountTxt.Location = New System.Drawing.Point(222, 206)
        Me.SendingAmountTxt.Name = "SendingAmountTxt"
        Me.SendingAmountTxt.ReadOnly = True
        Me.SendingAmountTxt.Size = New System.Drawing.Size(647, 23)
        Me.SendingAmountTxt.TabIndex = 38
        '
        'TransactionStatus
        '
        Me.TransactionStatus.BackColor = System.Drawing.Color.Ivory
        Me.TransactionStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TransactionStatus.Font = New System.Drawing.Font("Bahnschrift", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TransactionStatus.Location = New System.Drawing.Point(680, 235)
        Me.TransactionStatus.Name = "TransactionStatus"
        Me.TransactionStatus.Size = New System.Drawing.Size(189, 127)
        Me.TransactionStatus.TabIndex = 39
        Me.TransactionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SendingScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(881, 374)
        Me.Controls.Add(Me.TransactionStatus)
        Me.Controls.Add(Me.SendingAmountTxt)
        Me.Controls.Add(Me.AddressInputTxt)
        Me.Controls.Add(Me.AmountInput)
        Me.Controls.Add(Me.AddressInput)
        Me.Controls.Add(Me.BackBtn)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.CheckConfirm)
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
        Me.PerformLayout()

    End Sub
    Friend WithEvents GetHelpManual As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Disconnect As Button
    Friend WithEvents StatusLbl As Label
    Friend WithEvents CheckConfirm As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents BackBtn As Button
    Friend WithEvents AddressInput As Button
    Friend WithEvents AmountInput As Button
    Friend WithEvents SendingAmountTxt As TextBox
    Friend WithEvents TransactionStatus As Label
    Friend WithEvents AddressInputTxt As TextBox
End Class
