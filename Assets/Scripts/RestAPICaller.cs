using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace TriviaGame
{
    public class RestAPICaller : MonoBehaviour
    {
        //const string URI = "http://ec2-63-33-192-141.eu-west-1.compute.amazonaws.com";
        const string URI = "http://localhost:3000";
        const string GET_QUESTION = "/questions";
        const string CREATE_GAME = "/host";
        const string JOIN_GAME = "/join";

        //  Singleton
        private static RestAPICaller _instance;
        public static RestAPICaller Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("RestAPICaller");
                    DontDestroyOnLoad(go);
                    var component = go.AddComponent<RestAPICaller>();
                    _instance = component;
                }
                return _instance;
            }
        }

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
        }

        // Start is called before the first frame update
        void Start()
        {
            //GetQuestion(2);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerator GetRequest(string uri, Action<string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    callback(webRequest.downloadHandler.text);
                }
            }
        }

        public void GetQuestion(int limit, Action<List<Question>> callback)
        {
            string limitStr = (limit <= 0) ? "" : "?limit=" + limit;
            StartCoroutine(GetRequest(URI + GET_QUESTION + limitStr, (string questions) =>
            {
                callback(Question.JsonToQuestions(questions));
            }));
        }

        public void CreateGame(string playerName)
        {
            StartCoroutine(GetRequest(URI + CREATE_GAME, (string json) =>
            {
                CodeStruct codeStruct = JsonUtility.FromJson<CodeStruct>(json);
                SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
                TcpController.Instance.ConnectToTcpServer(codeStruct.port);
                TcpController.ROOM_CODE = codeStruct.code;

                PlayerController.Instance.players = new List<Player>();

                TcpController.isHost = true;

                TcpController.Instance.BroadcastJoinWithName(playerName);
                Debug.Log(JsonUtility.ToJson(codeStruct));
            }));
        }

        public void JoinGame(string playerName, string code)
        {
            StartCoroutine(GetRequest(URI + JOIN_GAME + "?code=" + code, (string json) =>
            {
                CodeStruct codeStruct = JsonUtility.FromJson<CodeStruct>(json);
                SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
                TcpController.Instance.ConnectToTcpServer(codeStruct.port);
                TcpController.Instance.BroadcastJoinWithName(playerName);
                TcpController.ROOM_CODE = code;
                

                /*PlayerController.Instance.players = new List<string>();
                PlayerController.Instance.players.Add(playerName);*/

                Debug.Log(JsonUtility.ToJson(codeStruct));
            }));
        }

        [Serializable]
        private struct CodeStruct
        {
            public int port;
            public string code;
        }
    }
}