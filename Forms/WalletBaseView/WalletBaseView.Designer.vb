<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WalletBaseView
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WalletBaseView))
        Me.CreateWallet = New System.Windows.Forms.Button()
        Me.GetHelpManual = New System.Windows.Forms.Button()
        Me.TitleBar = New System.Windows.Forms.Label()
        Me.LoginWallet = New System.Windows.Forms.Button()
        Me.StatusLbl = New System.Windows.Forms.Label()
        Me.ViewWallets = New System.Windows.Forms.Button()
        Me.DeleteWallet = New System.Windows.Forms.Button()
        Me.Disconnect = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.BackBtn = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CreateWallet
        '
        Me.CreateWallet.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.CreateWallet.Font = New System.Drawing.Font("Bahnschrift Light", 10.0!)
        Me.CreateWallet.ForeColor = System.Drawing.Color.Goldenrod
        Me.CreateWallet.Location = New System.Drawing.Point(187, 134)
        Me.CreateWallet.Name = "CreateWallet"
        Me.CreateWallet.Size = New System.Drawing.Size(111, 101)
        Me.CreateWallet.TabIndex = 2
        Me.CreateWallet.Text = "GENERATE NEW WALLET"
        Me.CreateWallet.UseVisualStyleBackColor = False
        '
        'GetHelpManual
        '
        Me.GetHelpManual.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.GetHelpManual.Font = New System.Drawing.Font("Bahnschrift Light", 12.0!)
        Me.GetHelpManual.ForeColor = System.Drawing.Color.Goldenrod
        Me.GetHelpManual.Location = New System.Drawing.Point(10, 187)
        Me.GetHelpManual.Name = "GetHelpManual"
        Me.GetHelpManual.Size = New System.Drawing.Size(169, 48)
        Me.GetHelpManual.TabIndex = 3
        Me.GetHelpManual.Text = "ABOUT THE WAYFARER SYSTEM"
        Me.GetHelpManual.UseVisualStyleBackColor = False
        '
        'TitleBar
        '
        Me.TitleBar.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.TitleBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TitleBar.Font = New System.Drawing.Font("Bahnschrift Light", 18.0!)
        Me.TitleBar.Location = New System.Drawing.Point(187, 9)
        Me.TitleBar.Name = "TitleBar"
        Me.TitleBar.Size = New System.Drawing.Size(497, 67)
        Me.TitleBar.TabIndex = 5
        Me.TitleBar.Text = "WALLET MANAGEMENT - LOGIN, GENERATE OR DELETE WALLET KEY-PAIRS"
        Me.TitleBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LoginWallet
        '
        Me.LoginWallet.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.LoginWallet.Font = New System.Drawing.Font("Bahnschrift Light", 10.0!)
        Me.LoginWallet.ForeColor = System.Drawing.Color.Goldenrod
        Me.LoginWallet.Location = New System.Drawing.Point(317, 135)
        Me.LoginWallet.Name = "LoginWallet"
        Me.LoginWallet.Size = New System.Drawing.Size(110, 100)
        Me.LoginWallet.TabIndex = 6
        Me.LoginWallet.Text = "LOG INTO EXISTING WALLET"
        Me.LoginWallet.UseVisualStyleBackColor = False
        '
        'StatusLbl
        '
        Me.StatusLbl.BackColor = System.Drawing.SystemColors.Control
        Me.StatusLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.StatusLbl.Font = New System.Drawing.Font("Bahnschrift Light", 8.0!)
        Me.StatusLbl.Location = New System.Drawing.Point(10, 242)
        Me.StatusLbl.Name = "StatusLbl"
        Me.StatusLbl.Size = New System.Drawing.Size(169, 71)
        Me.StatusLbl.TabIndex = 18
        Me.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ViewWallets
        '
        Me.ViewWallets.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ViewWallets.Font = New System.Drawing.Font("Bahnschrift Light", 10.0!)
        Me.ViewWallets.ForeColor = System.Drawing.Color.Goldenrod
        Me.ViewWallets.Location = New System.Drawing.Point(447, 134)
        Me.ViewWallets.Name = "ViewWallets"
        Me.ViewWallets.Size = New System.Drawing.Size(108, 101)
        Me.ViewWallets.TabIndex = 20
        Me.ViewWallets.Text = "VIEW EXISTING WALLETS"
        Me.ViewWallets.UseVisualStyleBackColor = False
        '
        'DeleteWallet
        '
        Me.DeleteWallet.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DeleteWallet.Font = New System.Drawing.Font("Bahnschrift Light", 10.0!)
        Me.DeleteWallet.ForeColor = System.Drawing.Color.Goldenrod
        Me.DeleteWallet.Location = New System.Drawing.Point(572, 134)
        Me.DeleteWallet.Name = "DeleteWallet"
        Me.DeleteWallet.Size = New System.Drawing.Size(112, 100)
        Me.DeleteWallet.TabIndex = 21
        Me.DeleteWallet.Text = "DELETE EXISTING WALLET"
        Me.DeleteWallet.UseVisualStyleBackColor = False
        '
        'Disconnect
        '
        Me.Disconnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.Disconnect.Font = New System.Drawing.Font("Bahnschrift Light", 10.0!)
        Me.Disconnect.ForeColor = System.Drawing.Color.Goldenrod
        Me.Disconnect.Location = New System.Drawing.Point(187, 244)
        Me.Disconnect.Name = "Disconnect"
        Me.Disconnect.Size = New System.Drawing.Size(368, 69)
        Me.Disconnect.TabIndex = 17
        Me.Disconnect.Text = "DISCONNECT FROM WAYFARER SYSTEM"
        Me.Disconnect.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.ProjectBlockchain.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(10, 10)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(169, 171)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 27
        Me.PictureBox1.TabStop = False
        '
        'BackBtn
        '
        Me.BackBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(50, Byte), Integer))
        Me.BackBtn.Font = New System.Drawing.Font("Bahnschrift", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BackBtn.ForeColor = System.Drawing.Color.Goldenrod
        Me.BackBtn.Location = New System.Drawing.Point(572, 244)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(112, 69)
        Me.BackBtn.TabIndex = 33
        Me.BackBtn.Text = "BACK TO MENU"
        Me.BackBtn.UseVisualStyleBackColor = False
        '
        'WalletBaseView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(696, 322)
        Me.Controls.Add(Me.BackBtn)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.DeleteWallet)
        Me.Controls.Add(Me.ViewWallets)
        Me.Controls.Add(Me.StatusLbl)
        Me.Controls.Add(Me.Disconnect)
        Me.Controls.Add(Me.LoginWallet)
        Me.Controls.Add(Me.TitleBar)
        Me.Controls.Add(Me.GetHelpManual)
        Me.Controls.Add(Me.CreateWallet)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "WalletBaseView"
        Me.Text = "WALLET MANAGEMENT"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CreateWallet As Button
    Friend WithEvents GetHelpManual As Button
    Friend WithEvents TitleBar As Label
    Friend WithEvents LoginWallet As Button
    Friend WithEvents StatusLbl As Label
    Friend WithEvents ViewWallets As Button
    Friend WithEvents DeleteWallet As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Disconnect As Button
    Friend WithEvents BackBtn As Button
End Class
