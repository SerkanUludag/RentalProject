using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {



            Rental r = new Rental();
            r.Id = 1;
            r.CustomerId = 1;
            r.CarId = 2;
            r.RentDate = new DateTime(2020, 10, 15);

            RentalManager rm = new RentalManager(new EfRentalDal());

            var result = rm.Add(r);
            Console.WriteLine(result.Message);







        }

        
    }
}