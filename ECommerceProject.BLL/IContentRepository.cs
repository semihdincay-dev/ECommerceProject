using ECommerceProject.DAL.Models;
using ECommerceProject.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ECommerceProject.BLL
{
    public interface IContentRepository
    {
        int UploadImageInDataBase(HttpPostedFileBase file, ContentViewModel contentViewModel);
        List<ContentViewModel> getAll();
        List<Contents> GetSearchingData(string SearchBy, string SearchValue);
        byte[] ConvertToBytes(HttpPostedFileBase image);
        byte[] GetImageFromDataBase(int Id);
        void UpdateProduct(HttpPostedFileBase file, ContentViewModel contentViewModel);
        Contents GetById(int Id);
        void Delete(int Id);

    }
}
