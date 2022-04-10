using System;
using Enemies;
using UnityEngine;

namespace Debugging
{
	[Obsolete]
	public class FlyingActorDebugs : MonoBehaviour
	{
		[SerializeField]
		private FlyingActorModelImpl model;

		private void Awake()
		{
			//This class is only meant to work in editor.
#if UNITY_EDITOR
			return;
#endif
			Destroy(this);
		}

		private void OnDrawGizmos()
		{
			if (!model)
				return;
			Gizmos.color = new Color(.0f, .75f, .25f, .15f);
			Gizmos.DrawSphere(transform.position, model.MeleeDistance);
		}
	}
}