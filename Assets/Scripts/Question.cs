using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;

namespace TriviaGame
{
    public enum Category { All }

    [DynamoDBTable("TriviaGame")]
    public class Question : IQuestion
    {
        /*[DynamoDBHashKey]
        private QuestionID id;
        [DynamoDBProperty]
        private string questionText;
        [DynamoDBProperty]
        private List<Answer> answerList;*/

        [DynamoDBHashKey("QuestionID", typeof(QuestionIDConverter))]
        public QuestionID Id { get; set; }
        [DynamoDBProperty]
        public Category Category { get; set; }
        [DynamoDBProperty]
        public string QuestionText { get; set; }
        [DynamoDBProperty(typeof(AnswerConverter))]
        public List<Answer> AnswerList { get; set; }

        // 0 parameter constructor required for DynamoDB
        public Question() { }

        public Question(string questionText)
        {
            this.QuestionText = questionText;

            AnswerList = new List<Answer>();
        }

        public void AddAnswer(params string[] answerPermutations)
        {
            List<string> lst = new List<string>(answerPermutations);

            Answer answerEntry = new Answer(lst);
            
            AnswerList.Add(answerEntry);
        }

        public void AddAnswer(List<string> answerPermutations)
        {
            Answer answerEntry = new Answer(answerPermutations);

            AnswerList.Add(answerEntry);
        }

        public string GetQuestionText()
        {
            return QuestionText;
        }

        public string CheckAnswer(string answer)
        {
            foreach (Answer answerEntry in AnswerList)
            {
                string checkedAnswer = answerEntry.CheckAnswer(answer);

                if (checkedAnswer != null)
                {
                    return checkedAnswer;
                }
            }

            return null;
        }

        public int TotalAnswersRemaining()
        {
            return AnswerList.Count;
        }

        public void RemoveAnswer(string toRemove)
        {
            foreach (Answer answerEntry in AnswerList)
            {
                string checkedAnswer = answerEntry.CheckAnswer(toRemove);

                if (checkedAnswer != null)
                {
                    AnswerList.Remove(answerEntry);
                    return;
                }
            }
        }

        // Represents one correct answer, but which can have many different correct inputs, i.e "Barack Obama" and "Obama" both being correct
        public class Answer : IEnumerable
        {
            [DynamoDBProperty]
            public List<string> answerPermutations;

            public Answer(string answer)
            {
                this.answerPermutations = new List<string>();

                answerPermutations.Add(answer);
            }

            public Answer(List<string> answerPermutations)
            {
                this.answerPermutations = answerPermutations;
            }

            // Returns answer if correct, null otherwise
            public string CheckAnswer(string answer)
            {
                bool answerExists = answerPermutations.Exists(s => s.Equals(answer, StringComparison.OrdinalIgnoreCase));

                if (answerExists)
                {
                    return answerPermutations[0];
                }

                return null;
            }

            public IEnumerator GetEnumerator()
            {
                return answerPermutations.GetEnumerator();
            }
        }

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
    
}
