<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmImportar.aspx.cs" Inherits="_77NeoWeb.FrmImportar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="Upl1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <br />
            <br />
            <asp:Button ID="BtnImportarV1" runat="server" Text="Importar" OnClick="BtnImportarV1_Click" />
            <asp:Button ID="BtnExportar" runat="server" Text="Exportar Excel" OnClick="BtnExportar_Click" />
            <asp:Button ID="BtnExportar2" runat="server" Text="Exportar Excel2" OnClick="BtnExportar2_Click" />
            <br />
            <asp:FileUpload ID="FileUpload1" runat="server" ToolTip="" />
            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />

            <br />
            <asp:Button ID="BtnV3" runat="server" Text="importar" OnClick="BtnV3_Click" />


            <div class="DivGrid DivContendorGrid">
                <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!"
                    CssClass="GridControl DiseñoGrid table" GridLines="Both" AllowPaging="true" PageSize="7"
                    OnSelectedIndexChanged="GrdBusq_SelectedIndexChanged" OnPageIndexChanging="GrdBusq_PageIndexChanging">
                    <FooterStyle CssClass="GridFooterStyle" />
                    <HeaderStyle CssClass="GridCabecera" />
                    <RowStyle CssClass="GridRowStyle" />
                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    <Columns>
                        <asp:CommandField HeaderText="Selección" SelectText="Enviar" ShowSelectButton="True" HeaderStyle-Width="33px" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                </asp:GridView>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExportar" />
            <asp:PostBackTrigger ControlID="BtnExportar2" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
