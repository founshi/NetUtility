using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace NetUtility.DeviceUnility
{
    public class PCDevice
    {
        /// <summary>
        /// 获取操作系统的登录用户名
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            return Environment.UserName;
        }
        /// <summary>
        /// 获取计算机名
        /// </summary>
        /// <returns></returns>
        public static string GetComputerName()
        {
            return System.Environment.MachineName;
        }
        /// <summary>
        /// 获取PC类型
        /// </summary>
        /// <returns></returns>
        public static string GetSystemType()
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {               
                st = mo["SystemType"].ToString();
            }
            return st;
        }
        /// <summary>
        /// 获取物理内存
        /// </summary>
        /// <returns></returns>
        public static string GetTotalPhysicalMemory()
        {
            string st = "";
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {

                st = mo["TotalPhysicalMemory"].ToString();

            }
            return st;
        }


    }
}
//AdminPasswordStatus
//AutomaticManagedPagefile
//AutomaticResetBootOption
//AutomaticResetCapability
//BootOptionOnLimit
//BootOptionOnWatchDog
//BootROMSupported
//BootStatus
//BootupState
//Caption
//ChassisBootupState
//ChassisSKUNumber
//CreationClassName
//CurrentTimeZone
//DaylightInEffect
//Description
//DNSHostName
//Domain
//DomainRole
//EnableDaylightSavingsTime
//FrontPanelResetStatus
//HypervisorPresent
//InfraredSupported
//InitialLoadInfo
//InstallDate
//KeyboardPasswordStatus
//LastLoadInfo
//Manufacturer
//Model
//Name
//NameFormat
//NetworkServerModeEnabled
//NumberOfLogicalProcessors
//NumberOfProcessors
//OEMLogoBitmap
//OEMStringArray
//PartOfDomain
//PauseAfterReset
//PCSystemType
//PCSystemTypeEx
//PowerManagementCapabilities
//PowerManagementSupported
//PowerOnPasswordStatus
//PowerState
//PowerSupplyState
//PrimaryOwnerContact
//PrimaryOwnerName
//ResetCapability
//ResetCount
//ResetLimit
//Roles
//Status
//SupportContactDescription
//SystemFamily
//SystemSKUNumber
//SystemStartupDelay
//SystemStartupOptions
//SystemStartupSetting
//SystemType  PC系统类型
//ThermalState
//TotalPhysicalMemory  物理总内存
//UserName    组中的用户名
//WakeUpType
//Workgroup  工作组
