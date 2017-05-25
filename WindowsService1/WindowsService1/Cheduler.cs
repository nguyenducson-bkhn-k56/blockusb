using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Runtime.InteropServices;
using System.IO;
using HardwareHelperLib;
using System.Threading;

namespace WindowsService1
{

    public partial class Cheduler : ServiceBase
    {
        struct Device {
            public bool state;
            public String pid;
            public String vid;
            public String deviceName;
        }

        #region Fields

		private FileSystemWatcher fileSystemWatcher;
		private IntPtr deviceNotifyHandle;
		private IntPtr deviceEventHandle;
		private IntPtr directoryHandle;
		private Win32.ServiceControlHandlerEx myCallback;
        private bool flagDisable;
        private List<Device> lstDevice;
        private Thread threadMain;
        
        #endregion

		#region USB Detection

		private int ServiceControlHandler(int control, int eventType, IntPtr eventData, IntPtr context)
		{
            try
            {
              if (control == Win32.SERVICE_CONTROL_STOP || control == Win32.SERVICE_CONTROL_SHUTDOWN)
                {
                   UnregisterHandles();
                    Win32.UnregisterDeviceNotification(deviceEventHandle);
                    base.Stop();
                }
                else if (control == Win32.SERVICE_CONTROL_DEVICEEVENT)
                {
                    switch (eventType)
                    {
                        case Win32.DBT_DEVICEARRIVAL:
                            Win32.DEV_BROADCAST_HDR hdr;
                            hdr = (Win32.DEV_BROADCAST_HDR)
                                Marshal.PtrToStructure(eventData, typeof(Win32.DEV_BROADCAST_HDR));
                            if (hdr.dbcc_devicetype == Win32.DBT_DEVTYP_DEVICEINTERFACE)
                            {
                                Win32.DEV_BROADCAST_DEVICEINTERFACE deviceInterface;
                                deviceInterface = (Win32.DEV_BROADCAST_DEVICEINTERFACE)
                                    Marshal.PtrToStructure(eventData, typeof(Win32.DEV_BROADCAST_DEVICEINTERFACE));
                                //Save Device name
                                String deviceName = new string(deviceInterface.dbcc_name);
                                if (deviceName != null && deviceName.Contains("VID"))
                                {
                                    Device device = new Device();
                                    device.deviceName = deviceName;
                                    device.state = false;
                                    device.pid = deviceName.Substring(deviceName.IndexOf("PID"), 8);
                                    device.vid = deviceName.Substring(deviceName.IndexOf("VID"), 8);
                                    lstDevice.Add(device);
                                }
                                flagDisable = true;
                            }
                            break;
                        case Win32.DBT_DEVICEQUERYREMOVE:
                            UnregisterHandles();
                            fileSystemWatcher.EnableRaisingEvents = false;
                            break;
                        case Win32.DBT_DEVNODESCHANGED:
                            if (flagDisable)
                            {
                                //HH_Lib hhlib = new HH_Lib();
                                //StringBuilder devicePid = new StringBuilder();
                                //devicePid.Append(vidDevice);
                                //devicePid.Append("&");
                                //devicePid.Append(pidDevice);
                                //String[] temp = new String[1];
                                //temp[0] = devicePid.ToString();
                                //flagDisable = !hhlib.SetDeviceState(temp, false);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex) { 
                    
            }
            
			return 0;
		}

		private void UnregisterHandles()
		{
			if (directoryHandle != IntPtr.Zero)
			{
				Win32.CloseHandle(directoryHandle);
				directoryHandle = IntPtr.Zero;
			}
			if (deviceNotifyHandle != IntPtr.Zero)
			{
				Win32.UnregisterDeviceNotification(deviceNotifyHandle);
				deviceNotifyHandle = IntPtr.Zero;
			}
		}

		private void RegisterForHandle(char c)
		{
			Win32.DEV_BROADCAST_HANDLE deviceHandle = new Win32.DEV_BROADCAST_HANDLE();
			int size = Marshal.SizeOf(deviceHandle);
			deviceHandle.dbch_size = size;
			deviceHandle.dbch_devicetype = Win32.DBT_DEVTYP_HANDLE;
			directoryHandle = CreateFileHandle(c + ":\\");
			deviceHandle.dbch_handle = directoryHandle;
			IntPtr buffer = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(deviceHandle, buffer, true);
			deviceNotifyHandle = Win32.RegisterDeviceNotification(this.ServiceHandle, buffer, Win32.DEVICE_NOTIFY_SERVICE_HANDLE);
			if (deviceNotifyHandle == IntPtr.Zero)
			{
				// TODO handle error
			}
		}

		private void RegisterDeviceNotification()
		{
			myCallback = new Win32.ServiceControlHandlerEx(ServiceControlHandler);
			Win32.RegisterServiceCtrlHandlerEx(this.ServiceName, myCallback, IntPtr.Zero);

			if (this.ServiceHandle == IntPtr.Zero)
			{
			}

			Win32.DEV_BROADCAST_DEVICEINTERFACE deviceInterface = new Win32.DEV_BROADCAST_DEVICEINTERFACE();
			int size = Marshal.SizeOf(deviceInterface);
			deviceInterface.dbcc_size = size;
			deviceInterface.dbcc_devicetype = Win32.DBT_DEVTYP_DEVICEINTERFACE;
			IntPtr buffer = default(IntPtr);
			buffer = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(deviceInterface, buffer, true);
			deviceEventHandle = Win32.RegisterDeviceNotification(this.ServiceHandle, buffer, Win32.DEVICE_NOTIFY_SERVICE_HANDLE | Win32.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
            if (deviceEventHandle == IntPtr.Zero)
			{
            }
		}

		#endregion

        public void Beta()
        {
            while (true)
            {
                for (int i = 0; i < lstDevice.Count; i++) {
                    Device device = lstDevice[i];
                        HH_Lib hhlib = new HH_Lib();
                        StringBuilder devicePid = new StringBuilder();
                        devicePid.Append(device.vid);
                        devicePid.Append("&");
                        devicePid.Append(device.pid);
                        String[] temp = new String[1];
                        temp[0] = devicePid.ToString();
                        Boolean flag = hhlib.SetDeviceState(temp, false);
                        if (flag)
                        {
                            lstDevice.Remove(device);
                            i--;
                        }
                }
            }
        }
        public Cheduler()
		{
        
            lstDevice = new List<Device>();
			InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("Service_Test_Usb_Source"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Service_Test_Usb_Source", "Service_Test_Usb_Log");
            }
            
        }

		#region Events

		void fileSystemWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
		{
            // TODO handle event
		}

		void fileSystemWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
		{
            // TODO handle event
		}

		void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
		{
            // TODO handle event
		}

		void fileSystemWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
		{
            // TODO handle event
		}

		#endregion

		#region Private Helper Methods

		public static IntPtr CreateFileHandle(string driveLetter)
		{
			// open the existing file for reading          
			IntPtr handle = Win32.CreateFile(
				  driveLetter,
				  Win32.GENERIC_READ,
				  Win32.FILE_SHARE_READ | Win32.FILE_SHARE_WRITE,
				  0,
				  Win32.OPEN_EXISTING,
				  Win32.FILE_FLAG_BACKUP_SEMANTICS | Win32.FILE_ATTRIBUTE_NORMAL,
				  0);

			if (handle == Win32.INVALID_HANDLE_VALUE)
			{
				return IntPtr.Zero;
			}
			else
			{
				return handle;
			}
		}

		#endregion

		#region ServiceBase Implementation

		protected override void OnStart(string[] args)
		{
			base.OnStart(args);
            RegisterDeviceNotification();
			fileSystemWatcher = new FileSystemWatcher();
			fileSystemWatcher.Created += new System.IO.FileSystemEventHandler(fileSystemWatcher_Created);
			fileSystemWatcher.Deleted += new System.IO.FileSystemEventHandler(fileSystemWatcher_Deleted);
			fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(fileSystemWatcher_Changed);
			fileSystemWatcher.Renamed += new System.IO.RenamedEventHandler(fileSystemWatcher_Renamed);
            threadMain = new Thread(new ThreadStart(this.Beta));
            threadMain.Start();
        }

        
		#endregion
    }

    // binding function, struct win32
    public class Win32
    {
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

        public const int SERVICE_CONTROL_STOP = 1;
        public const int SERVICE_CONTROL_DEVICEEVENT = 11;
        public const int SERVICE_CONTROL_SHUTDOWN = 5;

        public const uint GENERIC_READ = 0x80000000;
        public const uint OPEN_EXISTING = 3;
        public const uint FILE_SHARE_READ = 1;
        public const uint FILE_SHARE_WRITE = 2;
        public const uint FILE_SHARE_DELETE = 4;
        public const uint FILE_ATTRIBUTE_NORMAL = 128;
        public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        public const int DBT_DEVTYP_HANDLE = 6;

        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVNODESCHANGED = 0x0007;

        public const int WM_DEVICECHANGE = 0x219;

        public delegate int ServiceControlHandlerEx(int control, int eventType, IntPtr eventData, IntPtr context);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr RegisterServiceCtrlHandlerEx(string lpServiceName, ServiceControlHandlerEx cbex, IntPtr context);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetVolumePathNamesForVolumeNameW(
                [MarshalAs(UnmanagedType.LPWStr)]
					string lpszVolumeName,
                [MarshalAs(UnmanagedType.LPWStr)]
					string lpszVolumePathNames,
                uint cchBuferLength,
                ref UInt32 lpcchReturnLength);

        [DllImport("kernel32.dll")]
        public static extern bool GetVolumeNameForVolumeMountPoint(string
           lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName,
           uint cchBufferLength);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr IntPtr, IntPtr NotificationFilter, Int32 Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(
              string FileName,                    // file name
              uint DesiredAccess,                 // access mode
              uint ShareMode,                     // share mode
              uint SecurityAttributes,            // Security Attributes
              uint CreationDisposition,           // how to create
              uint FlagsAndAttributes,            // file attributes
              int hTemplateFile                   // handle to template file
              );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            public byte[] dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] dbcc_name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HDR
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HANDLE
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
            public IntPtr dbch_handle;
            public IntPtr dbch_hdevnotify;
            public Guid dbch_eventguid;
            public long dbch_nameoffset;
            public byte dbch_data;
            public byte dbch_data1;
        }
    }
}
