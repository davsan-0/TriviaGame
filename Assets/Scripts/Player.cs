using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace TriviaGame
{
    public class Player : NetworkBehaviour
    {
        [SyncVar(hook = "SetPlayerName")]
        public string playerName;
        [SyncVar(hook = "ApplyPlayerColor")]
        public Color color;

        [SyncVar(hook = "SetScore")]
        public int score = 0;

        public GameObject activePlayerImage;

        public bool IsActivePlayer
        {
            get
            {
                return isActivePlayer;
            }
            set
            {
                activePlayerImage.SetActive(value);
                isActivePlayer = value;
            }
        }

        private bool isActivePlayer;
        private TextMeshProUGUI text;
        private QuestionController gameController;
        private TMP_InputField inputField;

        private const string parentTag = "PlayerUI";

        void SetPlayerName(string playerName)
        {
            text.text = playerName + ": 0";
            this.playerName = playerName;
        }

        void ApplyPlayerColor(Color color)
        {
            text.color = color;
            this.color = color;
        }

        void SetScore(int score)
        {
            text.text = playerName + ": " + score;
            this.score = score;
        }

        void Awake()
        {
            GameObject _parent = GameObject.FindGameObjectWithTag(parentTag);
            if (transform.parent != _parent.transform)
            {
                transform.SetParent(_parent.transform, false);
            }

            text = GetComponentInChildren<TextMeshProUGUI>();

            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<QuestionController>();
            inputField = GameObject.FindGameObjectWithTag("MainInputField").GetComponent<TMP_InputField>();

            inputField.onValueChanged.AddListener(InputFieldValueChanged);
        }

        public void InputFieldValueChanged(string value)
        {
            bool correct = gameController.CheckAnswer(value);
            
            if(correct)
            {
                if (isLocalPlayer)
                    // Reveal answer for all
                    CmdRevealAnswer(value);
                else if (isServer)
                    gameController.RpcRevealAnswer(value);

                inputField.text = "";
                
            }
        }

        [Command]
        public void CmdRevealAnswer(string answer)
        {
            gameController.CmdRevealAnswer(answer);
            score++;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            SetPlayerName(playerName);
            ApplyPlayerColor(color);
            SetScore(score);
        }
    }
}