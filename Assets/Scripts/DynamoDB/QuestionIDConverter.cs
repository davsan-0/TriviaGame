using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class QuestionIDConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            DynamoDBEntry entry = new Primitive
            {
                Value = value.ToString()
            };

            return entry;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            return entry.AsString();
        }
    }
}
