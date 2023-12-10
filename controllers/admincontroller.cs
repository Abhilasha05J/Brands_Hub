using Newtonsoft.Json;
using onlineshoppingstore.DAL;
using onlineshoppingstore.Models;
using onlineshoppingstore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace onlineshoppingstore.Controllers
{
   
    public class AdminController : Controller
    {
        // GET: Admin
        
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        [Authorize]
        public List<SelectListItem> GetCategory()
        {
            
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstances<Tbl_Category>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });

            }
            return list;



        }
        public ActionResult Dashboard()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Orders()
        {
            return View();
        }


        [Authorize(Roles ="Admin")]
        public ActionResult Categories()
        {
            List<Tbl_Category> allcategories = _unitOfWork.GetRepositoryInstances<Tbl_Category>().GetAllRecordsIQueryble().Where(i => i.IsDelete == false).ToList();
            return View(allcategories);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddCategory()
        {
            return UpdateCategory(0);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult UpdateCategory(int categoryId)
        {
            CategoryDetail cd;
           if (categoryId != null)
            {
                cd = JsonConvert.DeserializeObject<CategoryDetail>(JsonConvert.SerializeObject(_unitOfWork.GetRepositoryInstances<Tbl_Category>().GetFirstorDefault(categoryId)));
            }
            else
          //  {
                cd = new CategoryDetail();
          //  }
            return View("UpdateCategory", cd);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult CategoryEdit(int catId)
        {

            return View(_unitOfWork.GetRepositoryInstances<Tbl_Category>().GetFirstorDefault(catId));
        }
        [HttpPost]

        [Authorize(Roles = "Admin")]
        public ActionResult CategoryEdit(Tbl_Category tbl)
        {


            _unitOfWork.GetRepositoryInstances<Tbl_Category>().Update(tbl);
            return RedirectToAction("Categories");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Product()
        {
            return View(_unitOfWork.GetRepositoryInstances<Tbl_product>().GetProduct());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ProductEdit(int productId)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstances<Tbl_product>().GetFirstorDefault(productId));
        }
        [HttpPost]

        [Authorize(Roles = "Admin")]
        public ActionResult ProductEdit(Tbl_product tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                // file is uploaded
                file.SaveAs(path);
            }
            tbl.ProductImage = file != null ? pic : tbl.ProductImage;

            tbl.ModifiedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstances<Tbl_product>().Update(tbl);
            return RedirectToAction("Product");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ProductAdd()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }
        [HttpPost]

        [Authorize(Roles = "Admin")]
        public ActionResult ProductAdd(Tbl_product tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                // file is uploaded
                file.SaveAs(path);
            }
            tbl.ProductImage = pic;

            tbl.CreatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstances<Tbl_product>().Add(tbl);
            return RedirectToAction("Product");
        }





public ActionResult loginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult loginAdmin(Tbl_Members Model)
        {
            using (var context = new dbMyOnlineShoppingEntities())
            {
                bool isvalid = context.Tbl_Members.Any(x => x.EmailId == Model.EmailId && x.Password == Model.Password);
                if (isvalid)
                {

                    return RedirectToAction("Dashboard");
                }
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View();
            }

        }
        public ActionResult signupAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signupAdmin(Tbl_Members Model)
        {
            using (var context = new dbMyOnlineShoppingEntities())
            {
                context.Tbl_Members.Add(Model);
                context.SaveChanges();
            }
            return RedirectToAction("loginAdmin");
        }
        public ActionResult logout()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("loginAdmin");
        }

    }
}