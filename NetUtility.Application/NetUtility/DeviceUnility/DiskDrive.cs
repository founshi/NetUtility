using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace NetUtility.DeviceUnility
{
    public class DiskDrive
    {
        /// <summary>
        /// 获取硬盘ID
        /// </summary>
        /// <returns></returns>
        public static string GetDiskID()
        {
            string HDid = "";
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo.Properties["Model"].Value == null) throw new Exception("为找到硬盘ID");
                HDid = mo.Properties["Model"].Value.ToString();
            }
            return HDid;
        }

    }
}
#region 硬盘相关信息参数
//Availability  --设备的状态。
//BytesPerSector  --在每个扇区的物理磁盘驱动器的字节数。
//Capabilities  --媒体访问设备的能力阵列。
//CapabilityDescriptions  --更详细的解释为任何在功能阵列表示的访问设备的功能的列表
//Caption  --对象的序列号
//CompressionMethod  --设备所使用的算法或工具，以支持压缩。
//ConfigManagerErrorCode  --Windows配置管理器错误代码。
//ConfigManagerUserConfig  --如果为True，该设备使用用户定义的配置。
//CreationClassName  --代表所在的类
//DefaultBlockSize  --此设备默认块大小，以字节为单位。
//Description  --描述
//DeviceID  --磁盘驱动器与系统中的其他设备的唯一标识符
//ErrorCleared  --如果为True，报告LastErrorCode错误现已清除。
//ErrorDescription  --关于可能采取的纠正措施记录在LastErrorCode错误，和信息的详细信息。
//ErrorMethodology  --误差检测和校正的类型被此设备支持。
//FirmwareRevision  --修订制造商分配的磁盘驱动器固件。
//Index  --给定的驱动器的物理驱动器号。此属性由GetDriveMapInfo方法填补。 0xFF的值表示给定的驱动器不映射到物理驱动器。
//InstallDate  --日期和时间对象安装。此属性不需要的值以表示已安装的对象。
//InterfaceType  --物理磁盘驱动器的类型 （IDE、sata）
//LastErrorCode  --报告的逻辑设备上一个错误代码。
//Manufacturer  --制造商名称
//MaxBlockSize  --最大块的大小，以字节为单位，通过该设备访问的媒体。
//MaxMediaSize  --最大介质尺寸的介质，以KB为单位，由该设备支持。
//MediaLoaded  --如果真，媒体为一磁盘驱动器加载，这意味着该设备具有一个可读的文件系统和可访问。对于固定磁盘驱动器，该属性将始终为TRUE。
//MediaType  --由该设备使用或访问的媒体类型。
//MinBlockSize  --最小的块大小，以字节为单位，通过该设备访问的媒体。
//Model  --磁盘驱动器的制造商的型号。
//Name  --名字
//NeedsCleaning  --如果真，媒体接入设备需要清洁。不论手动或自动清洗是可能显示在Capabilities属性。
//NumberOfMediaSupported  --可被支持的或插入的介质最大数量
//Partitions  --此物理磁盘驱动器上的分区是由操作系统识别的数目。
//PNPDeviceID  --即插即用逻辑设备的播放设备标识符。
//PowerManagementCapabilities  --逻辑设备的特定功率相关的能力阵列。
//PowerManagementSupported  --如果为True，该设备可以是电源管理
//SCSIBus  --盘驱动器的SCSI总线号。
//SCSILogicalUnit  --SCSI逻辑单元的磁盘驱动器的号码（LUN）。
//SCSIPort  --盘驱动器的SCSI端口号。
//SCSITargetId  --SCSI标识符号码的磁盘驱动器的。
//SectorsPerTrack  --在每个轨道此物理磁盘驱动器扇区数。
//SerialNumber  --由制造商分配的号来识别物理介质。
//Signature  --磁盘识别。该属性可以被用于识别一个共享资源。
//Size  --磁盘大小
//Status  --对象的当前状态。
//StatusInfo  --逻辑设备的状态
//SystemCreationClassName  --该作用域计算机的CreationClassName属性的值。
//SystemName  --系统名称
//TotalCylinders  --物理磁盘驱动器上柱面总数。该值可能不准确
//TotalHeads  --磁盘驱动器上磁头总数。该值可能不准确。
//TotalSectors  --物理磁盘驱动器上的扇区总数。该值可能不准确。
//TotalTracks  --物理磁盘驱动器上的曲目总数。该值可能不准确。
//TracksPerCylinder  --在物理磁盘驱动器上的每个柱面轨迹的数量。该值可能不准确。
#endregion