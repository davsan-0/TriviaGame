using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace TriviaGame
{
    public class Question : IQuestion
    {
        private string questionText;
        private List<Answer> answerList;

        public Question(string questionText)
        {
            this.questionText = questionText;

            answerList = new List<Answer>();
        }

        public void AddAnswer(params string[] answerPermutations)
        {
            List<string> lst = new List<string>(answerPermutations);

            Answer answerEntry = new Answer(lst);

            answerList.Add(answerEntry);
        }

        public void AddAnswer(List<string> answerPermutations)
        {
            Answer answerEntry = new Answer(answerPermutations);

            answerList.Add(answerEntry);
        }

        public string GetQuestionText()
        {
            return questionText;
        }

        public string CheckAnswer(string answer)
        {
            foreach (Answer answerEntry in answerList)
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
            return answerList.Count;
        }

        public void RemoveAnswer(string toRemove)
        {
            foreach (Answer answerEntry in answerList)
            {
                string checkedAnswer = answerEntry.CheckAnswer(toRemove);

                if (checkedAnswer != null)
                {
                    answerList.Remove(answerEntry);
                    return;
                }
            }
        }

        // Represents one correct answer, but which can have many different correct inputs, i.e "Barack Obama" and "Obama" both being correct
        private class Answer
        {
            private List<string> answerPermutations;

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
        }
    }
}
