using MongoDB.Driver;
using publisher_api.Mongo.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace publisher_api.Mongo
{
    public interface IMongoService
    {
        IMongoRepository<T> GetRepository<T>() where T:IEntity;
    }

    public class MongoService : IMongoService
    {
        private IMongoDatabase _database;
        public MongoService(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoRepository<T> GetRepository<T>() where T : IEntity
        {
            var collectionName = typeof(T).GetCustomAttribute<MongoCollection>()?.Name;
            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Class not bound to a mongodb collection");
            }
            return new MongoRepository<T>(_database.GetCollection<T>(collectionName));
        }
    }
}
