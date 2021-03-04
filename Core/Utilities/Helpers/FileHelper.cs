using Core.Utilities.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Utilities.Helpers
{
    public class FileHelper
    {
        public static string Add(IFormFile file)
        {
            var sourcePath = newPath(file);
            using (var stream = new FileStream(sourcePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            //var customPath = newPath(file);
            //File.Move(sourcePath, customPath);
            return sourcePath;
        }

        public static IResult Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                return new ErrorResult(e.Message);
            }
            return new SuccessResult();
        }

        public static string Update(string sourcePath, IFormFile file)
        {
            var np = newPath(file);
            if (sourcePath.Length > 0)
            {
                using (var stream = new FileStream(np, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            File.Delete(sourcePath);
            return np;
        }

        private static string newPath(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);

            string path = Environment.CurrentDirectory + @"\.." + @"\Images";

            var newPath = Guid.NewGuid().ToString() + "--" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + fileExtension;

            string result = $@"{path}\{newPath}";

            return result;
        }
    }
}
