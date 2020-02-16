using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TriviaGame
{
    public class LobbyPlayerUI : MonoBehaviour
    {
        public GameObject playerPrefab;
        public Transform playerParent;
        public Text codeText;
        public Button startGameButton;

        private Dictionary<Player, GameObject> playersDict;

        // Start is called before the first frame update
        void Start()
        {
            playersDict = new Dictionary<Player, GameObject>();


            PlayerController.Instance.OnPlayerAdded += PlayerJoined;
            PlayerController.Instance.OnPlayerRemoved += PlayerLeft;
            TcpController.Instance.StartGame += () => SceneManager.LoadScene("MainScene", LoadSceneMode.Single);

            codeText.text = TcpController.ROOM_CODE;

            if (TcpController.isHost)
            {
                startGameButton.onClick.AddListener(StartGameClicked);
            } else
            {
                startGameButton.gameObject.SetActive(false);
            }
                
        }

        private void StartGameClicked()
        {
            RestAPICaller.Instance.GetQuestion(100, (List<Question> questions) =>
            {
                SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
                TcpController.Instance.SendStartGame();

                QuestionController.Instance.questionList = questions;

                QuestionController.Instance.SetAndBroadcastRandomQuestion();
            });
        }

        private void PlayerJoined(Player player)
        {
            GameObject go = Instantiate(playerPrefab, playerParent);
            LobbyPlayerText lpt = go.GetComponent<LobbyPlayerText>();

            lpt.ChangeName(player.Name);
            lpt.ChangeColor(player.Color);

            playersDict.Add(player, go);
            //PlayerController.Instance.AddPlayer(name);
        }

        private void PlayerLeft(Player player)
        {
            GameObject go;
            playersDict.TryGetValue(player, out go);
            playersDict.Remove(player);

            Destroy(go);
        }
    }
}
