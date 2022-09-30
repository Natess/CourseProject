
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Controller
{
    public class SpawnPointsController : MonoBehaviour
    {
        public SpawnPoint[] playerSpawnPoints;
        public SpawnPoint[] crystalSpawnPoints;

        private void Start()
        {
            var spawnPoints = Resources.FindObjectsOfTypeAll(typeof(SpawnPoint)) as SpawnPoint[];
            if (spawnPoints == null || spawnPoints.Length == 0)
                throw new System.Exception("Spawn points not found");

            playerSpawnPoints = spawnPoints.Length > 5 ? spawnPoints.Take(5).ToArray()
                : spawnPoints.Take(spawnPoints.Length / 2).ToArray();
            crystalSpawnPoints = spawnPoints.Length > 5 ? spawnPoints.Skip(5).ToArray()
                : spawnPoints.Skip(spawnPoints.Length / 2).ToArray();
        }

        public Transform GetRandomPlayerSpawnPoint()
        {
            return playerSpawnPoints[Random.Range(0, playerSpawnPoints.Length)].transform;
        }

        public Transform GetRandomCrystalPoint()
        {
            return crystalSpawnPoints[Random.Range(0, crystalSpawnPoints.Length)].transform;
        }
    }
}
