using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Revive_Ui.model
{
	class profile
	{
		public string agentEmail { get; set; }
		public string agentUsername { get; set; }
		public string agentFullname { get; set; }
		public string agentPhone { get; set; }
		public string agentIssuerID { get; set; }
		public string agentUserID { get; set; }
		public string agentToken { get; set; }
		public string agentbalance { get; set; }
		public string deviceID { get; set; }
		public profile profiler(){
			string path = "c:\\config\\config.txt";
			if (File.Exists(path)){
				string[] strArray = new string[10];
				try{
					StreamReader streamReader = new StreamReader(path);
					string str;
					int index = 0;
					while ((str = streamReader.ReadLine()) != null){
						if (index == 0) this.agentEmail = str;
						if (index == 1) this.agentUsername = str;
						if (index == 2) this.agentFullname = str;
						if (index == 3) this.agentPhone = str;
						if (index == 4) this.agentIssuerID = str;
						if (index == 5) this.agentUserID = str;
						if (index == 6) this.agentToken = str;
						++index;
					}
					streamReader.Close();
					model mod = new model();
					var modi = mod.fetchProfile("select*from agent where agentEmail = '" + this.agentEmail + "'");
					agentbalance = modi[0];
					deviceID = modi[1];
				}catch (Exception){
				}
			}
			
			return new profile();
		}
	}
}
