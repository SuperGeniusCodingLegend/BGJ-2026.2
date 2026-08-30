using TMPro;
using UnityEngine;

namespace FlightRisk.Game.UI
{
    public class TmpDialogTextView : MonoBehaviour, IDialogTextView
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetText(string value) => text.SetText(value);
    }
}