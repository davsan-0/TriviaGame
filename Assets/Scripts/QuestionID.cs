using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class QuestionID : IComparable
    {
        private string id;

        public int CompareTo(object obj)
        {
            return this.id.CompareTo(obj.ToString());
        }

        public override string ToString()
        {
            return id;
        }

        // 0 parameter constructor required for DynamoDB
        public QuestionID() { }

        public QuestionID(string id)
        {
            this.id = id;
        }
    }
}
