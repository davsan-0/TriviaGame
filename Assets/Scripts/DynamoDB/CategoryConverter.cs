using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TriviaGame
{
    public class CategoryConverter : IPropertyConverter
    {
        
        public DynamoDBEntry ToEntry(object value)
        {
            HashSet<Category> categories = value as HashSet<Category>;
            if (categories == null) return "";

            string data = "";

            foreach (Category category in categories)
            {
                data += category.ToString() + ";";
            }
            if (data.Length > 0)
                data = data.Remove(data.Length - 1);

            DynamoDBEntry entry = new Primitive
            {
                Value = data
            };
            return entry;
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            HashSet<Category> categorySet = new HashSet<Category>();

            Primitive primitive = entry as Primitive;

            if (primitive == null || !(primitive.Value is String) || string.IsNullOrEmpty((string)primitive.Value))
                throw new ArgumentOutOfRangeException();

            string[] categoryArray = ((string)(primitive.Value)).Split(new string[] { ";" }, StringSplitOptions.None);

            foreach (string categoryString in categoryArray)
            {
                Category cat = (Category)Enum.Parse(typeof(Category), categoryString);
                categorySet.Add(cat);
            }

            return categorySet;
        }
    }
}
