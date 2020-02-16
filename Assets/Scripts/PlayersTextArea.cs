using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

            foreach (Player player in PlayerController.Instance.allPlayers)
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

            PlayerController.Instance.allPlayers[0].IsActivePlayer = true;
            PlayerController.Instance.OnPlayerRemoved += RemovePlayer;

            QuestionController.Instance.QuestionSet += (question) =>
            {
                foreach (KeyValuePair<Player, GameObject> item in playersDict)
                {
                    item.Value.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.SmallCaps;
                }
            };

            TcpController.Instance.PlayerSkipped += (player) =>
            {
                playersDict[player].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough | FontStyles.SmallCaps;
            };
        }
        

        private void RemovePlayer(Player player)
        {
            try
            {
                Destroy(playersDict[player]);
                playersDict.Remove(player);
            } catch (KeyNotFoundException e)
            {
                Debug.Log(e);
            }
        }
    }

}
