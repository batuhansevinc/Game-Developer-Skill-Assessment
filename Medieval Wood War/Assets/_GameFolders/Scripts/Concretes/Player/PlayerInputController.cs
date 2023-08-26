using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HappyHour.Concretes.Controllers
{
    public class PlayerInputController : MonoBehaviourPun, IPunObservable
    {
        WorkerController _currentSelectedWorker; 
        [SerializeField] Camera _playerCamera;

        private void Start()
        {
            _playerCamera = Camera.main;
        }

        private void Awake()
        {
            if (!photonView.IsMine)
            {
                enabled = false;
            }
        }
        
        void Update()
        {
           Move();
        }

        void Move()
        {
             if (_playerCamera == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = _playerCamera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null)
                {
                    WorkerController npcWorker = hit.collider.GetComponent<WorkerController>();
                    ResourceController resource = hit.collider.GetComponent<ResourceController>();
                    PlayerBaseController playerBase = hit.collider.GetComponent<PlayerBaseController>();
                    if (npcWorker && npcWorker.photonView.IsMine)
                    {
                        if (_currentSelectedWorker && _currentSelectedWorker != npcWorker)
                        {
                            _currentSelectedWorker.SetSelected(false);
                        }

                        _currentSelectedWorker = npcWorker;
                        _currentSelectedWorker.SetSelected(true);
                    }
                    else if (resource && _currentSelectedWorker)
                    {
                        _currentSelectedWorker.CollectResourceFrom(resource);
                        _currentSelectedWorker.SetSelected(false);
                        _currentSelectedWorker = null;
                    }
                    else if (playerBase && _currentSelectedWorker)
                    {
                        _currentSelectedWorker.SetSelected(false);
                        _currentSelectedWorker = null;
                    }
                }
                else
                {
                    if (_currentSelectedWorker && _currentSelectedWorker.photonView.IsMine)
                    {
                        _currentSelectedWorker.MoveToDestination(mousePos);
                    }
                }
            }
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
           
        }
    }
}
