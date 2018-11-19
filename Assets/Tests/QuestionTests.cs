using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace TriviaGame
{
    public class QuestionTests
    {
        public string questionText = "Vem är bäst?";

        Question question;

        public QuestionTests()
        {
            question = new Question(questionText);

            question.AddAnswer("David", "Davvo", "Dadi", "Dadalife");

            question.AddAnswer("Choklad", "Choco");
        }

        [Test]
        public void GetQuestionTextTest()
        {
            Assert.AreEqual(questionText, question.GetQuestionText());
        }

        [Test]
        public void CheckAnswerTest()
        {
            Assert.AreEqual("David", question.CheckAnswer("David"));
            Assert.AreEqual("David", question.CheckAnswer("Dadi"));
            Assert.AreEqual("Choklad", question.CheckAnswer("Choco"));
            Assert.AreEqual("David", question.CheckAnswer("Dadalife"));
            Assert.AreEqual("David", question.CheckAnswer("Davvo"));

            Assert.IsNull(question.CheckAnswer("InteDavid"));
        }

        [Test]
        public void TotalAnswersRemainingTest()
        {
            Assert.AreEqual(2, question.TotalAnswersRemaining());
        }

        [Test]
        public void RemoveAnswerTest()
        {
            question.AddAnswer("1", "2", "3");

            question.RemoveAnswer("2");

            Assert.IsNull(question.CheckAnswer("1"));
            Assert.IsNull(question.CheckAnswer("2"));
            Assert.IsNull(question.CheckAnswer("3"));
        }
    }
}
