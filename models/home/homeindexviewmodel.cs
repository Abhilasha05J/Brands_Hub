using onlineshoppingstore.DAL;
using onlineshoppingstore.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;


namespace onlineshoppingstore.Models.Home
{
    public class HomeIndexViewModel
    {
       public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        dbMyOnlineShoppingEntities context = new dbMyOnlineShoppingEntities();
        public IPagedList<Tbl_product> ListOfProducts { get; set; }
        public HomeIndexViewModel CreateModel(string search, int pagesize, int? page)
        {
            SqlParameter[] pram = new SqlParameter[] {
                   new SqlParameter("@search",search??(object)DBNull.Value)
                   };
           IPagedList<Tbl_product> data = context.Database.SqlQuery<Tbl_product>("GetBySearch @search", pram).ToList().ToPagedList(page ?? 1, pagesize);
            return new HomeIndexViewModel()
            {
                ListOfProducts= data
            };
        }
    }
}