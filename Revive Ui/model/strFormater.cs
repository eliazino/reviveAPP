using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Revive_Ui.model
{
	class strFormater{
		public string[] amount { get; set; }
		public string[] card_charge_fee { get; set; }
		public string[] card_balance { get; set; }
		public string[] created_time { get; set; }
		public string[] transaction_code { get; set; }
		public string[] device_charge_fee { get; set; }
		public string[] type { get; set; }
		public string[] serial_number { get; set; }
		public string[] meta_data { get; set; }
		public string mObj { get; set; }
		/*public string json = @"{'bulk_trans':[
					{
						'amount':'5000','card_charge_fee':'0','card_balance':0,'created_time':'2017-5-26 16:7:17','transaction_code':'wwwwa1-5909be499665661100a2059d-38378834-bb04-44ab-b2ef-d9832792d032','device_charge_fee':150,'type':'2',
							'meta_data':{
	'ticket':1,'user_type':'Worker','company_name':'','worker_type':'Scavenger','transaction_type':'1'
					},
							'serial_number':'1b93ac1b'
										}
]

,{'amount':'5000','card_charge_fee':'0','card_balance':0,'created_time':'2017-5-26 16:7:17','transaction_code':'wwwwa1-5909be499665661100a2059d-c69fc48b-5f62-419b-ac1d-3a711556e484','device_charge_fee':150,'type':'1','meta_data':{'ticket':1,'user_type':'Worker','company_name':'','worker_type':'Scavenger','transaction_type':'1'},'serial_number':'1b93ac1b'}


,{'amount':'1000','card_charge_fee':'0','card_balance':0,'created_time':'2017-5-26 16:5:23','transaction_code':'wwwwa1-5909be499665661100a2059d-43209a64-d2d8-4e9c-8129-eccbafdd51ec','device_charge_fee':30,'type':'2','meta_data':{'ticket':1,'user_type':'Company','company_name':'company 3','worker_type':'','transaction_type':'1'},'serial_number':'1b93ac1b'},{'amount':'1000','card_charge_fee':'0','card_balance':0,'created_time':'2017-5-26 16:5:23','transaction_code':'wwwwa1-5909be499665661100a2059d-787bcade-9939-456b-bcce-70fc8688e147','device_charge_fee':30,'type':'1','meta_data':{'ticket':1,'user_type':'Company','company_name':'company 3','worker_type':'','transaction_type':'1'},'serial_number':'1b93ac1b'}],'holder_id':'5909be499665661100a2059d','device_id':'5909be009665661100a20597'}

";*/
		public strFormater setParams(string[] iamount, string[] icardCFee, string[] icBalance, string[] icDate, string[] itcode, string[] ideviceCF, string[] ityp, string[] icomp_name, string[] transType,  string[] iserial, string[] deviceID, string[] agentID){
			if(iamount.Length > 0){
				JObject bulkObject = new JObject();
				JArray transactionsArray = new JArray();
				for (int s = 0; s < iamount.Length; s++){
					JObject transactionObject = new JObject();
					transactionObject.Add("amount", iamount[s]);
					transactionObject.Add("card_charge_fee", icardCFee[s]);
					transactionObject.Add("card_balance", icBalance[s]);
					transactionObject.Add("created_time", icDate[s]);
					transactionObject.Add("transaction_code", itcode[s]);
					transactionObject.Add("device_charge_fee", ideviceCF[s]);
					transactionObject.Add("type", "");
					JObject meta_data = new JObject();
					meta_data.Add("ticket", "0");
					meta_data.Add("user_type", ityp[s]);
					meta_data.Add("company_name", icomp_name[s]);
					meta_data.Add("worker_type", "");
					meta_data.Add("transaction_type", transType[s]);
					meta_data.Add("serial_number", iserial[s]);
					transactionObject.Add("meta_data", meta_data);
					transactionsArray.Add(transactionObject);
				}
				bulkObject.Add("bulk_trans", transactionsArray);
				bulkObject.Add("device_id", deviceID[0]);
				bulkObject.Add("holder_id", agentID[0]);
				mObj = bulkObject.ToString();
			}
			/*else{
				JObject bulkObject = new JObject();
				JArray transactionsArray = new JArray();
				for (int s = 0; s < iamount.Length; s++){
					JObject transactionObject = new JObject();
					transactionObject.Add("amount", iamount[s]);
					transactionObject.Add("card_charge_fee", icardCFee[s]);
					transactionObject.Add("card_balance", icBalance[s]);
					transactionObject.Add("created_time", icDate[s]);
					transactionObject.Add("transaction_code", itcode[s]);
					transactionObject.Add("device_charge_fee", ideviceCF[s]);
					transactionObject.Add("type", "");
					JObject meta_data = new JObject();
					meta_data.Add("ticket", "0");
					meta_data.Add("user_type", ityp[s]);
					meta_data.Add("company_name", icomp_name[s]);
					meta_data.Add("worker_type", "");
					meta_data.Add("transaction_type", transType[s]);
					meta_data.Add("serial_number", iserial[s]);
					transactionObject.Add("meta_data",meta_data);
					transactionsArray.Add(transactionObject);
				}
				bulkObject.Add("bulk_trans", transactionsArray);

			}*/
			return new strFormater();
		}
	}
}
