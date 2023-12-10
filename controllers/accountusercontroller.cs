using onlineshoppingstore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace onlineshoppingstore.Controllers
{
    public class AccountUserController : Controller
    {
        // GET: AccountUser
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Tbl_Members Model)
        {
            using (var context = new dbMyOnlineShoppingEntities())
            {
                bool isvalid = context.Tbl_Members.Any(x => x.EmailId == Model.EmailId && x.Password == Model.Password);
                if (isvalid)
                {
                    FormsAuthentication.SetAuthCookie(Model.EmailId, true);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View();
            }

        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Tbl_Members Model)
        {
            using (var context = new dbMyOnlineShoppingEntities())
            {
                context.Tbl_Members.Add(Model);
                context.SaveChanges();
            }
          
            return RedirectToAction("Login");
            
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}