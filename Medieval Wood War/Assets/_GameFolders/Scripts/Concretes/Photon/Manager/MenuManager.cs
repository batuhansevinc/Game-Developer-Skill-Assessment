using System.Collections.Generic;
using BatuhanSevinc.ScriptableObjects;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Serialization;

namespace HappyHour.Concretes.Managers
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject _userNameScreen;
        [SerializeField] PlayerData _playerData;
        [SerializeField] GameObject _lobbyPanel;
        [SerializeField] GameObject _startGamePanel;
        [SerializeField] GameObject _createUserNameButton;
        [SerializeField] TMP_InputField _userNameInput;
        [SerializeField] TMP_InputField _createOrJoinRoomInput;
        [SerializeField] GameEvent _gameStartEvent;

        public const string SKILL_LEVEL_PROPERTY_KEY = "SkillLevel";
        
        private List<RoomInfo> _cachedRoomList = new List<RoomInfo>();
        
        void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master server!");
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Connected to Lobby!");
            _userNameScreen.SetActive(true);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _cachedRoomList = roomList;
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Successfully joined the room: " + PhotonNetwork.CurrentRoom.Name);
            _startGamePanel.SetActive(true);
            _gameStartEvent.InvokeEvents();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join a random room. Creating a new room...");
            CreateRoom();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError("Failed to create room: " + message);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError("Failed to join room: " + message);
        }

        public void OnClick_CreateNameButton()
        {
            _playerData.playerStats.playerName = _userNameInput.text;
            PhotonNetwork.NickName = _playerData.playerStats.playerName;
            _userNameScreen.SetActive(false);
        }

        public void OnNameField_Changed()
        {
            if (_userNameInput.text.Length >= 2)
            {
                _createUserNameButton.SetActive(true);
            }
            else
            {
                _createUserNameButton.SetActive(false);
            }
        }

        public void Onclick_JoinRoom()
        {
            RoomInfo room = FindRoomWithMatchingSkill();
            if (room != null)
            {
                PhotonNetwork.JoinRoom(room.Name);
            }
            else
            {
                CreateOrJoinRoom();
            }
        }

        private RoomInfo FindRoomWithMatchingSkill()
        {
            int currentPlayerSkillLevel = 0;

            foreach (RoomInfo room in _cachedRoomList)
            {
                if (room.CustomProperties.ContainsKey(SKILL_LEVEL_PROPERTY_KEY))
                {
                    int roomSkillLevel = (int)room.CustomProperties[SKILL_LEVEL_PROPERTY_KEY];

                    if (roomSkillLevel == currentPlayerSkillLevel)
                    {
                        return room;
                    }
                }
            }

            return null;
        }

        public void CreateOrJoinRoom()
        {
            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(null, ro, TypedLobby.Default);
        }

        public void CreateRoom()
        {
            int currentPlayerSkillLevel = 0;

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { SKILL_LEVEL_PROPERTY_KEY, currentPlayerSkillLevel }
            };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { SKILL_LEVEL_PROPERTY_KEY };
            roomOptions.IsVisible = true;
            PhotonNetwork.CreateRoom(_createOrJoinRoomInput.text, roomOptions);
        }
    }
}