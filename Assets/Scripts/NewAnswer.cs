using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TriviaGame
{
    public class NewAnswer : MonoBehaviour
    {
        //public Sprite unsolvedSprite;
        public Sprite solvedSprite;

        [Space]
        public TextMeshProUGUI _text;
        public Image _image;

        public bool IsAnswered { get; }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RevealAnswer(string answer)
        {
            _text.text = answer;
            _image.sprite = solvedSprite;
        }

        public void SetNumber(int i)
        {
            _text.text = i.ToString();
        }
    }
}
