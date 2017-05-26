using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Revive_Ui.Utils
{
    class Entity
    {
        List<string> _message = new List<string>();
        private const string SettingsFile = "Settings.xmlt ";
        private const string TransactionFile = "CompanyProfile.xmlt ";
        private const string CheckInFile = "Checkins.xmlt ";
        private XDocument _doc;
        readonly string _presentsettingsfile;
        readonly string _presentprofilefile;
        readonly string _presentcheckinfile;
        private FileHandler filehandler;
        public Entity()
        {
            filehandler = new FileHandler();

            _presentsettingsfile = filehandler.GetPresentDirFile(SettingsFile);
            _presentprofilefile = filehandler.GetPresentDirFile(TransactionFile);
            _presentcheckinfile = filehandler.GetPresentDirFile(CheckInFile);
        }
        public bool SaveAccess(object o)
        {
            bool conf = false;
            try
            {
                var filehandle = new FileHandler();
                if (!filehandle.CheckFileExist(_presentsettingsfile))
                {
                    var access = NewAccess(o);
                    access.Save(_presentsettingsfile);
                    _message.Add("User Data Persisted");
                    conf = true;
                }
                else
                {
                    var doc = XElement.Load(_presentsettingsfile);
                    var access = AddAccess(o);
                    doc.Add(access);
                    doc.Save(_presentsettingsfile);
                    _message.Add("User Data Updated");
                    conf = true;
                }
            }
            catch (Exception ex)
            {
                conf = false;
                _message.Add(ex.Message);
            }
            return conf;
        }

        //SaveSerial
        public bool SaveCheckIn(object o)
        {
            bool conf = false;
            try
            {
                var filehandle = new FileHandler();
                if (!filehandle.CheckFileExist(_presentcheckinfile))
                {
                    var serial = NewSerial(o);
                    serial.Save(_presentcheckinfile);
                    _message.Add("CheckIn Data Persisted");
                    conf = true;
                }
                else
                {
                    var doc = XElement.Load(_presentcheckinfile);
                    var serial = AddSerial(o);
                    doc.Add(serial);
                    doc.Save(_presentcheckinfile);
                    _message.Add("CheckIn Data Updated");
                    conf = true;
                }
            }
            catch (Exception ex)
            {
                conf = false;
                _message.Add(ex.Message);
            }
            return conf;
        }

        public bool SaveProfile(object o)
        {
            bool conf = false;
           /* try
            {*/
                var filehandle = new FileHandler();
                if (!filehandle.CheckFileExist(_presentprofilefile))
                {
                    var profile = NewProfile(o);
                    profile.Save(_presentprofilefile);
                    _message.Add("User Profile Persisted");
                    conf = true;
                }
                else
                {
                    var doc = XElement.Load(_presentprofilefile);
                    var profile = AddProfile(o);
                    doc.Add(profile);
                    doc.Save(_presentprofilefile);
                    _message.Add("User Profile Updated");
                    conf = true;
                }
           /* }
            catch (Exception ex)
            {
                conf = false;
                _message.Add(ex.Message);
            }*/
            return conf;
        }

        private XDocument NewAccess(object param)
        {
            var accessparams = (LoginAcess)param;
            _doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("User Access Persistent"),
                new XElement("AllAccess",
                             new XElement("Access",
                                          new XAttribute("Username", accessparams.Username),
                                           new XAttribute("AccessToken", accessparams.AccessToken),
                                           new XAttribute("Identifier", accessparams.Identifier))));
            //new XAttribute("Session", userparams.Session))));
            return _doc;
        }
        private XElement AddAccess(object parameters)
        {
            var accessparams = (LoginAcess)parameters;
            var accessfield = new XElement("Access",
                                          new XAttribute("Username", accessparams.Username),
                                          new XAttribute("AccessToken", accessparams.AccessToken),
                                          new XAttribute("Identifier", accessparams.Identifier));
            return accessfield;

        }

        //For saving the serialNumber
        private XDocument NewSerial(object param)
        {
            var checkedIn = (CheckedIn)param;
            _doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("User Access Persistent"),
                new XElement("AllCheckedIn",
                             new XElement("SerialCheckedin",
                                          new XAttribute("CardId", checkedIn.CardId),
                                           new XAttribute("Status", checkedIn.Status),
                                           new XAttribute("VehicleId", checkedIn.VehicleId),
                                           new XAttribute("VehicleWeightIn", checkedIn.VehicleWeightIn),
                                           new XAttribute("VehicleWeightOut", checkedIn.VehicleWeightOut),
                                           new XAttribute("UnitIn", checkedIn.UnitIn),
                                           new XAttribute("UnitOut", checkedIn.UnitOut),
                                           new XAttribute("CompanyName", checkedIn.CompanyName),
                                           new XAttribute("CheckedInDate", checkedIn.CheckedInDate))));
            //new XAttribute("Session", userparams.Session))));
            return _doc;
        }
        private XElement AddSerial(object parameters)
        {
            var checkedIn = (CheckedIn)parameters;
            var checkedInfield = new XElement("SerialCheckedin",
                                          new XAttribute("CardId", checkedIn.CardId),
                                          new XAttribute("Status", checkedIn.Status),
                                           new XAttribute("VehicleId", checkedIn.VehicleId),
                                          new XAttribute("VehicleWeightIn", checkedIn.VehicleWeightIn),
                                           new XAttribute("VehicleWeightOut", checkedIn.VehicleWeightOut),
                                           new XAttribute("UnitIn", checkedIn.UnitIn),
                                           new XAttribute("UnitOut", checkedIn.UnitOut),
                                           new XAttribute("CompanyName", checkedIn.CompanyName),
                                          new XAttribute("CheckedInDate", checkedIn.CheckedInDate));
            return checkedInfield;
            
        }

        public XElement UpdateSerial(string vehicleId, CheckedIn parameters)
        {
             var query = from checkin in _doc.Descendants("SerialCheckedin")
                         where checkin.Attribute("VehicleId").Value == vehicleId
                        select new CheckedIn
                        {
                            VehicleId = checkin.Attribute("VehicleId").Value,
                            CheckedInDate = checkin.Attribute("CheckedInDate").Value,
                            VehicleWeightIn = checkin.Attribute("VehicleWeightIn").Value,
                            VehicleWeightOut = checkin.Attribute("VehicleWeightOut").Value,
                            UnitIn = checkin.Attribute("UnitIn").Value,
                            UnitOut = checkin.Attribute("UnitOut").Value,
                            CompanyName = checkin.Attribute("CompanyName").Value

                        };
            if (query.Any())
            {
                CheckedIn result = query.LastOrDefault();
                var checkedInfield = new XElement("SerialCheckedin",
                                          new XAttribute("CardId", result.CardId),
                                          new XAttribute("Status", "1"),
                                           new XAttribute("VehicleId", result.VehicleId),
                                          new XAttribute("VehicleWeightIn", parameters.VehicleWeightIn),
                                           new XAttribute("VehicleWeightOut", parameters.VehicleWeightOut),
                                           new XAttribute("UnitIn", parameters.UnitIn),
                                           new XAttribute("UnitOut", parameters.UnitOut),
                                           new XAttribute("CompanyName", parameters.CompanyName),
                                          new XAttribute("CheckedInDate", result.CheckedInDate),
                                          new XAttribute("CheckedOutDate", parameters.CheckedOutDate));
                return checkedInfield;

            }
            return null;
        }
        public List<CheckedIn> GetAllCheckedIns()
        {
            _doc = XDocument.Load(_presentcheckinfile);
            var query = from checkin in _doc.Descendants("SerialCheckedin")
                        where checkin.Attribute("Status").Value != "0"
                        select new CheckedIn
                        {
                            VehicleId = checkin.Attribute("VehicleId").Value,
                            CheckedInDate = checkin.Attribute("CheckedInDate").Value,
                            VehicleWeightIn = checkin.Attribute("VehicleWeightIn").Value,
                            VehicleWeightOut = checkin.Attribute("VehicleWeightOut").Value,
                            UnitIn = checkin.Attribute("UnitIn").Value,
                            UnitOut = checkin.Attribute("UnitOut").Value,
                            CompanyName = checkin.Attribute("CompanyName").Value

                        };
            if (query.Any()) return query.ToList();
            return null;
        }

        public CheckedIn GetCheckedInByVehicleId(string vehicleId)
        {
            _doc = XDocument.Load(_presentcheckinfile);
            var query = from checkin in _doc.Descendants("SerialCheckedin")
                        where checkin.Attribute("VehicleId").Value == vehicleId && checkin.Attribute("Status").Value == "0"
                        select new CheckedIn
                        {
                            VehicleId = checkin.Attribute("VehicleId").Value,
                            CheckedInDate = checkin.Attribute("CheckedInDate").Value,
                            VehicleWeightIn = checkin.Attribute("VehicleWeightIn").Value,
                            VehicleWeightOut = checkin.Attribute("VehicleWeightOut").Value,
                            UnitIn = checkin.Attribute("UnitIn").Value,
                            UnitOut = checkin.Attribute("UnitOut").Value,
                            CompanyName = checkin.Attribute("CompanyName").Value

                        };
            if (query.Any()) return query.LastOrDefault();
            return null;
        }

        public List<string> GetAllVehicleId()
        {
            _doc = XDocument.Load(_presentcheckinfile);
            var query = from checkin in _doc.Descendants("SerialCheckedin")
                where checkin.Attribute("VehicleId").Value != ""
                select checkin.Attribute("VehicleId").Value;
                
            if (query.Any()) return query.ToList();
            return null;
        }

        public bool CheckIfCheckedIn(string cardId)
        {
            bool conf = false;
            foreach (var chkIns in GetAllCheckedIns().Where(chkIns => chkIns.CardId == cardId))
            {
                conf = true;
            }
            return conf;
        }

        public bool CheckIfCheckedOut(string cardId)
        {
            bool conf = false;
            foreach (var chkIns in GetAllCheckedOuts().Where(chkIns => chkIns.CardId == cardId))
            {
                conf = true;
            }
            return conf;
        }

        public List<CheckedIn> GetAllCheckedOuts()
        {
            _doc = XDocument.Load(_presentcheckinfile);
            var query = from checkin in _doc.Descendants("SerialCheckedin")
                        where checkin.Attribute("Status").Value != "1"
                        select new CheckedIn
                        {
                            CardId = checkin.Attribute("CardId").Value,
                            CheckedInDate = checkin.Attribute("CheckedInDate").Value

                        };
            if (query.Any()) return query.ToList();
            return null;
        } 
        //For the profile

        private XDocument NewProfile(object param)
        {
            var profileparams = (Profile)param;
            _doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Profiles"),
                new XElement("AllProfiles",
                             new XElement("Profiles",
                                          new XAttribute("ComapanyName", profileparams.CompanyName),
                                          new XAttribute("Address", profileparams.Address),
                                          new XAttribute("Email", profileparams.Email),
                                           new XAttribute("SerialNumber", profileparams.SerialNumber),
                                           new XAttribute("PhoneNumber", profileparams.PhoneNumber),
                                           new XAttribute("NoOfTrucks", profileparams.NoOfTrucks),
                                           new XAttribute("IssuedDate", profileparams.IssuedDate))));
            //new XAttribute("Session", userparams.Session))));
            return _doc;
        }
        private XElement AddProfile(object parameters)
        {
            var profileparams = (Profile)parameters;
            var profilefield = new XElement("Profiles",
                                          new XAttribute("ComapanyName", profileparams.CompanyName),
                                          new XAttribute("Address", profileparams.Address),
                                          new XAttribute("Email", profileparams.Email),
                                           new XAttribute("SerialNumber", profileparams.SerialNumber),
                                           new XAttribute("PhoneNumber", profileparams.PhoneNumber),
                                           new XAttribute("NoOfTrucks", profileparams.NoOfTrucks),
                                           new XAttribute("IssuedDate", profileparams.IssuedDate));
            return profilefield;

        }

        public List<Profile> GetAllProfile()
        {
            _doc = XDocument.Load(_presentprofilefile);
            var query = from profile in _doc.Descendants("Profiles")
                where profile.Attribute("SerialNumber").Value != ""
                select new Profile
                {
                    CompanyName = profile.Attribute("ComapanyName").Value,
                    Address = profile.Attribute("Address").Value,
                    SerialNumber = profile.Attribute("SerialNumber").Value,
                    Email = profile.Attribute("Email").Value,
                    PhoneNumber = profile.Attribute("PhoneNumber").Value,
                    NoOfTrucks = profile.Attribute("NoOfTrucks").Value,
                    IssuedDate = profile.Attribute("IssuedDate").Value

                };
            if (query.Any()) return query.ToList();
            return null;
        } 
        public List<LoginAcess> GetAllLoggedInUsernames()
        {

            _doc = XDocument.Load(_presentsettingsfile);
            var query = from user in _doc.Descendants("Access")
                        where user.Attribute("Username").Value != ""
                        select new LoginAcess
                        {
                            Username = user.Attribute("Username").Value,
                            AccessToken = user.Attribute("AccessToken").Value,
                            Identifier = user.Attribute("Identifier").Value

                        };
            if (query.Any()) return query.ToList();
            return null;
        }

        public string GetAccessToken()
        {
            string token = "";
            try
            {
                _doc = XDocument.Load(_presentsettingsfile);
                var query = from user in _doc.Descendants("Access")
                    where user.Attribute("Username").Value != ""
                    select user.Attribute("AccessToken");
                token = query.FirstOrDefault().Value;
            }
            catch (Exception)
            {
                
            }
            return token;
        }

        public string GetIdentifier()
        {
            string identifier = "";
            try
            {
                _doc = XDocument.Load(_presentsettingsfile);
                var query = from user in _doc.Descendants("Access")
                            where user.Attribute("Username").Value != ""
                            select user.Attribute("Identifier");
                identifier = query.FirstOrDefault().Value;
            }
            catch (Exception)
            {

            }
            return identifier;
        }

        public bool DeleteItems(string SerialNum)
        {
            bool conf = false;
            try
            {
                _doc = XDocument.Load(_presentprofilefile);
                var query = from profile in _doc.Descendants("Profiles")
                    where profile.Attribute("SerialNumber").Value == SerialNum
                    select profile;
                query.Remove();
            }
            catch (Exception)
            {
                
            }
            return conf;
        }
        public List<string> Message { get { return _message; } }
    }



    public class LoginAcess
    {
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public string Identifier { get; set; }
    }

    public class CheckedIn
    {
        public string CardId { get; set; }
        public string VehicleId { get; set; }
        public string Status { get; set; }//0 checked in, 1 checked out
        public string CheckedInDate { get; set; }

        public string VehicleWeightIn { get; set; }

        public string VehicleWeightOut { get; set; }

        public string UnitIn { get; set; }
        public string UnitOut { get; set; }

        public object CompanyName { get; set; }

        public object CheckedOutDate { get; set; }
    }

   

    public class Profile
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string SerialNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string IssuedDate { get; set; }
        public object NoOfTrucks { get; set; }
    }
}
