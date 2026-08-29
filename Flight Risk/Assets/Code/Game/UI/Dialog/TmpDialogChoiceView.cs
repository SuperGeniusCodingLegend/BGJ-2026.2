using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FlightRisk.Game.UI
{
    [RequireComponent(typeof(Button))]
    public class TmpDialogChoiceView : MonoBehaviour, IDialogChoiceView
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color normalColor = Color.black;
        [SerializeField] private Color highlightedColor = Color.blue;

        public event Action Clicked;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => Clicked?.Invoke());
        }

        public void SetText(string value) => text.SetText(value);

        public void SetHighlighted(bool highlighted) => text.color = highlighted ? highlightedColor : normalColor;
    }
}