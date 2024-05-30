using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCSFileExplorerWV;
using OpenTK;
using StudioCCS.libCCS;
using StudioCCS.libETC;
using StudioCCS.libSTUDIOGEN2_5;
using static System.Windows.Forms.LinkLabel;

namespace StudioCCS
{
	// Token: 0x0200000E RID: 14
	public partial class MainForm : Form
	{
		// Token: 0x06000047 RID: 71 RVA: 0x00008BEC File Offset: 0x00006DEC
		public MainForm()
		{
			base.Load += this.MainForm_Load;
			this.InitializeComponent();
			Logger.SetLogControl(this.logView);
			this.tbtnPreview.Checked = true;
			this.texturedToolStripMenuItem.Checked = true;
			this.AllowDrop = true;
			this.glViewport = new GLControl();
			this.viewportSplit.Panel2.Controls.Add(this.glViewport);
			Toolkit.Init();
			this.glViewport.CreateControl();
			this.glViewport.CreateGraphics();
			this.glViewport.Dock = DockStyle.Fill;
			this.glViewport.BringToFront();
			Scene.Init(this.glViewport);
			this.glViewport.KeyDown += this.MainFormKeyDown;
			this.glViewport.KeyUp += this.MainFormKeyUp;
			this.glViewport.MouseMove += this.PicViewportMouseMove;
			this.glViewport.MouseWheel += this.PicViewportMouseWheel;
			this.sceneTreeView.Nodes.Add(this.SceneAnimationNode);
			this.renderTimer.Enabled = true;
			this.updateRecentMenu();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00008DC0 File Offset: 0x00006FC0
		[DebuggerStepThrough]
        private async Task ReadConfigFile()
        {
            await Task.Yield(); // Você deve substituir isso com sua lógica real de leitura de arquivo.
        }

        private async Task SaveConfigFile()
        {
            await Task.Yield(); // Você deve substituir isso com sua lógica real de salvamento de arquivo.
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await Task.Yield(); // Você deve substituir isso com sua lógica de carregamento real.
        }

        private async void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            await Task.Yield(); // Você deve substituir isso com sua lógica de tratamento de teclado real.
        }

