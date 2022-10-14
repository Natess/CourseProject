using Characters;
using Controller;
using Messages;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager
    {
        [SerializeField] UIController uiController;
        [SerializeField] CrystalController crystalSpawnController;
        [SerializeField] SpawnPointsController spawnPointsController;

        Dictionary<int, ShipController> players = new Dictionary<int, ShipController>();

        #region Server

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var spawnTransform = spawnPointsController.GetRandomPlayerSpawnPoint();//GetStartPosition();

            var player = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);

            players[conn.connectionId] = player.GetComponent<ShipController>();
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
        public override void OnStartServer()
        {
            base.OnStartServer();
            StartCoroutine(WaitOneSecond());

            NetworkServer.RegisterHandler(101, SetName);

            spawnPointsController = Singleton<SpawnPointsController>.Instance;
            crystalSpawnController.AllCollected += onAllCollected;
        }

        private IEnumerator WaitOneSecond()
        {
            yield return new WaitForSeconds(1.0f);
        }

        private void onAllCollected()
        {
            var mes = new PointsTableMessage();
            mes.Rows = players.Values.Select(x => new PointTableRow(x.gameObject.name, x.Point)).ToList();
            NetworkServer.SendToAll(102, mes);
        }
        private void SetName(NetworkMessage networkMessage)
        {
            players[networkMessage.conn.connectionId].PlayerName = networkMessage.reader.ReadString();
            players[networkMessage.conn.connectionId].gameObject.name = players[networkMessage.conn.connectionId].PlayerName;
        }

        #endregion

        #region Client

        public override void OnStartClient(NetworkClient client)
        {
            base.OnStartClient(client);
            client.RegisterHandler(102, ShowTable);
        }

        private void ShowTable(NetworkMessage netMsg)
        {
            var msg = new PointsTableMessage();
            msg.Deserialize(netMsg.reader);

            uiController.ShowTable(msg.ToString());
            NetworkManager.singleton.StopHost();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            var mes = new UserNameMessage();
            mes.UserName = uiController.UserName;
            conn.Send(101, mes);
        }

        #endregion
    }
}
