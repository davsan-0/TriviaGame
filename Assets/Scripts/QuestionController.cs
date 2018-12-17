using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

namespace TriviaGame
{
    public class QuestionController : NetworkBehaviour
    {
        public TextMeshProUGUI questionText;
        public TMP_InputField inputBox;
        public GameObject answerPrefab;
        public Transform answersBox;

        private IQuestion currentQuestion;
        [SyncVar]
        private string questionAsString; // Used for syncing over Network

        private List<UIAnswer> uiAnswerRef;

        // TEMP
        private MockQuestionList qList;

        // Use this for initialization
        void Start()
        {
            qList = new MockQuestionList();
            var q = qList.GetQuestion();

            uiAnswerRef = new List<UIAnswer>();

            var q2 = JsonToQuestion(QuestionToJson(q));
            SetCurrentQuestion(q2);
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        public void SetCurrentQuestion(IQuestion question)
        {
            currentQuestion = question;

            for (int i = 0; i < currentQuestion.TotalAnswersRemaining(); i++)
            {
                var go = (GameObject)Instantiate(answerPrefab, answersBox);
                uiAnswerRef.Add(go.GetComponent<UIAnswer>()); // Add reference to the created ui object
            }

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

            if (answer != null && uiAnswerRef.Count > 0)
            {
                RpcRevealAnswer(answer);
                inputBox.text = ""; // Clear input after correct answer
            }
        }

        [ClientRpc]
        public void RpcRevealAnswer(string answer)
        {
            Debug.Log("Correct answer: " + answer);
            uiAnswerRef[0].RevealAnswer(answer);
            uiAnswerRef.RemoveAt(0);

            currentQuestion.RemoveAnswer(answer);
        }

        private string QuestionToJson(Question question)
        {
            var qStruct = new QuestionStruct();
            qStruct.questionId = question.Id.ToString();
            qStruct.questionText = question.QuestionText;
            qStruct.category = question.Category.ToString();
            qStruct.answerList = question.AnswersAsString();

            return JsonUtility.ToJson(qStruct);
        }

        private Question JsonToQuestion(string json)
        {
            var qStruct = JsonUtility.FromJson<QuestionStruct>(json);

            Question question = new Question();
            question.Id = new QuestionID(qStruct.questionId);
            question.QuestionText = qStruct.questionText;
            question.Category = (Category)Enum.Parse(typeof(Category), qStruct.category);
            question.SetAnswersFromString(qStruct.answerList);

            return question;
        }

        [Serializable]
        private struct QuestionStruct
        {
            public string questionId;
            public string questionText;
            public string category;
            public string answerList;
        }
    }
}
