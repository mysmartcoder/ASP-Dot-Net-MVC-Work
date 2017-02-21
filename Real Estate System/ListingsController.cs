using RealtyNERD.BackOffice.Models;
using RealtyNERD.BackOffice.Models.Listings;
using RealtyNERD.BackOffice.Models.Managements;
using RealtyNERD.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealtyNERD.BackOffice.Controllers
{
    public class ListingsController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                //Check Authentication
                if (Session["user"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }                                                

                var name = System.Web.HttpContext.Current.User.Identity.Name;
                
                //Object Initiliaze 
                Buildings objBuilding = new Buildings();
                Managements objManagement = new Managements();
                ListingModel lstListing = new ListingModel();                
                Listings objListing = new Listings();
                List<listing> objLst = new List<listing>();
                Buildings objbuilding = new Buildings();
                Managements objmanagement = new Managements();
                
                //Start performing the logic section
                objLst = objListing.GetListings();
                if (objLst.Count > 0)
                {
                    foreach (var itm in objLst)
                    {
                        FilterListingControl objmodel = new FilterListingControl();
                        var Building = objbuilding.GetBuilding(itm.buildingid);
                        if (!string.IsNullOrEmpty(Convert.ToString(Building)))
                        {
                            objmodel.buildingName = Building.Name;
                            var Management = objmanagement.GetManagement(Building.managementid);
                            if (!string.IsNullOrEmpty(Convert.ToString(Management)))
                            {
                                objmodel.companyName = Management.name;
                            }
                            else
                            {
                                objmodel.companyName = string.Empty;
                            }
                        }
                        else
                        {
                            objmodel.buildingName = string.Empty;
                        }
                        objmodel.Address = itm.address;
                        objmodel.Price = itm.price;
                        objmodel.id = itm.id;
                        objmodel.Incentives = itm.incentives;
                        objmodel.Layout = itm.layout;
                        objmodel.Sqft = itm.sqft;
                        objmodel.UnitNumber = itm.unitnumber;
                        objmodel.Bathroom = itm.bathroom;
                        objmodel.BuildingUnitNumber = itm.unitnumber;
                        lstListing.FilterResult.Add(objmodel);
                    }
                }

                lstListing.BuildingList = objBuilding.GetBuildings().Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.id.ToString()
                }).ToList();

                lstListing.CompanyList = objManagement.GetManagements().Select(t => new SelectListItem
                {
                    Text = t.name,
                    Value = t.id.ToString()
                }).ToList();


                return View(lstListing);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult FilterList(ListingModel filter)
        {
            try
            {
                //Check Authentication
                if (Session["user"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                
                //Object Initialize
                listing objlst = new listing();
                ListingModel lstListing = new ListingModel();
                List<listing> objlist = new List<listing>();
                Buildings objbuilding = new Buildings();
                Managements objmanagement = new Managements();

                //Start performing the logic section
                objlst.managementid = Convert.ToInt32(filter.companyid);
                objlst.buildingid = Convert.ToInt32(filter.buildid);
                objlst.layout = Convert.ToString(filter.LayoutList);
                objlst.bathroom = Convert.ToString(filter.BathroomList);
                var strPrice = (int)((Common.FilterPrice)Enum.Parse(typeof(Common.FilterPrice), filter.FilterPriceList.ToString()));
                objlst.price = Convert.ToDecimal(strPrice);
                var strMedia = (int)((Common.Media)Enum.Parse(typeof(Common.Media), filter.MediaList.ToString()));

                if (strMedia == 1)
                {
                    objlst.has_photos = true;
                }
                else if (strMedia == 2)
                {
                    objlst.has_floorplans = true;
                }
                
                objlist = new Listings().FilterSearch(objlst);
                if (objlist.Count > 0)
                {
                    foreach (var itm in objlist)
                    {
                        FilterListingControl objFilterControl = new FilterListingControl();
                        var Building = objbuilding.GetBuilding(itm.buildingid);
                        if (!string.IsNullOrEmpty(Convert.ToString(Building)))
                        {
                            objFilterControl.buildingName = Building.Name;
                            var Management = objmanagement.GetManagement(Building.managementid);
                            if (!string.IsNullOrEmpty(Convert.ToString(Management)))
                            {
                                objFilterControl.companyName = Management.name;
                            }
                            else
                            {
                                objFilterControl.companyName = string.Empty;
                            }
                        }
                        else
                        {
                            objFilterControl.buildingName = string.Empty;
                        }
                        objFilterControl.Address = itm.address;
                        objFilterControl.Price = itm.price;
                        objFilterControl.id = itm.id;
                        objFilterControl.Incentives = itm.incentives;
                        objFilterControl.Layout = itm.layout;
                        objFilterControl.Sqft = itm.sqft;
                        objFilterControl.UnitNumber = itm.unitnumber;
                        objFilterControl.Bathroom = itm.bathroom;
                        objFilterControl.BuildingUnitNumber = itm.unitnumber;
                        lstListing.FilterResult.Add(objFilterControl);
                    }
                }
                return PartialView("_FilterListing", lstListing);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                //Check Authentication
                if (Session["user"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                //Object Initiliaze
                ListingModel model = new ListingModel();
                Listings lstdetails = new Listings();
                listing objlisting = new listing();
                Features objfeatures = new Features();
                Buildings objBuilding = new Buildings();
                List<ListingFeaturesControl> lstListingFeaturesControl = new List<ListingFeaturesControl>();
                BuildingFeatures objbuildingfeatures = new BuildingFeatures();
                List<feature> lstfeat = new List<feature>();
                objlisting = lstdetails.GetListing(id);

                //Start performing the logic section
                List<buildingfeature> newobjBuildingFeatures = objbuildingfeatures.GetBuildingFeature(objlisting.buildingid);                
                foreach (var item in newobjBuildingFeatures)
                {
                    feature objfeat = objfeatures.GetFeatureById(item.id);
                    lstfeat.Add(objfeat);
                }
                foreach (var item in lstfeat)
                {
                    ListingFeaturesControl objListFeat = new ListingFeaturesControl();
                    if (string.IsNullOrEmpty(Convert.ToString(objListFeat)))
                    {
                        objListFeat.features_name = item.features_name;
                        objListFeat.id = item.id;
                        lstListingFeaturesControl.Add(objListFeat);
                    }
                }
                var building = objBuilding.GetBuilding(objlisting.buildingid);
                model.BuildingName = building.Name;
                model.Features = lstListingFeaturesControl;
                model.Address = objlisting.address;
                model.id = objlisting.id;
                model.Price = Convert.ToInt32(objlisting.price);
                model.PropertyType = objlisting.propertytype;
                model.Layout = objlisting.layout;
                model.Bathroom = objlisting.bathroom;
                model.Sqft = objlisting.sqft;
                model.property_type = objlisting.property_type;
                if (objlisting.property_type == "Residential")
                {
                    model.res_type_str = objlisting.res_type;
                }
                else
                {
                    model.com_type_str = objlisting.com_type;
                }
                model.OwnershipType = objlisting.ownershiptype;
                model.UnitNumber = objlisting.unitnumber;
                model.FloorNumber = objlisting.floornumber;
                model.createdAt = (DateTime)objlisting.createddate;
                model.PetPolicyListstr = objlisting.petpolicy;
                model.Minleaseterm = objlisting.minleaseterm;
                model.DateAvailable = (DateTime)objlisting.dateavailable;
                ListingUploads getObjListingUpload = new ListingUploads();

                var filename = getObjListingUpload.GetListingUploadById(objlisting.id, "PHOTOS").Select(t => t.image).FirstOrDefault();
                if (!string.IsNullOrEmpty(filename))
                {
                    var filenamestr = filename.Split('\\');
                    ViewBag.PhotosList = filenamestr[6];
                }
                else
                {
                    ViewBag.PhotosList = "Desert.jpg";
                }
                ViewBag.PhotoCount = getObjListingUpload.GetListingUploadById(objlisting.id, "PHOTOS").Count.ToString();
                return View(model);
            }
            catch (Exception ex)
            {
                //handling exception
                throw ex;
            }
        }

        public ActionResult Add(listing addListing)
        {
            try
            {
                //Check Authentication
                if (Session["user"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                //Object Initiliaze
                ListingModel model = new ListingModel();
                Agents agent = new Agents();
                Buildings building = new Buildings();
                Features feature = new Features();
                //Start performing the logic section
                model.AgentList = agent.GetAgents().Select(t => new ListingAgentControl()
                {
                    Name = t.firstname + " " + t.lastname,
                    Id = t.id,
                    IsCheck = false
                }).ToList();                
                model.BuildingList = building.GetBuildings().Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.id.ToString()
                }).ToList();                
                model.BuildingFeaturesList = feature.GetFeatures().Select(t => new BuildingFeaturesControl()
                {
                    features_name = t.features_name,
                    id = t.id,
                    IsCheck = false
                }).ToList();
                model.OpenHouseList.Add(new OpenHouse());
                return View(model);
            }
            catch (Exception ex)
            {
                //handling exception
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Add(ListingModel model)
        {
            try
            {  
                //Check Authentication
                if (Session["user"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                //Object Initiliaze
                listing _listing = new listing();
                Buildings _building = new Buildings();
                Listings listings = new Listings();
                listingupload _listinguploads = new listingupload();
                ListingUploads listinguploads = new ListingUploads();

                //Start performing the logic section
                var building = _building.GetBuilding(model.BuildingId);
                _listing.address = building.address;
                _listing.propertytype = Convert.ToString(model.PropertyType);
                _listing.ownershiptype = Convert.ToString(model.OwnershipType);
                _listing.unitnumber = model.UnitNumber;
                _listing.floornumber = model.FloorNumber;
                _listing.layout = Convert.ToString(model.LayoutList);
                _listing.bathroom = Convert.ToString(model.BathroomList);
                _listing.sqft = model.Sqft;
                _listing.price = model.Price;
                _listing.dateavailable = model.DateAvailable;
                _listing.minleaseterm = model.Minleaseterm;
                _listing.maxleaseterm = model.Maxleaseterm;
                _listing.furnished = Convert.ToString(model.FurnishedList);
                _listing.furnishedamount = Convert.ToDecimal(model.FurnishedAmount);
                _listing.feestructure = Convert.ToString(model.Feestructure);
                _listing.incentives = Convert.ToString(model.IncentivesList);
                _listing.renterpays = model.Renterpays;
                _listing.commissionpercentage = model.CommissionPercentage;
                _listing.cobrokesplit = model.Cobrokesplit;
                _listing.exclusivetype = Convert.ToString(model.ExclusiveTypeList);
                if (model.Exclusiveagreement != null)
                {
                    var file = model.Exclusiveagreement;
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/images/ExecutiveAgreement/"), fileName);
                        file.SaveAs(path);
                        _listing.exclusiveagreement = path;
                    }
                }
                _listing.petpolicy = Convert.ToString(model.PetPolicyList);
                _listing.unitcondition = Convert.ToString(model.UnitConditionList);
                _listing.unitview = Convert.ToString(model.UnitViewList);
                _listing.unitexposure = Convert.ToString(model.UnitExposure);
                _listing.publicunitdescription = model.PublicUnitDescription;
                _listing.brokerviewinginstructions = model.BrokerViewingInstructions;
                _listing.occupancystatus = Convert.ToString(model.OccupancyStatusList);
                _listing.firstshowingdate = model.FirstShowingDate;
                _listing.keyinoffice = model.KeyInOffice;
                _listing.keydetails = model.KeyDetails;
                _listing.privateunitnotes = model.PrivateUnitNotes;
                _listing.listingprivate = model.ListingPrivate;
                _listing.managementid = 2;
                _listing.createddate = DateTime.Now;
                _listing.updateddate = DateTime.Now;
                _listing.isactive = true;
                _listing.status = 1;
                _listing.buildingid = model.BuildingId;
                _listing.sales_price = model.sales_price;
                _listing.sales_date = model.sales_date;
                _listing.sales_cc_main_fee = model.sales_cc_main_fee;
                _listing.sales_monthly_tax = model.sales_monthly_tax;
                _listing.sales_comm_struct = model.sales_comm_struct;
                _listing.sales_tax_deduction = model.sales_tax_deduction;
                _listing.sales_flip_tax = model.sales_flip_tax;
                _listing.sales_max_fin = model.sales_max_fin;
                _listing.sales_exe_type = Convert.ToString(model.sales_exe_type);
                _listing.property_type = model.property_type;
                if (model.property_type == "Residential")
                {
                    _listing.res_type = Convert.ToString(model.res_type);
                }
                else
                {
                    _listing.com_type = Convert.ToString(model.com_type);
                }
                _listing.com_sqft = model.com_sqft;
                _listing.com_electric_cost = model.com_electric_cost;
                _listing.com_lease_type = Convert.ToString(model.com_lease_type);
                _listing.is_divide = model.is_divide;
                _listing.is_food_ok = model.is_food_ok;
                _listing.is_vented = model.is_vented;
                _listing.listing_type = model.listing_type;
                if (model.PhotoUploadList.Count > 0)
                {
                    _listing.has_photos = true;
                }
                else { _listing.has_photos = false; }
                if (model.FloorUploadList.Count > 0)
                {
                    _listing.has_floorplans = true;
                }
                else { _listing.has_floorplans = false; }

                int Id = listings.AddListing(_listing, model.AgentList);

                if (model.PhotoUploadList.Count > 0)
                {
                    foreach (HttpPostedFileBase photofile in model.PhotoUploadList)
                    {
                        if (photofile != null && photofile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(photofile.FileName);
                            var path = Path.Combine(Server.MapPath("~/images/Photos/"), fileName);
                            photofile.SaveAs(path);
                            _listinguploads.image = path;
                            _listinguploads.type = "PHOTOS";
                            _listinguploads.listingid = Id;
                            listinguploads.AddListingUploads(_listinguploads);
                        }
                    }
                }
                if (model.FloorUploadList.Count > 0)
                {
                    foreach (HttpPostedFileBase floorfile in model.FloorUploadList)
                    {
                        if (floorfile != null && floorfile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(floorfile.FileName);
                            var path = Path.Combine(Server.MapPath("~/images/FloorPlans/"), fileName);
                            floorfile.SaveAs(path);
                            _listinguploads.image = path;
                            _listinguploads.type = "FLOORPLANS";
                            _listinguploads.listingid = Id;
                            listinguploads.AddListingUploads(_listinguploads);
                        }
                    }
                }
                if (model.BuildingFeaturesList.Count > 0)
                {
                    BuildingFeatures buildingFeatures = new BuildingFeatures();
                    foreach (var item in model.BuildingFeaturesList)
                    {
                        buildingfeature newBuildingFeature = new buildingfeature();
                        newBuildingFeature.buildingid = model.BuildingId;
                        newBuildingFeature.featuresid = item.id;
                        buildingFeatures.AddBuildingFeatures(newBuildingFeature);
                    }
                }
                if (model.OpenHouseList.Count > 0)
                {
                    OpenHouses objOpenHouses = new OpenHouses();
                    foreach (var item in model.OpenHouseList)
                    {
                        openhous newOpenHous = new openhous();
                        newOpenHous.appoinmentonly = item.IsAppointmentOnly;
                        newOpenHous.brokeronly = item.IsBrokerOnly;
                        newOpenHous.date = item.openhousedate;
                        newOpenHous.endtime = item.openhouseendtime;
                        newOpenHous.starttime = item.openhousestarttime;
                        newOpenHous.repeat = Convert.ToString(item.RepeatId);
                        newOpenHous.listingid = Id;
                        objOpenHouses.AddOpenHouse(newOpenHous);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //handling exception
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AutoComplete(string searchTerm)
        {
            try
            {
                Buildings buildings = new Buildings();
                var buildingName = (from Buildings in buildings.GetBuildings()
                                    where Buildings.Name.StartsWith(searchTerm)
                                    select new
                                    {
                                        label = Buildings.Name,
                                        val = Buildings.id
                                    }).ToList();

                if (buildingName.Count > 0)
                {
                    return Json(buildingName, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    building buildingterm = new building();
                    buildingterm.Name = searchTerm;
                    buildingterm.managementid = 3;
                    buildingterm.address = "address";
                    buildingterm.ownershiptype = "Post war";
                    buildingterm.unitaccess = "unitaccess";
                    buildingterm.buildingfeatures = "features";
                    buildingterm.phonenumber = "1231231234";
                    buildings.AddBuilding(buildingterm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                //handling exception
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ManagmentAutoComplete(string searchTerm)
        {
            try
            {
                Managements managements = new Managements();
                var managementName = (from Managements in managements.GetManagements()
                                      where Managements.name.StartsWith(searchTerm)
                                      select new
                                      {
                                          label = Managements.name,
                                          val = Managements.id
                                      }).ToList();
                if (managementName.Count > 0)
                {
                    return Json(managementName, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    management manageterm = new management();
                    manageterm.name = searchTerm;
                    manageterm.agnetid = 3;
                    managements.AddManagement(manageterm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                //handling exception
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AddFeature(string addTerm)
        {
            try
            {
                Features features = new Features();
                var featuresName = (from Features in features.GetFeatures()
                                    where Features.features_name.StartsWith(addTerm)
                                    select new
                                    {
                                        label = Features.features_name,
                                        val = Features.id
                                    }).ToList();
                if (featuresName.Count > 0)
                {
                    return Json(featuresName, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    feature featureterm = new feature();
                    featureterm.features_name = addTerm.ToUpper();
                    featureterm.isactive = true;
                    features.AddFeature(featureterm);
                    return null;
                }
            }
            catch (Exception ex)
            {
                //handling exception
                throw ex;
            }
        }


        [HttpGet]
        public JsonResult GetUnitDetail(int buildingId, int UnitNo)
        {
            try
            {
                Units units = new Units();
                var result = units.GetUnits().Where(t => t.buildingid == buildingId && t.unitnumber == Convert.ToString(UnitNo)).FirstOrDefault();
                if (result != null)
                {
                    return Json(new { Success = true, result = result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false, result = result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //handling exception
                throw ex;
            }

        }

        [HttpGet]
        public JsonResult GetOwnerDetails(int ownerid)
        {
            try
            {
                Managements managements = new Managements();
                var result = managements.GetManagements().Where(t => t.id == ownerid).FirstOrDefault();
                return Json(new { Success = true, result = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //handling exception
                throw ex;
            }
        }
    }
}
