using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OpenTK;

namespace StudioCCS.libCCS
{
	// Token: 0x02000014 RID: 20
	public class CCSAnime : CCSBaseObject
	{
		// Token: 0x060000AC RID: 172 RVA: 0x0000BE74 File Offset: 0x0000A074
		public CCSAnime(int _objectID, CCSFile _parentFile)
		{
			this.ObjectID = _objectID;
			this.ParentFile = _parentFile;
			this.ObjectType = 1792;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		public override bool Init()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		public override bool DeInit()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000BEDC File Offset: 0x0000A0DC
		public override bool Read(BinaryReader bStream, int sectionSize)
		{
			this.FrameCount = bStream.ReadInt32();
			CCSAnime.AnimationFrame animationFrame = new CCSAnime.AnimationFrame(0);
			this.Frames.Add(animationFrame);
			bStream.ReadInt32();
			int _frameNumber;
			for (;;)
			{
				int position = (int)bStream.BaseStream.Position;
				int num = bStream.ReadInt32() & 65535;
				int num2 = bStream.ReadInt32();
				int num3 = num;
				int num4 = num3;
				if (num4 <= 1537)
				{
					if (num4 != 258)
					{
						if (num4 != 514)
						{
							if (num4 != 1537)
							{
								goto IL_254;
							}
							CCSAnime.AmbientLightKeyFrame ambientLightKeyFrame = new CCSAnime.AmbientLightKeyFrame();
							ambientLightKeyFrame.Read(bStream);
							animationFrame.KeyFrames.Add(ambientLightKeyFrame);
						}
						else
						{
							CCSAnime.MaterialController materialController = new CCSAnime.MaterialController(this.ParentFile, this);
							materialController.Read(bStream, num2);
							this.Controllers.Add(materialController);
						}
					}
					else
					{
						bool flag = this.ParentFile.GetVersion() == CCSFileHeader.CCSVersion.Gen1;
						if (flag)
						{
							CCSAnime.ObjectController objectController = new CCSAnime.ObjectController(this.ParentFile, this);
							objectController.Read(bStream, num2);
							this.Controllers.Add(objectController);
						}
						else
						{
							bool flag2 = this.ParentFile.GetVersion() > CCSFileHeader.CCSVersion.Gen1;
							if (flag2)
							{
								CCSAnime.ObjectController_Gen2 objectControllerGen2 = new CCSAnime.ObjectController_Gen2(this.ParentFile, this);
								objectControllerGen2.Read(bStream, num2);
								this.Controllers.Add(objectControllerGen2);
							}
						}
					}
				}
				else if (num4 <= 1545)
				{
					if (num4 != 1539)
					{
						if (num4 != 1545)
						{
							goto IL_254;
						}
						CCSAnime.OmniLightController omniLightController = new CCSAnime.OmniLightController(this.ParentFile, this);
						omniLightController.Read(bStream, num2);
						this.Controllers.Add(omniLightController);
					}
					else
					{
						CCSAnime.DirectionalLightController directionalLightController = new CCSAnime.DirectionalLightController(this.ParentFile, this);
						directionalLightController.Read(bStream, num2);
						this.Controllers.Add(directionalLightController);
					}
				}
				else if (num4 != 6401)
				{
					if (num4 != 65281)
					{
						goto IL_254;
					}
					_frameNumber = bStream.ReadInt32();
					bool flag3 = _frameNumber != -1 && _frameNumber != -2;
					if (!flag3)
					{
						break;
					}
					animationFrame = new CCSAnime.AnimationFrame(_frameNumber);
					this.Frames.Add(animationFrame);
				}
				else
				{
					CCSAnime.MorphKeyFrame morphKeyFrame = new CCSAnime.MorphKeyFrame();
					morphKeyFrame.Read(bStream);
					animationFrame.KeyFrames.Add(morphKeyFrame);
				}
				continue;
				IL_254:
				Logger.LogWarning(string.Format("CCSAnime::Read(): Skipped unknown animation controller or keyframe type {0:X} at 0x{1:X}\n", num, position), Logger.LogType.LogAll, "Read", 200);
				Util.SkipSection(bStream, num2);
			}
			this.PlaybackType = _frameNumber;
			return true;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000C187 File Offset: 0x0000A387
		public override TreeNode ToNode()
		{
			return base.ToNode();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000C190 File Offset: 0x0000A390
		public void FrameForward()
		{
			bool hasEnded = this.HasEnded;
			if (!hasEnded)
			{
				this.CurrentFrame++;
				bool flag = this.CurrentFrame < this.FrameCount - 1;
				if (!flag)
				{
					bool flag2 = this.PlaybackType == -1;
					if (flag2)
					{
						this.CurrentFrame = 0;
						this.HasEnded = true;
					}
					else
					{
						this.CurrentFrame = 0;
					}
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000C1F4 File Offset: 0x0000A3F4
		public void SetFrame(int frameNumber)
		{
			foreach (CCSAnime.Controller controller in this.Controllers)
			{
				controller.SetFrame(frameNumber);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000C24C File Offset: 0x0000A44C
		public string GetPlayTypeString()
		{
			bool flag = this.PlaybackType == -1;
			string result;
			if (flag)
			{
				result = "Play Once";
			}
			else
			{
				result = ((this.PlaybackType == -2) ? "Repeat" : "Unknown");
			}
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000C28C File Offset: 0x0000A48C
		public string GetReport(int level = 0)
		{
			string report = Util.Indent(level, false) + string.Format("Animation 0x{0:X}: {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID)) + Util.Indent(level, false) + string.Format("{0} Frames, Type: {1}\n", this.FrameCount, this.GetPlayTypeString());
			foreach (CCSAnime.Controller controller in this.Controllers)
			{
				report += controller.GetReport(level + 1);
			}
			return report;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000C348 File Offset: 0x0000A548
		public void Render(Matrix4 ProjView, int extraOptions = 0)
		{
			this.FrameForward();
			CCSClump ccsClump = null;
			foreach (CCSAnime.Controller controller in this.Controllers)
			{
				controller.SetFrame(this.CurrentFrame);
				CCSExt ccsExt = this.ParentFile.GetObject<CCSExt>(controller.ObjectID);
				bool flag = ccsExt != null;
				if (flag)
				{
					CCSObject ccsObject = this.ParentFile.GetObject<CCSObject>(ccsExt.ReferencedObjectID);
					bool flag2 = ccsObject != null;
					if (flag2)
					{
						bool flag3 = ccsObject.ParentClump != ccsClump;
						if (flag3)
						{
							ccsObject.ParentClump.BindMatrixList();
							ccsClump = ccsObject.ParentClump;
						}
						ccsObject.ParentClump.FrameForward();
						ccsObject.Render(ProjView, extraOptions);
					}
				}
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000C444 File Offset: 0x0000A644
		public void DumpToText(StreamWriter outf)
		{
			outf.WriteLine(this.ParentFile.GetSubObjectName(this.ObjectID));
			outf.WriteLine(this.Controllers.Count.ToString());
			foreach (CCSAnime.Controller controller in this.Controllers)
			{
				controller.DumpToText(outf);
			}
		}

		// Token: 0x0400011A RID: 282
		public const int CCS_ANIME_FRAME = 65281;

		// Token: 0x0400011B RID: 283
		public const int CCS_ANIME_OBJECT_KEYFRAME = 257;

		// Token: 0x0400011C RID: 284
		public const int CCS_ANIME_OBJECT_CONTROLLER = 258;

		// Token: 0x0400011D RID: 285
		public const int CCS_ANIME_MATERIAL_CONTROLLER = 514;

		// Token: 0x0400011E RID: 286
		public const int CCS_ANIME_LIGHT_AMBIENT_KEYFRAME = 1537;

		// Token: 0x0400011F RID: 287
		public const int CCS_ANIME_LIGHT_DIRECTIONAL_CONTROLLER = 1539;

		// Token: 0x04000120 RID: 288
		public const int CCS_ANIME_LIGHT_OMNI_CONTROLLER = 1545;

		// Token: 0x04000121 RID: 289
		public const int CCS_ANIME_MORPH_KEYFRAME = 6401;

		// Token: 0x04000122 RID: 290
		public const int CCS_ANIME_PLAY_ONCE = -1;

		// Token: 0x04000123 RID: 291
		public const int CCS_ANIME_PLAY_REPEAT = -2;

		// Token: 0x04000124 RID: 292
		public const int CCS_ANIME_CONTROLLER_TYPE_NONE = 0;

		// Token: 0x04000125 RID: 293
		public const int CCS_ANIME_CONTROLLER_TYPE_FIXED = 1;

		// Token: 0x04000126 RID: 294
		public const int CCS_ANIME_CONTROLLER_TYPE_ANIMATED = 2;

		// Token: 0x04000127 RID: 295
		public List<CCSAnime.Controller> Controllers = new List<CCSAnime.Controller>();

		// Token: 0x04000128 RID: 296
		public List<CCSAnime.AnimationFrame> Frames = new List<CCSAnime.AnimationFrame>();

		// Token: 0x04000129 RID: 297
		public int FrameCount = 0;

		// Token: 0x0400012A RID: 298
		public int PlaybackType = 0;

		// Token: 0x0400012B RID: 299
		public int CurrentFrame = 0;

		// Token: 0x0400012C RID: 300
		public bool HasEnded = false;

		// Token: 0x02000043 RID: 67
		public abstract class Controller
		{
			// Token: 0x060001B2 RID: 434
			public abstract bool Read(BinaryReader bStream, int dataSize);

			// Token: 0x060001B3 RID: 435 RVA: 0x00016966 File Offset: 0x00014B66
			public int GetControllerType()
			{
				return this.ControllerType;
			}

			// Token: 0x060001B4 RID: 436 RVA: 0x0001696E File Offset: 0x00014B6E
			public int GetControllerParams()
			{
				return this.ControllerParams;
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x00016976 File Offset: 0x00014B76
			public int GetObjectID()
			{
				return this.ObjectID;
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x0001697E File Offset: 0x00014B7E
			public int GetTrackParams(int trackID)
			{
				return (trackID > 10) ? 0 : (this.ControllerParams >> 3 * trackID & 7);
			}

			// Token: 0x060001B7 RID: 439
			public abstract void SetFrame(int frameNum);

			// Token: 0x060001B8 RID: 440
			public abstract string GetReport(int level = 0);

			// Token: 0x060001B9 RID: 441
			public abstract void DumpToText(StreamWriter outf);

			// Token: 0x04000262 RID: 610
			public int ControllerType;

			// Token: 0x04000263 RID: 611
			public int ControllerParams;

			// Token: 0x04000264 RID: 612
			public int ObjectID;

			// Token: 0x04000265 RID: 613
			public CCSFile ParentFile = null;

			// Token: 0x04000266 RID: 614
			public CCSAnime ParentAnime = null;
		}

		// Token: 0x02000044 RID: 68
		public class ObjectController : CCSAnime.Controller
		{
			// Token: 0x060001BB RID: 443 RVA: 0x000169B0 File Offset: 0x00014BB0
			public ObjectController(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				this.ControllerType = 258;
				this.ParentFile = _parentFile;
				this.ParentAnime = _parentAnime;
			}

			// Token: 0x060001BC RID: 444 RVA: 0x00016A0C File Offset: 0x00014C0C
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				this.ObjectID = bStream.ReadInt32();
				this.ControllerParams = bStream.ReadInt32();
				this.PositionTrack.Read(bStream, base.GetTrackParams(0), this.ParentAnime.FrameCount);
				this.RotationTrack.Read(bStream, base.GetTrackParams(1), this.ParentAnime.FrameCount);
				this.ScaleTrack.Read(bStream, base.GetTrackParams(2), this.ParentAnime.FrameCount);
				this.AlphaTrack.Read(bStream, base.GetTrackParams(3), this.ParentAnime.FrameCount);
				return true;
			}

			// Token: 0x060001BD RID: 445 RVA: 0x00016AB4 File Offset: 0x00014CB4
			public override void SetFrame(int frameNum)
			{
				foreach (CCSAnime.Vec3Key_Scale key in this.ScaleTrack.Keys)
				{
					bool flag = key.FrameNumber() <= frameNum;
					if (!flag)
					{
						break;
					}
				}
				foreach (CCSAnime.F32Key key2 in this.AlphaTrack.Keys)
				{
					bool flag2 = key2.FrameNumber() <= frameNum;
					if (!flag2)
					{
						break;
					}
				}
				CCSExt ccsExt = this.ParentFile.GetObject<CCSExt>(this.ObjectID);
				bool flag3 = ccsExt == null;
				if (!flag3)
				{
					CCSObject ccsObject = this.ParentFile.GetObject<CCSObject>(ccsExt.ReferencedObjectID);
					bool flag4 = ccsObject != null;
					if (flag4)
					{
						Vector3 interpolatedValue = this.PositionTrack.GetInterpolatedValue(frameNum);
						Vector3 interpolatedValue2 = this.RotationTrack.GetInterpolatedValue(frameNum);
						Vector3 interpolatedValue3 = this.ScaleTrack.GetInterpolatedValue(frameNum);
						ccsObject.SetPose(interpolatedValue, interpolatedValue2, interpolatedValue3);
					}
				}
			}

			// Token: 0x060001BE RID: 446 RVA: 0x00016C00 File Offset: 0x00014E00
			public override void DumpToText(StreamWriter outf)
			{
				CCSExt ccsExt = this.ParentFile.GetObject<CCSExt>(this.ObjectID);
				bool flag = ccsExt == null;
				if (!flag)
				{
					CCSObject ccsObject = this.ParentFile.GetObject<CCSObject>(ccsExt.ReferencedObjectID);
					bool flag2 = ccsObject != null && ccsObject.ModelID != 0;
					if (flag2)
					{
						outf.WriteLine(this.ParentFile.GetSubObjectName(ccsObject.ModelID));
						Vector3 vector3_ = this.PositionTrack.GetValue(0).Value();
						Vector3 vector3_2 = this.RotationTrack.GetValue(0).Value();
						Vector3 vector3_3 = this.ScaleTrack.GetValue(0).Value();
						string format = "{0} {1} {2}";
						string str = string.Format(format, vector3_.X, vector3_.Y, vector3_.Z);
						string str2 = string.Format(format, vector3_2.X, vector3_2.Y, vector3_2.Z);
						string str3 = string.Format(format, vector3_3.X, vector3_3.Y, vector3_3.Z);
						outf.WriteLine(string.Format(format, str, str2, str3));
					}
				}
			}

			// Token: 0x060001BF RID: 447 RVA: 0x00016D4A File Offset: 0x00014F4A
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level, false) + string.Format("Object Controller for object 0x{0:X}, {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID));
			}

			// Token: 0x04000267 RID: 615
			public CCSAnime.Vec3Position_Track PositionTrack = new CCSAnime.Vec3Position_Track();

			// Token: 0x04000268 RID: 616
			public CCSAnime.Vec3Rotation_Track RotationTrack = new CCSAnime.Vec3Rotation_Track();

			// Token: 0x04000269 RID: 617
			public CCSAnime.Vec3Scale_Track ScaleTrack = new CCSAnime.Vec3Scale_Track();

			// Token: 0x0400026A RID: 618
			public CCSAnime.F32_Track AlphaTrack = new CCSAnime.F32_Track();
		}

		// Token: 0x02000045 RID: 69
		public class ObjectController_Gen2 : CCSAnime.Controller
		{
			// Token: 0x060001C0 RID: 448 RVA: 0x00016D80 File Offset: 0x00014F80
			public ObjectController_Gen2(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				this.ControllerType = 258;
				this.ParentFile = _parentFile;
				this.ParentAnime = _parentAnime;
			}

			// Token: 0x060001C1 RID: 449 RVA: 0x00016DE8 File Offset: 0x00014FE8
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				this.ObjectID = bStream.ReadInt32();
				this.ControllerParams = bStream.ReadInt32();
				this.PositionTrack.Read(bStream, base.GetTrackParams(0), this.ParentAnime.FrameCount);
				int position = (int)bStream.BaseStream.Position;
				this.RotationTrack.Read(bStream, base.GetTrackParams(1), this.ParentAnime.FrameCount);
				this.Rotation4Track.Read(bStream, base.GetTrackParams(8), this.ParentAnime.FrameCount);
				this.ScaleTrack.Read(bStream, base.GetTrackParams(2), this.ParentAnime.FrameCount);
				this.AlphaTrack.Read(bStream, base.GetTrackParams(3), this.ParentAnime.FrameCount);
				return true;
			}

			// Token: 0x060001C2 RID: 450 RVA: 0x00016EBC File Offset: 0x000150BC
			public override void SetFrame(int frameNum)
			{
				CCSAnime.Vec3Key_Scale vec3KeyScale = null;
				CCSAnime.F32Key f32Key = null;
				foreach (CCSAnime.Vec3Key_Scale key in this.ScaleTrack.Keys)
				{
					bool flag = key.FrameNumber() <= frameNum;
					if (!flag)
					{
						break;
					}
					vec3KeyScale = key;
				}
				foreach (CCSAnime.F32Key key2 in this.AlphaTrack.Keys)
				{
					bool flag2 = key2.FrameNumber() <= frameNum;
					if (!flag2)
					{
						break;
					}
					f32Key = key2;
				}
				CCSExt ccsExt = this.ParentFile.GetObject<CCSExt>(this.ObjectID);
				bool flag3 = ccsExt == null;
				if (!flag3)
				{
					CCSObject ccsObject = this.ParentFile.GetObject<CCSObject>(ccsExt.ReferencedObjectID);
					bool flag4 = ccsObject != null;
					if (flag4)
					{
						Vector3 interpolatedValue = this.PositionTrack.GetInterpolatedValue(frameNum);
						Quaternion identity = Quaternion.Identity;
						Vector3 Scale = Vector3.One;
						float num = 1f;
						Quaternion Rotation = Quaternion.FromEulerAngles(this.RotationTrack.GetInterpolatedValue(frameNum));
						bool flag5 = vec3KeyScale != null;
						if (flag5)
						{
							Scale = vec3KeyScale.Value();
						}
						bool flag6 = f32Key != null;
						if (flag6)
						{
							num = f32Key.Value();
						}
						ccsObject.SetPose(interpolatedValue, Rotation, Scale);
						ccsObject.Alpha = num;
					}
				}
			}

			// Token: 0x060001C3 RID: 451 RVA: 0x00016D4A File Offset: 0x00014F4A
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level, false) + string.Format("Object Controller for object 0x{0:X}, {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID));
			}

			// Token: 0x060001C4 RID: 452 RVA: 0x00017040 File Offset: 0x00015240
			public override void DumpToText(StreamWriter outf)
			{
				CCSExt ccsExt = this.ParentFile.GetObject<CCSExt>(this.ObjectID);
				bool flag = ccsExt == null;
				if (!flag)
				{
					CCSObject ccsObject = this.ParentFile.GetObject<CCSObject>(ccsExt.ReferencedObjectID);
					bool flag2 = ccsObject != null && ccsObject.ModelID != 0;
					if (flag2)
					{
						outf.WriteLine(this.ParentFile.GetSubObjectName(ccsObject.ModelID));
						Vector3 vector3_ = this.PositionTrack.GetValue(0).Value();
						Vector3 vector3_2 = this.RotationTrack.GetValue(0).Value();
						Vector3 vector3_3 = this.ScaleTrack.GetValue(0).Value();
						string format = "{0} {1} {2}";
						string str = string.Format(format, vector3_.X, vector3_.Y, vector3_.Z);
						string str2 = string.Format(format, vector3_2.X, vector3_2.Y, vector3_2.Z);
						string str3 = string.Format(format, vector3_3.X, vector3_3.Y, vector3_3.Z);
						outf.WriteLine(string.Format(format, str, str2, str3));
					}
				}
			}

			// Token: 0x0400026B RID: 619
			public CCSAnime.Vec3Position_Track PositionTrack = new CCSAnime.Vec3Position_Track();

			// Token: 0x0400026C RID: 620
			public CCSAnime.Vec3Rotation_Track RotationTrack = new CCSAnime.Vec3Rotation_Track();

			// Token: 0x0400026D RID: 621
			public CCSAnime.Vec3Scale_Track ScaleTrack = new CCSAnime.Vec3Scale_Track();

			// Token: 0x0400026E RID: 622
			public CCSAnime.F32_Track AlphaTrack = new CCSAnime.F32_Track();

			// Token: 0x0400026F RID: 623
			public CCSAnime.Vec4Rotation_Track Rotation4Track = new CCSAnime.Vec4Rotation_Track();
		}

		// Token: 0x02000046 RID: 70
		public class MaterialController : CCSAnime.Controller
		{
			// Token: 0x060001C5 RID: 453 RVA: 0x0001718C File Offset: 0x0001538C
			public MaterialController(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				this.ControllerType = 514;
				this.ParentFile = _parentFile;
				this.ParentAnime = _parentAnime;
			}

			// Token: 0x060001C6 RID: 454 RVA: 0x000171E8 File Offset: 0x000153E8
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				this.ObjectID = bStream.ReadInt32();
				this.ControllerParams = bStream.ReadInt32();
				this.UOffsetTrack.Read(bStream, base.GetTrackParams(0), this.ParentAnime.FrameCount);
				this.VOffsetTrack.Read(bStream, base.GetTrackParams(1), this.ParentAnime.FrameCount);
				this.UnkFTrack.Read(bStream, base.GetTrackParams(2), this.ParentAnime.FrameCount);
				this.UnkFTrack2.Read(bStream, base.GetTrackParams(3), this.ParentAnime.FrameCount);
				return true;
			}

			// Token: 0x060001C7 RID: 455 RVA: 0x00017290 File Offset: 0x00015490
			public override void SetFrame(int frameNum)
			{
				CCSMaterial ccsMaterial = this.ParentFile.GetObject<CCSMaterial>(this.ObjectID);
				bool flag = ccsMaterial == null;
				if (!flag)
				{
					float interpolatedValue = this.UOffsetTrack.GetNonInterpolatedValue(frameNum);
					float interpolatedValue2 = this.VOffsetTrack.GetNonInterpolatedValue(frameNum);
					ccsMaterial.TextureOffset = new Vector2(interpolatedValue, interpolatedValue2);
				}
			}

			// Token: 0x060001C8 RID: 456 RVA: 0x000172E1 File Offset: 0x000154E1
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level, false) + string.Format("Material Controller for material 0x{0:X}, {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID));
			}

			// Token: 0x060001C9 RID: 457 RVA: 0x00008ED7 File Offset: 0x000070D7
			public override void DumpToText(StreamWriter outf)
			{
			}

			// Token: 0x04000270 RID: 624
			public CCSAnime.F32_Track UOffsetTrack = new CCSAnime.F32_Track();

			// Token: 0x04000271 RID: 625
			public CCSAnime.F32_Track VOffsetTrack = new CCSAnime.F32_Track();

			// Token: 0x04000272 RID: 626
			public CCSAnime.F32_Track UnkFTrack = new CCSAnime.F32_Track();

			// Token: 0x04000273 RID: 627
			public CCSAnime.F32_Track UnkFTrack2 = new CCSAnime.F32_Track();
		}

		// Token: 0x02000047 RID: 71
		public class DirectionalLightController : CCSAnime.Controller
		{
			// Token: 0x060001CA RID: 458 RVA: 0x00017318 File Offset: 0x00015518
			public DirectionalLightController(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				this.ControllerType = 1539;
				this.ParentFile = _parentFile;
				this.ParentAnime = _parentAnime;
			}

			// Token: 0x060001CB RID: 459 RVA: 0x00017368 File Offset: 0x00015568
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				this.ObjectID = bStream.ReadInt32();
				this.ControllerParams = bStream.ReadInt32();
				this.DirectionTrack.Read(bStream, base.GetTrackParams(1), this.ParentAnime.FrameCount);
				this.ColorTrack.Read(bStream, base.GetTrackParams(2), this.ParentAnime.FrameCount);
				this.UnkTrack.Read(bStream, base.GetTrackParams(0), this.ParentAnime.FrameCount);
				return true;
			}

			// Token: 0x060001CC RID: 460 RVA: 0x00008ED7 File Offset: 0x000070D7
			public override void SetFrame(int frameNum)
			{
			}

			// Token: 0x060001CD RID: 461 RVA: 0x000173F0 File Offset: 0x000155F0
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level, false) + string.Format("Directional Light Controller for light 0x{0:X}, {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID));
			}

			// Token: 0x060001CE RID: 462 RVA: 0x00008ED7 File Offset: 0x000070D7
			public override void DumpToText(StreamWriter outf)
			{
			}

			// Token: 0x04000274 RID: 628
			public CCSAnime.Vec3Rotation_Track DirectionTrack = new CCSAnime.Vec3Rotation_Track();

			// Token: 0x04000275 RID: 629
			public CCSAnime.Vec4Color_Track ColorTrack = new CCSAnime.Vec4Color_Track();

			// Token: 0x04000276 RID: 630
			public CCSAnime.F32_Track UnkTrack = new CCSAnime.F32_Track();
		}

		// Token: 0x02000048 RID: 72
		public class OmniLightController : CCSAnime.Controller
		{
			// Token: 0x060001CF RID: 463 RVA: 0x00017424 File Offset: 0x00015624
			public OmniLightController(CCSFile _parentFile, CCSAnime _parentAnime)
			{
				this.ControllerType = 1545;
				this.ParentFile = _parentFile;
				this.ParentAnime = _parentAnime;
			}

			// Token: 0x060001D0 RID: 464 RVA: 0x0001748C File Offset: 0x0001568C
			public override bool Read(BinaryReader bStream, int dataSize)
			{
				this.ObjectID = bStream.ReadInt32();
				this.ControllerParams = bStream.ReadInt32();
				this.PositionTrack.Read(bStream, base.GetTrackParams(0), this.ParentAnime.FrameCount);
				this.ColorTrack.Read(bStream, base.GetTrackParams(2), this.ParentAnime.FrameCount);
				this.UnkF32Track1.Read(bStream, base.GetTrackParams(3), this.ParentAnime.FrameCount);
				this.UnkF32Track2.Read(bStream, base.GetTrackParams(4), this.ParentAnime.FrameCount);
				this.UnkF32Track3.Read(bStream, base.GetTrackParams(5), this.ParentAnime.FrameCount);
				return true;
			}

			// Token: 0x060001D1 RID: 465 RVA: 0x00008ED7 File Offset: 0x000070D7
			public override void SetFrame(int frameNum)
			{
			}

			// Token: 0x060001D2 RID: 466 RVA: 0x00017552 File Offset: 0x00015752
			public override string GetReport(int level = 0)
			{
				return Util.Indent(level, false) + string.Format("Omni Light Controller for light 0x{0:X}, {1}\n", this.ObjectID, this.ParentFile.GetSubObjectName(this.ObjectID));
			}

			// Token: 0x060001D3 RID: 467 RVA: 0x00008ED7 File Offset: 0x000070D7
			public override void DumpToText(StreamWriter outf)
			{
			}

			// Token: 0x04000277 RID: 631
			public CCSAnime.Vec3Position_Track PositionTrack = new CCSAnime.Vec3Position_Track();

			// Token: 0x04000278 RID: 632
			public CCSAnime.Vec4Color_Track ColorTrack = new CCSAnime.Vec4Color_Track();

			// Token: 0x04000279 RID: 633
			public CCSAnime.F32_Track UnkF32Track1 = new CCSAnime.F32_Track();

			// Token: 0x0400027A RID: 634
			public CCSAnime.F32_Track UnkF32Track2 = new CCSAnime.F32_Track();

			// Token: 0x0400027B RID: 635
			public CCSAnime.F32_Track UnkF32Track3 = new CCSAnime.F32_Track();
		}

		// Token: 0x02000049 RID: 73
		public interface IAnimationKey<T>
		{
			// Token: 0x060001D4 RID: 468
			void Read(BinaryReader bStream, int _frameNum);

			// Token: 0x060001D5 RID: 469
			T Value();

			// Token: 0x060001D6 RID: 470
			int FrameNumber();

			// Token: 0x060001D7 RID: 471
			void SetFrameCount(int fCount);

			// Token: 0x060001D8 RID: 472
			int GetFrameCount();
		}

		// Token: 0x0200004A RID: 74
		public class Vec4Key_Color : CCSAnime.IAnimationKey<Vector4>
		{
			// Token: 0x060001D9 RID: 473 RVA: 0x00017586 File Offset: 0x00015786
			public void Read(BinaryReader bStream, int _frameNum)
			{
				this.FrameNum = _frameNum;
				this.KeyValue = Util.ReadVec4RGBA32(bStream);
			}

			// Token: 0x060001DA RID: 474 RVA: 0x0001759C File Offset: 0x0001579C
			public Vector4 Value()
			{
				return this.KeyValue;
			}

			// Token: 0x060001DB RID: 475 RVA: 0x000175A4 File Offset: 0x000157A4
			public int FrameNumber()
			{
				return this.FrameNum;
			}

			// Token: 0x060001DC RID: 476 RVA: 0x000175AC File Offset: 0x000157AC
			public int GetFrameCount()
			{
				return this.FrameCount;
			}

			// Token: 0x060001DD RID: 477 RVA: 0x000175B4 File Offset: 0x000157B4
			public void SetFrameCount(int fCount)
			{
				this.FrameCount = fCount;
			}

			// Token: 0x0400027C RID: 636
			private int FrameNum;

			// Token: 0x0400027D RID: 637
			private int FrameCount;

			// Token: 0x0400027E RID: 638
			private Vector4 KeyValue;
		}

		// Token: 0x0200004B RID: 75
		public class Vec4Key_Rotation : CCSAnime.IAnimationKey<Quaternion>
		{
			// Token: 0x060001DF RID: 479 RVA: 0x000175BD File Offset: 0x000157BD
			public void Read(BinaryReader bStream, int _frameNum)
			{
				this.FrameNum = _frameNum;
				this.KeyValue = new Quaternion(bStream.ReadSingle(), bStream.ReadSingle(), bStream.ReadSingle(), bStream.ReadSingle());
			}

			// Token: 0x060001E0 RID: 480 RVA: 0x000175EA File Offset: 0x000157EA
			public Quaternion Value()
			{
				return this.KeyValue;
			}

			// Token: 0x060001E1 RID: 481 RVA: 0x000175F2 File Offset: 0x000157F2
			public int FrameNumber()
			{
				return this.FrameNum;
			}

			// Token: 0x060001E2 RID: 482 RVA: 0x000175FA File Offset: 0x000157FA
			public int GetFrameCount()
			{
				return this.FrameCount;
			}

			// Token: 0x060001E3 RID: 483 RVA: 0x00017602 File Offset: 0x00015802
			public void SetFrameCount(int fCount)
			{
				this.FrameCount = fCount;
			}

			// Token: 0x0400027F RID: 639
			private int FrameNum;

			// Token: 0x04000280 RID: 640
			private int FrameCount;

			// Token: 0x04000281 RID: 641
			private Quaternion KeyValue;
		}

		// Token: 0x0200004C RID: 76
		public class Vec3Key_Rotation : CCSAnime.IAnimationKey<Vector3>
		{
			// Token: 0x060001E5 RID: 485 RVA: 0x0001760B File Offset: 0x0001580B
			public void Read(BinaryReader bStream, int _frameNum)
			{
				this.FrameNum = _frameNum;
				this.KeyValue = Util.ReadVec3Rotation(bStream);
			}

			// Token: 0x060001E6 RID: 486 RVA: 0x00017621 File Offset: 0x00015821
			public Vector3 Value()
			{
				return this.KeyValue;
			}

			// Token: 0x060001E7 RID: 487 RVA: 0x00017629 File Offset: 0x00015829
			public int FrameNumber()
			{
				return this.FrameNum;
			}

			// Token: 0x060001E8 RID: 488 RVA: 0x00017631 File Offset: 0x00015831
			public int GetFrameCount()
			{
				return this.FrameCount;
			}

			// Token: 0x060001E9 RID: 489 RVA: 0x00017639 File Offset: 0x00015839
			public void SetFrameCount(int fCount)
			{
				this.FrameCount = fCount;
			}

			// Token: 0x04000282 RID: 642
			private Vector3 KeyValue;

			// Token: 0x04000283 RID: 643
			private int FrameNum;

			// Token: 0x04000284 RID: 644
			private int FrameCount;
		}

		// Token: 0x0200004D RID: 77
		public class Vec3Key_Position : CCSAnime.IAnimationKey<Vector3>
		{
			// Token: 0x060001EB RID: 491 RVA: 0x00017642 File Offset: 0x00015842
			public void Read(BinaryReader bStream, int _frameNum)
			{
				this.FrameNum = _frameNum;
				this.KeyValue = Util.ReadVec3Position(bStream);
			}

			// Token: 0x060001EC RID: 492 RVA: 0x00017658 File Offset: 0x00015858
			public Vector3 Value()
			{
				return this.KeyValue;
			}

			// Token: 0x060001ED RID: 493 RVA: 0x00017660 File Offset: 0x00015860
			public int FrameNumber()
			{
				return this.FrameNum;
			}

			// Token: 0x060001EE RID: 494 RVA: 0x00017668 File Offset: 0x00015868
			public int GetFrameCount()
			{
				return this.FrameCount;
			}

			// Token: 0x060001EF RID: 495 RVA: 0x00017670 File Offset: 0x00015870
			public void SetFrameCount(int fCount)
			{
				this.FrameCount = fCount;
			}

			// Token: 0x04000285 RID: 645
			private Vector3 KeyValue;

			// Token: 0x04000286 RID: 646
			private int FrameNum;

			// Token: 0x04000287 RID: 647
			private int FrameCount;
		}

		// Token: 0x0200004E RID: 78
		public class Vec3Key_Scale : CCSAnime.IAnimationKey<Vector3>
		{
			// Token: 0x060001F1 RID: 497 RVA: 0x00017679 File Offset: 0x00015879
			public void Read(BinaryReader bStream, int _frameNum)
			{
				this.FrameNum = _frameNum;
				this.KeyValue = Util.ReadVec3Scale(bStream);
			}

			// Token: 0x060001F2 RID: 498 RVA: 0x0001768F File Offset: 0x0001588F
			public Vector3 Value()
			{
				return this.KeyValue;
			}

			// Token: 0x060001F3 RID: 499 RVA: 0x00017697 File Offset: 0x00015897
			public int FrameNumber()
			{
				return this.FrameNum;
			}

			// Token: 0x060001F4 RID: 500 RVA: 0x0001769F File Offset: 0x0001589F
			public int GetFrameCount()
			{
				return this.FrameCount;
			}

			// Token: 0x060001F5 RID: 501 RVA: 0x000176A7 File Offset: 0x000158A7
			public void SetFrameCount(int fCount)
			{
				this.FrameCount = fCount;
			}

			// Token: 0x04000288 RID: 648
			private Vector3 KeyValue = Vector3.One;

			// Token: 0x04000289 RID: 649
			private int FrameNum;

			// Token: 0x0400028A RID: 650
			private int FrameCount;
		}

		// Token: 0x0200004F RID: 79
		public class Vec2Key_UV : CCSAnime.IAnimationKey<Vector2>
		{
			// Token: 0x060001F7 RID: 503 RVA: 0x000176C4 File Offset: 0x000158C4
			public void Read(BinaryReader bStream, int _frameNum)
			{
				this.FrameNum = _frameNum;
				this.KeyValue = Util.ReadVec2UV(bStream);
			}

			// Token: 0x060001F8 RID: 504 RVA: 0x000176DA File Offset: 0x000158DA
			public Vector2 Value()
			{
				return this.KeyValue;
			}

			// Token: 0x060001F9 RID: 505 RVA: 0x000176E2 File Offset: 0x000158E2
			public int FrameNumber()
			{
				return this.FrameNum;
			}

			// Token: 0x060001FA RID: 506 RVA: 0x000176EA File Offset: 0x000158EA
			public int GetFrameCount()
			{
				return this.FrameCount;
			}

			// Token: 0x060001FB RID: 507 RVA: 0x000176F2 File Offset: 0x000158F2
			public void SetFrameCount(int fCount)
			{
				this.FrameCount = fCount;
			}

			// Token: 0x0400028B RID: 651
			private int FrameNum;

			// Token: 0x0400028C RID: 652
			private int FrameCount;

			// Token: 0x0400028D RID: 653
			private Vector2 KeyValue;
		}

		// Token: 0x02000050 RID: 80
		public class F32Key : CCSAnime.IAnimationKey<float>
		{
			// Token: 0x060001FD RID: 509 RVA: 0x000176FB File Offset: 0x000158FB
			public void Read(BinaryReader bStream, int _frameNum)
			{
				this.FrameNum = _frameNum;
				this.KeyValue = bStream.ReadSingle();
			}

			// Token: 0x060001FE RID: 510 RVA: 0x00017711 File Offset: 0x00015911
			public float Value()
			{
				return this.KeyValue;
			}

			// Token: 0x060001FF RID: 511 RVA: 0x00017719 File Offset: 0x00015919
			public int FrameNumber()
			{
				return this.FrameNum;
			}

			// Token: 0x06000200 RID: 512 RVA: 0x00017721 File Offset: 0x00015921
			public int GetFrameCount()
			{
				return this.FrameCount;
			}

			// Token: 0x06000201 RID: 513 RVA: 0x00017729 File Offset: 0x00015929
			public void SetFrameCount(int fCount)
			{
				this.FrameCount = fCount;
			}

			// Token: 0x0400028E RID: 654
			private int FrameNum;

			// Token: 0x0400028F RID: 655
			private int FrameCount;

			// Token: 0x04000290 RID: 656
			private float KeyValue;
		}

		// Token: 0x02000051 RID: 81
		public class Int32Key : CCSAnime.IAnimationKey<int>
		{
			// Token: 0x06000203 RID: 515 RVA: 0x00017732 File Offset: 0x00015932
			public void Read(BinaryReader bStream, int _frameNum)
			{
				this.FrameNum = _frameNum;
				this.KeyValue = bStream.ReadInt32();
			}

			// Token: 0x06000204 RID: 516 RVA: 0x00017748 File Offset: 0x00015948
			public int Value()
			{
				return this.KeyValue;
			}

			// Token: 0x06000205 RID: 517 RVA: 0x00017750 File Offset: 0x00015950
			public int FrameNumber()
			{
				return this.FrameNum;
			}

			// Token: 0x06000206 RID: 518 RVA: 0x00017758 File Offset: 0x00015958
			public int GetFrameCount()
			{
				return this.FrameCount;
			}

			// Token: 0x06000207 RID: 519 RVA: 0x00017760 File Offset: 0x00015960
			public void SetFrameCount(int fCount)
			{
				this.FrameCount = fCount;
			}

			// Token: 0x04000291 RID: 657
			public int FrameNum;

			// Token: 0x04000292 RID: 658
			public int KeyValue;

			// Token: 0x04000293 RID: 659
			public int FrameCount;
		}

		// Token: 0x02000052 RID: 82
		public class Vec4Color_Track
		{
			// Token: 0x06000209 RID: 521 RVA: 0x0001776C File Offset: 0x0001596C
			public CCSAnime.Vec4Key_Color GetValue(int keyID)
			{
				bool flag = this.KeyCount == 0;
				CCSAnime.Vec4Key_Color result;
				if (flag)
				{
					result = this.FixedValue;
				}
				else
				{
					result = ((keyID < this.KeyCount) ? this.Keys[keyID] : this.Keys[this.Keys.Count - 1]);
				}
				return result;
			}

			// Token: 0x0600020A RID: 522 RVA: 0x000177C4 File Offset: 0x000159C4
			public void Read(BinaryReader bStream, int TrackType, int frameCount)
			{
				if (TrackType != 1)
				{
					if (TrackType == 2)
					{
						this.KeyCount = bStream.ReadInt32();
						for (int index = 0; index < this.KeyCount; index++)
						{
							int _frameNum = bStream.ReadInt32();
							CCSAnime.Vec4Key_Color vec4KeyColor = new CCSAnime.Vec4Key_Color();
							vec4KeyColor.Read(bStream, _frameNum);
							bool flag = this.Keys.Count > 0;
							if (flag)
							{
								CCSAnime.Vec4Key_Color key = this.Keys[this.Keys.Count - 1];
								bool flag2 = key.FrameNumber() == _frameNum;
								if (flag2)
								{
									this.Keys.Remove(key);
								}
							}
							bool flag3 = this.Keys.Count > 0;
							if (flag3)
							{
								this.Keys[this.Keys.Count - 1].SetFrameCount(vec4KeyColor.FrameNumber());
							}
							this.Keys.Add(vec4KeyColor);
						}
						this.Keys[this.Keys.Count - 1].SetFrameCount(frameCount);
					}
				}
				else
				{
					this.FixedValue.Read(bStream, 0);
				}
			}

			// Token: 0x0600020B RID: 523 RVA: 0x000178F4 File Offset: 0x00015AF4
			public Vector4 GetInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				Vector4 result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					result = this.GetValue(this.CurrentKey).Value();
				}
				return result;
			}

			// Token: 0x04000294 RID: 660
			public List<CCSAnime.Vec4Key_Color> Keys = new List<CCSAnime.Vec4Key_Color>();

			// Token: 0x04000295 RID: 661
			public CCSAnime.Vec4Key_Color FixedValue = new CCSAnime.Vec4Key_Color();

			// Token: 0x04000296 RID: 662
			private int KeyCount = 0;

			// Token: 0x04000297 RID: 663
			private int CurrentKey = 0;
		}

		// Token: 0x02000053 RID: 83
		public class Vec3Rotation_Track
		{
			// Token: 0x0600020D RID: 525 RVA: 0x00017970 File Offset: 0x00015B70
			public CCSAnime.Vec3Key_Rotation GetValue(int keyID)
			{
				bool flag = this.KeyCount == 0;
				CCSAnime.Vec3Key_Rotation result;
				if (flag)
				{
					result = this.FixedValue;
				}
				else
				{
					result = ((keyID < this.KeyCount) ? this.Keys[keyID] : this.Keys[this.Keys.Count - 1]);
				}
				return result;
			}

			// Token: 0x0600020E RID: 526 RVA: 0x000179C8 File Offset: 0x00015BC8
			public void Read(BinaryReader bStream, int TrackType, int frameCount)
			{
				if (TrackType != 1)
				{
					if (TrackType == 2)
					{
						this.KeyCount = bStream.ReadInt32();
						for (int index = 0; index < this.KeyCount; index++)
						{
							int _frameNum = bStream.ReadInt32();
							CCSAnime.Vec3Key_Rotation vec3KeyRotation = new CCSAnime.Vec3Key_Rotation();
							vec3KeyRotation.Read(bStream, _frameNum);
							bool flag = this.Keys.Count > 0;
							if (flag)
							{
								CCSAnime.Vec3Key_Rotation key = this.Keys[this.Keys.Count - 1];
								bool flag2 = key.FrameNumber() == _frameNum;
								if (flag2)
								{
									this.Keys.Remove(key);
								}
							}
							bool flag3 = this.Keys.Count > 0;
							if (flag3)
							{
								this.Keys[this.Keys.Count - 1].SetFrameCount(vec3KeyRotation.FrameNumber());
							}
							this.Keys.Add(vec3KeyRotation);
						}
						this.Keys[this.Keys.Count - 1].SetFrameCount(frameCount);
					}
				}
				else
				{
					this.FixedValue.Read(bStream, 0);
				}
			}

			// Token: 0x0600020F RID: 527 RVA: 0x00017AF8 File Offset: 0x00015CF8
			public Vector3 GetInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				Vector3 result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					CCSAnime.Vec3Key_Rotation vec3KeyRotation = this.GetValue(this.CurrentKey);
					bool flag3 = frameNumber >= vec3KeyRotation.GetFrameCount();
					if (flag3)
					{
						this.CurrentKey++;
						bool flag4 = this.CurrentKey > this.Keys.Count - 1;
						if (flag4)
						{
							this.CurrentKey = 0;
						}
					}
					CCSAnime.Vec3Key_Rotation vec3KeyRotation2 = this.GetValue(this.CurrentKey);
					int keyID = this.CurrentKey + 1;
					bool flag5 = keyID > this.Keys.Count - 1;
					if (flag5)
					{
						keyID = 0;
					}
					CCSAnime.Vec3Key_Rotation vec3KeyRotation3 = this.GetValue(keyID);
					float num = 1f / (float)(vec3KeyRotation2.GetFrameCount() - vec3KeyRotation2.FrameNumber());
					int num2 = frameNumber - vec3KeyRotation2.FrameNumber();
					result = Vector3.Lerp(vec3KeyRotation2.Value(), vec3KeyRotation3.Value(), num * (float)num2);
				}
				return result;
			}

			// Token: 0x04000298 RID: 664
			public List<CCSAnime.Vec3Key_Rotation> Keys = new List<CCSAnime.Vec3Key_Rotation>();

			// Token: 0x04000299 RID: 665
			public CCSAnime.Vec3Key_Rotation FixedValue = new CCSAnime.Vec3Key_Rotation();

			// Token: 0x0400029A RID: 666
			private int KeyCount = 0;

			// Token: 0x0400029B RID: 667
			private int CurrentKey = 0;
		}

		// Token: 0x02000054 RID: 84
		public class Vec4Rotation_Track
		{
			// Token: 0x06000211 RID: 529 RVA: 0x00017C2C File Offset: 0x00015E2C
			public CCSAnime.Vec4Key_Rotation GetValue(int keyID)
			{
				bool flag = this.KeyCount == 0;
				CCSAnime.Vec4Key_Rotation result;
				if (flag)
				{
					result = this.FixedValue;
				}
				else
				{
					result = ((keyID < this.KeyCount) ? this.Keys[keyID] : this.Keys[this.Keys.Count - 1]);
				}
				return result;
			}

			// Token: 0x06000212 RID: 530 RVA: 0x00017C84 File Offset: 0x00015E84
			public void Read(BinaryReader bStream, int TrackType, int frameCount)
			{
				if (TrackType != 1)
				{
					if (TrackType == 2)
					{
						this.KeyCount = bStream.ReadInt32();
						for (int index = 0; index < this.KeyCount; index++)
						{
							int _frameNum = bStream.ReadInt32();
							CCSAnime.Vec4Key_Rotation vec4KeyRotation = new CCSAnime.Vec4Key_Rotation();
							vec4KeyRotation.Read(bStream, _frameNum);
							bool flag = this.Keys.Count > 0;
							if (flag)
							{
								CCSAnime.Vec4Key_Rotation key = this.Keys[this.Keys.Count - 1];
								bool flag2 = key.FrameNumber() == _frameNum;
								if (flag2)
								{
									this.Keys.Remove(key);
								}
							}
							bool flag3 = this.Keys.Count > 0;
							if (flag3)
							{
								this.Keys[this.Keys.Count - 1].SetFrameCount(vec4KeyRotation.FrameNumber());
							}
							this.Keys.Add(vec4KeyRotation);
						}
						this.Keys[this.Keys.Count - 1].SetFrameCount(frameCount);
					}
				}
				else
				{
					this.FixedValue.Read(bStream, 0);
				}
			}

			// Token: 0x06000213 RID: 531 RVA: 0x00017DB4 File Offset: 0x00015FB4
			public Quaternion GetInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				Quaternion result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					result = this.GetValue(this.CurrentKey).Value();
				}
				return result;
			}

			// Token: 0x0400029C RID: 668
			public List<CCSAnime.Vec4Key_Rotation> Keys = new List<CCSAnime.Vec4Key_Rotation>();

			// Token: 0x0400029D RID: 669
			public CCSAnime.Vec4Key_Rotation FixedValue = new CCSAnime.Vec4Key_Rotation();

			// Token: 0x0400029E RID: 670
			private int KeyCount = 0;

			// Token: 0x0400029F RID: 671
			private int CurrentKey = 0;
		}

		// Token: 0x02000055 RID: 85
		public class Vec3Position_Track
		{
			// Token: 0x06000215 RID: 533 RVA: 0x00017E30 File Offset: 0x00016030
			public CCSAnime.Vec3Key_Position GetValue(int keyID)
			{
				bool flag = this.KeyCount == 0;
				CCSAnime.Vec3Key_Position result;
				if (flag)
				{
					result = this.FixedValue;
				}
				else
				{
					result = ((keyID < this.KeyCount) ? this.Keys[keyID] : this.Keys[this.Keys.Count - 1]);
				}
				return result;
			}

			// Token: 0x06000216 RID: 534 RVA: 0x00017E88 File Offset: 0x00016088
			public void Read(BinaryReader bStream, int TrackType, int frameCount)
			{
				if (TrackType != 1)
				{
					if (TrackType == 2)
					{
						this.KeyCount = bStream.ReadInt32();
						for (int index = 0; index < this.KeyCount; index++)
						{
							int _frameNum = bStream.ReadInt32();
							CCSAnime.Vec3Key_Position vec3KeyPosition = new CCSAnime.Vec3Key_Position();
							vec3KeyPosition.Read(bStream, _frameNum);
							bool flag = this.Keys.Count > 0;
							if (flag)
							{
								CCSAnime.Vec3Key_Position key = this.Keys[this.Keys.Count - 1];
								bool flag2 = key.FrameNumber() == _frameNum;
								if (flag2)
								{
									this.Keys.Remove(key);
								}
							}
							bool flag3 = this.Keys.Count > 0;
							if (flag3)
							{
								this.Keys[this.Keys.Count - 1].SetFrameCount(vec3KeyPosition.FrameNumber());
							}
							this.Keys.Add(vec3KeyPosition);
						}
						this.Keys[this.Keys.Count - 1].SetFrameCount(frameCount);
					}
				}
				else
				{
					this.FixedValue.Read(bStream, 0);
				}
			}

			// Token: 0x06000217 RID: 535 RVA: 0x00017FB8 File Offset: 0x000161B8
			public Vector3 GetInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				Vector3 result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					result = this.GetValue(this.CurrentKey).Value();
				}
				return result;
			}

