using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TriviaGame
{
    public class QuestionController : MonoBehaviour
    {
        public TextMeshProUGUI questionText;
        public TMP_InputField inputBox;

        private IQuestion currentQuestion;

        // TEMP
        private MockQuestionList qList;

        // Use this for initialization
        void Start()
        {
            qList = new MockQuestionList();
            var q = qList.GetQuestion();

            SetCurrentQuestion(q);
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        public void SetCurrentQuestion(IQuestion question)
        {
            currentQuestion = question;

            questionText.text = currentQuestion.GetQuestionText();
        }

        public void InputFieldValueChanged(string value)
        {
            if (currentQuestion == null)
            {
                Debug.Log("ERROR: currentQuestion is Null");
                return;
            }

            string answer = currentQuestion.CheckAnswer(value);

            if (answer != null)
            {
                Debug.Log("Correct answer: " + answer);
            }
        }
    }
}
