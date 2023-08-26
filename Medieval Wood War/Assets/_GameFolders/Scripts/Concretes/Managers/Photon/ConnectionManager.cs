using UnityEngine;
using Photon.Pun;
using HappyHour.Abstracts.Managers;
using HappyHour.Concretes.Controllers;
using System.Collections.Generic;

namespace HappyHour.Concretes.Managers
{
    public class ConnectionManager : AbstractConnectionManager
    {
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] GameObject _playerCamera;
        [SerializeField] GameObject _InputManager;
        [SerializeField] List<Transform> _spawnPoints;
        public static int _playercounter = 0;
        public static ConnectionManager instance;
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        
        public override void SpawnPlayer()
        {
            if (_spawnPoints.Count > 0)
            {
                Transform selectedSpawnPoint = _spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount-1];
                GameObject playerInstance = PhotonNetwork.Instantiate(_playerPrefab.name, selectedSpawnPoint.position, Quaternion.identity);
                
                if (playerInstance.GetComponent<PhotonView>().IsMine)
                {
                    playerInstance.GetComponent<PlayerController>().enabled = true;
                    Camera playerCamera = _playerCamera.GetComponent<Camera>();
                    GameObject inputManagerInstance = PhotonNetwork.Instantiate(_InputManager.name, Vector3.zero, Quaternion.identity);
                    Camera.main.transform.position = selectedSpawnPoint.transform.position;
                    PlayerInputController inputController = inputManagerInstance.GetComponent<PlayerInputController>();
                }
            }
            else
            {
                Debug.LogWarning("SpawnPoint is Empty random position created!");
                float randomValue = Random.Range(-5, 5);
                GameObject playerInstance = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector2(randomValue, 0), Quaternion.identity);
                
                if (playerInstance.GetComponent<PhotonView>().IsMine)
                {
                    playerInstance.GetComponent<PlayerController>().enabled = true;
                    Camera playerCamera = _playerCamera.GetComponent<Camera>();
                    GameObject inputManagerInstance = PhotonNetwork.Instantiate("InputManager", Vector3.zero, Quaternion.identity);
                    PlayerInputController inputController = inputManagerInstance.GetComponent<PlayerInputController>();
                }
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log(PhotonNetwork.NickName + " Joined room.");
            SpawnPlayer();
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
        {
            Debug.Log(player.NickName + " Joined the room.");
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
        {
            Debug.Log(player.NickName + " Left the ayrıldı.");
        }
    }
}
