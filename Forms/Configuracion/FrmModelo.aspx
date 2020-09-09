<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmModelo.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.FrmModelo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Modelos</title>
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
        .DivGrid {
            position: absolute;
            width: 75%;
            height: 380px;
            top: 45%;
            left: 10%;
            margin-top: 0px;
        }

        .TablaCampos {
            margin: 0 auto;
            text-align: left;
            top: 50px;
        }

        .TabFormL {
            margin: 0 auto;
            text-align: left;
            top: 160px;
        }

        .TabBusq {
            top: 32%;
            left: 2%;
        }


        .Campos {
            Height: 28px;
            Width: 250px;
        }

        .BtnFrmL {
            background-image: url("../Images/formulaV1.png");
            /*background-size: cover;*/
            width: 32px;
            height: 33px;
            left: 95%;
            top: 1%;
        }
        .BtnSveFrml
        {
            background-image: url("../Images/formulaV1.png");
            /*background-size: cover;*/
            width: 32px;
            height: 33px;
            left: 95%;
            top: 1%;
        }

        .TabBtnEdicion {
            position: absolute;
            top: 39%;
            left: 10%;
            width: 20%;
        }

        .DivPnlFrml {
            position: absolute;
            background-color: blue;
        }

        .PneleditarPerfil {
            position: absolute;
            width: 50%;
            height: 33%;
            left: 22%;
            top: 37%;
        }

        .TabBtiEditFrml {
            position: absolute;
            top: 78%;
            left: 45%;
            width: 15%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1><asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PnlDatos" runat="server">
                <div class="CentrarTable">
                    <table class="TablaBusqueda TabBusq">
                        <tr>
                            <td>
                                <asp:Label ID="LblBusq" runat="server" Text="Busqueda: " CssClass="LblTextoBusq"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                            <td>
                                <asp:ImageButton ID="BtIConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="BtIConsultar_Click" /></td>
                        </tr>
                    </table>
                    <table class="TablaCampos">
                        <tr>
                            <td>
                                <asp:Label ID="LblCodigo" runat="server" Text="Codigo: " CssClass="LblTextoBusq"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control Campos" MaxLength="3" Enabled="false"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="LblMod" runat="server" Text="Modelo: " CssClass="LblTextoBusq"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtMod" runat="server" CssClass="form-control Campos" MaxLength="30" Enabled="false"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="LblDesc" runat="server" Text="Descripción: " CssClass="LblTextoBusq"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control Campos" MaxLength="200" TextMode="MultiLine" Enabled="false"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblNumMot" runat="server" Text="Nro de motores: " CssClass="LblTextoBusq"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtNumMot" runat="server" OnKeyPress="javascript:return solonumeros(event)" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="LblNumTr" runat="server" Text="Nro de tripulación: " CssClass="LblTextoBusq"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtNumTr" runat="server" OnKeyPress="javascript:return solonumeros(event)" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="LblPasj" runat="server" Text="Nro de pasajeros: " CssClass="LblTextoBusq"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtPasj" runat="server" OnKeyPress="javascript:return solonumeros(event)" CssClass="form-control Campos" Enabled="false"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblFormL" runat="server" Text="Fórmula de levantes: " CssClass="LblTextoBusq"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="TxtFormL" runat="server" Width="350px" Height="28px" CssClass="form-control" placeholder="Sin formula" Enabled="false"></asp:TextBox></td>
                            <td>
                                <asp:ImageButton ID="BtIFormL" runat="server" ToolTip="Editar fórmula" CssClass="BtnFrmL" OnClick="BtIFormL_Click1" ImageUrl="~/images/FormulaV2.png" Enabled="false" /></td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblAlaF" runat="server" CssClass="LblEtiquet" Text="Ala Fija"></asp:Label></td>
                                        <td>
                                            <asp:RadioButton ID="RdbAlaF" runat="server" TextAlign="Left" GroupName="Ala" Enabled="false" Checked="true" /></td>
                                        <td>
                                            <asp:Label ID="LblAlaR" runat="server" CssClass="LblEtiquet" Text="Ala Rotatoria"></asp:Label></td>
                                        <td>
                                            <asp:RadioButton ID="RdbAlaRo" runat="server" TextAlign="Left" GroupName="Ala" Enabled="false" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                   
                    <table class="TabBtnEdicion">
                        <tr>
                            <td>
                                <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnIngresar_Click" Text="Ingresar" /></td>
                            <td>
                                <asp:Button ID="BtnModificar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnModificar_Click" Text="Modificar" /></td>
                            <td>
                                <asp:Button ID="BtnEliminar" runat="server" CssClass=" btn btn-success botones BtnEdicion" OnClick="BtnEliminar_Click" Text="Eliminar" /></td>
                        </tr>
                    </table>
                </div>
                <div class="DivGrid DivContendorGrid">
                    <asp:GridView ID="GrdDatos" runat="server" DataKeyNames="CodModelo" EmptyDataText="No existen registros ..!"
                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="8" OnSelectedIndexChanged="GrdDatos_SelectedIndexChanged">
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <Columns>
                            <asp:CommandField SelectText="Activar" ShowSelectButton="True" />
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID="PnlFrml" runat="server" Visible="false">
                <asp:Panel ID="PnlEditarPerfil" runat="server" CssClass="PneleditarPerfil" BorderStyle="Solid" BorderColor="#3399ff" BackColor="#66ccff">
                    <asp:Label ID="Label1" runat="server" Text="Formula" CssClass="LblTextoBusq" Font-Size="X-Large"></asp:Label>
                    <asp:TextBox ID="TxtNewFml" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="BtnPA" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="BtnPA_Click" Text="(" /></td>
                            <td>
                                <asp:Button ID="BtnPC" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="BtnPC_Click" Text=")" /></td>
                            <td>
                                <asp:Button ID="BtnMas" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="BtnMas_Click" Text="+" /></td>
                            <td>
                                <asp:Button ID="BtnMenos" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="BtnMenos_Click" Text="-" /></td>
                            <td>
                                <asp:Button ID="BtnPor" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="BtnPor_Click" Text="*" /></td>
                            <td>
                                <asp:Button ID="BtnDiv" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="BtnDiv_Click" Text="/" /></td>
                            <td>
                                <asp:Button ID="BtnCiclo" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="BtnCiclo_Click" Text="C" ToolTip="Ciclos" /></td>
                            <td>
                                <asp:Button ID="BtnLevant" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="BtnLevant_Click" Text="L" ToolTip="Levantes" /></td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="Btn1" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn1_Click" Text="1" /></td>
                            <td>
                                <asp:Button ID="Btn2" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn2_Click" Text="2" /></td>
                            <td>
                                <asp:Button ID="Btn3" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn3_Click" Text="3" /></td>
                            <td>
                                <asp:Button ID="Btn4" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn4_Click" Text="4" /></td>
                            <td>
                                <asp:Button ID="Btn5" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn5_Click" Text="5" /></td>
                            <td>
                                <asp:Button ID="Btn6" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn6_Click" Text="6" /></td>
                            <td>
                                <asp:Button ID="Btn7" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn7_Click" Text="7" /></td>
                            <td>
                                <asp:Button ID="Btn8" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn8_Click" Text="8" /></td>
                            <td>
                                <asp:Button ID="Btn9" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn9_Click" Text="9" /></td>
                            <td>
                                <asp:Button ID="Btn0" runat="server" CssClass=" btn btn-success BtnSignosFrml" OnClick="Btn0_Click" Text="0" /></td>
                            <td>
                                <asp:Button ID="BtnLimp" runat="server" CssClass=" btn-dark BtnSignosFrml" OnClick="BtnLimp_Click" Text="Limpiar" Width="80px" /></td>
                        </tr>
                    </table>
                    <table class="TabBtiEditFrml">
                        <tr>
                            <td>
                                <asp:ImageButton ID="BtiAceptar" runat="server" CssClass="BtnAceptar" ImageUrl="~/images/Save.png" ToolTip="Editar" OnClick="BtiAceptar_Click" /></td>
                            <td>
                                <asp:ImageButton ID="BtiCancelar" runat="server" CssClass="BtnCancelar" ImageUrl="~/images/Cancel.png"  ToolTip="Cancelar" OnClick="BtiCancelar_Click" /></td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
