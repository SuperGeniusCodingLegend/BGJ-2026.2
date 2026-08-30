using UnityEngine;

namespace FlightRisk.Game.Player
{
    public class PlayerEntity : MonoBehaviour , IServiceProvider<PlayerEntity>
    {
        public LookController Looker => looker;
        public MoveController Mover => mover;

        [SerializeField] private LookController looker;
        [SerializeField] private MoveController mover;

        private void Awake()
        {
            this.InjectService(this);
        }
    }
}