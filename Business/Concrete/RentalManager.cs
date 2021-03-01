using Business.Abstract;
using Business.Constants.Messages;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Concrete
{
    public class RentalManager: IRentalService
    {
        IRentalDal _rentalDal;

        public RentalManager(IRentalDal RentalDal)
        {
            _rentalDal = RentalDal;
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental entity)
        {
            var result = checkReturnDate(entity.CarId);

            if(result.Success == true)
            {
                _rentalDal.Add(entity);
                return new SuccessResult("Rental added succesfully");
            }
            else
            {
                return new ErrorResult("Car is not avaible");
            }
            
        }
        private IResult checkReturnDate(int carId)
        {
            var result = _rentalDal.GetAll(r => r.CarId == carId && r.ReturnDate == default(DateTime));
            if(result.Count > 0)
            {
                return new ErrorResult();
            }
            else
            {
                return new SuccessResult();
            }
        }


        public IResult Delete(Rental entity)
        {
            _rentalDal.Delete(entity);
            return new SuccessResult("Rental deleted successfully");
        }

        public IDataResult<List<Rental>> GetAll()
        {
            if (_rentalDal.GetAll().Count > 0)
            {
                return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll());
            }
            else
            {
                return new ErrorDataResult<List<Rental>>("No rental found");
            }
        }

        public IDataResult<Rental> GetById(int id)
        {
            if (_rentalDal.Get(b => b.Id == id) != null)                      // check
            {
                return new SuccessDataResult<Rental>(_rentalDal.Get(b => b.Id == id));
            }
            else
            {
                return new ErrorDataResult<Rental>("rental with given id is not found");
            }
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetails(Expression<Func<Rental, bool>> filter = null)
        {
            if (_rentalDal.GetRentalDetails(filter).Count > 0)
            {
                return new SuccessDataResult<List<RentalDetailDto>>(_rentalDal.GetRentalDetails(filter));
            }
            else
            {
                return new ErrorDataResult<List<RentalDetailDto>>("No rental found with this filter");
            }
        }

        public IResult Update(Rental entity)
        {
            _rentalDal.Update(entity);
            return new SuccessResult("rental updated!");
        }
    }
}
