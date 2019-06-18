using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TriviaGame
{
    public class LobbyPlayerText : MonoBehaviour
    {

        public Text playerText;
        //public Color playerColor;

        // Start is called before the first frame update
        void Awake()
        {

        }

        public void ChangeName(string name)
        {
            playerText.text = name;
        }

        public void ChangeColor(Color color)
        {
            playerText.color = color;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}