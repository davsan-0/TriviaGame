using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace TriviaGame
{
    public class Player : MonoBehaviour
    {
        private string _playerName;
        private Color _color;
        private int _score;

        public string PlayerName {
            get
            {
                return _playerName;
            }
            set
            {
                text.text = value + ": 0";
                _playerName = value;
            }
        }

        public Color PlayerColor
        {
            get
            {
                return _color;
            }
            set
            {
                text.color = value;
                _color = value;
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                text.text = _playerName + ": " + _score;
                _score = value;
            }
        }
        //[SyncVar(hook = "ApplyPlayerColor")]
        /*public Color color;

        //[SyncVar(hook = "SetScore")]
        public int score = 0;*/

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
        private QuestionController questionController;
        private TMP_InputField inputField;

        private const string parentTag = "PlayerUI";

        /*public void SetPlayerName(string playerName)
        {
            text.text = playerName + ": 0";
            this.playerName = playerName;
        }

        public void ApplyPlayerColor(Color color)
        {
            text.color = color;
            this.color = color;
        }

        void SetScore(int score)
        {
            text.text = playerName + ": " + score;
            this.score = score;
        }*/

        void Awake()
        {
            GameObject _parent = GameObject.FindGameObjectWithTag(parentTag);
            if (transform.parent != _parent.transform)
            {
                transform.SetParent(_parent.transform, false);
            }

            text = GetComponentInChildren<TextMeshProUGUI>();

            inputField = GameObject.FindGameObjectWithTag("MainInputField").GetComponent<TMP_InputField>();

            inputField.onValueChanged.AddListener(InputFieldValueChanged);
        }

        public void InputFieldValueChanged(string value)
        {
            bool correct = QuestionController.Instance.CheckAnswer(value);
            
            if(correct)
            {
                /*if (isLocalPlayer)
                    // Reveal answer for all
                    CmdRevealAnswer(value);
                else if (isServer)
                    gameController.RpcRevealAnswer(value);*/

                inputField.text = "";
                
            }
        }

        /*[Command]
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
        }*/
    }
}