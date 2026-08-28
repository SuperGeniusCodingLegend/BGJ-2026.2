using System;
using System.Collections;
using UnityEngine;

namespace FlightRisk
{
    /// <summary>
    /// "WHAT IS MY PURPOSE?"
    /// "you run coroutines"
    /// "OH MY GOD"
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        public static void Run(IEnumerator coroutine) => instance.StartCoroutine(coroutine);

        private static CoroutineRunner instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (instance == this) Debug.LogError("[CoroutineRunner] CoroutineRunner instance was destroyed!\nNO NO NO NO THIS IS NOT HOW IT'S SUPPOSED TO GO");
        }
    }
}