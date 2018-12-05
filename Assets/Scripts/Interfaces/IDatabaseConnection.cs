using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public interface IDatabaseConnection
    {
        IQuestion GetQuestion(string type, List<QuestionID> excludedQuestions = null);
    }
}
