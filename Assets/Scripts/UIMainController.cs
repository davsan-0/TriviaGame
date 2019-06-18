using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TriviaGame
{
    public class UIMainController : MonoBehaviour
    {
        public TextMeshProUGUI questionText;
        public TMP_InputField inputBox;
        public GameObject answerPrefab;
        public Transform answersBox;
        public AudioSource correctAnswerAudio;

        private List<UIAnswer> uiAnswerRef;

        // Start is called before the first frame update
        void Start()
        {
            uiAnswerRef = new List<UIAnswer>();

            QuestionController.Instance.AnswerDiscovered += RevealAnswer;
            QuestionController.Instance.QuestionSet += SetCurrentQuestion;

            inputBox.onValueChanged.AddListener(InputFieldValueChanged);

            if (QuestionController.Instance.GetCurrentQuestion() != null)
            {
                SetCurrentQuestion(QuestionController.Instance.GetCurrentQuestion());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetCurrentQuestion(Question question)
        {
            if (question == null)
            {
                Debug.LogError("Question is Null");
                return;
            }

            foreach (UIAnswer uiAnswer in uiAnswerRef)
            {
                Destroy(uiAnswer.gameObject);
            }

            uiAnswerRef = new List<UIAnswer>();
            Debug.Log("qText = " + question.GetQuestionText());
            for (int i = 0; i < question.TotalAnswersCount(); i++)
            {
                GameObject go = Instantiate(answerPrefab, answersBox);
                uiAnswerRef.Add(go.GetComponent<UIAnswer>()); // Add reference to the created ui object
            }

            /*foreach (string answer in question.EnteredAnswersList)
            {
                RevealAnswer(answer);
            }
            */

            questionText.text = question.GetQuestionText();
        }

        public void RevealAnswer(string answer)
        {
            Debug.Log("Correct answer: " + answer);
            uiAnswerRef[0].RevealAnswer(answer);
            uiAnswerRef.RemoveAt(0);

            //currentQuestion.RemoveAnswer(answer);
        }

        public void InputFieldValueChanged(string value)
        {
            bool correct = QuestionController.Instance.CheckAnswerAndBroadcast(value);

            if (correct)
            {
                inputBox.text = "";
            }
        }

    }
}
