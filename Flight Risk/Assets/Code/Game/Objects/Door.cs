using UnityEngine;
using DG.Tweening;

namespace FlightRisk.Game.Objects
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform door;
        [SerializeField] private Vector3 doorOpenOffset;
        [SerializeField] private float doorAnimationTime;

        private Vector3 doorOriginalPosition;

        private void Awake()
        {
            doorOriginalPosition = door.transform.localPosition;
        }

        public void OpenDoor()
        {
            door.DOLocalMove(doorOriginalPosition + doorOpenOffset, doorAnimationTime);
        }

        public void CloseDoor()
        {
            door.DOLocalMove(doorOriginalPosition, doorAnimationTime);
        }
    }
}