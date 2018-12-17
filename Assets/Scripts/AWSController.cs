using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class AWSController : MonoBehaviour
    {
        public string IdentityPoolId = "eu-central-1:553bfa5d-57b3-46ab-8860-a4e96448ce8c";
        public string CognitoPoolRegion = RegionEndpoint.EUCentral1.SystemName;
        public string DynamoRegion = RegionEndpoint.EUCentral1.SystemName;

        private RegionEndpoint _CognitoPoolRegion
        {
            get { return RegionEndpoint.GetBySystemName(CognitoPoolRegion); }
        }

        private RegionEndpoint _DynamoRegion
        {
            get { return RegionEndpoint.GetBySystemName(DynamoRegion); }
        }

        private static IAmazonDynamoDB _ddbClient;

        private AWSCredentials _credentials;

        private DynamoDBContext _context;

        private AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoPoolRegion);
                return _credentials;
            }
        }

        protected IAmazonDynamoDB Client
        {
            get
            {
                if (_ddbClient == null)
                {
                    _ddbClient = new AmazonDynamoDBClient(Credentials, _DynamoRegion);
                }

                return _ddbClient;
            }
        }

        protected DynamoDBContext Context
        {
            get
            {
                if (_context == null)
                    _context = new DynamoDBContext(Client);

                return _context;
            }
        }

        void Start()
        {
            UnityInitializer.AttachToGameObject(this.gameObject);
            AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
            /*
            Question q = new Question("Name a United States President");
            q.Id = new QuestionID("prez");
            //q.Id = "prez";
            q.Category = Category.History;
            q.AddAnswer("Donald Trump", "Trump");
            q.AddAnswer("Barack Obama", "Obama");
            q.AddAnswer("Gerald Ford", "Ford", "Ford Motors??", "Salle", "Rille");
            q.AddAnswer("John Quincy Adams");
            q.AddAnswer("George Washington", "Washington");
            q.AddAnswer("Herbert Hoover", "Hoover");

            Context.SaveAsync<Question>(q, (result) =>
            {
                if (result.Exception == null)
                {
                    Debug.Log("Question saved!");
                } else
                {
                    Debug.Log("Error: " + result.Exception.Message);
                }
            });*/
            /*Book myBook = new Book
            {
                Id = 1001,
                Title = "object persistence-AWS SDK for.NET SDK-Book 1001",
                ISBN = "111-1111111001",
                BookAuthors = new List<string> { "Author 1", "Author 2" },
            };

            // Save the book.
            Context.SaveAsync(myBook, (result) => {
                if (result.Exception == null)
                    Debug.Log("Book saved");
                else
                    Debug.Log("Book error: " + result.Exception.Message);
            });*/
        }
    }
    [DynamoDBTable("ProductCatalog")]
    public class Book
    {
        [DynamoDBHashKey]   // Hash key.
        public int Id { get; set; }
        [DynamoDBProperty]
        public string Title { get; set; }
        [DynamoDBProperty]
        public string ISBN { get; set; }
        [DynamoDBProperty("Authors")]    // Multi-valued (set type) attribute. 
        public List<string> BookAuthors { get; set; }
    }
}
