using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TriviaGame
{
    public class QuestionController : MonoBehaviour
    {
        public event Action<Question> QuestionSet;
        public event Action<string> AnswerDiscovered;

        private Question currentQuestion;

        public string questionAsString;

        

        public List<Question> questionList;

        //  Singleton
        private static QuestionController _instance;
        public static QuestionController Instance
        {
            get { if (_instance == null)
                {
                    var go = new GameObject("QuestionController");
                    DontDestroyOnLoad(go);
                    var component = go.AddComponent<QuestionController>();
                    _instance = component;
                }
                return _instance;
            }
        }

        // Use this for initialization
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

            TcpController.Instance.QuestionReceived += SetQuestion;
            TcpController.Instance.AnswerReceived += TcpCheckAnswer;
            //uiAnswerRef = new List<UIAnswer>();
        }

        private void SetQuestion(Question question)
        {
            currentQuestion = question;
            QuestionSet?.Invoke(currentQuestion);
        }

        public void SetAndBroadcastRandomQuestion()
        {
            Debug.Log("SetAndBroadcastRandomQuestion");
            Question question = questionList[0];
            questionList.RemoveAt(0);

            SetQuestion(question);

            TcpController.Instance.SendQuestion(question);
        }

        void Start()
        {

        }

        /*void SetCurrentQuestionFromString(string question)
        {
            if (!isServer)
            {
                SetCurrentQuestion(JsonToQuestion(question));
            }

            this.questionAsString = question;
        }*/

        private void TcpCheckAnswer(string answer)
        {
            CheckAnswer(answer);
        }

        public bool CheckAnswer(string answer)
        {
            if (currentQuestion == null)
            {
                Debug.Log("ERROR: currentQuestion is Null");
                return false;
            }

            string correctAnswer = currentQuestion.CheckAnswer(answer);

            if (correctAnswer != null)
            {
                AnswerDiscovered?.Invoke(correctAnswer);
                return true;
            }
            return false;
        }

        public bool CheckAnswerAndBroadcast(string answer)
        {
            bool correct = CheckAnswer(answer);

            if (correct)
            {
                TcpController.Instance.SendAnswer(answer);
            }

            return correct;
        }

        public Question GetCurrentQuestion()
        {
            return currentQuestion;
        }
    }
}
