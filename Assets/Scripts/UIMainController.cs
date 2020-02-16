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
       
        public Canvas mainCanvas;

        [Space]
        public GameObject modalPrefab;

        [Space]
        [Header("Bottom Buttons")]
        public Button menuButton;
        public Button revealAnswersButton;
        public Button skipButton;

        private List<NewAnswer> uiAnswerRef;

        // Start is called before the first frame update
        void Start()
        {
            uiAnswerRef = new List<NewAnswer>();

            QuestionController.Instance.AnswerDiscovered += RevealAnswer;
            QuestionController.Instance.QuestionSet += SetCurrentQuestion;

            Player me = PlayerController.Instance.allPlayers.Find(player => player.IsMe == true);
            me.OnActivePlayerChanged += IsActivePlayer;

            

            inputBox.onValueChanged.AddListener(InputFieldValueChanged);

            if (QuestionController.Instance.GetCurrentQuestion() != null)
            {
                SetCurrentQuestion(QuestionController.Instance.GetCurrentQuestion());
            }

            menuButton.onClick.AddListener(() => {
                GameObject go = Instantiate(modalPrefab, mainCanvas.transform);
                ModalWindow mw = go.GetComponent<ModalWindow>();

                mw.SetText("Do you want to change the question?");
                mw.OnAcceptButtonClicked += () => QuestionController.Instance.SetAndBroadcastRandomQuestion();

            });

            revealAnswersButton.onClick.AddListener(() => {
                GameObject go = Instantiate(modalPrefab, mainCanvas.transform);
                ModalWindow mw = go.GetComponent<ModalWindow>();

                mw.SetText("Do you want to reveal all answers?");
                mw.OnAcceptButtonClicked += () => QuestionController.Instance.RevealAllAnswersAndBroadcast();
            });

            if (!TcpController.isHost)
            {
                menuButton.gameObject.SetActive(false);
                revealAnswersButton.gameObject.SetActive(false);
            }

            skipButton.onClick.AddListener(() =>
            {
                GameObject go = Instantiate(modalPrefab, mainCanvas.transform);
                ModalWindow mw = go.GetComponent<ModalWindow>();

                mw.SetText("Do you want to surrender?");
                mw.OnAcceptButtonClicked += () => TcpController.Instance.SendSkip();
            });

            PlayerController.Instance.allPlayers.Find(player => player.IsMe).OnActivePlayerChanged += (isActive) =>
            {
                skipButton.interactable = isActive;

                if (isActive)
                {
                    Handheld.Vibrate();
                }
            };
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

            uiAnswerRef = new List<NewAnswer>();
            Debug.Log("qText = " + question.GetQuestionText());
            for (int i = 0; i < question.TotalAnswersCount(); i++)
            {
                GameObject go = Instantiate(answerPrefab, answersBox);
                NewAnswer na = go.GetComponent<NewAnswer>();
                na.SetNumber(i + 1);
                uiAnswerRef.Add(na); // Add reference to the created ui object
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