        // Token: 0x0600004C RID: 76 RVA: 0x00008ED7 File Offset: 0x000070D7
        private void LoadCCSToolStripMenuItemClick(object sender, EventArgs e)
		{
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00008EDC File Offset: 0x000070DC
		private void MenuItemClickHandler(object sender, EventArgs e)
		{
			try
			{
				ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
				string[] daArgs = new string[]
				{
					clickedItem.Text
				};
				string fileType = clickedItem.Text.Substring(clickedItem.Text.Length - 3);
				fileType = Convert.ToString(fileType).ToLower();
				bool flag = fileType == "ccs";
				if (flag)
				{
					this.loadPackedCCS(daArgs);
				}
				else
				{
					bool flag2 = fileType == "tmp";
					if (flag2)
					{
						this.LoadFiles(daArgs);
					}
				}
			}
			catch
			{
				MessageBox.Show("An error occurred while trying to open the file. Did you move or delete it?", this.studioVersionString, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00008F8C File Offset: 0x0000718C
		private void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Application.Exit();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00008F94 File Offset: 0x00007194
		public void updateRecentMenu()
		{
			bool flag = File.Exists("fileslog.txt");
			if (flag)
			{
				this.recentToolStripMenuItem.DropDownItems.Clear();
				string[] filesArray = File.ReadAllLines("fileslog.txt");
				Array.Reverse(filesArray);
				ToolStripMenuItem[] items = new ToolStripMenuItem[2];
				for (int i = 0; i < filesArray.Length; i++)
				{
					ToolStripMenuItem item = new ToolStripMenuItem();
					item.Name = "option" + i.ToString();
					item.Text = filesArray[i];
					item.Click += this.MenuItemClickHandler;
					this.recentToolStripMenuItem.DropDownItems.Add(item);
				}
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00009048 File Offset: 0x00007248
		public static void UnpackToFolder(string filename, string folder, ToolStripProgressBar pb1 = null, ToolStripStatusLabel strip = null)
		{
			FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
			fs.Seek(0L, SeekOrigin.End);
			long size = fs.Position;
			fs.Seek(0L, SeekOrigin.Begin);
			byte[] buff = new byte[4];
			bool flag = pb1 != null;
			if (flag)
			{
				pb1.Maximum = (int)size;
			}
			int fileindex = 0;
			while (fs.Position < size)
			{
				fs.Read(buff, 0, 4);
				bool flag2 = FileHelper.isGzipMagic(buff, 0);
				if (flag2)
				{
					int pos = (int)fs.Position - 4;
					int start = pos;
					while ((long)pos < size)
					{
						pos += 2048;
						fs.Seek(2044L, SeekOrigin.Current);
						fs.Read(buff, 0, 4);
						bool flag3 = FileHelper.isGzipMagic(buff, 0);
						if (flag3)
						{
							fs.Seek(-4L, SeekOrigin.Current);
							break;
						}
					}
					fs.Seek((long)start, SeekOrigin.Begin);
					buff = new byte[pos - start];
					fs.Read(buff, 0, pos - start);
					buff = FileHelper.unzipArray(buff);
					string name = "";
					int tpos = 12;
					while (buff[tpos] > 0)
					{
						string str = name;
						char c = (char)buff[tpos++];
						name = str + c.ToString();
					}
					File.WriteAllBytes(folder + name + ".tmp", buff);
					fileindex++;
					bool flag4 = pb1 != null;
					if (flag4)
					{
						pb1.Value = start;
						strip.Text = name;
						Application.DoEvents();
					}
					buff = new byte[4];
				}
				else
				{
					fs.Seek(2044L, SeekOrigin.Current);
				}
			}
			bool flag5 = pb1 != null;
			if (flag5)
			{
				pb1.Value = 0;
				strip.Text = "";
			}
			fs.Close();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000920C File Offset: 0x0000740C
		public void loadPackedCCS(string[] fileNames)
		{
			foreach (string fileName in fileNames)
			{
				this.addToRecent(fileName);
				string safeFileName = Path.GetFileName(fileName);
				string path = Directory.GetCurrentDirectory() + "/workspace/" + safeFileName + "/";
				DirectoryInfo lastfolder = Directory.CreateDirectory(path);
				BINHelper.UnpackToFolder(fileName, path, null, null);
				DirectoryInfo pathInfo = new DirectoryInfo(path);
				List<string> filesToAdd = new List<string>();
				foreach (FileInfo subFile in pathInfo.GetFiles())
				{
					filesToAdd.Add(subFile.FullName);
				}
				this.LoadFiles(filesToAdd.ToArray());
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000092C4 File Offset: 0x000074C4
		private void addToRecent(string fileNames)
		{
            List<string> allFileLines = new List<string>();
			allFileLines = File.Exists("fileslog.txt") == true ? File.ReadAllLines("fileslog.txt").ToList() : new List<string>();
            if (allFileLines.Count >= 9)
            {
                allFileLines.RemoveRange(9, allFileLines.Count - 9);

                File.WriteAllLines("fileslog.txt", allFileLines);
            }
            if (File.Exists("fileslog.txt") == false || !allFileLines.All(line => line.Contains(fileNames)))
			{
                File.AppendAllText("fileslog.txt", fileNames + Environment.NewLine);
                this.updateRecentMenu();
            }
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000092E4 File Offset: 0x000074E4
		private void LoadFiles(string[] fileNames)
		{
			foreach (string fileName in fileNames)
			{
				TreeNode node = Scene.LoadCCSFile(fileName);
				bool flag = node != null;
				if (flag)
				{
					this.ccsTree.Nodes.Add(node);
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000932C File Offset: 0x0000752C
		private void TbtnSceneCheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.tbtnScene.Checked;
			if (@checked)
			{
				this.tbtnPreview.CheckedChanged -= this.TbtnPreviewCheckedChanged;
				this.tbtnAll.CheckedChanged -= this.TbtnAllCheckedChanged;
				this.tbtnPreview.Checked = false;
				this.tbtnAll.Checked = false;
				this.tbtnPreview.CheckedChanged += this.TbtnPreviewCheckedChanged;
				this.tbtnAll.CheckedChanged += this.TbtnAllCheckedChanged;
				Scene.SceneDisplay = Scene.SceneMode.Scene;
				this.ccsTree.Visible = false;
				this.sceneTreeView.Visible = true;
				this.ccsPropertyGrid.Visible = true;
				this.treeSplit.Panel2Collapsed = false;
				this.viewportSplit.Panel1Collapsed = false;
			}
			else
			{
				this.tbtnScene.Checked = true;
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00009420 File Offset: 0x00007620
		private void TbtnPreviewCheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.tbtnPreview.Checked;
			if (@checked)
			{
				this.tbtnScene.CheckedChanged -= this.TbtnSceneCheckedChanged;
				this.tbtnAll.CheckedChanged -= this.TbtnAllCheckedChanged;
				this.tbtnScene.Checked = false;
				this.tbtnAll.Checked = false;
				this.tbtnScene.CheckedChanged += this.TbtnSceneCheckedChanged;
				this.tbtnAll.CheckedChanged += this.TbtnAllCheckedChanged;
				this.sceneTreeView.Visible = false;
				this.ccsPropertyGrid.Visible = false;
				this.ccsTree.Visible = true;
				this.treeSplit.Panel2Collapsed = true;
				this.viewportSplit.Panel1Collapsed = false;
				Scene.SceneDisplay = Scene.SceneMode.Preview;
			}
			else
			{
				this.tbtnPreview.Checked = true;
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00009514 File Offset: 0x00007714
		private void TbtnAllCheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.tbtnAll.Checked;
			if (@checked)
			{
				this.tbtnScene.CheckedChanged -= this.TbtnSceneCheckedChanged;
				this.tbtnPreview.CheckedChanged -= this.TbtnPreviewCheckedChanged;
				this.tbtnScene.Checked = false;
				this.tbtnPreview.Checked = false;
				this.tbtnScene.CheckedChanged += this.TbtnSceneCheckedChanged;
				this.tbtnPreview.CheckedChanged += this.TbtnPreviewCheckedChanged;
				this.sceneTreeView.Visible = false;
				this.ccsPropertyGrid.Visible = false;
				this.ccsTree.Visible = false;
				this.treeSplit.Panel2Collapsed = true;
				this.viewportSplit.Panel1Collapsed = true;
				Scene.SceneDisplay = Scene.SceneMode.All;
			}
			else
			{
				this.tbtnAll.Checked = true;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00009606 File Offset: 0x00007806
		private void WireframeToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawWireframe = this.wireframeToolStripMenuItem.Checked;
			this.SetRenderModeLabel();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00009620 File Offset: 0x00007820
		private void VertexColorsToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawVertexColors = this.vertexColorsToolStripMenuItem.Checked;
			this.SetRenderModeLabel();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000963A File Offset: 0x0000783A
		private void SmoothShadedToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawVertexNormals = this.smoothShadedToolStripMenuItem.Checked;
			this.SetRenderModeLabel();
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00009654 File Offset: 0x00007854
		private void TexturedToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawTextures = this.texturedToolStripMenuItem.Checked;
			this.SetRenderModeLabel();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000966E File Offset: 0x0000786E
		private void BackfaceCullingToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.BackfaceCull = this.backfaceCullingToolStripMenuItem.Checked;
			this.SetRenderModeLabel();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00009688 File Offset: 0x00007888
		private void SetRenderModeLabel()
		{
			this.tlblRenderMode.Text = "";
			bool flag = (Scene.GetRenderMode() & 15) == 0;
			if (flag)
			{
				this.tlblRenderMode.Text = "None";
			}
			else
			{
				List<string> values = new List<string>();
				bool drawWireframe = Scene.DrawWireframe;
				if (drawWireframe)
				{
					values.Add("Wireframe");
				}
				bool drawVertexColors = Scene.DrawVertexColors;
				if (drawVertexColors)
				{
					values.Add("Vertex Colors");
				}
				bool drawVertexNormals = Scene.DrawVertexNormals;
				if (drawVertexNormals)
				{
					values.Add("Vertex Normals");
				}
				bool drawTextures = Scene.DrawTextures;
				if (drawTextures)
				{
					values.Add("Textured");
				}
				this.tlblRenderMode.Text = string.Join("/", values);
				bool @checked = this.backfaceCullingToolStripMenuItem.Checked;
				if (@checked)
				{
					ToolStripLabel toolStripLabel = this.tlblRenderMode;
					toolStripLabel.Text += " (Backface Culling)";
				}
				else
				{
					ToolStripLabel toolStripLabel2 = this.tlblRenderMode;
					toolStripLabel2.Text += " (No Backface Culling)";
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00009790 File Offset: 0x00007990
		private void DrawGridToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawViewGrid = this.drawGridToolStripMenuItem.Checked;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000097A2 File Offset: 0x000079A2
		private void DrawCollisionMeshesToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawCollisionMeshes = this.drawCollisionMeshesToolStripMenuItem.Checked;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000097B4 File Offset: 0x000079B4
		private void DrawDummiesToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawDummyHelpers = this.drawDummiesToolStripMenuItem.Checked;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000097C6 File Offset: 0x000079C6
		private void DrawLightHelpersToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawLightHelpers = this.drawLightHelpersToolStripMenuItem.Checked;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000097D8 File Offset: 0x000079D8
		private void DrawAxisMarkerInTopOfViewportToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawViewAxis = this.drawAxisMarkerInTopOfViewportToolStripMenuItem.Checked;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000097EB File Offset: 0x000079EB
		private void DrawAxisMarkerAtWorldCenterToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			Scene.DrawWorldCenter = this.drawAxisMarkerAtWorldCenterToolStripMenuItem.Checked;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000097FE File Offset: 0x000079FE
		private void PicViewportMouseWheel(object sender, MouseEventArgs e)
		{
			Scene.MouseWheel(e);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00009807 File Offset: 0x00007A07
		private void PicViewportMouseMove(object sender, MouseEventArgs e)
		{
			Scene.MouseMove(e);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00009810 File Offset: 0x00007A10
		private void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			Scene.DeInit();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00009810 File Offset: 0x00007A10
		private void MainFormVisibleChanged(object sender, FormClosingEventArgs e)
		{
			Scene.DeInit();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00009818 File Offset: 0x00007A18
		private void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			Scene.KeyPress(e);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00009821 File Offset: 0x00007A21
		private void MainFormKeyUp(object sender, KeyEventArgs e)
		{
			Scene.KeyRelease(e);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000982C File Offset: 0x00007A2C
		private void MainFormDragDrop(object sender, DragEventArgs e)
		{
			this.tmp_tchurusbangus.Clear();
			this.ccs_tchurusbangus.Clear();
			string[] tchurusbangus = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (string item in tchurusbangus)
			{
				string fileType = item.Substring(item.Length - 3);
				fileType = Convert.ToString(fileType).ToLower();
				bool flag = fileType == "tmp";
				if (flag)
				{
					this.addToRecent(item);
					this.tmp_tchurusbangus.Add(item);
				}
				else
				{
					bool flag2 = fileType == "ccs";
					if (flag2)
					{
						this.ccs_tchurusbangus.Add(item);
					}
				}
			}
			bool flag3 = this.tmp_tchurusbangus.Count != 0;
			if (flag3)
			{
				this.LoadFiles(this.tmp_tchurusbangus.ToArray());
			}
			bool flag4 = this.ccs_tchurusbangus.Count != 0;
			if (flag4)
			{
				this.loadPackedCCS(this.ccs_tchurusbangus.ToArray());
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000993C File Offset: 0x00007B3C
		private void MainFormDragEnter(object sender, DragEventArgs e)
		{
			bool flag = !e.Data.GetDataPresent(DataFormats.FileDrop);
			if (!flag)
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000996C File Offset: 0x00007B6C
		private void RenderTimerTick(object sender, EventArgs e)
		{
			Scene.Render();
			ArcBallCamera arcBallCamera = Scene.CurrentCamera();
			this.statusCameraLabel.Text = string.Format("Camera: {0}, {1}, {2}", string.Format("Rotation: {0}, {1}, {2}", arcBallCamera.Rotation.X, arcBallCamera.Rotation.Y, arcBallCamera.Rotation.Z), string.Format("Target: {0}, {2}, {1}", arcBallCamera.Target.X, arcBallCamera.Target.Y, arcBallCamera.Target.Z), string.Format("Distance: {0}", arcBallCamera.Distance));
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00009A28 File Offset: 0x00007C28
		private void CcsTreeNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			bool flag = e.Button != MouseButtons.Right;
			if (!flag)
			{
				this.ccsTree.SelectedNode = e.Node;
				TreeNode node = e.Node;
				bool flag2 = node.Tag != null;
				if (flag2)
				{
					TreeNodeTag tag = (TreeNodeTag)node.Tag;
					CCSFile file = tag.File;
					bool flag3 = tag.Type == TreeNodeTag.NodeType.File;
					if (flag3)
					{
						this.ccsFileContextMenu.Show(this.ccsTree, e.X, e.Y);
					}
					else
					{
						bool flag4 = tag.Type == TreeNodeTag.NodeType.Main;
						if (flag4)
						{
							bool flag5 = tag.ObjectType == 2304;
							if (flag5)
							{
								this.ccsClumpContextMenu.Show(this.ccsTree, e.X, e.Y);
							}
							else
							{
								bool flag6 = tag.ObjectType == 1792;
								if (flag6)
								{
									this.ccsAnimeContextMenu.Show(this.ccsTree, e.X, e.Y);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00009B34 File Offset: 0x00007D34
		private void UnloadToolStripMenuItemClick(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.ccsTree.SelectedNode;
			bool flag = selectedNode.Tag == null;
			if (!flag)
			{
				Scene.UnloadCCSFile(((TreeNodeTag)selectedNode.Tag).File);
				this.ccsTree.SelectedNode = null;
				this.ccsTree.Nodes.Remove(selectedNode);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00009B94 File Offset: 0x00007D94
		private void CcsTreeAfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			Scene.SelectedPreviewItemTag = (TreeNodeTag)node.Tag;
			bool flag = node.Tag == null;
			if (!flag)
			{
				TreeNodeTag tag = (TreeNodeTag)node.Tag;
				bool flag2 = tag.ObjectType == 1792;
				if (flag2)
				{
					CCSAnime ccsAnime = tag.File.GetObject<CCSAnime>(tag.ObjectID);
					bool flag3 = ccsAnime != null;
					if (flag3)
					{
						ccsAnime.HasEnded = false;
						ccsAnime.CurrentFrame = 0;
					}
				}
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00009C18 File Offset: 0x00007E18
		private void ViewCCSReportMenuItemClick(object sender, EventArgs e)
		{
			TreeNodeTag tag = (TreeNodeTag)this.ccsTree.SelectedNode.Tag;
			bool flag = tag == null;
			if (!flag)
			{
				frmInfo frmInfo = new frmInfo();
				string report = tag.File.GetReport();
				frmInfo.SetReportText(report);
				frmInfo.Show();
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00009C68 File Offset: 0x00007E68
		private void LoadMatrixMenuItemClick(object sender, EventArgs e)
		{
			TreeNodeTag tag = (TreeNodeTag)this.ccsTree.SelectedNode.Tag;
			bool flag = tag == null;
			if (!flag)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Bin Files (*.bin)|*.bin|All Files(*.*)|*.*";
				openFileDialog.Title = "Select Binary File to load";
				bool flag2 = openFileDialog.ShowDialog() != DialogResult.OK;
				if (!flag2)
				{
					CCSClump @object = tag.File.GetObject<CCSClump>(tag.ObjectID);
					if (@object != null)
					{
						@object.LoadMatrixList(openFileDialog.FileName);
					}
				}
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00009CEC File Offset: 0x00007EEC
		private void EditBonesToolStripMenuItemClick(object sender, EventArgs e)
		{
			TreeNodeTag tag = (TreeNodeTag)this.ccsTree.SelectedNode.Tag;
			bool flag = tag == null;
			if (!flag)
			{
				frmEditBone frmEditBone = new frmEditBone();
				frmEditBone.SetClump(tag.File.GetObject<CCSClump>(tag.ObjectID));
				frmEditBone.Show();
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00009D40 File Offset: 0x00007F40
		private void AddToSceneToolStripMenuItem1Click(object sender, EventArgs e)
		{
			TreeNodeTag tag = (TreeNodeTag)this.ccsTree.SelectedNode.Tag;
			bool flag = tag == null;
			if (!flag)
			{
				CCSAnime anime = tag.File.GetObject<CCSAnime>(tag.ObjectID);
				bool flag2 = anime != null;
				if (flag2)
				{
					Scene.AddAnime(anime);
					this.SceneAnimationNode.Nodes.Add(anime.ToNode());
				}
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00009DA8 File Offset: 0x00007FA8
		private void SetPoseToolStripMenuItemClick(object sender, EventArgs e)
		{
			TreeNodeTag tag = (TreeNodeTag)this.ccsTree.SelectedNode.Tag;
			if (tag != null)
			{
				CCSAnime @object = tag.File.GetObject<CCSAnime>(tag.ObjectID);
				if (@object != null)
				{
					@object.FrameForward();
				}
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00009DF0 File Offset: 0x00007FF0
		private void DumpToOBJToolStripMenuItemClick(object sender, EventArgs e)
		{
			using (frmExportToOBJ frmExportToObj = new frmExportToOBJ())
			{
				bool flag = frmExportToObj.ShowDialog() != DialogResult.OK;
				if (!flag)
				{
					Scene.DumpToObj(frmExportToObj.txtExportPath.Text, frmExportToObj.chkExportCollision.Checked, frmExportToObj.chkSplitSubModels.Checked, frmExportToObj.chkSplitCollision.Checked, frmExportToObj.chkModelWithNormals.Checked, frmExportToObj.chkExportDummies.Checked, frmExportToObj.chkDumpAnime.Checked);
				}
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00009E88 File Offset: 0x00008088
		private void DumpToSMDToolStripMenuItemClick(object sender, EventArgs e)
		{
			using (frmExportToOBJ frmExportToObj = new frmExportToOBJ())
			{
				frmExportToObj.chkSplitCollision.Enabled = false;
				frmExportToObj.chkSplitSubModels.Enabled = false;
				frmExportToObj.chkExportCollision.Enabled = false;
				frmExportToObj.chkExportDummies.Enabled = false;
				frmExportToObj.Text = "Export to SMD...";
				bool flag = frmExportToObj.ShowDialog() != DialogResult.OK;
				if (!flag)
				{
					Scene.DumpToSMD(frmExportToObj.txtExportPath.Text, frmExportToObj.chkModelWithNormals.Checked);
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00009F2C File Offset: 0x0000812C
		private void SceneTreeViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			bool flag = e.Button != MouseButtons.Right;
			if (!flag)
			{
				this.sceneTreeView.SelectedNode = e.Node;
				TreeNode node = e.Node;
				bool flag2 = node.Tag != null;
				if (flag2)
				{
					TreeNodeTag tag = (TreeNodeTag)node.Tag;
					CCSFile file = tag.File;
					bool flag3 = tag.Type == TreeNodeTag.NodeType.Main && tag.ObjectType == 1792;
					if (flag3)
					{
						this.sceneAnimeContextMenu.Show(this.sceneTreeView, e.X, e.Y);
					}
				}
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00009FCC File Offset: 0x000081CC
		private void SceneAnimeContext_RemoveMenuItemClick(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.sceneTreeView.SelectedNode;
			TreeNodeTag tag = (TreeNodeTag)selectedNode.Tag;
			bool flag = tag == null;
			if (!flag)
			{
				this.sceneTreeView.Nodes.Remove(selectedNode);
				CCSAnime anime = tag.File.GetObject<CCSAnime>(tag.ObjectID);
				bool flag2 = anime != null;
				if (flag2)
				{
					Scene.RemoveAnime(anime);
				}
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000A069 File Offset: 0x00008269
		private void toolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("This is a fork of NCDyson's StudioCCS for reading CCSF Gen 2.5 files.\nMade by Rapha#9426 and msf#1282. Testers are always welcome! ^-^", this.studioVersionString);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000A080 File Offset: 0x00008280
		private void loadPackedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "TMP Files (*.tmp)|*.tmp|All Files(*.*)|*.*";
			openFileDialog.Title = "Select TMP Files to load";
			openFileDialog.Multiselect = true;
			bool flag = openFileDialog.ShowDialog() != DialogResult.OK;
			if (!flag)
			{
				foreach (string fileName in openFileDialog.FileNames)
				{
					this.addToRecent(fileName);
				}
				this.LoadFiles(openFileDialog.FileNames);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000A0FC File Offset: 0x000082FC
		private void loadCCSToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "CCS Files (*.ccs)|*.ccs|All Files(*.*)|*.*";
			openFileDialog.Title = "Select CCS Files to load";
			openFileDialog.Multiselect = true;
			bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
			if (flag)
			{
				this.loadPackedCCS(openFileDialog.FileNames);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000A14C File Offset: 0x0000834C
		private void keyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string message = "When confirming, press a key on your keyboard to define which key will be used in fast speed.";
			MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
			DialogResult result = MessageBox.Show(message, this.studioVersionString, buttons);
			bool flag = result == DialogResult.OK;
			if (flag)
			{
				this.isChangingKey = true;
			}
		}

        private async void setSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Yield(); // Lógica real do evento setSpeedToolStripMenuItem_Click.
        }

        private async void axisToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            await Task.Yield(); // Lógica real do evento axisToolStripMenuItem_CheckedChanged.
        }

        private async void defaultToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            await Task.Yield(); // Lógica real do evento defaultToolStripMenuItem_CheckedChanged.
        }

        // Token: 0x06000080 RID: 128 RVA: 0x0000A25C File Offset: 0x0000845C
        private void keySHIFTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string message = "When confirming, press a key on your keyboard to define which key will be used in quick changing movement.";
			MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
			DialogResult result = MessageBox.Show(message, this.studioVersionString, buttons);
			bool flag = result == DialogResult.OK;
			if (flag)
			{
				this.isChangingQuickKey = true;
			}
		}

		// Token: 0x0400007D RID: 125
		private TreeNode SceneAnimationNode = new TreeNode("Animations");

		// Token: 0x0400007E RID: 126
		private GLControl glViewport = null;

		// Token: 0x040000C5 RID: 197
		public const float studioVersion = 2.4f;

		// Token: 0x040000C6 RID: 198
		public string studioVersionString = "Modded StudioCCS " + 2.4f.ToString(new CultureInfo("en-US"));

		// Token: 0x040000C7 RID: 199
		public string studioPath = Directory.GetCurrentDirectory();

		// Token: 0x040000C8 RID: 200
		public bool isChangingKey = false;

		// Token: 0x040000C9 RID: 201
		public bool isChangingQuickKey = false;

		// Token: 0x040000CA RID: 202
		public configFileClass configFile = new configFileClass();

		// Token: 0x040000CB RID: 203
		public List<string> tmp_tchurusbangus = new List<string>();

		// Token: 0x040000CC RID: 204
		public List<string> ccs_tchurusbangus = new List<string>();
	}
}
