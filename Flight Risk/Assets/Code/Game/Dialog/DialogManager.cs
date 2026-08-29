using UnityEngine;

namespace FlightRisk.Game.Dialogs
{
    public class DialogManager : MonoBehaviour
    {
        private DialogNode currentNode;
        private int highlightedChoiceIndex = 0;

        private void Awake()
        {
            GameEvents.TrySubscribe((uint)GameEvents.Interactions.OpenDialog, OnOpenDialog);
        }

        private void OnOpenDialog(object payload)
        {
            StartDialog(payload as DialogNode);
        }

        private void StartDialog(DialogNode node)
        {
            currentNode = node;
            highlightedChoiceIndex = 0;
            GameEvents.TryInvoke((uint)GameEvents.Dialog.Start);
            GameEvents.TryInvoke((uint)GameEvents.Dialog.NodeShown, currentNode);
        }

        public void Confirm()
        {
            if (currentNode == null) return;

            DialogNode target = currentNode.Choices != null && currentNode.Choices.Count > 0
                ? currentNode.Choices[highlightedChoiceIndex].NextNode
                : currentNode.AutoAdvanceNode;

            if (target == null)
            {
                EndDialog();
                return;
            }

            currentNode = target;
            highlightedChoiceIndex = 0;
            GameEvents.TryInvoke((uint)GameEvents.Dialog.NodeShown, currentNode);
        }

        public void CycleChoice(int direction)
        {
            if (currentNode == null || currentNode.Choices == null || currentNode.Choices.Count == 0)
            {
                return;
            }

            int count = currentNode.Choices.Count;
            highlightedChoiceIndex = (highlightedChoiceIndex + direction + count) % count;
            GameEvents.TryInvoke((uint)GameEvents.Dialog.ChoiceHighlighted, highlightedChoiceIndex);
        }

        private void EndDialog()
        {
            GameEvents.TryInvoke((uint)GameEvents.Dialog.End, currentNode);
            currentNode = null;
        }
    }
}