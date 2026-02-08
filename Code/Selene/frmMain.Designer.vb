<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.scEvenOuter = New System.Windows.Forms.SplitContainer()
        Me.scMainOuter = New System.Windows.Forms.SplitContainer()
        Me.pnlSidebar = New System.Windows.Forms.Panel()
        Me.flpMacros = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnMacro1 = New System.Windows.Forms.Button()
        Me.btnMacro2 = New System.Windows.Forms.Button()
        Me.btnMacro3 = New System.Windows.Forms.Button()
        Me.btnMacro4 = New System.Windows.Forms.Button()
        Me.btnMacro5 = New System.Windows.Forms.Button()
        Me.btnMacro6 = New System.Windows.Forms.Button()
        Me.btnMacro7 = New System.Windows.Forms.Button()
        Me.btnMacro8 = New System.Windows.Forms.Button()
        Me.btnMacro9 = New System.Windows.Forms.Button()
        Me.btnMacro10 = New System.Windows.Forms.Button()
        Me.btnMacro11 = New System.Windows.Forms.Button()
        Me.btnMacro12 = New System.Windows.Forms.Button()
        Me.grpConnection = New System.Windows.Forms.GroupBox()
        Me.lblStatusSquare = New System.Windows.Forms.Label()
        Me.lblRX = New System.Windows.Forms.Label()
        Me.lblTX = New System.Windows.Forms.Label()
        Me.btnRefreshPorts = New System.Windows.Forms.Button()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.cmbBaud = New System.Windows.Forms.ComboBox()
        Me.cmbPorts = New System.Windows.Forms.ComboBox()
        Me.scWorkArea = New System.Windows.Forms.SplitContainer()
        Me.tcCodeEditors = New System.Windows.Forms.TabControl()
        Me.pnlQuickCmd = New System.Windows.Forms.Panel()
        Me.rtbxTerminal = New System.Windows.Forms.RichTextBox()
        Me.grpManageFileSystem = New System.Windows.Forms.GroupBox()
        Me.btnListFiles = New System.Windows.Forms.Button()
        Me.btnFormat = New System.Windows.Forms.Button()
        Me.grpRawCommnds = New System.Windows.Forms.GroupBox()
        Me.lnkEditList = New System.Windows.Forms.LinkLabel()
        Me.lnkAddToList = New System.Windows.Forms.LinkLabel()
        Me.cmbRawCommands = New System.Windows.Forms.ComboBox()
        Me.btnSendRaw = New System.Windows.Forms.Button()
        Me.lnkClearResponse = New System.Windows.Forms.LinkLabel()
        Me.lnkDetachResponse = New System.Windows.Forms.LinkLabel()
        Me.grpRun = New System.Windows.Forms.GroupBox()
        Me.chkUncommentCode = New System.Windows.Forms.CheckBox()
        Me.btnRunSelectedCode = New System.Windows.Forms.Button()
        Me.grpSearch = New System.Windows.Forms.GroupBox()
        Me.btnSearchOnCode = New System.Windows.Forms.Button()
        Me.txtSeacrhTerm = New System.Windows.Forms.TextBox()
        Me.grpFileActions = New System.Windows.Forms.GroupBox()
        Me.chkAutoExecute = New System.Windows.Forms.CheckBox()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.txtfile = New System.Windows.Forms.TextBox()
        Me.btnRead = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnWrite = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveAsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AutoConnectOnStartupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreSessionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ThemesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NextThemeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PrevToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectThemeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DefaultToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.DarkMatterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BeehiveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ThinkGreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.IntoTheLightToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MeanOrangeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HappyDayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SoLimboToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OldCoffeeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PaintedStaiwayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VisitWebsiteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblCurrentThem = New System.Windows.Forms.Label()
        Me.lblWorkingFile = New System.Windows.Forms.Label()
        Me.tmrIndicatorReset = New System.Windows.Forms.Timer(Me.components)
        Me.tmrReadTimeout = New System.Windows.Forms.Timer(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblCurrentTheme = New System.Windows.Forms.Label()
        Me.VisitGithubRepoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FirmwareFlasherToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.scEvenOuter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scEvenOuter.Panel1.SuspendLayout()
        Me.scEvenOuter.Panel2.SuspendLayout()
        Me.scEvenOuter.SuspendLayout()
        CType(Me.scMainOuter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMainOuter.Panel1.SuspendLayout()
        Me.scMainOuter.Panel2.SuspendLayout()
        Me.scMainOuter.SuspendLayout()
        Me.pnlSidebar.SuspendLayout()
        Me.flpMacros.SuspendLayout()
        Me.grpConnection.SuspendLayout()
        CType(Me.scWorkArea, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scWorkArea.Panel1.SuspendLayout()
        Me.scWorkArea.Panel2.SuspendLayout()
        Me.scWorkArea.SuspendLayout()
        Me.pnlQuickCmd.SuspendLayout()
        Me.grpManageFileSystem.SuspendLayout()
        Me.grpRawCommnds.SuspendLayout()
        Me.grpRun.SuspendLayout()
        Me.grpSearch.SuspendLayout()
        Me.grpFileActions.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'scEvenOuter
        '
        Me.scEvenOuter.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scEvenOuter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scEvenOuter.Location = New System.Drawing.Point(0, 27)
        Me.scEvenOuter.Name = "scEvenOuter"
        Me.scEvenOuter.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scEvenOuter.Panel1
        '
        Me.scEvenOuter.Panel1.Controls.Add(Me.scMainOuter)
        '
        'scEvenOuter.Panel2
        '
        Me.scEvenOuter.Panel2.Controls.Add(Me.grpManageFileSystem)
        Me.scEvenOuter.Panel2.Controls.Add(Me.grpRawCommnds)
        Me.scEvenOuter.Panel2.Controls.Add(Me.lnkClearResponse)
        Me.scEvenOuter.Panel2.Controls.Add(Me.lnkDetachResponse)
        Me.scEvenOuter.Panel2.Controls.Add(Me.grpRun)
        Me.scEvenOuter.Panel2.Controls.Add(Me.grpSearch)
        Me.scEvenOuter.Panel2.Controls.Add(Me.grpFileActions)
        Me.scEvenOuter.Panel2MinSize = 130
        Me.scEvenOuter.Size = New System.Drawing.Size(1264, 734)
        Me.scEvenOuter.SplitterDistance = 600
        Me.scEvenOuter.TabIndex = 1
        '
        'scMainOuter
        '
        Me.scMainOuter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMainOuter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scMainOuter.Location = New System.Drawing.Point(0, 0)
        Me.scMainOuter.Name = "scMainOuter"
        '
        'scMainOuter.Panel1
        '
        Me.scMainOuter.Panel1.Controls.Add(Me.pnlSidebar)
        '
        'scMainOuter.Panel2
        '
        Me.scMainOuter.Panel2.Controls.Add(Me.scWorkArea)
        Me.scMainOuter.Size = New System.Drawing.Size(1264, 600)
        Me.scMainOuter.SplitterDistance = 150
        Me.scMainOuter.TabIndex = 0
        '
        'pnlSidebar
        '
        Me.pnlSidebar.Controls.Add(Me.flpMacros)
        Me.pnlSidebar.Controls.Add(Me.grpConnection)
        Me.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSidebar.Location = New System.Drawing.Point(0, 0)
        Me.pnlSidebar.Name = "pnlSidebar"
        Me.pnlSidebar.Size = New System.Drawing.Size(150, 600)
        Me.pnlSidebar.TabIndex = 0
        '
        'flpMacros
        '
        Me.flpMacros.AutoScroll = True
        Me.flpMacros.Controls.Add(Me.btnMacro1)
        Me.flpMacros.Controls.Add(Me.btnMacro2)
        Me.flpMacros.Controls.Add(Me.btnMacro3)
        Me.flpMacros.Controls.Add(Me.btnMacro4)
        Me.flpMacros.Controls.Add(Me.btnMacro5)
        Me.flpMacros.Controls.Add(Me.btnMacro6)
        Me.flpMacros.Controls.Add(Me.btnMacro7)
        Me.flpMacros.Controls.Add(Me.btnMacro8)
        Me.flpMacros.Controls.Add(Me.btnMacro9)
        Me.flpMacros.Controls.Add(Me.btnMacro10)
        Me.flpMacros.Controls.Add(Me.btnMacro11)
        Me.flpMacros.Controls.Add(Me.btnMacro12)
        Me.flpMacros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpMacros.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpMacros.Location = New System.Drawing.Point(0, 140)
        Me.flpMacros.Name = "flpMacros"
        Me.flpMacros.Padding = New System.Windows.Forms.Padding(5)
        Me.flpMacros.Size = New System.Drawing.Size(150, 460)
        Me.flpMacros.TabIndex = 1
        Me.flpMacros.WrapContents = False
        '
        'btnMacro1
        '
        Me.btnMacro1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro1.Location = New System.Drawing.Point(8, 8)
        Me.btnMacro1.Name = "btnMacro1"
        Me.btnMacro1.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro1.TabIndex = 0
        Me.btnMacro1.Text = "Macro 1"
        Me.btnMacro1.UseVisualStyleBackColor = True
        '
        'btnMacro2
        '
        Me.btnMacro2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro2.Location = New System.Drawing.Point(8, 49)
        Me.btnMacro2.Name = "btnMacro2"
        Me.btnMacro2.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro2.TabIndex = 1
        Me.btnMacro2.Text = "Macro 2"
        Me.btnMacro2.UseVisualStyleBackColor = True
        '
        'btnMacro3
        '
        Me.btnMacro3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro3.Location = New System.Drawing.Point(8, 90)
        Me.btnMacro3.Name = "btnMacro3"
        Me.btnMacro3.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro3.TabIndex = 2
        Me.btnMacro3.Text = "Macro 3"
        Me.btnMacro3.UseVisualStyleBackColor = True
        '
        'btnMacro4
        '
        Me.btnMacro4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro4.Location = New System.Drawing.Point(8, 131)
        Me.btnMacro4.Name = "btnMacro4"
        Me.btnMacro4.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro4.TabIndex = 3
        Me.btnMacro4.Text = "Macro 4"
        Me.btnMacro4.UseVisualStyleBackColor = True
        '
        'btnMacro5
        '
        Me.btnMacro5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro5.Location = New System.Drawing.Point(8, 172)
        Me.btnMacro5.Name = "btnMacro5"
        Me.btnMacro5.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro5.TabIndex = 4
        Me.btnMacro5.Text = "Macro 5"
        Me.btnMacro5.UseVisualStyleBackColor = True
        '
        'btnMacro6
        '
        Me.btnMacro6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro6.Location = New System.Drawing.Point(8, 213)
        Me.btnMacro6.Name = "btnMacro6"
        Me.btnMacro6.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro6.TabIndex = 5
        Me.btnMacro6.Text = "Macro 6"
        Me.btnMacro6.UseVisualStyleBackColor = True
        '
        'btnMacro7
        '
        Me.btnMacro7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro7.Location = New System.Drawing.Point(8, 254)
        Me.btnMacro7.Name = "btnMacro7"
        Me.btnMacro7.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro7.TabIndex = 6
        Me.btnMacro7.Text = "Macro 7"
        Me.btnMacro7.UseVisualStyleBackColor = True
        '
        'btnMacro8
        '
        Me.btnMacro8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro8.Location = New System.Drawing.Point(8, 295)
        Me.btnMacro8.Name = "btnMacro8"
        Me.btnMacro8.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro8.TabIndex = 7
        Me.btnMacro8.Text = "Macro 8"
        Me.btnMacro8.UseVisualStyleBackColor = True
        '
        'btnMacro9
        '
        Me.btnMacro9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro9.Location = New System.Drawing.Point(8, 336)
        Me.btnMacro9.Name = "btnMacro9"
        Me.btnMacro9.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro9.TabIndex = 8
        Me.btnMacro9.Text = "Macro 9"
        Me.btnMacro9.UseVisualStyleBackColor = True
        '
        'btnMacro10
        '
        Me.btnMacro10.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro10.Location = New System.Drawing.Point(8, 377)
        Me.btnMacro10.Name = "btnMacro10"
        Me.btnMacro10.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro10.TabIndex = 9
        Me.btnMacro10.Text = "Macro 10"
        Me.btnMacro10.UseVisualStyleBackColor = True
        '
        'btnMacro11
        '
        Me.btnMacro11.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMacro11.Location = New System.Drawing.Point(8, 418)
        Me.btnMacro11.Name = "btnMacro11"
        Me.btnMacro11.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro11.TabIndex = 10
        Me.btnMacro11.Text = "Macro 11"
        Me.btnMacro11.UseVisualStyleBackColor = True
        '
        'btnMacro12
        '
        Me.btnMacro12.Location = New System.Drawing.Point(8, 459)
        Me.btnMacro12.Name = "btnMacro12"
        Me.btnMacro12.Size = New System.Drawing.Size(115, 35)
        Me.btnMacro12.TabIndex = 11
        Me.btnMacro12.Text = "Macro 12"
        Me.btnMacro12.UseVisualStyleBackColor = True
        '
        'grpConnection
        '
        Me.grpConnection.Controls.Add(Me.lblStatusSquare)
        Me.grpConnection.Controls.Add(Me.lblRX)
        Me.grpConnection.Controls.Add(Me.lblTX)
        Me.grpConnection.Controls.Add(Me.btnRefreshPorts)
        Me.grpConnection.Controls.Add(Me.btnConnect)
        Me.grpConnection.Controls.Add(Me.cmbBaud)
        Me.grpConnection.Controls.Add(Me.cmbPorts)
        Me.grpConnection.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpConnection.Location = New System.Drawing.Point(0, 0)
        Me.grpConnection.Name = "grpConnection"
        Me.grpConnection.Size = New System.Drawing.Size(150, 140)
        Me.grpConnection.TabIndex = 0
        Me.grpConnection.TabStop = False
        Me.grpConnection.Text = "Connection"
        '
        'lblStatusSquare
        '
        Me.lblStatusSquare.BackColor = System.Drawing.Color.Maroon
        Me.lblStatusSquare.Location = New System.Drawing.Point(110, 85)
        Me.lblStatusSquare.Name = "lblStatusSquare"
        Me.lblStatusSquare.Size = New System.Drawing.Size(15, 15)
        Me.lblStatusSquare.TabIndex = 6
        '
        'lblRX
        '
        Me.lblRX.BackColor = System.Drawing.Color.DimGray
        Me.lblRX.Font = New System.Drawing.Font("Microsoft Sans Serif", 5.25!)
        Me.lblRX.ForeColor = System.Drawing.Color.White
        Me.lblRX.Location = New System.Drawing.Point(85, 110)
        Me.lblRX.Name = "lblRX"
        Me.lblRX.Size = New System.Drawing.Size(15, 15)
        Me.lblRX.TabIndex = 5
        Me.lblRX.Text = "RX"
        Me.lblRX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblTX
        '
        Me.lblTX.BackColor = System.Drawing.Color.DimGray
        Me.lblTX.Font = New System.Drawing.Font("Microsoft Sans Serif", 5.25!)
        Me.lblTX.ForeColor = System.Drawing.Color.White
        Me.lblTX.Location = New System.Drawing.Point(64, 110)
        Me.lblTX.Name = "lblTX"
        Me.lblTX.Size = New System.Drawing.Size(15, 15)
        Me.lblTX.TabIndex = 4
        Me.lblTX.Text = "TX"
        Me.lblTX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnRefreshPorts
        '
        Me.btnRefreshPorts.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefreshPorts.Location = New System.Drawing.Point(105, 20)
        Me.btnRefreshPorts.Name = "btnRefreshPorts"
        Me.btnRefreshPorts.Size = New System.Drawing.Size(25, 23)
        Me.btnRefreshPorts.TabIndex = 3
        Me.btnRefreshPorts.Text = "↻"
        Me.btnRefreshPorts.UseVisualStyleBackColor = True
        '
        'btnConnect
        '
        Me.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConnect.Location = New System.Drawing.Point(10, 80)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(90, 25)
        Me.btnConnect.TabIndex = 2
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'cmbBaud
        '
        Me.cmbBaud.Items.AddRange(New Object() {"9600", "19200", "38400", "57600", "74880", "115200", "230400", "460800", "512000", "921600"})
        Me.cmbBaud.Location = New System.Drawing.Point(10, 50)
        Me.cmbBaud.Name = "cmbBaud"
        Me.cmbBaud.Size = New System.Drawing.Size(90, 21)
        Me.cmbBaud.TabIndex = 1
        Me.cmbBaud.Text = "115200"
        '
        'cmbPorts
        '
        Me.cmbPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbPorts.Location = New System.Drawing.Point(10, 20)
        Me.cmbPorts.Name = "cmbPorts"
        Me.cmbPorts.Size = New System.Drawing.Size(90, 21)
        Me.cmbPorts.TabIndex = 0
        '
        'scWorkArea
        '
        Me.scWorkArea.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scWorkArea.Location = New System.Drawing.Point(0, 0)
        Me.scWorkArea.Name = "scWorkArea"
        '
        'scWorkArea.Panel1
        '
        Me.scWorkArea.Panel1.Controls.Add(Me.tcCodeEditors)
        '
        'scWorkArea.Panel2
        '
        Me.scWorkArea.Panel2.Controls.Add(Me.pnlQuickCmd)
        Me.scWorkArea.Size = New System.Drawing.Size(1110, 600)
        Me.scWorkArea.SplitterDistance = 578
        Me.scWorkArea.TabIndex = 0
        '
        'tcCodeEditors
        '
        Me.tcCodeEditors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcCodeEditors.Location = New System.Drawing.Point(0, 0)
        Me.tcCodeEditors.Name = "tcCodeEditors"
        Me.tcCodeEditors.SelectedIndex = 0
        Me.tcCodeEditors.Size = New System.Drawing.Size(578, 600)
        Me.tcCodeEditors.TabIndex = 0
        '
        'pnlQuickCmd
        '
        Me.pnlQuickCmd.Controls.Add(Me.rtbxTerminal)
        Me.pnlQuickCmd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlQuickCmd.Location = New System.Drawing.Point(0, 0)
        Me.pnlQuickCmd.Name = "pnlQuickCmd"
        Me.pnlQuickCmd.Size = New System.Drawing.Size(528, 600)
        Me.pnlQuickCmd.TabIndex = 1
        '
        'rtbxTerminal
        '
        Me.rtbxTerminal.BackColor = System.Drawing.Color.Gray
        Me.rtbxTerminal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbxTerminal.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbxTerminal.ForeColor = System.Drawing.Color.LimeGreen
        Me.rtbxTerminal.Location = New System.Drawing.Point(0, 0)
        Me.rtbxTerminal.Name = "rtbxTerminal"
        Me.rtbxTerminal.ReadOnly = True
        Me.rtbxTerminal.Size = New System.Drawing.Size(528, 600)
        Me.rtbxTerminal.TabIndex = 0
        Me.rtbxTerminal.Text = ""
        '
        'grpManageFileSystem
        '
        Me.grpManageFileSystem.Controls.Add(Me.btnListFiles)
        Me.grpManageFileSystem.Controls.Add(Me.btnFormat)
        Me.grpManageFileSystem.Location = New System.Drawing.Point(730, 5)
        Me.grpManageFileSystem.Name = "grpManageFileSystem"
        Me.grpManageFileSystem.Size = New System.Drawing.Size(140, 115)
        Me.grpManageFileSystem.TabIndex = 12
        Me.grpManageFileSystem.TabStop = False
        Me.grpManageFileSystem.Text = "Manage Filesystem"
        '
        'btnListFiles
        '
        Me.btnListFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnListFiles.Location = New System.Drawing.Point(20, 60)
        Me.btnListFiles.Name = "btnListFiles"
        Me.btnListFiles.Size = New System.Drawing.Size(100, 35)
        Me.btnListFiles.TabIndex = 11
        Me.btnListFiles.Text = "List files"
        Me.btnListFiles.UseVisualStyleBackColor = True
        '
        'btnFormat
        '
        Me.btnFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFormat.Location = New System.Drawing.Point(20, 20)
        Me.btnFormat.Name = "btnFormat"
        Me.btnFormat.Size = New System.Drawing.Size(100, 35)
        Me.btnFormat.TabIndex = 0
        Me.btnFormat.Text = "Format"
        Me.btnFormat.UseVisualStyleBackColor = True
        '
        'grpRawCommnds
        '
        Me.grpRawCommnds.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpRawCommnds.Controls.Add(Me.lnkEditList)
        Me.grpRawCommnds.Controls.Add(Me.lnkAddToList)
        Me.grpRawCommnds.Controls.Add(Me.cmbRawCommands)
        Me.grpRawCommnds.Controls.Add(Me.btnSendRaw)
        Me.grpRawCommnds.Location = New System.Drawing.Point(880, 25)
        Me.grpRawCommnds.Name = "grpRawCommnds"
        Me.grpRawCommnds.Size = New System.Drawing.Size(370, 93)
        Me.grpRawCommnds.TabIndex = 9
        Me.grpRawCommnds.TabStop = False
        Me.grpRawCommnds.Text = "RAW commands"
        '
        'lnkEditList
        '
        Me.lnkEditList.AutoSize = True
        Me.lnkEditList.Location = New System.Drawing.Point(70, 61)
        Me.lnkEditList.Name = "lnkEditList"
        Me.lnkEditList.Size = New System.Drawing.Size(40, 13)
        Me.lnkEditList.TabIndex = 3
        Me.lnkEditList.TabStop = True
        Me.lnkEditList.Text = "Edit list"
        '
        'lnkAddToList
        '
        Me.lnkAddToList.AutoSize = True
        Me.lnkAddToList.Location = New System.Drawing.Point(11, 60)
        Me.lnkAddToList.Name = "lnkAddToList"
        Me.lnkAddToList.Size = New System.Drawing.Size(53, 13)
        Me.lnkAddToList.TabIndex = 2
        Me.lnkAddToList.TabStop = True
        Me.lnkAddToList.Text = "Add to list"
        '
        'cmbRawCommands
        '
        Me.cmbRawCommands.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbRawCommands.FormattingEnabled = True
        Me.cmbRawCommands.Location = New System.Drawing.Point(9, 26)
        Me.cmbRawCommands.Name = "cmbRawCommands"
        Me.cmbRawCommands.Size = New System.Drawing.Size(270, 21)
        Me.cmbRawCommands.TabIndex = 1
        '
        'btnSendRaw
        '
        Me.btnSendRaw.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSendRaw.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSendRaw.Location = New System.Drawing.Point(285, 18)
        Me.btnSendRaw.Name = "btnSendRaw"
        Me.btnSendRaw.Size = New System.Drawing.Size(75, 35)
        Me.btnSendRaw.TabIndex = 0
        Me.btnSendRaw.Text = "Send"
        Me.btnSendRaw.UseVisualStyleBackColor = True
        '
        'lnkClearResponse
        '
        Me.lnkClearResponse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkClearResponse.AutoSize = True
        Me.lnkClearResponse.Location = New System.Drawing.Point(1135, 5)
        Me.lnkClearResponse.Name = "lnkClearResponse"
        Me.lnkClearResponse.Size = New System.Drawing.Size(116, 13)
        Me.lnkClearResponse.TabIndex = 8
        Me.lnkClearResponse.TabStop = True
        Me.lnkClearResponse.Text = "Clear response window"
        '
        'lnkDetachResponse
        '
        Me.lnkDetachResponse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lnkDetachResponse.AutoSize = True
        Me.lnkDetachResponse.Location = New System.Drawing.Point(950, 5)
        Me.lnkDetachResponse.Name = "lnkDetachResponse"
        Me.lnkDetachResponse.Size = New System.Drawing.Size(127, 13)
        Me.lnkDetachResponse.TabIndex = 7
        Me.lnkDetachResponse.TabStop = True
        Me.lnkDetachResponse.Text = "Detach response window"
        '
        'grpRun
        '
        Me.grpRun.Controls.Add(Me.chkUncommentCode)
        Me.grpRun.Controls.Add(Me.btnRunSelectedCode)
        Me.grpRun.Location = New System.Drawing.Point(10, 60)
        Me.grpRun.Name = "grpRun"
        Me.grpRun.Size = New System.Drawing.Size(270, 60)
        Me.grpRun.TabIndex = 5
        Me.grpRun.TabStop = False
        Me.grpRun.Text = "Run"
        '
        'chkUncommentCode
        '
        Me.chkUncommentCode.AutoSize = True
        Me.chkUncommentCode.Location = New System.Drawing.Point(130, 25)
        Me.chkUncommentCode.Name = "chkUncommentCode"
        Me.chkUncommentCode.Size = New System.Drawing.Size(117, 17)
        Me.chkUncommentCode.TabIndex = 1
        Me.chkUncommentCode.Text = "Remove comments"
        Me.chkUncommentCode.UseVisualStyleBackColor = True
        '
        'btnRunSelectedCode
        '
        Me.btnRunSelectedCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRunSelectedCode.Location = New System.Drawing.Point(10, 20)
        Me.btnRunSelectedCode.Name = "btnRunSelectedCode"
        Me.btnRunSelectedCode.Size = New System.Drawing.Size(100, 30)
        Me.btnRunSelectedCode.TabIndex = 0
        Me.btnRunSelectedCode.Text = "Run selected"
        Me.btnRunSelectedCode.UseVisualStyleBackColor = True
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.btnSearchOnCode)
        Me.grpSearch.Controls.Add(Me.txtSeacrhTerm)
        Me.grpSearch.Location = New System.Drawing.Point(10, 5)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(270, 50)
        Me.grpSearch.TabIndex = 6
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "Search"
        '
        'btnSearchOnCode
        '
        Me.btnSearchOnCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearchOnCode.Location = New System.Drawing.Point(130, 15)
        Me.btnSearchOnCode.Name = "btnSearchOnCode"
        Me.btnSearchOnCode.Size = New System.Drawing.Size(120, 25)
        Me.btnSearchOnCode.TabIndex = 1
        Me.btnSearchOnCode.Text = "Search and highlight"
        Me.btnSearchOnCode.UseVisualStyleBackColor = True
        '
        'txtSeacrhTerm
        '
        Me.txtSeacrhTerm.Location = New System.Drawing.Point(10, 18)
        Me.txtSeacrhTerm.Name = "txtSeacrhTerm"
        Me.txtSeacrhTerm.Size = New System.Drawing.Size(114, 20)
        Me.txtSeacrhTerm.TabIndex = 0
        '
        'grpFileActions
        '
        Me.grpFileActions.Controls.Add(Me.chkAutoExecute)
        Me.grpFileActions.Controls.Add(Me.btnExecute)
        Me.grpFileActions.Controls.Add(Me.txtfile)
        Me.grpFileActions.Controls.Add(Me.btnRead)
        Me.grpFileActions.Controls.Add(Me.btnDelete)
        Me.grpFileActions.Controls.Add(Me.btnWrite)
        Me.grpFileActions.Location = New System.Drawing.Point(290, 5)
        Me.grpFileActions.Name = "grpFileActions"
        Me.grpFileActions.Size = New System.Drawing.Size(430, 115)
        Me.grpFileActions.TabIndex = 4
        Me.grpFileActions.TabStop = False
        Me.grpFileActions.Text = "File actions"
        '
        'chkAutoExecute
        '
        Me.chkAutoExecute.AutoSize = True
        Me.chkAutoExecute.Location = New System.Drawing.Point(260, 25)
        Me.chkAutoExecute.Name = "chkAutoExecute"
        Me.chkAutoExecute.Size = New System.Drawing.Size(138, 17)
        Me.chkAutoExecute.TabIndex = 5
        Me.chkAutoExecute.Text = "Auto execute after write"
        Me.chkAutoExecute.UseVisualStyleBackColor = True
        '
        'btnExecute
        '
        Me.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExecute.Location = New System.Drawing.Point(120, 60)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(90, 35)
        Me.btnExecute.TabIndex = 4
        Me.btnExecute.Text = "Execute"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'txtfile
        '
        Me.txtfile.Location = New System.Drawing.Point(15, 25)
        Me.txtfile.Name = "txtfile"
        Me.txtfile.Size = New System.Drawing.Size(230, 20)
        Me.txtfile.TabIndex = 3
        '
        'btnRead
        '
        Me.btnRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRead.Location = New System.Drawing.Point(320, 60)
        Me.btnRead.Name = "btnRead"
        Me.btnRead.Size = New System.Drawing.Size(90, 35)
        Me.btnRead.TabIndex = 2
        Me.btnRead.Text = "Read"
        Me.btnRead.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Location = New System.Drawing.Point(220, 60)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(90, 35)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnWrite
        '
        Me.btnWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWrite.Location = New System.Drawing.Point(15, 60)
        Me.btnWrite.Name = "btnWrite"
        Me.btnWrite.Size = New System.Drawing.Size(90, 35)
        Me.btnWrite.TabIndex = 0
        Me.btnWrite.Text = "Write"
        Me.btnWrite.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem1, Me.OptionsToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.ThemesToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1264, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem1
        '
        Me.FileToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenFileToolStripMenuItem, Me.CloseFileToolStripMenuItem, Me.CloseAllFilesToolStripMenuItem, Me.ToolStripMenuItem1, Me.SaveToolStripMenuItem, Me.SaveAsToolStripMenuItem, Me.ToolStripMenuItem2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem1.Name = "FileToolStripMenuItem1"
        Me.FileToolStripMenuItem1.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem1.Text = "File"
        '
        'OpenFileToolStripMenuItem
        '
        Me.OpenFileToolStripMenuItem.Name = "OpenFileToolStripMenuItem"
        Me.OpenFileToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.OpenFileToolStripMenuItem.Text = "Open file(s)"
        '
        'CloseFileToolStripMenuItem
        '
        Me.CloseFileToolStripMenuItem.Name = "CloseFileToolStripMenuItem"
        Me.CloseFileToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CloseFileToolStripMenuItem.Text = "Close file"
        '
        'CloseAllFilesToolStripMenuItem
        '
        Me.CloseAllFilesToolStripMenuItem.Name = "CloseAllFilesToolStripMenuItem"
        Me.CloseAllFilesToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CloseAllFilesToolStripMenuItem.Text = "Close all files"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(177, 6)
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'SaveAsToolStripMenuItem
        '
        Me.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem"
        Me.SaveAsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SaveAsToolStripMenuItem.Text = "Save as..."
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(177, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsToolStripMenuItem, Me.AutoConnectOnStartupToolStripMenuItem, Me.RestoreSessionToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        Me.SettingsToolStripMenuItem.Visible = False
        '
        'AutoConnectOnStartupToolStripMenuItem
        '
        Me.AutoConnectOnStartupToolStripMenuItem.Name = "AutoConnectOnStartupToolStripMenuItem"
        Me.AutoConnectOnStartupToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.AutoConnectOnStartupToolStripMenuItem.Text = "Auto connect on startup"
        '
        'RestoreSessionToolStripMenuItem
        '
        Me.RestoreSessionToolStripMenuItem.Name = "RestoreSessionToolStripMenuItem"
        Me.RestoreSessionToolStripMenuItem.Size = New System.Drawing.Size(211, 22)
        Me.RestoreSessionToolStripMenuItem.Text = "Restore session on startup"
        '
        'ThemesToolStripMenuItem
        '
        Me.ThemesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NextThemeToolStripMenuItem, Me.PrevToolStripMenuItem, Me.SelectThemeToolStripMenuItem})
        Me.ThemesToolStripMenuItem.Name = "ThemesToolStripMenuItem"
        Me.ThemesToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.ThemesToolStripMenuItem.Text = "Themes"
        '
        'NextThemeToolStripMenuItem
        '
        Me.NextThemeToolStripMenuItem.Name = "NextThemeToolStripMenuItem"
        Me.NextThemeToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.NextThemeToolStripMenuItem.Text = "Next theme"
        '
        'PrevToolStripMenuItem
        '
        Me.PrevToolStripMenuItem.Name = "PrevToolStripMenuItem"
        Me.PrevToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PrevToolStripMenuItem.Text = "Previous Theme"
        '
        'SelectThemeToolStripMenuItem
        '
        Me.SelectThemeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DefaultToolStripMenuItem, Me.ToolStripMenuItem4, Me.DarkMatterToolStripMenuItem, Me.BeehiveToolStripMenuItem, Me.ThinkGreenToolStripMenuItem, Me.IntoTheLightToolStripMenuItem, Me.MeanOrangeToolStripMenuItem, Me.HappyDayToolStripMenuItem, Me.SoLimboToolStripMenuItem, Me.OldCoffeeToolStripMenuItem, Me.PaintedStaiwayToolStripMenuItem})
        Me.SelectThemeToolStripMenuItem.Name = "SelectThemeToolStripMenuItem"
        Me.SelectThemeToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.SelectThemeToolStripMenuItem.Text = "Select Theme"
        '
        'DefaultToolStripMenuItem
        '
        Me.DefaultToolStripMenuItem.Name = "DefaultToolStripMenuItem"
        Me.DefaultToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.DefaultToolStripMenuItem.Text = "Default"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(154, 6)
        '
        'DarkMatterToolStripMenuItem
        '
        Me.DarkMatterToolStripMenuItem.Name = "DarkMatterToolStripMenuItem"
        Me.DarkMatterToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.DarkMatterToolStripMenuItem.Text = "DarkMatter"
        '
        'BeehiveToolStripMenuItem
        '
        Me.BeehiveToolStripMenuItem.Name = "BeehiveToolStripMenuItem"
        Me.BeehiveToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.BeehiveToolStripMenuItem.Text = "Beehive"
        '
        'ThinkGreenToolStripMenuItem
        '
        Me.ThinkGreenToolStripMenuItem.Name = "ThinkGreenToolStripMenuItem"
        Me.ThinkGreenToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.ThinkGreenToolStripMenuItem.Text = "Thin Green"
        '
        'IntoTheLightToolStripMenuItem
        '
        Me.IntoTheLightToolStripMenuItem.Name = "IntoTheLightToolStripMenuItem"
        Me.IntoTheLightToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.IntoTheLightToolStripMenuItem.Text = "Into The Light"
        '
        'MeanOrangeToolStripMenuItem
        '
        Me.MeanOrangeToolStripMenuItem.Name = "MeanOrangeToolStripMenuItem"
        Me.MeanOrangeToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.MeanOrangeToolStripMenuItem.Text = "Mean Orange"
        '
        'HappyDayToolStripMenuItem
        '
        Me.HappyDayToolStripMenuItem.Name = "HappyDayToolStripMenuItem"
        Me.HappyDayToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.HappyDayToolStripMenuItem.Text = "Happy Day"
        '
        'SoLimboToolStripMenuItem
        '
        Me.SoLimboToolStripMenuItem.Name = "SoLimboToolStripMenuItem"
        Me.SoLimboToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.SoLimboToolStripMenuItem.Text = "So Limbo"
        '
        'OldCoffeeToolStripMenuItem
        '
        Me.OldCoffeeToolStripMenuItem.Name = "OldCoffeeToolStripMenuItem"
        Me.OldCoffeeToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.OldCoffeeToolStripMenuItem.Text = "Old Coffee"
        '
        'PaintedStaiwayToolStripMenuItem
        '
        Me.PaintedStaiwayToolStripMenuItem.Name = "PaintedStaiwayToolStripMenuItem"
        Me.PaintedStaiwayToolStripMenuItem.Size = New System.Drawing.Size(157, 22)
        Me.PaintedStaiwayToolStripMenuItem.Text = "Painted Staiway"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.VisitWebsiteToolStripMenuItem, Me.VisitGithubRepoToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'VisitWebsiteToolStripMenuItem
        '
        Me.VisitWebsiteToolStripMenuItem.Name = "VisitWebsiteToolStripMenuItem"
        Me.VisitWebsiteToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.VisitWebsiteToolStripMenuItem.Text = "Visit website"
        '
        'lblCurrentThem
        '
        Me.lblCurrentThem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCurrentThem.AutoSize = True
        Me.lblCurrentThem.Location = New System.Drawing.Point(1200, 5)
        Me.lblCurrentThem.Name = "lblCurrentThem"
        Me.lblCurrentThem.Size = New System.Drawing.Size(40, 13)
        Me.lblCurrentThem.TabIndex = 4
        Me.lblCurrentThem.Text = "Theme"
        '
        'lblWorkingFile
        '
        Me.lblWorkingFile.AutoSize = True
        Me.lblWorkingFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblWorkingFile.Location = New System.Drawing.Point(364, 9)
        Me.lblWorkingFile.Name = "lblWorkingFile"
        Me.lblWorkingFile.Size = New System.Drawing.Size(69, 13)
        Me.lblWorkingFile.TabIndex = 5
        Me.lblWorkingFile.Text = "WorkingPath"
        '
        'tmrIndicatorReset
        '
        '
        'tmrReadTimeout
        '
        Me.tmrReadTimeout.Interval = 5000
        '
        'lblCurrentTheme
        '
        Me.lblCurrentTheme.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCurrentTheme.AutoSize = True
        Me.lblCurrentTheme.Location = New System.Drawing.Point(1146, 5)
        Me.lblCurrentTheme.Name = "lblCurrentTheme"
        Me.lblCurrentTheme.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblCurrentTheme.Size = New System.Drawing.Size(16, 13)
        Me.lblCurrentTheme.TabIndex = 6
        Me.lblCurrentTheme.Text = "..."
        Me.lblCurrentTheme.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'VisitGithubRepoToolStripMenuItem
        '
        Me.VisitGithubRepoToolStripMenuItem.Name = "VisitGithubRepoToolStripMenuItem"
        Me.VisitGithubRepoToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.VisitGithubRepoToolStripMenuItem.Text = "Visit Github repo"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FirmwareFlasherToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'FirmwareFlasherToolStripMenuItem
        '
        Me.FirmwareFlasherToolStripMenuItem.Name = "FirmwareFlasherToolStripMenuItem"
        Me.FirmwareFlasherToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.FirmwareFlasherToolStripMenuItem.Text = "Firmware flasher..."
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 761)
        Me.Controls.Add(Me.lblCurrentTheme)
        Me.Controls.Add(Me.lblWorkingFile)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.lblCurrentThem)
        Me.Controls.Add(Me.scEvenOuter)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "Selene "
        Me.scEvenOuter.Panel1.ResumeLayout(False)
        Me.scEvenOuter.Panel2.ResumeLayout(False)
        Me.scEvenOuter.Panel2.PerformLayout()
        CType(Me.scEvenOuter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scEvenOuter.ResumeLayout(False)
        Me.scMainOuter.Panel1.ResumeLayout(False)
        Me.scMainOuter.Panel2.ResumeLayout(False)
        CType(Me.scMainOuter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMainOuter.ResumeLayout(False)
        Me.pnlSidebar.ResumeLayout(False)
        Me.flpMacros.ResumeLayout(False)
        Me.grpConnection.ResumeLayout(False)
        Me.scWorkArea.Panel1.ResumeLayout(False)
        Me.scWorkArea.Panel2.ResumeLayout(False)
        CType(Me.scWorkArea, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scWorkArea.ResumeLayout(False)
        Me.pnlQuickCmd.ResumeLayout(False)
        Me.grpManageFileSystem.ResumeLayout(False)
        Me.grpRawCommnds.ResumeLayout(False)
        Me.grpRawCommnds.PerformLayout()
        Me.grpRun.ResumeLayout(False)
        Me.grpRun.PerformLayout()
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.grpFileActions.ResumeLayout(False)
        Me.grpFileActions.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents scEvenOuter As SplitContainer
    Friend WithEvents scMainOuter As SplitContainer
    Friend WithEvents pnlSidebar As Panel
    Friend WithEvents flpMacros As FlowLayoutPanel
    Friend WithEvents btnMacro1 As Button
    Friend WithEvents btnMacro2 As Button
    Friend WithEvents btnMacro3 As Button
    Friend WithEvents btnMacro4 As Button
    Friend WithEvents btnMacro5 As Button
    Friend WithEvents btnMacro6 As Button
    Friend WithEvents btnMacro7 As Button
    Friend WithEvents btnMacro8 As Button
    Friend WithEvents btnMacro9 As Button
    Friend WithEvents btnMacro10 As Button
    Friend WithEvents btnMacro11 As Button
    Friend WithEvents btnMacro12 As Button
    Friend WithEvents grpConnection As GroupBox
    Friend WithEvents lblStatusSquare As Label
    Friend WithEvents lblRX As Label
    Friend WithEvents lblTX As Label
    Friend WithEvents btnRefreshPorts As Button
    Friend WithEvents btnConnect As Button
    Friend WithEvents cmbBaud As ComboBox
    Friend WithEvents cmbPorts As ComboBox
    Friend WithEvents scWorkArea As SplitContainer
    Friend WithEvents tcCodeEditors As TabControl
    Friend WithEvents pnlQuickCmd As Panel
    Friend WithEvents cmbRawCommands As ComboBox
    Friend WithEvents btnSendRaw As Button
    Friend WithEvents grpFileActions As GroupBox
    Friend WithEvents chkUncommentCode As CheckBox
    Friend WithEvents btnSearchOnCode As Button
    Friend WithEvents txtSeacrhTerm As TextBox
    Friend WithEvents btnRunSelectedCode As Button
    Friend WithEvents txtfile As TextBox
    Friend WithEvents btnRead As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnWrite As Button
    Friend WithEvents grpSearch As GroupBox
    Friend WithEvents grpRun As GroupBox
    Friend WithEvents chkAutoExecute As CheckBox
    Friend WithEvents btnExecute As Button
    Friend WithEvents grpRawCommnds As GroupBox
    Friend WithEvents lnkEditList As LinkLabel
    Friend WithEvents lnkAddToList As LinkLabel
    Friend WithEvents lnkDetachResponse As LinkLabel
    Friend WithEvents grpManageFileSystem As GroupBox
    Friend WithEvents btnListFiles As Button
    Friend WithEvents btnFormat As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents OpenFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseFileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CloseAllFilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents SaveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SaveAsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AutoConnectOnStartupToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ThemesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NextThemeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PrevToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelectThemeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DarkMatterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BeehiveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ThinkGreenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents IntoTheLightToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MeanOrangeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HappyDayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SoLimboToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OldCoffeeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PaintedStaiwayToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VisitWebsiteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblCurrentThem As Label
    Friend WithEvents lblWorkingFile As Label
    Friend WithEvents tmrIndicatorReset As Timer
    Friend WithEvents tmrReadTimeout As Timer
    Friend WithEvents Timer1 As Timer
    Friend WithEvents RestoreSessionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents rtbxTerminal As RichTextBox
    Friend WithEvents lnkClearResponse As LinkLabel
    Friend WithEvents DefaultToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As ToolStripSeparator
    Friend WithEvents lblCurrentTheme As Label
    Friend WithEvents VisitGithubRepoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FirmwareFlasherToolStripMenuItem As ToolStripMenuItem
End Class