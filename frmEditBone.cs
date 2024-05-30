using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using StudioCCS.libCCS;

namespace StudioCCS
{
	// Token: 0x02000005 RID: 5
	public partial class frmEditBone : Form
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002C74 File Offset: 0x00000E74
		public frmEditBone()
		{
			this.InitializeComponent();
			this.TextBoxes.Add(this.txtPosX);
			this.TextBoxes.Add(this.txtPosY);
			this.TextBoxes.Add(this.txtPosZ);
			this.TextBoxes.Add(this.txtRotX);
			this.TextBoxes.Add(this.txtRotY);
			this.TextBoxes.Add(this.txtRotZ);
			this.TextBoxes.Add(this.txtScaleX);
			this.TextBoxes.Add(this.txtScaleY);
			this.TextBoxes.Add(this.txtScaleZ);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002D5C File Offset: 0x00000F5C
		public void SetClump(CCSClump clump)
		{
			this.OperatingClump = clump;
			this.OperatingFile = clump.ParentFile;
			this.OperatingClump.RenderBones = true;
			string subObjectName = this.OperatingFile.GetSubObjectName(this.OperatingClump.ObjectID);
			TreeNode treeNode = new TreeNode(subObjectName);
			List<TreeNode> treeNodeList = new List<TreeNode>();
			for (int _nodeID = 0; _nodeID < this.OperatingClump.NodeCount; _nodeID++)
			{
				CCSObject ccsObject = this.OperatingClump.GetObject(_nodeID);
				TreeNode node = new TreeNode(this.OperatingFile.GetSubObjectName(ccsObject.ObjectID));
				node.Tag = new frmEditBone.BoneNodeTag
				{
					Bone = ccsObject
				};
				treeNodeList.Add(node);
				int parentObjectId = ccsObject.ParentObjectID;
				bool flag = parentObjectId != 0;
				if (flag)
				{
					int index = this.OperatingClump.SearchNodeID(parentObjectId);
					bool flag2 = index == -1;
					if (flag2)
					{
						treeNode.Nodes.Add(node);
					}
					else
					{
						treeNodeList[index].Nodes.Add(node);
					}
				}
				else
				{
					treeNode.Nodes.Add(node);
				}
			}
			foreach (object obj in treeNode.Nodes)
			{
				TreeNode node2 = (TreeNode)obj;
				this.treeBones.Nodes.Add(node2);
			}
			this.Text = string.Format("Edit Bones for {0}...", subObjectName);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002EF0 File Offset: 0x000010F0
		private void BtnUpdateClick(object sender, EventArgs e)
		{
			float num = 0.0174533f;
			bool flag = true;
			float[] numArray = new float[9];
			int index = 0;
			while (index < this.TextBoxes.Count)
			{
				TextBox textBox = this.TextBoxes[index];
				float result = 0f;
				try
				{
					float.TryParse(textBox.Text, out result);
				}
				catch
				{
					textBox.BackColor = Color.Red;
					flag = false;
					goto IL_67;
				}
				goto IL_53;
				IL_67:
				index++;
				continue;
				IL_53:
				textBox.BackColor = Color.White;
				numArray[index] = result;
				goto IL_67;
			}
			bool flag2 = !flag;
			if (!flag2)
			{
				Vector3 vector3_ = new Vector3(numArray[0], numArray[1], numArray[2]);
				Vector3 vector3_2 = new Vector3(numArray[3] * num, numArray[4] * num, numArray[5] * num);
				Vector3 vector3_3 = new Vector3(numArray[6], numArray[7], numArray[8]);
				bool flag3 = this.OperatingObject != null;
				if (flag3)
				{
					bool flag4 = this.OperatingFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
					if (flag4)
					{
						this.OperatingClump.PosePositions[this.OperatingObject.NodeID] = vector3_;
						this.OperatingClump.PoseRotations[this.OperatingObject.NodeID] = vector3_2;
						this.OperatingClump.PoseScales[this.OperatingObject.NodeID] = vector3_3;
					}
					else
					{
						this.OperatingClump.BindPositions[this.OperatingObject.NodeID] = vector3_;
						this.OperatingClump.BindRotations[this.OperatingObject.NodeID] = vector3_2;
						this.OperatingClump.BindScales[this.OperatingObject.NodeID] = vector3_3;
					}
				}
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000030B0 File Offset: 0x000012B0
		private void LoadPoseToolStripMenuItemClick(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Bin Files (*.bin)|*.bin|All Files (*.*)|*.*";
			openFileDialog.Title = "Load Clump Pose";
			bool flag = openFileDialog.ShowDialog() != DialogResult.OK;
			if (!flag)
			{
				this.OperatingClump.LoadPose(openFileDialog.FileName);
				Logger.LogInfo(string.Format("Loaded pose for {0} from {1}.\n", this.OperatingFile.GetSubObjectName(this.OperatingClump.ObjectID), openFileDialog.FileName), Logger.LogType.LogAll, "LoadPoseToolStripMenuItemClick", 182);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003138 File Offset: 0x00001338
		private void SavePoseToolStripMenuItemClick(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Bin Files (*.bin)|*.bin|All Files (*.*)|*.*";
			saveFileDialog.Title = "Save Clump Pose";
			bool flag = saveFileDialog.ShowDialog() != DialogResult.OK;
			if (!flag)
			{
				this.OperatingClump.SavePose(saveFileDialog.FileName);
				Logger.LogInfo(string.Format("Saved pose for {0} to {1}.\n", this.OperatingFile.GetSubObjectName(this.OperatingClump.ObjectID), saveFileDialog.FileName), Logger.LogType.LogAll, "SavePoseToolStripMenuItemClick", 195);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000031C0 File Offset: 0x000013C0
		private void TreeBonesAfterSelect(object sender, TreeViewEventArgs e)
		{
			frmEditBone.BoneNodeTag tag = (frmEditBone.BoneNodeTag)e.Node.Tag;
			bool flag = tag == null;
			if (!flag)
			{
				this.OperatingObject = tag.Bone;
				this.OperatingClump.SelectedBoneID = this.OperatingObject.NodeID;
				Vector3 vector3_ = this.OperatingClump.BindPositions[this.OperatingObject.NodeID];
				Vector3 vector3_2 = this.OperatingClump.BindRotations[this.OperatingObject.NodeID];
				Vector3 vector3_3 = this.OperatingClump.BindScales[this.OperatingObject.NodeID];
				bool flag2 = this.OperatingFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
				if (flag2)
				{
					vector3_ = this.OperatingClump.PosePositions[this.OperatingObject.NodeID];
					vector3_2 = this.OperatingClump.PoseRotations[this.OperatingObject.NodeID];
					vector3_3 = this.OperatingClump.PoseScales[this.OperatingObject.NodeID];
				}
				this.txtPosX.Text = vector3_.X.ToString();
				this.txtPosY.Text = vector3_.Y.ToString();
				this.txtPosZ.Text = vector3_.Z.ToString();
				float num = 3.141593f;
				this.txtRotX.Text = (vector3_2.X * 180f / num).ToString();
				this.txtRotY.Text = (vector3_2.Y * 180f / num).ToString();
				this.txtRotZ.Text = (vector3_2.Z * 180f / num).ToString();
				this.txtScaleX.Text = vector3_3.X.ToString();
				this.txtScaleY.Text = vector3_3.Y.ToString();
				this.txtScaleZ.Text = vector3_3.Z.ToString();
				this.lblBoneName.Text = this.OperatingFile.GetSubObjectName(this.OperatingObject.ObjectID);
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000033F4 File Offset: 0x000015F4
		private void ClearRotationValuesToolStripMenuItemClick(object sender, EventArgs e)
		{
			for (int index = 0; index < this.OperatingClump.NodeCount; index++)
			{
				bool flag = this.OperatingFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
				if (flag)
				{
					this.OperatingClump.PoseRotations[index] = Vector3.Zero;
				}
				else
				{
					this.OperatingClump.BindRotations[index] = Vector3.Zero;
					this.OperatingClump.PoseQuats[index] = new Quaternion(0f, 0f, 0f, 1f);
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000348F File Offset: 0x0000168F
		private void FrmEditBoneFormClosed(object sender, FormClosedEventArgs e)
		{
			this.OperatingClump.RenderBones = false;
			this.OperatingClump.SelectedBoneID = -1;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000034AC File Offset: 0x000016AC
		private void ClearScaleValuesToolStripMenuItemClick(object sender, EventArgs e)
		{
			for (int index = 0; index < this.OperatingClump.NodeCount; index++)
			{
				this.OperatingClump.BindScales[index] = Vector3.One;
				this.OperatingClump.PoseScales[index] = Vector3.One;
			}
		}

		// Token: 0x0400000F RID: 15
		public CCSFile OperatingFile = null;

		// Token: 0x04000010 RID: 16
		public CCSClump OperatingClump = null;

		// Token: 0x04000011 RID: 17
		public CCSObject OperatingObject = null;

		// Token: 0x04000012 RID: 18
		public List<TextBox> TextBoxes = new List<TextBox>();

		// Token: 0x02000033 RID: 51
		public class BoneNodeTag
		{
			// Token: 0x04000216 RID: 534
			public CCSObject Bone;
		}
	}
}
