using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GroupManager : MonoBehaviour
{
    public PlayerData playerData;

    public Text playerNameText;
    public TextMeshProUGUI playerScoreText;

    void Start()
    {
        UpdateGroup();
    }

    private void Update()
    {
        UpdateGroup();
    }

    public void UpdateGroup()
    {
        playerNameText.text = playerData.playerStats.playerName;
        playerScoreText.text = playerData.playerStats.score.ToString();
    }
}