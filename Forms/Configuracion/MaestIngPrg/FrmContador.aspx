﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmContador.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.MaestIngPrg.FrmContador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Contador</title>
    <style type="text/css">
        .TablaCampos {
            margin: 0 auto;
            text-align: left;
            width: 70%;
        }

        .Campos {
            Height: 30px;
            Width: 100%;
        }

        .TabBtnEdicion {
            margin: 0 auto;
            text-align: left;
            top: 39%;
            width: 20%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
                $('#<%=DdlBuscar.ClientID%>, #<%=DdlUndMed.ClientID%>, #<%=DdlIdent.ClientID%>').chosen();
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <div class="CentrarTable">
                <table class="TablaBusqueda">
                    <tr>
                        <td>
                            <asp:Label ID="LblConsultar" runat="server" Text="Contador: " CssClass="LblTextoBusq"></asp:Label></td>
                        <td width="70%">
                            <asp:DropDownList ID="DdlBuscar" runat="server" CssClass="form-control DdlBusqueda" Height="30px" Font-Size="Smaller" AutoPostBack="True" OnTextChanged="DdlBuscar_TextChanged"></asp:DropDownList>
                        <td>
                            <asp:ImageButton ID="IbtExpExcel" runat="server" ToolTip="Exportar" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpExcel_Click" /></td>
                    </tr>
                </table>
            </div>
            <br /><br /><br />
            <div class="CentrarTable">
                <table class="TablaCampos table table-sm">
                    <tr>
                        <td>
                            <asp:Label ID="LblCodigo" runat="server" CssClass="LblTextoBusq" Text="Código:" /></td>
                        <td>
                            <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control Campos" MaxLength="3" Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:Label ID="LblDescrip" runat="server" CssClass="LblTextoBusq" Text="Descripción:" /></td>
                        <td>
                            <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control Campos" MaxLength="60" TextMode="MultiLine" Enabled="false"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="LblUndMed" runat="server" CssClass="LblTextoBusq" Text="Unidad medida:" /></td>
                        <td>
                            <asp:DropDownList ID="DdlUndMed" runat="server" CssClass="form-control Campos" Height="30px" Font-Size="10px" Enabled="false"></asp:DropDownList></td>
                        <td>
                            <asp:Label ID="LblIdentif" runat="server" CssClass="LblTextoBusq" Text="Identificador:" /></td>
                        <td>
                            <asp:DropDownList ID="DdlIdent" runat="server" CssClass="form-control Campos" Height="30px" Font-Size="10px" Enabled="false"></asp:DropDownList></td>
                        <td width="5%"></td>
                        <td>
                            <asp:CheckBox ID="CkReset" runat="server" CssClass="LblTextoBusq" Text=" Reseteable" Enabled="false" /></td>
                    </tr>
                </table>
                <table class="TabBtnEdicion">
                    <tr>
                        <td>
                            <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnIngresar_Click" Text="Ingresar" /></td>
                        <td>
                            <asp:Button ID="BtnModificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnModificar_Click" Text="Modificar" /></td>
                        <td>
                            <asp:Button ID="BtnEliminar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnEliminar_Click" Text="Eliminar" OnClientClick="return confirm('¿Desea eliminar el registro?');" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbtExpExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
