using RealtyNERD.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyNERD.DataAccess
{
    public class Listings
    {
        realtyNERDDBEntities db = null;

        //Data Context 
        public Listings()
        {            
            db = new realtyNERDDBEntities();
        }

        ~Listings()
        {
            db.Dispose();
        }

        //Method to fetch all the listing data from the database
        public List<listing> GetListings()
        {
            try
            {
                IQueryable<listing> _listings = from table in db.listings
                                                select table;

                return _listings.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method to fetch the listing data by id from the database
        public listing GetListing(int id)
        {
            try
            {                
                listing _listing = (from table in db.listings
                                    where table.id.Equals(id)
                                    select table).FirstOrDefault();

                return _listing;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method to Add the listing data into the database
        public int AddListing(listing _Listing, List<ListingAgentControl> _ListingAgents)
        {
            int id = 0;
            try
            {
                listing _listings = (from table in db.listings
                                    where table.id.Equals(_Listing.id)
                                    select table).FirstOrDefault();
                if (_listings != null)
                {
                    id = _listings.id;
                }
                
                if (_listings == null)
                {
                    db.listings.Add(_Listing);
                    db.SaveChanges();
                    id = _Listing.id;
                    ListingAgents ListingAgents = new ListingAgents();
                    foreach (var item in _ListingAgents)
                    {
                        if (item.IsCheck)
                        {
                            listingagent _listingagent = new listingagent();
                            _listingagent.listingid = id;
                            _listingagent.agentid = item.Id;
                            ListingAgents.AddListingAgents(_listingagent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }

        //Method to Filter out the listing data
        public List<listing> FilterSearch(listing filter)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;             

                var result = db.listings.Where(f => (filter.layout == "0" || f.layout == filter.layout) &&
                                                    (filter.bathroom == "0" || f.bathroom == filter.bathroom) &&
                                                    (filter.price == 0 || f.price <= filter.price) &&
                                                    (filter.managementid == 0 || f.managementid == filter.managementid) &&
                                                    (filter.buildingid == 0 || f.buildingid == filter.buildingid) && 
                                                    (filter.has_photos == null || f.has_photos == true) &&
                                                    (filter.has_floorplans == null || f.has_floorplans == true)).ToList();
                return result;
                              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method to Update the listing data into the database
        public void EditListing(listing _Listing)
        {
            try
            {
                listing _listing = (from table in db.listings
                                    where table.id.Equals(_Listing.id)
                                    select table).FirstOrDefault();

                if (_listing != null)
                {
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
    public class FilterListingControl
    {
        public string companyName { get; set; }
        public string buildingName { get; set; }
        public string Address { get; set; }
        public decimal? Price { get; set; }
        public string BuildingUnitNumber { get; set; }
        public string UnitNumber { get; set; }
        public int id { get; set; }
        public string Incentives { get; set; }
        public string Layout { get; set; }
        public string Bathroom { get; set; }
        public string Sqft { get; set; }
    }
}
