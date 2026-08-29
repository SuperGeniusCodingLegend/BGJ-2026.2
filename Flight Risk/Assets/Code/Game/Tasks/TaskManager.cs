using System.Collections.Generic;
using UnityEngine;
using FlightRisk.Game.Tasks;

namespace FlightRisk.Game
{
    /// <summary>
    /// haha
    /// </summary>
    public class TaskManager : MonoBehaviour
    {
        [SerializeField] private List<Task> regularTasks; // sprinkled across the game.
        [SerializeField] private List<Task> specialTasks; // at least one per a time threshold of our choosing.

        private void SpawnTask()
        {

        }
    }
}
