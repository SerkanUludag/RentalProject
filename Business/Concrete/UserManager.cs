using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Security.Hashing;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

        [ValidationAspect(typeof(UserValidator))]
        public void Update(User user)
        {
            _userDal.Update(user);
        }

        public void UpdatePassword(string password, string email)
        {
            byte[] passwordHash, passwordSalt;
            var userToUpdate = GetByMail(email);
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            userToUpdate.PasswordHash = passwordHash;
            userToUpdate.PasswordSalt = passwordSalt;
            Update(userToUpdate);

        }
    }
}
