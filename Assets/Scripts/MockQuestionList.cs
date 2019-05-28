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

            q = new Question("Name US States");
            q.Id = new QuestionID(Uuid());
            q.AddAnswer("Alabama");
            q.AddAnswer("Alaska");
            q.AddAnswer("Arizona");
            q.AddAnswer("Arkansas");
            q.AddAnswer("California");
            q.AddAnswer("Colorado");
            q.AddAnswer("Connecticut");
            q.AddAnswer("Delaware");
            q.AddAnswer("Florida");
            q.AddAnswer("Georgia");
            q.AddAnswer("Hawaii");
            q.AddAnswer("Idaho");
            q.AddAnswer("Illinois");
            q.AddAnswer("Indiana");
            q.AddAnswer("Iowa");
            q.AddAnswer("Kansas");
            q.AddAnswer("Kentucky");
            q.AddAnswer("Louisiana");
            q.AddAnswer("Maine");
            q.AddAnswer("Maryland");
            q.AddAnswer("Massachusetts");
            q.AddAnswer("Michigan");
            q.AddAnswer("Minnesota");
            q.AddAnswer("Mississippi");
            q.AddAnswer("Missouri");
            q.AddAnswer("Montana");
            q.AddAnswer("Nebraska");
            q.AddAnswer("Nevada");
            q.AddAnswer("New Hampshire");
            q.AddAnswer("New Jersey");
            q.AddAnswer("New Mexico");
            q.AddAnswer("New York");
            q.AddAnswer("North Carolina");
            q.AddAnswer("North Dakota");
            q.AddAnswer("Ohio");
            q.AddAnswer("Oklahoma");
            q.AddAnswer("Oregon");
            q.AddAnswer("Pennsylvania");
            q.AddAnswer("Rhode Island");
            q.AddAnswer("South Carolina");
            q.AddAnswer("South Dakota");
            q.AddAnswer("Tennessee");
            q.AddAnswer("Texas");
            q.AddAnswer("Utah");
            q.AddAnswer("Vermont");
            q.AddAnswer("Virginia");
            q.AddAnswer("Washington");
            q.AddAnswer("West Virginia");
            q.AddAnswer("Wisconsin");
            q.AddAnswer("Wyoming");
            questionList.Add(q);

            q = new Question("Name Countries in Europe");
            q.Id = new QuestionID(Uuid());
            q.AddAnswer("Albania");
            q.AddAnswer("Andorra");
            q.AddAnswer("Armenia");
            q.AddAnswer("Austria");
            q.AddAnswer("Azerbaijan");
            q.AddAnswer("Belarus");
            q.AddAnswer("Belgium");
            q.AddAnswer("Bosnia and Herzegovina", "Bosnia");
            q.AddAnswer("Bulgaria");
            q.AddAnswer("Croatia");
            q.AddAnswer("Cyprus");
            q.AddAnswer("Czech Republic", "Czech");
            q.AddAnswer("Denmark");
            q.AddAnswer("Estonia");
            q.AddAnswer("Finland");
            q.AddAnswer("France");
            q.AddAnswer("Georgia");
            q.AddAnswer("Germany");
            q.AddAnswer("Greece");
            q.AddAnswer("Hungary");
            q.AddAnswer("Iceland");
            q.AddAnswer("Ireland");
            q.AddAnswer("Italy");
            q.AddAnswer("Kazakhstan");
            q.AddAnswer("Kosovo");
            q.AddAnswer("Latvia");
            q.AddAnswer("Liechtenstein");
            q.AddAnswer("Lithuania");
            q.AddAnswer("Luxembourg");
            q.AddAnswer("Macedonia", "FYROM");
            q.AddAnswer("Malta");
            q.AddAnswer("Moldova");
            q.AddAnswer("Monaco");
            q.AddAnswer("Montenegro");
            q.AddAnswer("Netherlands", "Holland");
            q.AddAnswer("Norway");
            q.AddAnswer("Poland");
            q.AddAnswer("Portugal");
            q.AddAnswer("Romania");
            q.AddAnswer("Russia");
            q.AddAnswer("San Marino");
            q.AddAnswer("Serbia");
            q.AddAnswer("Slovakia");
            q.AddAnswer("Slovenia");
            q.AddAnswer("Spain");
            q.AddAnswer("Sweden");
            q.AddAnswer("Switzerland");
            q.AddAnswer("Turkey");
            q.AddAnswer("Ukraine");
            q.AddAnswer("United Kingdom", "UK");
            q.AddAnswer("Vatican City", "Holy See");
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
