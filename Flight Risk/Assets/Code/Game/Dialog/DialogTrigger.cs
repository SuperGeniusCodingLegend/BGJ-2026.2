using UnityEngine;

namespace FlightRisk.Game.Dialogs
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private DialogNode openingNode;

        public void TriggerDialog()
        {
            GameEvents.TryInvoke((uint)GameEvents.Interactions.OpenDialog, openingNode);
        }
    }
}