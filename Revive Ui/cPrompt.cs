using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revive_Ui
{
    public partial class cPrompt : MetroForm {
         public string Text1 { get; private set; }
         public bool setted = false;
        //public string password = {get; private;}
        public cPrompt(){
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e){
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e){
            if (textBox1.Text.Length > 1) {
                this.Text1 = textBox1.Text;
                setted = true;
                this.Hide();
            }else{

            }
        }
    }
}
