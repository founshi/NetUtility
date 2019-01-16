using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetUtility.FileUnility
{
    enum LogStatus
    {
        sucess = 0,
        warn = 1,
        error = 2,

    }

    class LogPlus
    {

        public static void SetLog(string msg, Exception ex = null, LogStatus logstatus = LogStatus.sucess)
        {
            switch (logstatus)
            {
                case LogStatus.sucess:
                    if (!string.IsNullOrEmpty(msg)) Log.SetLog(string.Format("[{0}],执行状态:{1},消息:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "SUCESS", msg));
                    if (ex != null)
                    {
                        string exmsg = string.Format("[{0}],执行状态:{1},错误消息:{2}\r\n报错所在文件:{3}\r\n报错具体行数:{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "SUCESS", ex.Message, ex.Source == null ? "" : ex.Source, ex.StackTrace == null ? "" : ex.StackTrace);
                        Log.SetLog(exmsg);
                    }
                    break;
                case LogStatus.warn:
                    if (!string.IsNullOrEmpty(msg)) Log.SetLog(string.Format("[{0}],执行状态:{1},消息:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "WARN", msg));
                    if (ex != null)
                    {
                        string exmsg = string.Format("[{0}],执行状态:{1},错误消息:{2}\r\n报错所在文件:{3}\r\n报错具体行数:{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "WARN", ex.Message, ex.Source == null ? "" : ex.Source, ex.StackTrace == null ? "" : ex.StackTrace);
                        Log.SetLog(exmsg);
                    }
                    break;
                case LogStatus.error:
                    if (!string.IsNullOrEmpty(msg)) Log.SetLog(string.Format("[{0}],执行状态:{1},消息:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "ERROR", msg));
                    if (ex != null)
                    {
                        string exmsg = string.Format("[{0}],执行状态:{1},错误消息:{2}\r\n报错所在文件:{3}\r\n报错具体行数:{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "ERROR", ex.Message, ex.Source == null ? "" : ex.Source, ex.StackTrace == null ? "" : ex.StackTrace);
                        Log.SetLog(exmsg);
                    }
                    break;
                default:
                    if (!string.IsNullOrEmpty(msg)) Log.SetLog(string.Format("[{0}],执行状态:{1},消息:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "SUCESS", msg));
                    if (ex != null)
                    {
                        string exmsg = string.Format("[{0}],执行状态:{1},错误消息:{2}\r\n报错所在文件:{3}\r\n报错具体行数:{4}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "SUCESS", ex.Message, ex.Source == null ? "" : ex.Source, ex.StackTrace == null ? "" : ex.StackTrace);
                        Log.SetLog(exmsg);
                    }
                    break;
            }




        }

        public static void SetLogToJson(string json)
        {
            string JsonPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            JsonPath = Path.Combine(JsonPath, "Json");
            Log.CreateDirectory(JsonPath);
            JsonPath = Path.Combine(JsonPath, string.Format("{0}_json.{1}", DateTime.Now.ToString("yyyyMMdd"), "txt"));

            Log.SetLog(json, JsonPath);
        }

        public static void SetLogNoQrCode(string testContext)
        {
            string QrCodePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            QrCodePath = Path.Combine(QrCodePath, "QRCode");
            Log.CreateDirectory(QrCodePath);
            QrCodePath = Path.Combine(QrCodePath, string.Format("{0}_QRCode.{1}", DateTime.Now.ToString("yyyyMMdd"), "txt"));
            Log.SetLog(testContext, QrCodePath);
        }

        public static void SetLogResultFail(string testContext)
        {
            string ResultFailPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            ResultFailPath = Path.Combine(ResultFailPath, "ResultFail");
            Log.CreateDirectory(ResultFailPath);
            ResultFailPath = Path.Combine(ResultFailPath, string.Format("{0}_ResultFail.{1}", DateTime.Now.ToString("yyyyMMdd"), "txt"));
            Log.SetLog(testContext, ResultFailPath);

        }



    }
}

