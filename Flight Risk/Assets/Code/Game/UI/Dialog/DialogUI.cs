using System.Text;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FlightRisk.Game.Dialogs;

namespace FlightRisk.Game.UI
{
    public class DialogUI : MonoBehaviour
    {
        [SerializeField] private DialogManager dialogManager;
        [SerializeField] private GameObject panelGO;
        [SerializeField] private GameObject speakerGO;
        [SerializeField] private GameObject bodyGO;
        [SerializeField] private List<GameObject> choiceRowGOs;
        [SerializeField] private AudioSource audioSource;

        private IDialogTextView speakerView;
        private IDialogTextView bodyView;
        private readonly List<IDialogChoiceView> choiceViews = new();

        private void Awake()
        {
            panelGO.SetActive(false);

            speakerView = speakerGO.GetComponent<IDialogTextView>();
            bodyView = bodyGO.GetComponent<IDialogTextView>();

            for (int i = 0; i < choiceRowGOs.Count; ++i)
            {
                IDialogChoiceView view = choiceRowGOs[i].GetComponent<IDialogChoiceView>();
                choiceViews.Add(view);

                int index = i;

                if (view != null)
                {
                    view.Clicked += () => dialogManager.SelectChoice(index);
                }
            }

            GameEvents.TrySubscribe((uint)GameEvents.Dialog.Start, OnDialogStart);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.NodeShown, OnNodeShown);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.ChoiceHighlighted, OnChoiceHighlighted);
            GameEvents.TrySubscribe((uint)GameEvents.Dialog.End, OnDialogEnd);
        }

        private void OnDialogStart(object payload)
        {
            panelGO.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void OnDialogEnd(object payload)
        {
            panelGO.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnNodeShown(object payload)
        {
            DialogNode node = (DialogNode)payload;
            RenderLine(node);
            PlayLineAudio(node);
            RenderChoices(node);
        }

        private void RenderLine(DialogNode node)
        {
            speakerView?.SetText(node.Line.SpeakerName);
            bodyView?.SetText(node.Line.Text);
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

            for (int i = 0; i < choiceRowGOs.Count; ++i)
            {
                bool active = i < choiceCount;
                choiceRowGOs[i].SetActive(active);
                if (active)
                {
                    choiceViews[i]?.SetText(node.Choices[i].Text);
                }
            }

            SetHighlightedChoice(0);
        }

        private void SetHighlightedChoice(int index)
        {
            for (int i = 0; i < choiceRowGOs.Count; ++i)
            {
                if (!choiceRowGOs[i].activeSelf)
                {
                    continue;
                }

                choiceViews[i]?.SetHighlighted(i == index);
            }
        }

        private void OnChoiceHighlighted(object payload) => SetHighlightedChoice((int)payload);
    }
}