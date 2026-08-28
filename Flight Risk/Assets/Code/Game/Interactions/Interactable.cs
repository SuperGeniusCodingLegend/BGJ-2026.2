using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlightRisk.Game.Interactions
{
    public class Interactable : MonoBehaviour
    {
        public string ActionPrompt => actionPrompt;

        [SerializeField] protected string actionPrompt;
        [SerializeField] protected UnityEvent onEnterInteractRaycast;
        [SerializeField] protected UnityEvent onExitInteractRaycast;
        [SerializeField] protected UnityEvent onInteract;

        public virtual void EnterInteractRaycast()
        {
            onEnterInteractRaycast?.Invoke();
        }

        public virtual void ExitInteractRaycast()
        {
            onExitInteractRaycast?.Invoke();
        }

        public virtual void Interact()
        {
            onInteract?.Invoke();
        }
    }
}
