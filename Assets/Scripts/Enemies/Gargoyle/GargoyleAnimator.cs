using UnityEngine;

namespace Enemies.Gargoyle
{
	[RequireComponent(typeof(Animator))]
	public class GargoyleAnimator : MonoBehaviour
	{
		private const string ForceFlyingKey = "IsFlying";
		private static readonly int ForceFlying = Animator.StringToHash(ForceFlyingKey);

		[SerializeField]
		private Animator animator;

		private void OnValidate()
		{
			if (!animator)
				animator = GetComponent<Animator>();
		}

		public void SetForceFlying(bool value)
		{
			animator.SetBool(ForceFlying, value);
		}
	}
}