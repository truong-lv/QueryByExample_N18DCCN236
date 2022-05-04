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
        //Tạo truy vấn cho mệnh đề FROM
        public static List<String> listTableName = new List<string>();

        //public static List<String> listColumnName = new List<string>();
        //public static List<String> listColumnNameTemp1 = new List<string>();
        //public static List<String> listTableNameTemp1 = new List<string>();

        public static List<String> listColumnNameTemp2 = new List<string>();
        public static List<String> listTableNameTemp2 = new List<string>();
        public static DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                this.GetTable();
                

                dt.Columns.Add("Field", Type.GetType("System.String"));
                dt.Columns.Add("Table", Type.GetType("System.String"));
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
        

        protected void PerformActionOnNodesRecursive(TreeViewNodeCollection nodes, Action<TreeViewNode> action)
        {
            foreach (TreeViewNode node in nodes)
            {
                action(node);
                if (node.Nodes.Count > 0)
                    PerformActionOnNodesRecursive(node.Nodes, action);
            }
        }
        protected void ASPxTreeView1_CheckedChanged(object source, TreeViewNodeEventArgs e)
        {
            // NODES TABLE
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
            else//NODES COLUMN
            {
                if (!e.Node.Parent.Checked)
                {
                    e.Node.Parent.Checked = true;
                    CheckBoxListTable_SelectedIndexChanged();
                }
                if (e.Node.Checked)
                {
                    dt.Rows.Add();
                    dt.Rows[dt.Rows.Count-1]["Field"] = e.Node.Text.ToString();
                    dt.Rows[dt.Rows.Count-1]["Table"] = e.Node.Name.ToString();

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    String tenCot="", tenBang="";
                   
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        tenCot=dt.Rows[i]["Field"].ToString();
                        tenBang=dt.Rows[i]["Table"].ToString();
                        if (tenCot.Equals(e.Node.Text.ToString()) && tenBang.Equals(e.Node.Name.ToString()))
                        {
                            dt.Rows.RemoveAt(i);
                        }
                    }
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                CheckBoxListColumn_SelectedIndexChanged();
            }
        }
        protected void CheckBoxListTable_SelectedIndexChanged()
        {
            
            listTableName.Clear();
            listTableNameTemp2.Clear();
            txtQuery.Text = "";
            lbErorr.Text = "";
            PerformActionOnNodesRecursive(ASPxTreeView1.Nodes, delegate (TreeViewNode node) {
                if (node.Parent.Parent == null)
                {
                    if (node.Checked)
                    {
                        listTableName.Add(node.Text);
                        listTableNameTemp2.Add(node.Name.ToString());
                    }
                }
            });

            //PerformActionOnNodesRecursive(ASPxTreeView1.Nodes, delegate (TreeViewNode node)
            //{
            //    if (node.Parent.Parent != null && node.Parent.Checked)
            //    {
            //        listColumnNameTemp2.Add(node.Text.ToString());
            //        listTableNameTemp2.Add(node.Parent.Name.ToString());
            //    }
            //});
        }

        protected void CheckBoxListColumn_SelectedIndexChanged()
        {
            listColumnNameTemp2.Clear();
            txtQuery.Text = "";
            lbErorr.Text = "";
            PerformActionOnNodesRecursive(ASPxTreeView1.Nodes, delegate (TreeViewNode node) {
                if (node.Parent.Parent != null && node.Parent.Checked)
                {
                    if(node.Checked)
                    {
                        listColumnNameTemp2.Add(node.Text.ToString());
                        
                    }
                }
            });
            
            


        }

        //Check valid SQL syntax
        //public List<string> Parse(string sql)
        //{
        //    TSql100Parser parser = new TSql100Parser(false);
        //    IScriptFragment fragment;
        //    IList<ParseError> errors;
        //    fragment = parser.Parse(new StringReader(sql), out errors);
        //    if (errors != null && errors.Count > 0)
        //    {
        //        List<string> errorList = new List<string>();
        //        foreach (var error in errors)
        //        {
        //            errorList.Add(error.Message);
        //        }
        //        return errorList;
        //    }
        //    return null;
        //}
        public String GetRelationship(String tableA, String tableB)
        {
            List<String> listJoin = new List<string>();
            SqlConnection conn = new SqlConnection();
            String result = "";
            
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBstring"].ConnectionString;
            SqlCommand cmd = new SqlCommand();
            string query = "exec SP_FIND_FOREIGNKEY " + tableA + ", " + tableB;

            cmd.CommandText = query;
            cmd.Connection = conn;
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                listJoin.Add(sdr.GetString(0));
            }
            conn.Close();
            for(int i = 0; i < listJoin.Count / 2; i++)
            {
                result += listJoin.ElementAt(i) + "=" + listJoin.ElementAt(listJoin.Count / 2 -1+ i)+((i<listJoin.Count/2-1)?" AND ":"");
            }
            return result;
        }
        protected void btnCreateQuery_Click(object sender, EventArgs e)
        {
            string mess = "";
            lbErorr.Text = "";
            string tableName = string.Join(", ", listTableName);

            int columnCount = 0;
            if (GridView1.Rows.Count > 0)
            {
                columnCount = GridView1.Rows[0].Cells.Count;
            }    
            int fieldCell = columnCount - 2;
            int tableCell = columnCount - 1;
            String columnName = "";
            mess = "SELECT ";
            String strBang = "";
            String strCot = "";
            List<String> listDk = new List<string>();
            List<String> listDkOr = new List<string>();
            List<String> listState = new List<string>();
            List<String> listGroupBy = new List<string>();
            List<String> listSort = new List<string>();
            List<String> listHavingAnd = new List<string>();
            List<String> listHavingOr = new List<string>();
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                TextBox dieuKien = (TextBox)GridView1.Rows[i].Cells[3].FindControl("TextBoxDieuKien");
                TextBox dieuKienOr = (TextBox)GridView1.Rows[i].Cells[4].FindControl("TextBoxOr");
                CheckBox check = (CheckBox)GridView1.Rows[i].Cells[0].FindControl("ColumnChecked");
                DropDownList total = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("DropDownListState");
                DropDownList sort = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("DropDownListSort");

                strBang = GridView1.Rows[i].Cells[tableCell].Text;
                strCot = GridView1.Rows[i].Cells[fieldCell].Text;

                //Lấy total nếu vào đk này -> có thể có Having và ta cần set đk cho having
                if (total.SelectedValue.ToString()!="")
                {
                    String strTotal = total.SelectedValue + "(" + strBang.ToString() + "." + strCot.ToString() + ")";
                    listState.Add(strTotal+" AS " + total.SelectedValue.ToLower() + "Of" + strCot.ToString());
                    
                    if (dieuKien.Text.ToString() != "")
                    {
                        listHavingAnd.Add(strTotal + dieuKien.Text.ToString());

                    }
                    //Lấy Đk OR
                    if (dieuKienOr.Text.ToString() != "")
                    {
                        listHavingOr.Add(strTotal + dieuKienOr.Text.ToString());
                    }
                }
                
                else// nếu không vào thì ta set đk bthuong
                {
                    //Lấy Đk AND
                    if (dieuKien.Text.ToString() != "")
                    {
                        listDk.Add(strBang.ToString() + "." + strCot.ToString() + dieuKien.Text.ToString());

                    }

                    //Lấy Đk OR
                    if (dieuKienOr.Text.ToString() != "")
                    {
                        listDkOr.Add(strBang.ToString() + "." + strCot.ToString() + dieuKienOr.Text.ToString());
                    }

                    //Lấy tên Field cần SELECT
                    if (check.Checked) { mess += strBang.ToString() + "." + strCot.ToString() + ", "; }
                       
                }

                //Lấy sort
                if (sort.SelectedValue.ToString() != "")
                {
                    listSort.Add(strBang.ToString() + "." + strCot.ToString() + " " + sort.SelectedValue);
                }
            }

            //lấy những field cần Group By
            if (listState.Count() > 0)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    DropDownList total = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("DropDownListState");
                    if(total.SelectedValue.ToString() == "")
                    {
                        strBang = GridView1.Rows[i].Cells[tableCell].Text;
                        strCot = GridView1.Rows[i].Cells[fieldCell].Text;
                        listGroupBy.Add(strBang.ToString() + "." + strCot.ToString());
                    }
                }
            }
            
            mess = mess.Substring(0, mess.Length - 2);
            if(mess== "SELEC")
            {
                txtQuery.Text = "";
                return;
            }

            //JOIN các bảng lại với nhau
            String where = "";
            for (int i = 0; i < listTableNameTemp2.Count - 1; i++)
            {
                for (int j = i + 1; j < listTableNameTemp2.Count; j++)
                {
                    where += GetRelationship(listTableNameTemp2.ElementAt(i), listTableNameTemp2.ElementAt(j));
                }
            }

            String dk= string.Join(" AND ", listDk);
            String dkOr = string.Join(" AND ", listDkOr);

            String totals =(listState.Count > 0) ? ", " + string.Join(", ", listState) : "";
            String groupBy = (listGroupBy.Count > 0) ? "\n GROUP BY " + string.Join(", ", listGroupBy) : "";
            String havingAnd= (listHavingAnd.Count > 0) ? "\n HAVING (" + string.Join(" AND ", listHavingAnd)+")" : "";
            String havingOr = "";
            if (listHavingAnd.Count==0)
            {
                if (listHavingOr.Count > 0)
                {
                    havingOr = "\n HAVING (" + string.Join(" AND ", listHavingOr)+")";
                }
            }
            else if (listHavingOr.Count > 0)
            {
                havingOr = " OR (" + string.Join(" AND ", listHavingOr)+")";
            }

            String sorts = listSort.Count()>0?"\n ORDER BY "+string.Join(", ", listSort):"";

            if (!where.Equals(""))
            {
                where = "\nWHERE " + where;
                if (!dk.Equals(""))
                {
                    where += " AND " + dk;
                }
                if (!dkOr.Equals(""))
                {
                    where += " OR (" + dkOr + ")";
                }
            }
            else if (!dk.Equals(""))
            {
                where = "\nWHERE " + dk;
                if (!dkOr.Equals(""))
                {
                    where += " OR (" + dkOr + ")";
                }
            }
            else if (!dkOr.Equals(""))
            {
                where = "\nWHERE " + dkOr;
            }

            mess += totals + "\n FROM " + tableName +where+ groupBy+ havingAnd+ havingOr + sorts;
            txtQuery.Text = mess;
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            String query = txtQuery.Text;

            //Kiêm tra câu truy vấn
            try {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBstring"].ConnectionString;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
            }
            catch (SqlException ex)
            {
                lbErorr.Text =ex.Message;
                return;
            }
            
            String title = txtTitle.Text;
            Session["title"] = title;
            Session["query"] = query;
            Response.Redirect("MyReport.aspx");
            Server.Execute("MyReport.aspx");
        }

        protected void btnClearnSelect_Click(object sender, EventArgs e)
        {
            PerformActionOnNodesRecursive(ASPxTreeView1.Nodes, delegate (TreeViewNode node) { 
                node.Checked = false;
            });
            CheckBoxListTable_SelectedIndexChanged();
            CheckBoxListColumn_SelectedIndexChanged();
        }

        protected void Checked_OnChanged(object sender, EventArgs e)
        {
            btnCreateQuery_Click(sender, e);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}