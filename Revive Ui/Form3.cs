// Decompiled with JetBrains decompiler
// Type: Revive_Ui.Form3
// Assembly: Revive Ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C40F0DE1-91BB-41D2-88ED-72C2530A0EDE
// Assembly location: C:\Users\waheed\Desktop\some_folder\Works\seesharp\Revive Ui\Revive Ui\bin\Debug\Revive Ui.exe

using MetroFramework;
using MetroFramework.Forms;
using Newtonsoft.Json.Linq;
using Revive_Ui.model;
using Revive_Ui.Properties;
using Revive_Ui.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Revive_Ui
{
  public partial class Form3 : MetroForm
  {
    public Form3()
    {
      this.InitializeComponent();
      //this.Load += new EventHandler(this.Form3_Load);
    }
    private void label1_Click(object sender, EventArgs e)
    {
    }

    private void groupBox1_Enter(object sender, EventArgs e)
    {
    }

    private void groupBox2_Enter(object sender, EventArgs e)
    {
    }

    private void label8_Click(object sender, EventArgs e)
    {
    }

    private void textBox4_TextChanged(object sender, EventArgs e)
    {
    }

    private void biil_truck_Click(object sender, EventArgs e)
    {
    }

    private void label11_Click(object sender, EventArgs e)
    {
    }

    private void textBox9_TextChanged(object sender, EventArgs e)
    {
    }

    private void label10_Click(object sender, EventArgs e)
    {
    }

    private void label9_Click(object sender, EventArgs e)
    {
    }

    private void textBox5_TextChanged(object sender, EventArgs e)
    {
    }

    private void label6_Click(object sender, EventArgs e)
    {
    }

    private void textBox3_TextChanged(object sender, EventArgs e)
    {
    }

    private void textBox8_TextChanged(object sender, EventArgs e)
    {
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void Settings_Click(object sender, EventArgs e)
    {
    }

    private void btnCheckIn_Click(object sender, EventArgs e)
    {
      string text1 = this.txtCompanyNameIn.Text;
      string text2 = this.txtWeightBeforeIn.Text;
      string text3 = this.txtVehicleNoIn.Text;
      string text4 = this.cboxUnitIn.Text;
      string text5 = this.cboxVehicleTypeIn.Text;
      if (text1 != string.Empty && text2 != string.Empty && text3 != string.Empty)
      {
        double result;
        if (double.TryParse(text2, out result))
        {
          double num = double.Parse(text2);
          if (text4 == "Tons")
            num *= 907.18474;
          string statement1 = "insert into checks (truckReg, weightBefore, company, weightAfter) values ('" + text3 + "', '" + (object) num + "', '" + text1 + "',0)";
          string statement2 = "select*from checks where truckReg = '" + text3 + "' and weightAfter = 0";
          try
          {
            Revive_Ui.model.model model = new Revive_Ui.model.model();
            if (model.colExists(statement2) == 1)
            {
              this.label28.Text = "Check in was already in place for " + text3 + ". Continue to check out";
            }
            else
            {
              model.execQuery(statement1);
              this.label28.Text = "Check in was succesfull";
            }
          }
          catch (Exception ex)
          {
          }
        }
        else
        {
          int num1 = (int) MessageBox.Show("The initial weight is not valid", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else
      {
        int num2 = (int) MessageBox.Show("Please fill the important fields", "Revive Alert", MessageBoxButtons.OK);
      }
    }

    private void txtVehicleNo_TextChanged(object sender, EventArgs e)
    {
      string text = this.txtVehicleNo.Text;
      this.txtCompanyOut.Text = "";
      this.txtWeightOut.Text = "";
      if (text.Length <= 0)
        return;
      string statement = "select checks.company, checks.weightBefore as weightBefore from checks where checks.truckReg = '" + text + "' and checks.weightAfter = 0";
      try
      {
        dataTables dataTables = new Revive_Ui.model.model().fetchCheckOut(statement);
        if (dataTables.companyName.Length > 0)
        {
          this.cboxUnitOut.SelectedIndex = 0;
          this.txtWeightOut.Text = dataTables.weightBefore[0];
          this.txtCompanyOut.Text = dataTables.companyName[0];
        }
        else
          Console.WriteLine("some Found not");
      }
      catch (Exception)
      {
      }
    }

    private void btnGenerateInvoice_Click(object sender, EventArgs e)
    {
      string text1 = this.txtCompanyOut.Text;
      string text2 = this.txtWeightOut.Text;
      int selectedIndex = this.cboxUnitOnLoad.SelectedIndex;
      string text3 = this.cboxMaterials.Text;
      string text4 = this.txtVehicleNo.Text;
      string text5 = this.txtWeightOnLoad.Text;
      string str = "";
      double result;
      if (double.TryParse(text5, out result))
      {
        double num1 = double.Parse(text2);
        double num2 = double.Parse(text5);
        if (selectedIndex == 1)
          num2 *= 907.18474;
        if (num1 > num2)
        {
          int num3 = (int) MessageBox.Show("There seems to be error with the final Weight. Make sure you have selected the appropriate Unit ", "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          try
          {
            string[] strArray = this.label32.Text.Split(',');
            if (this.cboxMaterials.SelectedIndex >= 0)
            {
              double num4 = (num2 - num1) * double.Parse(strArray[this.cboxMaterials.SelectedIndex]);
              if (this.rbtnCashPayment.Checked)
                str = "Cash";
              else if (this.rdbCardPayment.Checked)
              {
                str = "Card";
              }
              else
              {
                int num5 = (int) MessageBox.Show("Payment Method has not been selected", "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              }
              if (str.Length > 1)
              {
                int totalSeconds = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                string statement1 = "update checks set weightAfter = '" + (object) num2 + "', material = '" + text3 + "', amountDue = '" + (object) num4 + "', transactionTime = '" + (object) totalSeconds + "', company= '" + text1 + "' where truckReg = '" + text4 + "'";
                string statement2 = "insert into transactionlog (company, truck, weightBefore, weightAfter, material, amountDue, transactionTime, paymentType) values('" + text1 + "','" + text4 + "', '" + (object) num1 + "', '" + (object) num2 + "', '" + text3 + "', '" + (object) num4 + "', '" + (object) totalSeconds + "', '" + str + "')";
                Revive_Ui.model.model model = new Revive_Ui.model.model();
                model.execQuery(statement1);
                model.execQuery(statement2);
                int num6 = (int) MessageBox.Show("Transaction completed!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              }
            }
            else
            {
              int num7 = (int) MessageBox.Show("You have not selected a material", "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
      else
      {
        int num = (int) MessageBox.Show("The weight is not in correct format", "Thats an Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void Form3_Shown(object sender, EventArgs e)
    {
      string path1 = "c:\\config\\config.txt";
      string path2 = "Revive.sqlite";
      if (System.IO.File.Exists(path1))
      {
        int index = 0;
        string[] strArray = new string[10];
        StreamReader streamReader = new StreamReader(path1);
        string str;
        while ((str = streamReader.ReadLine()) != null)
        {
          strArray[index] = str;
          ++index;
        }
        streamReader.Close();
        if (System.IO.File.Exists(path2))
        {
          int num = (int) MessageBox.Show("Welcome Back again", "Agent ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.pictureBox3.Visible = false;
          this.label14.Text = strArray[2];
        }
        else
        {
          int num = (int) MessageBox.Show("Something seems to be missing with Authentication, Kindly try again", "Account Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.Close();
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Yikes!! Something went totally wrong! Log in again. Thats the solution", " Exiting Software", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
    }

    private void txtPspCompany_KeyUp(object sender, KeyEventArgs e)
    {
      string text = this.txtPspCompany.Text;
      this.comboBox1.Items.Clear();
      this.label5.Text = "";
      this.label25.Text = "";
      this.label38.Text = "";
      this.label39.Text = "";
      this.label40.Text = "";
      if (text.Length <= 1)
        return;
      string statement = "select*from company where companyName like '%" + text + "%'";
      try
      {
        dataTables dataTables = new Revive_Ui.model.model().fetchCompanies(statement);
        if (dataTables.companyName.Length > 0)
        {
          Console.WriteLine("some Found");
          this.label25.Text = string.Join(",", dataTables.companyID);
          this.label38.Text = string.Join("¬", dataTables.companyAddress);
          this.label39.Text = string.Join("¬", dataTables.companyphone);
          this.label40.Text = string.Join("¬", dataTables.companyemail);
          for (int index = 0; index < dataTables.companyName.Length; ++index)
            this.comboBox1.Items.Add((object) dataTables.companyName[index]);
          this.comboBox1.Visible = true;
        }
        else
        {
          Console.WriteLine("some Found not");
          this.comboBox1.Visible = false;
        }
      }
      catch (Exception ex)
      {
      }
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
      string text1 = this.label5.Text;
      string text2 = this.txtPspCompany.Text;
      string text3 = this.txtNoOfTruck.Text;
      string text4 = this.txtPhoneNo.Text;
      string text5 = this.txtEmail.Text;
      string text6 = this.txtAddress.Text;
      int num1 = 0;
      if (text3.Length < 1 || text1.Length < 1)
      {
        int num2 = (int) MessageBox.Show("Input error occured, important fields cannot be empty", "Input errors", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string statement1 = "select*from trucks where truckRegNumber ='" + text3 + "'";
        try
        {
          num1 = new Revive_Ui.model.model().colExists(statement1);
        }
        catch (Exception ex)
        {
        }
        if (num1 == 0)
        {
          if (text1.Length < 1)
          {
            int num3 = (int) MessageBox.Show("Please select from the suggestion to continue", "Invalid Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            double num4 = double.Parse(text1);
            int totalSeconds = (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            string statement2 = "insert into trucks (truckRegNumber, company, lastseen) values ('" + text3 + "', '" + (object) num4 + "', '" + (object) totalSeconds + "')";
            try
            {
              new Revive_Ui.model.model().inserts(statement2);
              this.label36.Text = "Insert truck was succesful";
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message);
            }
          }
        }
        else
        {
          int num5 = (int) MessageBox.Show("Duplicate entry! The truck already exist", "Duplicate Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      string[] strArray1 = this.label25.Text.Split(',');
      string[] strArray2 = this.label38.Text.Split('¬');
      string[] strArray3 = this.label39.Text.Split('¬');
      string[] strArray4 = this.label40.Text.Split('¬');
      this.label5.Text = strArray1[this.comboBox1.SelectedIndex];
      this.txtPspCompany.Text = this.comboBox1.Text;
      this.txtEmail.Text = strArray4[this.comboBox1.SelectedIndex];
      this.txtAddress.Text = strArray2[this.comboBox1.SelectedIndex];
      this.txtPhoneNo.Text = strArray3[this.comboBox1.SelectedIndex];
    }

    private void comboBox1_MouseMove(object sender, MouseEventArgs e)
    {
    }

    private void txtVehicleNoIn_KeyUp(object sender, KeyEventArgs e)
    {
      string text = this.txtVehicleNoIn.Text;
      if (text.Length <= 0)
        return;
      string statement = "select company.companyName as companyName, company.email as email, company.address as address, company.phone as phone, company.id as id from trucks left join company on trucks.company = company.id where trucks.truckRegNumber = '" + text + "'";
      try
      {
        dataTables dataTables = new Revive_Ui.model.model().fetchCompanies(statement);
        if (dataTables.companyName.Length > 0){
          Console.WriteLine("some Found");
          this.txtCompanyNameIn.Text = dataTables.companyName[0];
        }
        else
        {
          this.txtCompanyNameIn.Text = "";
          Console.WriteLine("some Found not");
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Thats an error");
      }
    }

    private void tabControl1_Selected(object sender, TabControlEventArgs e)
    {
      if (this.tabControl1.SelectedIndex == 1)
      {
        Console.WriteLine("yes");
        try
        {
          this.cboxMaterials.Items.Clear();
          dataTables dataTables = new Revive_Ui.model.model().fetchMaterials("select*from scrapmaterials");
          int index = 0;
          this.label31.Text = string.Join(",", dataTables.scrapID);
          this.label32.Text = string.Join(",", dataTables.scrapRate);
          for (; index < dataTables.scrapID.Length; ++index)
            this.cboxMaterials.Items.Add((object) dataTables.scrapName[index]);
        }
        catch (Exception ex)
        {
        }
      }
      else if (this.tabControl1.SelectedIndex == 3)
      {
        try
        {
          this.comboBox2.Items.Clear();
          dataTables dataTables = new Revive_Ui.model.model().fetchMaterials("select*from scrapmaterials");
          int index = 0;
          this.label34.Text = string.Join(",", dataTables.scrapID);
          this.label35.Text = string.Join(",", dataTables.scrapRate);
          for (; index < dataTables.scrapID.Length; ++index)
            this.comboBox2.Items.Add((object) dataTables.scrapName[index]);
        }
        catch (Exception ex)
        {
        }
      }
      else if (this.tabControl1.SelectedIndex == 4)
      {
        string statement = "select * from transactionlog";
        this.transactionsTab.Rows.Clear();
        foreach (DataGridViewBand column in (BaseCollection) this.transactionsTab.Columns)
          column.DefaultCellStyle.Font = new Font("Arial", 10f, GraphicsUnit.Pixel);
        try
        {
          dataTables dataTables = new Revive_Ui.model.model().fetchTransactions(statement);
          for (int index = 0; index < dataTables.trucksReg.Length; ++index)
          {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(double.Parse(dataTables.transactionTime[index])).ToLocalTime();
            this.transactionsTab.Rows.Add((object) (index + 1), (object) dateTime.Date, (object) dataTables.companyName[index], (object) dataTables.trucksReg[index], (object) dataTables.transactionWB[index], (object) dataTables.transactionWA[index], (object) dataTables.transactionMtrl[index], (object) dataTables.transactionDue[index], (object) dataTables.paymentType[index]);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
      else
      {
        if (this.tabControl1.SelectedIndex != 5)
          return;
        string statement = "select truckRegNumber, lastSeen, companyName, email, address, phone from trucks left join company on company.id = trucks.company";
        this.trucksTab.Rows.Clear();
        foreach (DataGridViewBand column in (BaseCollection) this.trucksTab.Columns)
          column.DefaultCellStyle.Font = new Font("Arial", 10f, GraphicsUnit.Pixel);
        try
        {
          dataTables dataTables = new Revive_Ui.model.model().fetchVehicles(statement);
          for (int index = 0; index < dataTables.trucksReg.Length; ++index)
            this.trucksTab.Rows.Add((object) (index + 1), (object) dataTables.trucksComp[index], (object) dataTables.companyAddress[index], (object) dataTables.trucksReg[index], (object) dataTables.companyemail[index], (object) dataTables.companyemail[index]);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      string text1 = this.txtTruckRate.Text;
      string text2 = this.settingsmaterialPrice.Text;
      double result;
      if (text1.Length <= 1 || text2.Length <= 1 || !double.TryParse(text2, out result))
        return;
      double num = double.Parse(text2);
      string statement = "insert into scrapmaterials (materialName, materialRate) values ('" + text1 + "', '" + (object) num + "')";
      Revive_Ui.model.model model = new Revive_Ui.model.model();
      model.execQuery(statement);
      this.txtTruckRate.Text = "";
      this.settingsmaterialPrice.Text = "";
      this.label33.Text = "The material has been added!";
      try
      {
        this.comboBox2.Items.Clear();
        dataTables dataTables = model.fetchMaterials("select*from scrapmaterials");
        int index = 0;
        this.label34.Text = string.Join(",", dataTables.scrapID);
        this.label35.Text = string.Join(",", dataTables.scrapRate);
        for (; index < dataTables.scrapID.Length; ++index)
          this.comboBox2.Items.Add((object) dataTables.scrapName[index]);
      }
      catch (Exception ex)
      {
      }
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.textBox2.Text = this.label35.Text.Split(',')[this.comboBox2.SelectedIndex];
    }

    private void btnAdjustPrice_Click(object sender, EventArgs e)
    {
      cPrompt cPrompt = new cPrompt();
      int num1 = (int) cPrompt.ShowDialog();
      if (cPrompt.setted)
      {
        string statement1 = "select*from agent where agentPassword = '" + cPrompt.Text1 + "'";
        try
        {
          Revive_Ui.model.model model = new Revive_Ui.model.model();
          if (model.colExists(statement1) == 1)
          {
            string text = this.textBox2.Text;
            string str = this.label34.Text.Split(',')[this.comboBox2.SelectedIndex];
            double result;
            if (!double.TryParse(text, out result))
              return;
            string statement2 = "update scrapmaterials set materialRate = '" + text + "' where id = '" + str + "'";
            try
            {
              model.execQuery(statement2);
              this.label33.Text = "The material price has been updated!";
              try
              {
                this.comboBox2.Items.Clear();
                dataTables dataTables = model.fetchMaterials("select*from scrapmaterials");
                int index = 0;
                this.label34.Text = string.Join(",", dataTables.scrapID);
                this.label35.Text = string.Join(",", dataTables.scrapRate);
                for (; index < dataTables.scrapID.Length; ++index)
                  this.comboBox2.Items.Add((object) dataTables.scrapName[index]);
              }
              catch (Exception ex)
              {
              }
            }
            catch (Exception ex)
            {
            }
          }
          else
          {
            int num2 = (int) MessageBox.Show("Access is Denied for user", "Access Control", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        catch (Exception ex)
        {
        }
      }
      else
      {
        int num3 = (int) MessageBox.Show("Access Denied", "N", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void txtSearchCar_KeyUp(object sender, KeyEventArgs e)
    {
      string text = this.txtSearchCar.Text;
      string statement = text.Length <= 2 ? "select truckRegNumber, lastSeen, companyName, email, address, phone from trucks left join company on company.id = trucks.company" : "select truckRegNumber, lastSeen, companyName, email, address, phone from trucks left join company on company.id = trucks.company where companyName like '%" + text + "%'";
      this.trucksTab.Rows.Clear();
      foreach (DataGridViewBand column in (BaseCollection) this.trucksTab.Columns)
        column.DefaultCellStyle.Font = new Font("Arial", 10f, GraphicsUnit.Pixel);
      try
      {
        dataTables dataTables = new Revive_Ui.model.model().fetchVehicles(statement);
        for (int index = 0; index < dataTables.trucksReg.Length; ++index)
          this.trucksTab.Rows.Add((object) (index + 1), (object) dataTables.trucksComp[index], (object) dataTables.companyAddress[index], (object) dataTables.trucksReg[index], (object) dataTables.companyemail[index], (object) dataTables.companyemail[index]);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }

    private void reg_truck_Click(object sender, EventArgs e)
    {
    }

    private void pictureBox4_Click(object sender, EventArgs e)
    {
      string path = "c:\\config\\config.txt";
      if (!System.IO.File.Exists(path))
        return;
      int index = 0;
      string[] strArray = new string[10];
      try
      {
        StreamReader streamReader = new StreamReader(path);
        string str;
        while ((str = streamReader.ReadLine()) != null)
        {
          strArray[index] = str;
          ++index;
        }
        streamReader.Close();
        string c = strArray[4];
        this.trySyncCompany(strArray[5], strArray[6], c);
      }
      catch (Exception ex)
      {
      }
    }

    public void trySyncCompany(string a, string b, string c)
    {
      string requestUriString = string.Format("https://sanwocore.herokuapp.com/organization/getbyissuer/" + c);
      if (b.Length <= 1 || a.Length <= 1)
        return;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUriString);
        httpWebRequest.Method = "GET";
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Headers.Add("i", a);
        httpWebRequest.Headers.Add("a", b);
        using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
          {
            foreach (JToken jtoken in (IEnumerable<JToken>) JObject.Parse(streamReader.ReadToEnd())["data"])
            {
              string str1 = jtoken[(object) "name"].ToString();
              string str2 = jtoken[(object) "address"].ToString();
              string str3 = jtoken[(object) "phone"].ToString();
              string str4 = jtoken[(object) "email"].ToString();
              new Revive_Ui.model.model().execQuery("insert into company (companyName, address, email, phone) values ('" + str1 + "', '" + str2 + "', '" + str4 + "', '" + str3 + "')");
            }
            int num = (int) MessageBox.Show("The update was succesful", "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }
   }
}
