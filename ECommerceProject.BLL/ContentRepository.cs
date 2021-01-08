using ECommerceProject.DAL.Models;
using ECommerceProject.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ECommerceProject.BLL
{
    public class ContentRepository : IContentRepository
    {
        DBContext db = new DBContext();
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        public void Delete(int Id)
        {
            db.Contents.Remove(db.Contents.FirstOrDefault(z => z.ID == Id));
            db.SaveChanges();
        }

        public List<Contents> GetSearchingData(string SearchBy, string SearchValue)
        {
            List<Contents> prodList = new List<Contents>();

            //if (SearchBy == "ID")
            //{
            //    try
            //    {
            //        int Id = Convert.ToInt32(SearchValue);
            //        prodList = db.Contents.Where(z => z.ID == Id || SearchValue == null).ToList();
            //    }
            //    catch (FormatException)
            //    {
            //        Console.WriteLine("{0} Is Not An Id", SearchValue);
            //    }
            //}
            
            if (SearchBy == "Title")
            {
                prodList = db.Contents.Where(z => z.Title.Contains(SearchValue) || SearchValue == null).ToList();
            }
            else
            {
                prodList = db.Contents.Where(z => z.Description.Contains(SearchValue) || SearchValue == null).ToList();
            }
            return prodList;
        }

        public List<ContentViewModel> getAll()
        {
            var products = db.Contents.Select(z => new
            {
                z.ID,
                z.Code,
                z.Title,
                z.Description,
                z.Price,
                z.Stock,
                z.Image
            });

            List<ContentViewModel> contentViewModel = products.Select(item => new ContentViewModel()
            {
                ID = item.ID,
                Code = item.Code,
                Title = item.Title,
                Description = item.Description,
                Price = item.Price,
                Stock = item.Stock,
                Image = item.Image
            }).ToList();

            return contentViewModel.ToList();
        }

        public Contents GetById(int Id)
        {
            return db.Contents.FirstOrDefault(z => z.ID == Id);
        }

        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Contents where temp.ID == Id select temp.Image;
            byte[] cover = q.First();
            return cover;
        }

        public void UpdateProduct(HttpPostedFileBase file, ContentViewModel contentViewModel)
        {
            var _product = db.Contents.FirstOrDefault(z => z.ID == contentViewModel.ID);
            _product.Image = ConvertToBytes(file);

            db.SaveChanges();
        }

        public int UploadImageInDataBase(HttpPostedFileBase file, ContentViewModel contentViewModel)
        {
            contentViewModel.Image = ConvertToBytes(file);
            var Content = new Contents
            {
                Code = contentViewModel.Code,
                Title = contentViewModel.Title,
                Price = contentViewModel.Price,
                Stock = contentViewModel.Stock,
                Image = contentViewModel.Image,
                Description = contentViewModel.Description

            };
            db.Contents.Add(Content);
            int i = db.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
