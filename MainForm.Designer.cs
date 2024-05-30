namespace StudioCCS
{
	// Token: 0x0200000E RID: 14
	public partial class MainForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00006A24 File Offset: 0x00004C24
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::StudioCCS.MainForm));
			this.LogSplit = new global::System.Windows.Forms.SplitContainer();
			this.viewportSplit = new global::System.Windows.Forms.SplitContainer();
			this.treeSplit = new global::System.Windows.Forms.SplitContainer();
			this.sceneTreeView = new global::System.Windows.Forms.TreeView();
			this.ccsTree = new global::System.Windows.Forms.TreeView();
			this.ccsPropertyGrid = new global::System.Windows.Forms.PropertyGrid();
			this.viewToolstrip = new global::System.Windows.Forms.ToolStrip();
			this.tbtnPreview = new global::System.Windows.Forms.ToolStripButton();
			this.tbtnScene = new global::System.Windows.Forms.ToolStripButton();
			this.tbtnAll = new global::System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new global::System.Windows.Forms.ToolStripSeparator();
			this.toolStripSplitButton1 = new global::System.Windows.Forms.ToolStripSplitButton();
			this.wireframeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.vertexColorsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.smoothShadedToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.texturedToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new global::System.Windows.Forms.ToolStripSeparator();
			this.backfaceCullingToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.drawGridToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new global::System.Windows.Forms.ToolStripSeparator();
			this.sceneModeToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.drawCollisionMeshesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.drawDummiesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.drawLightHelpersToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.tlblRenderMode = new global::System.Windows.Forms.ToolStripLabel();
			this.logView = new global::System.Windows.Forms.RichTextBox();
			this.statusStrip1 = new global::System.Windows.Forms.StatusStrip();
			this.statusCameraLabel = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.renderTimer = new global::System.Windows.Forms.Timer(this.components);
			this.menuStrip1 = new global::System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.loadCCSToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.loadPackedToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.loadCCSToolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new global::System.Windows.Forms.ToolStripSeparator();
			this.recentToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.sceneToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.iMOQToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.loadTownToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dumpToOBJToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.dumpToSMDToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.defaultToAxisMovementToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.defaultToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.axisToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new global::System.Windows.Forms.ToolStripSeparator();
			this.QuickChangeMoveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.fastCameraMoveToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.keyToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setSpeedToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ccsFileContextMenu = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.unloadToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.addAllToSceneToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.viewCCSReportMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.sceneContextMenu = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.ccsClumpContextMenu = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.editBonesToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.ccsAnimeContextMenu = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.addToSceneToolStripMenuItem1 = new global::System.Windows.Forms.ToolStripMenuItem();
			this.setPoseToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.sceneAnimeContextMenu = new global::System.Windows.Forms.ContextMenuStrip(this.components);
			this.sceneAnimeContext_RemoveMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			((global::System.ComponentModel.ISupportInitialize)this.LogSplit).BeginInit();
			this.LogSplit.Panel1.SuspendLayout();
			this.LogSplit.Panel2.SuspendLayout();
			this.LogSplit.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.viewportSplit).BeginInit();
			this.viewportSplit.Panel1.SuspendLayout();
			this.viewportSplit.Panel2.SuspendLayout();
			this.viewportSplit.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.treeSplit).BeginInit();
			this.treeSplit.Panel1.SuspendLayout();
			this.treeSplit.Panel2.SuspendLayout();
			this.treeSplit.SuspendLayout();
			this.viewToolstrip.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.ccsFileContextMenu.SuspendLayout();
			this.ccsClumpContextMenu.SuspendLayout();
			this.ccsAnimeContextMenu.SuspendLayout();
			this.sceneAnimeContextMenu.SuspendLayout();
			base.SuspendLayout();
			this.LogSplit.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.LogSplit.Location = new global::System.Drawing.Point(0, 24);
			this.LogSplit.Name = "LogSplit";
			this.LogSplit.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.LogSplit.Panel1.Controls.Add(this.viewportSplit);
			this.LogSplit.Panel2.Controls.Add(this.logView);
			this.LogSplit.Panel2.Controls.Add(this.statusStrip1);
			this.LogSplit.Size = new global::System.Drawing.Size(901, 559);
			this.LogSplit.SplitterDistance = 438;
			this.LogSplit.TabIndex = 0;
			this.viewportSplit.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.viewportSplit.Location = new global::System.Drawing.Point(0, 0);
			this.viewportSplit.Name = "viewportSplit";
			this.viewportSplit.Panel1.Controls.Add(this.treeSplit);
			this.viewportSplit.Panel2.BackColor = global::System.Drawing.SystemColors.Control;
			this.viewportSplit.Panel2.Controls.Add(this.viewToolstrip);
			this.viewportSplit.Size = new global::System.Drawing.Size(901, 438);
			this.viewportSplit.SplitterDistance = 183;
			this.viewportSplit.TabIndex = 0;
			this.treeSplit.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.treeSplit.Location = new global::System.Drawing.Point(0, 0);
			this.treeSplit.Name = "treeSplit";
			this.treeSplit.Orientation = global::System.Windows.Forms.Orientation.Horizontal;
			this.treeSplit.Panel1.Controls.Add(this.sceneTreeView);
			this.treeSplit.Panel1.Controls.Add(this.ccsTree);
			this.treeSplit.Panel2.Controls.Add(this.ccsPropertyGrid);
			this.treeSplit.Size = new global::System.Drawing.Size(183, 438);
			this.treeSplit.SplitterDistance = 294;
			this.treeSplit.TabIndex = 0;
			this.sceneTreeView.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.sceneTreeView.Location = new global::System.Drawing.Point(0, 0);
			this.sceneTreeView.Name = "sceneTreeView";
			this.sceneTreeView.Size = new global::System.Drawing.Size(183, 294);
			this.sceneTreeView.TabIndex = 1;
			this.sceneTreeView.Visible = false;
			this.sceneTreeView.NodeMouseClick += new global::System.Windows.Forms.TreeNodeMouseClickEventHandler(this.SceneTreeViewNodeMouseClick);
			this.ccsTree.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.ccsTree.Location = new global::System.Drawing.Point(0, 0);
			this.ccsTree.Name = "ccsTree";
			this.ccsTree.Size = new global::System.Drawing.Size(183, 294);
			this.ccsTree.TabIndex = 0;
			this.ccsTree.AfterSelect += new global::System.Windows.Forms.TreeViewEventHandler(this.CcsTreeAfterSelect);
			this.ccsTree.NodeMouseClick += new global::System.Windows.Forms.TreeNodeMouseClickEventHandler(this.CcsTreeNodeMouseClick);
			this.ccsPropertyGrid.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.ccsPropertyGrid.LineColor = global::System.Drawing.SystemColors.ControlDark;
			this.ccsPropertyGrid.Location = new global::System.Drawing.Point(0, 0);
			this.ccsPropertyGrid.Name = "ccsPropertyGrid";
			this.ccsPropertyGrid.Size = new global::System.Drawing.Size(183, 140);
			this.ccsPropertyGrid.TabIndex = 0;
			this.viewToolstrip.GripStyle = global::System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.viewToolstrip.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.tbtnPreview,
				this.tbtnScene,
				this.tbtnAll,
				this.toolStripSeparator1,
				this.toolStripSplitButton1,
				this.tlblRenderMode
			});
			this.viewToolstrip.Location = new global::System.Drawing.Point(0, 0);
			this.viewToolstrip.Name = "viewToolstrip";
			this.viewToolstrip.Size = new global::System.Drawing.Size(714, 25);
			this.viewToolstrip.TabIndex = 0;
			this.viewToolstrip.Text = "toolStrip1";
			this.tbtnPreview.CheckOnClick = true;
			this.tbtnPreview.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbtnPreview.Image = this.tbtnPreview.Image;
			this.tbtnPreview.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.tbtnPreview.Name = "tbtnPreview";
			this.tbtnPreview.Size = new global::System.Drawing.Size(52, 22);
			this.tbtnPreview.Text = "Preview";
			this.tbtnPreview.CheckedChanged += new global::System.EventHandler(this.TbtnPreviewCheckedChanged);
			this.tbtnScene.CheckOnClick = true;
			this.tbtnScene.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbtnScene.Image = this.tbtnScene.Image;
			this.tbtnScene.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.tbtnScene.Name = "tbtnScene";
			this.tbtnScene.Size = new global::System.Drawing.Size(42, 22);
			this.tbtnScene.Text = "Scene";
			this.tbtnScene.CheckedChanged += new global::System.EventHandler(this.TbtnSceneCheckedChanged);
			this.tbtnAll.CheckOnClick = true;
			this.tbtnAll.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tbtnAll.Image = this.tbtnAll.Image;
			this.tbtnAll.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.tbtnAll.Name = "tbtnAll";
			this.tbtnAll.Size = new global::System.Drawing.Size(25, 22);
			this.tbtnAll.Text = "All";
			this.tbtnAll.CheckedChanged += new global::System.EventHandler(this.TbtnAllCheckedChanged);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new global::System.Drawing.Size(6, 25);
			this.toolStripSplitButton1.DisplayStyle = global::System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripSplitButton1.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.wireframeToolStripMenuItem,
				this.vertexColorsToolStripMenuItem,
				this.smoothShadedToolStripMenuItem,
				this.texturedToolStripMenuItem,
				this.toolStripSeparator3,
				this.backfaceCullingToolStripMenuItem,
				this.drawAxisMarkerInTopOfViewportToolStripMenuItem,
				this.drawAxisMarkerAtWorldCenterToolStripMenuItem,
				this.drawGridToolStripMenuItem,
				this.toolStripSeparator4,
				this.sceneModeToolStripMenuItem
			});
			this.toolStripSplitButton1.Image = this.toolStripSplitButton1.Image;
			this.toolStripSplitButton1.ImageTransparentColor = global::System.Drawing.Color.Magenta;
			this.toolStripSplitButton1.Name = "toolStripSplitButton1";
			this.toolStripSplitButton1.Size = new global::System.Drawing.Size(95, 22);
			this.toolStripSplitButton1.Text = "Draw Options";
			this.wireframeToolStripMenuItem.CheckOnClick = true;
			this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
			this.wireframeToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.wireframeToolStripMenuItem.Text = "Wireframe";
			this.wireframeToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.WireframeToolStripMenuItemCheckedChanged);
			this.vertexColorsToolStripMenuItem.CheckOnClick = true;
			this.vertexColorsToolStripMenuItem.Name = "vertexColorsToolStripMenuItem";
			this.vertexColorsToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.vertexColorsToolStripMenuItem.Text = "Vertex Colors";
			this.vertexColorsToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.VertexColorsToolStripMenuItemCheckedChanged);
			this.smoothShadedToolStripMenuItem.CheckOnClick = true;
			this.smoothShadedToolStripMenuItem.Name = "smoothShadedToolStripMenuItem";
			this.smoothShadedToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.smoothShadedToolStripMenuItem.Text = "Smooth Shading";
			this.smoothShadedToolStripMenuItem.Visible = false;
			this.smoothShadedToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.SmoothShadedToolStripMenuItemCheckedChanged);
			this.texturedToolStripMenuItem.CheckOnClick = true;
			this.texturedToolStripMenuItem.Name = "texturedToolStripMenuItem";
			this.texturedToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.texturedToolStripMenuItem.Text = "Texturing";
			this.texturedToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.TexturedToolStripMenuItemCheckedChanged);
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new global::System.Drawing.Size(214, 6);
			this.backfaceCullingToolStripMenuItem.CheckOnClick = true;
			this.backfaceCullingToolStripMenuItem.Name = "backfaceCullingToolStripMenuItem";
			this.backfaceCullingToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.backfaceCullingToolStripMenuItem.Text = "Backface Culling";
			this.backfaceCullingToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.BackfaceCullingToolStripMenuItemCheckedChanged);
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Checked = true;
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.CheckOnClick = true;
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Name = "drawAxisMarkerInTopOfViewportToolStripMenuItem";
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Text = "Draw View Orientation Axis";
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.ToolTipText = "Draw Axis Marker for View Orientation in Top Right Corner of Viewport";
			this.drawAxisMarkerInTopOfViewportToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.DrawAxisMarkerInTopOfViewportToolStripMenuItemCheckedChanged);
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Checked = true;
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.CheckOnClick = true;
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Name = "drawAxisMarkerAtWorldCenterToolStripMenuItem";
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Text = "Draw View Center";
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.ToolTipText = "Draw Axis Marker at Center of World";
			this.drawAxisMarkerAtWorldCenterToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.DrawAxisMarkerAtWorldCenterToolStripMenuItemCheckedChanged);
			this.drawGridToolStripMenuItem.Checked = true;
			this.drawGridToolStripMenuItem.CheckOnClick = true;
			this.drawGridToolStripMenuItem.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.drawGridToolStripMenuItem.Name = "drawGridToolStripMenuItem";
			this.drawGridToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.drawGridToolStripMenuItem.Text = "Draw Grid";
			this.drawGridToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.DrawGridToolStripMenuItemCheckedChanged);
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new global::System.Drawing.Size(214, 6);
			this.sceneModeToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.drawCollisionMeshesToolStripMenuItem,
				this.drawDummiesToolStripMenuItem,
				this.drawLightHelpersToolStripMenuItem
			});
			this.sceneModeToolStripMenuItem.Name = "sceneModeToolStripMenuItem";
			this.sceneModeToolStripMenuItem.Size = new global::System.Drawing.Size(217, 22);
			this.sceneModeToolStripMenuItem.Text = "Scene Mode";
			this.drawCollisionMeshesToolStripMenuItem.Checked = true;
			this.drawCollisionMeshesToolStripMenuItem.CheckOnClick = true;
			this.drawCollisionMeshesToolStripMenuItem.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.drawCollisionMeshesToolStripMenuItem.Name = "drawCollisionMeshesToolStripMenuItem";
			this.drawCollisionMeshesToolStripMenuItem.Size = new global::System.Drawing.Size(193, 22);
			this.drawCollisionMeshesToolStripMenuItem.Text = "Draw Collision Meshes";
			this.drawCollisionMeshesToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.DrawCollisionMeshesToolStripMenuItemCheckedChanged);
			this.drawDummiesToolStripMenuItem.Checked = true;
			this.drawDummiesToolStripMenuItem.CheckOnClick = true;
			this.drawDummiesToolStripMenuItem.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.drawDummiesToolStripMenuItem.Name = "drawDummiesToolStripMenuItem";
			this.drawDummiesToolStripMenuItem.Size = new global::System.Drawing.Size(193, 22);
			this.drawDummiesToolStripMenuItem.Text = "Draw Dummy Helpers";
			this.drawDummiesToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.DrawDummiesToolStripMenuItemCheckedChanged);
			this.drawLightHelpersToolStripMenuItem.Checked = true;
			this.drawLightHelpersToolStripMenuItem.CheckOnClick = true;
			this.drawLightHelpersToolStripMenuItem.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.drawLightHelpersToolStripMenuItem.Name = "drawLightHelpersToolStripMenuItem";
			this.drawLightHelpersToolStripMenuItem.Size = new global::System.Drawing.Size(193, 22);
			this.drawLightHelpersToolStripMenuItem.Text = "Draw Light Helpers";
			this.drawLightHelpersToolStripMenuItem.Visible = false;
			this.drawLightHelpersToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.DrawLightHelpersToolStripMenuItemCheckedChanged);
			this.tlblRenderMode.Alignment = global::System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tlblRenderMode.Name = "tlblRenderMode";
			this.tlblRenderMode.Size = new global::System.Drawing.Size(0, 22);
			this.logView.BackColor = global::System.Drawing.SystemColors.ControlDarkDark;
			this.logView.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.logView.ForeColor = global::System.Drawing.SystemColors.Window;
			this.logView.HideSelection = false;
			this.logView.Location = new global::System.Drawing.Point(0, 0);
			this.logView.Name = "logView";
			this.logView.ReadOnly = true;
			this.logView.Size = new global::System.Drawing.Size(901, 95);
			this.logView.TabIndex = 1;
			this.logView.Text = "";
			this.statusStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.statusCameraLabel
			});
			this.statusStrip1.Location = new global::System.Drawing.Point(0, 95);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new global::System.Drawing.Size(901, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			this.statusCameraLabel.Name = "statusCameraLabel";
			this.statusCameraLabel.Size = new global::System.Drawing.Size(48, 17);
			this.statusCameraLabel.Text = "Camera";
			this.renderTimer.Interval = 20;
			this.renderTimer.Tick += new global::System.EventHandler(this.RenderTimerTick);
			this.menuStrip1.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.fileToolStripMenuItem,
				this.sceneToolStripMenuItem,
				this.optionsToolStripMenuItem,
				this.toolStripMenuItem
			});
			this.menuStrip1.Location = new global::System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new global::System.Drawing.Size(901, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			this.fileToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.loadCCSToolStripMenuItem,
				this.saveStripMenuItem,
				this.toolStripSeparator2,
				this.recentToolStripMenuItem,
				this.exitToolStripMenuItem
			});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new global::System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			this.loadCCSToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.loadPackedToolStripMenuItem,
				this.loadCCSToolStripMenuItem1
			});
			this.loadCCSToolStripMenuItem.Name = "loadCCSToolStripMenuItem";
			this.loadCCSToolStripMenuItem.Size = new global::System.Drawing.Size(121, 22);
			this.loadCCSToolStripMenuItem.Text = "Load File";
			this.loadCCSToolStripMenuItem.Click += new global::System.EventHandler(this.LoadCCSToolStripMenuItemClick);
			this.loadPackedToolStripMenuItem.Name = "loadPackedToolStripMenuItem";
			this.loadPackedToolStripMenuItem.Size = new global::System.Drawing.Size(130, 22);
			this.loadPackedToolStripMenuItem.Text = "Load .TMP";
			this.loadPackedToolStripMenuItem.Click += new global::System.EventHandler(this.loadPackedToolStripMenuItem_Click);
			this.loadCCSToolStripMenuItem1.Name = "loadCCSToolStripMenuItem1";
			this.loadCCSToolStripMenuItem1.Size = new global::System.Drawing.Size(130, 22);
			this.loadCCSToolStripMenuItem1.Text = "Load .CCS";
			this.loadCCSToolStripMenuItem1.Click += new global::System.EventHandler(this.loadCCSToolStripMenuItem1_Click);
			this.saveStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.saveAsToolStripMenuItem,
				this.saveToolStripMenuItem
			});
			this.saveStripMenuItem.Enabled = false;
			this.saveStripMenuItem.Name = "saveStripMenuItem";
			this.saveStripMenuItem.Size = new global::System.Drawing.Size(121, 22);
			this.saveStripMenuItem.Text = "Save";
			this.saveAsToolStripMenuItem.Enabled = false;
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new global::System.Drawing.Size(187, 22);
			this.saveAsToolStripMenuItem.Text = "Save as...";
			this.saveToolStripMenuItem.Enabled = false;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new global::System.Drawing.Size(187, 22);
			this.saveToolStripMenuItem.Text = "Save Packed as CCS...";
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new global::System.Drawing.Size(118, 6);
			this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
			this.recentToolStripMenuItem.Size = new global::System.Drawing.Size(121, 22);
			this.recentToolStripMenuItem.Text = "Recent";
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new global::System.Drawing.Size(121, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new global::System.EventHandler(this.ExitToolStripMenuItemClick);
			this.sceneToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.iMOQToolStripMenuItem,
				this.dumpToOBJToolStripMenuItem,
				this.dumpToSMDToolStripMenuItem
			});
			this.sceneToolStripMenuItem.Name = "sceneToolStripMenuItem";
			this.sceneToolStripMenuItem.Size = new global::System.Drawing.Size(50, 20);
			this.sceneToolStripMenuItem.Text = "Scene";
			this.iMOQToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.loadTownToolStripMenuItem
			});
			this.iMOQToolStripMenuItem.Name = "iMOQToolStripMenuItem";
			this.iMOQToolStripMenuItem.Size = new global::System.Drawing.Size(149, 22);
			this.iMOQToolStripMenuItem.Text = "IMOQ";
			this.iMOQToolStripMenuItem.Visible = false;
			this.loadTownToolStripMenuItem.Name = "loadTownToolStripMenuItem";
			this.loadTownToolStripMenuItem.Size = new global::System.Drawing.Size(131, 22);
			this.loadTownToolStripMenuItem.Text = "Load Town";
			this.dumpToOBJToolStripMenuItem.Name = "dumpToOBJToolStripMenuItem";
			this.dumpToOBJToolStripMenuItem.Size = new global::System.Drawing.Size(149, 22);
			this.dumpToOBJToolStripMenuItem.Text = "Dump to OBJ";
			this.dumpToOBJToolStripMenuItem.Click += new global::System.EventHandler(this.DumpToOBJToolStripMenuItemClick);
			this.dumpToSMDToolStripMenuItem.Name = "dumpToSMDToolStripMenuItem";
			this.dumpToSMDToolStripMenuItem.Size = new global::System.Drawing.Size(149, 22);
			this.dumpToSMDToolStripMenuItem.Text = "Dump to SMD";
			this.dumpToSMDToolStripMenuItem.Visible = false;
			this.dumpToSMDToolStripMenuItem.Click += new global::System.EventHandler(this.DumpToSMDToolStripMenuItemClick);
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.defaultToAxisMovementToolStripMenuItem,
				this.fastCameraMoveToolStripMenuItem
			});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new global::System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			this.defaultToAxisMovementToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.defaultToolStripMenuItem,
				this.axisToolStripMenuItem,
				this.toolStripSeparator5,
				this.QuickChangeMoveToolStripMenuItem
			});
			this.defaultToAxisMovementToolStripMenuItem.Name = "defaultToAxisMovementToolStripMenuItem";
			this.defaultToAxisMovementToolStripMenuItem.Size = new global::System.Drawing.Size(172, 22);
			this.defaultToAxisMovementToolStripMenuItem.Text = "Movement";
			this.defaultToolStripMenuItem.Checked = true;
			this.defaultToolStripMenuItem.CheckOnClick = true;
			this.defaultToolStripMenuItem.CheckState = global::System.Windows.Forms.CheckState.Checked;
			this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
			this.defaultToolStripMenuItem.Size = new global::System.Drawing.Size(207, 22);
			this.defaultToolStripMenuItem.Text = "Default";
			this.defaultToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.defaultToolStripMenuItem_CheckedChanged);
			this.axisToolStripMenuItem.CheckOnClick = true;
			this.axisToolStripMenuItem.Name = "axisToolStripMenuItem";
			this.axisToolStripMenuItem.Size = new global::System.Drawing.Size(207, 22);
			this.axisToolStripMenuItem.Text = "Axis";
			this.axisToolStripMenuItem.CheckedChanged += new global::System.EventHandler(this.axisToolStripMenuItem_CheckedChanged);
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new global::System.Drawing.Size(204, 6);
			this.QuickChangeMoveToolStripMenuItem.Name = "QuickChangeMoveToolStripMenuItem";
			this.QuickChangeMoveToolStripMenuItem.Size = new global::System.Drawing.Size(207, 22);
			this.QuickChangeMoveToolStripMenuItem.Text = "Quick Change Key: SHIFT";
			this.QuickChangeMoveToolStripMenuItem.Click += new global::System.EventHandler(this.keySHIFTToolStripMenuItem_Click);
			this.fastCameraMoveToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.keyToolStripMenuItem,
				this.setSpeedToolStripMenuItem
			});
			this.fastCameraMoveToolStripMenuItem.Name = "fastCameraMoveToolStripMenuItem";
			this.fastCameraMoveToolStripMenuItem.Size = new global::System.Drawing.Size(172, 22);
			this.fastCameraMoveToolStripMenuItem.Text = "Fast Camera Move";
			this.keyToolStripMenuItem.Name = "keyToolStripMenuItem";
			this.keyToolStripMenuItem.Size = new global::System.Drawing.Size(178, 22);
			this.keyToolStripMenuItem.Text = "Key: C";
			this.keyToolStripMenuItem.Click += new global::System.EventHandler(this.keyToolStripMenuItem_Click);
			this.setSpeedToolStripMenuItem.Name = "setSpeedToolStripMenuItem";
			this.setSpeedToolStripMenuItem.Size = new global::System.Drawing.Size(178, 22);
			this.setSpeedToolStripMenuItem.Text = "Speed Multiplier: 10";
			this.setSpeedToolStripMenuItem.Click += new global::System.EventHandler(this.setSpeedToolStripMenuItem_Click);
			this.toolStripMenuItem.Name = "toolStripMenuItem";
			this.toolStripMenuItem.Size = new global::System.Drawing.Size(52, 20);
			this.toolStripMenuItem.Text = "About";
			this.toolStripMenuItem.Click += new global::System.EventHandler(this.toolStripMenuItem_Click);
			this.ccsFileContextMenu.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.unloadToolStripMenuItem,
				this.addAllToSceneToolStripMenuItem,
				this.viewCCSReportMenuItem
			});
			this.ccsFileContextMenu.Name = "ccsFileContextMenu";
			this.ccsFileContextMenu.Size = new global::System.Drawing.Size(163, 70);
			this.unloadToolStripMenuItem.Name = "unloadToolStripMenuItem";
			this.unloadToolStripMenuItem.Size = new global::System.Drawing.Size(162, 22);
			this.unloadToolStripMenuItem.Text = "Unload";
			this.unloadToolStripMenuItem.Click += new global::System.EventHandler(this.UnloadToolStripMenuItemClick);
			this.addAllToSceneToolStripMenuItem.Name = "addAllToSceneToolStripMenuItem";
			this.addAllToSceneToolStripMenuItem.Size = new global::System.Drawing.Size(162, 22);
			this.addAllToSceneToolStripMenuItem.Text = "Add All To Scene";
			this.addAllToSceneToolStripMenuItem.Visible = false;
			this.viewCCSReportMenuItem.Name = "viewCCSReportMenuItem";
			this.viewCCSReportMenuItem.Size = new global::System.Drawing.Size(162, 22);
			this.viewCCSReportMenuItem.Text = "View Info Report";
			this.viewCCSReportMenuItem.Click += new global::System.EventHandler(this.ViewCCSReportMenuItemClick);
			this.sceneContextMenu.Name = "sceneContextMenu";
			this.sceneContextMenu.Size = new global::System.Drawing.Size(61, 4);
			this.ccsClumpContextMenu.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.editBonesToolStripMenuItem
			});
			this.ccsClumpContextMenu.Name = "ccsClumpContextMenu";
			this.ccsClumpContextMenu.Size = new global::System.Drawing.Size(130, 26);
			this.editBonesToolStripMenuItem.Name = "editBonesToolStripMenuItem";
			this.editBonesToolStripMenuItem.Size = new global::System.Drawing.Size(129, 22);
			this.editBonesToolStripMenuItem.Text = "Edit Bones";
			this.editBonesToolStripMenuItem.Click += new global::System.EventHandler(this.EditBonesToolStripMenuItemClick);
			this.ccsAnimeContextMenu.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.addToSceneToolStripMenuItem1,
				this.setPoseToolStripMenuItem
			});
			this.ccsAnimeContextMenu.Name = "ccsAnimeContextMenu";
			this.ccsAnimeContextMenu.Size = new global::System.Drawing.Size(145, 48);
			this.addToSceneToolStripMenuItem1.Name = "addToSceneToolStripMenuItem1";
			this.addToSceneToolStripMenuItem1.Size = new global::System.Drawing.Size(144, 22);
			this.addToSceneToolStripMenuItem1.Text = "Add to Scene";
			this.addToSceneToolStripMenuItem1.Click += new global::System.EventHandler(this.AddToSceneToolStripMenuItem1Click);
			this.setPoseToolStripMenuItem.Name = "setPoseToolStripMenuItem";
			this.setPoseToolStripMenuItem.Size = new global::System.Drawing.Size(144, 22);
			this.setPoseToolStripMenuItem.Text = "Set Pose";
			this.setPoseToolStripMenuItem.Click += new global::System.EventHandler(this.SetPoseToolStripMenuItemClick);
			this.sceneAnimeContextMenu.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.sceneAnimeContext_RemoveMenuItem
			});
			this.sceneAnimeContextMenu.Name = "sceneAnimeContextMenu";
			this.sceneAnimeContextMenu.Size = new global::System.Drawing.Size(118, 26);
			this.sceneAnimeContext_RemoveMenuItem.Name = "sceneAnimeContext_RemoveMenuItem";
			this.sceneAnimeContext_RemoveMenuItem.Size = new global::System.Drawing.Size(117, 22);
			this.sceneAnimeContext_RemoveMenuItem.Text = "Remove";
			this.sceneAnimeContext_RemoveMenuItem.Click += new global::System.EventHandler(this.SceneAnimeContext_RemoveMenuItemClick);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(901, 583);
			base.Controls.Add(this.LogSplit);
			base.Controls.Add(this.menuStrip1);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.KeyPreview = true;
			base.Name = "MainForm";
			this.Text = "Modded StudioCCS";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
			base.DragDrop += new global::System.Windows.Forms.DragEventHandler(this.MainFormDragDrop);
			base.DragEnter += new global::System.Windows.Forms.DragEventHandler(this.MainFormDragEnter);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			base.KeyUp += new global::System.Windows.Forms.KeyEventHandler(this.MainFormKeyUp);
			this.LogSplit.Panel1.ResumeLayout(false);
			this.LogSplit.Panel2.ResumeLayout(false);
			this.LogSplit.Panel2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.LogSplit).EndInit();
			this.LogSplit.ResumeLayout(false);
			this.viewportSplit.Panel1.ResumeLayout(false);
			this.viewportSplit.Panel2.ResumeLayout(false);
			this.viewportSplit.Panel2.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.viewportSplit).EndInit();
			this.viewportSplit.ResumeLayout(false);
			this.treeSplit.Panel1.ResumeLayout(false);
			this.treeSplit.Panel2.ResumeLayout(false);
			((global::System.ComponentModel.ISupportInitialize)this.treeSplit).EndInit();
			this.treeSplit.ResumeLayout(false);
			this.viewToolstrip.ResumeLayout(false);
			this.viewToolstrip.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ccsFileContextMenu.ResumeLayout(false);
			this.ccsClumpContextMenu.ResumeLayout(false);
			this.ccsAnimeContextMenu.ResumeLayout(false);
			this.sceneAnimeContextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000A034 File Offset: 0x00008234
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400007F RID: 127
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x04000080 RID: 128
		private global::System.Windows.Forms.SplitContainer LogSplit;

		// Token: 0x04000081 RID: 129
		private global::System.Windows.Forms.SplitContainer viewportSplit;

		// Token: 0x04000082 RID: 130
		private global::System.Windows.Forms.SplitContainer treeSplit;

		// Token: 0x04000083 RID: 131
		private global::System.Windows.Forms.TreeView ccsTree;

		// Token: 0x04000084 RID: 132
		private global::System.Windows.Forms.PropertyGrid ccsPropertyGrid;

		// Token: 0x04000085 RID: 133
		private global::System.Windows.Forms.ToolStrip viewToolstrip;

		// Token: 0x04000086 RID: 134
		private global::System.Windows.Forms.ToolStripButton tbtnPreview;

		// Token: 0x04000087 RID: 135
		private global::System.Windows.Forms.ToolStripButton tbtnScene;

		// Token: 0x04000088 RID: 136
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

		// Token: 0x04000089 RID: 137
		private global::System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;

		// Token: 0x0400008A RID: 138
		private global::System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;

		// Token: 0x0400008B RID: 139
		private global::System.Windows.Forms.ToolStripMenuItem smoothShadedToolStripMenuItem;

		// Token: 0x0400008C RID: 140
		private global::System.Windows.Forms.ToolStripMenuItem texturedToolStripMenuItem;

		// Token: 0x0400008D RID: 141
		private global::System.Windows.Forms.RichTextBox logView;

		// Token: 0x0400008E RID: 142
		private global::System.Windows.Forms.StatusStrip statusStrip1;

		// Token: 0x0400008F RID: 143
		private global::System.Windows.Forms.Timer renderTimer;

		// Token: 0x04000090 RID: 144
		private global::System.Windows.Forms.MenuStrip menuStrip1;

		// Token: 0x04000091 RID: 145
		private global::System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;

		// Token: 0x04000092 RID: 146
		private global::System.Windows.Forms.ToolStripMenuItem loadCCSToolStripMenuItem;

		// Token: 0x04000093 RID: 147
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

		// Token: 0x04000094 RID: 148
		private global::System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;

		// Token: 0x04000095 RID: 149
		private global::System.Windows.Forms.ToolStripMenuItem sceneToolStripMenuItem;

		// Token: 0x04000096 RID: 150
		private global::System.Windows.Forms.ContextMenuStrip ccsFileContextMenu;

		// Token: 0x04000097 RID: 151
		private global::System.Windows.Forms.TreeView sceneTreeView;

		// Token: 0x04000098 RID: 152
		private global::System.Windows.Forms.ToolStripMenuItem unloadToolStripMenuItem;

		// Token: 0x04000099 RID: 153
		private global::System.Windows.Forms.ToolStripMenuItem addAllToSceneToolStripMenuItem;

		// Token: 0x0400009A RID: 154
		private global::System.Windows.Forms.ContextMenuStrip sceneContextMenu;

		// Token: 0x0400009B RID: 155
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

		// Token: 0x0400009C RID: 156
		private global::System.Windows.Forms.ToolStripMenuItem backfaceCullingToolStripMenuItem;

		// Token: 0x0400009D RID: 157
		private global::System.Windows.Forms.ToolStripLabel tlblRenderMode;

		// Token: 0x0400009E RID: 158
		private global::System.Windows.Forms.ToolStripMenuItem drawAxisMarkerInTopOfViewportToolStripMenuItem;

		// Token: 0x0400009F RID: 159
		private global::System.Windows.Forms.ToolStripMenuItem drawAxisMarkerAtWorldCenterToolStripMenuItem;

		// Token: 0x040000A0 RID: 160
		private global::System.Windows.Forms.ToolStripMenuItem iMOQToolStripMenuItem;

		// Token: 0x040000A1 RID: 161
		private global::System.Windows.Forms.ToolStripMenuItem loadTownToolStripMenuItem;

		// Token: 0x040000A2 RID: 162
		private global::System.Windows.Forms.ToolStripMenuItem drawGridToolStripMenuItem;

		// Token: 0x040000A3 RID: 163
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator4;

		// Token: 0x040000A4 RID: 164
		private global::System.Windows.Forms.ToolStripMenuItem sceneModeToolStripMenuItem;

		// Token: 0x040000A5 RID: 165
		private global::System.Windows.Forms.ToolStripMenuItem drawCollisionMeshesToolStripMenuItem;

		// Token: 0x040000A6 RID: 166
		private global::System.Windows.Forms.ToolStripMenuItem drawDummiesToolStripMenuItem;

		// Token: 0x040000A7 RID: 167
		private global::System.Windows.Forms.ToolStripMenuItem drawLightHelpersToolStripMenuItem;

		// Token: 0x040000A8 RID: 168
		private global::System.Windows.Forms.ToolStripMenuItem vertexColorsToolStripMenuItem;

		// Token: 0x040000A9 RID: 169
		private global::System.Windows.Forms.ToolStripMenuItem viewCCSReportMenuItem;

		// Token: 0x040000AA RID: 170
		private global::System.Windows.Forms.ToolStripMenuItem dumpToOBJToolStripMenuItem;

		// Token: 0x040000AB RID: 171
		private global::System.Windows.Forms.ToolStripMenuItem dumpToSMDToolStripMenuItem;

		// Token: 0x040000AC RID: 172
		private global::System.Windows.Forms.ContextMenuStrip ccsClumpContextMenu;

		// Token: 0x040000AD RID: 173
		private global::System.Windows.Forms.ToolStripMenuItem editBonesToolStripMenuItem;

		// Token: 0x040000AE RID: 174
		private global::System.Windows.Forms.ContextMenuStrip ccsAnimeContextMenu;

		// Token: 0x040000AF RID: 175
		private global::System.Windows.Forms.ToolStripMenuItem addToSceneToolStripMenuItem1;

		// Token: 0x040000B0 RID: 176
		private global::System.Windows.Forms.ContextMenuStrip sceneAnimeContextMenu;

		// Token: 0x040000B1 RID: 177
		private global::System.Windows.Forms.ToolStripMenuItem sceneAnimeContext_RemoveMenuItem;

		// Token: 0x040000B2 RID: 178
		private global::System.Windows.Forms.ToolStripMenuItem setPoseToolStripMenuItem;

		// Token: 0x040000B3 RID: 179
		private global::System.Windows.Forms.ToolStripButton tbtnAll;

		// Token: 0x040000B4 RID: 180
		private global::System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;

		// Token: 0x040000B5 RID: 181
		private global::System.Windows.Forms.ToolStripMenuItem toolStripMenuItem;

		// Token: 0x040000B6 RID: 182
		private global::System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;

		// Token: 0x040000B7 RID: 183
		private global::System.Windows.Forms.ToolStripStatusLabel statusCameraLabel;

		// Token: 0x040000B8 RID: 184
		private global::System.Windows.Forms.ToolStripMenuItem defaultToAxisMovementToolStripMenuItem;

		// Token: 0x040000B9 RID: 185
		private global::System.Windows.Forms.ToolStripMenuItem saveStripMenuItem;

		// Token: 0x040000BA RID: 186
		private global::System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;

		// Token: 0x040000BB RID: 187
		private global::System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;

		// Token: 0x040000BC RID: 188
		private global::System.Windows.Forms.ToolStripMenuItem loadPackedToolStripMenuItem;

		// Token: 0x040000BD RID: 189
		private global::System.Windows.Forms.ToolStripMenuItem loadCCSToolStripMenuItem1;

		// Token: 0x040000BE RID: 190
		private global::System.Windows.Forms.ToolStripMenuItem fastCameraMoveToolStripMenuItem;

		// Token: 0x040000BF RID: 191
		private global::System.Windows.Forms.ToolStripMenuItem keyToolStripMenuItem;

		// Token: 0x040000C0 RID: 192
		private global::System.Windows.Forms.ToolStripMenuItem setSpeedToolStripMenuItem;

		// Token: 0x040000C1 RID: 193
		private global::System.Windows.Forms.ToolStripMenuItem axisToolStripMenuItem;

		// Token: 0x040000C2 RID: 194
		private global::System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;

		// Token: 0x040000C3 RID: 195
		private global::System.Windows.Forms.ToolStripSeparator toolStripSeparator5;

		// Token: 0x040000C4 RID: 196
		private global::System.Windows.Forms.ToolStripMenuItem QuickChangeMoveToolStripMenuItem;
	}
}
