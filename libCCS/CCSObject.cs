using System;
using System.IO;
using System.Windows.Forms;
using OpenTK;

namespace StudioCCS.libCCS
{
	// Token: 0x02000027 RID: 39
	public class CCSObject : CCSBaseObject
	{
		// Token: 0x06000148 RID: 328 RVA: 0x00013C20 File Offset: 0x00011E20
		public CCSObject(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 256;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00013C90 File Offset: 0x00011E90
		public override bool Init()
		{
			bool flag = this.ChildModel != null;
			if (flag)
			{
				this.ChildModel.Init();
			}
			bool flag2 = this.ShadowModel != null;
			if (flag2)
			{
				this.ShadowModel.Init();
			}
			return true;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00013CD8 File Offset: 0x00011ED8
		public override bool DeInit()
		{
			bool flag = this.ChildModel != null;
			if (flag)
			{
				this.ChildModel.DeInit();
			}
			bool flag2 = this.ShadowModel != null;
			if (flag2)
			{
				this.ShadowModel.DeInit();
			}
			return true;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00013D20 File Offset: 0x00011F20
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.ParentObjectID = bStream.ReadInt32();
			this.ModelID = bStream.ReadInt32();
			this.ShadowID = bStream.ReadInt32();
			bool flag = this.ParentFile.GetVersion() > CCSFileHeader.CCSVersion.Gen1;
			if (flag)
			{
				int num = (int)bStream.ReadUInt32();
			}
			return true;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00013D74 File Offset: 0x00011F74
		public override TreeNode ToNode()
		{
			TreeNode node = base.ToNode();
			bool flag = this.ModelID == 0;
			if (flag)
			{
				node.Nodes.Add("Model: <NONE>");
			}
			else
			{
				bool flag2 = this.ChildModel == null;
				if (flag2)
				{
					TreeNode node2 = Util.NonExistantNode(this.ParentFile, this.ModelID);
					node2.Text = "Model: " + node2.Text;
					node.Nodes.Add(node2);
				}
				else
				{
					TreeNode node3 = this.ChildModel.ToNode();
					node3.Text = "Model: " + node3.Text;
					node.Nodes.Add(node3);
				}
			}
			bool flag3 = this.ShadowID == 0;
			if (flag3)
			{
				node.Nodes.Add("Shadow: <NONE>");
			}
			else
			{
				bool flag4 = this.ShadowModel == null;
				if (flag4)
				{
					TreeNode node4 = Util.NonExistantNode(this.ParentFile, this.ShadowID);
					node4.Text = "Shadow: " + node4.Text;
					node.Nodes.Add(node4);
				}
				else
				{
					TreeNode node5 = this.ShadowModel.ToNode();
					node5.Text = "Shadow: " + node5.Text;
					node.Nodes.Add(node5);
				}
			}
			return node;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00013ED8 File Offset: 0x000120D8
		public void SetParentClump(CCSClump _parentClump, int _nodeID)
		{
			this.ParentClump = _parentClump;
			this.NodeID = _nodeID;
			bool flag = this.ModelID != 0;
			if (flag)
			{
				this.ChildModel = this.ParentFile.GetObject<CCSModel>(this.ModelID);
				bool flag2 = this.ChildModel != null;
				if (flag2)
				{
					this.ChildModel.SetClump(_parentClump, this);
				}
			}
			bool flag3 = this.ShadowID == 0;
			if (!flag3)
			{
				this.ShadowModel = this.ParentFile.GetObject<CCSModel>(this.ShadowID);
				bool flag4 = this.ShadowModel != null;
				if (flag4)
				{
					this.ShadowModel.SetClump(_parentClump, this);
				}
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00013F78 File Offset: 0x00012178
		public void FrameForward()
		{
			Matrix4 matrix4 = Matrix4.Identity;
			bool flag = this.ParentObjectID != 0;
			if (flag)
			{
				CCSObject ccsObject = this.ParentFile.GetObject<CCSObject>(this.ParentObjectID);
				bool flag2 = ccsObject != null;
				if (flag2)
				{
					matrix4 = ccsObject.GetFinalMatrix();
				}
			}
			this.SetFinalMatrix(this.GetPoseMatrix() * matrix4);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00013FD0 File Offset: 0x000121D0
		public void Render(Matrix4 mtx, int extraOptions = 0)
		{
			bool flag = this.ChildModel == null;
			if (!flag)
			{
				this.ChildModel.Render(mtx, extraOptions, -1);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00013FFC File Offset: 0x000121FC
		public string GetReport(int level = 0)
		{
			string str = Util.Indent(level, false) + string.Format("Object 0x{0:X}: {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID));
			string str2 = ((this.ParentObjectID != 0) ? (str + Util.Indent(level + 1, false) + string.Format("Parent: 0x{0:X}: {1}\n", this.ParentObjectID, this.ParentFile.GetSubObjectName(this.ParentObjectID))) : (str + Util.Indent(level + 1, false) + "Parent: None\n")) + Util.Indent(level + 1, false) + "Model:\n";
			string str3 = Util.Indent(level + 2, false) + "<None>\n";
			bool flag = this.ChildModel != null;
			if (flag)
			{
				str3 = this.ChildModel.GetReport(level + 2);
			}
			string str4 = str2 + str3 + Util.Indent(level + 1, false) + "Shadow Model:\n";
			string str5 = Util.Indent(level + 2, false) + "<None>\n";
			bool flag2 = this.ShadowModel != null;
			if (flag2)
			{
				str5 = this.ShadowModel.GetReport(level + 2);
			}
			return str4 + str5;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0001412E File Offset: 0x0001232E
		public Matrix4 GetPoseMatrix()
		{
			return this.ParentClump.GetPoseMatrix(this.NodeID);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00014141 File Offset: 0x00012341
		public void SetPose(Vector3 Position, Vector3 Rotation, Vector3 Scale)
		{
			this.ParentClump.SetPose(this.NodeID, Position, Rotation, Scale);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00014158 File Offset: 0x00012358
		public void SetPose(Vector3 Position, Quaternion Rotation, Vector3 Scale)
		{
			this.ParentClump.SetPose(this.NodeID, Position, Rotation, Scale);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0001416F File Offset: 0x0001236F
		public Matrix4 GetFinalMatrix()
		{
			return this.ParentClump.GetFinalMatrix(this.NodeID);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00014182 File Offset: 0x00012382
		public void SetFinalMatrix(Matrix4 newMatrix)
		{
			this.ParentClump.SetFinalMatrix(this.NodeID, newMatrix);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00014197 File Offset: 0x00012397
		public int DumpToObj(StreamWriter fStream, int vOffset, bool split, bool withNormals)
		{
			return (this.ChildModel != null) ? this.ChildModel.DumpToObj(fStream, vOffset, split, withNormals) : vOffset;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000141B4 File Offset: 0x000123B4
		public void DumpToSMD(StreamWriter outf, bool withNormals)
		{
			bool flag = this.ChildModel == null;
			if (!flag)
			{
				this.ChildModel.DumpToSMD(outf, withNormals);
			}
		}

		// Token: 0x040001D9 RID: 473
		public int ParentObjectID = 0;

		// Token: 0x040001DA RID: 474
		public int ModelID = 0;

		// Token: 0x040001DB RID: 475
		public int ShadowID = 0;

		// Token: 0x040001DC RID: 476
		public float Alpha = 0.1f;

		// Token: 0x040001DD RID: 477
		public CCSClump ParentClump = null;

		// Token: 0x040001DE RID: 478
		public CCSModel ChildModel = null;

		// Token: 0x040001DF RID: 479
		public CCSModel ShadowModel = null;

		// Token: 0x040001E0 RID: 480
		public Matrix4 AnimationMatrix = Matrix4.Identity;

		// Token: 0x040001E1 RID: 481
		public int NodeID;
	}
}
