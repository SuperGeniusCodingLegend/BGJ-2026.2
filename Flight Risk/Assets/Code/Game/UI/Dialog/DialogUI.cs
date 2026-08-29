using System.Text;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FlightRisk.Game.Dialogs;

namespace FlightRisk.Game.UI
{
    public class DialogUI : MonoBehaviour
    {
        [SerializeField] private GameObject panelGO;
        [SerializeField] private TextMeshProUGUI speakerText;
        [SerializeField] private TextMeshProUGUI bodyText;
        [SerializeField] private List<TextMeshProUGUI> choiceRows;
        [SerializeField] private Color normalChoiceColor = Color.white;
        [SerializeField] private Color highlightedChoiceColor = Color.yellow;
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            panelGO.SetActive(false);

            GameEvents.TrySubscribe((uint)GameEvents.Dialog.Start, OnDialogStart);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.NodeShown, OnNodeShown);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.ChoiceHighlighted, OnChoiceHighlighted);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.End, OnDialogEnd);
        }

        private void OnDialogStart(object payload) => panelGO.SetActive(true);

        private void OnDialogEnd(object payload) => panelGO.SetActive(false);

        private void OnNodeShown(object payload)
        {
            DialogNode node = (DialogNode)payload;
            RenderLine(node);
            PlayLineAudio(node);
            RenderChoices(node);
        }

        private void SetHighlightedChoice(int index)
        {
            for (int i = 0; i < choiceRows.Count; i++)
            {
                if (!choiceRows[i].gameObject.activeSelf) continue;
                choiceRows[i].color = i == index ? highlightedChoiceColor : normalChoiceColor;
            }
        }

        private void RenderLine(DialogNode node)
        {
            if (node.Line == null)
            {
                speakerText.SetText(string.Empty);
                bodyText.SetText(string.Empty);
                return;
            }

            speakerText.SetText(node.Line.SpeakerName);
            bodyText.SetText(node.Line.Text);
        }

        private void PlayLineAudio(DialogNode node)
        {
            if (audioSource == null)
            {
                return;
            }

            audioSource.Stop();

            if (node.Line.LineAudio != null)
            {
                audioSource.clip = node.Line.LineAudio;
                audioSource.Play();
            }
        }

        private void RenderChoices(DialogNode node)
        {
            int choiceCount = node.Choices?.Count ?? 0;

            for (int i = 0; i < choiceRows.Count; ++i)
            {
                bool active = i < choiceCount;
                choiceRows[i].gameObject.SetActive(active);
                if (active)
                {
                    choiceRows[i].SetText(node.Choices[i].Text);
                }
            }

            SetHighlightedChoice(0);
        }

        private void OnChoiceHighlighted(object payload)
        {
            int index = (int)payload;
            SetHighlightedChoice(index);
        }
    }
}