using UnityEngine;
using System;


namespace Lockstep
{
	public static class ExtendCollider
	{
		public static void getOBBB(this BoxCollider col, ref BBBuilder _OBBBuilder)
		{
			var qua = col.transform.eulerAngles;
			Quaternion newQ = Quaternion.Euler(qua.x, 0, qua.z);
			_OBBBuilder.size = col.size;
			_OBBBuilder.size.Scale(col.transform.lossyScale);
			_OBBBuilder.center = col.center;
			_OBBBuilder.center.Scale(col.transform.lossyScale);
			_OBBBuilder.Rotate(newQ);
			_OBBBuilder.degreeY = qua.y;
			_OBBBuilder.center += col.transform.position;
			#if UNITY_EDITOR
			_OBBBuilder.prepareDraw();
			#endif
		}

		public static void getOBBB(this SphereCollider col, ref BBBuilder _OBBBuilder)
		{
			_OBBBuilder.size = new Vector3(col.radius * 2, col.radius * 2, col.radius * 2);
			_OBBBuilder.size.Scale(col.transform.lossyScale);
			_OBBBuilder.center = col.center;
			_OBBBuilder.center.Scale(col.transform.lossyScale);
			_OBBBuilder.center += col.transform.position;
		}

		public static void getOBBB(this CapsuleCollider col, ref BBBuilder _OBBBuilder)
		{
			_OBBBuilder.size = new Vector3(col.radius * 2, col.radius * 2, col.radius * 2);
			_OBBBuilder.size.Scale(col.transform.lossyScale);
			_OBBBuilder.center = col.center;
			_OBBBuilder.center.Scale(col.transform.lossyScale);
			_OBBBuilder.center += col.transform.position;
		}
	}
}



