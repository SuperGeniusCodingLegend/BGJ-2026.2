using UnityEngine;

namespace FlightRisk.Game.NPCs
{
    public abstract class NPC : MonoBehaviour
    {
        public enum Mood { Regular, Happy, Sad, Angry, Scared, Frustrated }

        [SerializeField] protected SpriteRenderer faceRenderer;
        [SerializeField] protected Transform dialogFocusPoint;
    }
}
