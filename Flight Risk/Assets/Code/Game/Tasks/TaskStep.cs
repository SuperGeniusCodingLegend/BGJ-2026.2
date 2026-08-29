using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FlightRisk.Game.Tasks
{
    public abstract class TaskStep : MonoBehaviour
    {
        public enum State { Active, Complete, Failed }

        public State CurrentState { get; protected set; } = State.Active;

        [SerializeField] protected float timeToComplete;
        [SerializeField] protected UnityEvent onStepEnter;
        [SerializeField] protected UnityEvent onStepComplete;
        [SerializeField] protected UnityEvent onStepFail;

        private float timer;

        protected abstract bool CheckStepComplete();

        public virtual State StepTick()
        {
            timer += Time.deltaTime;
            if (timer > timeToComplete) return State.Failed;
            return CheckStepComplete() ? State.Complete : State.Active;
        }

        public virtual void EnterStep()
        {
            onStepEnter?.Invoke();
        }

        public virtual void CompleteStep()
        {
            onStepComplete?.Invoke();
        }

        public virtual void FailStep()
        {
            onStepFail?.Invoke();
        }
    }
}