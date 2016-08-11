using Lockstep;
using System;
using UnityEngine;

namespace Lockstep
{
	public class FixedOBB2D
	{
		public Vector2d m_Min = Vector2d.zero;
		public Vector2d m_Max = Vector2d.zero;

		public Vector2d m_Center = Vector2d.zero;
		public Vector2d m_Half = Vector2d.zero;
		//检测轴单位向量
		public Vector2d m_AxisX = Vector2d.zero;
		public Vector2d m_AxisY = Vector2d.zero;
		[FixedNumber]
		public long m_Rotation = 0;

		public FixedOBB2D()
		{

		}


		public FixedOBB2D(float x, float y, float halfx, float halfy)
		{
			init(x, y, halfx, halfy);
		}

		public FixedOBB2D(long x, long y, long halfx, long halfy)
		{
			init(x, y, halfx, halfy);
		}

		public void init(float x, float y, float halfx, float halfy)
		{
			m_Center.x = FixedMath.Create(x);
			m_Center.y = FixedMath.Create(y);
			m_Half.x = FixedMath.Create(halfx);
			m_Half.y = FixedMath.Create(halfy);
			setRotation((long)0);
		}

		public void init(long x, long y, long halfx, long halfy)
		{
			m_Center.x = x;
			m_Center.y = y;
			m_Half.x = halfx;
			m_Half.y = halfy;
			setRotation((long)0);
		}

		void updateBouding()
		{
			Vector2d[] list = new Vector2d[4];
			Vector2d px = m_AxisX * m_Half.x;
			Vector2d py = m_AxisY * m_Half.y;
			list[0] = px + py;
			list[1] = px - py;
			list[2] = Vector2d.zero - px - py;
			list[3] = Vector2d.zero - px + py;
			m_Max = list[0];
			m_Min = list[0];
			for (int i = 1; i < list.Length; ++i) {
				var test = list[i];
				if (test.x < m_Min.x) {
					m_Min.x = test.x;
				}
				if (test.y < m_Min.y) {
					m_Min.y = test.y;
				}
				if (test.x > m_Max.x) {
					m_Max.x = test.x;
				}
				if (test.y > m_Max.y) {
					m_Max.y = test.y;
				}
			}
			m_Min = m_Min + m_Center;
			m_Max = m_Max + m_Center;
		}

		public void update(float x, float y, float halfx, float halfy)
		{
			init(x, y, halfx, halfy);
		}

		public void setRotation(float rotation)
		{
			setRotation(FixedMath.Create(rotation));
		}

		public void setRotation(long rotation)
		{
			m_Rotation = rotation;
			m_AxisX.x = FixedMath.Trig.Cos(rotation);
			m_AxisX.y = FixedMath.Trig.Sin(rotation);

			m_AxisY.x = -m_AxisX.y;
			m_AxisY.y = m_AxisX.x;
			updateBouding();
		}

		public long getProjectionRadius(Vector2d axis)
		{
			long pAxisX = Math.Abs(axis.Dot(m_AxisX));
			long pAxisY = Math.Abs(axis.Dot(m_AxisY));
			return m_Half.x.Mul(pAxisX) + m_Half.y.Mul(pAxisY);
		}

		public bool isCollision(FixedOBB2D obb)
		{
			Vector2d centerdis = m_Center - obb.m_Center;
			//4条检测轴
			Vector2d[] axes = {
				m_AxisX,
				m_AxisY,
				obb.m_AxisX,
				obb.m_AxisY,
			};
			long r1, r2, r3;
			for (int i = 0; i < axes.Length; ++i) {
				r1 = this.getProjectionRadius(axes[i]);
				r2 = obb.getProjectionRadius(axes[i]);
				r3 = Math.Abs(centerdis.Dot(axes[i]));
				if (r1 + r2 <= r3) {
					//Debug.LogFormat("R1_{0:F}, R2_{1:F}, R:{2:F}, R:{3:F}", r1.ToFloat(), r2.ToFloat(), (r1 + r2).ToFloat(), r3.ToFloat());
					return false;
				}
			}

			return true;
		}
	}
}



