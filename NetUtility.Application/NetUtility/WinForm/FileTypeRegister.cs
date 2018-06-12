using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetUtility.WinForm
{

    /// <summary>
    ///在WIN10上面测试失败。
    ///在WIN10上面测试失败
    ///在WIN10上面测试失败
    ///在WIN10上面测试失败
    ///在WIN10上面测试失败
    ///在WIN10上面测试失败
    /// </summary>

    public class FileTypeRegInfo
    {
        /// <summary>
        /// 目标类型文件的扩展名
        /// </summary>
        public string ExtendName { get; set; }//".xcf"
        /// <summary>
        /// 目标文件类型说明
        /// </summary>
        public string Description { get; set; }//"XCodeFactory项目文件"

        /// <summary>
        /// 目标类型文件关联的图标
        /// </summary>
        public string IcoPath { get; set; }
        /// <summary>
        /// 打开目标类型文件的应用程序
        /// </summary>
        public string ExePath { get; set; }

        public FileTypeRegInfo()
        {
        }

        public FileTypeRegInfo(string extendName)
        {
            this.ExtendName = extendName;
        }
        public FileTypeRegInfo(string extendName, string description, string icoPath, string exePath)
        {
            this.ExtendName = extendName;
            this.Description = description;
            this.ExePath = exePath;
            this.IcoPath = icoPath;
        }


    }


    public class FileTypeRegister
    {
        public static void RegisterFileType(FileTypeRegInfo regInfo)
        {
            //if (FileTypeRegister.FileTypeRegistered(regInfo.ExtendName))
            //{
            //    return;
            //}

            string relationName = regInfo.ExtendName.Substring(1, regInfo.ExtendName.Length - 1).ToUpper() + "_FileType";

            RegistryKey fileTypeKey = Registry.ClassesRoot.CreateSubKey(regInfo.ExtendName);
            fileTypeKey.SetValue("", relationName);
            fileTypeKey.Close();

            RegistryKey relationKey = Registry.ClassesRoot.CreateSubKey(relationName);
            relationKey.SetValue("", regInfo.Description);

            RegistryKey iconKey = relationKey.CreateSubKey("DefaultIcon");
            iconKey.SetValue("", regInfo.IcoPath);

            RegistryKey shellKey = relationKey.CreateSubKey("Shell");
            RegistryKey openKey = shellKey.CreateSubKey("Open");
            RegistryKey commandKey = openKey.CreateSubKey("Command");
            commandKey.SetValue("", regInfo.ExePath + " %1");

            relationKey.Close();
        }

        public static bool FileTypeRegistered(string extendName)
        {
            RegistryKey softwareKey = Registry.ClassesRoot.OpenSubKey(extendName);
            if (softwareKey != null)
            {
                return true;
            }

            return false;
        }
    }

    /*
      
      要注意的是commandKey.SetValue("" ,regInfo.ExePath + " %1") ;其中" %1"表示将被双击的文件的路径传给目标应用程序，这样在双击a.xcf文件时，XCodeFactory才知道要打开哪个文件，所以应用程序的Main方法要被改写为带有参数的形式，就像下面的样子：

        [STAThread]
        static void Main(string[] args)
        {            
            if((args!= null) && (args.Length > 0))
            {                
                string filePath = "" ;
                for(int i=0 ;i<args.Length ;i++)
                {
                    filePath += " " + args[i] ;
                }                

                MainForm.XcfFilePath = filePath.Trim() ;
            }
            
            Application.Run(new MainForm());
        }            
       关于自定义文件类型的注册，本文实现的是最基本的功能，如果需要更多的高级功能，也可以类推实现之。
      */

}
