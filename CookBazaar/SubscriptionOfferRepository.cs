using CookBazaar.Domain;
using CookBazaar.Domain.LKP;
using CookBazaar.Domain.LKP.Subscriptions;
using CookBazaar.Domain.SYS.Subscriptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBazaar.Repository.SYS.Subscriptions
{
    public class SubscriptionOfferRepository : DBProvider
    {
        public SubscriptionOfferRepository()
        {

        }

        public SubscriptionOfferRepository(bool isValid)
        {
            IsModelStateValid = isValid;
        }

        public List<SubscriptionOffer> GetAll()
        {
            try
            {
                return CookBazaarContext.SubscriptionOffers.Include(s => s.subscriptionCategory).Include(c => c.subscriptionPeriods).ToList();
            }
            catch (Exception ex)
            {
                Repository.ExceptionLog.AddException(ex, "SubscriptionOfferRep/GetAll", "");
                return new List<SubscriptionOffer>();
            }
        }
                
        public async Task<SubscriptionOfferSearch> GetFrontSearchItems()
        {            
            SubscriptionOfferSearch Items = new SubscriptionOfferSearch();

            Items.SearchResult =await FrontSearch(Items);

            if (Items.SearchResult.Count > 0)
            {
                Items.FromPrice = Items.SearchResult.Min(p => p.Price);
                Items.ToPrice = Items.SearchResult.Max(p => p.Price);
            }
            try
            {
                List<SubscriptionPeriod> Periods =await CookBazaarContext.SubscriptionPeriods.Where(cat => cat.IsActive == true).ToListAsync();
                foreach (SubscriptionPeriod period in Periods)
                {
                    Items.Periods.Add(new SubscriptionPeriodControl()
                    {
                        PeriodID = period.PeriodID,
                        PeriodName = Domain.Resources.LKP.Subscriptions.SubscriptionPeriods.SubscriptionPeriod.PeriodNameTextField == "EnglishPeriodName" ? period.EnglishPeriodName : period.ArabicPeriodName,
                        IsCheck = false
                    });
                }

                List<SubscriptionCategory> Categories = CookBazaarContext.SubscriptionCategories.Where(cat => cat.IsActive == true).ToList();
                foreach (SubscriptionCategory type in Categories)
                {
                    Items.Categories.Add(new SubscriptionCategoryControl()
                    {
                        CategoryID = type.CategoryID,
                        CategoryName = Domain.Resources.LKP.Subscriptions.SubscriptionCategories.SubscriptionCategory.CategoryNameTextField == "EnglishCategoryName" ? type.EnglishCategoryName : type.ArabicCategoryName,
                        IsCheck = false
                    });
                }


                return Items;
            }
            catch (Exception)
            {
                return Items;
            }
        }

        public async Task<SubscriptionOfferSearch> GetFrontSearchItems(SubscriptionOfferSearch Items)
        {   
            Items.SearchResult =await FrontSearch(Items);
            if (Items.SearchResult.Count > 0)
            {
                Items.FromPrice = Items.SearchResult.Min(p => p.Price);
                Items.ToPrice = Items.SearchResult.Max(p => p.Price);
            }
          
            try
            {
                List<SubscriptionPeriod> Periods =await CookBazaarContext.SubscriptionPeriods.Where(cat => cat.IsActive == true).ToListAsync();
                foreach (SubscriptionPeriod period in Periods)
                {
                    Items.Periods.Add(new SubscriptionPeriodControl()
                    {
                        PeriodID = period.PeriodID,
                        PeriodName = Domain.Resources.LKP.Subscriptions.SubscriptionPeriods.SubscriptionPeriod.PeriodNameTextField == "EnglishPeriodName" ? period.EnglishPeriodName : period.ArabicPeriodName,
                        IsCheck = false
                    });
                }

                List<SubscriptionCategory> Categories = CookBazaarContext.SubscriptionCategories.Where(cat => cat.IsActive == true).ToList();
                foreach (SubscriptionCategory type in Categories)
                {
                    Items.Categories.Add(new SubscriptionCategoryControl()
                    {
                        CategoryID = type.CategoryID,
                        CategoryName = Domain.Resources.LKP.Subscriptions.SubscriptionCategories.SubscriptionCategory.CategoryNameTextField == "EnglishCategoryName" ? type.EnglishCategoryName : type.ArabicCategoryName,
                        IsCheck = false
                    });
                }


                return Items;
            }
            catch (Exception)
            {
                return Items;
            }
        }

        public async Task<List<SubscriptionOffer>> FrontSearch(SubscriptionOfferSearch filter)
        {
            try
            {
                int periodsCount = filter.Periods.Count > 0 ? filter.Periods.Where(c => c.IsCheck == true).ToList().Count : 0;
                int categoriesCount = filter.Categories.Count > 0 ? filter.Categories.Where(c => c.IsCheck == true).ToList().Count : 0;
                var result = CookBazaarContext.SubscriptionOffers.Include(s => s.subscriptionCategory).Include(c => c.subscriptionPeriods).Where(offer => (offer.OfferEndDate > DateTime.Now) && offer.CityID == filter.CityID) && (filter.FromPrice == 0 || offer.Price >= filter.FromPrice) && (filter.ToPrice == 0 || offer.Price <= filter.ToPrice) && (categoriesCount == 0 || filter.CategoryKeys.Contains(offer.CategoryID)) && (periodsCount == 0 || offer.subscriptionPeriods.Any(period => filter.PeriodKeys.Contains(period.PeriodID))));
                return await result.ToListAsync();
            }
            catch (Exception ex)
            {
                Repository.ExceptionLog.AddException(ex, "SubscriptionOfferRep/FrontSearch", "");
                return new List<SubscriptionOffer>();
            }
        }

        public List<SubscriptionOffer> GetCustom(SearchFilters filter, out int totalRecords, out int recordsFiltered)
        {
            totalRecords = recordsFiltered = 0;
            var v = (from a in CookBazaarContext.SubscriptionOffers.Include(s => s.subscriptionCategory).Include(s => s.city).Include(c => c.subscriptionPeriods) select a);

            totalRecords = recordsFiltered = v.Count();

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                v = v.Where(c => c.ArabicOfferName.Contains(filter.SearchString) || c.EnglishOfferName.Contains(filter.SearchString) || c.subscriptionCategory.ArabicCategoryName.Contains(filter.SearchString) || c.city.ArabicCityName.Contains(filter.SearchString) || c.OfferID.ToString().Contains(filter.SearchString));
                recordsFiltered = v.Count();
            }

            if (!string.IsNullOrEmpty(filter.SortColumn) && !string.IsNullOrEmpty(filter.SortDirection))
            {
                if (filter.SortDirection.ToUpper() == "DESC")
                {
                    switch (filter.SortColumn)
                    {
                        case "0":
                            {
                                v = v.OrderByDescending(c => c.OfferID);
                                break;
                            }
                        case "1":
                            {
                                v = v.OrderByDescending(c => c.ArabicOfferName);
                                break;
                            }
                        case "2":
                            {
                                v = v.OrderByDescending(c => c.EnglishOfferName);
                                break;
                            }
                        case "3":
                            {
                                v = v.OrderByDescending(c => c.subscriptionCategory.ArabicCategoryName);
                                break;
                            }
                        case "4":
                            {
                                v = v.OrderByDescending(c => c.city.ArabicCityName);
                                break;
                            }
                    }
                }
                else
                {
                    switch (filter.SortColumn)
                    {
                        case "0":
                            {
                                v = v.OrderBy(c => c.OfferID);
                                break;
                            }
                        case "1":
                            {
                                v = v.OrderBy(c => c.ArabicOfferName);
                                break;
                            }
                        case "2":
                            {
                                v = v.OrderBy(c => c.EnglishOfferName);
                                break;
                            }
                        case "3":
                            {
                                v = v.OrderBy(c => c.subscriptionCategory.ArabicCategoryName);
                                break;
                            }
                        case "4":
                            {
                                v = v.OrderBy(c => c.city.ArabicCityName);
                                break;
                            }
                    }
                }
            }
            return v.Skip(filter.StartIndex).Take(filter.PageLength).ToList();
        }

        public SubscriptionOffer GetOfferByID(int id)
        {
            return CookBazaarContext.SubscriptionOffers.Include(s => s.subscriptionCategory).Include(c => c.subscriptionPeriods).Include(c => c.WorkingDays).FirstOrDefault(p => p.OfferID == id);
        }

        public SubscriptionOffer GetNewOffer()
        {
            
            SubscriptionOffer offer = new SubscriptionOffer();
            offer.OfferStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).AddDays(3);
            offer.OfferEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).AddDays(30);
            offer.OfferDate = DateTime.Now;
            offer.IsActive = true;
            List<SubscriptionPeriod> Periods = CookBazaarContext.SubscriptionPeriods.Where(c => c.IsActive == true).ToList();
            List<Day> Days = CookBazaarContext.Days.ToList();
            offer.PeriodsControl.Clear();
            offer.DaysControl.Clear();
            foreach (SubscriptionPeriod period in Periods)
            {
                offer.PeriodsControl.Add(new SubscriptionPeriodControl()
                {
                    IsCheck = false,
                    PeriodID = period.PeriodID,
                    PeriodName = Domain.Resources.LKP.Subscriptions.SubscriptionPeriods.SubscriptionPeriod.PeriodNameTextField == "EnglsihPeriodName" ? period.EnglishPeriodName : period.ArabicPeriodName
                });                
            }
            foreach (Day day in Days)
            {
                offer.DaysControl.Add(new DayControl()
                {
                    IsCheck = false,
                    DayID = day.DayID,
                    DayName = Domain.Resources.LKP.Days.Day.DayNameTextField == "EnglishDayName" ? day.EnglishDayName : day.ArabicDayName
                });
            }
            return offer;
        }

        public dynamic Add(SubscriptionOffer offer)
        {
            var result = new { state = "", title = "", message = "", close = false };
            try
            {
                if (IsModelStateValid)
                {
                    CookBazaarContext.SubscriptionOffers.Add(offer);
                    foreach (SubscriptionPeriodControl period in offer.PeriodsControl)
                    {
                        if(period.IsCheck)
                        offer.subscriptionPeriods.Add(CookBazaarContext.SubscriptionPeriods.Find(period.PeriodID));
                    }
                    foreach (DayControl day in offer.DaysControl)
                    {
                        if (day.IsCheck)
                        offer.WorkingDays.Add(CookBazaarContext.Days.Find(day.DayID));
                    }
                    CookBazaarContext.SaveChanges();
                    result = new
                    {
                        state = "success",
                        title = Domain.Resources.GeneralMessage.SuccessTitle,
                        message = Domain.Resources.SYS.SubscriptionOffers.SubscriptionOffer.AddingSuccessfully,
                        close = true
                    };
                }
                else
                {
                    result = new
                    {
                        state = "warning",
                        title = Domain.Resources.GeneralMessage.ErrorTitle,
                        message = Domain.Resources.GeneralMessage.ErrorWhileProcessingData,
                        close = false
                    };
                }
            }
            catch (Exception ex)
            {
                Repository.ExceptionLog.AddException(ex, "SubscriptionOfferRep/Add", "");
                result = new
                {
                    state = "warning",
                    title = Domain.Resources.GeneralMessage.ErrorTitle,
                    message = Domain.Resources.GeneralMessage.ErrorWhileSaving,
                    close = false
                };
            }
            return result;
        }

        public dynamic Edit(SubscriptionOffer offer)
        {
            var result = new { state = "", title = "", message = "", close = false };
            try
            {
                if (IsModelStateValid)
                {
                    CookBazaarContext.Entry(offer).State = EntityState.Modified;
                    CookBazaarContext.SaveChanges();
                    result = new
                    {
                        state = "success",
                        title = Domain.Resources.GeneralMessage.SuccessTitle,
                        message = Domain.Resources.SYS.SubscriptionOffers.SubscriptionOffer.EdittingSuccessfully,
                        close = true
                    };
                }
                else
                {
                    result = new
                    {
                        state = "warning",
                        title = Domain.Resources.GeneralMessage.ErrorTitle,
                        message = Domain.Resources.GeneralMessage.ErrorWhileProcessingData,
                        close = false
                    };
                }
            }
            catch (Exception ex)
            {
                Repository.ExceptionLog.AddException(ex, "SubscriptionOfferRep/Edit", "");
                result = new
                {
                    state = "warning",
                    title = Domain.Resources.GeneralMessage.ErrorTitle,
                    message = Domain.Resources.GeneralMessage.ErrorWhileSaving,
                    close = false
                };
            }
            return result;
        }

        public dynamic Delete(int id)
        {
            var result = new { state = "", title = "", message = "", close = false };
            try
            {
                if (IsModelStateValid)
                {
                    SubscriptionOffer offer = CookBazaarContext.SubscriptionOffers.Find(id);
                    CookBazaarContext.SubscriptionOffers.Remove(offer);
                    CookBazaarContext.SaveChanges();
                    result = new
                    {
                        state = "success",
                        title = Domain.Resources.GeneralMessage.SuccessTitle,
                        message = Domain.Resources.SYS.SubscriptionOffers.SubscriptionOffer.DeletingSuccessfully,
                        close = true
                    };
                }
                else
                {
                    result = new
                    {
                        state = "warning",
                        title = Domain.Resources.GeneralMessage.ErrorTitle,
                        message = Domain.Resources.GeneralMessage.ErrorWhileProcessingData,
                        close = false
                    };
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    result = new
                    {
                        state = "warning",
                        title = Domain.Resources.GeneralMessage.ErrorTitle,
                        message = Domain.Resources.SYS.SubscriptionOffers.SubscriptionOffer.RelatedDataException,
                        close = false
                    };
                }
                else
                {
                    Repository.ExceptionLog.AddException(ex, "SubscriptionOfferRep/Delete", "");
                    result = new
                    {
                        state = "warning",
                        title = Domain.Resources.GeneralMessage.ErrorTitle,
                        message = Domain.Resources.GeneralMessage.GeneralExceptionMessage,
                        close = false
                    };
                }
            }
            return result;

        }

        public dynamic Active(int id)
        {
            var result = new { state = "", title = "", message = "", close = false };
            try
            {
                if (IsModelStateValid)
                {
                    SubscriptionOffer offer = CookBazaarContext.SubscriptionOffers.Find(id);
                    offer.IsActive = !offer.IsActive;
                    CookBazaarContext.Entry(offer).State = EntityState.Modified;
                    CookBazaarContext.SaveChanges();
                    result = new
                    {
                        state = "success",
                        title = Domain.Resources.GeneralMessage.SuccessTitle,
                        message = (offer.IsActive ? Domain.Resources.SYS.SubscriptionOffers.SubscriptionOffer.ActivatingSuccessfully : Domain.Resources.SYS.SubscriptionOffers.SubscriptionOffer.DeactivatingSuccessfully),
                        close = true
                    };
                }
                else
                {
                    result = new
                    {
                        state = "warning",
                        title = Domain.Resources.GeneralMessage.ErrorTitle,
                        message = Domain.Resources.GeneralMessage.ErrorWhileProcessingData,
                        close = false
                    };
                }
            }
            catch (Exception ex)
            {
                Repository.ExceptionLog.AddException(ex, "SubscriptionOfferRep/Active", "");
                result = new
                {
                    state = "warning",
                    title = Domain.Resources.GeneralMessage.ErrorTitle,
                    message = Domain.Resources.GeneralMessage.GeneralExceptionMessage,
                    close = false
                };
            }
            return result;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
