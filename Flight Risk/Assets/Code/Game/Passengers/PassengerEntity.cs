using UnityEngine;

namespace FlightRisk.Game.Passengers
{
    public class PassengerEntity : MonoBehaviour
    {
        public enum Mood { Happy, Depressed, Angry, Stuckup, Asshole }

        [SerializeField] private Transform taskParent;
        [SerializeField] private Mood defaultMood;
    }
}
