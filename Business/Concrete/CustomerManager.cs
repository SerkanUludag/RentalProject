using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CustomerManager: ICustomerService
    {
        ICustomerDal _CustomerDal;

        public CustomerManager(ICustomerDal CustomerDal)
        {
            _CustomerDal = CustomerDal;
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IResult Add(Customer entity)
        {
            _CustomerDal.Add(entity);
            return new SuccessResult("Customer added succesfully");

        }

        public IResult Delete(Customer entity)
        {
            _CustomerDal.Delete(entity);
            return new SuccessResult("Customer deleted successfully");
        }

        public IDataResult<List<Customer>> GetAll()
        {
            if (_CustomerDal.GetAll().Count > 0)
            {
                return new SuccessDataResult<List<Customer>>(_CustomerDal.GetAll());
            }
            else
            {
                return new ErrorDataResult<List<Customer>>("No car found");
            }
        }

        public IDataResult<Customer> GetById(int id)
        {
            if (_CustomerDal.Get(b => b.UserId == id) != null)                      // check
            {
                return new SuccessDataResult<Customer>(_CustomerDal.Get(b => b.UserId == id));
            }
            else
            {
                return new ErrorDataResult<Customer>("Car with given id is not found");
            }
        }

        public IResult Update(Customer entity)
        {
            _CustomerDal.Update(entity);
            return new SuccessResult("Car updated!");
        }
    }
}
