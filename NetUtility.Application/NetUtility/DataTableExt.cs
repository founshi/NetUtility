using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NetUtility
{
    public class DataTableExt
    {

        /*************************************************************
         * * 动态创建datatable
         * * DataTable dt = new DataTable();
         * *增加列
         * * for (int i = 0; i < columnCount; i++)
         * * {
         * *    dt.Columns.Add("col" + i.ToString());
         * * }
         * * 新增行
         * * DataRow dr = dt.NewRow();
         * * for (int i = 0; i < columnCount; i++)
         * * {
         * *        dr[i] = "";
         * * }
         * * dt.Rows.Add(dr);
         * * 遍历 DataRow
         * * for (int i = 0; i < dr.Table.Columns.Count; i++)
         * * dr.Table.Columns[i]
         * *
         * *
         * *
         * *
         * *
         * *
         * *
         * *************************************************************/

        #region 将datarow的列转成一逗号分割的字符串
        /// <summary>
        /// 将datarow的列转成一逗号分割的字符串
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public string CreateFieldString(DataRow dr)
        {
            StringBuilder _rowString = new StringBuilder();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                _rowString.Append(dr.Table.Columns[i].ToString());
                _rowString.Append(",");
            }
            return _rowString.ToString();
        }
        #endregion




    }
}
