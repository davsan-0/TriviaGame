using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class AnswerConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            List<Answer> answerList = new List<Answer>();

            Primitive primitive = entry as Primitive;

            if (primitive == null || !(primitive.Value is String) || string.IsNullOrEmpty((string)primitive.Value))
                throw new ArgumentOutOfRangeException();

            string[] answerGroup = ((string)(primitive.Value)).Split(new string[] { "|" }, StringSplitOptions.None);

            foreach (string answer in answerGroup)
            {
                string[] allPermutations = ((string)(answer)).Split(new string[] { ";" }, StringSplitOptions.None);
                Answer finishedAnswer = new Answer(new List<string>(allPermutations));
                answerList.Add(finishedAnswer);
            }


            return answerList;
        }

        public DynamoDBEntry ToEntry(object value)
        {
            List<Answer> answerList = value as List<Answer>;
            if (answerList == null) throw new ArgumentOutOfRangeException();

            string data = "";

            foreach (Answer answer in answerList)
            {
                foreach (string answerPermutation in answer)
                {
                    data += answerPermutation + ";";
                }
                data = data.Remove(data.Length - 1);
                data += "|";
            }
            if (data.Length > 0)
                data = data.Remove(data.Length - 1);

            DynamoDBEntry entry = new Primitive
            {
                Value = data
            };
            return entry;
        }
    }
}
