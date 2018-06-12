using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace NetUtility.DeviceUnility
{
    public class CPUDevice
    {
        /// <summary>
        /// 获取CPU的ID
        /// </summary>
        /// <returns></returns>
        public static string GetCPUId()
        {
            string strCpuID = "";
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return strCpuID;

        }



    }
}
#region CPU参数说明
//AddressWidth  --在32位操作系统，该值是32，在64位操作系统是64。
// Architecture  --所使用的平台的处理器架构。
// AssetTag  --代表该处理器的资产标签。
// Availability  --设备的状态。
// Caption  --设备的简短描述
// Characteristics  --处理器支持定义的功能
// ConfigManagerErrorCode  --Windows API的配置管理器错误代码
// ConfigManagerUserConfig  --如果为TRUE，该装置是使用用户定义的配置
// CpuStatus  --处理器的当前状态
// CreationClassName  --出现在用来创建一个实例继承链的第一个具体类的名称
// CurrentClockSpeed  --处理器的当前速度，以MHz为单位
// CurrentVoltage  --处理器的电压。如果第八位被设置，位0-6包含电压乘以10，如果第八位没有置位，则位在VoltageCaps设定表示的电压值。 CurrentVoltage时SMBIOS指定的电压值只设置
// DataWidth  --在32位处理器，该值是32，在64位处理器是64
// Description  --描述
// DeviceID  --在系统上的处理器的唯一标识符
// ErrorCleared  --如果为真，报上一个错误代码的被清除
// ErrorDescription  --错误的代码描述
// ExtClock  --外部时钟频率，以MHz为单位
// Family  --处理器系列类型
// InstallDate  --安装日期
// L2CacheSize  --二级缓存大小
// L2CacheSpeed  --二级缓存处理器的时钟速度
// L3CacheSize  --三级缓存的大小
// L3CacheSpeed  --三级缓存处理器的时钟速度
// LastErrorCode  --报告的逻辑设备上一个错误代码
// Level  --处理器类型的定义。该值取决于处理器的体系结构
// LoadPercentage  --每个处理器的负载能力，平均到最后一秒
// Manufacturer   --处理器的制造商
// MaxClockSpeed  --处理器的最大速度，以MHz为单位
// Name  --处理器的名称
// NumberOfCores  --芯为处理器的当前实例的数目。核心是在集成电路上的物理处理器
// NumberOfEnabledCore  --每个处理器插槽启用的内核数
// NumberOfLogicalProcessors  --用于处理器的当前实例逻辑处理器的数量
// OtherFamilyDescription  --处理器系列类型
// PartNumber  --这款处理器的产品编号制造商所设置
// PNPDeviceID  --即插即用逻辑设备的播放设备标识符
// PowerManagementCapabilities  --逻辑设备的特定功率相关的能力阵列
// PowerManagementSupported  --如果为TRUE，该装置的功率可以被管理，这意味着它可以被放入挂起模式
// ProcessorId  --描述处理器功能的处理器的信息
// ProcessorType  --处理器的主要功能
// Revision  --系统修订级别取决于体系结构
// Role  --所述处理器的作用
// SecondLevelAddressTranslationExtensions  --如果为True，该处理器支持用于虚拟地址转换扩展
// SerialNumber --处理器的序列号
// SocketDesignation  --芯片插座的线路上使用的类型
// Status  --对象的当前状态
// StatusInfo  --对象的当前状态信息
// Stepping  --在处理器家族处理器的版本
// SystemCreationClassName  --创建类名属性的作用域计算机的价值
// SystemName  --系统的名称
// ThreadCount  --每个处理器插槽的线程数
// UniqueId  --全局唯一标识符的处理器
// UpgradeMethod  --CPU插槽的信息
// Version  --依赖于架构处理器的版本号
// VirtualizationFirmwareEnabled  --如果真，固件可以虚拟化扩展
// VMMonitorModeExtensions  --如果为True，该处理器支持Intel或AMD虚拟机监控器扩展。
// VoltageCaps  --该处理器的电压的能力 
#endregion