using System;
using System.Collections;
using BatuhanSevinc.Abstracts.Patterns;
using BatuhanSevinc.ScriptableObjects.GameEventListeners;
using HappyHour.Concretes.Controllers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HappyHour.Concretes.Managers
{
    public class UIManager : SingletonMonoDestroy<UIManager>
    {
        [SerializeField] GameObject _startPanel;
        [SerializeField] GameObject _lobbyPanel;
        [SerializeField] GameObject _finishPanel;
        [SerializeField] GameObject _timeboxRed;
        [SerializeField] GameObject _counter;
        [SerializeField] GameObject _tabToStart;
        [SerializeField] TextMeshProUGUI _timeText;
        [SerializeField] TextMeshProUGUI _woodText;
        [SerializeField] NormalGameEventListener _resourceCollectedListener;

        private void Awake()
        {
            SetSingleton(this);
        }

        public GameObject FinishPanel => _finishPanel;
        public GameObject TimeboxRed => _timeboxRed;
        public TextMeshProUGUI TimeText => _timeText;

        public TextMeshProUGUI WoodText
        {
            get => _woodText;
            set => _woodText = value;
        }


        private void OnEnable()
        {
            _resourceCollectedListener.ParameterEventWithObject += UpdateResourceUI;
        }

        private void OnDisable()
        {
            _resourceCollectedListener.ParameterEventWithObject -= UpdateResourceUI;
        }

        void UpdateResourceUI(object progress)
        {
            Debug.Log("Wood Collected: " + progress);
            WoodText.text = progress.ToString();
        }

        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(3);
            _startPanel.SetActive(false);
            GameManager.Instance.StartGame();
        }

        public void ClosePanel()
        {
            StartCoroutine(StartGame());
            _startPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0f);
            _counter.SetActive(true);
            _tabToStart.SetActive(false);
            _lobbyPanel.SetActive(false);
        }
    }
}