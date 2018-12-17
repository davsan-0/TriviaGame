using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnswer : MonoBehaviour
{
    public bool IsAnswered { get { return isAnswered; } }
    public GameObject answeredGo;
    public GameObject unansweredGo;

    private bool isAnswered = false;
    private string answerString;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RevealAnswer(string answer)
    {
        answeredGo.GetComponentInChildren<Text>().text = answer;

        unansweredGo.SetActive(false);
        answeredGo.SetActive(true);
    }
}
