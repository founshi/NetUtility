using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetUtility.App
{
    public partial class DataGridViewAddButton : Form
    {
        public DataGridViewAddButton()
        {
            InitializeComponent();
        }

        private void DataGridViewAddButton_Load(object sender, EventArgs e)
        {
            List<Student> _mlist = new List<Student>(){
            new Student(){ Gener="男", Id=1, Name="张三"},
            new Student(){ Gener="男", Id=2, Name="李四"},
            new Student(){ Gener="女", Id=3, Name="王五"},
            new Student(){ Gener="男", Id=4, Name="赵六"},
            new Student(){ Gener="女", Id=5, Name="曹操"},
            };

            this.dataGridView1.DataSource = _mlist;

            MulAutoBtnEdit();
        }


        private void MulAutoBtnEdit()
        {
            this.dataGridView1.Columns.Add("ColBtnEdit", "嵌入操作按钮");
            this.dataGridView1.Columns["ColBtnEdit"].Width = 150;
            int index = this.dataGridView1.Columns["ColBtnEdit"].Index;
            this.dataGridView1.Columns["ColBtnEdit"].Resizable = DataGridViewTriState.False;
            Button btnAdd = GetBtnByType("BtnAdd", "新增");
            Button btnEdit = GetBtnByType("BtnEdit", "修改");
            Button btnDel = GetBtnByType("BtnDel", "删除");
            this.dataGridView1.Controls.Add(btnAdd);
            this.dataGridView1.Controls.Add(btnEdit);
            this.dataGridView1.Controls.Add(btnDel);
            Rectangle rectangle = this.dataGridView1.GetCellDisplayRectangle(index, 0, true);//获取当前单元格上的矩形区域
            btnAdd.Size = btnEdit.Size = btnDel.Size = new Size(rectangle.Width / 3 + 1, rectangle.Height);
            btnAdd.Location = new Point(rectangle.Left, rectangle.Top);
            btnEdit.Location = new Point(rectangle.Left + btnAdd.Width, rectangle.Top);
            btnDel.Location = new Point(rectangle.Left + btnAdd.Width + btnDel.Width, rectangle.Top);
        }

        private Button GetBtnByType(string strBtnName, string strBtnText)
        {
            Button btn = new Button();
            btn.Name = strBtnName;
            btn.Text = strBtnText;
            btn.Click += btn_Click;
            return btn;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = (Button)sender;
                MessageBox.Show(string.Format("点击按钮：{0}", btn.Text));
            }
        }



    }

    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Gener { get; set; }
    }


}
