using Business.Abstract;
using Business.Constants.Messages;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;
        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }

        [ValidationAspect(typeof(CarImageValidator))]
        public IResult Add(IFormFile file, CarImage carImage)
        {

            IResult result = BusinessRules.Run(CheckIfCarHasMoreThanFiveImages(carImage.CarId));

            if (result != null)
            {
                return result;
            }

            carImage.ImagePath = FileHelper.Add(file);
            carImage.Date = DateTime.Now;
            _carImageDal.Add(carImage);

            return new SuccessResult("Succesfully added");
        }

        [CacheAspect]
        public IDataResult<List<CarImage>> GetAll()
        {
            var result = _carImageDal.GetAll();
            if (result.Count > 0)
            {
                return new SuccessDataResult<List<CarImage>>(result);
            }
            else
            {
                return new ErrorDataResult<List<CarImage>>("No image found");
            }
        }

        [CacheAspect]
        public IDataResult<CarImage> GetById(int id)
        {
            if (_carImageDal.Get(i => i.Id == id) != null)
            {
                return new SuccessDataResult<CarImage>(_carImageDal.Get(b => b.Id == id));
            }
            else
            {
                return new ErrorDataResult<CarImage>("car image with given id is not found");
            }
        }

        [CacheAspect]
        public IDataResult<List<CarImage>> GetImagesByCarId(int carId)
        {
            return new SuccessDataResult<List<CarImage>>(CheckIfCarHasImage(carId));
        }

        public IResult Delete(CarImage carImage)
        {
            FileHelper.Delete(carImage.ImagePath);
            _carImageDal.Delete(carImage);
            return new SuccessResult("Successfully deleted");
        }
        

        [ValidationAspect(typeof(CarImageValidator))]
        public IResult Update(IFormFile file, CarImage carImage)
        {
            carImage.ImagePath = FileHelper.Update(_carImageDal.Get(i => i.Id == carImage.Id).ImagePath, file);
            carImage.Date = DateTime.Now;
            _carImageDal.Update(carImage);
            return new SuccessResult("Succesfully updated");

        }
        
        private IResult CheckIfCarHasMoreThanFiveImages(int carId)
        {
            var result = _carImageDal.GetAll(i => i.CarId == carId);
            if (result.Count >= 5)
            {
                return new ErrorResult(Messages.MoreThanFiveImageError);
            }
            return new SuccessResult();
        }

        private List<CarImage> CheckIfCarHasImage(int carId)
        {
            const string defaultPath = @"C:\Users\hsulu\Desktop\rental-project\Images\logo.jpg";


            var result = _carImageDal.GetAll(i => i.CarId == carId).Any();
            if(!result)
            {
                return new List<CarImage> { new CarImage { CarId = carId, ImagePath = defaultPath, Date = DateTime.Now } };
            }
            return _carImageDal.GetAll(i => i.CarId == carId);
        }
    }
}
