﻿using onlineshoppingstore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineshoppingstore.Models.Home
{
    public class Item
    {
        public Tbl_product Product { get; set; }
        public int Quantity { get; set; }
    }
}