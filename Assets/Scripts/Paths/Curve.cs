using System;
using UnityEngine;

namespace Paths
{
	[Serializable]
	public struct Curve
	{
		public Vector2 startPosition;
		public Vector2 startTangent;
		public Vector2 endPosition;
		public Vector2 endTangent;
	}
}