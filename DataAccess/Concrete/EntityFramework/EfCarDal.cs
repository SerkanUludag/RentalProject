using Core.DataAccess.EntityFramework;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, RentalContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            using (RentalContext rc = new RentalContext())
            {
                var result = from cr in filter is null ? rc.Cars : rc.Cars.Where(filter)
                             join cl in rc.Colors
                             on cr.ColorId equals cl.Id
                             join br in rc.Brands
                             on cr.BrandId equals br.Id
                             select new CarDetailDto
                             {
                                 CarName = cr.Description,
                                 BrandName = br.Name,
                                 ColorName = cl.Name,
                                 DailyPrice = cr.DailyPrice,
                                 ModelYear = cr.ModelYear,
                                 CarId = cr.Id

                             };

                return new List<CarDetailDto>(result.ToList());
                             
                                
            }
        }
    }
}
