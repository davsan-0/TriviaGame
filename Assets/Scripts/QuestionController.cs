using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TriviaGame
{
    public class QuestionController : MonoBehaviour
    {
        public event Action<Question> QuestionSet;
        public event Action<TcpController.AnswerStruct> AnswerDiscovered;

        private Question currentQuestion;

        

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
            Debug.Log("SetQuestion subscribed");
            TcpController.Instance.AnswerReceived += TcpCheckAnswer;
            //uiAnswerRef = new List<UIAnswer>();
        }

        private void SetQuestion(Question question)
        {
            Debug.Log("New Question set");
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

        private void TcpCheckAnswer(TcpController.AnswerStruct answer)
        {
            CheckAnswer(answer);
        }

        public bool CheckAnswer(TcpController.AnswerStruct answer)
        {
            if (currentQuestion == null)
            {
                Debug.Log("ERROR: currentQuestion is Null");
                return false;
            }

            string correctAnswer = currentQuestion.CheckAnswer(answer.answer);

            if (correctAnswer != null)
            {
                TcpController.AnswerStruct answerStruct;
                answerStruct.id = answer.id;
                answerStruct.answer = correctAnswer;
                currentQuestion.RemoveAnswer(correctAnswer);
                AnswerDiscovered?.Invoke(answerStruct);
                return true;
            }
            return false;
        }

        public bool CheckAnswerAndBroadcast(string answer)
        {
            TcpController.AnswerStruct answerStruct;
            answerStruct.id = PlayerController.Instance.players.Find(player => player.IsMe == true).Id;
            answerStruct.answer = answer;

            bool correct = CheckAnswer(answerStruct);

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
