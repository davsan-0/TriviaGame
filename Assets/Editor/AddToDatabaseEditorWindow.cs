using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace TriviaGame
{
    public class AddToDatabaseEditorWindow : EditorWindow {

        string questionText;

        Category category;

        int answers;
        int i = 0;

        public List<string> answerList;

        [MenuItem("Window/AddToDatabase")]
        public static void ShowWindow()
        {
            GetWindow<AddToDatabaseEditorWindow>("Add To Database");
        }

        

        

        

	    // Use this for initialization
	    void Start () {
            answerList = new List<string>();
	    }
	
	    // Update is called once per frame
	    void Update () {
		
	    }

        void OnGUI()
        {
            GUILayout.Label("Data", EditorStyles.boldLabel);
            questionText = EditorGUILayout.TextField("Question Text", questionText);

            category = (Category)EditorGUILayout.EnumPopup("Category", category);
    

            if (GUILayout.Button("Upload"))
            {
                Debug.Log(answerList.ToString());
            }
            //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            //myBool = EditorGUILayout.Toggle("Toggle", myBool);
            //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            //EditorGUILayout.EndToggleGroup();
        }
    }
}
