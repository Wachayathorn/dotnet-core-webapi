using System;
using System.Collections.Generic;
using System.Linq;
using webapi.Entities;

namespace webapi.Repository
{
    public interface IUserRepository
    {
        User GetUserById(Guid id);
        IEnumerable<User> GetUsers();
        User AddUser(User data);
        User GetUserByUsername(string username);
        User UpdateUser(User data);
        User DeleteUser(User data);
    }

    public class UserRepository : IUserRepository
    {
        private readonly List<User> users = new()
        {
            new User { Id = Guid.NewGuid(), Firstname = "Alex", Lastname = "McKlen", CreateDate = new DateTime(), Username = "alex", Password = "P@ssw0rd" },
            new User { Id = Guid.NewGuid(), Firstname = "Mike", Lastname = "McKlen", CreateDate = new DateTime(), Username = "mike", Password = "P@ssw0rd" },
        };

        public IEnumerable<User> GetUsers()
        {
            return users;
        }

        public User GetUserById(Guid id)
        {
            return users.Where(user => user.Id == id).SingleOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            return users.Where(user => user.Username == username).SingleOrDefault();
        }

        public User AddUser(User data)
        {
            users.Add(data);
            return data;
        }

        public User UpdateUser(User data)
        {
            var user = this.GetUserById(data.Id);
            user.Firstname = data.Firstname;
            user.Lastname = data.Lastname;
            user.Username = data.Username;
            user.Password = data.Password;
            user.UpdateDate = data.UpdateDate;
            this.DeleteUser(user);
            users.Add(user);
            return user;
        }

        public User DeleteUser(User data)
        {
            users.Remove(data);
            return data;
        }
    }
}