using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ESCPOS_NET
{
    public class DeviceFinder
    {
        #region Win API
        private struct SP_DEVICE_INTERFACE_DATA
        {
            internal int cbSize;

            internal Guid InterfaceClassGuid;

            internal int Flags;

            internal IntPtr Reserved;
        }
        private struct SP_DEVINFO_DATA
        {
            internal int cbSize;

            internal Guid ClassGuid;

            internal int DevInst;

            internal IntPtr Reserved;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct DEVPROPKEY
        {
            public Guid fmtid;
            public uint pid;
        }
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, int MemberIndex, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, IntPtr DeviceInfoData);
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, int DeviceInterfaceDetailDataSize, ref int RequiredSize, ref SP_DEVINFO_DATA DeviceInfoData);
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetupDiGetDeviceProperty(IntPtr deviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData, ref DEVPROPKEY propertyKey, out uint propertyType, IntPtr propertyBuffer, uint propertyBufferSize, out uint requiredSize, uint flags);
        [DllImport("setupapi.dll", SetLastError = true)]
        private static extern int SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);
        private static void DEFINE_DEVPROPKEY(out DEVPROPKEY key, uint l, ushort w1, ushort w2, byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, byte b7, byte b8, uint pid)
        {
            key.fmtid = new Guid(l, w1, w2, b1, b2, b3, b4, b5, b6, b7, b8);
            key.pid = pid;
        }
        #endregion
        #region Device Methods
        public static List<DeviceDetails> GetDevices()
        {
            //https://learn.microsoft.com/en-us/windows-hardware/drivers/install/guid-devinterface-usb-device
            //USB devices: a5dcbf10-6530-11d2-901f-00c04fb951ed
            //Bluetooth devices: 00f40965-e89d-4487-9890-87c3abb211f4
            var devices = GetDevicesbyClassID("a5dcbf10-6530-11d2-901f-00c04fb951ed");
            return devices;
        }
        private static List<DeviceDetails> GetDevicesbyClassID(string classguid)
        {
            IntPtr intPtr = IntPtr.Zero;
            var devices = new List<DeviceDetails>();
            try
            {
                Guid guid = new(classguid);
                intPtr = SetupDiGetClassDevs(ref guid, IntPtr.Zero, IntPtr.Zero, 18);
                if (intPtr == INVALID_HANDLE_VALUE)
                {
                    Win32Exception("Failed to enumerate devices.");
                }

                int num = 0;
                while (true)
                {
                    SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = default;
                    DeviceInterfaceData.cbSize = Marshal.SizeOf((object)DeviceInterfaceData);
                    if (!SetupDiEnumDeviceInterfaces(intPtr, IntPtr.Zero, ref guid, num, ref DeviceInterfaceData))
                    {
                        break;
                    }

                    int RequiredSize = 0;
                    if (!SetupDiGetDeviceInterfaceDetail(intPtr, ref DeviceInterfaceData, IntPtr.Zero, 0, ref RequiredSize, IntPtr.Zero) && Marshal.GetLastWin32Error() != 122)
                    {
                        Win32Exception("Failed to get interface details buffer size.");
                    }

                    IntPtr intPtr2 = IntPtr.Zero;
                    try
                    {
                        intPtr2 = Marshal.AllocHGlobal(RequiredSize);
                        Marshal.WriteInt32(intPtr2, (IntPtr.Size == 4) ? (4 + Marshal.SystemDefaultCharSize) : 8);
                        SP_DEVINFO_DATA DeviceInfoData = default;
                        DeviceInfoData.cbSize = Marshal.SizeOf((object)DeviceInfoData);
                        if (!SetupDiGetDeviceInterfaceDetail(intPtr, ref DeviceInterfaceData, intPtr2, RequiredSize, ref RequiredSize, ref DeviceInfoData))
                        {
                            Win32Exception("Failed to get device interface details.");
                        }
                        string path = Marshal.PtrToStringUni(new IntPtr(intPtr2.ToInt64() + 4));

                        DeviceDetails deviceDetails = GetDeviceDetails(path, intPtr, DeviceInfoData);
                        devices.Add(deviceDetails);
                    }
                    finally
                    {
                        if (intPtr2 != IntPtr.Zero)
                        {
                            Marshal.FreeHGlobal(intPtr2);
                            intPtr2 = IntPtr.Zero;
                        }
                    }
                    num++;
                }
                if (Marshal.GetLastWin32Error() != 259)
                {
                    Win32Exception("Failed to get device interface.");
                }
            }
            finally
            {
                if (intPtr != IntPtr.Zero && intPtr != INVALID_HANDLE_VALUE)
                {
                    SetupDiDestroyDeviceInfoList(intPtr);
                }
            }
            return devices;
        }
        private static DeviceDetails GetDeviceDetails(string devicePath, IntPtr deviceInfoSet, SP_DEVINFO_DATA deviceInfoData)
        {
            DeviceDetails result = new DeviceDetails
            {
                DevicePath = devicePath
            };
            if (!string.IsNullOrWhiteSpace(devicePath) && devicePath.Contains("#"))
            {
                var spserial = devicePath.Split('#');
                result.SerialNum = spserial[spserial.Length - 2];
                //last in array is guid and last second is the serial number 
                //serial number might not be the actual serial number for the device it may be system generated
            }
            DEFINE_DEVPROPKEY(out DEVPROPKEY key, 0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2, 4);
            result.BusName = GetPropertyForDevice(deviceInfoSet, deviceInfoData, key)[0];
            DEFINE_DEVPROPKEY(out key, 0xb725f130, 0x47ef, 0x101a, 0xa5, 0xf1, 0x02, 0x60, 0x8c, 0x9e, 0xeb, 0xac, 10);
            result.DisplayName = GetPropertyForDevice(deviceInfoSet, deviceInfoData, key)[0];
            DEFINE_DEVPROPKEY(out key, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 2);
            result.DeviceDescription = GetPropertyForDevice(deviceInfoSet, deviceInfoData, key)[0];
            DEFINE_DEVPROPKEY(out key, 0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0, 13);
            result.Manufacturer = GetPropertyForDevice(deviceInfoSet, deviceInfoData, key)[0];
            DEFINE_DEVPROPKEY(out key, 0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57, 256);
            string[] multiStringProperty = GetPropertyForDevice(deviceInfoSet, deviceInfoData, key);
            Regex regex = new Regex("VID_([0-9A-F]{4})&PID_([0-9A-F]{4})", RegexOptions.IgnoreCase);
            bool flag = false;
            string[] array = multiStringProperty;
            foreach (string input in array)
            {
                Match match = regex.Match(input);
                if (match.Success)
                {
                    result.VID = ushort.Parse(match.Groups[1].Value, NumberStyles.AllowHexSpecifier);
                    result.PID = ushort.Parse(match.Groups[2].Value, NumberStyles.AllowHexSpecifier);
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                Win32Exception("Failed to find VID and PID for USB device. No hardware ID could be parsed.");
            }

            return result;
        }
        private static string[] GetPropertyForDevice(IntPtr deviceInfoSet, SP_DEVINFO_DATA deviceInfoData, DEVPROPKEY key)
        {
            IntPtr buffer = IntPtr.Zero;
            try
            {
                uint buflen = 512;
                buffer = Marshal.AllocHGlobal((int)buflen);
                if (!SetupDiGetDeviceProperty(deviceInfoSet, ref deviceInfoData, ref key, out uint proptype, buffer, buflen, out uint outsize, 0))
                {
                    Win32Exception("Failed to get property for device");
                }
                byte[] lbuffer = new byte[outsize];
                Marshal.Copy(buffer, lbuffer, 0, (int)outsize);
                var val = Encoding.Unicode.GetString(lbuffer);
                var aval = val.Split('\0');
                return aval;
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(buffer);
            }
        }
        private static void Win32Exception(string message)
        {
            throw new Exception(message, new Win32Exception(Marshal.GetLastWin32Error()));
        }
        #endregion
    }
}
