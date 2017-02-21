using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CookBazaar.Domain.SYS.Subscriptions;
using CookBazaar.Repository;
using CookBazaar.Repository.SYS.Subscriptions;
using CookBazaar.Repository.LKP.Subscriptions;
using System.Threading.Tasks;
using CookBazaar.Domain.LKP.Subscriptions;
using CookBazaar.Repository.ENT.Subscriptions;
using CookBazaar.Domain.ENT.Subscriptions;

namespace CookBazaar.Web.Controllers
{
    public class SearchController : Controller
    {
        private CookBazaarDBContext db = new CookBazaarDBContext();
        SubscriptionOfferRepository getOffer = new SubscriptionOfferRepository();

        // GET: Subscription offer search
        public async Task<ActionResult> Index()
        {
            int cityID = 1;
            if (HttpContext.Request.Cookies["Current_City"] != null)
            {
                cityID = Convert.ToInt32(HttpContext.Request.Cookies["Current_City"].Values["CityID"]);
            }
            SubscriptionOfferSearch searchcontrols = await new SubscriptionOfferRepository().GetFrontSearchItems(new SubscriptionOfferSearch() { CityID = cityID });
            return View(searchcontrols);
        }

        // GET: Subscription offer filter search
        public async Task<ActionResult> Search(SubscriptionOfferSearch search)
        {
            int cityID = 1;
            if (HttpContext.Request.Cookies["Current_City"] != null)
            {
                cityID = Convert.ToInt32(HttpContext.Request.Cookies["Current_City"].Values["CityID"]);
            }
            search.CityID = cityID;
            search.SearchResult = await new SubscriptionOfferRepository().FrontSearch(search);
            return PartialView("_FrontSearch", search);
        }           

        // popup for itemdetail
        //GET : Search/ItemDetail/2
        public async Task<ActionResult> ItemDetail(int Id)
        {
            SubscriptionOffer model = new SubscriptionOffer();
            var SubscriptionOfferDetail = getOffer.GetOfferByID(Id);
            var OfferDetail = await getOffer.GetFrontSearchItems();
            if (SubscriptionOfferDetail != null)
            {
                model.OfferID = Id;
                model.EnglishOfferName = SubscriptionOfferDetail.EnglishOfferName;
                model.OfferIMG = SubscriptionOfferDetail.OfferIMG;
                model.EnglishDescriptions = SubscriptionOfferDetail.EnglishDescriptions;
                model.subscriptionCategory = SubscriptionOfferDetail.subscriptionCategory;
                model.OfferStartDate = (SubscriptionOfferDetail.OfferStartDate);
                model.OfferEndDate = SubscriptionOfferDetail.OfferEndDate;
                model.DeliveryEnd = SubscriptionOfferDetail.DeliveryEnd;
                model.DeliveryStart = SubscriptionOfferDetail.DeliveryStart;
                model.city = SubscriptionOfferDetail.city;
                model.Price = SubscriptionOfferDetail.Price;
                model.WorkingDays = SubscriptionOfferDetail.WorkingDays;
                model.subscriptionPeriods = SubscriptionOfferDetail.subscriptionPeriods;
                foreach (var item in SubscriptionOfferDetail.subscriptionPeriods)
                {
                    decimal price = item.WorkingDays * model.Price;
                    decimal Discount = (price * item.Discount) / 100;
                    decimal TotalPrice = price - Discount;
                    item.EnglishPeriodName = item.EnglishPeriodName + " : " + TotalPrice;
                }
                var lastItem = SubscriptionOfferDetail.WorkingDays.Last();
                foreach (var item in SubscriptionOfferDetail.WorkingDays)
                {
                    if (item.Equals(lastItem))
                        item.EnglishDayName = item.EnglishDayName;
                    else
                        item.EnglishDayName = item.EnglishDayName + ",";
                }
                model.IsActive = SubscriptionOfferDetail.IsActive;
                model.OfferEndDate = SubscriptionOfferDetail.OfferEndDate;
            }
            return PartialView("_ItemDetail", model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
