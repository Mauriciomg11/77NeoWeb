<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="WebForm_IngresoMaestro_Detalle.aspx.cs" Inherits="_77NeoWeb.WebForm_IngresoMaestro_Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <script type="text/jscript">
        function solonumeros(e) {
            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }
            if (key < 48 || key > 57) {
                return false;
            }

            return true;
        }
        function Decimal(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode == 46) {
                var inputValue = $("#inputfield").val()
                if (inputValue.indexOf('.') < 1) {
                    return true;
                }
                return false;
            }
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="BtnConsult" runat="server" Text="Consultar" ToolTip="Consultar" OnClick="BtnConsult_Click" />
            <asp:Button ID="BtnHabilitar" runat="server" Text="Habilitar" ToolTip="Habilitar" OnClick="BtnHabilitar_Click" />
            <asp:Button ID="BtnNuevo" runat="server" Text="Nuevo" ToolTip="Nuevo" OnClick="BtnNuevo_Click" Enabled="false" />
            <asp:Button ID="BtnEdit" runat="server" Text="Editar" ToolTip="Editar" OnClick="BtnEdit_Click" Enabled="false" /><br /><br />
            <asp:Label ID="Label3" runat="server" Text="Id"></asp:Label>
            <asp:TextBox ID="TxtId" runat="server" onkeypress="return Decimal(event);"></asp:TextBox><br />
            <asp:Label ID="Label1" runat="server" Text="Madre"></asp:Label>
            <asp:TextBox ID="TxtMadre" runat="server" Enabled="false" Width="286px"/><br />
            <asp:Label ID="Label2" runat="server" Text="Papa" />
            <asp:TextBox ID="txtPadre" runat="server" Enabled="false" Width="295px" /><br />
            <asp:GridView ID="GrdHijo" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdDet"
                CssClass="DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                OnRowCommand="GrdHijo_RowCommand" OnRowDeleting="GrdHijo_RowDeleting" OnRowDataBound="GrdHijo_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Hijo">
                        <ItemTemplate>
                            <asp:TextBox ID="TxtNomHijoP" Text='<%# Eval("NomHijos") %>' runat="server" MaxLength="200" Width="100%" Enabled="false" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TxtNomHijoPP" runat="server" MaxLength="200" Width="100%" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Edad">
                        <ItemTemplate>
                            <asp:TextBox ID="TxtEdadP" Text='<%# Eval("Edad") %>' runat="server" MaxLength="200" Width="100%" Enabled="false" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TxtEdadPP" runat="server" MaxLength="200" Width="100%" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-Width="10%">
                        <ItemTemplate>                         
                            <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                        </ItemTemplate>                       
                        <FooterTemplate>
                            <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle CssClass="GridFooterStyle" />
                <HeaderStyle CssClass="GridCabecera" />
                <RowStyle CssClass="GridRowStyle" />
                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
            </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
