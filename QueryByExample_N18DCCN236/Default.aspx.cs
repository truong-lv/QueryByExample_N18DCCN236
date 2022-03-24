using DevExpress.Web.Bootstrap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QueryByExample_N18DCCN236
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                this.GetTable();
            }
        }
        void GetTable()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBstring"].ConnectionString;
            SqlCommand cmd = new SqlCommand();
            string query = "SELECT object_id AS VALUE, name as TABLE_NAME FROM SYS.tables";

            cmd.CommandText = query;
            cmd.Connection = conn;
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                BootstrapListEditItem item = new BootstrapListEditItem();
                item.Text = sdr["TABLE_NAME"].ToString();
                item.Value = sdr["VALUE"].ToString();
                ListBox.Items.Add(item);
            }
            conn.Close();

        }

        private void GetColumnName(String tableName)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBstring"].ConnectionString;
            SqlCommand cmd = new SqlCommand();
            string query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "' AND COLUMN_NAME NOT LIKE 'rowguid%'";

            cmd.CommandText = query;
            cmd.Connection = conn;
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                BootstrapListEditItem item = new BootstrapListEditItem();
                item.Text = sdr["COLUMN_NAME"].ToString();
                item.Value = tableName;
                ListBoxColumn.Items.Add(item);
            }
            conn.Close();
        }

        protected void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListBox.Items.Clear();
            ListBoxColumn.Items.Clear();
            //listColumnNameTemp2.Clear();
            //listTableNameTemp2.Clear();
            Label.Text = ListBox.Items.Count().ToString();
            foreach (BootstrapListEditItem item in ListBox.Items)
            {
                if (item.Selected)
                {
                    GetColumnName(item.Text);

                }
            }
            //for (int i = 0; i < listTableName.Count; i++)
            //{
            //    GetColumnName(listTableName[i].ToString());

            //}
            //foreach (ListItem item in CheckBoxListColumn.Items)
            //{
            //    listColumnNameTemp2.Add(item.Text.ToString());
            //    listTableNameTemp2.Add(item.Value.ToString());
            //}
        }
    }
}