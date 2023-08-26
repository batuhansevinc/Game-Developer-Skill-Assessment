using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace HappyHour.Concretes.Controllers
{
    public class PlayerBaseController : MonoBehaviourPunCallbacks,IPunObservable
    {
        [FormerlySerializedAs("playerController")] [Header("Dependencies")]
        public PlayerBaseManager playerBaseManager;
        public GameObject workerPrefab;
        public Transform spawnPoint;
        private void Awake()
        {
            if (!photonView.IsMine)
            {
                playerBaseManager.enabled = false;
            }
        }
        
        private void Start()
        {
            if (photonView.IsMine)
            {
                for (int i = 0; i < 3; i++)
                {
                    SpawnWorker();
                }
            }
        }

        void SpawnWorker()
        {
            GameObject workerInstance = PhotonNetwork.Instantiate(workerPrefab.name, spawnPoint.position, Quaternion.identity);
            WorkerController workerController = workerInstance.GetComponent<WorkerController>();
            if (workerController)
            {
                workerController.SetBasePosition(transform);
                workerController._nameText.text = GetComponent<PlayerBaseManager>().CurrentPlayerData.playerStats.playerName;
                workerController._nameText.color = Color.green;
            }
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }
    }
}