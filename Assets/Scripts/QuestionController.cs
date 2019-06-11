﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace TriviaGame
{
    public class QuestionController : MonoBehaviour
    {
        public TextMeshProUGUI questionText;
        public TMP_InputField inputBox;
        public GameObject answerPrefab;
        public Transform answersBox;
        public AudioSource correctAnswerAudio;

        private Question currentQuestion;

        //[SyncVar(hook = "SetCurrentQuestionFromString")]
        public string questionAsString;

        private List<UIAnswer> uiAnswerRef;

        // TEMP
        private MockQuestionList qList;

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

            uiAnswerRef = new List<UIAnswer>();
        }

        void Start()
        {
            /*if (isServer)
            {
                qList = new MockQuestionList();
                var q = qList.GetQuestion();
                SetCurrentQuestion(q);
            }*/
            qList = new MockQuestionList();
            Debug.Log(QuestionToJson(qList.GetQuestion()));
        }

        void SetCurrentQuestionFromString(string question)
        {
            /*if (!isServer)
            {
                SetCurrentQuestion(JsonToQuestion(question));
            }

            this.questionAsString = question;*/
        }

        public void SetCurrentQuestion(Question question)
        {
            if (currentQuestion == null || currentQuestion.id != question.id)
            {
                currentQuestion = question;

                for (int i = 0; i < currentQuestion.TotalAnswersCount(); i++)
                {
                    var go = (GameObject)Instantiate(answerPrefab, answersBox);
                    uiAnswerRef.Add(go.GetComponent<UIAnswer>()); // Add reference to the created ui object
                }

                /*foreach (string answer in question.EnteredAnswersList)
                {
                    RevealAnswer(answer);
                }
                */

                questionText.text = currentQuestion.GetQuestionText();
            }

            // Update the SyncVar for clients
           /* if (isServer)
            {
                Debug.Log("Server");
                questionAsString = QuestionToJson(question);
            }*/
        }

        public bool CheckAnswer(string answer)
        {
            if (currentQuestion == null)
            {
                Debug.Log("ERROR: currentQuestion is Null");
                return false;
            }

            string correctAnswer = currentQuestion.CheckAnswer(answer);

            if (correctAnswer != null && uiAnswerRef.Count > 0)
            {
                return true;
            }
            return false;
        }

        public void RevealAnswer(string answer)
        {
            Debug.Log("Correct answer: " + answer);
            uiAnswerRef[0].RevealAnswer(answer);
            uiAnswerRef.RemoveAt(0);

            currentQuestion.RemoveAnswer(answer);
        }

       // [ClientRpc]
        public void RpcRevealAnswer(string answer)
        {
            string correctAnswer = currentQuestion.CheckAnswer(answer);
            if (correctAnswer == null) return;

            Debug.Log("Correct answer: " + correctAnswer);

            correctAnswerAudio.Play();

            uiAnswerRef[0].RevealAnswer(correctAnswer);
            uiAnswerRef.RemoveAt(0);

            currentQuestion.RemoveAnswer(answer);

            /*if (isServer)
                questionAsString = QuestionToJson(currentQuestion);*/
        }

        /*public void CmdRevealAnswer(string answer)
        {
            RpcRevealAnswer(answer);
        }*/

        

        public string QuestionToJson(Question question)
        {
            var qStruct = new QuestionStruct();
            qStruct.questionId = question.id.ToString();
            qStruct.questionText = question.questionText;
            qStruct.category = question.category.ToString();
            qStruct.answerList = question.AnswersAsString();
            //qStruct.answerList = JsonUtility.ToJson(question.answerList);
            qStruct.enteredAnswers = question.EnteredAnswersAsString();

            return JsonUtility.ToJson(qStruct);
        }

        public Question JsonToQuestion(string json)
        {
            var qStruct = JsonUtility.FromJson<QuestionStruct>(json);

            Question question = new Question();
            question.id = qStruct.questionId;
            question.questionText = qStruct.questionText;
            question.category = (Category)Enum.Parse(typeof(Category), qStruct.category);
            question.SetAnswersFromString(qStruct.answerList);
            question.SetEnteredAnswersFromString(qStruct.enteredAnswers);

            return question;
        }

        [Serializable]
        private struct QuestionStruct
        {
            public string questionId;
            public string questionText;
            public string category;
            public string answerList;
            public string enteredAnswers;
        }

        /*public override void OnStartClient()
        {
            base.OnStartClient();

            SetCurrentQuestionFromString(questionAsString);
        }*/
    }
}
