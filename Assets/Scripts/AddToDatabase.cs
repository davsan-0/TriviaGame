using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace TriviaGame
{
    [ExecuteInEditMode]
    public class AddToDatabase : MonoBehaviour {

        [TextArea]
        public string questionText;

        public Category category;
    
        public List<string> answer;

        [Serializable]
        public class StringList
        {
            public List<string> answerPermutations;
        }

        public void InsertInDatabase()
        {
            Debug.Log("Inserted " + questionText);
        }
    }
}
