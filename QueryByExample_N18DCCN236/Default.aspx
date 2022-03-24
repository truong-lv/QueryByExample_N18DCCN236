<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QueryByExample_N18DCCN236._Default" %>

<%@ Register assembly="DevExpress.Web.Bootstrap.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.Bootstrap" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <dx:ASPxLabel ID="Label" runat="server" Text="Debug">
            </dx:ASPxLabel>
            <dx:BootstrapListBox  ID="ListBox" runat="server" OnSelectedIndexChanged="ListBox_SelectedIndexChanged" AutoPostBack="true" SelectionMode="CheckColumn" EnableSelectAll="true">
            </dx:BootstrapListBox>
        </div>
        <div class="col-md-8">
            <dx:BootstrapListBox SelectionMode="CheckColumn" EnableSelectAll="true" ID="ListBoxColumn" runat="server" >
            </dx:BootstrapListBox>
        </div>
       
    </div>

</asp:Content>
