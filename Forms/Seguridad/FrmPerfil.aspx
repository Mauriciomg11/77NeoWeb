﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPerfil.aspx.cs" Inherits="_77NeoWeb.Forms.Seguridad.FrmPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Perfiles</title>
    <style type="text/css">
        .centrarDivPpal {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            top: 2px;
            left: 30%;
            /*determinamos una anchura*/
            width: 37%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: 2px;
            /*determinamos una altura*/
            height: 100%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            margin-top: 2px;
            border: 1px solid #808080;
            padding: 5px;
            background-color: rgba(0, 0, 0, 0.5);
            color: #000;
        }


        .DivGridUS {
            position: absolute;
            width: 30%;
            height: 600px;
            top: 46%;
            left: 40%;
            margin-top: 0px;
            overflow: scroll;
        }

        .DivGridPerfilAsig {
            position: absolute;
            width: 35%;
            height: 600px;
            top: 400px;
            left: 15%;
            margin-top: 0px;
            overflow: scroll;
        }

        .DivGridPerfilSinAsig {
            position: absolute;
            width: 35%;
            height: 600px;
            top: 400px;
            left: 55%;
            margin-top: 0px;
            overflow: scroll;
        }


        .TablaBusqueda2 {
            position: relative;
            text-align: center;
            left: 30%;
            width: 15%;
            height: 5%;
            top: 90px;
        }



        .PneleditarPerfil {
            /*position: absolute;*/
            width: 50%;
            height: 13%;
            left: 30%;
        }

        .OpcBusq {
            top: 52%
        }

        .OpcBusqUsu {
            top: 49%;
            left: 2%;
        }

        .CssList {
            position: relative;
            width: 90%;
            border: 2px solid black;
            border-color: cadetblue;
            left: 1px;
            top: 50px;
        }

        .DivGrid2 {
            position: absolute;
            width: 30%;
            height: auto;
            top: 46%;
            left: 5%;
            margin-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
            $('#<%=DdlGruposRP.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
    <%--<h1>Roles y perfiles</h1>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br /><br /><br />--%>

    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <br />
            <br />
            <br />
            <br />
            <%-- <div class="CssList">--%>
            <asp:Label ID="LblBusq" runat="server" CssClass="colorTexto" Text="Grupo"></asp:Label><br />
            <asp:DropDownList ID="DdlGruposRP" runat="server" CssClass="form-control" Font-Size="Smaller" AutoPostBack="True" OnTextChanged="DdlGruposRP_TextChanged"></asp:DropDownList><br />
            <%--  </div>--%>
            <br />
            <asp:Panel ID="PnlRol" runat="server" Visible="true">
                <div class="CentrarContenedor DivMarco">
                    <div class="CentrarTable">
                        <div class="row">
                            <div class="col-sm-10">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBusUsu" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtBusqUsu" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                                        <td>
                                            <asp:ImageButton ID="IbnBusUsu" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbnBusUsu_Click" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-sm-2">
                                <asp:ImageButton ID="IbtIr" runat="server" CssClass="BotonIr" ImageUrl="~/images/FlechaIrV1.png" ToolTip="Ir a perfiles" OnClick="IbtIr_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4">
                                <%--<div class="DivGrid2 DivContendorGrid">--%>
                                <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodIdUsrGrupo"
                                    CellPadding="3" CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="10" OnPageIndexChanging="GrdDatos_PageIndexChanging"
                                    OnRowCommand="GrdDatos_RowCommand" OnSelectedIndexChanged="GrdDatos_SelectedIndexChanged">
                                    <FooterStyle CssClass="GridFooterStyle" />
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    <Columns>
                                        <asp:CommandField HeaderText="Retirar" SelectText="Retirar" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario  Asignados" />
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                </asp:GridView>
                                <%--</div>--%>
                            </div>
                            <div class="col-sm-4">
                                <%--<div class="DivGridUS DivContendorGrid">--%>
                                <asp:GridView ID="GrdDatosUsin" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodUsuario"
                                    CellPadding="3" CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="10" OnPageIndexChanging="GrdDatosUsin_PageIndexChanging"
                                    OnRowCommand="GrdDatosUsin_RowCommand" OnSelectedIndexChanged="GrdDatosUsin_SelectedIndexChanged">
                                    <FooterStyle CssClass="GridFooterStyle" />
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    <Columns>
                                        <asp:CommandField HeaderText="Asignar" SelectText="Asignar" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                        <asp:BoundField DataField="Usuario" HeaderText="Usuario por Asignar" />
                                    </Columns>
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                </asp:GridView>
                            </div>
                            <%-- </div>--%>
                        </div>
                    </div>
                </div>
                <%--  <asp:ImageButton ID="IbtIr" runat="server" CssClass="BotonIr" ImageUrl="~/images/FlechaIrV1.png" ToolTip="Ir a perfiles" OnClick="IbtIr_Click" /> --%>
            </asp:Panel>
            <asp:Panel ID="PnlPerfil" runat="server" Visible="false">
                <div class="CentrarContenedor DivMarco">
                    <div class="CentrarTable">
                        <div class="row">
                            <div class="col-sm-10">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                                        <td>
                                            <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-sm-2">
                                <asp:ImageButton ID="IbtRegresar" runat="server" CssClass="BotonVolver" ImageUrl="~/images/FlechaRegresarV1.png" ToolTip="Regresar a roles" OnClick="IbtRegresar_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-10">
                                <asp:Panel ID="PnlEditarPerfil" runat="server" CssClass="PneleditarPerfil" BorderStyle="Solid" BorderColor="#3399ff" BackColor="#66ccff" Height="70px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblNombrePantalla" runat="server" Text="Label"></asp:Label></td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CkbIng" runat="server" Text="Ingresar" Font-Size="Smaller" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbMod" runat="server" Text="Modificar" Font-Size="Smaller" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbCons" runat="server" Text="Consultar" Font-Size="Smaller" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbImpr" runat="server" Text="Imprimir" Font-Size="Smaller" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbElim" runat="server" Text="Eliminar" Font-Size="Smaller" Visible="false" /></td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="CkbCE1" runat="server" Text="CE1" Font-Size="XX-Small" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbCE2" runat="server" Text="CE2" Font-Size="XX-Small" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbCE3" runat="server" Text="CE3" Font-Size="XX-Small" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbCE4" runat="server" Text="CE4" Font-Size="XX-Small" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbCE5" runat="server" Text="CE5" Font-Size="XX-Small" Visible="false" /></td>
                                            <td>
                                                <asp:CheckBox ID="CkbCE6" runat="server" Text="CE6" Font-Size="XX-Small" Visible="false" /></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div class="col-sm-1">
                                <asp:ImageButton ID="IbtAsignarPerfil" runat="server" CssClass="BtnAsingarPerfil" ImageUrl="~/images/Save.png" ToolTip="Asiganar" OnClick="IbtAsignarPerfil_Click" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>



                <div class="DivGridPerfilAsig">

                    <asp:GridView ID="GrdPerfilAsig" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodidUsrPerfil"
                        CellPadding="3" CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both"
                        OnRowCommand="GrdPerfilAsig_RowCommand" OnSelectedIndexChanged="GrdPerfilAsig_SelectedIndexChanged" OnRowDeleting="GrdPerfilAsig_RowDeleting"
                        OnRowDataBound="GrdPerfilAsig_RowDataBound">
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <Columns>
                            <asp:CommandField HeaderText="Ver" SelectText="permisos" ShowSelectButton="True" HeaderStyle-Width="33px" />
                            <asp:CommandField ShowDeleteButton="true" ButtonType="Image" DeleteImageUrl="~/images/deleteV3.png"
                                ControlStyle-Width="10px"></asp:CommandField>
                            <asp:BoundField DataField="DescSangria" HeaderText="Pantallas Asignadas" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="NomFormWeb" HeaderText="NF" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="DivGridPerfilSinAsig">
                    <asp:GridView ID="GrdSinAsig" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodIdFormulario"
                        CellPadding="3" CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnPageIndexChanging="GrdSinAsig_PageIndexChanging"
                        OnRowCommand="GrdSinAsig_RowCommand" OnRowDataBound="GrdSinAsig_RowDataBound">
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <Columns>
                            <asp:CommandField HeaderText="Selección" SelectText="Seleccionar" ShowSelectButton="True" HeaderStyle-Width="38px" />
                            <asp:BoundField DataField="DescSangria" HeaderText="Pantalla" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="NomFormWeb" HeaderText="NF" Visible="false" />
                        </Columns>
                        <%--<PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />--%>
                    </asp:GridView>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DdlGruposRP" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <%--   </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
