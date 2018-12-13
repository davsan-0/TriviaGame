using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;

namespace TriviaGame
{
    public enum Category
    {
        Science_and_Nature,
        Entertainment,
        Geography,
        History,
        Arts_and_Literature,
        Sports_and_Leisure
    }

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
            //Category = new HashSet<Category>();
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
    }
    
}
