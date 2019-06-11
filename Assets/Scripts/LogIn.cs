using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TriviaGame
{
    public class LogIn : MonoBehaviour
    {
        public Button joinButton;
        public Button createButton;
        [Space]
        public Text nameInput;
        public Text codeInput;


        // Start is called before the first frame update
        void Start()
        {
            createButton.onClick.AddListener(CreateGame);
            joinButton.onClick.AddListener(JoinGame);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CreateGame()
        {
            if (nameInput.text == "")
            {
                return;
            }

            RestAPICaller.Instance.CreateGame(nameInput.text);
        }

        private void JoinGame()
        {
            if (nameInput.text == "" || codeInput.text == "")
            {
                return;
            }

            RestAPICaller.Instance.JoinGame(nameInput.text, codeInput.text);
        }
    }
}
