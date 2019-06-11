using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class QuestionID : IEquatable<QuestionID>
    {
        private string id;

        /*public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            if (!(value is QuestionID))
            {
                throw new ArgumentException("Argument must be QuestionID");
            }
            
            return this.id.CompareTo(((QuestionID)value).ToString());
        }*/
        /*public int CompareTo(object value)
        {
            Debug.Log("HEJ3");
            return this.CompareTo(value as QuestionID);
        }

        public int CompareTo(QuestionID value)
        {
            Debug.Log("HEJ4");
            return this.id.CompareTo(value.ToString());
        }*/

        public static bool operator ==(QuestionID obj1, QuestionID obj2)
        {
            return String.Equals(obj1.ToString(), obj2.ToString());
        }

        // this is second one '!='
        public static bool operator !=(QuestionID obj1, QuestionID obj2)
        {
            return !String.Equals(obj1.ToString(), obj2.ToString());
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as QuestionID);
        }

        public bool Equals(QuestionID other)
        {
            return this.id.Equals(other.ToString());
        }

        public override string ToString()
        {
            return id;
        }

        public new static bool Equals(object obj1, object obj2)
        {
            return true;
        }

        public static bool Equals(QuestionID obj1, QuestionID obj2)
        {
            return true;
        }
        

        // 0 parameter constructor required for DynamoDB
        public QuestionID() { }

        public QuestionID(string id)
        {
            this.id = id;
        }
    }
}
