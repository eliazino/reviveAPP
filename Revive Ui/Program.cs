using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Revive_Ui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(){
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String DBfile = @"Revive.sqlite";
            if (File.Exists(DBfile)){
                if (File.Exists(@"c:\config\config.txt")){
                    Application.Run(new Form3());
                }else{
                    Application.Run(new Form1());
                }
            }else{
                model.model mod = new model.model();
                mod.createDatabaseFile();
                mod.createTables();
                Application.Run(new Form1());
            }
        }
    }
}
