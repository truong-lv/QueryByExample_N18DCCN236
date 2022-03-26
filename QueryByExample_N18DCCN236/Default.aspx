<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QueryByExample_N18DCCN236._Default" %>

<%@ Register assembly="DevExpress.Web.Bootstrap.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Data.Linq" tagprefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
       <div style="display:flex; justify-content:center;padding:15px 0">
           <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="#3333CC" Text="TIÊU ĐỀ BÁO CÁO:" Theme="MetropolisBlue">
           </dx:ASPxLabel>
        <dx:BootstrapTextBox ID="txtTitle" runat="server" >
        </dx:BootstrapTextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="row">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Bold="True" Text="Câu truy vấn:">
                </dx:ASPxLabel>
                <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" MaxLength="400" Height="200px" Width="100%" ></asp:TextBox>
            </div>
            <div class="row">
                <dx:BootstrapButton ID="BootstrapButton1" runat="server" AutoPostBack="false" OnClick="BootstrapButton1_Click" Text="Tạo truy vấn">
                </dx:BootstrapButton>
                <dx:BootstrapButton ID="BootstrapButton2" runat="server" AutoPostBack="False" OnClick="BootstrapButton2_Click" Text="Tạo báo cáo">
                </dx:BootstrapButton>
                <asp:GridView ID="GridView1" runat="server" BackColor="White"  BorderColor="#CCCCCC"  BorderWidth="1px" CellPadding="3" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" runat="server" AutoPostBack="true" Text="Chọn tất cả"/>
                        </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ColumnChecked" runat="server"  /> <%--OnCheckedChanged="Checked_OnChanged"--%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="State" >
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownList1" runat="server" ><%--OnSelectedIndexChanged="DropdownList1_Selected"--%>
                                <asp:ListItem Text="Non_Selected" Value=""></asp:ListItem>
                                <asp:ListItem Text="SORT ASC" Value="ASC"></asp:ListItem>
                                <asp:ListItem Text="SORT DESC" Value="DESC"></asp:ListItem>
                                <asp:ListItem Text="COUNT" Value="COUNT"></asp:ListItem>
                                <asp:ListItem Text="MIN" Value="MIN"></asp:ListItem>                                     
                                <asp:ListItem Text="MAX" Value="MAX"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>           
                   
                    <asp:TemplateField HeaderText="Điều Kiện">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxDieuKien" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                <HeaderStyle BackColor="#006699" Font-Bold="true" ForeColor="White"/>
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                <RowStyle ForeColor="#000066"/>
                <SelectedRowStyle BackColor="#006699" ForeColor="White" Font-Bold="true"/>
                <SortedAscendingCellStyle BackColor="#000066"/>
                <SortedAscendingHeaderStyle BackColor="#000066"/>
                <SortedAscendingCellStyle BackColor="#007DBB"/>
                <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                <SortedDescendingHeaderStyle BackColor="#00547E"/>
            </asp:GridView>
            </div>
        </div>
         <div class="col-md-4" >
             <div style="background-color:white;padding:15px">
                 <div  style="padding:0 15px; display:flex; justify-content:space-between;align-items:center">
                     <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Chọn Bảng:">
                     </dx:ASPxLabel>

                     <dx:BootstrapButton ID="ASPxButton1" runat="server" Text="Bỏ tất cả" OnClick="ASPxButton1_Click">
                     </dx:BootstrapButton>
                 </div>
                 <hr />
                <dx:ASPxTreeView ID="ASPxTreeView1" runat="server" ClientIDMode="AutoID" AllowCheckNodes="True" OnCheckedChanged="ASPxTreeView1_CheckedChanged">
                </dx:ASPxTreeView>
             </div>
        </div>
    </div>
</asp:Content>
