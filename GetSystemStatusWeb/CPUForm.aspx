<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CPUForm.aspx.cs" Inherits="GetSystemStatusWeb.CPUForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>CPU</h2>
    <p><asp:Label id="cpuName" runat="server"></asp:Label></p>
    <p id="chart_panel" runat="server">

    </p>
</asp:Content>
