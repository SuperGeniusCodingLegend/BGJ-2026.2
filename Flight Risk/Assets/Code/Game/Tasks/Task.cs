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
        [SerializeField] protected UnityEvent onTaskAdvance;
        [SerializeField] protected UnityEvent onTaskComplete;
        [SerializeField] protected UnityEvent onTaskFail;

        private TaskStep currentStep;
        private int currentStepIndex;

        public virtual State TaskTick()
        {
            if (currentStep == null)
            {
                currentStep = steps[0];
                onTaskStart?.Invoke();
            }

            var state = currentStep.StepTick();
            if (state == TaskStep.State.Active) return State.Active;

            if (state == TaskStep.State.Failed) 
            {
                onTaskFail?.Invoke();
                return State.Failed;
            }

            if (currentStepIndex == steps.Count) 
            {
                onTaskComplete?.Invoke();
                return State.Complete;
            }
            else
            {
                AdvanceToNextStep();
                onTaskAdvance?.Invoke();
                return State.Active;
            }
        }

        private void AdvanceToNextStep()
        {
            currentStepIndex++;
            currentStep = steps[currentStepIndex];
        }
    }
}
