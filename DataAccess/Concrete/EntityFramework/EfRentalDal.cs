using Core.DataAccess.EntityFramework;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Concrete.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRentalDal : EfEntityRepositoryBase<Rental, RentalContext>, IRentalDal
    {
        // DTO
        public List<RentalDetailDto> GetRentalDetails(Expression<Func<Rental, bool>> filter = null)
        {
            using (RentalContext context = new RentalContext())
            {
                var result = from r in filter == null ? context.Rentals : context.Rentals.Where(filter)
                             join car in context.Cars on r.CarId equals car.Id
                             join cus in context.Customers on r.CustomerId equals cus.Id
                             join usr in context.Users on cus.UserId equals usr.Id
                             select new RentalDetailDto
                             {
                                 RentalId = r.Id,
                                 CarName = car.Description,
                                 FirstName = usr.FirstName,
                                 RentDate = r.RentDate,
                                 ReturnDate = r.ReturnDate
                             };

                return new List<RentalDetailDto>(result.ToList());
            }
        }
    }
}
