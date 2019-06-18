using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class PlayersTextArea : MonoBehaviour
    {
        public GameObject playerPrefab;

        private Dictionary<Player, GameObject> playersDict;

        // Start is called before the first frame update
        void Start()
        {
            playersDict = new Dictionary<Player, GameObject>();

            foreach (Player player in PlayerController.Instance.players)
            {
                GameObject go = Instantiate(playerPrefab);
                PlayerMain pm = go.GetComponent<PlayerMain>();
                pm.SetPlayerColor(player.Color);
                pm.SetPlayerName(player.Name);

                player.OnActivePlayerChanged += pm.SetActivePlayer;
                player.OnScoreChanged += pm.SetScore;
                player.OnColorChanged += pm.SetPlayerColor;
                player.OnNameChanged += pm.SetPlayerName;

                playersDict.Add(player, go);
            }

            PlayerController.Instance.players[0].IsActivePlayer = true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
