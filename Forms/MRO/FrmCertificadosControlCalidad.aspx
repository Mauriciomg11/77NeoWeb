<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmCertificadosControlCalidad.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmCertificadosControlCalidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 60%;
            margin-left: -30%;
            height: 85%;
            padding: 5px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .CentrarContenedor {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -49%;
            /*determinamos una altura*/
            height: 90%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }
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
        } function myFuncionddl() {
          <%--  $('#<%=DdlBodega.ClientID%>').chosen();
            $('#<%=DdlCliente.ClientID%>').chosen();--%>
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MlVw" runat="server">
                <asp:View ID="Vw0General" runat="server">
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnConsultar_Click1" OnClientClick="target ='';" Text="consultar" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblNumOT" runat="server" CssClass="LblEtiquet" Text="OT Nro.:" />
                                <asp:TextBox ID="TxtNumOT" runat="server" CssClass=" heightCampo" Enabled="false" Width="30%" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div id="Datos Aeronave" class="col-sm-6">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="LblTitDatosHK" runat="server" Text="Dat HK" />
                                        </h6>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblMatr" runat="server" CssClass="LblEtiquet" Text="hk" />
                                        <asp:TextBox ID="TxtMatr" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblSnHK" runat="server" CssClass="LblEtiquet" Text="sre" />
                                        <asp:TextBox ID="TxtSnHK" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblModelo" runat="server" CssClass="LblEtiquet" Text="mode" />
                                        <asp:TextBox ID="TxtModelo" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                </div>
                            </div>
                            <div id="Datos Elementos" class="col-sm-6">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="Label1" runat="server" Text="Dat Elem" />
                                        </h6>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblPnElem" runat="server" CssClass="LblEtiquet" Text="Pn" />
                                        <asp:TextBox ID="TxtPnElem" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblSnElem" runat="server" CssClass="LblEtiquet" Text="sre" />
                                        <asp:TextBox ID="TxtSnElem" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblDescElem" runat="server" CssClass="LblEtiquet" Text="descr" />
                                        <asp:TextBox ID="TxtDescElem" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" TextMode="MultiLine" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-1">
                                  <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnConsultar_Click1" OnClientClick="target ='';" Text="consultar" />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Busq" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOpcBusq" runat="server" Text="Opciones de búsqueda" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                    <div class="CentrarBusq DivMarco">
                        <table class="TablaBusqueda">
                            <tr>
                                <td colspan="3">
                                    <asp:RadioButton ID="RdbBusqNumOT" runat="server" CssClass="LblEtiquet" Text="&nbsp ot" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqSN" runat="server" CssClass="LblEtiquet" Text="&nbsp S/N:" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N:" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqHK" runat="server" CssClass="LblEtiquet" Text="&nbsp hk" GroupName="Busq" />
                                &nbsp&nbsp&nbsp                                   
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                                <td>
                                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                                <td>
                                    <asp:ImageButton ID="IbtBusqueda" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtBusqueda_Click" /></td>
                            </tr>
                        </table>
                        <br />
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="SNHK, NomModelo, DescrElem"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowCommand="GrdBusq_RowCommand" OnRowDataBound="GrdBusq_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="UplAbrir" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="IbtIr" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Ir" ToolTip="Ir" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="IbtIr" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OT">
                                        <ItemTemplate>
                                            <asp:Label ID="LblOT" Text='<%# Eval("OT") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aplica">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Aplicabilidad") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sn">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSnElem" Text='<%# Eval("SN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPnElem" Text='<%# Eval("PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CodAk">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCodHk" Text='<%# Eval("CodAeronave") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hk">
                                        <ItemTemplate>
                                            <asp:Label ID="LblHk" Text='<%# Eval("Matricula") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fec">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaOT") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="DdlBodega" EventName="TextChanged" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
