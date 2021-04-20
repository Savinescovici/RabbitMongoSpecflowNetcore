using System;

namespace publisher_api.Mongo.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoCollection : Attribute
    {
        public string Name { get; set; }
        public MongoCollection(string name)
        {
            this.Name = name;
        }
    }
}
