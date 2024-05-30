using System;
using OpenTK;

namespace StudioCCS
{
	// Token: 0x02000003 RID: 3
	public class ArcBallCamera
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000023BC File Offset: 0x000005BC
		public ArcBallCamera()
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000242C File Offset: 0x0000062C
		public ArcBallCamera(Vector3 _position, float _distance)
		{
			this.Position = _position;
			this.Rotation = new Vector3(0f, 0f, 0f);
			this.Distance = _distance;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000024C4 File Offset: 0x000006C4
		private void Clamp()
		{
			bool flag = this.Rotation.Y > 90f;
			if (flag)
			{
				this.Rotation.Y = 90f;
			}
			bool flag2 = this.Rotation.Y < -90f;
			if (flag2)
			{
				this.Rotation.Y = -90f;
			}
			bool flag3 = this.Rotation.X > 360f;
			if (flag3)
			{
				this.Rotation.X = 0f;
			}
			bool flag4 = this.Rotation.X < 0f;
			if (flag4)
			{
				this.Rotation.X = 360f;
			}
			bool flag5 = this.Distance < 0.1f;
			if (flag5)
			{
				this.Distance = 0.1f;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002588 File Offset: 0x00000788
		public Matrix4 GetMatrix()
		{
			this.Calculate();
			Matrix4 cameraMatrix = Matrix4.LookAt(this.Position, Vector3.Zero, Vector3.UnitY);
			return Matrix4.CreateTranslation(this.Target) * cameraMatrix;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000025C8 File Offset: 0x000007C8
		public Matrix4 GetMatrixDistanced(float dist)
		{
			Matrix4 cameraMatrix = Matrix4.LookAt(new Vector3
			{
				X = (float)((double)dist * -(float)Math.Sin((double)(this.Rotation.X * 0.017453292f)) * Math.Cos((double)(this.Rotation.Y * 0.017453292f))),
				Y = (float)((double)dist * -(float)Math.Sin((double)(this.Rotation.Y * 0.017453292f))),
				Z = -(float)((double)(-(double)dist) * Math.Cos((double)(this.Rotation.X * 0.017453292f)) * Math.Cos((double)(this.Rotation.Y * 0.017453292f)))
			}, Vector3.Zero, Vector3.UnitY);
			return Matrix4.CreateTranslation(0f, 0f, 0f) * cameraMatrix;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000026A8 File Offset: 0x000008A8
		private void Calculate()
		{
			this.Clamp();
			this.Position.X = (float)((double)this.Distance * -(float)Math.Sin((double)(this.Rotation.X * 0.017453292f)) * Math.Cos((double)(this.Rotation.Y * 0.017453292f)));
			this.Position.Y = (float)((double)this.Distance * -(float)Math.Sin((double)(this.Rotation.Y * 0.017453292f)));
			this.Position.Z = -(float)((double)(-(double)this.Distance) * Math.Cos((double)(this.Rotation.X * 0.017453292f)) * Math.Cos((double)(this.Rotation.Y * 0.017453292f)));
		}

		// Token: 0x04000001 RID: 1
		public Vector3 Position = new Vector3(0f, 0f, 0f);

		// Token: 0x04000002 RID: 2
		public Vector3 Rotation = new Vector3(0f, -10f, 0f);

		// Token: 0x04000003 RID: 3
		public Vector3 Target = new Vector3(0f, 0f, 0f);

		// Token: 0x04000004 RID: 4
		public float Distance = 10f;
	}
}
