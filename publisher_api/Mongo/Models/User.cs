using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using publisher_api.Mongo.Attributes;
using System;

namespace publisher_api.Mongo.Models
{
    [MongoCollection("users")]
    public class User : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
