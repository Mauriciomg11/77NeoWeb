﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmPlantillaMaestra.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.MaestIngPrg.FrmPlantillaMaestra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Plantilla_Maestra</title>
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
    <style type="text/css">
        .DivGrid1 {
            position: absolute;
            width: 32%;
            height: 300px;
            left: 23%;
            margin-top: 0px;
            top: 210px;
        }


        .DivGridUltN {
            position: absolute;
            width: 40%;
            height: 300px;
            top: 210px;
            left: 56%;
            margin-top: 0px;
        }

        .DivGridPsc {
            position: relative;
            width: 30%;
            height: 200px;
            top: 84%;
            left: 56%;
            margin-top: 0px;
        }

        .DivGridPn {
            position: relative;
            width: 54%;
            height: 200px;
            top: 84%;
            left: 1%;
            margin-top: 0px;
        }

        .CsListBox {
            position: relative;
            Height: 200px;
        }

        .ListCap {
            position: absolute;
            top: 210px;
            left: 1%;
            Width: 21%;
            Height: 300px;
        }

        .TablaFlota {
            position: relative;
            left: 40%;
            width: 40%;
            top: 60px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/jscript">
        function myFuncionddl() {
            $('#<%=DdlFlota.ClientID%>').chosen();
            $('[id*=DdlPscPP],[id*=DdlPnPP]').chosen();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CuerpoPagina" runat="server">

    <asp:UpdatePanel ID="UpPn2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="CentrarContenedor DivMarco">
                <table class="TablaFlota">
                    <tr>
                        <td>
                            <asp:Label ID="LblFlota" runat="server" Text="Modelo: " CssClass="LblTextoBusq" /></td>
                        <td>
                            <asp:DropDownList ID="DdlFlota" runat="server" CssClass="form-control" Width="100%" Height="30px" Font-Size="Smaller" AutoPostBack="true" OnTextChanged="DdlFlota_TextChanged" /></td>
                        <td>
                            <asp:ImageButton ID="IbtExpExcel" runat="server" ToolTip="Exportar" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpExcel_Click" /></td>
                    </tr>
                </table>
                <div class="row">
                    <div class="col-sm-3">
                        <asp:Label ID="LblNumMot" runat="server" Text="ATAS" CssClass="LblTextoBusq" /><%--LblATA --%>
                        <asp:ListBox ID="LstCapitulo" runat="server" CssClass="CsListBox" Font-Size="10px" OnSelectedIndexChanged="LstCapitulo_SelectedIndexChanged" AutoPostBack="True" />
                    </div>
                    <div class="col-sm-4">
                        <br />
                        <br />
                        <br />
                        <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdSubCapituloN3,SubCapitulo"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="3"
                            OnRowCommand="GrdDatos_RowCommand" OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit"
                            OnRowDeleting="GrdDatos_RowDeleting" OnPageIndexChanging="GrdDatos_PageIndexChanging" OnRowDataBound="GrdDatos_RowDataBound" OnSelectedIndexChanged="GrdDatos_SelectedIndexChanged">
                            <Columns>
                                <asp:TemplateField HeaderText="SubATA">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodSubCapituloN3") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtCodSubN3" Text='<%# Eval("CodSubCapituloN3") %>' runat="server" Enabled="false" Width="40px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtCodSubN3PP" runat="server" MaxLength="2" Width="40px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="200px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtDescPP" runat="server" Width="200px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                        <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                        </asp:GridView>
                    </div>
                    <div class="col-sm-5">
                        <br />
                        <br />
                        <br />
                        <asp:GridView ID="GrdUltNvl" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdSubCapituloN4,CodUltimoNivel,NumElement"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="3"
                            OnRowCommand="GrdUltNvl_RowCommand" OnRowEditing="GrdUltNvl_RowEditing" OnRowUpdating="GrdUltNvl_RowUpdating" OnRowCancelingEdit="GrdUltNvl_RowCancelingEdit"
                            OnRowDataBound="GrdUltNvl_RowDataBound" OnPageIndexChanging="GrdUltNvl_PageIndexChanging" OnSelectedIndexChanged="GrdUltNvl_SelectedIndexChanged"
                            OnRowDeleting="GrdUltNvl_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="Ubicación Técnica">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodUltimoNivel") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtCodSubN4" Text='<%# Eval("CodUltimoNivel") %>' runat="server" Enabled="false" Width="42px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtCodSubN4PP" runat="server" MaxLength="2" Width="42px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="200px" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtDescPP" runat="server" Width="200px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Número Element">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("NumElement") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtNumE" Text='<%# Eval("NumElement") %>' runat="server" Width="20px" OnKeyPress="javascript:return solonumeros(event)" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="TxtNumEPP" runat="server" Width="20px" OnKeyPress="javascript:return solonumeros(event)" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="13%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                        <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                    </FooterTemplate>
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
                <br />
                <div class="row">
                    <div class="col-sm-8">
                        <asp:GridView ID="GrdPn" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodigoIDPlantillaDetalle,CodReferencia"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both"
                            OnRowCommand="GrdPn_RowCommand" OnRowDeleting="GrdPn_RowDeleting" OnRowDataBound="GrdPn_RowDataBound">
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            <Columns>
                                <asp:TemplateField HeaderText="Parte principal">
                                    <ItemTemplate>
                                        <asp:TextBox ID="TxtPnE" Text='<%# Eval("PN") %>' runat="server" Width="350px" Enabled="false" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlPnPP" runat="server" Width="350px" Height="28px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="250px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-sm-4">
                        <asp:GridView ID="GrdPosicion" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodID, Codigo"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both"
                            OnRowCommand="GrdPosicion_RowCommand" OnRowDeleting="GrdPosicion_RowDeleting"
                            OnRowDataBound="GrdPosicion_RowDataBound">
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            <Columns>
                                <asp:TemplateField HeaderText="Ubicación">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodUbicacionFisica") %>' runat="server" Width="30px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Posición">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Posicion") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="DdlPscPP" runat="server" Width="150px" Height="28px" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="LstCapitulo" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="IbtExpExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
