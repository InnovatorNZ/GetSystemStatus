<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GetSystemStatusWeb._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>GetSystemStatus Web Version</h1>
        <p class="lead">Monitor your computer through browser.</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>CPU Usage</h2>
            <p>
                Monitor CPU per core utilizations.
            </p>
            <p>
                <a class="btn btn-default" href="CPUForm.aspx">Look &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>RAM Usage</h2>
            <p>
                Coming soon
            </p>
        </div>
        <div class="col-md-4">
            <h2>Disk usage</h2>
            <p>
                Coming soon
            </p>
        </div>
    </div>
    <!--
    <div class="row" id="chartrow" runat="server">
        <div class="col-md-4">
            <asp:Chart ID="Chart1" runat="server">
                <series>
                    <asp:Series Name="Series1">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
            </asp:Chart>
        </div>
    </div>
    -->
    <div id="morechartrow" runat="server">

    </div>

</asp:Content>
