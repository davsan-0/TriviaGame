using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class PlayerController : MonoBehaviour
    {

        public List<Player> activePlayers;
        public List<Player> allPlayers;

        public Dictionary<string, Player> playersDict;

        private Color[] playerColors = new Color[] { Color.red, Color.yellow, Color.cyan, Color.blue, Color.green, Color.magenta, Color.white, Color.gray };

        public Action<Player> OnPlayerAdded;
        public Action<Player> OnPlayerRemoved;

        //  Singleton
        private static PlayerController _instance;
        public static PlayerController Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("PlayerController");
                    DontDestroyOnLoad(go);
                    var component = go.AddComponent<PlayerController>();
                    _instance = component;
                }
                return _instance;
            }
        }

        // Use this for initialization
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                throw new System.Exception("An instance of this singleton already exists.");
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }

            activePlayers = new List<Player>();
            allPlayers = new List<Player>();

            TcpController.Instance.PlayerJoined += AddPlayer;
            TcpController.Instance.PlayerLeft += RemovePlayer;
            QuestionController.Instance.AnswerDiscovered += AnswerDiscovered;
            QuestionController.Instance.QuestionSet += (question) => activePlayers = new List<Player>(allPlayers);
            TcpController.Instance.PlayerSkipped += (player) => {
                int i = activePlayers.IndexOf(player);
                player.IsActivePlayer = false;
                activePlayers[(i + 1) % activePlayers.Count].IsActivePlayer = true;
                activePlayers.RemoveAt(i);
            };
        }

        public void AnswerDiscovered(TcpController.AnswerStruct answer)
        {
            if (answer.id == "") return;

            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (answer.id == activePlayers[i].Id)
                {
                    activePlayers[i].Score++;
                    activePlayers[i].IsActivePlayer = false;

                    activePlayers[(i + 1) % activePlayers.Count].IsActivePlayer = true;
                }
            }
           
        }

        public void AddPlayer(Player player)
        {
            player.Color = playerColors[allPlayers.Count];

            activePlayers.Add(player);
            allPlayers.Add(player);

            OnPlayerAdded?.Invoke(player);
        }

        public void RemovePlayer(string id)
        {
            int index = allPlayers.FindIndex(player => player.Id == id);

            if (index != -1) // -1 is if it doesn't find anything
            {
                Player toRemove = allPlayers[index];
                int i = activePlayers.IndexOf(toRemove);

                if (toRemove.IsActivePlayer)
                {
                    toRemove.IsActivePlayer = false;
                    activePlayers[(i + 1) % activePlayers.Count].IsActivePlayer = true;
                }

                activePlayers.RemoveAt(i);
                allPlayers.RemoveAt(index);
                OnPlayerRemoved?.Invoke(toRemove);
            }
        }

        // IEnumerator is to be able to run from other than main thread using UnityMainThreadDispatcher
        /*public IEnumerator AddPlayer(string name)
        {
            GameObject playerGO = GameObject.Instantiate(playerPrefab);
            Player player = playerGO.GetComponent<Player>();

            player.PlayerName = name;

            yield return null;
        }*/
    }
}
