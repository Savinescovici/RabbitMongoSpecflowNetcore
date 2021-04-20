using publisher_api.Dto;
using publisher_api.Services;
using System;
using Moq;
using System.Linq;
using TechTalk.SpecFlow;
using publisher_api.Mongo;
using publisher_api.Mongo.Models;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using TechTalk.SpecFlow.Assist;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Driver;

namespace publisher_api.Specs.Featuers
{
    [Binding]
    public class UsersFeatureSteps
    {
        private UserDto _userDto;
        private User _userResult;
        private List<User> _usersCollection;
        private Guid _deleteUserId;
        private User _getDeletedUserResult;

        private UsersScenarionContext _context;

        public UsersFeatureSteps(UsersScenarionContext context)
        {
            _context = context;
        }

        [Given(@"users with email and name")]
        public void GivenEmailAndName(IEnumerable<UserDto> users)
        {
            _userDto = users.First();
        }
        
        [When(@"calling create user")]
        public async Task WhenCallingCreateUser()
        {
            _context.Repository.CreateAsync(Arg.Any<User>()).Returns(new User { Email = _userDto.Email, UserName = _userDto.Name });
            _userResult = await _context.UserService.AddUser(_userDto);
        }
        
        [Then(@"new user should be created")]
        public void ThenNewUserShouldBeCreated()
        {
            Assert.Equal(_userDto.Name, _userResult.UserName);
            Assert.Equal(_userDto.Email, _userResult.Email);
        }

        [Given(@"collection of users")]
        public void GivenCollectionOfUsers(Table table)
        {
            _usersCollection = table.CreateSet<User>().ToList();
        }

        [Given(@"user with id as (.*)")]
        public void GivenUserWithIdAs(Guid userId)
        {
            _deleteUserId = userId;
        }


        [When(@"calling delete user")]
        public async Task WhenCallingDeleteUser()
        {
            _context.Repository.When(x => x.DeleteAsync(Arg.Any<FilterDefinition<User>>())).Do(x =>
            {
                var user = _usersCollection.FirstOrDefault(u => u.UserId == _deleteUserId);
                _usersCollection.Remove(user);
            });
            await _context.UserService.Delete(_deleteUserId);

        }

        [When(@"calling get user with same id")]
        public async Task WhenCallingGetUserWithSameId()
        {
            _context.Repository.FindAsync(Arg.Any<FilterDefinition<User>>()).Returns((User)null);
            _getDeletedUserResult = await _context.UserService.GetUser(_deleteUserId);
        }

        [Then(@"user should not be found")]
        public void ThenUserShouldNotBeFound()
        {
            Assert.Null(_getDeletedUserResult);
        }

        [Then(@"delete user should have been called")]
        public async Task ThenDeleteUserShouldHaveBeenCalled()
        {
            await _context.Repository.Received(1).DeleteAsync(Arg.Any<FilterDefinition<User>>());
        }

    }
}
