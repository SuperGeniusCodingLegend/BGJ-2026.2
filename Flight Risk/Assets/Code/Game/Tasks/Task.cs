using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FlightRisk.Game.Tasks
{
    public class Task : MonoBehaviour
    {
        public enum State { Active, Complete, Failed }

        public float SatisfactionGainOnComplete => satisfactionGainOnComplete;
        public float SatisfactionLossOnFail => satisfactionLossOnFail;

        [SerializeField] protected List<TaskStep> steps;
        [SerializeField] protected float satisfactionGainOnComplete;
        [SerializeField] protected float satisfactionLossOnFail;
        [SerializeField] protected UnityEvent onTaskStart;
        [SerializeField] protected UnityEvent onTaskComplete;
        [SerializeField] protected UnityEvent onTaskFail;

        private TaskStep currentStep;
        private int currentStepIndex = -1;

        public virtual State TaskTick()
        {
            if (currentStepIndex == -1)
            {
                AdvanceToNextStep();
                StartTask();
            }

            var state = currentStep.StepTick();
            if (state == TaskStep.State.Active) return State.Active;

            if (state == TaskStep.State.Failed)
            {
                FailTask();
                return State.Failed;
            }

            if (currentStepIndex == steps.Count)
            {
                CompleteTask();
                return State.Complete;
            }
            else
            {
                AdvanceToNextStep();
                return State.Active;
            }
        }

        private void AdvanceToNextStep()
        {
            currentStepIndex++;
            currentStep = steps[currentStepIndex];
        }

        protected virtual void StartTask()
        {
            Debug.Log($"{gameObject.name} was entered. Do it or you're fired.");
            onTaskStart?.Invoke();
        }

        protected virtual void CompleteTask()
        {
            Debug.Log($"{gameObject.name} was complete. Not very impressed.");
            onTaskComplete?.Invoke();
        }

        protected virtual void FailTask()
        {
            Debug.Log($"{gameObject.name} has failed. HOW FUCKING DARE YOU.");
            onTaskFail?.Invoke();
        }
    }
}
