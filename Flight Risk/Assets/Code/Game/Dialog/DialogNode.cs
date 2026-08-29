using System.Collections.Generic;
using UnityEngine;

namespace FlightRisk.Game.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog Node", menuName = "FlightRisk/Dialogs/Node")]
    public class DialogNode : ScriptableObject
    {
        [SerializeField] private DialogLine line;
        [SerializeField] private List<DialogChoice> choices;
        [SerializeField] private DialogNode autoAdvanceNode;
        public enum Outcome { None, Success, Failure };
        [SerializeField] private Outcome outcome;

        public DialogLine Line => line;
        public List<DialogChoice> Choices => choices;
        public DialogNode AutoAdvanceNode => autoAdvanceNode;
        public Outcome NodeOutcome => outcome;
    }
}