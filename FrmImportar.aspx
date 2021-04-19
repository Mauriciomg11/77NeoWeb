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
            <asp:RadioButton ID="RdbAK" runat="server" CssClass="LblEtiquet" Text="&nbsp Datos Aeronave" GroupName="D" />
            <asp:RadioButton ID="RdbHHK" runat="server" CssClass="LblEtiquet" Text="&nbsp Hisórico contadores HK" GroupName="D" />
            <asp:RadioButton ID="RdbInvHK" runat="server" CssClass="LblEtiquet" Text="&nbsp Elementos Instalados HK" GroupName="D" />
            <asp:RadioButton ID="RdbHistSN" runat="server" CssClass="LblEtiquet" Text="&nbsp Hisórico Elementos (SN)" GroupName="D" />
            <asp:RadioButton ID="RdbRtes" runat="server" CssClass="LblEtiquet" Text="&nbsp Reportes" GroupName="D" />
            <asp:RadioButton ID="RdbPlantMstra" runat="server" CssClass="LblEtiquet" Text="&nbsp Plantilla Maestra" GroupName="D" />
            <asp:RadioButton ID="RdbSvcMnto" runat="server" CssClass="LblEtiquet" Text="&nbsp Servicios Manto" GroupName="D" />
            <asp:RadioButton ID="RdbRcsoFscoSM" runat="server" CssClass="LblEtiquet" Text="&nbsp Rercurso SvcManto" GroupName="D" />
            <asp:RadioButton ID="RdbLicncSM" runat="server" CssClass="LblEtiquet" Text="&nbsp Licencia SvcManto" GroupName="D" />
            <asp:RadioButton ID="RdbOT" runat="server" CssClass="LblEtiquet" Text="&nbsp OT" GroupName="D" />
            <asp:RadioButton ID="RdbWS" runat="server" CssClass="LblEtiquet" Text="&nbsp Work Sheet" GroupName="D" />
            <asp:RadioButton ID="RdbInventr" runat="server" CssClass="LblEtiquet" Text="&nbsp Inventario" GroupName="D" />
            <asp:RadioButton ID="RdbHisSvcCumpl" runat="server" CssClass="LblEtiquet" Text="&nbsp Servicios Cumplidos" GroupName="D" />
            <asp:RadioButton ID="RdbLV" runat="server" CssClass="LblEtiquet" Text="&nbsp Libros de Vuelo" GroupName="D" />
            <asp:RadioButton ID="RdbStatusRprt" runat="server" CssClass="LblEtiquet" Text="&nbsp Status Report" GroupName="D" />
            <br />
            <asp:Button ID="BtnExportar" runat="server" Text="Exportar Excel" OnClick="BtnExportar_Click" />
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Button ID="BtnImportarV1" runat="server" Text="Importar" OnClick="BtnImportarV1_Click" />

            <asp:Button ID="BtnExportar2" runat="server" Text="Cursor" OnClick="BtnExportar2_Click" />
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
