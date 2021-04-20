Feature: UsersFeature

Scenario: Create user
	Given users with email and name
        | Email       | Name  |
        | savin@yahoo.com | savin |
        | alin@yahoo.com  | alin  |
	When calling create user
	Then new user should be created

Scenario: Remove user
    Given collection of users
      | UserId | Email | UserName            |
      | 12ef47b5-1d78-4308-9d00-12c1cb5aca78 | savin@yahoo.com | savin |
      | 6364198c-b01a-48b1-a54d-3559e18d425e | alin@yahoo.com  | alin |
	Given user with id as 12ef47b5-1d78-4308-9d00-12c1cb5aca78
	When calling delete user
	And calling get user with same id
    Then user should not be found
    And delete user should have been called