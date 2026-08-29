using System.Collections.Generic;
using UnityEngine;
using FlightRisk.Game.Tasks;

namespace FlightRisk.Game
{
    /// <summary>
    /// haha
    /// </summary>
    public class TaskManager : MonoBehaviour , IRequireService<PassengerManager>
    {
        [SerializeField] private List<Task> regularTasks; // sprinkled across the game.
        [SerializeField] private List<Task> specialTasks; // at least one per a time threshold of our choosing.

        private PassengerManager passengerManager;

        private HashSet<Task> runningTasks = new();
        private HashSet<Task> tasksEndedThisFrame = new();

        private bool canSpawnTasks;

        private void Awake()
        {
            this.WaitForService(pissman => passengerManager = pissman);
        }

        private void Update()
        {
            RunCurrentTasks();
        }

        private void RunCurrentTasks()
        {
            if (runningTasks.Count == 0) return;

            foreach (var task in runningTasks)
            {
                var state = task.TaskTick();
                if (state == Task.State.Active) continue;

                if (state == Task.State.Complete)
                    GameStatus.PassengerSatisfaction += Mathf.Abs(task.SatisfactionGainOnComplete);
                else 
                    GameStatus.PassengerSatisfaction -= Mathf.Abs(task.SatisfactionLossOnFail);

                tasksEndedThisFrame.Add(task);
            }

            runningTasks.RemoveWhere(task => tasksEndedThisFrame.Contains(task));
            tasksEndedThisFrame.Clear();
        }

        private void SpawnPeriodicTasks()
        {

        }

        private Task CreateActiveTask(Task taskPrefab)
        {
            var passenger = passengerManager.GetFreePassenger();
            var task = Instantiate(taskPrefab, passenger.TaskParent);
            return task;
        }
    }
}
