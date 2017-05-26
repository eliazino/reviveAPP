using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;

namespace Revive_Ui.model
{
    class model{
        public void createDatabaseFile(){
            try {
                SQLiteConnection.CreateFile(@"Revive.sqlite");
                FileInfo f = new FileInfo("Revive.sqlite");
                string fullname = f.FullName;
            }
            catch (Exception e){
                //YuCk Yea!! Error there.
                MessageBox.Show(e.ToString(),"Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private SQLiteConnection _generateCon(){
            SQLiteConnection con = new SQLiteConnection("data source=Revive.sqlite");
            return con;
        }
        private SQLiteCommand _generateCom(SQLiteConnection con){
            return new SQLiteCommand(con);
        }
        public void createTables(){
            String agentTable = @"CREATE TABLE `agent` (
	            `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	            `agentEmail`	varchar(200) DEFAULT NULL,
	            `agentID`	varchar(200) NOT NULL,
	            `agentPassword`	varchar(200),
	            `lastSeen`	INTEGER NOT NULL,
	            `device_ID`	TEXT,
	            `fullname`	TEXT,
	            `address`	TEXT,
	            `telephone`	TEXT,
	            `balance`	TEXT
            )";
            String companyTable = "CREATE TABLE IF NOT EXISTS [company] ( [id] INTEGER NOT NULL PRIMARY KEY, [companyName] varchar(200) NOT NULL, [email] varchar(200)";
            companyTable = companyTable+" DEFAULT NULL, [address] varchar(200) NOT NULL, [phone] varchar(20) NOT NULL)";
            String trucksTable = "CREATE TABLE IF NOT EXISTS [trucks] ( [id] INTEGER NOT NULL PRIMARY KEY, [truckRegNumber] varchar(100) NOT NULL, [company]";
            trucksTable = trucksTable+" INTEGER, [lastSeen] INTEGER NOT NULL, [truckType] varchar(200))";
            String scrapMaterials = "CREATE TABLE IF NOT EXISTS [scrapmaterials] ( [id] INTEGER NOT NULL PRIMARY KEY, [materialName] text NOT NULL,  [materialRate]";
            scrapMaterials = scrapMaterials+" INTEGER NOT NULL)";
            String transactionLog = "CREATE TABLE IF NOT EXISTS [transactionlog] ( [id] INTEGER NOT NULL PRIMARY KEY, [truck] varchar(200), [weightBefore] varchar(200)";
            transactionLog = transactionLog + ", [weightAfter] varchar(200),  [material] varchar(200),[company] varchar(500),  [amountDue] varchar(200),";
            transactionLog = transactionLog +" [transactionTime] varchar(200) NOT NULL, [paymentType] varchar(20))";
            String checks = @"CREATE TABLE IF NOT EXISTS [checks] ( [id] INTEGER NOT NULL PRIMARY KEY, [truckReg] varchar(200), [weightBefore] varchar(200)";
            checks = checks + @", [weightAfter] varchar(200),  [material] varchar(200),  [amountDue] varchar(200),";
            checks = checks + @" [transactionTime] varchar(200), [company] varchar(200))";
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = agentTable;
                command.ExecuteNonQuery();
                command.CommandText = companyTable;
                command.ExecuteNonQuery();
                command.CommandText = trucksTable;
                command.ExecuteNonQuery();
                command.CommandText = scrapMaterials;
                command.ExecuteNonQuery();
                command.CommandText = transactionLog;
                command.ExecuteNonQuery();
                command.CommandText = checks;
                command.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception e) {
                //YuCk Yea!! Another Error there. muuuuuuuu!!!!!!!!!
                //File.Delete(@"Revive.sqlite");
                MessageBox.Show(e.ToString(), "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool execQuery(String statement){
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                command.ExecuteNonQuery();
                con.Close();
                return true;
            }catch(Exception Exc){
                MessageBox.Show(Exc.ToString(), "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public long inserts(String statement){
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                command.ExecuteNonQuery();
                string sql = @"select last_insert_rowid()";
                command.CommandText = sql;
                var lastId = (long)command.ExecuteScalar();
                Console.WriteLine(lastId);
                con.Close();
                return lastId;
            }catch (Exception Exc){
                MessageBox.Show(Exc.ToString(), "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        public int colExists(string statement){
            int t = 0;
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                command.ExecuteNonQuery();
                SQLiteDataReader fetch = command.ExecuteReader();
                while(fetch.Read()){
                    if (fetch["id"].ToString().Length == 0){
                        t = 0;
                    }else{
                        t = 1;
                    }
                    break;
                }
                con.Close();
                return t;
            }catch (Exception Exc){
                MessageBox.Show(Exc.ToString(), "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        public dataTables fetchCompanies(string statement){
            var data = new dataTables();
            var cnames = new List<string>();
            var id = new List<string>();
            var email = new List<string>();
            var phone = new List<string>();
            var address = new List<string>();
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                SQLiteDataReader fetch = command.ExecuteReader();
                while(fetch.Read()){
                    cnames.Add(fetch["companyName"].ToString());
                    email.Add(fetch["email"].ToString());
                    phone.Add(fetch["phone"].ToString());
                    address.Add(fetch["address"].ToString());
                    id.Add(fetch["id"].ToString());
                }
                string[] cnamesArr = cnames.ToArray();
                string[] idArr = id.ToArray();
                string[] emailAd = email.ToArray();
                string[] phoneAd = phone.ToArray();
                string[] addr = address.ToArray();
                data.companies(cnamesArr, idArr, addr, emailAd, phoneAd);
                con.Close();
                return data;
            }catch(Exception e){
                MessageBox.Show(e.Message, "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return data;
            }
        }
        public void dropTable(string statement){
            try{
                statement = @"delete from checks where true";
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                command.ExecuteNonQuery();
                con.Close();
            }catch (Exception){

            }
        }
        public dataTables fetchCheckOut(string statement){
            var data = new dataTables();
            var cnames = new List<string>();
            var wbefore = new List<string>();
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                SQLiteDataReader fetch = command.ExecuteReader();
                while (fetch.Read()){
                    cnames.Add(fetch["company"].ToString());
                    wbefore.Add(fetch["weightBefore"].ToString());
                }
                string[] cnamesArr = cnames.ToArray();
                string[] weighbf = wbefore.ToArray();
                //Console.WriteLine(weighbf[0]);
                data.checkOut(cnamesArr, weighbf);
                con.Close();
                return data;
            }
            catch (Exception){
                //MessageBox.Show(e.Message, "Thats an Error in fetching", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return data;
            }
        }
        public dataTables fetchTransactions(string statement){
            var data = new dataTables();
            var companies = new List<string>();
            var vehicle = new List<string>();
            var wbefore = new List<string>();
            var wafter = new List<string>();
            var material = new List<string>();
            var amount = new List<string>();
            var ptype = new List<string>();
            var ttime = new List<string>();
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                SQLiteDataReader fetch = command.ExecuteReader();
                while(fetch.Read()){
                    companies.Add(fetch["company"].ToString());
                    Console.WriteLine(fetch["company"].ToString());
                    vehicle.Add(fetch["truck"].ToString());
                    wbefore.Add(fetch["weightBefore"].ToString());
                    wafter.Add(fetch["weightAfter"].ToString());
                    material.Add(fetch["material"].ToString());
                    amount.Add(fetch["amountDue"].ToString());
                    ptype.Add(fetch["paymentType"].ToString());
                    ttime.Add(fetch["transactionTime"].ToString());
                    data.transactions(companies.ToArray(), vehicle.ToArray(), wbefore.ToArray(), wafter.ToArray(), material.ToArray(), amount.ToArray(), ptype.ToArray(), ttime.ToArray());
                }
                con.Close();
                return data;
            }catch (Exception){
                return data;
            }
        }
        public dataTables fetchVehicles(string statement){
            var data = new dataTables();
            var companies = new List<string>();
            var vehicle = new List<string>();
            var email = new List<string>();
            var address = new List<string>();
            var phone = new List<string>();
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                SQLiteDataReader fetch = command.ExecuteReader();
                while (fetch.Read()){
                    companies.Add(fetch["companyName"].ToString());
                    vehicle.Add(fetch["truckRegNumber"].ToString());
                    email.Add(fetch["email"].ToString());
                    address.Add(fetch["address"].ToString());
                    phone.Add(fetch["phone"].ToString());
                    Console.WriteLine(fetch["companyName"].ToString());
                    data.trucks(companies.ToArray(), vehicle.ToArray(), email.ToArray(), address.ToArray(), phone.ToArray());
                }
                con.Close();
                return data;
            }
            catch (Exception)
            {
                return data;
            }
        }
        public dataTables fetchMaterials(string statement){
            var data = new dataTables();
            var mnames = new List<string>();
            var mIDs = new List<string>();
            var mRates = new List<string>();
            try{
                SQLiteConnection con = _generateCon();
                SQLiteCommand command = _generateCom(con);
                con.Open();
                command.CommandText = statement;
                SQLiteDataReader fetch = command.ExecuteReader();
                while (fetch.Read()){
                    mnames.Add(fetch["materialName"].ToString());
                    mIDs.Add(fetch["id"].ToString());
                    mRates.Add(fetch["materialRate"].ToString());
                }
                string[] cnamesArr = mnames.ToArray();
                string[] idArr = mIDs.ToArray();
                string[] cRateArr = mRates.ToArray();
                data.materials(idArr, cnamesArr, cRateArr);
                con.Close();
                return data;
            }catch (Exception e){
                MessageBox.Show(e.Message, "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return data;
            }
        }
    }
}
