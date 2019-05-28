using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public interface IQuestion
    {
        // Returns the Question text
        string GetQuestionText();

        // Checks if 'answer' is correct, if yes -> returns the answer, if no -> returns null
        string CheckAnswer(string answer);

        // Returns how many answers that remain
        int AnswersRemainingCount();

        // Removes the answer from the list of remaining answers
        void RemoveAnswer(string toRemove);
    }
}
