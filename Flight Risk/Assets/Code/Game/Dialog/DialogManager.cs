using UnityEngine;

namespace FlightRisk.Game.Dialogs
{
    public class DialogManager : MonoBehaviour , IServiceProvider<DialogManager>
    {
        private DialogNode currentNode;
        private int highlightedChoiceIndex = 0;

        private void Awake()
        {
            this.InjectService(this);
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
            Debug.Log($"[DialogManager] Started Dialog with node: {(currentNode ? currentNode.ToString() : "NULL")}.");

            GameEvents.TryInvoke((uint)GameEvents.Dialog.Start);
            GameEvents.TryInvoke((uint)GameEvents.Dialog.NodeShown, currentNode);
        }

        public void SelectChoice(int index)
        {
            if (currentNode == null || currentNode.Choices == null || index < 0 || index >= currentNode.Choices.Count)
            {
                Debug.LogError($"[DialogManager] Issues selecting a choice with current node {(currentNode ? currentNode.ToString() : "NULL")}.");
                return;
            }

            highlightedChoiceIndex = index;
            Confirm();
        }

        public void Confirm()
        {
            if (currentNode == null) 
            {
                Debug.LogError($"[DialogManager] Tried to confirm with a null currentNode.");
                return;
            }

            DialogNode target =  currentNode.Choices != null && currentNode.Choices.Count > 0 ?
                currentNode.Choices[highlightedChoiceIndex].NextNode :
                currentNode.AutoAdvanceNode;

            if (target == null)
            {
                EndDialog();
                return;
            }

            currentNode = target;
            highlightedChoiceIndex = 0;
            Debug.Log($"[DialogManager] Continued Dialog with node: {(currentNode ? currentNode.ToString() : "NULL")}.");
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
            Debug.Log($"[DialogManager] Ended Dialog with node: {(currentNode ? currentNode.ToString() : "NULL")}.");
            GameEvents.TryInvoke((uint)GameEvents.Dialog.End, currentNode);
            currentNode = null;
        }
    }
}