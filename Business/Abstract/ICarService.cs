using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Business.Abstract
{
    public interface ICarService
    {
        IResult Add(Car entity);

        IResult Delete(Car entity);
        IDataResult<List<Car>> GetAll();

        IDataResult<List<Car>> GetCarsByBrandId(int id);

        IDataResult<List<Car>> GetCarsByColorId(int id);

        IDataResult<List<CarDetailDto>> GetCarDetails(Expression<Func<Car, bool>> filter = null);
    }
}
