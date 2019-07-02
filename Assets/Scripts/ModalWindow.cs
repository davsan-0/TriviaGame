using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindow : MonoBehaviour
{
    public Button acceptButton;
    public Button cancelButton;

    public Text text;

    public Action OnAcceptButtonClicked;

    // Start is called before the first frame update
    void Start()
    {
        acceptButton.onClick.AddListener(() => {
            OnAcceptButtonClicked?.Invoke();
            Destroy(this.gameObject);
        });
        cancelButton.onClick.AddListener(() => Destroy(this.gameObject));
    }

    public void SetText(string text)
    {
        this.text.text = text; // BRA KOD!
    }
}
