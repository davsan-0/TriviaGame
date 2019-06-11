using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Text.RegularExpressions;

namespace TriviaGame { 
    public class TcpController : MonoBehaviour
    {
        //private const string HOST_ADDR = "ec2-63-33-192-141.eu-west-1.compute.amazonaws.com";
        private const string HOST_ADDR = "localhost";

        private const string CMD_NAME = "name";
        private const string CMD_QUESTION = "question";
        private const string CMD_ANSWER = "answer";

        public static int PORT;
        public static string ROOM_CODE;
        public static bool isHost;

        private TcpClient socketConnection;
        private Thread clientReceiveThread;

        public event Action<string> PlayerJoined;
        public event Action<string> AnswerReceived;
        public event Action<Question> QuestionReceived;

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
        private void ListenForData()
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

                            try
                            {
                                CommandStruct cmdStruct = JsonUtility.FromJson<CommandStruct>(serverMessage);
                                switch (cmdStruct.cmd)
                                {
                                    case "name":
                                        //UnityMainThreadDispatcher.Instance.Enqueue(PlayerController.Instance.AddPlayer(cmdStruct.val));
                                        UnityMainThreadDispatcher.Instance.Enqueue(() => { PlayerJoined?.Invoke(cmdStruct.val); });
                                        break;
                                    case "answer":
                                        UnityMainThreadDispatcher.Instance.Enqueue(() => { AnswerReceived?.Invoke(cmdStruct.val); });
                                        break;
                                    case "question":
                                        UnityMainThreadDispatcher.Instance.Enqueue(() => {
                                            Question question = QuestionController.Instance.JsonToQuestion(cmdStruct.val);
                                            QuestionReceived?.Invoke(question);
                                        });
                                        break;
                                    default:
                                        Debug.Log("Unknown command: " + cmdStruct.cmd);
                                        break;
                                }
                            } catch (ArgumentException e)
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

        private void SendName()
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
                    string clientMessage = "#name#Dadi";
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

        [Serializable]
        private struct CommandStruct
        {
            public string cmd;
            public string val;
        }
    }
}