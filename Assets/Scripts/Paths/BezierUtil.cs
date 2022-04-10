using UnityEngine;

namespace Paths
{
	public static class BezierUtil
	{
		public static Vector3 GetPosition(float t, ref Curve curve)
		{
			var p0Weight = (-Mathf.Pow(t, 3) + 3 * Mathf.Pow(t, 2) - 3 * t + 1);
			var p1Weight = (3 * Mathf.Pow(t, 3) - 6 * Mathf.Pow(t, 2) + 3 * t);
			var p2Weight = (-3 * Mathf.Pow(t, 3) + 3 * Mathf.Pow(t, 2));
			var p3Weight = (Mathf.Pow(t, 3));
			var finalPosition = curve.startPosition * p0Weight
								+ curve.startTangent * p1Weight
								+ curve.endPosition * p2Weight
								+ curve.endTangent * p3Weight;
			return finalPosition;
		}
	}
}