using Photon.Pun;
using UnityEngine;
using Pathfinding;
using HappyHour.Interfaces;
using System.Collections;
using BatuhanSevinc.ScriptableObjects;
using TMPro;
using UnityEngine.Serialization;

namespace HappyHour.Concretes.Controllers
{
    public class WorkerController : MonoBehaviourPun, IPunObservable, IMovable, IResourceCollector, ICarrier
    {
        [SerializeField] GameEvent _resourceCollectedEvent;
        AIPath _aiPath;
        AIDestinationSetter _destinationSetter;
        Animator _animator;
        GameObject _currentTargetObject = null;
        public TMP_Text _nameText;

        public GameObject SelectedGameObject;
        public Transform BasePosition;
        public bool IsWorking = false;
        public int MaxCarryAmount => 50;
        public int CurrentCarryAmount { get; set; } = 0;

        private void Start()
        {
            _aiPath = GetComponent<AIPath>();
            _destinationSetter = GetComponent<AIDestinationSetter>();
            _animator = GetComponent<Animator>();

            if (SelectedGameObject)
            {
                SelectedGameObject.SetActive(false);
            }
        }
        
        public void SetBasePosition(Transform baseTransform)
        {
            BasePosition = baseTransform;
        }

        public void SetSelected(bool isSelected)
        {
            if (SelectedGameObject)
            {
                SelectedGameObject.SetActive(isSelected);
            }
        }

        [PunRPC]
        public void SetDestination(Vector3 destination)
        {
            if (photonView.IsMine)
            {
                if (_currentTargetObject == null)
                {
                    _currentTargetObject = new GameObject("Target");
                }
                _currentTargetObject.transform.position = destination;
                _destinationSetter.target = _currentTargetObject.transform;
            }
        }

        public void MoveToDestination(Vector3 destination)
        {
            photonView.RPC("SetDestination", RpcTarget.All, destination);
            if (IsWorking)
            {
                StopCoroutine(ResourceCollectRoutine(null));
            }
        }

        public void CollectResourceFrom(ResourceController resourceController)
        {
            IsWorking = true;
            StartCoroutine(ResourceCollectRoutine(resourceController));
        }

        private IEnumerator ResourceCollectRoutine(ResourceController resourceController)
        {
            while (IsWorking)
            {
                MoveToDestination(resourceController.transform.position);
                if(Vector3.Distance(transform.position, resourceController.transform.position) < 1f)
                {
                    CurrentCarryAmount += 10;
                }
                if (CurrentCarryAmount >= MaxCarryAmount)
                {
                    ReturnToBase();
                    yield return new WaitUntil(() => Vector3.Distance(transform.position, BasePosition.position) < 1f); 
                    DeliverResources();
                    CurrentCarryAmount = 0; 
                }
                yield return new WaitForSeconds(1f);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (photonView.IsMine)
            {
                if (other.TryGetComponent<PlayerController>(out PlayerController playerController))
                {
                    playerController.CurrentPlayerData.playerInventory.AddWood(CurrentCarryAmount);
                    int progress = playerController.CurrentPlayerData.playerInventory.wood;
                    _resourceCollectedEvent.InvokeEventsWithObject(progress);
                    Debug.Log("wood invoked: " + progress);
                    CurrentCarryAmount = 0;
                    DeliverResources();
                }
            }
        }

        public void ReturnToBase()
        {
            MoveToDestination(BasePosition.position);
        }

        public void DeliverResources()
        {
            Debug.Log("Resources delivered to the base.");
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(_animator.GetBool("isMoving"));
            }
            else
            {
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();
                _animator.SetBool("isMoving", (bool)stream.ReceiveNext());
            }
        }
    }
}
