using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace TriviaGame
{
    public class PlayerMain : MonoBehaviour
    {
        private string _playerName;
        private Color _color;
        private int _score;

        public string PlayerName {
            get
            {
                return _playerName;
            }
        }

        public Color PlayerColor
        {
            get
            {
                return _color;
            }
        }

        public int Score
        {
            get
            {
                return _score;
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
        }

        private bool isActivePlayer;
        private TextMeshProUGUI text;
        private QuestionController questionController;
        private TMP_InputField inputField;

        private const string parentTag = "PlayerUI";

        public void SetPlayerName(string value)
        {
            _playerName = value;
            text.text = _playerName + ": 0";
        }

        public void SetPlayerColor(Color value)
        {
            _color = value;
            text.color = _color;
        }

        public void SetScore(int value)
        {
            _score = value;
            text.text = _playerName + ": " + _score;
        }

        public void SetActivePlayer(bool value)
        {
            isActivePlayer = value;
            activePlayerImage.SetActive(isActivePlayer);
        }

        void Awake()
        {
            GameObject _parent = GameObject.FindGameObjectWithTag(parentTag);
            if (transform.parent != _parent.transform)
            {
                transform.SetParent(_parent.transform, false);
            }

            text = GetComponentInChildren<TextMeshProUGUI>();

            inputField = GameObject.FindGameObjectWithTag("MainInputField").GetComponent<TMP_InputField>();

            //inputField.onValueChanged.AddListener(InputFieldValueChanged);
        }

        /*
        public void InputFieldValueChanged(string value)
        {
            bool correct = QuestionController.Instance.CheckAnswer(value);
            
            if(correct)
            {

                inputField.text = "";
                
            }
        }*/

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