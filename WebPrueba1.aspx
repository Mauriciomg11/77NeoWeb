<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="WebPrueba1.aspx.cs" Inherits="_77NeoWeb.WebPrueba1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="Botones" class="row">
        <div class="col-sm-4">
            <asp:Button ID="BtnCargaMaxiva" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnCargaMaxiva_Click" Text="Cargar" Width="100%" />
            <asp:FileUpload ID="FileUpCot" runat="server" Font-Size="9px" />
        </div>
    </div>
    <div class="row">
        <div class="table-responsive Scroll-table2">
            <asp:GridView ID="Grdprueba" runat="server" EmptyDataText="No existen registros ..!"
                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                <FooterStyle CssClass="GridFooterStyle" />
                <HeaderStyle CssClass="GridCabecera" />
                <RowStyle CssClass="GridRowStyle" />
                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
            </asp:GridView>
        </div>
    </div>
</asp:Content>
