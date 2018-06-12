using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetUtility.WinForm;
using Microsoft.Win32;

namespace NetUtility.Con
{
    public static class RegistryHelperInstance
    {
        /// <summary>
        /// 获取根目标的子项
        /// </summary>
        public static void GetSubKey()
        {
            RegistryKey Rjc = RegistryHelper.GetRootRegisterKey(WRegisterRootKeyType.HKEY_LOCAL_MACHINE);
            Console.WriteLine(Rjc.SubKeyCount);
            //枚举所有的项
            foreach (string str in Rjc.GetSubKeyNames())
            {
                Console.WriteLine(str);
            }
        }
        /// <summary>
        /// 创建注册表 但是程序必须要使用管理员的身份运行
        /// </summary>
        public static void CreateKeyAndSetVal()
        {

            //指定要操作的注册表路径  HKEY_LOCAL_MACHINE\SOFTWARE
            RegistryKey fatherrjc = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software",true);

            //创建一个注册表子项 - 在 HKEY_LOCAL_MACHINE\SOFTWARE 下创建 - 创建完记得刷新，否则看不到
            RegistryHelper.CreateRegistryKey(fatherrjc, "三肿专用注册表");

            //创建一个注册表子项，并写入一个键值 - 在 HKEY_LOCAL_MACHINE\SOFTWARE 下创建 - 创建完记得刷新，否则看不到
            RegistryHelper.CreateRegistryKey(fatherrjc, "三肿专用注册表2", "测试键值", "我是内容");

        }

        public static void DeleteKeyAndVal()
        {
            //删除注册表的一个键值
            //指定要操作的注册表路径  HKEY_LOCAL_MACHINE\SOFTWARE\三肿专用注册表2
            RegistryKey deleterjc = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"Software\三肿专用注册表2", true);
            if (deleterjc != null)
            {
                RegistryHelper.DeleteRegistryValue(deleterjc, "测试键值");

                //删除注册表的一个子项
                RegistryHelper.DeleteRegistryKey(WRegisterRootKeyType.HKEY_LOCAL_MACHINE, @"Software", "三肿专用注册表2");

            }
          bool val=  RegistryHelper.DeleteRegistryKey(WRegisterRootKeyType.HKEY_LOCAL_MACHINE, @"Software", "三肿专用注册表");
          Console.WriteLine(val.ToString());
        }

        public static void GetKeyAndVal()
        {
            //获取注册表项指定值
            string KeyValue = RegistryHelper.GetRegistryValue(WRegisterRootKeyType.HKEY_LOCAL_MACHINE, @"Software\三肿专用注册表2", "测试键值");
            Console.WriteLine(KeyValue);
        }


    }
}
