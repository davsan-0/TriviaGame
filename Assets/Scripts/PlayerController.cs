using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class PlayerController : MonoBehaviour
    {

        public GameObject playerPrefab;

        public List<string> players;

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

            players = new List<string>();
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
