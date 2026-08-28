using UnityEngine;
using TMPro;

namespace FlightRisk.Game.UI
{
    public class InteractUI : MonoBehaviour
    {
        [SerializeField] private GameObject promptGO;
        [SerializeField] private TextMeshProUGUI actionText;

        private void Awake()
        {
            promptGO.SetActive(false);

            GameEvents.TrySubscribe((uint)GameEvents.Interactions.Enter, OnInteractEnter);
            GameEvents.TrySubscribe((uint)GameEvents.Interactions.Exit, OnInteractExit);
        }

        private void OnInteractEnter(object input)
        {
            actionText.SetText((string)input);
            promptGO.SetActive(true);
        }

        private void OnInteractExit(object input)
        {
            promptGO.SetActive(false);
        }
    }
}