<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TransactPoolView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TransactPoolView))
        Me.GetHelpManual = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Disconnect = New System.Windows.Forms.Button()
        Me.StatusLbl = New System.Windows.Forms.Label()
        Me.TransactTxt = New System.Windows.Forms.RichTextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.BackBtn = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GetHelpManual
        '
        Me.GetHelpManual.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GetHelpManual.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.GetHelpManual.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetHelpManual.Location = New System.Drawing.Point(8, 181)
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
        Me.Label1.Font = New System.Drawing.Font("Bahnschrift Light", 18.0!)
        Me.Label1.Location = New System.Drawing.Point(187, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(433, 102)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "LIVE TRANSACTION POOL - TRANSACTIONS TO BE CONFIRMED ONTO THE BLOCKCHAIN"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Disconnect
        '
        Me.Disconnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Disconnect.Font = New System.Drawing.Font("Bahnschrift", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Disconnect.ForeColor = System.Drawing.Color.Goldenrod
        Me.Disconnect.Location = New System.Drawing.Point(8, 357)
        Me.Disconnect.Name = "Disconnect"
        Me.Disconnect.Size = New System.Drawing.Size(455, 51)
        Me.Disconnect.TabIndex = 17
        Me.Disconnect.Text = "DISCONNECT FROM WAYFARER SYSTEM"
        Me.Disconnect.UseVisualStyleBackColor = False
        '
        'StatusLbl
        '
        Me.StatusLbl.BackColor = System.Drawing.Color.Ivory
        Me.StatusLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StatusLbl.Font = New System.Drawing.Font("Bahnschrift", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLbl.Location = New System.Drawing.Point(8, 285)
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(173, 66)
        Me.StatusLbl.TabIndex = 18
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TransactTxt
        '
        Me.TransactTxt.BackColor = System.Drawing.Color.Ivory
        Me.TransactTxt.Font = New System.Drawing.Font("Bahnschrift Light", 10.0!)
        Me.TransactTxt.ForeColor = System.Drawing.Color.Red
        Me.TransactTxt.Location = New System.Drawing.Point(189, 108)
        Me.TransactTxt.Name = "TransactTxt"
        Me.TransactTxt.ReadOnly = True
        Me.TransactTxt.Size = New System.Drawing.Size(431, 243)
        Me.TransactTxt.TabIndex = 23
        Me.TransactTxt.Text = resources.GetString("TransactTxt.Text")
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ProjectBlockchain.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(8, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(173, 172)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 24
        Me.PictureBox1.TabStop = False
        '
        'BackBtn
        '
        Me.BackBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.BackBtn.Font = New System.Drawing.Font("Bahnschrift", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackBtn.ForeColor = System.Drawing.Color.Goldenrod
        Me.BackBtn.Location = New System.Drawing.Point(469, 357)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(151, 51)
        Me.BackBtn.TabIndex = 34
        Me.BackBtn.Text = "BACK TO MENU"
        Me.BackBtn.UseVisualStyleBackColor = False
        '
        'TransactPoolView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(634, 420)
        Me.Controls.Add(Me.BackBtn)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TransactTxt)
        Me.Controls.Add(Me.StatusLbl)
        Me.Controls.Add(Me.Disconnect)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GetHelpManual)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "TransactPoolView"
        Me.Text = "TRANSACTION POOL VIEW"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GetHelpManual As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Disconnect As Button
    Friend WithEvents StatusLbl As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents BackBtn As Button
    Friend WithEvents TransactTxt As RichTextBox
End Class
