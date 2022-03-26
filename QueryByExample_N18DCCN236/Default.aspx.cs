using DevExpress.Web;
using DevExpress.Web.Bootstrap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QueryByExample_N18DCCN236
{
    public partial class _Default : Page
    {
        public static List<String> listTableName = new List<string>();
        public static List<String> listColumnName = new List<string>();
        public static List<String> listColumnNameTemp1 = new List<string>();
        public static List<String> listTableNameTemp1 = new List<string>();
        public static List<String> listColumnNameTemp2 = new List<string>();
        public static List<String> listTableNameTemp2 = new List<string>();

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
                //BootstrapListEditItem item = new BootstrapListEditItem();
                TreeViewNode item = new TreeViewNode();
                item.Text = sdr["TABLE_NAME"].ToString();
                item.Name= sdr["VALUE"].ToString();
                item.AllowCheck = true;
                GetColumnName(item.Text, ref item);
                ASPxTreeView1.Nodes.Add(item);
                //ListTableName.Items.Add(item);
                //ListTableName.AutoPostBack=true;
            }
            conn.Close();

        }

        private void GetColumnName(String tableName, ref TreeViewNode node)
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
                //BootstrapListEditItem item = new BootstrapListEditItem();
                TreeViewNode item = new TreeViewNode();
                item.Text = sdr["COLUMN_NAME"].ToString();
                item.Name = tableName.ToString();
                item.AllowCheck = true;
                node.Nodes.Add(item);
                DataTable dt = new DataTable(tableName);

            }
            conn.Close();
        }

        protected void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ListBox.Items.Clear();
            //ListBoxColumn.Items.Clear();
            //listColumnNameTemp2.Clear();
            //listTableNameTemp2.Clear();
            //foreach (BootstrapListEditItem item in ListTableName.Items)
            //{
            //    if (item.Selected)
            //    {
            //        GetColumnName(item.Text);

            //    }
            //}
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

        

        protected void PerformActionOnNodesRecursive(TreeViewNodeCollection nodes, Action<TreeViewNode> action)
        {
            foreach (TreeViewNode node in nodes)
            {
                action(node);
                if (node.Nodes.Count > 0)
                    PerformActionOnNodesRecursive(node.Nodes, action);
            }
        }
        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            PerformActionOnNodesRecursive(ASPxTreeView1.Nodes, delegate (TreeViewNode node) { node.Checked = false;});
        }

        protected void ASPxTreeView1_CheckedChanged(object source, TreeViewNodeEventArgs e)
        {
            if (e.Node.Parent.Parent==null)
            {
                if (!e.Node.Checked)
                {
                    PerformActionOnNodesRecursive(e.Node.Nodes, delegate (TreeViewNode node)
                    {
                        node.Checked = false;
                    });
                    CheckBoxListColumn_SelectedIndexChanged();
                }
                CheckBoxListTable_SelectedIndexChanged();
            }
            else if(e.Node.Parent.Checked)
            {
                CheckBoxListColumn_SelectedIndexChanged();
            }
            else if(!e.Node.Parent.Checked)
            {
                e.Node.Checked = false;
            }
        }
        protected void CheckBoxListTable_SelectedIndexChanged()
        {
            listTableName.Clear();
            listColumnNameTemp2.Clear();
            listTableNameTemp2.Clear();
            txtQuery.Text = "";
            PerformActionOnNodesRecursive(ASPxTreeView1.Nodes, delegate (TreeViewNode node) {
                if (node.Parent.Parent == null)
                {
                    if (node.Checked)
                    {
                        listTableName.Add(node.Text);

                    }
                }
            });

            PerformActionOnNodesRecursive(ASPxTreeView1.Nodes, delegate (TreeViewNode node) {
                if (node.Parent.Parent != null &&node.Parent.Checked)
                {
                    listColumnNameTemp2.Add(node.Text.ToString());
                    listTableNameTemp2.Add(node.Name.ToString());
                }
            });
            //string where = "";
            //where += string.Join(", ", listColumnNameTemp2);

            //LabelMess.Text = where;
        }

        protected void CheckBoxListColumn_SelectedIndexChanged()
        {
            PerformActionOnNodesRecursive(ASPxTreeView1.Nodes, delegate (TreeViewNode node) {
                if (node.Parent.Parent != null && node.Parent.Checked)
                {
                    if(node.Checked)
                    {
                        listColumnNameTemp1.Add(node.Text.ToString());
                        listTableNameTemp1.Add(node.Name.ToString());
                    }
                }
            });
            
            DataTable dt = new DataTable();

            dt.Columns.Add("TenCot", Type.GetType("System.String"));
            dt.Columns.Add("TenBang", Type.GetType("System.String"));

            string[] arrTemp1 = listColumnNameTemp1.ToArray();
            string[] arrTemp2 = listTableNameTemp1.ToArray();

            for (int i = 0; i < arrTemp1.GetLength(0); i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["TenCot"] = arrTemp1[i];
                dt.Rows[i]["TenBang"] = arrTemp2[i];
            }
            listColumnNameTemp1.Clear();
            listTableNameTemp1.Clear();

            GridView1.DataSource = dt;
            GridView1.DataBind();


        }

        protected void BootstrapButton1_Click(object sender, EventArgs e)
        {
            string mess = "";

            string tableName = string.Join(", ", listTableName);
            String columnName = "";
            mess = "SELECT ";
            String dk = "";
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                TextBox strBang = new TextBox();
                TextBox strCot = new TextBox();
                TextBox dieuKien = (TextBox)GridView1.Rows[i].Cells[2].FindControl("TextBoxDieuKien");
                if (dieuKien.Text.ToString() != "")
                {
                    strBang.Text = GridView1.Rows[i].Cells[4].Text;
                    strCot.Text = GridView1.Rows[i].Cells[3].Text;
                    dk += " AND " + strBang.Text.ToString() + "." + strCot.Text.ToString() + dieuKien.Text.ToString();
                }

                strBang.Text = GridView1.Rows[i].Cells[4].Text;
                strCot.Text = GridView1.Rows[i].Cells[3].Text;
                String dropdown = "";


                columnName = strBang.Text.ToString() + "." + strCot.Text.ToString();

                if (i < GridView1.Rows.Count - 1)
                {
                    columnName += ", ";
                }
                mess += columnName;

            }

            String where = "";
            int w = 0;
            for (int i = 0; i < listColumnNameTemp2.Count - 1; i++)
            {
                for (int j = i + 1; j < listColumnNameTemp2.Count; j++)
                {
                    if (listColumnNameTemp2[j] == listColumnNameTemp2[i])
                    {

                        w++;
                        if (w > 1)
                        {
                            where += " AND " + listTableNameTemp2[i].ToString() + "." + listColumnNameTemp2[i] + " = " + listTableNameTemp2[j].ToString() + "." + listColumnNameTemp2[j];
                        }
                        else
                        {
                            where += listTableNameTemp2[i].ToString() + "." + listColumnNameTemp2[i] + " = " + listTableNameTemp2[j].ToString() + "." + listColumnNameTemp2[j];
                        }
                    }
                }
            }
            if (!where.Equals(""))
            {
                where = " WHERE " + where;
            }

            mess += " FROM " + tableName + where + dk;
            txtQuery.Text = mess;
        }

        protected void BootstrapButton2_Click(object sender, EventArgs e)
        {
           
            String query = txtQuery.Text;
            String title = txtTitle.Text;
            Session["title"] = title;
            Session["query"] = query;
            Response.Redirect("MyReport.aspx");
            Server.Execute("MyReport.aspx");
        }
    }
}