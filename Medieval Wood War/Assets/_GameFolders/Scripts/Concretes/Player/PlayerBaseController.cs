using Photon.Pun;
using TMPro;
using UnityEngine;

namespace HappyHour.Concretes.Controllers
{
    public class PlayerBaseController : MonoBehaviourPunCallbacks,IPunObservable
    {
        [Header("Dependencies")]
        public PlayerController playerController;
        public GameObject workerPrefab;
        public Transform spawnPoint;
        private void Awake()
        {
            if (!photonView.IsMine)
            {
                playerController.enabled = false;
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
                workerController._nameText.text = GetComponent<PlayerController>().CurrentPlayerData.playerStats.playerName;
                workerController._nameText.color = Color.green;
            }
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }
    }
}