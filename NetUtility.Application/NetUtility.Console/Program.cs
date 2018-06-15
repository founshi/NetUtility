using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetUtility.WinForm;
using NetUtility.DeviceUnility;
using NetUtility.DirectoryUnility;
using NetUtility.WebUnility;
using System.Text.RegularExpressions;
namespace NetUtility.Con
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                //ASRSVersionCheck _ASRSVersionCheck = new ASRSVersionCheck(@"http://172.20.81.241:8055/publish.htm");
                //Console.WriteLine(_ASRSVersionCheck.GetCurrentVersion());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.WriteLine("Press any key to contiun....");
            Console.ReadKey();
        }
    }


    
    /// <summary>
    /// 注册表查找安装程序
    /// </summary>
    public class GetUnistall
    {
        public static void ShowAll()
        {
            RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall", false);
            if (key != null)
            {
                foreach (string keyName in key.GetSubKeyNames())
                {
                    RegistryKey key2 = key.OpenSubKey(keyName, false);
                    if (key2 != null)
                    {
                        string softwareName = key2.GetValue("DisplayName", "").ToString();
                        string installLocation = key2.GetValue("InstallLocation", "").ToString();
                        if (!string.IsNullOrEmpty(installLocation))
                        {
                            Console.WriteLine(string.Format("软件名：{0} -- 安装路径：{1}\r\n", softwareName, installLocation));
                        }
                        else
                        {
                            string installSorce = key2.GetValue("InstallSource", "").ToString();
                            if (!string.IsNullOrEmpty(installSorce))
                            {
                                Console.WriteLine(string.Format("软件名：{0} -- 安装路径：{1}\r\n", softwareName, installSorce));
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(keyName.Trim() + "没有找到安装路径");
                    }
                }
            }

        }

    }


}
