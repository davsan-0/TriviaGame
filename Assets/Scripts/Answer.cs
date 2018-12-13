using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
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
}
