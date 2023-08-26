using UnityEngine;
using Photon.Pun;
using HappyHour.Abstract;
using HappyHour.Concretes.Controllers;

namespace HappyHour.Concretes
{
    public class ResourceController : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] ResourceData baseResourceData;
        [SerializeField] ResourceInventory baseResourceInventory;
        [SerializeField] GameObject baseTreePrefab;
        [SerializeField] GameObject choppedTreePrefab;
        [HideInInspector]
        public ResourceData currentResourceData;
        [HideInInspector]
        public ResourceInventory currentResourceInventory;

        WorkerController _workerInventory;

        float collectionInterval = 5f;
        float timeSinceLastCollection = 0f;

        private void Awake()
        {
            currentResourceData = Instantiate(baseResourceData);
            currentResourceInventory = Instantiate(baseResourceInventory);
            currentResourceData.resourceInventory = currentResourceInventory;
        }

        private void Start()
        {
            choppedTreePrefab.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Worker"))
            {
                _workerInventory = other.GetComponent<WorkerController>();
                timeSinceLastCollection += Time.deltaTime;
                if (timeSinceLastCollection >= collectionInterval)
                {
                    photonView.RPC("ChangeTreeState", RpcTarget.All);

                    if (currentResourceData.resourceInventory.RemoveResource(10))
                    {
                        _workerInventory.CurrentCarryAmount += 10;
                        photonView.RPC("UpdateResourceOnOtherClients", RpcTarget.OthersBuffered, 10);
                    }

                    if (currentResourceData.resourceInventory.wood <= 0)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
                    timeSinceLastCollection = 0f;
                }
            }
        }

        [PunRPC]
        void ChangeTreeState()
        {
            baseTreePrefab.SetActive(false);
            choppedTreePrefab.SetActive(true);
        }

        [PunRPC]
        void UpdateResourceOnOtherClients(int amount)
        {
            currentResourceData.resourceInventory.RemoveResource(amount);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(currentResourceInventory.wood);
            }
            else
            {
                currentResourceInventory.wood = (int)stream.ReceiveNext();
            }
        }
    }
}
