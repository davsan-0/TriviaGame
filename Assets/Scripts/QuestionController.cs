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

        // Use this for initialization
        void Start()
        {
            /*
            Question q = new Question("Who is the current President of the United States?");
            q.AddAnswer("Donald Trump", "Trump", "The Donald");

            SetCurrentQuestion(q);*/

            Question q = new Question("Name a United States President");
            q.AddAnswer("Donald Trump", "Trump");
            q.AddAnswer("Barack Obama", "Obama");
            q.AddAnswer("Donald Trump", "Trump");
            q.AddAnswer("George Washington", "Washington");
            q.AddAnswer("Herbert Hoover", "Hoover");

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
