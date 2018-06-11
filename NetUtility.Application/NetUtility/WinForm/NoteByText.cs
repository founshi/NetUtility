/*****************重新消息处理函数:(将关闭按钮不起作用)******************1
 * 重新消息处理函数:(将关闭按钮不起作用)
 * protected override void WndProc(ref System.Windows.Forms.Message m)
{
    const int WM_SYSCOMMAND = 0x0112;
    const int SC_CLOSE = 0xF060;
    if ((m.Msg == WM_SYSCOMMAND) && ((int)m.WParam == SC_CLOSE))
    {
		return;
    }
    base.WndProc(ref m);
}
*/
/***************关闭窗口前询问************************2
 关闭窗口前询问
if (MessageBox.Show("您确定要关闭窗口吗?", "提示?", MessageBoxButtons.YesNo) == DialogResult.Yes)
{ 
	e.Cancel=false;
}
else
{
	e.Cancel = true;
} 
 */

/****************控制程序重复开启***************3
 private static bool IsRuning()
{     
    string MName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
    string pName = System.IO.Path.GetFileNameWithoutExtension(MName);
    System.Diagnostics.Process[] MyProce =System.Diagnostics.Process.GetProcessesByName(pName);
    if (MyProce.Length > 1)
    {
		return true;
    }
    else
    {
		return false;
    }    
}
  static void Main()
{
    if (IsRuning())
    {
		MessageBox.Show("请勿重复开启程序", "温馨提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
    }
    else
    {
		Application.EnableVisualStyles();//启动应用程序的可视样式
		Application.SetCompatibleTextRenderingDefault(false);//新控件使用GDI+
		Application.Run(new Form1());
    }
} 
 */

/*****************实现文件的拖拽****************4
 1.将某个控件的AllowDrop属性改为true
2.添加某个控件DragEnter事件,并写代码
if (e.Data.GetDataPresent(DataFormats.FileDrop))
{
    e.Effect = DragDropEffects.Copy;
}
else
{
    e.Effect = DragDropEffects.None;
}		
3.添加某个控件DragDrop事件,并写代码
string[] dragFilePath = (string[])e.Data.GetData(DataFormats.FileDrop, false);//获取文件路径
MessageBox.Show(dragFilePath[0].ToString());//文件路径	
*/
/*****************枚举所以文件文件夹到TreeView中****************5
 private void EnumAllFiles()
{
		TreeNode RootNode = new TreeNode();
		RootNode.Text = "我的电脑";
		treeView1.Nodes.Add(RootNode);
		//treeView1.ExpandAll();
		DriveInfo[] Drf = DriveInfo.GetDrives();//获取所有的驱动
		foreach (DriveInfo itemA in Drf)
		{
			Application.DoEvents();
			if (itemA.IsReady)
			{
				TreeNode SonNode = new TreeNode();
				SonNode.Text = itemA.Name;
				RootNode.Nodes.Add(SonNode);

				DirectoryInfo dr = new DirectoryInfo(itemA.Name);
				FileSystemInfo[] fsi = dr.GetFileSystemInfos();
				foreach (FileSystemInfo item in fsi)
				{
					Application.DoEvents();
					if (item is DirectoryInfo)//文件夹
					{
						DirectoryInfo md = new DirectoryInfo(item.FullName);
						TreeNode Nodemx = new TreeNode();
						Nodemx.Text = md.Name;
						SonNode.Nodes.Add(Nodemx);
						GetFile(item.FullName, Nodemx);
					}
					else//文件
					{
						FileInfo fi = new FileInfo(item.FullName);
						SonNode.Nodes.Add(item.FullName, fi.Name);
					}
				}
			}
		}
}	
private void GetFile(string mmPath, TreeNode mNodex)
{    
    try
    {
		DirectoryInfo dr = new DirectoryInfo(mmPath);
		FileSystemInfo[] fsi = dr.GetFileSystemInfos();

		foreach (FileSystemInfo item in fsi)
		{
			Application.DoEvents();
			if (item is DirectoryInfo)//文件夹
			{
				DirectoryInfo md = new DirectoryInfo(item.FullName);
				TreeNode Nodemx = new TreeNode();
				Nodemx.Text = md.Name;
				mNodex.Nodes.Add(Nodemx);
				GetFile(item.FullName, Nodemx);
			}
			else//文件
			{
				FileInfo fi = new FileInfo(item.FullName);
				mNodex.Nodes.Add(item.FullName, fi.Name);
			}
		}
	}
    catch
    {
    }
}
*/
/*****************将excel表导入到dataSet(使用微软Excel)****************6
public DataSet getXSLData(string filepath)
{
    string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
    System.Data.OleDb.OleDbConnection Conn = new System.Data.OleDb.OleDbConnection(strCon);
    string strCom = "SELECT * FROM [Sheet1$]";
    Conn.Open();
    System.Data.OleDb.OleDbDataAdapter myCommand = new System.Data.OleDb.OleDbDataAdapter(strCom, Conn);
    DataSet ds = new DataSet();
    myCommand.Fill(ds, "[Sheet1$]");
    Conn.Close();
    return ds;
}
*/
/*****************使用serialPort控件(类)读取串口信息****************7
 //1.打开串口
this.serialPort1.PortName = "COM1";
this.serialPort1.BaudRate = 1200;
this.serialPort1.ReceivedBytesThreshold = 1;
this.serialPort1.Open();
//2.读取串口信息
byte[] ReadBuffer = new byte[this.serialPort1.ReadBufferSize + 1];
int count = this.serialPort1.Read(ReadBuffer, 0, this.serialPort1.ReadBufferSize);
mm += Encoding.ASCII.GetString(ReadBuffer, 0, count);
if (mm.Length >= 10 && !IsDoing)
{
    IsDoing = true;
    //向TextBox中添加值
    this.textBox1.Invoke(new MethodInvoker
							(delegate{this.textBox1.AppendText(mm);})
        );
    ReadNum += 1;
    this.listBox1.Invoke(new MethodInvoker(
			delegate{this.listBox1.Items.Add(ReadNum.ToString()+"-->" +mm);})
        );
    mm = null;
}
//3.关闭
this.serialPort1.Close();
*/
/*****************在线程中往窗体上添加文字,向TextBox中添加值****************8
 例子A:this.textBox1.Invoke(new MethodInvoker
	(delegate{this.textBox1.AppendText(mm);})
);
例子B:this.listBox1.Invoke(new MethodInvoker
    (delegate{this.listBox1.Items.Add(ReadNum.ToString()+"-->" +mm);})
);
*/
/*****************修改DataSet/DataTable某列的名称****************9
Dt.Tables[0].Columns[0].ColumnName = "条码";
Dt.Tables[0].Columns[1].ColumnName = "成品代号";
*/
/*****************SMTP发送邮件****************10
MailAddress frm = new MailAddress("123707@CSTC.COM.CN");
MailAddress to = new MailAddress("qudy@CSTC.COM.CN");
MailMessage mesg = new MailMessage("123707@CSTC.COM.CN", "123707@CSTC.COM.CN,125295@CSTC.COM.CN");
mesg.Subject = "Test";//主题
mesg.Body = "Hello,MyFirst Mail Test!!!";//信息
SmtpClient clt = new SmtpClient("10.30.1.18", 25);//服务器
clt.Credentials = new System.Net.NetworkCredential("123707", "123456");//登陆名和密码
clt.Send(mesg);
 
*/
/*****************XXXX****************11
*/
/*****************XXXX****************12
*/
/*****************XXXX****************13
*/