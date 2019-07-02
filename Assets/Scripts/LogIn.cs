using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace TriviaGame
{
    public class LogIn : MonoBehaviour
    {
        public Button joinButton;
        public Button createButton;
        [Space]
        public Text nameText;
        public Text codeText;
        [Space]
        public InputField codeInput;


        // Start is called before the first frame update
        void Start()
        {
            TouchScreenKeyboard.hideInput = true;

            createButton.onClick.AddListener(CreateGame);
            joinButton.onClick.AddListener(JoinGame);

            codeInput.onValidateInput += (input, charIndex, addedChar) => { return Char.ToUpper(addedChar); };
        }

        private void ToUppercase(string str)
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (codeText.text != codeText.text.ToUpper())
            {
                codeText.text = codeText.text.ToUpper();
            }
        }

        private void CreateGame()
        {
            if (nameText.text == "")
            {
                return;
            }

            RestAPICaller.Instance.CreateGame(nameText.text);
        }

        private void JoinGame()
        {
            if (nameText.text == "" || codeText.text == "")
            {
                return;
            }

            RestAPICaller.Instance.JoinGame(nameText.text, codeText.text);
        }
    }
}
