using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame
{
    public class LobbyPlayerUI : MonoBehaviour
    {
        public GameObject playerPrefab;
        public Transform playerParent;
        public Text codeText;

        // Start is called before the first frame update
        void Start()
        {
            TcpController.Instance.PlayerJoined += Instance_PlayerJoined;

            codeText.text = TcpController.ROOM_CODE;

            foreach (string player in PlayerController.Instance.players)
            {
                Instance_PlayerJoined(player);
            }
        }

        private void Instance_PlayerJoined(string name)
        {
            var go = Instantiate(playerPrefab, playerParent);
            go.GetComponentInChildren<Text>().text = name;
            go.transform.SetSiblingIndex(playerParent.childCount - 2);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
