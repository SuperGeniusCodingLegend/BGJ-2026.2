using System;
using UnityEngine;

namespace FlightRisk.Game.UI
{
    public interface IDialogChoiceView : IDialogTextView
    {
        void SetHighlighted(bool highlighted);
        event Action Clicked;
    }
}