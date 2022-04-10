using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

namespace Paths
{
	public class PathTraveler : MonoBehaviour
	{
		[Header("Setup")]
		[SerializeField]
		private Curve[] pathAir;

		[SerializeField]
		private Curve[] pathGround;

		[SerializeField]
		private Curve[] pathDive;

		[SerializeField]
		private AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1);

		[Header("Events")]
		[SerializeField]
		private UnityEvent onTravel;

		[SerializeField]
		private UnityEvent onArrived;

		[SerializeField]
		private UnityEvent onLanding;

		[SerializeField]
		private UnityEvent onLanded;

		[SerializeField]
		private UnityEvent onTakingOff;

		[SerializeField]
		private UnityEvent onTookOff;

		[SerializeField]
		private UnityEvent onDiving;

		[SerializeField]
		private UnityEvent onDove;

		private Curve[] _currentPath;

		private int _currentIndex = 0;

		public bool IsGoingRight { get; private set; }

		public Curve[] PathAir
		{
			get => pathAir;
			set => pathAir = value;
		}

		public Curve[] PathGround
		{
			get => pathGround;
			set => pathGround = value;
		}
		public Curve[] PathDive
		{
			get => pathDive;
			set => pathDive = value;
		}

		private void Awake()
		{
			if (pathAir.Length != pathGround.Length)
				Debug.LogError("Both paths need to have the same length", this);
			if (pathAir.Length < 1)
				Debug.LogError("A path should have at least 1 curve, better if it has 2 or more.");
			_currentPath = pathAir;
		}

		private void Start()
		{
			transform.position = _currentPath[_currentIndex].startPosition;
			IsGoingRight = true;
		}

		/// <summary>
		/// Lands the traveler
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public IEnumerator Land(float duration)
		{
			onLanding.Invoke();
			yield return Lerp(duration, lerp =>
										{
											transform.position = Vector3.Lerp(_currentPath[_currentIndex].startPosition,
																			pathGround[_currentIndex].startPosition,
																			speedCurve.Evaluate(lerp));
										});
			_currentPath = pathGround;
			onLanded.Invoke();
		}

		/// <summary>
		/// Puts the traveler in the air
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public IEnumerator Fly(float duration)
		{
			onTakingOff.Invoke();
			yield return Lerp(duration, lerp =>
										{
											transform.position = Vector3.Lerp(_currentPath[_currentIndex].startPosition,
																			pathAir[_currentIndex].startPosition,
																			speedCurve.Evaluate(lerp));
										});
			_currentPath = pathAir;
			onTookOff.Invoke();
		}

		public IEnumerator Dive(float duration)
		{
			onDiving.Invoke();
			_currentPath = pathDive;
			yield return GoToNextPoint(duration);
			_currentPath = pathAir;
			onDove.Invoke();
		}

		/// <summary>
		/// Goes to the next point in the current path
		/// </summary>
		/// <param name="duration"></param>
		/// <returns></returns>
		public IEnumerator GoToNextPoint(float duration)
		{
			onTravel.Invoke();
			yield return MoveThroughCurve(duration);
			if (++_currentIndex > _currentPath.Length - 1)
				_currentIndex = 0;
			onArrived.Invoke();
			if(_currentPath == pathGround)
				onLanded.Invoke();
		}

		private IEnumerator MoveThroughCurve(float duration)
		{
			yield return Lerp(duration, lerp =>
										{
											Curve curve = _currentPath[_currentIndex];
											var nextPosition = BezierUtility.BezierPoint(
											curve.startPosition,
											curve.startTangent,
											curve.endTangent,
											curve.endPosition,
											speedCurve.Evaluate(lerp));
											IsGoingRight = (nextPosition.x > transform.position.x);
											transform.position = nextPosition;
										});
		}

		private static IEnumerator Lerp(float duration, Action<float> behaviour)
		{
			float beginning = Time.time;
			float t;
			var wait = new WaitForSeconds(1.0f / 60);
			while ((t = Time.time - beginning) < duration)
			{
				behaviour(t / duration);
				yield return wait;
			}

			behaviour(1);
		}

#if UNITY_EDITOR

		[ContextMenu("Go To Next Point")]
		private void GoToNextPoint()
		{
			StartCoroutine(GoToNextPoint(1));
		}

		[ContextMenu("Land")]
		private void Land()
		{
			StartCoroutine(Land(1));
		}

		[ContextMenu("Take Off")]
		private void Fly()
		{
			StartCoroutine(Fly(1));
		}

		[ContextMenu("Dive")]
		private void Dive()
		{
			StartCoroutine(Dive(1));
		}
#endif

		private void OnDrawGizmos()
		{
			foreach (Curve curve in pathAir)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(curve.startPosition, curve.startTangent);
				Gizmos.DrawLine(curve.endPosition, curve.endTangent);
				Gizmos.color = new Color(.25f, .5f, .25f);
				Curve temp = curve;
				Gizmos.DrawLine(curve.startPosition, BezierUtil.GetPosition(.5f, ref temp));
				Gizmos.DrawLine(curve.endPosition, BezierUtil.GetPosition(.5f, ref temp));
			}

			foreach (Curve curve in pathGround)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(curve.startPosition, curve.startTangent);
				Gizmos.DrawLine(curve.endPosition, curve.endTangent);
				Gizmos.color = new Color(.0f, .25f, .75f);
				Curve temp = curve;
				Gizmos.DrawLine(curve.startPosition, BezierUtil.GetPosition(.5f, ref temp));
				Gizmos.DrawLine(curve.endPosition, BezierUtil.GetPosition(.5f, ref temp));
			}

			foreach (Curve curve in pathDive)
			{
				Gizmos.color = new Color(.9f, .5f, .0f);
				Gizmos.DrawLine(curve.startPosition, curve.startTangent);
				Gizmos.DrawLine(curve.endPosition, curve.endTangent);
				Gizmos.color = new Color(.7f, .25f, .0f);
				Curve temp = curve;
				Gizmos.DrawLine(curve.startPosition, BezierUtil.GetPosition(.5f, ref temp));
				Gizmos.DrawLine(curve.endPosition, BezierUtil.GetPosition(.5f, ref temp));
			}
		}
	}
}