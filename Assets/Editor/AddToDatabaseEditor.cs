using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace TriviaGame
{
    [CustomEditor(typeof(AddToDatabase))]
    [CanEditMultipleObjects]
    public class AddToDatabaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AddToDatabase myScript = (AddToDatabase)target;

            GUILayout.Space(10f);

            if (GUILayout.Button("Insert in Database"))
            {
                myScript.InsertInDatabase();
            }
        }

    }
}
