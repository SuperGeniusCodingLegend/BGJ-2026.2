using UnityEngine;

namespace FlightRisk.Game.Tasks
{
    public class GiveItemStep : TaskStep
    {
        [SerializeField] private Item itemNeeded;

        private bool hasGivenItem;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Item item)) return;
        }

        protected override bool CheckStepComplete() => hasGivenItem;
    }
}