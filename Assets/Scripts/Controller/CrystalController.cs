using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Controller
{
    public class CrystalController : NetworkBehaviour
    {
        [SerializeField] int countCrystal = 3;
        [SerializeField] GameObject prefabCrystal;
        //[SerializeField] GameObject prefabCrystalCounter;
        //private CrystalCounter crystalCounter;
        private int crystalCounter;
        SpawnPointsController spawnPointController;
        public event Action AllCollected;
        public event Action<GameObject> CrystalAdded;
        public NetworkHash128 AssetId => assetId;
        private NetworkHash128 assetId;


        //private List<SimpleCrystal> crystals;
        void Start()
        {
            assetId = prefabCrystal.GetComponent<NetworkIdentity>().assetId;
            ClientScene.RegisterSpawnHandler(assetId, Spawn, UnSpawn);
            //crystalCounter = Singleton<CrystalCounter>.Instance;
        }


        public override void OnStartServer()
        {
            spawnPointController = Singleton<SpawnPointsController>.Instance;
            //var cCounter = Instantiate(prefabCrystalCounter, spawnPointController.GetRandomPlayerSpawnPoint());
            //if (NetworkServer.active)
            //    NetworkServer.Spawn(cCounter);
            //crystalCounter = cCounter.GetComponent<CrystalCounter>();
            crystalCounter = countCrystal;

            assetId = prefabCrystal.GetComponent<NetworkIdentity>().assetId;
            for (int i = 0; i < countCrystal; i++)
            {
                var newTransform = spawnPointController.GetRandomCrystalPoint();
                var crystal = Spawn(newTransform.position);
                if (NetworkServer.active)
                    NetworkServer.Spawn(crystal, AssetId);
            }
        }

        internal void DecreaseCount()
        {
            --crystalCounter;//.Count;
            if (crystalCounter == 0)
            {
                AllCollected?.Invoke();
            }
        }

        public GameObject Spawn(Vector3 position)
        {
            return Instantiate(prefabCrystal, position, Quaternion.identity);
        }

        public GameObject Spawn(Vector3 position, NetworkHash128 assetId)
        {
            return Spawn(position);
        }

        public void UnSpawn(GameObject spawned)
        {
            Destroy(spawned);
        }

    }
}
