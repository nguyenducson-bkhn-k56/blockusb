using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
namespace WindowsService1
{
    public class Device {
        public String nameDevice { get; set; }
        public String pidDevice { get; set; }
        public String vidDevice { get; set; }
        public Boolean state { get; set; }

        public int numberCalled { get; set; }
    }

    public class DeviceProcess
    {
        private const String nameFolder = "/local/UsbConnection/";
        private const String nameFileDevice = "data.dat";
        private const String nameData = "";
        private static DeviceProcess dataProcess;
        private static List<Device> lstDeviceAllow;
        protected DeviceProcess() { 
            
        }

        /***
         * singleton DataProcess
         */
        public static DeviceProcess getInstance()
        {
            if (dataProcess == null) {
                dataProcess = new DeviceProcess();
                lstDeviceAllow = new List<Device>();
            }
            return dataProcess;
        }

        public void getLstDeviceAllow(){
            try
            {
       
                lstDeviceAllow.Clear();
                String pathAppfolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                String filePath = pathAppfolder + nameFolder + nameFileDevice;
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string data = reader.ReadToEnd().Replace("\n\r", "==").Replace("\r\n", "==");
                        string[] lstInfoData = Regex.Split(data, "==");
                        if(lstInfoData!=null)
                        foreach (String device in lstInfoData) {
                            string[] deviceInfo = device.Split(' ');
                            try
                            {
                                Device newDevice = new Device();
                                newDevice.nameDevice = deviceInfo[0];
                                newDevice.pidDevice = deviceInfo[1];
                                newDevice.vidDevice = deviceInfo[2];
                                lstDeviceAllow.Add(newDevice);
                            }
                            catch (IndexOutOfRangeException ex) { 
                                // viet log ra
                            }
                        }
                        reader.Close();
                        reader.Dispose();
                    }
                }
                else {
                    if (!Directory.Exists(pathAppfolder + nameFolder)) {
                        Directory.CreateDirectory(pathAppfolder + nameFolder);
                    }

                    using (StreamWriter writer = File.AppendText(filePath))
                    { 
                        
                    }
                }
            }
            catch (Exception ex) { 

            }
            
        }

        // kiem tra xem thiet bi co trong danh sach cho phep khong
        public Boolean isAllowDivice(Device deviceInput) {
            try
            {
                if (lstDeviceAllow == null || lstDeviceAllow.Count == 0)
                    return false;
                foreach (Device device in lstDeviceAllow) {
                    if (deviceInput.vidDevice == null || deviceInput.pidDevice == null)
                        continue;
                    if (deviceInput.vidDevice == device.vidDevice && deviceInput.pidDevice == device.pidDevice) {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            { 

            }
            return false;
        }
    }
}
