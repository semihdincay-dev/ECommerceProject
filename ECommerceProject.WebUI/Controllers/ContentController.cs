using ECommerceProject.BLL;
using ECommerceProject.DAL.Models;
using ECommerceProject.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ECommerceProject.Controllers
{
    //[RoutePrefix("Content")]
    //[ValidateInput(false)]

    public class ContentController : Controller
    {
        private ContentRepository service = new ContentRepository();
        /// <summary>
        /// Veritabanından ürünleri listeliyoruz
        /// </summary>
        /// <returns></returns>
        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            List<ContentViewModel> productList = service.getAll();
            return View(productList);
        }
        /* SearchingValue yu alıyoruz View' dan ve veritabanında karşılaştırıyoruz */
        public JsonResult GetSearchingData(string SearchBy,string SearchValue)
        {
            var filteredList = service.GetSearchingData(SearchBy, SearchValue);

            return Json(filteredList,JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Databaseden Image' i alıyoruz.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }

        public byte[] GetImageFromDataBase(int Id)
        {
            return service.GetImageFromDataBase(Id);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// Ürün ve Fotoğraf ekleme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost]
        public ActionResult Create(ContentViewModel model)
        {
            HttpPostedFileBase file = Request.Files["ImageData"];
            int i = service.UploadImageInDataBase(file, model);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            List<ContentViewModel> productList = service.getAll();
            return View(productList);
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            return View(service.GetById(id));
        }
        [HttpPost]
        public ActionResult EditProduct(ContentViewModel product)
        {
            service.UpdateProduct(Request.Files["ImageData"], product);

            return RedirectToAction("Edit");
        }

        public ActionResult Delete(int id)
        {
            service.Delete(id);

            return RedirectToAction("Edit");
        }
    }
}