using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using StudioCCS.libCCS;

namespace StudioCCS
{
	// Token: 0x02000010 RID: 16
	public static class Scene
	{
		// Token: 0x06000083 RID: 131 RVA: 0x0000A2B8 File Offset: 0x000084B8
		public static int LoadProgram(string programName, bool hasGeometryShader = false)
		{
			bool result = true;
			int vShaderID = GL.CreateShader(ShaderType.VertexShader);
			int fShaderID = GL.CreateShader(ShaderType.FragmentShader);
			int gShaderID = 0;
			if (hasGeometryShader)
			{
				gShaderID = GL.CreateShader(ShaderType.GeometryShader);
			}
			bool flag = !Scene.LoadShader("data/shaders/" + programName + ".vsh", vShaderID);
			if (flag)
			{
				result = false;
			}
			bool flag2 = !Scene.LoadShader("data/shaders/" + programName + ".fsh", fShaderID);
			if (flag2)
			{
				result = false;
			}
			if (hasGeometryShader)
			{
				bool flag3 = !Scene.LoadShader("data/shaders/" + programName + ".gsh", gShaderID);
				if (flag3)
				{
					result = false;
				}
			}
			int programID = GL.CreateProgram();
			bool flag4 = result;
			if (flag4)
			{
				GL.AttachShader(programID, vShaderID);
				GL.AttachShader(programID, fShaderID);
				if (hasGeometryShader)
				{
					GL.AttachShader(programID, gShaderID);
				}
				GL.LinkProgram(programID);
				int programLinkResult = 0;
				GL.GetProgram(programID, GetProgramParameterName.LinkStatus, out programLinkResult);
				bool flag5 = programLinkResult == 0;
				if (flag5)
				{
					Logger.LogError(string.Format("Error linking program {0}:\n{1}\n", programName, GL.GetProgramInfoLog(programID)), Logger.LogType.LogAll, "", 0);
					result = false;
				}
			}
			GL.DeleteShader(vShaderID);
			GL.DeleteShader(fShaderID);
			if (hasGeometryShader)
			{
				GL.DeleteShader(gShaderID);
			}
			bool flag6 = result;
			int result2;
			if (flag6)
			{
				result2 = programID;
			}
			else
			{
				GL.DeleteProgram(programID);
				result2 = -1;
			}
			return result2;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000A41C File Offset: 0x0000861C
		private static bool LoadShader(string fileName, int shaderID)
		{
			bool result;
			using (StreamReader sr = new StreamReader(fileName))
			{
				string shaderCode = sr.ReadToEnd();
				GL.ShaderSource(shaderID, shaderCode);
				GL.CompileShader(shaderID);
				int compileResult = 0;
				GL.GetShader(shaderID, ShaderParameter.CompileStatus, out compileResult);
				bool flag = compileResult == 0;
				if (flag)
				{
					Logger.LogError(string.Format("Error compiling shader {0}:\n{1}\n", fileName, GL.GetShaderInfoLog(shaderID)), Logger.LogType.LogAll, "", 0);
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000A4A8 File Offset: 0x000086A8
		public static TreeNode LoadCCSFile(string fileName)
		{
			CCSFile tmpCCS = new CCSFile();
			Stopwatch sw = new Stopwatch();
			sw.Start();
			bool flag = tmpCCS.Read(fileName);
			if (flag)
			{
				bool flag2 = tmpCCS.Init();
				if (flag2)
				{
					sw.Stop();
					Debug.WriteLine("Read and Initialized {0} in {1}ms...", new object[]
					{
						fileName,
						sw.ElapsedMilliseconds
					});
					Scene.CCSFileList.Add(tmpCCS);
					return tmpCCS.ToNode();
				}
			}
			sw.Stop();
			Debug.WriteLine("Failed to read {0} in {1}ms...", new object[]
			{
				fileName,
				sw.ElapsedMilliseconds
			});
			return null;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000A554 File Offset: 0x00008754
		public static bool UnloadCCSFile(CCSFile file)
		{
			file.DeInit();
			Scene.CCSFileList.Remove(file);
			return true;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000A57C File Offset: 0x0000877C
		public static TreeNode ToNode()
		{
			TreeNode retNode = new TreeNode();
			foreach (CCSFile tmpCCS in Scene.CCSFileList)
			{
				retNode.Nodes.Add(tmpCCS.ToNode());
			}
			return retNode;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000A5EC File Offset: 0x000087EC
		public static TreeNode ToSceneNode()
		{
			TreeNode tmpMainAnmNode = new TreeNode("Animations");
			foreach (CCSAnime tmpAnmNode in Scene.ActiveAnimes)
			{
				tmpMainAnmNode.Nodes.Add(tmpAnmNode.ToNode());
			}
			return tmpMainAnmNode;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000A660 File Offset: 0x00008860
		public static void Init(GLControl glCtrl)
		{
			Scene.control = glCtrl;
			Scene.control.MakeCurrent();
			Scene.WasInit = true;
			GL.ClearColor(0.2509804f, 0.2509804f, 0.2509804f, 1f);
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			GL.Enable(EnableCap.AlphaTest);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D);
			GL.Disable(EnableCap.CullFace);
			AxisMarker.Init();
			WireHelper.Init();
			Grid.Init();
			TexturePreview.Init();
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000A70C File Offset: 0x0000890C
		public static void DeInit()
		{
			foreach (CCSFile tmpCCS in Scene.CCSFileList)
			{
				tmpCCS.DeInit();
			}
			AxisMarker.DeInit();
			WireHelper.DeInit();
			Grid.DeInit();
			TexturePreview.DeInit();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000A77C File Offset: 0x0000897C
		private static void HandleInput()
		{
			ArcBallCamera curCamera = Scene.CurrentCamera();
			Vector3 CamTarget = curCamera.Target;
			bool shiftMod = Scene.ShiftModifier > Scene.KeyStatus.Up;
			bool flag = !Scene.DefaultToAxisMovement;
			if (flag)
			{
				shiftMod = !shiftMod;
			}
			bool flag2 = shiftMod;
			if (flag2)
			{
				Vector3 movement = new Vector3(0f, 0f, 0f);
				Matrix4 viewMtx = curCamera.GetMatrix();
				Vector3 forward = new Vector3(viewMtx[0, 2], viewMtx[1, 2], viewMtx[2, 2]).Normalized();
				Vector3 up = new Vector3(viewMtx[0, 1], viewMtx[1, 1], viewMtx[2, 1]).Normalized();
				Vector3 right = Vector3.Cross(forward, up).Normalized();
				bool flag3 = Scene.MoveForward > Scene.KeyStatus.Up;
				if (flag3)
				{
					movement += forward;
				}
				bool flag4 = Scene.MoveBackward > Scene.KeyStatus.Up;
				if (flag4)
				{
					movement -= forward;
				}
				bool flag5 = Scene.MoveLeft > Scene.KeyStatus.Up;
				if (flag5)
				{
					movement -= right;
				}
				bool flag6 = Scene.MoveRight > Scene.KeyStatus.Up;
				if (flag6)
				{
					movement += right;
				}
				bool flag7 = Scene.ControlModifier == Scene.KeyStatus.Up;
				if (flag7)
				{
					movement *= new Vector3(1f, 0f, 1f);
				}
				bool flag8 = Scene.MoveUp > Scene.KeyStatus.Up;
				if (flag8)
				{
					movement += Vector3.UnitY;
				}
				bool flag9 = Scene.MoveDown > Scene.KeyStatus.Up;
				if (flag9)
				{
					movement -= Vector3.UnitY;
				}
				CamTarget += movement * Scene.MovementSpeed * Scene.DeltaTime;
			}
			else
			{
				bool flag10 = Scene.MoveForward > Scene.KeyStatus.Up;
				if (flag10)
				{
					CamTarget.Z -= Scene.DeltaTime * Scene.MovementSpeed;
				}
				bool flag11 = Scene.MoveBackward > Scene.KeyStatus.Up;
				if (flag11)
				{
					CamTarget.Z += Scene.DeltaTime * Scene.MovementSpeed;
				}
				bool flag12 = Scene.MoveLeft > Scene.KeyStatus.Up;
				if (flag12)
				{
					CamTarget.X -= Scene.DeltaTime * Scene.MovementSpeed;
				}
				bool flag13 = Scene.MoveRight > Scene.KeyStatus.Up;
				if (flag13)
				{
					CamTarget.X += Scene.DeltaTime * Scene.MovementSpeed;
				}
				bool flag14 = Scene.MoveUp > Scene.KeyStatus.Up;
				if (flag14)
				{
					CamTarget.Y -= Scene.DeltaTime * Scene.MovementSpeed;
				}
				bool flag15 = Scene.MoveDown > Scene.KeyStatus.Up;
				if (flag15)
				{
					CamTarget.Y += Scene.DeltaTime * Scene.MovementSpeed;
				}
			}
			curCamera.Target = CamTarget;
			float keyZoom = 0.0075f;
			float distToZoom = Scene.DeltaTime * keyZoom;
			bool flag16 = Scene.ShiftModifier > Scene.KeyStatus.Up;
			if (flag16)
			{
				distToZoom *= 0.25f;
			}
			bool flag17 = Scene.ZoomIn > Scene.KeyStatus.Up;
			if (flag17)
			{
				curCamera.Distance -= distToZoom;
			}
			bool flag18 = Scene.ZoomOut > Scene.KeyStatus.Up;
			if (flag18)
			{
				curCamera.Distance += distToZoom;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000AA90 File Offset: 0x00008C90
		public static void Render()
		{
			Scene.MakeCurrent();
			bool backfaceCull = Scene.BackfaceCull;
			if (backfaceCull)
			{
				GL.Enable(EnableCap.CullFace);
			}
			else
			{
				GL.Disable(EnableCap.CullFace);
			}
			Scene.Timer.Stop();
			Scene.DeltaTime = (float)Scene.Timer.Elapsed.TotalMilliseconds;
			Scene.Timer.Reset();
			Scene.Timer.Start();
			Scene.HandleInput();
			ArcBallCamera curCamera = Scene.CurrentCamera();
			GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
			Scene.SetMainViewport();
			Matrix4 CamMtx = curCamera.GetMatrix();
			Matrix4 ProjViewMtx = CamMtx * Scene.ProjectionMtx;
			Matrix4 CCSMatrix = Matrix4.CreateRotationX(-1.5707964f) * ProjViewMtx;
			Matrix4 Helper = Matrix4.CreateTranslation(-4f, 0f, 0f) * ProjViewMtx;
			Matrix4 Helper2 = Matrix4.CreateTranslation(4f, 0f, 0f) * ProjViewMtx;
			bool drawWorldCenter = Scene.DrawWorldCenter;
			if (drawWorldCenter)
			{
				Scene.RenderViewAxisGizmo(10f, Scene.ProjectionMtx, false);
			}
			bool drawViewAxis = Scene.DrawViewAxis;
			if (drawViewAxis)
			{
				Scene.SetAxisViewport();
				Scene.RenderViewAxisGizmo(1.75f, Scene.AxisProjectionMtx, true);
				Scene.SetMainViewport();
			}
			bool drawViewGrid = Scene.DrawViewGrid;
			if (drawViewGrid)
			{
				Grid.Render(Matrix4.CreateTranslation(0f, 0f, 0f) * ProjViewMtx, 1f);
			}
			bool flag = Scene.SceneDisplay == Scene.SceneMode.Preview;
			if (flag)
			{
				Scene.PreviewRender(CCSMatrix);
			}
			else
			{
				bool flag2 = Scene.SceneDisplay == Scene.SceneMode.Scene;
				if (flag2)
				{
					Scene.SceneRender(CCSMatrix);
				}
				else
				{
					Scene.AllRender(CCSMatrix);
				}
			}
			Scene.control.SwapBuffers();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000AC38 File Offset: 0x00008E38
		private static bool IsPreviewTexture()
		{
			bool flag = Scene.SelectedPreviewItemTag == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = Scene.SelectedPreviewItemTag.ObjectType != 768;
				result = !flag2;
			}
			return result;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000AC78 File Offset: 0x00008E78
		public static int GetRenderMode()
		{
			int retVal = 0;
			bool drawWireframe = Scene.DrawWireframe;
			if (drawWireframe)
			{
				retVal |= Scene.SCENE_DRAW_LINES;
			}
			bool drawVertexColors = Scene.DrawVertexColors;
			if (drawVertexColors)
			{
				retVal |= Scene.SCENE_DRAW_VERTEX_COLORS;
			}
			bool drawVertexNormals = Scene.DrawVertexNormals;
			if (drawVertexNormals)
			{
				retVal |= Scene.SCENE_DRAW_SMOOTH;
			}
			bool drawTextures = Scene.DrawTextures;
			if (drawTextures)
			{
				retVal |= Scene.SCENE_DRAW_TEXTURE;
			}
			return retVal;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x0000ACD5 File Offset: 0x00008ED5
		private static void AllRender(Matrix4 ProjViewMtx)
		{
			Scene.RenderAllCCS(ProjViewMtx);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x0000ACE0 File Offset: 0x00008EE0
		private static void SceneRender(Matrix4 ProjViewMtx)
		{
			foreach (CCSAnime tmpAnime in Scene.ActiveAnimes)
			{
				tmpAnime.Render(ProjViewMtx, Scene.GetRenderMode());
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000AD40 File Offset: 0x00008F40
		private static void PreviewRender(Matrix4 ProjViewMtx)
		{
			int extraOptions = Scene.GetRenderMode();
			bool flag = Scene.SelectedPreviewItemTag != null;
			if (flag)
			{
				bool flag2 = Scene.SelectedPreviewItemTag.ObjectType == 2304;
				if (flag2)
				{
					CCSClump tmpClump = Scene.SelectedPreviewItemTag.File.GetObject<CCSClump>(Scene.SelectedPreviewItemTag.ObjectID);
					tmpClump.Render(ProjViewMtx, extraOptions);
				}
				else
				{
					bool flag3 = Scene.SelectedPreviewItemTag.ObjectType == 256;
					if (flag3)
					{
						CCSObject tmpObj = Scene.SelectedPreviewItemTag.File.GetObject<CCSObject>(Scene.SelectedPreviewItemTag.ObjectID);
						tmpObj.ParentClump.FrameForward();
						tmpObj.Render(ProjViewMtx, extraOptions);
					}
					else
					{
						bool flag4 = Scene.SelectedPreviewItemTag.ObjectType == 2048;
						if (flag4)
						{
							int subNode = -1;
							bool flag5 = Scene.SelectedPreviewItemTag.Type == TreeNodeTag.NodeType.SubNode;
							if (flag5)
							{
								subNode = Scene.SelectedPreviewItemTag.SubID;
							}
							CCSModel tmpModel = Scene.SelectedPreviewItemTag.File.GetObject<CCSModel>(Scene.SelectedPreviewItemTag.ObjectID);
							tmpModel.ClumpRef.FrameForward();
							tmpModel.Render(ProjViewMtx, extraOptions, subNode);
						}
						else
						{
							bool flag6 = Scene.SelectedPreviewItemTag.ObjectType == 768;
							if (flag6)
							{
								CCSTexture tex = Scene.SelectedPreviewItemTag.File.GetObject<CCSTexture>(Scene.SelectedPreviewItemTag.ObjectID);
								bool flag7 = tex != null;
								if (flag7)
								{
									float texW = (float)tex.Width;
									float texH = (float)tex.Height;
									TexturePreview.Render(ProjViewMtx, tex.TextureID, texW, texH);
								}
							}
							else
							{
								bool flag8 = Scene.SelectedPreviewItemTag.ObjectType == 1792;
								if (flag8)
								{
									CCSAnime tmpAnime = Scene.SelectedPreviewItemTag.File.GetObject<CCSAnime>(Scene.SelectedPreviewItemTag.ObjectID);
									bool flag9 = tmpAnime != null;
									if (flag9)
									{
										tmpAnime.Render(ProjViewMtx, extraOptions);
									}
								}
								else
								{
									bool flag10 = Scene.SelectedPreviewItemTag.ObjectType == 4864 | Scene.SelectedPreviewItemTag.ObjectType == 5120;
									if (flag10)
									{
										foreach (CCSFile ccsFile in Scene.CCSFileList)
										{
											CCSDummy ccsDummy = Scene.SelectedPreviewItemTag.File.GetObject<CCSDummy>(Scene.SelectedPreviewItemTag.ObjectID);
											WireHelper.RenderDummyHelper(ProjViewMtx, ccsDummy);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x0000AFB8 File Offset: 0x000091B8
		private static void RenderAllCCS(Matrix4 ProjViewMtx)
		{
			int extraOptions = Scene.GetRenderMode();
			foreach (CCSFile tmpCCS in Scene.CCSFileList)
			{
				List<CCSClump> clumpList = tmpCCS.ClumpList;
				foreach (CCSClump tmpClump in clumpList)
				{
					tmpClump.Render(ProjViewMtx, extraOptions);
				}
				bool drawCollisionMeshes = Scene.DrawCollisionMeshes;
				if (drawCollisionMeshes)
				{
					List<CCSHitMesh> hitList = tmpCCS.HitList;
					foreach (CCSHitMesh tmpHit in hitList)
					{
						tmpHit.RenderAll(ProjViewMtx, -1);
					}
				}
				bool drawDummyHelpers = Scene.DrawDummyHelpers;
				if (drawDummyHelpers)
				{
					List<CCSDummy> dummyList = tmpCCS.DummyList;
					foreach (CCSDummy tmpDummy in dummyList)
					{
						WireHelper.RenderDummyHelper(ProjViewMtx, tmpDummy);
					}
				}
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000B118 File Offset: 0x00009318
		public static void RenderViewAxisGizmo(float size, Matrix4 ProjMtx, bool disableDepth = false)
		{
			if (disableDepth)
			{
				GL.Disable(EnableCap.DepthTest);
			}
			ArcBallCamera curCam = Scene.CurrentCamera();
			Matrix4 ProjViewMtx = curCam.GetMatrixDistanced(size) * ProjMtx * Matrix4.Identity;
			AxisMarker.Render(Matrix4.CreateTranslation(0f, 0f, 0f) * ProjViewMtx, 1f);
			GL.Enable(EnableCap.DepthTest);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000B184 File Offset: 0x00009384
		private static void SetMainViewport()
		{
			GL.Viewport(0, 0, Scene.control.Width, Scene.control.Height);
			Scene.ProjectionMtx = Matrix4.CreatePerspectiveFieldOfView(0.7853982f, (float)Scene.control.Width / (float)Scene.control.Height, 0.1f, 100000f);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000B1E0 File Offset: 0x000093E0
		private static void SetAxisViewport()
		{
			GL.Viewport(Scene.control.Width - 80, Scene.control.Height - 80, 80, 80);
			Scene.AxisProjectionMtx = Matrix4.CreatePerspectiveFieldOfView(0.7853982f, 1f, 0.01f, 100000f);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000B230 File Offset: 0x00009430
		public static void MouseMove(MouseEventArgs e)
		{
			float mX = (float)e.X;
			float mY = (float)e.Y;
			float dX = mX - Scene.LastMouseX;
			float dY = mY - Scene.LastMouseY;
			ArcBallCamera curCam = Scene.CurrentCamera();
			Vector3 camRot = curCam.Rotation;
			Vector3 camTarget = curCam.Target;
			bool flag = (e.Button & MouseButtons.Right) > MouseButtons.None;
			if (flag)
			{
				float dXm = Scene.MouseSensitivity * dX;
				float dYm = Scene.MouseSensitivity * dY;
				curCam.Rotation = new Vector3(camRot.X + dXm, camRot.Y + dYm, 0f);
			}
			Scene.LastMouseX = mX;
			Scene.LastMouseY = mY;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000B2D4 File Offset: 0x000094D4
		public static void MouseWheel(MouseEventArgs e)
		{
			ArcBallCamera curCam = Scene.CurrentCamera();
			float distToZoom = (float)e.Delta * Scene.MouseWheelSensitivity * Scene.DeltaTime;
			bool flag = Scene.ShiftModifier > Scene.KeyStatus.Up;
			if (flag)
			{
				distToZoom *= 0.25f;
			}
			curCam.Distance += distToZoom;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000B320 File Offset: 0x00009520
		public static void KeyPress(KeyEventArgs e)
		{
			bool flag = e.KeyCode == Scene.MoveForward_Key;
			if (flag)
			{
				Scene.MoveForward = ((Scene.MoveForward == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
			}
			else
			{
				bool flag2 = e.KeyCode == Scene.MoveBackward_Key;
				if (flag2)
				{
					Scene.MoveBackward = ((Scene.MoveBackward == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
				}
				else
				{
					bool flag3 = e.KeyCode == Scene.MoveLeft_Key;
					if (flag3)
					{
						Scene.MoveLeft = ((Scene.MoveLeft == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
					}
					else
					{
						bool flag4 = e.KeyCode == Scene.MoveRight_Key;
						if (flag4)
						{
							Scene.MoveRight = ((Scene.MoveRight == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
						}
						else
						{
							bool flag5 = e.KeyCode == Scene.MoveUp_Key;
							if (flag5)
							{
								Scene.MoveUp = ((Scene.MoveUp == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
							}
							else
							{
								bool flag6 = e.KeyCode == Scene.MoveDown_Key;
								if (flag6)
								{
									Scene.MoveDown = ((Scene.MoveDown == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
								}
								else
								{
									bool flag7 = e.KeyCode == Scene.ZoomIn_Key;
									if (flag7)
									{
										Scene.ZoomIn = ((Scene.ZoomIn == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
									}
									else
									{
										bool flag8 = e.KeyCode == Scene.ZoomOut_Key;
										if (flag8)
										{
											Scene.ZoomOut = ((Scene.ZoomOut == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
										}
									}
								}
							}
						}
					}
				}
			}
			bool flag9 = e.KeyCode == Scene.QuickChange_Key;
			if (flag9)
			{
				Scene.ShiftModifier = ((Scene.ShiftModifier == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
			}
			bool flag10 = e.KeyCode == Scene.FastMove_Key;
			if (flag10)
			{
				Scene.MovementSpeed = 0.0025f * (float)Scene.MultiplySpeed;
			}
			bool flag11 = e.Control;
			if (flag11)
			{
				Scene.ControlModifier = ((Scene.ControlModifier == Scene.KeyStatus.Pressed) ? Scene.KeyStatus.Repeated : Scene.KeyStatus.Pressed);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public static void KeyRelease(KeyEventArgs e)
		{
			bool flag = e.KeyCode == Scene.MoveForward_Key;
			if (flag)
			{
				Scene.MoveForward = Scene.KeyStatus.Up;
			}
			else
			{
				bool flag2 = e.KeyCode == Scene.MoveBackward_Key;
				if (flag2)
				{
					Scene.MoveBackward = Scene.KeyStatus.Up;
				}
				else
				{
					bool flag3 = e.KeyCode == Scene.MoveLeft_Key;
					if (flag3)
					{
						Scene.MoveLeft = Scene.KeyStatus.Up;
					}
					else
					{
						bool flag4 = e.KeyCode == Scene.MoveRight_Key;
						if (flag4)
						{
							Scene.MoveRight = Scene.KeyStatus.Up;
						}
						else
						{
							bool flag5 = e.KeyCode == Scene.MoveUp_Key;
							if (flag5)
							{
								Scene.MoveUp = Scene.KeyStatus.Up;
							}
							else
							{
								bool flag6 = e.KeyCode == Scene.MoveDown_Key;
								if (flag6)
								{
									Scene.MoveDown = Scene.KeyStatus.Up;
								}
								else
								{
									bool flag7 = e.KeyCode == Scene.ZoomIn_Key;
									if (flag7)
									{
										Scene.ZoomIn = Scene.KeyStatus.Up;
									}
									else
									{
										bool flag8 = e.KeyCode == Scene.ZoomOut_Key;
										if (flag8)
										{
											Scene.ZoomOut = Scene.KeyStatus.Up;
										}
									}
								}
							}
						}
					}
				}
			}
			bool flag9 = e.KeyCode == Scene.FastMove_Key;
			if (flag9)
			{
				Scene.MovementSpeed = 0.0025f;
			}
			bool flag10 = e.KeyCode != Scene.QuickChange_Key;
			if (flag10)
			{
				Scene.ShiftModifier = Scene.KeyStatus.Up;
			}
			bool flag11 = !e.Control;
			if (flag11)
			{
				Scene.ControlModifier = Scene.KeyStatus.Up;
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000B5FC File Offset: 0x000097FC
		public static ArcBallCamera CurrentCamera()
		{
			bool flag = Scene.SceneDisplay == Scene.SceneMode.Preview;
			ArcBallCamera result;
			if (flag)
			{
				result = Scene.PreviewCamera;
			}
			else
			{
				bool flag2 = Scene.SceneDisplay == Scene.SceneMode.Scene;
				if (flag2)
				{
					result = Scene.SceneCamera;
				}
				else
				{
					result = Scene.AllCamera;
				}
			}
			return result;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000B63B File Offset: 0x0000983B
		public static void MakeCurrent()
		{
			Scene.control.MakeCurrent();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000B64C File Offset: 0x0000984C
		public static void DumpToObj(string outputPath, bool collision, bool splitSubModels, bool splitCollision, bool withNormals, bool dummies, bool animes)
		{
			foreach (CCSFile tmpCCS in Scene.CCSFileList)
			{
				tmpCCS.DumpToObj(outputPath, collision, splitSubModels, splitCollision, withNormals, dummies);
				if (animes)
				{
					tmpCCS.DumpAnimationsToText(outputPath);
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000B6BC File Offset: 0x000098BC
		public static void DumpToSMD(string outputPath, bool withNormals)
		{
			foreach (CCSFile tmpCCS in Scene.CCSFileList)
			{
				tmpCCS.DumpToSMD(outputPath, withNormals);
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000B718 File Offset: 0x00009918
		public static void AddAnime(CCSAnime anime)
		{
			for (int i = 0; i < Scene.ActiveAnimes.Count; i++)
			{
				CCSAnime tmpAnime = Scene.ActiveAnimes[i];
				bool flag = tmpAnime == anime;
				if (flag)
				{
					return;
				}
			}
			Scene.ActiveAnimes.Add(anime);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000B764 File Offset: 0x00009964
		public static void RemoveAnime(CCSAnime anime)
		{
			Scene.ActiveAnimes.RemoveAll((CCSAnime item) => item == anime);
		}

		// Token: 0x040000CD RID: 205
		private const int AxisViewSize = 80;

		// Token: 0x040000CE RID: 206
		public static int SCENE_DRAW_LINES = 1;

		// Token: 0x040000CF RID: 207
		public static int SCENE_DRAW_VERTEX_COLORS = 2;

		// Token: 0x040000D0 RID: 208
		public static int SCENE_DRAW_SMOOTH = 4;

		// Token: 0x040000D1 RID: 209
		public static int SCENE_DRAW_TEXTURE = 8;

		// Token: 0x040000D2 RID: 210
		public static int SCENE_DRAW_SELECTION = 16;

		// Token: 0x040000D3 RID: 211
		public static int SCENE_DRAW_FLIP_TEXCOORDS = 32;

		// Token: 0x040000D4 RID: 212
		public static Scene.SceneMode SceneDisplay = Scene.SceneMode.Preview;

		// Token: 0x040000D5 RID: 213
		public static bool BackfaceCull = false;

		// Token: 0x040000D6 RID: 214
		public static GLControl control = null;

		// Token: 0x040000D7 RID: 215
		public static bool WasInit = false;

		// Token: 0x040000D8 RID: 216
		public static int ViewWidth;

		// Token: 0x040000D9 RID: 217
		public static int ViewHeight;

		// Token: 0x040000DA RID: 218
		public static Stopwatch Timer = new Stopwatch();

		// Token: 0x040000DB RID: 219
		public static Matrix4 ProjectionMtx = Matrix4.Identity;

		// Token: 0x040000DC RID: 220
		public static Matrix4 AxisProjectionMtx = Matrix4.Identity;

		// Token: 0x040000DD RID: 221
		public static bool DrawViewAxis = true;

		// Token: 0x040000DE RID: 222
		public static bool DrawViewGrid = true;

		// Token: 0x040000DF RID: 223
		public static bool DrawCollisionMeshes = true;

		// Token: 0x040000E0 RID: 224
		public static bool DrawWorldCenter = true;

		// Token: 0x040000E1 RID: 225
		public static bool DrawDummyHelpers = true;

		// Token: 0x040000E2 RID: 226
		public static bool DrawLightHelpers = true;

		// Token: 0x040000E3 RID: 227
		public static bool DrawWireframe = false;

		// Token: 0x040000E4 RID: 228
		public static bool DrawVertexColors = false;

		// Token: 0x040000E5 RID: 229
		public static bool DrawVertexNormals = false;

		// Token: 0x040000E6 RID: 230
		public static bool DrawTextures = false;

		// Token: 0x040000E7 RID: 231
		public static ArcBallCamera PreviewCamera = new ArcBallCamera();

		// Token: 0x040000E8 RID: 232
		public static ArcBallCamera SceneCamera = new ArcBallCamera();

		// Token: 0x040000E9 RID: 233
		public static ArcBallCamera AllCamera = new ArcBallCamera();

		// Token: 0x040000EA RID: 234
		public static bool DefaultToAxisMovement = false;

		// Token: 0x040000EB RID: 235
		public static float LastMouseX = 0f;

		// Token: 0x040000EC RID: 236
		public static float LastMouseY = 0f;

		// Token: 0x040000ED RID: 237
		public static float MouseSensitivity = 0.2f;

		// Token: 0x040000EE RID: 238
		public static float MouseWheelSensitivity = 5E-05f;

		// Token: 0x040000EF RID: 239
		public static float MovementSpeed = 0.0025f;

		// Token: 0x040000F0 RID: 240
		public static float DeltaTime = 1f;

		// Token: 0x040000F1 RID: 241
		public static short MultiplySpeed = 20;

		// Token: 0x040000F2 RID: 242
		private static Scene.KeyStatus MoveForward = Scene.KeyStatus.Up;

		// Token: 0x040000F3 RID: 243
		private static Scene.KeyStatus MoveBackward = Scene.KeyStatus.Up;

		// Token: 0x040000F4 RID: 244
		private static Scene.KeyStatus MoveLeft = Scene.KeyStatus.Up;

		// Token: 0x040000F5 RID: 245
		private static Scene.KeyStatus MoveRight = Scene.KeyStatus.Up;

		// Token: 0x040000F6 RID: 246
		private static Scene.KeyStatus MoveUp = Scene.KeyStatus.Up;

		// Token: 0x040000F7 RID: 247
		private static Scene.KeyStatus MoveDown = Scene.KeyStatus.Up;

		// Token: 0x040000F8 RID: 248
		private static Scene.KeyStatus ZoomIn = Scene.KeyStatus.Up;

		// Token: 0x040000F9 RID: 249
		private static Scene.KeyStatus ZoomOut = Scene.KeyStatus.Up;

		// Token: 0x040000FA RID: 250
		private static Scene.KeyStatus ShiftModifier = Scene.KeyStatus.Up;

		// Token: 0x040000FB RID: 251
		private static Scene.KeyStatus ControlModifier = Scene.KeyStatus.Up;

		// Token: 0x040000FC RID: 252
		private static Keys MoveForward_Key = Keys.W;

		// Token: 0x040000FD RID: 253
		private static Keys MoveBackward_Key = Keys.S;

		// Token: 0x040000FE RID: 254
		private static Keys MoveLeft_Key = Keys.A;

		// Token: 0x040000FF RID: 255
		private static Keys MoveRight_Key = Keys.D;

		// Token: 0x04000100 RID: 256
		private static Keys MoveUp_Key = Keys.X;

		// Token: 0x04000101 RID: 257
		private static Keys MoveDown_Key = Keys.Z;

		// Token: 0x04000102 RID: 258
		public static Keys FastMove_Key = Keys.C;

		// Token: 0x04000103 RID: 259
		public static Keys QuickChange_Key = Keys.Shift;

		// Token: 0x04000104 RID: 260
		private static Keys ZoomIn_Key = Keys.Oemplus;

		// Token: 0x04000105 RID: 261
		private static Keys ZoomOut_Key = Keys.OemMinus;

		// Token: 0x04000106 RID: 262
		public static List<CCSFile> CCSFileList = new List<CCSFile>();

		// Token: 0x04000107 RID: 263
		public static List<CCSAnime> ActiveAnimes = new List<CCSAnime>();

		// Token: 0x04000108 RID: 264
		public static TreeNodeTag SelectedPreviewItemTag = null;

		// Token: 0x0200003D RID: 61
		public class SceneInstanceObject
		{
			// Token: 0x060001AC RID: 428 RVA: 0x00016910 File Offset: 0x00014B10
			public SceneInstanceObject(TreeNodeTag tag)
			{
				this.File = tag.File;
				this.ObjectType = tag.ObjectType;
			}

			// Token: 0x060001AD RID: 429 RVA: 0x00016947 File Offset: 0x00014B47
			public void AttachTo(Scene.SceneInstanceObject NewParent)
			{
				this.Parent = NewParent;
			}

			// Token: 0x060001AE RID: 430 RVA: 0x00016951 File Offset: 0x00014B51
			public void Detatch()
			{
				this.Parent = null;
			}

			// Token: 0x060001AF RID: 431 RVA: 0x00008ED7 File Offset: 0x000070D7
			public void Render()
			{
			}

			// Token: 0x0400024D RID: 589
			public CCSFile File = null;

			// Token: 0x0400024E RID: 590
			public CCSBaseObject Object = null;

			// Token: 0x0400024F RID: 591
			public int ObjectType = 0;

			// Token: 0x04000250 RID: 592
			public Vector3 PositionOffset;

			// Token: 0x04000251 RID: 593
			public Vector3 RotationOffset;

			// Token: 0x04000252 RID: 594
			public Scene.SceneInstanceObject Parent;
		}

		// Token: 0x0200003E RID: 62
		public enum SceneMode
		{
			// Token: 0x04000254 RID: 596
			Preview,
			// Token: 0x04000255 RID: 597
			Scene,
			// Token: 0x04000256 RID: 598
			All
		}

		// Token: 0x0200003F RID: 63
		public enum KeyStatus
		{
			// Token: 0x04000258 RID: 600
			Up,
			// Token: 0x04000259 RID: 601
			Pressed,
			// Token: 0x0400025A RID: 602
			Repeated
		}
	}
}
