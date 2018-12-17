using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{

    public class MockQuestionList
    {
        public List<Question> questionList;

        private Question q;
        public MockQuestionList()
        {
            questionList = new List<Question>();

            q = new Question("Name Presidents of the United States");
            q.Id = new QuestionID(Uuid());
            q.AddAnswer("George Washington", "Washington");
            q.AddAnswer("John Adams", "Adams");  
            q.AddAnswer("Thomas Jefferson", "Jefferson");
            q.AddAnswer("James Madison", "Madison");
            q.AddAnswer("James Monroe", "Monroe");
            q.AddAnswer("John Quincy Adams", "Jon Quincy Adams", "Quincy Adams");
            q.AddAnswer("Andrew Jackson", "Jackson");
            q.AddAnswer("Martin Van Buren", "Van Buren");
            q.AddAnswer("William Henry Harrison", "Harrison");
            q.AddAnswer("John Tyler", "Tyler");
            q.AddAnswer("James K. Polk", "James K Polk", "Polk");
            q.AddAnswer("Zachary Taylor", "Taylor");
            q.AddAnswer("Millard Fillmore", "Fillmore");
            q.AddAnswer("Franklin Pierce", "Pierce");
            q.AddAnswer("James Buchanan", "Buchanan");
            q.AddAnswer("Abraham Lincoln", "Lincoln");
            q.AddAnswer("Andrew Johnson");
            q.AddAnswer("Ulysses S. Grant", "Ulysses S Grant", "Grant");
            q.AddAnswer("Rutherford B. Hayes", "Hayes");
            q.AddAnswer("James A. Garfield", "Garfield");
            q.AddAnswer("Chester A. Arthur", "Arthur");
            q.AddAnswer("Grover Cleveland", "Cleveland");
            q.AddAnswer("Benjamin Harrison", "Harrison");
            q.AddAnswer("William McKinley", "McKinley");
            q.AddAnswer("Theodore Roosevelt", "Teddy Roosevelt", "Ted Roosevelt", "Roosevelt");
            q.AddAnswer("William Howard Taft", "Taft");
            q.AddAnswer("Woodrow Wilson", "Wilson");
            q.AddAnswer("Warren G. Harding", "Harding");
            q.AddAnswer("Calvin Coolidge", "Coolidge");
            q.AddAnswer("Herbert Hoover", "Hoover");
            q.AddAnswer("Franklin D. Roosevelt", "FDR");
            q.AddAnswer("Harry S. Truman", "S. Truman", "S Truman", "Truman");
            q.AddAnswer("Dwight D. Eisenhower", "Eisenhower");
            q.AddAnswer("John F. Kennedy", "Kennedy");
            q.AddAnswer("Lyndon B. Johnson", "Johnson");
            q.AddAnswer("Richard Nixon", "Nixon");
            q.AddAnswer("Gerald Ford", "Ford");
            q.AddAnswer("Jimmy Carter", "Carter");
            q.AddAnswer("Ronald Reagan", "Reagan");
            q.AddAnswer("George H. W. Bush", "H W Bush", "H. W. Bush", "George Bush");
            q.AddAnswer("Bill Clinton", "Clinton");
            q.AddAnswer("George W. Bush", "W. Bush", "W Bush");
            q.AddAnswer("Barack Obama", "Obama");
            q.AddAnswer("Donald Trump", "Trump");
            questionList.Add(q);
        }

        public Question GetQuestion()
        {
            int i = Random.Range(0, questionList.Count);
            Question q = questionList[i];
            questionList.RemoveAt(i);

            return q;
        }

        private string Uuid()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
