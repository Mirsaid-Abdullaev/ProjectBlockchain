<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainMenu))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GetWalletView = New System.Windows.Forms.Button()
        Me.GetTransactionPool = New System.Windows.Forms.Button()
        Me.GetViewingForm = New System.Windows.Forms.Button()
        Me.GetSendingView = New System.Windows.Forms.Button()
        Me.StatusLbl = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.GetHelpManual = New System.Windows.Forms.Button()
        Me.Disconnect = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Bahnschrift Light", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(256, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(537, 92)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "MAIN MENU: CHOOSE AN OPTION TO CONTINUE WITH"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GetWalletView
        '
        Me.GetWalletView.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GetWalletView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GetWalletView.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetWalletView.Location = New System.Drawing.Point(256, 117)
        Me.GetWalletView.Name = "GetWalletView"
        Me.GetWalletView.Size = New System.Drawing.Size(261, 121)
        Me.GetWalletView.TabIndex = 7
        Me.GetWalletView.Text = "WALLET MANAGEMENT"
        Me.GetWalletView.UseVisualStyleBackColor = False
        '
        'GetTransactionPool
        '
        Me.GetTransactionPool.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GetTransactionPool.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GetTransactionPool.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetTransactionPool.Location = New System.Drawing.Point(532, 249)
        Me.GetTransactionPool.Name = "GetTransactionPool"
        Me.GetTransactionPool.Size = New System.Drawing.Size(261, 121)
        Me.GetTransactionPool.TabIndex = 10
        Me.GetTransactionPool.Text = "VIEW LIVE TRANSACTION POOL"
        Me.GetTransactionPool.UseVisualStyleBackColor = False
        '
        'GetViewingForm
        '
        Me.GetViewingForm.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GetViewingForm.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GetViewingForm.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetViewingForm.Location = New System.Drawing.Point(256, 249)
        Me.GetViewingForm.Name = "GetViewingForm"
        Me.GetViewingForm.Size = New System.Drawing.Size(261, 121)
        Me.GetViewingForm.TabIndex = 13
        Me.GetViewingForm.Text = "BLOCKCHAIN EXPLORER"
        Me.GetViewingForm.UseVisualStyleBackColor = False
        '
        'GetSendingView
        '
        Me.GetSendingView.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.GetSendingView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GetSendingView.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetSendingView.Location = New System.Drawing.Point(532, 117)
        Me.GetSendingView.Name = "GetSendingView"
        Me.GetSendingView.Size = New System.Drawing.Size(261, 121)
        Me.GetSendingView.TabIndex = 11
        Me.GetSendingView.Text = "SEND CRYPTOCURRENCY TO WALLET"
        Me.GetSendingView.UseVisualStyleBackColor = False
        '
        'StatusLbl
        '
        Me.StatusLbl.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StatusLbl.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.StatusLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLbl.Location = New System.Drawing.Point(14, 301)
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(236, 150)
        Me.StatusLbl.TabIndex = 18
        Me.StatusLbl.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ProjectBlockchain.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(238, 214)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 20
        Me.PictureBox1.TabStop = False
        '
        'GetHelpManual
        '
        Me.GetHelpManual.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GetHelpManual.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.GetHelpManual.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetHelpManual.Location = New System.Drawing.Point(12, 232)
        Me.GetHelpManual.Name = "GetHelpManual"
        Me.GetHelpManual.Size = New System.Drawing.Size(238, 66)
        Me.GetHelpManual.TabIndex = 21
        Me.GetHelpManual.Text = "ABOUT THE WAYFARER SYSTEM"
        Me.GetHelpManual.UseVisualStyleBackColor = False
        '
        'Disconnect
        '
        Me.Disconnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Disconnect.Font = New System.Drawing.Font("Bahnschrift", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Disconnect.ForeColor = System.Drawing.Color.Goldenrod
        Me.Disconnect.Location = New System.Drawing.Point(256, 376)
        Me.Disconnect.Name = "Disconnect"
        Me.Disconnect.Size = New System.Drawing.Size(537, 75)
        Me.Disconnect.TabIndex = 22
        Me.Disconnect.Text = "DISCONNECT FROM WAYFARER SYSTEM"
        Me.Disconnect.UseVisualStyleBackColor = False
        '
        'MainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(806, 465)
        Me.Controls.Add(Me.Disconnect)
        Me.Controls.Add(Me.GetHelpManual)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.StatusLbl)
        Me.Controls.Add(Me.GetViewingForm)
        Me.Controls.Add(Me.GetSendingView)
        Me.Controls.Add(Me.GetTransactionPool)
        Me.Controls.Add(Me.GetWalletView)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "MainMenu"
        Me.Text = "MAIN MENU"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As Label
    Friend WithEvents GetWalletView As Button
    Friend WithEvents GetTransactionPool As Button
    Friend WithEvents GetViewingForm As Button
    Friend WithEvents GetSendingView As Button
    Friend WithEvents StatusLbl As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents GetHelpManual As Button
    Friend WithEvents Disconnect As Button
End Class