			// Token: 0x040002A0 RID: 672
			public List<CCSAnime.Vec3Key_Position> Keys = new List<CCSAnime.Vec3Key_Position>();

			// Token: 0x040002A1 RID: 673
			public CCSAnime.Vec3Key_Position FixedValue = new CCSAnime.Vec3Key_Position();

			// Token: 0x040002A2 RID: 674
			private int KeyCount = 0;

			// Token: 0x040002A3 RID: 675
			private int CurrentKey = 0;
		}

		// Token: 0x02000056 RID: 86
		public class Vec3Scale_Track
		{
			// Token: 0x06000219 RID: 537 RVA: 0x00018034 File Offset: 0x00016234
			public CCSAnime.Vec3Key_Scale GetValue(int keyID)
			{
				bool flag = this.KeyCount == 0;
				CCSAnime.Vec3Key_Scale result;
				if (flag)
				{
					result = this.FixedValue;
				}
				else
				{
					result = ((keyID < this.KeyCount) ? this.Keys[keyID] : this.Keys[this.Keys.Count - 1]);
				}
				return result;
			}

			// Token: 0x0600021A RID: 538 RVA: 0x0001808C File Offset: 0x0001628C
			public void Read(BinaryReader bStream, int TrackType, int frameCount)
			{
				if (TrackType != 1)
				{
					if (TrackType == 2)
					{
						this.KeyCount = bStream.ReadInt32();
						for (int index = 0; index < this.KeyCount; index++)
						{
							int _frameNum = bStream.ReadInt32();
							CCSAnime.Vec3Key_Scale vec3KeyScale = new CCSAnime.Vec3Key_Scale();
							vec3KeyScale.Read(bStream, _frameNum);
							bool flag = this.Keys.Count > 0;
							if (flag)
							{
								CCSAnime.Vec3Key_Scale key = this.Keys[this.Keys.Count - 1];
								bool flag2 = key.FrameNumber() == _frameNum;
								if (flag2)
								{
									this.Keys.Remove(key);
								}
							}
							bool flag3 = this.Keys.Count > 0;
							if (flag3)
							{
								this.Keys[this.Keys.Count - 1].SetFrameCount(vec3KeyScale.FrameNumber());
							}
							this.Keys.Add(vec3KeyScale);
						}
						this.Keys[this.Keys.Count - 1].SetFrameCount(frameCount);
					}
				}
				else
				{
					this.FixedValue.Read(bStream, 0);
				}
			}

