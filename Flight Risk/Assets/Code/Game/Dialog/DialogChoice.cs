using UnityEngine;

namespace FlightRisk.Game.Dialogs
{
    [System.Serializable]
    public class DialogChoice
    {
        [SerializeField] private string text;
        [SerializeField] private DialogNode nextNode;

        public string Text => text;
        public DialogNode NextNode => nextNode;

        
    }
}