using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FlightRisk.Game.NPCs;

namespace FlightRisk.Game
{
    public class PassengerManager : MonoBehaviour , IServiceProvider<PassengerManager>
    {
        private const int MIN_PASSENGER_SPAWN_POINTS = 1;
        private const int MIN_PASSENGER_PREFABS = 1;

        private const int MIN_REQUIRED_OCCUPANCY_PERCENTAGE = 0;
        private const int MAX_REQUIRED_OCCUPANCY_PERCENTAGE = 100;

        private const int MIN_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE = 0;
        private const int MAX_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE = 100;

        private Passenger RandomPassengerPrefab => passengerPrefabs[Random.Range(0, passengerPrefabs.Count)];

        [SerializeField] private Transform passengerPool;
        [SerializeField] private List<Transform> passengerSpawnPoints;
        [SerializeField] private List<Passenger> passengerPrefabs;

        [SerializeField, Range(MIN_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE, MAX_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE)] 
        private int optionalSpawnPointOccupancyChanceThreshold = 50;
        [SerializeField, Range(MIN_REQUIRED_OCCUPANCY_PERCENTAGE, MAX_REQUIRED_OCCUPANCY_PERCENTAGE)] 
        private int requiredOccuppancySpawnPointPercentage = 50;

        private readonly HashSet<Passenger> freePassengers = new();
        private readonly HashSet<Passenger> occupiedPassengers = new();

        private void Awake()
        {
            if (passengerSpawnPoints.Count < MIN_PASSENGER_SPAWN_POINTS)
            {
                Debug.LogError($"PassengerManager: passengerSpawnPoints.Count ({passengerSpawnPoints.Count}) must be at least {MIN_PASSENGER_SPAWN_POINTS}");
            }

            if (passengerPrefabs.Count < MIN_PASSENGER_PREFABS)
            {
                Debug.LogError($"PassengerManager: passengerPrefabs.Count ({passengerPrefabs.Count}) must be at least {MIN_PASSENGER_PREFABS}");
            }

            SpawnPassengers();
            this.InjectService(this);
        }

        private void SpawnPassengers()
        {
            List<int> spawnPointIndicesShuffled = Enumerable.Range(0, passengerSpawnPoints.Count).OrderBy(x => Random.value).ToList();

            int spawnPointIndicesShuffledIndex = 0;
            int requiredPassengerCount = Mathf.CeilToInt(passengerSpawnPoints.Count * (requiredOccuppancySpawnPointPercentage / 100.0f));

            for (int i = 0; i < requiredPassengerCount; i++)
            {
                int spawnPointIndex = spawnPointIndicesShuffled[spawnPointIndicesShuffledIndex++];
                SpawnFreePassengerInPositionIndex(spawnPointIndex);
            }

            for (int i = requiredPassengerCount; i < passengerSpawnPoints.Count; i++)
            {
                int spawnPointIndex = spawnPointIndicesShuffled[spawnPointIndicesShuffledIndex++];

                if (Random.Range(0, 100) <= optionalSpawnPointOccupancyChanceThreshold)
                {
                    SpawnFreePassengerInPositionIndex(spawnPointIndex);
                }
            }
        }

        private void SpawnFreePassengerInPositionIndex(int spawnPointIndex)
        {
            freePassengers.Add(SpawnPassenger(RandomPassengerPrefab, passengerSpawnPoints[spawnPointIndex]));
        }

        private Passenger SpawnPassenger(Passenger prefab, Transform spawnPoint)
        {
            var newPassenger = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, passengerPool);
            return newPassenger;
        }

        public bool TryGetFreePassenger(out Passenger passenger)
        {
            passenger = null;

            if (freePassengers.Count == 0)
            {
                Debug.LogError("No free passengers left!");
                return false;
            }

            passenger = freePassengers.OrderBy(p => Random.value).FirstOrDefault(); // TODO: Find a better alternative for this.

            occupiedPassengers.Add(passenger);
            freePassengers.Remove(passenger);

            return true;
        }

        public void FreeUpPassenger(Passenger passenger)
        {
            if (!occupiedPassengers.Contains(passenger))
            {
                Debug.LogError($"Passenger {passenger.gameObject.name} is not a part of the occupied passengers set!");
                return;
            }

            freePassengers.Add(passenger);
            occupiedPassengers.Remove(passenger);
        }
    }
}

