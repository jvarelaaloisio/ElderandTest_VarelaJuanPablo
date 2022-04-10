using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Paths.Editor
{
	[CustomEditor(typeof(PathTraveler), false)]
	public class PathTravelerInspector : UnityEditor.Editor
	{
		private PathTraveler _traveler;

		protected virtual void OnEnable()
		{
			_traveler = (PathTraveler) target;
		}

		private void OnSceneGUI()
		{
			for (var i = 0; i < _traveler.PathAir.Length; i++)
			{
				Color color = new Color(0, (float)(i + 1) / _traveler.PathAir.Length, 1);
				Curve curve = _traveler.PathAir[i];
				Handles.DrawBezier(curve.startPosition,
									curve.endPosition,
									curve.startTangent,
									curve.endTangent,
									color,
									Texture2D.whiteTexture,
									3);
			}
			for (var i = 0; i < _traveler.PathGround.Length; i++)
			{
				Color color = new Color(.5f, .25f * (i + 1) / _traveler.PathGround.Length, 0);
				Curve curve = _traveler.PathGround[i];
				Handles.DrawBezier(curve.startPosition,
									curve.endPosition,
									curve.startTangent,
									curve.endTangent,
									color,
									Texture2D.whiteTexture,
									3);
			}
			for (var i = 0; i < _traveler.PathDive.Length; i++)
			{
				Color color = new Color(1, 0, .25f *(i + 1) / _traveler.PathDive.Length);
				Curve curve = _traveler.PathDive[i];
				Handles.DrawBezier(curve.startPosition,
									curve.endPosition,
									curve.startTangent,
									curve.endTangent,
									color,
									Texture2D.whiteTexture,
									3);
			}
		}
	}
}