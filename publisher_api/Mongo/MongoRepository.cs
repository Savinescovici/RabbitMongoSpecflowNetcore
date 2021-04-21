using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace publisher_api.Mongo
{
    public interface IMongoRepository<T> where T: IEntity
    {
        Task<T> CreateAsync(T entity);
        Task<List<T>> GetListAsync(FilterDefinition<T> filter, SortDefinition<T> sort = default, CancellationToken token = default);
        Task<T> FindAsync(FilterDefinition<T> filter, CancellationToken token = default);
        Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);
        Task DeleteAsync(FilterDefinition<T> filter);
    }

    public class MongoRepository<T> : IMongoRepository<T> where T: IEntity
    {
        private IMongoCollection<T> _collection;
        public MongoRepository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<List<T>> GetListAsync(FilterDefinition<T> filter, SortDefinition<T> sort = default, CancellationToken token = default)
        {
            return await (await _collection.FindAsync(filter, new FindOptions<T, T> { Sort = sort }, token)).ToListAsync();
        }

        public async Task<T> FindAsync(FilterDefinition<T> filter, CancellationToken token = default)
        {
            return await (await _collection.FindAsync(filter, null, token)).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(FilterDefinition<T> filter)
        {
            await _collection.DeleteOneAsync(filter);
        }

    }
}
