using Controller;
using Main;
using Mechanics;
using Network;
using UI;
using UnityEngine;
using UnityEngine.Networking;

namespace Characters
{
    public class ShipController : NetworkMovableObject
    {
        public string PlayerName
        {
            get => playerName;
            set => playerName = value;
        }

        protected override float speed => shipSpeed;

        [SerializeField] private Transform cameraAttach;
        private CameraOrbit cameraOrbit;
        private PlayerLabel playerLabel;
        private float shipSpeed;
        private Rigidbody rb;

        [SyncVar] private string playerName;
        [SyncVar] public int Point;

        SpawnPointsController spawnPointController;
        CrystalController crystalController;
        private void Awake()
        {
        }

        public override void OnStartServer()
        {
            if (isServer)
            {
                spawnPointController = Singleton<SpawnPointsController>.Instance;
                crystalController = Singleton<CrystalController>.Instance;
            }
        }

        private void OnGUI()
        {
            if (cameraOrbit == null)
            {
                return;
            }
            cameraOrbit.ShowPlayerLabels(playerLabel);
        }

        public override void OnStartAuthority()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                return;
            }
            //gameObject.name = playerName;
            cameraOrbit = FindObjectOfType<CameraOrbit>();
            cameraOrbit.Initiate(cameraAttach == null ? transform : cameraAttach);
            playerLabel = GetComponentInChildren<PlayerLabel>();
            base.OnStartAuthority();
        }

        protected override void HasAuthorityMovement()
        {
            var spaceShipSettings = SettingsContainer.Instance?.SpaceShipSettings;
            if (spaceShipSettings == null)
            {
                return;
            }

            var isFaster = Input.GetKey(KeyCode.LeftShift);
            var speed = spaceShipSettings.ShipSpeed;
            var faster = isFaster ? spaceShipSettings.Faster : 1.0f;

            shipSpeed = Mathf.Lerp(shipSpeed, speed * faster,
                SettingsContainer.Instance.SpaceShipSettings.Acceleration);

            var currentFov = isFaster
                ? SettingsContainer.Instance.SpaceShipSettings.FasterFov
                : SettingsContainer.Instance.SpaceShipSettings.NormalFov;
            cameraOrbit.SetFov(currentFov, SettingsContainer.Instance.SpaceShipSettings.ChangeFovSpeed);

            var velocity = cameraOrbit.transform.TransformDirection(Vector3.forward) * shipSpeed;
            rb.velocity = velocity * Time.deltaTime;

            if (!Input.GetKey(KeyCode.C))
            {
                var targetRotation = Quaternion.LookRotation(
                    Quaternion.AngleAxis(cameraOrbit.LookAngle, -transform.right) *
                    velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            }
        }

        protected override void FromServerUpdate() { }
        protected override void SendToServer() { }

        [ClientCallback]
        private void LateUpdate()
        {
            cameraOrbit?.CameraMovement();
            gameObject.name = playerName;
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<SimpleCrystal>(out SimpleCrystal crystal))
            {
                Point += 1;
                //crystalController.DecreaseCount();
                CmdDestroyCrystal(crystal.gameObject);
            }
            else
            {
                gameObject.SetActive(false);
                RpcSetPosition(spawnPointController.GetRandomPlayerSpawnPoint().position);
                gameObject.SetActive(true);
            }
    }

        [Server]
        void CmdDestroyCrystal(GameObject crystal)
        {
            crystalController.UnSpawn(crystal.gameObject);
            NetworkServer.UnSpawn(crystal.gameObject);
            crystalController.DecreaseCount();
        }

        [ClientRpc]
        public void RpcSetPosition(Vector3 newPosition)
        {
            gameObject.transform.position = newPosition;
        }
    }
}
