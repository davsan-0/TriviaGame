using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = "SetPlayerName")]
    public string playerName;
    [SyncVar(hook = "ApplyPlayerColor")]
    public Color color;

    public GameObject activePlayerImage;

    public bool IsActivePlayer
    {
        get
        {
            return isActivePlayer;
        }
        set
        {
            activePlayerImage.SetActive(value);
            isActivePlayer = value;
        }
    }

    private bool isActivePlayer;
    private TextMeshProUGUI text;

    private const string parentTag = "PlayerUI";

    void SetPlayerName(string name)
    {
        text.text = name;
    }

    void ApplyPlayerColor(Color color)
    {
        text.color = color;
    }

    void Awake()
    {
        GameObject _parent = GameObject.FindGameObjectWithTag(parentTag);
        if (transform.parent != _parent.transform)
        {
            transform.SetParent(_parent.transform, false);
        }

        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        SetPlayerName(playerName);
        ApplyPlayerColor(color);
    }
}
