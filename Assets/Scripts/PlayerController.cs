using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class PlayerController : MonoBehaviour
    {

        public List<Player> players;

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

            players = new List<Player>();

            TcpController.Instance.PlayerJoined += AddPlayer;
            TcpController.Instance.PlayerLeft += RemovePlayer;
            QuestionController.Instance.AnswerDiscovered += AnswerDiscovered;
        }

        public void AnswerDiscovered(TcpController.AnswerStruct answer)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (answer.id == players[i].Id)
                {
                    players[i].Score++;
                    players[i].IsActivePlayer = false;

                    players[(i + 1) % players.Count].IsActivePlayer = true;
                }
            }
           
        }

        public void AddPlayer(Player player)
        {
            player.Color = playerColors[players.Count];

            players.Add(player);

            OnPlayerAdded?.Invoke(player);
        }

        public void RemovePlayer(string id)
        {
            int index = players.FindIndex(player => player.Id == id);

            if (index != -1) // -1 is if it doesn't find anything
            {
                Player toRemove = players[index];
                if (toRemove.IsActivePlayer)
                {
                    toRemove.IsActivePlayer = false;
                    players[(index + 1) % players.Count].IsActivePlayer = true;
                }

                players.RemoveAt(index);
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
