using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public class Device {
        public String nameDevice { get; set; }

        public String pidDevice { get; set; }

        public String vidDevice { get; set; }
    }

    public class Process
    {
        private const String nameFolder = "/local/UsbConnection/";
        private const String nameFileDevice = "data.dat";
        // instance la thuc the 
        private static Process instance;
        private String folderPath = null;
        private static MD5 md5;
        protected Process() { 
        
        }

        public static Process getInstance() {
            if (instance == null) {
                instance = new Process();
                md5 = MD5.Create();
            }
            return instance;
        }

        /**
        * 
        */
        public Boolean Login(String userName, String passWord)
        {
            Boolean result = false;
     
            /**
             *code process  
             */
            return result;
        }

        /**
         * 
         */
        public Boolean AddDevice(String deviceName, String pidDevice, String vidDevice)
        {
            Boolean result = false;
            /**
             * code process
             */
            return result;
        }

        /**
         * 
         */
        public Boolean RemoveDevice(String pidDevice, String vidDevice)
        {
            Boolean result = false;
            /**
             * code process
             */
            return result;
        }

        /**
         * 
         */
        public Boolean RemoveDevice(int index)
        {
            Boolean result = false;
            /**
             * code process
             */
            return result;
        }

        /**
         * 
         * verify password 
         */ 
        public Boolean verifyPassword(String userName, String passWord) {
            Boolean result = false;
            try
            {
                if (userName == null || passWord == null)
                    return result;
                // get md5 hash
                String passEncrypt = GetMd5Hash(md5, passWord);
                // get user password from file
                String passFromFile = getPasswordByUser(userName);
                if (passFromFile != null)
                {
                    if (passFromFile == passEncrypt)
                        result = true;
                }
            }
            catch (Exception ex) {
                Console.Write(ex.ToString());
            }
            return result;
        }

        /**
         * 
         * 
         */
        public String getPasswordByUser(String userName) {
            String result= null;
            try
            {
                folderPath = AppDomain.CurrentDomain.BaseDirectory;
                String filePath = folderPath + "/" + Constant.File_LOGIN_NAME;
                if (!File.Exists(filePath))
                    throw new Exception("file not exist");
                StreamReader reader = new StreamReader(filePath);
                string allData = reader.ReadToEnd().Replace("/r/n", "==").Replace("/r/n", "==");
                string[] userArray = Regex.Split(allData, "==");
                foreach (string userRow in userArray){
                    String[] userInfo = userRow.Split(' ');
                    if (userInfo.Length < 2)
                        continue;
                    if (userInfo[0].Equals(userName))
                    {
                        result = userInfo[1];
                        break;
                    }
                }
            }
            catch (Exception ex) {
                Console.Write(ex.ToString());
            }
            return result;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public Boolean editFileDevice(List<Device> lstDevice){
            try
            {
                String pathAppfolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                String filePath = pathAppfolder + nameFolder + nameFileDevice;
                if (Directory.Exists(pathAppfolder + nameFolder)) {
                    Directory.CreateDirectory(pathAppfolder + nameFolder);
                }
                using (StreamWriter writer = File.Exists(filePath) ? File.AppendText(filePath): File.CreateText(filePath)) {
                    // remove all file 
                    File.WriteAllText(filePath, String.Empty);
                    // write all data to file
                    if (lstDevice != null && lstDevice.Count > 0)
                    {
                        foreach (Device device in lstDevice)
                        {
                            StringBuilder line = new StringBuilder();
                            line.Append(device.nameDevice);
                            line.Append(' ');
                            line.Append(device.pidDevice);
                            line.Append(' ');
                            line.Append(device.vidDevice);
                            writer.WriteLine(line.ToString());
                        }
                    }                    
                    writer.Close();
                    writer.Dispose();
                }
                
            }
            catch (Exception ex) { 
                
            }
            return false;
        }

        /***
         * lay danh sach cac thiet bi
         */ 
        public List<Device> getAllDevice() {
            List<Device> lstDevice = new List<Device>();
            try
            {
                String pathAppfolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                String filePath = pathAppfolder + nameFolder + nameFileDevice;
                if (!File.Exists(filePath)) {
                    return null;
                }
                StreamReader reader = new StreamReader(filePath);
                String allData = reader.ReadToEnd().Replace("\n\r","==").Replace("\r\n","==");
                String[] allLine = Regex.Split(allData, "==");
                if (allLine != null && allLine.Count() > 0) {
                    foreach (String line in allLine) {
                        Device device = new Device();
                        String[] infoDevice  = line.Split(' ');
                        try
                        {
                           device.nameDevice =  infoDevice[0];
                           device.pidDevice = infoDevice[1];
                           device.vidDevice = infoDevice[2];
                           lstDevice.Add(device);
                        }
                        catch (IndexOutOfRangeException ex) { 
                            // logger
                        }
                    }  
                }
            }
            catch (Exception ex) { 

            }
            return lstDevice;
        } 
    }
}
