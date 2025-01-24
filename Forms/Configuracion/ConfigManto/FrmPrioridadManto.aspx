<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPrioridadManto.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.ConfigManto.FrmPrioridadManto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       /* .DivGrid {
            position: absolute;
            width: 80%;
            height: 600px;
            top: 15%;
            left: 10%;
            margin-top: 0px;
        }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>

            <div class="CentrarTable">
                <div class="DivGrid DivContendorGrid">
                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodPrioridadSolicitudMat"
                        CssClass="DiseñoGrid table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                        OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit" OnRowDataBound="GrdDatos_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Cód" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("NombrePSM") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label Text='<%# Eval("NombrePSM") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripc" HeaderStyle-Width="40%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDesc" Text='<%# Eval("Descripcion") %>' runat="server" MaxLength="50" Width="100%" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Crite" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("CriterioDias") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtCritD" Text='<%# Eval("CriterioDias") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DiaInicial" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("DiaInicial") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDI" Text='<%# Eval("DiaInicial") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DiaFinal" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("DiaFinal") %>' runat="server" Width="100%" TextMode="Number" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TxtDF" Text='<%# Eval("DiaFinal") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acti">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CkbActP" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="CkbAct" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="CkbActPP" runat="server" Checked="true" Enabled="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-Width="10%">
                                  <%--<ItemTemplate>
                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                  <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                    <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                </EditItemTemplate>--%>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
