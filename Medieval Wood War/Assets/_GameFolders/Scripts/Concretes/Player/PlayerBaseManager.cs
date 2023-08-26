using UnityEngine;
using Photon.Pun;

namespace HappyHour.Concretes.Controllers
{
    public class PlayerBaseManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public PlayerData basePlayerData;
        public PlayerBase basePlayerBase;
        public PlayerInventory basePlayerInventory;
        public PlayerStats basePlayerStats;

        private PlayerData _currentPlayerData;
        private PlayerBase _currentPlayerBase;
        private PlayerInventory _currentPlayerInventory;
        private PlayerStats _currentPlayerStats;

        public PlayerInventory CurrentPlayerInventory 
        {
            get => _currentPlayerInventory;
            set => _currentPlayerInventory = value;
        }

        public PlayerData CurrentPlayerData
        {
            get => _currentPlayerData;
            set => _currentPlayerData = value;
        }

        private void Awake()
        {
            if (photonView.IsMine)
            {
                _currentPlayerData = Instantiate(basePlayerData);
                _currentPlayerBase = Instantiate(basePlayerBase);
                _currentPlayerInventory = Instantiate(basePlayerInventory);
                _currentPlayerStats = Instantiate(basePlayerStats);

                _currentPlayerData.playerBase = _currentPlayerBase;
                _currentPlayerData.playerInventory = _currentPlayerInventory;
                _currentPlayerData.playerStats = _currentPlayerStats;
            }
        }

        public override void OnJoinedRoom()
        {
            GameObject workerPrefab = _currentPlayerData.workerPrefab;
            if(photonView.IsMine)
            {
                PhotonNetwork.Instantiate("PhotonPrefabs/" + workerPrefab.name, transform.position + Vector3.right * 2, Quaternion.identity);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }
    }
}