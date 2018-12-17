using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TGNetworkManager : NetworkManager
{
    public Transform playersTextArea;

    private int playerIncrement = 0;
    private Color[] playerColors = new Color[] { Color.red, Color.yellow, Color.cyan, Color.magenta, Color.white };

    private void Start()
    {

    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Debug.Log(playersTextArea);
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, playersTextArea);
        player.GetComponent<Player>().color = playerColors[playerIncrement % playerColors.Length];
        playerIncrement++;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
