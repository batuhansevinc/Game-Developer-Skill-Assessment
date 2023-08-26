using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFiller : MonoBehaviour
{
    public PlayerData playerData;
    public TextMeshPro _nameText;

    private void Awake()
    {
        _nameText = GetComponent<TextMeshPro>();
        _nameText.text = playerData.playerStats.playerName;
    }
}