			// Token: 0x0600021B RID: 539 RVA: 0x000181BC File Offset: 0x000163BC
			public Vector3 GetInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				Vector3 result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					CCSAnime.Vec3Key_Scale vec3KeyScale = this.GetValue(this.CurrentKey);
					bool flag3 = frameNumber >= vec3KeyScale.GetFrameCount();
					if (flag3)
					{
						this.CurrentKey++;
						bool flag4 = this.CurrentKey > this.Keys.Count - 1;
						if (flag4)
						{
							this.CurrentKey = 0;
						}
					}
					CCSAnime.Vec3Key_Scale vec3KeyScale2 = this.GetValue(this.CurrentKey);
					int keyID = this.CurrentKey + 1;
					bool flag5 = keyID > this.Keys.Count - 1;
					if (flag5)
					{
						keyID = 0;
					}
					CCSAnime.Vec3Key_Scale vec3KeyScale3 = this.GetValue(keyID);
					float num = 1f / (float)(vec3KeyScale2.GetFrameCount() - vec3KeyScale2.FrameNumber());
					int num2 = frameNumber - vec3KeyScale2.FrameNumber();
					result = Vector3.Lerp(vec3KeyScale2.Value(), vec3KeyScale3.Value(), num * (float)num2);
				}
				return result;
			}

			// Token: 0x040002A4 RID: 676
			public List<CCSAnime.Vec3Key_Scale> Keys = new List<CCSAnime.Vec3Key_Scale>();

			// Token: 0x040002A5 RID: 677
			public CCSAnime.Vec3Key_Scale FixedValue = new CCSAnime.Vec3Key_Scale();

			// Token: 0x040002A6 RID: 678
			private int KeyCount = 0;

			// Token: 0x040002A7 RID: 679
			private int CurrentKey = 0;
		}

		// Token: 0x02000057 RID: 87
		public class Vec2UV_Track
		{
			// Token: 0x0600021D RID: 541 RVA: 0x000182F0 File Offset: 0x000164F0
			public CCSAnime.Vec2Key_UV GetValue(int keyID)
			{
				bool flag = this.KeyCount == 0;
				CCSAnime.Vec2Key_UV result;
				if (flag)
				{
					result = this.FixedValue;
				}
				else
				{
					result = ((keyID < this.KeyCount) ? this.Keys[keyID] : this.Keys[this.Keys.Count - 1]);
				}
				return result;
			}

			// Token: 0x0600021E RID: 542 RVA: 0x00018348 File Offset: 0x00016548
			public void Read(BinaryReader bStream, int TrackType, int frameCount)
			{
				if (TrackType != 1)
				{
					if (TrackType == 2)
					{
						this.KeyCount = bStream.ReadInt32();
						for (int index = 0; index < this.KeyCount; index++)
						{
							int _frameNum = bStream.ReadInt32();
							CCSAnime.Vec2Key_UV vec2KeyUv = new CCSAnime.Vec2Key_UV();
							vec2KeyUv.Read(bStream, _frameNum);
							bool flag = this.Keys.Count > 0;
							if (flag)
							{
								CCSAnime.Vec2Key_UV key = this.Keys[this.Keys.Count - 1];
								bool flag2 = key.FrameNumber() == _frameNum;
								if (flag2)
								{
									this.Keys.Remove(key);
								}
							}
							bool flag3 = this.Keys.Count > 0;
							if (flag3)
							{
								this.Keys[this.Keys.Count - 1].SetFrameCount(vec2KeyUv.FrameNumber());
							}
							this.Keys.Add(vec2KeyUv);
						}
						this.Keys[this.Keys.Count - 1].SetFrameCount(frameCount);
					}
				}
				else
				{
					this.FixedValue.Read(bStream, 0);
				}
			}

			// Token: 0x0600021F RID: 543 RVA: 0x00018478 File Offset: 0x00016678
			public Vector2 GetInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				Vector2 result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					result = this.GetValue(this.CurrentKey).Value();
				}
				return result;
			}

			// Token: 0x040002A8 RID: 680
			public List<CCSAnime.Vec2Key_UV> Keys = new List<CCSAnime.Vec2Key_UV>();

			// Token: 0x040002A9 RID: 681
			public CCSAnime.Vec2Key_UV FixedValue = new CCSAnime.Vec2Key_UV();

			// Token: 0x040002AA RID: 682
			private int KeyCount = 0;

			// Token: 0x040002AB RID: 683
			private int CurrentKey = 0;
		}

		// Token: 0x02000058 RID: 88
		public class F32_Track
		{
			// Token: 0x06000221 RID: 545 RVA: 0x000184F4 File Offset: 0x000166F4
			public CCSAnime.F32Key GetValue(int keyID)
			{
				bool flag = this.KeyCount == 0;
				CCSAnime.F32Key result;
				if (flag)
				{
					result = this.FixedValue;
				}
				else
				{
					result = ((keyID < this.KeyCount) ? this.Keys[keyID] : this.Keys[this.Keys.Count - 1]);
				}
				return result;
			}

			// Token: 0x06000222 RID: 546 RVA: 0x0001854C File Offset: 0x0001674C
			public void Read(BinaryReader bStream, int TrackType, int frameCount)
			{
				if (TrackType != 1)
				{
					if (TrackType == 2)
					{
						this.KeyCount = bStream.ReadInt32();
						for (int index = 0; index < this.KeyCount; index++)
						{
							int _frameNum = bStream.ReadInt32();
							CCSAnime.F32Key f32Key = new CCSAnime.F32Key();
							f32Key.Read(bStream, _frameNum);
							bool flag = this.Keys.Count > 0;
							if (flag)
							{
								CCSAnime.F32Key key = this.Keys[this.Keys.Count - 1];
								bool flag2 = key.FrameNumber() == _frameNum;
								if (flag2)
								{
									this.Keys.Remove(key);
								}
							}
							bool flag3 = this.Keys.Count > 0;
							if (flag3)
							{
								this.Keys[this.Keys.Count - 1].SetFrameCount(f32Key.FrameNumber());
							}
							this.Keys.Add(f32Key);
						}
						this.Keys[this.Keys.Count - 1].SetFrameCount(frameCount - 1);
					}
				}
				else
				{
					this.FixedValue.Read(bStream, 0);
				}
			}

			// Token: 0x06000223 RID: 547 RVA: 0x0001867C File Offset: 0x0001687C
			public float GetInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				float result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					result = this.GetValue(this.CurrentKey).Value();
				}
				return result;
			}

			// Token: 0x06000224 RID: 548 RVA: 0x000186CC File Offset: 0x000168CC
			public float GetNonInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				float result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					CCSAnime.F32Key f32Key = this.GetValue(this.CurrentKey);
					bool flag3 = frameNumber >= f32Key.GetFrameCount();
					if (flag3)
					{
						this.CurrentKey++;
						bool flag4 = this.CurrentKey > this.Keys.Count - 1;
						if (flag4)
						{
							this.CurrentKey = 0;
						}
					}
					result = this.GetValue(this.CurrentKey).Value();
				}
				return result;
			}

			// Token: 0x040002AC RID: 684
			public List<CCSAnime.F32Key> Keys = new List<CCSAnime.F32Key>();

			// Token: 0x040002AD RID: 685
			public CCSAnime.F32Key FixedValue = new CCSAnime.F32Key();

			// Token: 0x040002AE RID: 686
			private int KeyCount = 0;

			// Token: 0x040002AF RID: 687
			private int CurrentKey = 0;
		}

		// Token: 0x02000059 RID: 89
		public class Int32_Track
		{
			// Token: 0x06000226 RID: 550 RVA: 0x00018798 File Offset: 0x00016998
			public CCSAnime.Int32Key GetValue(int keyID)
			{
				bool flag = this.KeyCount == 0;
				CCSAnime.Int32Key result;
				if (flag)
				{
					result = this.FixedValue;
				}
				else
				{
					result = ((keyID < this.KeyCount) ? this.Keys[keyID] : this.Keys[this.Keys.Count - 1]);
				}
				return result;
			}

			// Token: 0x06000227 RID: 551 RVA: 0x000187F0 File Offset: 0x000169F0
			public void Read(BinaryReader bStream, int TrackType, int frameCount)
			{
				if (TrackType != 1)
				{
					if (TrackType == 2)
					{
						this.KeyCount = bStream.ReadInt32();
						for (int index = 0; index < this.KeyCount; index++)
						{
							int _frameNum = bStream.ReadInt32();
							CCSAnime.Int32Key int32Key = new CCSAnime.Int32Key();
							int32Key.Read(bStream, _frameNum);
							bool flag = this.Keys.Count > 0;
							if (flag)
							{
								CCSAnime.Int32Key key = this.Keys[this.Keys.Count - 1];
								bool flag2 = key.FrameNumber() == _frameNum;
								if (flag2)
								{
									this.Keys.Remove(key);
								}
							}
							bool flag3 = this.Keys.Count > 0;
							if (flag3)
							{
								this.Keys[this.Keys.Count - 1].SetFrameCount(int32Key.FrameNumber());
							}
							this.Keys.Add(int32Key);
						}
						this.Keys[this.Keys.Count - 1].SetFrameCount(frameCount);
					}
				}
				else
				{
					this.FixedValue.Read(bStream, 0);
				}
			}

			// Token: 0x06000228 RID: 552 RVA: 0x00018920 File Offset: 0x00016B20
			public int GetInterpolatedValue(int frameNumber)
			{
				bool flag = this.KeyCount == 0;
				int result;
				if (flag)
				{
					result = this.FixedValue.Value();
				}
				else
				{
					bool flag2 = frameNumber == 0;
					if (flag2)
					{
						this.CurrentKey = 0;
					}
					result = this.GetValue(this.CurrentKey).Value();
				}
				return result;
			}

			// Token: 0x040002B0 RID: 688
			public List<CCSAnime.Int32Key> Keys = new List<CCSAnime.Int32Key>();

			// Token: 0x040002B1 RID: 689
			public CCSAnime.Int32Key FixedValue = new CCSAnime.Int32Key();

			// Token: 0x040002B2 RID: 690
			private int KeyCount = 0;

			// Token: 0x040002B3 RID: 691
			private int CurrentKey = 0;
		}

		// Token: 0x0200005A RID: 90
		public abstract class AnimationKeyFrame
		{
			// Token: 0x0600022A RID: 554
			public abstract void Read(BinaryReader bStream);

			// Token: 0x040002B4 RID: 692
			public int KeyFrameType;
		}

		// Token: 0x0200005B RID: 91
		public struct MorphKeyValue
		{
			// Token: 0x040002B5 RID: 693
			public int ModelID;

			// Token: 0x040002B6 RID: 694
			public float Value;
		}

		// Token: 0x0200005C RID: 92
		public class MorphKeyFrame : CCSAnime.AnimationKeyFrame
		{
			// Token: 0x0600022C RID: 556 RVA: 0x0001899A File Offset: 0x00016B9A
			public MorphKeyFrame()
			{
				this.KeyFrameType = 6401;
			}

			// Token: 0x0600022D RID: 557 RVA: 0x000189C4 File Offset: 0x00016BC4
			public override void Read(BinaryReader bStream)
			{
				this.MorphID = bStream.ReadInt32();
				this.MorphKeyCount = bStream.ReadInt32();
				this.MorpherKeys = new CCSAnime.MorphKeyValue[this.MorphKeyCount];
				for (int index = 0; index < this.MorphKeyCount; index++)
				{
					CCSAnime.MorphKeyValue morphKeyValue = default(CCSAnime.MorphKeyValue);
					morphKeyValue.ModelID = bStream.ReadInt32();
					morphKeyValue.Value = bStream.ReadSingle();
				}
			}

			// Token: 0x040002B7 RID: 695
			public int MorphID = 0;

			// Token: 0x040002B8 RID: 696
			public CCSAnime.MorphKeyValue[] MorpherKeys = null;

			// Token: 0x040002B9 RID: 697
			public int MorphKeyCount = 0;
		}

		// Token: 0x0200005D RID: 93
		public class AmbientLightKeyFrame : CCSAnime.AnimationKeyFrame
		{
			// Token: 0x0600022E RID: 558 RVA: 0x00018A36 File Offset: 0x00016C36
			public AmbientLightKeyFrame()
			{
				this.KeyFrameType = 1537;
			}

			// Token: 0x0600022F RID: 559 RVA: 0x00018A69 File Offset: 0x00016C69
			public override void Read(BinaryReader bStream)
			{
				this.Value = Util.ReadVec4RGBA32(bStream);
			}

			// Token: 0x040002BA RID: 698
			public Vector4 Value = new Vector4(0f, 0f, 0f, 0f);
		}

		// Token: 0x0200005E RID: 94
		public class AnimationFrame
		{
			// Token: 0x06000230 RID: 560 RVA: 0x00018A77 File Offset: 0x00016C77
			public AnimationFrame(int _frameNumber)
			{
				this.FrameNumber = _frameNumber;
			}

			// Token: 0x040002BB RID: 699
			public List<CCSAnime.AnimationKeyFrame> KeyFrames = new List<CCSAnime.AnimationKeyFrame>();

			// Token: 0x040002BC RID: 700
			public int FrameNumber = 0;
		}
	}
}
