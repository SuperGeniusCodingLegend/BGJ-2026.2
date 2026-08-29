using UnityEngine;
using UnityEngine.Events;

namespace FlightRisk.Game.Tasks
{
    public abstract class TaskStep : MonoBehaviour
    {
        public enum State { Active, Complete, Failed }

        public State CurrentState { get; protected set; } = State.Active;

        [SerializeField] protected float timeToComplete = 60;
        [SerializeField] protected UnityEvent onStepEnter;
        [SerializeField] protected UnityEvent onStepComplete;
        [SerializeField] protected UnityEvent onStepFail;

        private float timer;
        private bool started;

        protected abstract bool CheckStepComplete();

        public virtual State StepTick()
        {
            if (!started) 
            {
                EnterStep();
                started = true;
            }

            timer += Time.deltaTime;

            if (timer > timeToComplete) 
            {
                FailStep();
                return State.Failed;
            }
            
            if (CheckStepComplete())
            {
                CompleteStep();
                return State.Complete;
            }

            return State.Active;
        }

        protected virtual void EnterStep()
        {
            Debug.Log($"{gameObject.name} was entered. Tick tock, time is ticking.");
            onStepEnter?.Invoke();
        }

        protected virtual void CompleteStep()
        {
            Debug.Log($"{gameObject.name} was complete. You want a medal?");
            onStepComplete?.Invoke();
        }

        protected virtual void FailStep()
        {
            Debug.Log($"{gameObject.name} has failed. YOU FUCKING IDIOT.");
            onStepFail?.Invoke();
        }
    }
}