using System.Collections.Generic;
using UnityEngine;

namespace FlightRisk.Game.Dialogs
{
    [CreateAssetMenu(fileName = "New Dialog Node", menuName = "FlightRisk/Dialogs/Node")]
    public class DialogNode : ScriptableObject
    {
        public enum Outcome { None, Success, Failure };

        public DialogLine Line => line;
        public List<DialogChoice> Choices => choices;
        public DialogNode AutoAdvanceNode => autoAdvanceNode;
        public Outcome NodeOutcome => outcome;

        [SerializeField] private DialogLine line;
        [SerializeField] private List<DialogChoice> choices;
        [SerializeField] private DialogNode autoAdvanceNode;
        [SerializeField] private Outcome outcome;

        public override string ToString()
        {
            var s = "Choice: " + (Line != null ? Line.Text : "NULL_LINE");

            foreach (var choice in Choices)
            {
                s += "\n" + "Choice: " + (choice != null ? choice.Text : "NULL_CHOICE");
            }

            s += "\n" + "Outcome: " + outcome.ToString();

            return s;
        }
    }
}