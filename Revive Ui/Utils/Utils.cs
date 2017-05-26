using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Revive_Ui.Utils
{
    public class Utils
    {
        public Utils()
        {
            
        }
        public static bool IsConnected()
        {
            try
            {
                string myAddress = "www.google.com";
                IPAddress[] addresslist = Dns.GetHostAddresses(myAddress);

                if (addresslist[0].ToString().Length > 6)
                {
                    return true;
                }
                else
                    return false;

            }
            catch
            {
                return false;
            }

        }
        public static bool Login(string username, string password)
        {
            bool conf = false;
            var URL = string.Format("{0}{1}", Properties.Resources.StagingBaseUrl, "/user/authenticate");
            if (username != "" && password != "")
            {
                //JObject accessJObject = new JObject();
                JObject jsonObject = new JObject();

                jsonObject.Add("identifier", username);
                jsonObject.Add("password", password);
                // jsonObject.Add("device_code", SUtils.GetHDDSerialNo());
                //jsonObject.Add("api_key", jsonObject);
                //MessageBox.Show(jsonObject.ToString());
                //Do Portal Login
                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
                    //request.Headers["api_key"] = Properties.Resources.api_key;
                    //request.Headers.Add(Constant.KEY_API_KEY, Constant.api_key);

                    request.Method = "POST";
                    request.ContentType = "application/json";
                    byte[] byteArray = Encoding.ASCII.GetBytes(jsonObject.ToString());
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    WebResponse response = request.GetResponse();
                    //MessageBox.Show(((HttpWebResponse)response).StatusDescription);
                    dataStream = response.GetResponseStream();

                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    //ParseResponse(responseFromServer);
                    //Entity myEntity = new Entity();
                    //myEntity.SaveAccess(responseFromServer);
                    //MessageBox.Show(responseFromServer);

                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    JObject serverResponse = JObject.Parse(responseFromServer);
                    if (int.Parse(serverResponse["status"].ToString()) == 1)
                    {
                        //Utils.AddHmo(responseFromServer);
                        string mObject = JObject.Parse(responseFromServer)["data"].ToString();
                        JObject userDetails = JObject.Parse(mObject);
                        string token = userDetails["access_token"].ToString();
                        string identifier = userDetails["_id"].ToString();
                        //AccessToken = token;
                        Entity db = new Entity();
                        LoginAcess a = new LoginAcess { Username = username, AccessToken = token,Identifier = identifier};
                        db.SaveAccess(a);
                        /*JObject jObject = JObject.Parse(getAllHospitals());
                        JArray mObjects = JArray.Parse(jObject["data"].ToString());

                        var hospitals = db.GetAllHospitals();
                        int i = 0;
                        foreach (var item in mObjects)
                        {
                            Hospital hospital = new Hospital();
                            hospital.Name = item[Constant.KEY_NAME].ToString();
                            hospital.Address = item[Constant.KEY_ADDRESS].ToString();
                            hospital.HospiatlId = item[Constant.KEY_USERID].ToString();
                            if (hospitals.Count == 0)
                            {
                                db.AddHospital(hospital);
                            }
                            i++;
                        }*/
                        conf = true;
                    }
                    else
                    {
                        conf = false;
                    }
                }
                catch (Exception)
                {
                   // MessageBox.Show(ex.Message);
                }

            }
            return conf;
        }
        //Entity ent = new Entity();
        public static JObject getJSOBObject(Profile profile)
        {
            DateTime date = DateTime.Now;
            JObject jprofile = new JObject();
            jprofile.Add("fullname", profile.CompanyName);
            jprofile.Add("gender", profile.Address);
            jprofile.Add("phone", profile.PhoneNumber);
            jprofile.Add("email", profile.Email);
            JObject jObject = new JObject();
            jObject.Add("profiles", jprofile);
            jObject.Add("organization", "sanwo");
            jObject.Add("serial_number", profile.SerialNumber);
            jObject.Add("reference_number", Guid.NewGuid().ToString().Substring(0, 8));
            jObject.Add("expiry_date", "2019");
            jObject.Add("tag_type", "1");
            jObject.Add("issuer_id", "15");
            jObject.Add("batch_number", string.Format("{0}-{1}", date.Year, date.Month));
            jObject.Add("email", profile.Email);
            return jObject;

        }
        public static string SynRegistration()
        {
            string message = "{}";
            try
            {
                var URL = string.Format("{0}{1}", Properties.Resources.StagingBaseUrl,
                    string.Format("/card/provision/register/bulk"));
                JArray jArray = new JArray();
                foreach (var user in new Entity().GetAllProfile())
                {
                    jArray.Add(getJSOBObject(user));
                }
                JObject obj = new JObject();
                obj.Add("regprovision", jArray.ToString());
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
                //MessageBox.Show(jArray.ToString());
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers["a"] = new Entity().GetAccessToken();
                request.Headers["i"] = new Entity().GetIdentifier();
                string payload = obj.ToString();
                byte[] byteArray = Encoding.ASCII.GetBytes(payload);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                //MessageBox.Show(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                //ParseResponse(responseFromServer);
                //Entity myEntity = new Entity();
                //myEntity.SaveAccess(responseFromServer);
                //MessageBox.Show(responseFromServer);

                reader.Close();
                dataStream.Close();
                response.Close();
                message = responseFromServer;
            }
            catch (Exception)
            {

            }
            return message;
        }
    }
}
