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
        public static List<String> listTableNameID = new List<string>();

        public static DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                this.GetTable();
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Field", Type.GetType("System.String"));
                    dt.Columns.Add("Table", Type.GetType("System.String"));
                }
            }
            
        }
        void GetTable()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBstring"].ConnectionString;
            SqlCommand cmd = new SqlCommand();
            string query = "SELECT object_id AS VALUE, name as TABLE_NAME FROM SYS.tables WHERE name NOT LIKE 'sys%'";

            cmd.CommandText = query;
            cmd.Connection = conn;
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                TreeViewNode item = new TreeViewNode();
                item.Text = sdr["TABLE_NAME"].ToString();
                item.Name= sdr["VALUE"].ToString();
                item.AllowCheck = true;
                GetColumnName(item.Text, ref item);
                ASPxTreeView1.Nodes.Add(item);
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

        public void removeDataTableRow(String tenCotXoa, String tenBangXoa)
        {
            String tenCot = "", tenBang = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tenCot = dt.Rows[i]["Field"].ToString();
                tenBang = dt.Rows[i]["Table"].ToString();
                if (tenCot.Equals(tenCotXoa) && tenBang.Equals(tenBangXoa))
                {
                    dt.Rows.RemoveAt(i);
                }
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();

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
                        removeDataTableRow(node.Text.ToString(), node.Name.ToString());
                    });
                    listTableNameID.Remove(e.Node.Name.ToString());
                    listTableName.Remove(e.Node.Text.ToString());
                }
                else 
                {
                    listTableName.Add(e.Node.Text);
                    listTableNameID.Add(e.Node.Name.ToString());
                }
            }
            else//NODES COLUMN
            {
                if (e.Node.Checked)
                {
                    if (!e.Node.Parent.Checked)
                    {
                        e.Node.Parent.Checked = true;
                        listTableName.Add(e.Node.Parent.Text);
                        listTableNameID.Add(e.Node.Parent.Name.ToString());
                    }

                    dt.Rows.Add();
                    dt.Rows[dt.Rows.Count-1]["Field"] = e.Node.Text.ToString();
                    dt.Rows[dt.Rows.Count-1]["Table"] = e.Node.Name.ToString();

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    removeDataTableRow(e.Node.Text.ToString(), e.Node.Name.ToString());
                }
            }
            btnCreateQuery_Click(new object(), new EventArgs());
        }
        
        //Kết các bảng có quan hệ Khóa ngoại với nhau
        public void GetRelationship(ref List<String> listJoin, String tableA, String tableB)
        {
            List<String> listKey = new List<string>();
            SqlConnection conn = new SqlConnection();
            
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["DBstring"].ConnectionString;
            SqlCommand cmd = new SqlCommand();
            string query = "(SELECT TAB.name +'.'+ COL.name FROM sys.columns COL, sys.tables TAB, sys.sysforeignkeys  " +
                            "WHERE((fkeyid = "+tableA+" AND rkeyid = "+tableB+ ") OR (rkeyid =  " + tableA + " and fkeyid = " + tableB + ")) AND COL.object_id = TAB.object_id " +
                            "AND COL.object_id = fkeyid AND COL.column_id = fkey) " +
                            "UNION " +
                            "(SELECT TAB.name + '.' + COL.name FROM sys.columns COL, sys.tables TAB, sys.sysforeignkeys " +
                            " WHERE((fkeyid = "+tableA+" AND rkeyid = "+tableB+ ") OR(rkeyid = " + tableA + " and fkeyid = " + tableB + ")) AND COL.object_id = TAB.object_id " +
                            " AND COL.object_id = rkeyid AND COL.column_id = rkey)";

            cmd.CommandText = query;
            cmd.Connection = conn;
            conn.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                listKey.Add(sdr.GetString(0));
            }
            conn.Close();
            for(int i = 0; i < listKey.Count / 2; i++)
            {
                listJoin.Add(listKey.ElementAt(i) + "=" + listKey.ElementAt(listKey.Count / 2 + i));
            }
        }

        protected void btnCreateQuery_Click(object sender, EventArgs e)
        {
            string mess = "";
            lbErorr.Text = "";
            //Xử lý mệnh đề FROM
            string tableName = (listTableName.Count > 0) ? "\nFROM " + string.Join(", ", listTableName) : "";
            if (tableName.Equals(""))
            {
                txtQuery.Text = "";
                return;
            }

            int columnCount = 0;
            if (GridView1.Rows.Count > 0)
            {
                columnCount = GridView1.Rows[0].Cells.Count;
            }    
            int fieldCell = columnCount - 2;
            int tableCell = columnCount - 1;
            String strBang = "";
            String strCot = "";
            List<String> listSelect = new List<string>();
            List<String> listJoin = new List<string>();
            List<String> listDk = new List<string>();
            List<String> listDkOr = new List<string>();
            List<String> listState = new List<string>();
            List<String> listGroupBy = new List<string>();
            List<String> listSort = new List<string>();
            List<String> listHavingAnd = new List<string>();
            List<String> listHavingOr = new List<string>();
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                
                TextBox rename = (TextBox)GridView1.Rows[i].Cells[4].FindControl("TextBoxName");
                CheckBox check = (CheckBox)GridView1.Rows[i].Cells[0].FindControl("ColumnChecked");
                DropDownList total = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("DropDownListState");
                DropDownList sort = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("DropDownListSort");
                TextBox dieuKien = (TextBox)GridView1.Rows[i].Cells[3].FindControl("TextBoxDieuKien");
                TextBox dieuKienOr = (TextBox)GridView1.Rows[i].Cells[4].FindControl("TextBoxOr");
                TextBox tbHavingAnd = (TextBox)GridView1.Rows[i].Cells[5].FindControl("TextBoxHavingAnd");
                TextBox tbHavingOr = (TextBox)GridView1.Rows[i].Cells[6].FindControl("TextBoxHavingOr");

                strBang = GridView1.Rows[i].Cells[tableCell].Text;
                strCot = GridView1.Rows[i].Cells[fieldCell].Text;

                //Nếu có chọn total(sum,min,max) -> có thể có Having và ta cần set đk cho having
                if (total.SelectedValue.ToString()!="")
                {
                    String strTotal = total.SelectedValue + "(" + strBang.ToString() + "." + strCot.ToString() + ")";
                    
                    if (rename.Text.ToString() != "")
                    {
                        listState.Add(strTotal + " AS " + total.SelectedValue.ToString().ToLower()+"Of" +rename.Text.ToString());
                    }
                    else
                    {
                        listState.Add(strTotal + " AS " + total.SelectedValue.ToString().ToLower() + "Of" + strCot.ToString());
                    }
                    
                    
                    if (tbHavingAnd.Text.ToString() != "")
                    {
                        listHavingAnd.Add(strTotal + tbHavingAnd.Text.ToString());

                    }
                    //Lấy Đk OR
                    if (tbHavingOr.Text.ToString() != "")
                    {
                        listHavingOr.Add(strTotal + tbHavingOr.Text.ToString());
                    }

                    //Lấy sort theo total(sum,min,max,..)
                    if (sort.SelectedValue.ToString() != "")
                    {
                        listSort.Add(strTotal + " " + sort.SelectedValue);
                    }
                }
                else //Lấy sort theo đk bthuong
                if (sort.SelectedValue.ToString() != "")
                {
                    listSort.Add(strBang.ToString() + "." + strCot.ToString() + " " + sort.SelectedValue);
                }


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
                if (check.Checked)
                {
                    if (rename.Text.ToString() != "")
                    {
                        listSelect.Add(strBang.ToString() + "." + strCot.ToString() + " AS " + rename.Text.ToString());
                    }
                    else
                    {
                        listSelect.Add(strBang.ToString() + "." + strCot.ToString());
                    }
                }
            }

            //lấy những field cần Group By - (là những field đc select nhưng ko có thực hiện các lệnh tính toán nào)
            if (listState.Count() > 0)
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    CheckBox check = (CheckBox)GridView1.Rows[i].Cells[0].FindControl("ColumnChecked");
                    if(check.Checked)
                    {
                        strBang = GridView1.Rows[i].Cells[tableCell].Text;
                        strCot = GridView1.Rows[i].Cells[fieldCell].Text;
                        listGroupBy.Add(strBang.ToString() + "." + strCot.ToString());
                    }
                }
            }
            

            //JOIN các bảng lại với nhau
            
            for (int i = 0; i < listTableNameID.Count - 1; i++)
            {
                for (int j = i + 1; j < listTableNameID.Count; j++)
                {
                    GetRelationship(ref listJoin, listTableNameID.ElementAt(i), listTableNameID.ElementAt(j));
                }
            }

            // Xử lý mệnh đề Select
            String select = (listSelect.Count > 0) ? "SELECT " + string.Join(", ", listSelect) : "SELECT * ";

            // Xử lý nối câu đk chọn với các câu đk mà người dùng nhập
            String where = string.Join(" AND ", listJoin);
            String dk= string.Join(" AND ", listDk);
            String dkOr = string.Join(" AND ", listDkOr);

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

            //Xử lý mệnh đề tính toán và group by (nếu có)
            String totals = (listState.Count > 0) ? ", " + string.Join(", ", listState) : "";
            String groupBy = (listGroupBy.Count > 0) ? "\n GROUP BY " + string.Join(", ", listGroupBy) : "";

            //Xử lý mệnh đề Having nếu có
            String havingAnd = (listHavingAnd.Count > 0) ? "\n HAVING (" + string.Join(" AND ", listHavingAnd) + ")" : "";
            String havingOr = "";

            if (listHavingAnd.Count == 0)
            {
                if (listHavingOr.Count > 0)
                {
                    havingOr = "\n HAVING (" + string.Join(" AND ", listHavingOr) + ")";
                }
            }
            else if (listHavingOr.Count > 0)
            {
                havingOr = " OR (" + string.Join(" AND ", listHavingOr) + ")";
            }

            //xử lý mệnh đề Order By
            String sorts = listSort.Count() > 0 ? "\n ORDER BY " + string.Join(", ", listSort) : "";

            //Cộng gộp các mệnh đề lại
            mess += select+ totals + tableName +where+ groupBy+ havingAnd+ havingOr + sorts;
            txtQuery.Text = mess;
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            btnCreateQuery_Click(sender, e);
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

            dt.Rows.Clear();
            GridView1.DataSource = dt;
            GridView1.DataBind();

            listTableName.Clear();
            listTableNameID.Clear();

            txtQuery.Text = "";
            lbErorr.Text = "";
        }

        protected void DropDownListState_Selected(object sender, EventArgs e)
        {
            btnCreateQuery_Click(sender, e);
        }

        protected void DropDownListSort_Selected(object sender, EventArgs e)
        {
            btnCreateQuery_Click(sender, e);
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