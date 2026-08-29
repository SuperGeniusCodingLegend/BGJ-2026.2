using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FlightRisk.Game.Passengers;

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

        [SerializeField] private Transform passengerPool;
        [SerializeField] private List<Transform> passengerSpawnPoints;
        [SerializeField] private List<PassengerEntity> passengerPrefabs;

        [SerializeField, Range(MIN_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE, MAX_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE)] 
        private int optionalSpawnPointOccupancyChanceThreshold = 50;
        [SerializeField, Range(MIN_REQUIRED_OCCUPANCY_PERCENTAGE, MAX_REQUIRED_OCCUPANCY_PERCENTAGE)] 
        private int requiredOccuppancySpawnPointPercentage = 50;

        private readonly List<PassengerEntity> freePassengers = new();
        private readonly Dictionary<PassengerEntity, int> occupiedPassengers = new();

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
                SpawnPassenger(spawnPointIndex);
            }

            for (int i = requiredPassengerCount; i < passengerSpawnPoints.Count; i++)
            {
                int spawnPointIndex = spawnPointIndicesShuffled[spawnPointIndicesShuffledIndex++];

                if (Random.Range(0, 100) <= optionalSpawnPointOccupancyChanceThreshold)
                {
                    SpawnPassenger(spawnPointIndex);
                }
            }
        }

        private void SpawnPassenger(int spawnPointIndex)
        {
            PassengerEntity passenger = 
                Instantiate(
                    passengerPrefabs[Random.Range(0, passengerPrefabs.Count)],
                    passengerSpawnPoints[spawnPointIndex].position,
                    passengerSpawnPoints[spawnPointIndex].rotation,
                    passengerPool);

            freePassengers.Add(passenger);
        }

        public PassengerEntity GetFreePassenger()
        {
            int passengerIndex = 0;
            PassengerEntity passenger = null;

            while (passenger == null)
            {
                passengerIndex = Random.Range(0, passengerPrefabs.Count);
                passenger = freePassengers[passengerIndex];
            }

            occupiedPassengers.Add(passenger, passengerIndex);
            freePassengers[passengerIndex] = null;

            return passenger;
        }

        public void FreeUpPassenger(PassengerEntity passenger)
        {
            if (!occupiedPassengers.TryGetValue(passenger, out int passengerIndex))
            {
                Debug.LogError($"Passenger {passenger.gameObject.name} is not a part of the occupied passengers dictionary!");
                return;
            }

            freePassengers[passengerIndex] = passenger;
            occupiedPassengers.Remove(passenger);
        }
    }
}

