using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Revive_Ui.model
{
    class dataTables
    {
        public string[] companyName { get; set; }
        public string[] companyID { get; set; }
        public string[] companyemail { get; set; }
        public string[] companyphone { get; set; }
        public string[] companyAddress { get; set; }
        public string[] scrapID { get; set; }
        public string[] scrapName { get; set; }
        public string[] scrapRate { get; set; }
        public string[] transactionID { get; set; }
        public string[] transactionTruck { get; set; }
        public string[] transactionWB { get; set; }
        public string[] transactionWA { get; set; }
        public string[] transactionMtrl { get; set; }
        public string[] transactionDue { get; set; }
        public string[] transactionTime { get; set; }
        public string[] trucksID { get; set; }
        public string[] trucksReg { get; set; }
        public string[] trucksComp { get; set; }
        public string[] trucksLastSeen { get; set; }
        public string[] weightBefore { get; set; }
         public string[] weightAfter{ get; set; }
         public string[] paymentType { get; set; }
        public dataTables companies(string[] cname, string[] ID, string[] address, string[] email, string[] phone){
            companyName = cname;
            companyID = ID;
            companyAddress = address;
            companyemail = email;
            companyphone = phone;
            return new dataTables();
        }
        public dataTables checkOut(string[] company, string[] weightbefore){
            companyName = company;
            weightBefore = weightbefore;
            return new dataTables();
        }
        public dataTables materials(string[] id, string[] names, string[] rate){
            scrapName = names;
            scrapRate = rate;
            scrapID = id;
            return new dataTables();
        }
        public dataTables transactions(string[] comp, string[] truck, string[] wb, string[] wa, string[] mtr, string[] amt, string[] pt, string[] ttime){
            companyName = comp;
            trucksReg = truck;
            transactionWB = wa;
            transactionWA = wa;
            transactionMtrl = mtr;
            transactionDue = amt;
            paymentType = pt;
            transactionTime = ttime;
            return new dataTables();
        }
        public dataTables trucks(string[] companyName, string[] vehicleReg, string[] email, string[] address, string[] phone){
            trucksReg = vehicleReg;
            trucksComp = companyName;
            companyemail = email;
            companyphone = phone;
            companyAddress = address;
            return new dataTables();
        }
    }
}
