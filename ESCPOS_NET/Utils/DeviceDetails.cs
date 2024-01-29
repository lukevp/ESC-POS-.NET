
namespace ESCPOS_NET
{
    public class DeviceDetails
    {
        public string DisplayName { get; set; }
        /// <summary>
        /// DEVPKEY_Device_BusReportedDeviceDesc <see href="https://github.com/tpn/winsdk-10/blob/master/Include/10.0.16299.0/shared/devpkey.h">see reference</see>
        /// </summary>
        public string BusName { get; set; }
        public string SerialNum { get; set; }
        public string DeviceDescription { get; set; }
        public string DevicePath { get; set; }
        public string Manufacturer { get; set; }
        public ushort VID { get; set; }
        public ushort PID { get; set; }
    }
}
