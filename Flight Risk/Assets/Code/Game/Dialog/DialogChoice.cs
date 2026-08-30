using UnityEngine;

namespace FlightRisk.Game.Dialogs
{
    [System.Serializable]
    public class DialogChoice
    {
        public string Text => text;
        public DialogNode NextNode => nextNode;

        [SerializeField] private string text;
        [SerializeField] private DialogNode nextNode;
    }
}