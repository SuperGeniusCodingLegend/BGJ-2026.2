using System;
using System.Collections;
using UnityEngine;

namespace FlightRisk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Uses the coroutine runner to run a coroutine. Pretty self explanatory.
        /// </summary>
        /// <param name="coroutine"></param>
        public static void RunCoroutine(this object obj, IEnumerator coroutine) => CoroutineRunner.Run(coroutine);

        /// <summary>
        /// Uses the coroutine runner to delay an action by an amount of seconds.
        /// </summary>
        /// <param name="delay">The amount of seconds to delay.</param>
        /// <param name="afterDelay">What you want to happen after the delay.</param>
        public static void DelayByFrame(this object obj, float delay, Action afterDelay) => CoroutineRunner.Run(DelayRoutine(delay, afterDelay));

        private static IEnumerator DelayRoutine(float delay, Action afterDelay)
        {
            yield return new WaitForSeconds(delay);
            afterDelay?.Invoke();
        }

        /// <summary>
        /// Uses the coroutine runner to delay an action by a single frame.
        /// </summary>
        /// <param name="afterFrame">What you want to happen after a frame.</param>
        public static void DelayByFrame(this object obj, Action afterFrame) => CoroutineRunner.Run(DelayByFrameRoutine(afterFrame));

        private static IEnumerator DelayByFrameRoutine(Action afterFrame)
        {
            yield return null;
            afterFrame?.Invoke();
        }

        public static void WaitUntill(this object obj, Func<bool> predicate, Action afterWait) => CoroutineRunner.Run(WaitUntillRoutine(predicate, afterWait));

        private static IEnumerator WaitUntillRoutine(Func<bool> predicate, Action afterWait)
        {
            yield return new WaitUntil(predicate);
            afterWait?.Invoke();
        }
    }
}