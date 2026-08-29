using UnityEngine;

namespace FlightRisk.Game.Passengers
{
    public class PassengerEntity : MonoBehaviour
    {
        public enum Mood { Happy, Depressed, Angry, Stuckup, Asshole }

        public Transform TaskParent => taskParent;

        [SerializeField] private Mood defaultMood;
        [SerializeField] private Transform taskParent;

        
    }
}
