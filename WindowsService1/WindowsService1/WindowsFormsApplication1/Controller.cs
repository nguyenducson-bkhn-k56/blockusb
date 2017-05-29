using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace WindowsFormsApplication1
{
    class Controller
    {
        /**
         * 
         */
        public Boolean Login(String userName, String passWord) {
            try
            {
                // login data
                Process process = Process.getInstance();
                return process.verifyPassword(userName, passWord);
            }
            catch (Exception ex) {
                return false;
            }
            
        }

        /**
         * 
         */
        public Boolean EditDevice(DataTable tableDevice)
        {
            /**
             * code process
             */ 
            Process process = Process.getInstance();
            if (tableDevice == null)
                return false;
            List<Device> lstDevice = new List<Device>();
            foreach (DataRow row in tableDevice.Rows) {
                Device device = new Device();
                device.nameDevice = row[Constant.DeviceTable.DEVICE_NAME].ToString();
                device.pidDevice = row[Constant.DeviceTable.DEVICE_PID].ToString();
                device.vidDevice = row[Constant.DeviceTable.DEVICE_VID].ToString();
                lstDevice.Add(device);
            }
            return process.editFileDevice(lstDevice);
        }

        /**
         * 
         */
        public Boolean RemoveDevice(String pidDevice,String vidDevice) {
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
         */
        public List<Device> getAllDevice()
        {
            Process process = Process.getInstance();
            return process.getAllDevice();
        }
    }
}
