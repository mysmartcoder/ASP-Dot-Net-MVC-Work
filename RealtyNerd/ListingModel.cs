using RealtyNERD.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealtyNERD.BackOffice.Models.Listings
{
    public class ListingModel
    {
        public ListingModel()
        {
            AgentList = new List<ListingAgentControl>();
            OpenHouseList = new List<OpenHouse>();
            BuildingList = new List<SelectListItem>();
            CompanyList = new List<SelectListItem>();
            BuildingFeaturesList = new List<BuildingFeaturesControl>();
            Features = new List<ListingFeaturesControl>();
            FilterResult = new List<FilterListingControl>();
        }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PropertyType { get; set; }
        public string OwnershipType { get; set; }
        public string BuildingUnitNumber { get; set; }
        public string UnitNumber { get; set; }
        public string FloorNumber { get; set; }
        public string Layout { get; set; }
        public string Bathroom { get; set; }
        public string Sqft { get; set; }
        public decimal? Price { get; set; }
        public System.DateTime DateAvailable { get; set; }
        public string Minleaseterm { get; set; }
        public string Maxleaseterm { get; set; }
        public string Furnished { get; set; }
        public decimal FurnishedAmount { get; set; }
        public string Feestructure { get; set; }
        public string Incentives { get; set; }
        public string Renterpays { get; set; }
        public decimal CommissionPercentage { get; set; }
        public decimal Cobrokesplit { get; set; }
        public string Exclusivetype { get; set; }
        public HttpPostedFileBase Exclusiveagreement { get; set; }
        public string UnitExposure { get; set; }
        public string PublicUnitDescription { get; set; }
        public string BrokerViewingInstructions { get; set; }
        public string OccupancyStatus { get; set; }
        public System.DateTime FirstShowingDate { get; set; }
        public bool KeyInOffice { get; set; }
        public string KeyDetails { get; set; }
        public string PrivateUnitNotes { get; set; }
        public bool ListingPrivate { get; set; }
        public int managementid { get; set; }
        public string BuildingName { get; set; }
        public string buildid { get; set; }
        public string companyid { get; set; }
        public int id { get; set; }
        public string image { get; set; }
        public Common.Bathrooms BathroomList { get; set; }
        public Common.Layout LayoutList { get; set; }
        public Common.Furnished FurnishedList { get; set; }
        public Common.Incentives IncentivesList { get; set; }
        public Common.ExclusiveType ExclusiveTypeList { get; set; }
        public Common.PetPolicy PetPolicyList { get; set; }
        public Common.FilterPrice FilterPriceList { get; set; }
        public Common.Media MediaList { get; set; }
        public string PetPolicyListstr { get; set; }
        public Common.UnitCondition UnitConditionList { get; set; }
        public Common.UnitView UnitViewList { get; set; }
        public Common.OccupancyStatus OccupancyStatusList { get; set; }
        public List<ListingAgentControl> AgentList { get; set; }
        public string AgentId { get; set; }
        public string appfee { get; set; }
        public string primaryphone { get; set; }
        public string primaryemail { get; set; }
        public string website { get; set; }
        public string TotalRooms { get; set; }
        public string Feature { get; set; }
        public string Directions { get; set; }
        public List<OpenHouse> OpenHouseList { get; set; }
        public int BuildingId { get; set; }
        public Nullable<decimal> sales_price { get; set; }
        public Nullable<System.DateTime> sales_date { get; set; }
        public Nullable<decimal> sales_cc_main_fee { get; set; }
        public Nullable<decimal> sales_monthly_tax { get; set; }
        public string sales_comm_struct { get; set; }
        public Nullable<decimal> sales_tax_deduction { get; set; }
        public Nullable<decimal> sales_flip_tax { get; set; }
        public Nullable<decimal> sales_max_fin { get; set; }
        public Common.SaleExclusiveType sales_exe_type { get; set; }
        public string property_type { get; set; }
        public Common.ResidentialType res_type { get; set; }
        public string res_type_str { get; set; }
        public Common.CommercialType com_type { get; set; }
        public string com_type_str { get; set; }
        public string com_sqft { get; set; }
        public Common.LeaseType com_lease_type { get; set; }
        public bool is_divide { get; set; }
        public bool is_vented { get; set; }
        public bool is_food_ok { get; set; }
        public string listing_type { get; set; }
        public string com_electric_cost { get; set; }
        public bool sales_sponsor_unit { get; set; }
        public IList<SelectListItem> BuildingList { get; set; }
        public IList<SelectListItem> CompanyList { get; set; }
        public IList<HttpPostedFileBase> PhotoUploadList { get; set; }
        public IList<HttpPostedFileBase> FloorUploadList { get; set; }
        public List<BuildingFeaturesControl> BuildingFeaturesList { get; set; }
        public List<ListingFeaturesControl> Features { get; set; }
        public List<FilterListingControl> FilterResult { get; set; }
        public DateTime createdAt { get; set; }
    }
    public class OpenHouse
    {
        public System.DateTime openhousedate { get; set; }
        public System.TimeSpan openhousestarttime { get; set; }
        public System.TimeSpan openhouseendtime { get; set; }
        public Common.Repeat RepeatList { get; set; }
        public int RepeatId { get; set; }
        public bool IsBrokerOnly { get; set; }
        public bool IsAppointmentOnly { get; set; }
    }
}