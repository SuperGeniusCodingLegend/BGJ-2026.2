using System.Collections.Generic;
using UnityEngine;
using FlightRisk.Game.Tasks;

namespace FlightRisk.Game
{
    public class TaskManager : MonoBehaviour , IRequireService<PassengerManager>
    {
        public int CurrentRunningTasks;

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
            CurrentRunningTasks = runningTasks.Count;
            RunCurrentTasks();
        }

        private void RunCurrentTasks()
        {
            if (runningTasks.Count == 0) return;

            foreach (var task in runningTasks)
            {
                var state = task.TaskTick();
                if (state == Task.State.Active) continue;

                if (state == Task.State.Complete) // TODO: Wire this through passenger manager? game manager?
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

        private bool TryCreateTask(Task taskPrefab, out Task createdTask)
        {
            createdTask = null;
            if (!passengerManager.TryGetFreePassenger(out var passenger)) return false;
            createdTask = Instantiate(taskPrefab, passenger.TaskParent);
            return true;
        }

        [ContextMenu("Test Create Task")]
        public void TestCreateTask()
        {
            if (!TryCreateTask(regularTasks[0], out var task)) return;
            runningTasks.Add(task);
        }
    }
}