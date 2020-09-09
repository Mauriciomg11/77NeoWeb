<%@ Page Title="" Language="C#" MasterPageFile="~/MasterReport.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmExportar.aspx.cs" Inherits="_77NeoWeb.Forms.FrmExportar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Exportar</title>
    <style type="text/css">
        .DivGrid {
            position: absolute;
            OVERFLOW: auto;
            width: 97%;
            height: 81%;
            top: 18%;
            left: 2%;
            margin-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h1>
        <asp:Label ID="LblTitulo" runat="server" Text="Label"></asp:Label></h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ImageButton ID="IbnCerrar" runat="server" CssClass="BotonCerrarPagExportar" ImageUrl="~/images/ExitV2.png"  ToolTip="Cerrar" OnClick="IbnCerrar_Click"></asp:ImageButton>
    <table id="TbOpc" class="TablaBusqueda">
        <tr>
            <td id="Td1">
                <asp:Label ID="Lbl1" runat="server" CssClass="LblEtiquet" Text="" Visible="false" ></asp:Label></td>
            <td >
                <asp:RadioButton ID="Rdb1" runat="server" GroupName="ExExc" Visible="false"/></td>
            <td >
                <asp:Label ID="Lbl2" runat="server" CssClass="LblEtiquet" Text="" Visible="false" ></asp:Label></td>
            <td >
                <asp:RadioButton ID="Rdb2" runat="server" GroupName="ExExc" Visible="false"/></td>
            <td >
                <asp:Label ID="Lbl3" runat="server" CssClass="LblEtiquet" Text="" Visible="false"></asp:Label></td>
            <td >
                <asp:RadioButton ID="Rdb3" runat="server" GroupName="ExExc" Visible="false"/></td>
        </tr>
    </table>
    <table class="TablaBusqueda">

        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
            <td>
                <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
            <td>
                <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
            <td>
                <asp:ImageButton ID="IbtExpExcel" runat="server" ToolTip="Exportar" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png"  OnClick="IbtExpExcel_Click" /></td>
        </tr>
    </table>
    <div class="DivGrid DivContendorGrid">
        <asp:GridView ID="GrdDatos" runat="server" EmptyDataText="No existen registros ..!"
            CssClass="GridControl DiseñoGrid table" GridLines="Both"  AllowPaging="true" PageSize="7"  
            OnPageIndexChanging="GrdDatos_PageIndexChanging">
            <FooterStyle CssClass="GridFooterStyle" />
            <HeaderStyle CssClass="GridCabecera" />
            <RowStyle CssClass="GridRowStyle" />
            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
        </asp:GridView>
    </div>
</asp:Content>
