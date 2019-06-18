using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

namespace TriviaGame { 
    public class TcpController : MonoBehaviour
    {
        //private const string HOST_ADDR = "ec2-63-33-192-141.eu-west-1.compute.amazonaws.com";
        private const string HOST_ADDR = "localhost";

        private const string CMD_NAME = "name";
        private const string CMD_QUESTION = "question";
        private const string CMD_ANSWER = "answer";
        private const string CMD_STARTGAME = "startgame";

        public static int PORT;
        public static string ROOM_CODE;
        public static bool isHost;

        private TcpClient socketConnection;
        private Thread clientReceiveThread;

        public event Action<string> PlayerJoined;
        public event Action<string> PlayerLeft;
        public event Action<string> AnswerReceived;
        public event Action<Question> QuestionReceived;
        public event Action StartGame;

        //  Singleton
        private static TcpController _instance;
        public static TcpController Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("TcpController");
                    DontDestroyOnLoad(go);
                    var component = go.AddComponent<TcpController>();
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

        public void SendQuestion(Question question)
        {
            SendCommand(CMD_QUESTION, Question.QuestionToJson(question));
        }

        // Use this for initialization 	
        void Start()
        {
            //ConnectToTcpServer();
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerJoined?.Invoke("Daddo");
            }
        }
        /// <summary> 	
        /// Setup socket connection. 	
        /// </summary> 	
        public void ConnectToTcpServer(int port)
        {
            PORT = port;
            try
            {
                clientReceiveThread = new Thread(new ThreadStart(ListenForData));
                clientReceiveThread.IsBackground = true;
                clientReceiveThread.Start();
            }
            catch (Exception e)
            {
                Debug.Log("On client connect exception " + e);
            }
        }
        /// <summary> 	
        /// Runs in background clientReceiveThread; Listens for incoming data. 	
        /// </summary>     
        private void ListenForDat2a()
        {
            try
            {
                socketConnection = new TcpClient(HOST_ADDR, PORT);
                Byte[] bytes = new Byte[1024];
                while (true)
                {
                    // Get a stream object for reading 				
                    using (NetworkStream stream = socketConnection.GetStream())
                    {
                        int length;
                        // Read incoming stream into byte arrary. 					
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            // Convert byte array to string message. 						
                            string serverMessage = Encoding.ASCII.GetString(incomingData);
                            Debug.Log(serverMessage);
                            Regex rx = new Regex(@"{(.*?)}");
                            MatchCollection matches = rx.Matches(serverMessage);

                            List<string> jsons = ParseStringToJSONList(serverMessage);

                            foreach (string json in jsons)
                            {
                                try
                                {
                                    CommandStruct cmdStruct = JsonUtility.FromJson<CommandStruct>(json);
                                    switch (cmdStruct.cmd)
                                    {
                                        case "name":
                                            //UnityMainThreadDispatcher.Instance.Enqueue(PlayerController.Instance.AddPlayer(cmdStruct.val));
                                            UnityMainThreadDispatcher.Instance.Enqueue(() => { PlayerJoined?.Invoke(cmdStruct.val);
                                                Debug.Log("name = " + cmdStruct.val);
                                            });
                                            break;
                                        case "answer":
                                            UnityMainThreadDispatcher.Instance.Enqueue(() => { AnswerReceived?.Invoke(cmdStruct.val);
                                                Debug.Log("answer = " + cmdStruct.val);
                                            });
                                            break;
                                        case "question":
                                            UnityMainThreadDispatcher.Instance.Enqueue(() => {
                                                Question question = Question.JsonToQuestion(cmdStruct.val);
                                                QuestionReceived?.Invoke(question);
                                            });
                                            break;
                                        case "playerleft":
                                            UnityMainThreadDispatcher.Instance.Enqueue(() => { PlayerLeft?.Invoke(cmdStruct.val); });
                                            Debug.Log("playerleft = " + cmdStruct.val);
                                            break;
                                        case "startgame":
                                            UnityMainThreadDispatcher.Instance.Enqueue(() => { StartGame?.Invoke(); });
                                            Debug.Log("startgame");
                                            break;
                                        default:
                                            Debug.Log("Unknown command: " + cmdStruct.cmd);
                                            break;
                                    }
                                }
                                catch (ArgumentException e)
                                {
                                    Debug.Log("Argument Exception: " + e);
                                }
                            }
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
        }

        private void ListenForData()
        {
            try
            {
                socketConnection = new TcpClient(HOST_ADDR, PORT);
                byte[] bytes = new byte[1024];
                while (true)
                {
                    // Get a stream object for reading 				
                    using (NetworkStream stream = new NetworkStream(socketConnection.Client, false)) 
                    {
                        int numBytesRead = 0;
                        StringBuilder serverMessage = new StringBuilder();
                        do
                        {
                            numBytesRead = stream.Read(bytes, 0, bytes.Length);
                            serverMessage.AppendFormat("{0}", Encoding.ASCII.GetString(bytes, 0, numBytesRead));
                        } while (stream.DataAvailable);

                        Debug.Log("serverMessage = " + serverMessage);
                        List<string> jsons = ParseStringToJSONList(serverMessage.ToString());

                        foreach (string json in jsons)
                        {
                            try
                            {
                                CommandStruct cmdStruct = JsonUtility.FromJson<CommandStruct>(json);
                                switch (cmdStruct.cmd)
                                {
                                    case "name":
                                        //UnityMainThreadDispatcher.Instance.Enqueue(PlayerController.Instance.AddPlayer(cmdStruct.val));
                                        UnityMainThreadDispatcher.Instance.Enqueue(() => {
                                            PlayerJoined?.Invoke(cmdStruct.val);
                                            Debug.Log("name = " + cmdStruct.val);
                                        });
                                        break;
                                    case "answer":
                                        UnityMainThreadDispatcher.Instance.Enqueue(() => {
                                            AnswerReceived?.Invoke(cmdStruct.val);
                                            Debug.Log("answer = " + cmdStruct.val);
                                        });
                                        break;
                                    case "question":
                                        UnityMainThreadDispatcher.Instance.Enqueue(() => {
                                            Question question = Question.JsonToQuestion(cmdStruct.val);
                                            QuestionReceived?.Invoke(question);
                                        });
                                        break;
                                    case "playerleft":
                                        UnityMainThreadDispatcher.Instance.Enqueue(() => { PlayerLeft?.Invoke(cmdStruct.val); });
                                        Debug.Log("playerleft = " + cmdStruct.val);
                                        break;
                                    case "startgame":
                                        UnityMainThreadDispatcher.Instance.Enqueue(() => { StartGame?.Invoke(); });
                                        Debug.Log("startgame");
                                        break;
                                    default:
                                        Debug.Log("Unknown command: " + cmdStruct.cmd);
                                        break;
                                }
                            }
                            catch (ArgumentException e)
                            {
                                Debug.Log("Argument Exception: " + e);
                            }
                        }
                    }
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
        }
        /// <summary> 	
        /// Send message to server using socket connection. 	
        /// </summary> 	
        private void SendCommand(string command, string value)
        {
            if (socketConnection == null)
            {
                return;
            }
            try
            {
                // Get a stream object for writing. 			
                NetworkStream stream = socketConnection.GetStream();
                if (stream.CanWrite)
                {
                    var cmdStruct = new CommandStruct { cmd = command, val = value };

                    string clientMessage = JsonUtility.ToJson(cmdStruct);
                    // Convert string message to byte array.                 
                    byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                    // Write byte array to socketConnection stream.                 
                    stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                    Debug.Log("Client sent his message - should be received by server");
                }
            }
            catch (SocketException socketException)
            {
                Debug.Log("Socket exception: " + socketException);
            }
        }

        private List<string> ParseStringToJSONList(string str)
        {
            List<string> res = new List<string>();

            int cmd_start = 0;
            int count = 0; // # of {
            for (int i = 0; i < str.Length; i++)
            {
                switch (str[i])
                {
                    case '{':
                        if (count == 0)
                        {
                            cmd_start = i;
                        }
                        count++;
                        break;
                    case '}':
                        count--;
                        if (count == 0)
                        {
                            res.Add(str.Substring(cmd_start, i + 1 - cmd_start));
                        }
                        break;
                }
            }

            return res;
        }

        public void BroadcastJoinWithName(string playerName)
        {
            StartCoroutine(BroadcastJoinWithNameCoroutine(playerName));
        }

        private IEnumerator BroadcastJoinWithNameCoroutine(string playerName)
        {
            while (socketConnection == null)
            {
                Debug.Log("Waiting for connection to broadcast name.");
                yield return null;
            }
            SendCommand(CMD_NAME, playerName);
        }

        public void SendStartGame()
        {
            SendCommand(CMD_STARTGAME, "");
        }

        public void SendAnswer(string answer)
        {
            SendCommand(CMD_ANSWER, answer);
        }

        [Serializable]
        private struct CommandStruct
        {
            public string cmd;
            public string val;
        }
    }
}