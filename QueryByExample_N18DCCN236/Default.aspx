<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QueryByExample_N18DCCN236._Default" %>

<%@ Register assembly="DevExpress.Web.Bootstrap.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Data.Linq" tagprefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
       <div style="display:flex; justify-content:left;align-items:center;padding:15px 0">
           <dx:ASPxLabel ID="ASPxLabel2" runat="server" Font-Bold="False" Font-Size="Medium" Text="TIÊU ĐỀ BÁO CÁO:" Theme="Office2010Black">
           </dx:ASPxLabel>
           <dx:ASPxTextBox ID="txtTitle" runat="server" Width="30%" Font-Bold="True" Font-Names="Tahoma" Font-Size="Medium" Theme="Default">
           </dx:ASPxTextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="row">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" Font-Bold="False" Text="Câu truy vấn:">
                </dx:ASPxLabel>
                <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" MaxLength="400" Height="200px" Width="100%" ></asp:TextBox>
            </div>
            <div class="row">
                <div style="padding:5px;">
                    <dx:ASPxButton ID="btnCreateQuery" runat="server" Font-Names="Tahoma" OnClick="btnCreateQuery_Click" Text="Tạo truy vấn" Theme="MaterialCompact">
                        <Image IconID="dashboards_newdatasource_svg_white_16x16">
                        </Image>
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btnReport" runat="server" OnClick="btnReport_Click" Text="Tạo Báo Cáo" Theme="Office365">
                        <Image IconID="reports_showprintingwarnings_svg_white_16x16">
                        </Image>
                    </dx:ASPxButton>
                </div>
                <asp:GridView ID="GridView1" runat="server" BackColor="White"  BorderColor="#CCCCCC"  BorderWidth="1px" CellPadding="3" Width="100%" ClientIDMode="AutoID" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="label" runat="server" Text="Show"/>
                        </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ColumnChecked" runat="server" OnCheckedChanged="Checked_OnChanged" AutoPostBack="true"/>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="State" >
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownListState" runat="server" ><%--OnSelectedIndexChanged="DropdownList1_Selected"--%>
                                <asp:ListItem Text="Non_Selected" Value=""></asp:ListItem>
                                <asp:ListItem Text="SUM" Value="SUM"></asp:ListItem>
                                <asp:ListItem Text="COUNT" Value="COUNT"></asp:ListItem>
                                <asp:ListItem Text="MIN" Value="MIN"></asp:ListItem>                                     
                                <asp:ListItem Text="MAX" Value="MAX"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>       
                    
                   <asp:TemplateField HeaderText="Sort" >
                        <ItemTemplate>
                            <asp:DropDownList ID="DropDownListSort" runat="server" ><%--OnSelectedIndexChanged="DropdownList1_Selected"--%>
                                <asp:ListItem Text="Non_Selected" Value=""></asp:ListItem>
                                <asp:ListItem Text="SORT ASC" Value="ASC"></asp:ListItem>
                                <asp:ListItem Text="SORT DESC" Value="DESC"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    
                    <asp:TemplateField HeaderText="Criteria">
                        <ItemTemplate >
                            <asp:TextBox ID="TextBoxDieuKien" runat="server" Width="90%"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Or">
                        <ItemTemplate >
                            <asp:TextBox ID="TextBoxOr" runat="server" Width="90%"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                <HeaderStyle BackColor="#006699" Font-Bold="true" ForeColor="White" Wrap="False" Height="30px"/>
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

                     <dx:ASPxButton ID="btnClearnSelect" runat="server" OnClick="btnClearnSelect_Click" Text="Bỏ chọn tất cả" Theme="Office2003Olive">
                         <Image IconID="diagramicons_del_svg_white_16x16">
                         </Image>
                     </dx:ASPxButton>
                 </div>
                 <hr />
                <dx:ASPxTreeView ID="ASPxTreeView1" runat="server" ClientIDMode="AutoID" AllowCheckNodes="True" OnCheckedChanged="ASPxTreeView1_CheckedChanged">
                    <SettingsLoadingPanel Delay="0" />
                </dx:ASPxTreeView>
             </div>
        </div>
    </div>
</asp:Content>
