using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineshoppingstore.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
             
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
               
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                    "PaymentWithPaypal/PaymentWithPaypal?";

                    var guid = Convert.ToString((new Random()).Next(100000000));
                    
                    var createPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    
                    var links = createPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    
                    Session.Add(guid, createPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                   
                    if (executedPayment.ToString().ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception)
            {
                return View("FailureView");
            }
           
            return View("SuccessView");
        }
       
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private PayPal.Api.Payment payment;
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var ItemList = new ItemList() { items = new List<Item>()};
            if (Session["cart"]!="")
            {
                List<Models.Home.Item> cart = (List<Models.Home.Item>)(Session["cart"]);
                foreach (var item in cart)
                {
                    ItemList.items.Add(new Item()
                    {
                        name = item.Product.ProductName.ToString(),
                        currency = "TK",
                        price = item.Product.Price.ToString(),
                        quantity = item.Product.Quantity.ToString(),
                        sku = "sku"
                    });

                }
            }
           


            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            
            var amount = new Amount()
            {
                currency = "USD",
                total = Session["SesTotal"].ToString(),
                details = details
            };
            var transactionList = new List<Transaction>();
            
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "#100000", 
                amount = amount,
                item_list = ItemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
     
            return this.payment.Create(apiContext);
        }
    }
  }
