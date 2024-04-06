using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	[AddComponentMenu("Unturned/Timer Event Hook")]
	public class TimerEventHook : MonoBehaviour
	{
		/// <summary>
		/// Invoked when timer expires.
		/// </summary>
		public UnityEvent OnTimerTriggered;

		/// <summary>
		/// Number of seconds to use when SetDefaultTimer is invoked.
		/// </summary>
		public float DefaultDuration;

		/// <summary>
		/// Should timer loop when SetDefaultTimer is invoked?
		/// </summary>
		public bool DefaultLooping;

		public void SetTimer(float duration)
		{
			SetTimer(duration, false);
		}

		public void SetTimer(float duration, bool looping)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
			}

			shouldTimerLoop = looping;
			coroutine = InternalStartTimer(duration);
			StartCoroutine(coroutine);
		}

		public void SetDefaultTimer()
		{
			SetTimer(DefaultDuration, DefaultLooping);
		}

		/// <summary>
		/// Stop pending timer from triggering.
		/// </summary>
		public void CancelTimer()
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
			shouldTimerLoop = false;
		}

		private IEnumerator InternalStartTimer(float duration)
		{
			yield return new WaitForSeconds(duration);
			coroutine = null; // Clear beforehand in case listener sets a new timer.
			OnTimerTriggered.Invoke();

			// Listener can cancel looping during trigger event.
			if (shouldTimerLoop && coroutine == null)
			{
				coroutine = InternalStartTimer(duration);
				StartCoroutine(coroutine);
			}
		}

		/// <summary>
		/// Handle to stop the coroutine.
		/// </summary>
		private IEnumerator coroutine;
		private bool shouldTimerLoop;
	}
}
