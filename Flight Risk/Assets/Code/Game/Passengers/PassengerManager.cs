using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FlightRisk.Game.Passengers
{
    public class PassengerManager : MonoBehaviour
    {

        private const int MIN_PASSENGER_SPAWN_POINTS = 1;
        private const int MIN_PASSENGER_PREFABS = 1;
        private const int MIN_REQUIRED_OCCUPANCY_PERCENTAGE = 0;
        private const int MAX_REQUIRED_OCCUPANCY_PERCENTAGE = 100;
        private const int MIN_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE = 0;
        private const int MAX_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE = 100;
        [SerializeField] private List<Transform> passengerSpawnPoints;
        [SerializeField] private List<GameObject> passengerPrefabs;
        [SerializeField] private int optionalSpawnPointOccupancyChanceThreshold = 50;
        [SerializeField] private int requiredOccuppancySpawnPointPercentage = 50;

        private List<GameObject> passengers = new List<GameObject>();

        private void spawnPassenger(int spawnPointIndex)
        {
            GameObject passengerPrefab = passengerPrefabs[Random.Range(0, passengerPrefabs.Count)];
            Transform spawnPoint = passengerSpawnPoints[spawnPointIndex];
            GameObject passengerInstance = Instantiate(passengerPrefab, spawnPoint.position, spawnPoint.rotation);
            passengers.Add(passengerInstance);
        }

        private void spawnPassengers()
        {
            List<int> spawnPointIndicesShuffled = System.Linq.Enumerable.Range(0, passengerSpawnPoints.Count).OrderBy(x => Random.value).ToList();
            int spawnPointIndicesShuffledIndex = 0;
            int requiredPassengerCount = Mathf.CeilToInt(passengerSpawnPoints.Count * (requiredOccuppancySpawnPointPercentage / 100.0f));

            for (int i = 0; i < requiredPassengerCount; i++)
            {
                int spawnPointIndex = spawnPointIndicesShuffled[spawnPointIndicesShuffledIndex++];
                spawnPassenger(spawnPointIndex);
            }

            for (int i = requiredPassengerCount; i < passengerSpawnPoints.Count; i++)
            {
                int spawnPointIndex = spawnPointIndicesShuffled[spawnPointIndicesShuffledIndex++];
                int randomChance = Random.Range(0, 100);
                if (randomChance < optionalSpawnPointOccupancyChanceThreshold)
                {
                    spawnPassenger(spawnPointIndex);
                }
            }
        }

        void Awake()
        {
            if (requiredOccuppancySpawnPointPercentage < MIN_REQUIRED_OCCUPANCY_PERCENTAGE || requiredOccuppancySpawnPointPercentage > MAX_REQUIRED_OCCUPANCY_PERCENTAGE)
            {
                Debug.LogError($"PassengerManager: requiredOccuppancySpawnPointPercentage ({requiredOccuppancySpawnPointPercentage}) must be between {MIN_REQUIRED_OCCUPANCY_PERCENTAGE} and {MAX_REQUIRED_OCCUPANCY_PERCENTAGE}");
                requiredOccuppancySpawnPointPercentage = Mathf.Clamp(requiredOccuppancySpawnPointPercentage, MIN_REQUIRED_OCCUPANCY_PERCENTAGE, MAX_REQUIRED_OCCUPANCY_PERCENTAGE);
            }

            if (optionalSpawnPointOccupancyChanceThreshold < MIN_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE || optionalSpawnPointOccupancyChanceThreshold > MAX_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE)
            {
                Debug.LogError($"PassengerManager: optionalSpawnPointOccupancyChanceThreshold ({optionalSpawnPointOccupancyChanceThreshold}) must be between {MIN_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE} and {MAX_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE}");
                optionalSpawnPointOccupancyChanceThreshold = Mathf.Clamp(optionalSpawnPointOccupancyChanceThreshold, MIN_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE, MAX_OPTIONAL_OCCUPANCY_CHANCE_THRESHOLD_PERCENTAGE);
            }

            if (passengerSpawnPoints.Count < MIN_PASSENGER_SPAWN_POINTS)
            {
                Debug.LogError($"PassengerManager: passengerSpawnPoints.Count ({passengerSpawnPoints.Count}) must be at least {MIN_PASSENGER_SPAWN_POINTS}");
                throw new System.Exception($"PassengerManager: passengerSpawnPoints.Count ({passengerSpawnPoints.Count}) must be at least {MIN_PASSENGER_SPAWN_POINTS}");
            }

            if (passengerPrefabs.Count < MIN_PASSENGER_PREFABS)
            {
                Debug.LogError($"PassengerManager: passengerPrefabs.Count ({passengerPrefabs.Count}) must be at least {MIN_PASSENGER_PREFABS}");
                throw new System.Exception($"PassengerManager: passengerPrefabs.Count ({passengerPrefabs.Count}) must be at least {MIN_PASSENGER_PREFABS}");
            }

            spawnPassengers();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

