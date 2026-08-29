using UnityEngine;

namespace FlightRisk.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float timeToCompleteInSeconds = 60 * 15;

        private float gameTimer;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (gameTimer < timeToCompleteInSeconds) gameTimer += Time.deltaTime;
            GameStatus.GameCompletePercent = Mathf.Clamp01(gameTimer / timeToCompleteInSeconds);
        }
    }
}
