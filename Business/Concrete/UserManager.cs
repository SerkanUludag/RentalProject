using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _UserDal;

        public UserManager(IUserDal UserDal)
        {
            _UserDal = UserDal;
        }

        [ValidationAspect(typeof(UserValidator))]
        public IResult Add(User entity)
        {
            _UserDal.Add(entity);
            return new SuccessResult("User added succesfully");

        }

        public IResult Delete(User entity)
        {
            _UserDal.Delete(entity);
            return new SuccessResult("User deleted successfully");
        }

        public IDataResult<List<User>> GetAll()
        {
            if (_UserDal.GetAll().Count > 0)
            {
                return new SuccessDataResult<List<User>>(_UserDal.GetAll());
            }
            else
            {
                return new ErrorDataResult<List<User>>("No car found");
            }
        }

        public IDataResult<User> GetById(int id)
        {
            if (_UserDal.Get(b => b.Id == id) != null)                      // check
            {
                return new SuccessDataResult<User>(_UserDal.Get(b => b.Id == id));
            }
            else
            {
                return new ErrorDataResult<User>("Car with given id is not found");
            }
        }

        public IResult Update(User entity)
        {
            _UserDal.Update(entity);
            return new SuccessResult("Car updated!");
        }
    }
}
