using System;
using System.IO;

namespace Revive_Ui.Utils
{
    class FileHandler
    {
        string _path = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer) + "\\Revive";
        public FileHandler()
        {

            DataPath = _path;
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }
        }
        public string DataPath
        {
            set { _path = value; }
            get { return _path; }
        }
        public string GetPresentDirFile(string filename)
        {
            return DataPath + "\\" + filename;
        }

        public bool DeleteFile(string filename)
        {
            var conf = false;
            var myfile = DataPath + "\\" + filename;
            try
            {
                File.Delete(myfile);
                conf = true;
            }
            catch (Exception)
            {
                conf = false;
            }
            return conf;
        }
        public bool CheckFileExist(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return false;
            }
            return true;
        }
    }
}
