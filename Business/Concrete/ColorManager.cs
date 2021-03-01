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
    public class ColorManager : IColorService
    {
        IColorDal _ColorDal;

        public ColorManager(IColorDal ColorDal)
        {
            _ColorDal = ColorDal;
        }

        [ValidationAspect(typeof(ColorValidator))]
        public IResult Add(Color entity)
        {
            _ColorDal.Add(entity);
            return new SuccessResult("Color added succesfully");

        }

        public IResult Delete(Color entity)
        {
            _ColorDal.Delete(entity);
            return new SuccessResult("Color deleted successfully");
        }

        public IDataResult<List<Color>> GetAll()
        {
            if (_ColorDal.GetAll().Count > 0)
            {
                return new SuccessDataResult<List<Color>>(_ColorDal.GetAll());
            }
            else
            {
                return new ErrorDataResult<List<Color>>("No car found");
            }
        }

        public IDataResult<Color> GetById(int id)
        {
            if (_ColorDal.Get(b => b.Id == id) != null)                      // check
            {
                return new SuccessDataResult<Color>(_ColorDal.Get(b => b.Id == id));
            }
            else
            {
                return new ErrorDataResult<Color>("Car with given id is not found");
            }
        }

        public IResult Update(Color entity)
        {
            _ColorDal.Update(entity);
            return new SuccessResult("Car updated!");
        }
    }
}
