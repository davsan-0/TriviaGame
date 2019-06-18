using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class PlayerController : MonoBehaviour
    {

        public GameObject playerPrefab;

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
        }

        public void AddPlayer(string name)
        {
            Player player = new Player(name);
            player.Color = playerColors[players.Count];

            players.Add(player);

            OnPlayerAdded?.Invoke(player);
        }

        public void RemovePlayer(string name)
        {
            int index = players.FindIndex(player => player.Name == name);

            if (index != -1) // -1 is if it doesn't find anything
            {
                Player toRemove = players[index];
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
