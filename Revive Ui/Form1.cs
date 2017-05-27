using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MetroFramework.Forms;

namespace Revive_Ui
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public static bool IsConnected(){
            try{
                string myAddress = "www.google.com";
                IPAddress[] addresslist = Dns.GetHostAddresses(myAddress);
                if (addresslist[0].ToString().Length > 6) {
                    return true;
                } else {
                    return false;
                }

            } catch {
                return false;
            }
        }
        public static async Task<String> PostFormUrlEncoded<TResult>(string url, IEnumerable<KeyValuePair<string, string>> postData) {
            using (var httpClient = new HttpClient()) {
                using (var content = new FormUrlEncodedContent(postData)){
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        public static bool Login(string username, string password){
            bool conf = false;
            try{
                var URL = Revive_Ui.Properties.Resources.StagingBaseUrl.ToString() + "user/authenticate";
                List<KeyValuePair<String, String>> values = new List<KeyValuePair<String, String>>();
                KeyValuePair<String, String> user = new KeyValuePair<string, string>("identifier", username);
                KeyValuePair<String, String> utype = new KeyValuePair<string, string>("user_type", "4");
                KeyValuePair<String, String> pass = new KeyValuePair<string, string>("password", password);
                KeyValuePair<String, String> device = new KeyValuePair<string, string>("device_code", "wwwwa1");
                values.Add(user);
                values.Add(utype);
                values.Add(pass);
                values.Add(device);
                string x = (Task.Run(async () => await PostFormUrlEncoded<string>(URL, values))).Result;
                JObject serverResponse = JObject.Parse(x);
                if (int.Parse(serverResponse["status"].ToString()) == 1){
                    string fullname = serverResponse["data"]["profile"]["data"]["fullname"].ToString();
                    string address = serverResponse["data"]["profile"]["data"]["address"].ToString();
                    string telephone = serverResponse["data"]["profile"]["data"]["telephone"].ToString();
                    string email = serverResponse["data"]["profile"]["data"]["email"].ToString();
                    string balance = serverResponse["data"]["device"]["balance"].ToString();
                    string issuerID = serverResponse["data"]["device"]["issuer_id"].ToString();
                    string userID = serverResponse["data"]["_id"].ToString();
                    string accessToken = serverResponse["data"]["access_token"].ToString();
                    string local = email + "\r\n" + username + "\r\n" + fullname + "\r\n"+telephone+ "\r\n"+issuerID+ "\r\n"+userID+ "\r\n"+accessToken;
                    // Write the string to a file.
                    try{
                        System.IO.StreamWriter file = new System.IO.StreamWriter(@"c:\config\config.txt");
                        file.WriteLine(local);
                        file.Close();
                        model.model mod = new model.model();
                        Int32 lastseen = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        string Q = @"insert into agent (agentEmail, agentID, agentPassword, lastseen, device_ID, fullname, address, telephone, balance) values ('" + email + "', '" + username + "', '" + password + "', '" + lastseen + "', 'wwwwa1', '" + fullname + "', '" + address + "', '" + telephone + "', '" + balance + "')";
                        mod.execQuery(Q);
                        conf = true;
                    }catch (Exception emx){
                        MessageBox.Show(emx.Message, "Exception!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }else{
                    MessageBox.Show("Access Denied. Incorrect username or passord", "Server Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conf = false;
                }
            }catch(Exception Ex){
                MessageBox.Show(Ex.Message);
                conf = false;
            }
            return conf;
        }
        private void pictureBox2_Click(object sender, EventArgs e) {
            //this.Hide();
            if (IsConnected()){
                string agentID = textBox2.Text;
                string password = textBox1.Text;
                if (agentID.Length > 1 && password.Length > 1){
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    pictureBox2.Enabled = false;
                    pictureBox6.Visible = true;
                    bool response = Login(agentID, password);
                    if (response){
                        Form3 f3 = new Form3();
                        this.Hide();
                        f3.Show();
                    }else {
                        textBox1.Enabled = true;
                        textBox2.Enabled = true;
                        pictureBox2.Enabled = true;
                        pictureBox6.Visible = false;
                        MessageBox.Show("something is wrong");
                    }
                }else{
                    MessageBox.Show("You cannot leave any field empty", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else {
                MessageBox.Show("Network is unavailable. Please connect and try again", "Network currently unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
