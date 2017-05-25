using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Boolean AddDevice(String deviceName, String pidDevice, String vidDevice) {
            Boolean result = false;
            /**
             * code process
             */ 
            return result;
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
    }
}
