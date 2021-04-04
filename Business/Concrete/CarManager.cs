using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
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
    public class CarManager : ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            this._carDal = carDal;
        }

        [ValidationAspect(typeof(CarValidator))]
        [CacheRemoveAspect("ICarService.Get")]
        public IResult Add(Car entity)
        {
            if(entity.Description.Length >= 2 && entity.DailyPrice > 0)
            {
                _carDal.Add(entity);
                return new SuccessResult("Succesfully added");
            }
            else
            {
                return new ErrorResult("New entity doesnt meet criteria!!");
            }
            
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetAll()
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll());
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetById(int id)
        {
            if (_carDal.GetAll(c => c.Id == id).Count > 0)
            {
                return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.Id == id));
            }
            else
            {
                return new ErrorDataResult<List<Car>>("No car found with this id");
            }

        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByBrandId(int id)
        {
            if (_carDal.GetAll(c => c.BrandId == id).Count > 0)
            {
                return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.BrandId == id));
            }
            else
            {
                return new ErrorDataResult<List<Car>>("No car found with this brand id");
            }

        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByColorId(int id)
        {
            if (_carDal.GetAll(c => c.ColorId == id).Count > 0)
            {
                return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.ColorId == id));
            }
            else
            {
                return new ErrorDataResult<List<Car>>("No car found with this color id");
            }
        }

        
        public IDataResult<List<CarDetailDto>> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            if (_carDal.GetCarDetails(filter).Count > 0)
            {
                return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(filter));
            }
            else
            {
                return new ErrorDataResult<List<CarDetailDto>>("No car found with this filter");
            }
        }

        [CacheRemoveAspect("ICarService.Get")]
        public IResult Delete(Car entity)
        {
            _carDal.Delete(entity);
            return new SuccessResult("Succesfully deleted");
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailsById(Expression<Func<Car, bool>> filter = null)
        {
            if (_carDal.GetCarDetails(filter).Count > 0)
            {
                return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(filter));
            }
            else
            {
                return new ErrorDataResult<List<CarDetailDto>>("No car found with this id");
            }
        }
    }
}
