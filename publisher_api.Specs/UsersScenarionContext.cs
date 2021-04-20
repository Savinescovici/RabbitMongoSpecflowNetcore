using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using publisher_api.Mongo;
using publisher_api.Mongo.Models;
using publisher_api.Services;

namespace publisher_api.Specs
{
    public class UsersScenarionContext
    {
        public IMongoService MongoService { get; private set; }
        public IUsersService UserService { get; private set; }
        public IMongoRepository<User> Repository { get; private set; }

        public UsersScenarionContext()
        {
            Repository = Substitute.For<IMongoRepository<User>>();
            MongoService = Substitute.For<IMongoService>();
            MongoService.GetRepository<User>().Returns(Repository);
            UserService = new UsersService(MongoService);
        }
    }
}
