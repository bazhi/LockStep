using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace Lockstep
{
	public static class LSUtility
	{
		public static void Clear(this Array array)
		{
			System.Array.Clear(array, 0, array.Length);
		}

		public static void SetActiveIfNot(this GameObject go, bool activeState)
		{
			if (go.activeSelf != activeState)
				go.SetActive(activeState);
		}

		public static bool RefEquals(this System.Object obj, object other)
		{
			return System.Object.ReferenceEquals(obj, other);
		}

		public static bool IsNull(this System.Object obj)
		{
			return obj.RefEquals(null);
		}

		public static bool IsNotNull(this System.Object obj)
		{
			return obj.RefEquals(null) == false;
		}

		public static void Shift(this Array array, int min, int max, int shiftAmount)
		{
			if (shiftAmount == 0)
				return;
			Array.Copy(array, min, array, min + shiftAmount, max - min);

		}

		#region BitMask Manipulation

		//ulong mask
		public static void SetBitTrue(ref ulong mask, int bitIndex)
		{
			mask |= (ulong)1 << bitIndex;
		}

		public static void SetBitFalse(ref ulong mask, int bitIndex)
		{
			mask &= ~((ulong)1 << bitIndex);
		}

		public static bool GetBitTrue(this ulong mask, int bitIndex)
		{
			return (mask & ((ulong)1 << bitIndex)) != 0;
		}

		public static bool GetBitFalse(this ulong mask, int bitIndex)
		{
			return (mask & ((ulong)1 << bitIndex)) == 0;
		}

		//uint mask
		public static void SetBitTrue(ref uint mask, int bitIndex)
		{
			mask |= (uint)1 << bitIndex;
		}

		public static void SetBitFalse(ref uint mask, int bitIndex)
		{
			mask &= ~((uint)1 << bitIndex);
		}

		public static bool GetBitTrue(this uint mask, int bitIndex)
		{
			return (mask & ((uint)1 << bitIndex)) != 0;
		}

		public static bool GetBitFalse(this uint mask, int bitIndex)
		{
			return (mask & ((uint)1 << bitIndex)) == 0;
		}

		#endregion
	}
}
namespace Lockstep.Integration
{

}