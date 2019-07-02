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
        public Button menuButton;
        public Canvas mainCanvas;

        [Space]
        public GameObject modalPrefab;

        private List<UIAnswer> uiAnswerRef;

        // Start is called before the first frame update
        void Start()
        {
            uiAnswerRef = new List<UIAnswer>();

            QuestionController.Instance.AnswerDiscovered += RevealAnswer;
            QuestionController.Instance.QuestionSet += SetCurrentQuestion;

            Player me = PlayerController.Instance.players.Find(player => player.IsMe == true);
            me.OnActivePlayerChanged += IsActivePlayer;

            if (!TcpController.isHost)
            {
                menuButton.gameObject.SetActive(false);
            }

            inputBox.onValueChanged.AddListener(InputFieldValueChanged);

            if (QuestionController.Instance.GetCurrentQuestion() != null)
            {
                SetCurrentQuestion(QuestionController.Instance.GetCurrentQuestion());
            }

            menuButton.onClick.AddListener(() => {
                GameObject go = Instantiate(modalPrefab, mainCanvas.transform);
                ModalWindow mw = go.GetComponent<ModalWindow>();

                mw.SetText("Are you sure you want to change the Question?");
                mw.OnAcceptButtonClicked += () => QuestionController.Instance.SetAndBroadcastRandomQuestion();

            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void IsActivePlayer(bool value)
        {
            
            if (value)
            {
                inputBox.interactable = value;
                inputBox.ActivateInputField();
            } else
            {
                inputBox.DeactivateInputField(true);
                inputBox.interactable = value;
            }
        }

        public void SetCurrentQuestion(Question question)
        {
            Debug.Log("Instantiating question stuff");
            if (question == null)
            {
                Debug.LogError("Question is Null");
                return;
            }

            foreach (Transform child in answersBox)
            {
                Destroy(child.gameObject);
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

        public void RevealAnswer(TcpController.AnswerStruct answer)
        {
            Debug.Log("Correct answer: " + answer.answer);
            uiAnswerRef[0].RevealAnswer(answer.answer);
            uiAnswerRef.RemoveAt(0);

            correctAnswerAudio.Play();
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
