<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPropuesta.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmPropuesta" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="RpVw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
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

        .CentrarContndSn {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -49%;
            /*determinamos una altura*/
            height: 85%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }

        .CentrarBoton {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 60%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -30%;
            /*determinamos una altura*/
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 60%;
            margin-left: -30%;
            height: 85%;
            padding: 5px;
        }

        .ScrollDet1 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 190px;
        }

        .ScrollDet2 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 90%;
        }

        .Font_btnAlert {
            font-size: 11px;
            font-stretch: semi-condensed;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .TextR {
            text-align: right;
        }

        .AnchoModal {
            width: 50%;
        }

        .heightSvc {
            height: 290px;
        }

        .heightPltll {
            height: 330px;
        }

        .AlinearRight {
            text-align: right;
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
        }
        function myFuncionddl() {
            $('#<%=DdlTipo.ClientID%>').chosen();
            $('#<%=DdlCliente.ClientID%>').chosen();
            $('#<%=DdlFormPag.ClientID%>').chosen();
            $('#<%=DdlPptSuper.ClientID%>').chosen();
            $('#<%=DdlMoned.ClientID%>').chosen();
            $('#<%=DdlEstado.ClientID%>').chosen();
            $('#<%=DdlTipoSol.ClientID%>').chosen();
            $('[id *=DdlAeronavePP]').chosen();
            $('[id *=DdlPNRFPP]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTituloModal" runat="server" Text="Mensaje" /></h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Label ID="LblTexMensjModl" runat="server" Text="Desea continuar?" />
                    </p>
                </div>
                <asp:UpdatePanel ID="UpPlMdl" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-footer">
                            <asp:Button ID="BtnSiModl" runat="server" CssClass="btn btn-default" Text="Sí" OnClick="BtnSiModl_Click" />
                            <asp:Button ID="BtnNoModl" runat="server" class="btn btn-default" Text="No" OnClick="BtnNoModl_Click" />
                            <%--data-dismiss="modal" --%>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnSiModl" />
                        <asp:PostBackTrigger ControlID="BtnNoModl" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>

    <div id="ModalBusqPN" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitModalBusqPN" runat="server" Text="P/N" /></h4>
                </div>
                <div class="modal-body">
                    <table class="TablaBusqueda">
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="RdbMOdalBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="BusqPn" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbMOdalBusqSN" runat="server" CssClass="LblEtiquet" Text="&nbsp  S/N" GroupName="BusqPn" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbMOdalBusqDesc" runat="server" CssClass="LblEtiquet" Text="&nbsp descripcion" GroupName="BusqPn" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblModalBusq" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                            <td>
                                <asp:TextBox ID="TxtModalBusq" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                            <td>
                                <asp:ImageButton ID="IbtModalBusq" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtModalBusq_Click" /></td>
                        </tr>
                    </table>
                    <div class="CentrarGrid pre-scrollable">
                        <asp:GridView ID="GrdModalBusqPN" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                            DataKeyNames="IdentificadorElemR,CodBodega,CodTercero,Cantidad,Bodega"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowCommand="GrdModalBusqPN_RowCommand" OnRowDataBound="GrdModalBusqPN_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="IbtIrPN" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="IrPN" ToolTip="Ir" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="P/N">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="S/N">
                                    <ItemTemplate>
                                        <asp:Label ID="LblSn" Text='<%# Eval("SN") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="descripcion">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnCloseModalBusqPN" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>

            </div>

        </div>
    </div>

    <div id="ModalAlerta" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitAlrt" runat="server" Text="Alertas" /></h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpPlAlert" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-5 DivMarco">
                                    <div class="CentrarGrid pre-scrollable">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="LblTitAlertaOTDuplicadas" runat="server" Text="OT duplicadas" /></h6>
                                        <asp:GridView ID="GrdAlrtOtDuplicada" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                            CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Propuesta">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("Propuesta") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ot">
                                                    <ItemTemplate>
                                                        <asp:Label Text='<%# Eval("OT") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridCabecera" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="col-sm-7 DivMarco">
                                    <div class="CentrarGrid pre-scrollable">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="LblTitAlertaSinDetAprob" runat="server" Text="Sin detelle aprobado" Visible="false" /></h6>
                                        <asp:GridView ID="GrdAlrtDetSinAprb" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" Visible="false" DataKeyNames="IdPropuesta"
                                            CssClass="GridControl DiseñoGrid table-sm" GridLines="Both" OnRowEditing="GrdAlrtDetSinAprb_RowEditing" OnRowUpdating="GrdAlrtDetSinAprb_RowUpdating"
                                            OnRowCancelingEdit="GrdAlrtDetSinAprb_RowCancelingEdit" OnRowDataBound="GrdAlrtDetSinAprb_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CkbAprobP" Checked='<%# Eval("OK").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="CkbAprob" Checked='<%# Eval("OK").ToString()=="1" ? true : false %>' runat="server" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Propuesta">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblIdPpt" Text='<%# Eval("IdPropuesta") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField FooterStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                                        <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="GridCabecera" />
                                            <RowStyle CssClass="GridRowStyle" />
                                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnCerrarAlerta" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>

            </div>

        </div>
    </div>

    <asp:MultiView ID="MultVw" runat="server">
        <asp:View ID="Vw0Datos" runat="server">
            <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-1">
                                <asp:Button ID="BtnNotfPCP" runat="server" CssClass="btn btn-outline-danger Font_btnAlert " Width="100%" OnClick="BtnNotfPCP_Click" Text="PCP" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnNotfLog" runat="server" CssClass="btn btn-outline-danger Font_btnAlert" Width="100%" OnClick="BtnNotfLog_Click" Text="logíistica" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnNotfAprob" runat="server" CssClass="btn btn-outline-danger Font_btnAlert" Width="100%" OnClick="BtnNotfAprob_Click" Text="aprobación" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnNotfCumpld" runat="server" CssClass="btn btn-outline-danger Font_btnAlert" Width="100%" OnClick="BtnNotfCumpld_Click" Text="cumplida" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnNotfCancel" runat="server" CssClass="btn btn-outline-danger Font_btnAlert" Width="100%" OnClick="BtnNotfCancel_Click" Text="cancelada" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnNotfDevolc" runat="server" CssClass="btn btn-outline-danger Font_btnAlert" Width="100%" OnClick="BtnNotfDevolc_Click" Text="devolucion" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnNotfNoAprob" runat="server" CssClass="btn btn-outline-danger Font_btnAlert" Width="100%" OnClick="BtnNotfNoAprob_Click" Text="No aprobada" />
                            </div>
                            &nbsp;&nbsp
                            <div class="col-sm-3">
                                <asp:Label ID="LblNumPpt" runat="server" CssClass="LblEtiquet" Text="Propuesta Nro.:" />
                                <asp:TextBox ID="TxtNumPpt" runat="server" CssClass=" heightCampo" Enabled="false" Width="30%" />
                                <asp:Label ID="LblMaster" runat="server" Text="MASTER" ForeColor="DarkRed" Font-Bold="true" />
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox ID="TxtFecha" runat="server" CssClass=" heightCampo" Enabled="false" Width="150%" />
                            </div>
                        </div>
                        <table>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                        <div class="row">
                            <div class="col-sm-1">
                                <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnConsultar_Click" OnClientClick="target ='';" Text="consultar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnIngresar_Click" OnClientClick="target ='';" Text="nuevo" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnModificar_Click" OnClientClick="target ='';" Text="modificar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnEliminar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnEliminar_Click" OnClientClick="target ='';" Text="eliminar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnEditCondic" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnEditCondic_Click" Text="condiciones" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnDetalle" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnDetalle_Click" OnClientClick="target ='';" Text="Trabajos" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnImprimir" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnImprimir_Click" OnClientClick="target ='';" Text="imprimir" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnExportPPT" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExportPPT_Click" OnClientClick="target ='';" Text="Exportar propuesta" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnExportDet" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExportDet_Click" OnClientClick="target ='';" Text="Exportar detalle" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnAux" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnAux_Click" OnClientClick="target ='_blank';" Text="Auxiliares" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text=" tipo" />
                                <asp:DropDownList ID="DdlTipo" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlTipo_TextChanged" AutoPostBack="true" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblCliente" runat="server" CssClass="LblEtiquet" Text="cliente" />
                                <asp:DropDownList ID="DdlCliente" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlCliente_TextChanged" AutoPostBack="true" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblFormPag" runat="server" CssClass="LblEtiquet" Text="forma de pago" />
                                <asp:DropDownList ID="DdlFormPag" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblPptSuper" runat="server" CssClass="LblEtiquet" Text="Propuesta principal" />
                                <asp:DropDownList ID="DdlPptSuper" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LbPptComerc" runat="server" CssClass="LblEtiquet" Text="propuesta comercial" />
                                <asp:TextBox ID="TxtPptComerc" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="50" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblNumContrat" runat="server" CssClass="LblEtiquet" Text="contrato Nro." />
                                <asp:TextBox ID="TxtNumContrat" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="50" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblMoned" runat="server" CssClass="LblEtiquet" Text="Moneda" />
                                <asp:DropDownList ID="DdlMoned" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechTRM" runat="server" CssClass="LblEtiquet" Text="fecha TRM" />
                                <asp:TextBox ID="TxtFechTRM" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="8" Enabled="false" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblValorTrm" runat="server" CssClass="LblEtiquet" Text="valor TRM" />
                                <asp:TextBox ID="TxtValorTrm" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Number" step="1" onkeypress="return solonumeros(event);" Text="0" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblEstado" runat="server" CssClass="LblEtiquet" Text="estado" /><br />
                                <asp:DropDownList ID="DdlEstado" runat="server" CssClass="heightCampo" Width="90%" Enabled="false" />
                                <asp:ImageButton ID="IbtReturnEstado" runat="server" ImageUrl="~/images/RegresarV6.png" ImageAlign="AbsBottom" Height="22px" Width="22px" OnClick="IbtReturnEstado_Click" /><%-- --%>
                                <asp:ImageButton ID="IbtActualizarEstado" runat="server" ImageUrl="~/images/Save.png" ImageAlign="AbsBottom" Height="22px" Width="22px" Visible="false" OnClick="IbtActualizarEstado_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechAprob" runat="server" CssClass="LblEtiquet" Text="fecha aprobacion" />
                                <asp:TextBox ID="TxtFechAprob" runat="server" CssClass="form-control-sm heightCampo" Width="75%" TextMode="Date" MaxLength="8" Enabled="false" />
                                <asp:ImageButton ID="IbtDesaprobar" runat="server" ImageUrl="~/images/RegresarV6.png" ImageAlign="AbsBottom" Height="22px" Width="22px" OnClick="IbtDesaprobar_Click" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechEntreg" runat="server" CssClass="LblEtiquet" Text="fecha entrega" />
                                <asp:TextBox ID="TxtFechEntreg" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="8" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechValidez" runat="server" CssClass="LblEtiquet" Text="fecha validez" />
                                <asp:TextBox ID="TxtFechValidez" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="8" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechEntregTrab" runat="server" CssClass="LblEtiquet" Text="fecha entrega trabajo" />
                                <asp:TextBox ID="TxtFechEntregTrab" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="8" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTipoSol" runat="server" CssClass="LblEtiquet" Text="Tipo solicitud" />
                                <asp:DropDownList ID="DdlTipoSol" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <br />
                                <asp:RadioButton ID="RdbSinDanOcul" runat="server" CssClass="LblEtiquet" Text="&nbsp Sin daño " GroupName="DO" Enabled="false" />
                                <asp:RadioButton ID="RdbDanOcul" runat="server" CssClass="LblEtiquet" Text="&nbsp daño oculto" GroupName="DO" Enabled="false" />
                            </div>
                        </div>
                        <table>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                        <div class="row">
                            <div class="col-sm-5">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblSubTtl" runat="server" CssClass="LblEtiquet" Text="Sub Total:" /></td>
                                                <td>
                                                    <asp:TextBox ID="TxtSubTtl" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                                    <asp:TextBox ID="TxtSubTtlN" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Visible="false" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblImpuest" runat="server" CssClass="LblEtiquet" Text="Impuesto:" /></td>
                                                <td>
                                                    <asp:TextBox ID="TxtImpuest" runat="server" CssClass="form-control-sm heightCampo" Width="35%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                                    <asp:TextBox ID="TxtImpuestN" runat="server" CssClass="form-control-sm heightCampo TextR" Width="35%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Visible="false" />
                                                    <asp:TextBox ID="TxtVlrImpuest" runat="server" CssClass="form-control-sm heightCampo TextR" Width="62%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                                    <asp:TextBox ID="TxtVlrImpuestN" runat="server" CssClass="form-control-sm heightCampo TextR" Width="62%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblTotal" runat="server" CssClass="LblEtiquet" Text="Total:" Font-Bold="true" /></td>
                                                <td>
                                                    <asp:TextBox ID="TxtTotal" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Font-Bold="true" Enabled="false" />
                                                    <asp:TextBox ID="TxtTotalN" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" Visible="false" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="col-sm-6">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblAjusVent" runat="server" CssClass="LblEtiquet" Text="Ajuste Venta:" /></td>
                                                <td>
                                                    <asp:TextBox ID="TxtAjusVent" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                                    <asp:TextBox ID="TxtAjusVentN" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Visible="false" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="LblVlrRecurso" runat="server" CssClass="LblEtiquet" Text="Valor Recurso:" /></td>
                                                <td>
                                                    <asp:TextBox ID="TxtVlrRecurso" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                                    <asp:TextBox ID="TxtVlrRecursoN" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Visible="false" Enabled="false" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVlrMnObr" runat="server" CssClass="LblEtiquet" Text="Mano de Obra:" /></td>
                                                <td>
                                                    <asp:TextBox ID="TxtVlrMnObr" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                                    <asp:TextBox ID="TxtVlrMnObrN" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Visible="false" Enabled="false" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-7">
                                <div class="row">
                                    <div class="col-sm-9">
                                        <asp:Label ID="LblMotvAjust" runat="server" CssClass="LblEtiquet" Text="Motivo Ajuste:" />&nbsp
                                        <asp:TextBox ID="TxtMotvAjust" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="65%" MaxLength="150" />
                                    </div>
                                    <div class="col-sm-3">

                                        <asp:CheckBox ID="CkbAplicImpuesto" runat="server" CssClass="LblEtiquet" Text="Aplica impuesto" Enabled="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblObserv" runat="server" CssClass="LblEtiquet" Text="Observaciones:" />
                                        <asp:TextBox ID="TxtObserv" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="85%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:Label ID="LblGarant" runat="server" CssClass="LblEtiquet" Text="Garantia:" />
                                        <asp:TextBox ID="TxtGarant" runat="server" CssClass="form-control-sm heightCampo" Width="45%" TextMode="Number" step="0" onkeypress="return solonumeros(event);" Text="0" Enabled="false" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblGanacInter" runat="server" CssClass="LblEtiquet" Text="Ganancia Internacional:" />
                                        <asp:TextBox ID="TxtGanacInter" runat="server" CssClass="form-control-sm heightCampo" Width="30%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Text="0" Enabled="false" />
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:Label ID="LblGanacNacional" runat="server" CssClass="LblEtiquet" Text="Ganancia Nacional:" />
                                        <asp:TextBox ID="TxtGanacNacional" runat="server" CssClass="form-control-sm heightCampo" Width="30%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Text="0" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitDetalleGrl" runat="server" Text="Detalle general" /></h6>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="IbtAprDet1All" runat="server" ImageUrl="~/images/Check1.png" ImageAlign="AbsBottom" Height="25px" Width="25px" OnClick="IbtAprDet1All_Click" Visible="false" /></td>
                                        <td>
                                            <asp:ImageButton ID="IbtDesAprDet1All" runat="server" ImageUrl="~/images/UnCheck.png" ImageAlign="AbsBottom" Height="25px" Width="25px" OnClick="IbtDesAprDet1All_Click" Visible="false" /></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:RadioButton ID="RdbDet1BuqAll" runat="server" CssClass="LblEtiquet" Text="&nbsp Todos" GroupName="Det1" />
                                            <asp:RadioButton ID="RdbDet1BuqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="Det1" />
                                            <asp:RadioButton ID="RdbDet1BuqOT" runat="server" CssClass="LblEtiquet" Text="&nbsp OT" GroupName="Det1" />
                                            <asp:RadioButton ID="RdbDet1BuqRte" runat="server" CssClass="LblEtiquet" Text="&nbsp Reporte" GroupName="Det1" />
                                            <asp:RadioButton ID="RdbDet1BuqSvc" runat="server" CssClass="LblEtiquet" Text="&nbsp Trabajo" GroupName="Det1" />
                                        </td>
                                        <td>&nbsp&nbsp&nbsp</td>
                                        <td>
                                            <asp:TextBox ID="TxtBusqDet1" runat="server" Width="350px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                                        <td>
                                            <asp:ImageButton ID="IbtConsultarDet1" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultarDet1_Click" /></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox ID="CkbAplicOT" runat="server" CssClass="LblEtiquet" Text="Desde OT" Enabled="false" Font-Size="18px" ForeColor="DarkRed" Font-Bold="true" />
                                        </td>
                                    </tr>
                                </table>
                                <div class="ScrollDet1">
                                    <asp:GridView ID="GrdDet1" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                        DataKeyNames="IdDetPropuesta,IdDetPropSrv,IdServicio,Reporte,Posicion"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="200%" EmptyDataText="No existen registros ..!"
                                        OnRowCommand="GrdDet1_RowCommand" OnRowEditing="GrdDet1_RowEditing" OnRowUpdating="GrdDet1_RowUpdating"
                                        OnRowCancelingEdit="GrdDet1_RowCancelingEdit" OnRowDeleting="GrdDet1_RowDeleting" OnRowDataBound="GrdDet1_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField FooterStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" Visible="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                                    <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:UpdatePanel ID="UplSvcMas" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtAddPlntll" Width="20px" Height="20px" ImageUrl="~/images/AddNewV3.png" runat="server" CommandName="AddPlantilla" ToolTip="agresar items desde plantilla" Visible="false" />
                                                            <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" Width="20px" Height="20px" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" Visible="false" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtAddNew" />
                                                            <asp:PostBackTrigger ControlID="IbtAddPlntll" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <%----%>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pos">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Posicion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblPos" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aprob">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbAprobP" Checked='<%# Eval("Aprobado").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="CkbAprob" Checked='<%# Eval("Aprobado").ToString()=="1" ? true : false %>' runat="server" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="CkbAprobPP" runat="server" Checked="false" Enabled="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtPN" Text='<%# Eval("PN") %>' runat="server" MaxLength="80" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="DdlPNRFPP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlPNRFPP_TextChanged" />
                                                    <asp:TextBox ID="TxtPNRFPP" runat="server" MaxLength="80" Width="100%" Enabled="false" Visible="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Referencia" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtDescPNPP" runat="server" MaxLength="80" Width="100%" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant. Sol">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CantidadSol") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtCantSol" Text='<%# Eval("CantidadSol") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtCantSolPP" runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Text="0" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant Real" FooterStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CantRealDP") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtCantReal" Text='<%# Eval("CantRealDP") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ValorUnd" FooterStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ValorUndT") %>' runat="server" Width="100%" CssClass="AlinearRight" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtVlrUnd" Text='<%# Eval("ValorUnd") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtVlrUndPP" runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Text="0" Enabled="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% utilidad" FooterStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("PorcentajeUtilidad") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtPorcUtld" Text='<%# Eval("PorcentajeUtilidad") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="costo Venta">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CostoVentaT") %>' runat="server" Width="100%" CssClass="AlinearRight" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblCostVnta" Text='<%# Eval("CostoVenta") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="unid Med">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("UnidadMedida") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Und Compra">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("UndCompraDPV") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CodMoneda">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodMoneda") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblMnd" Text='<%# Eval("CodMoneda") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ValorMonedaProp">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("VlrMonLocal") %>' runat="server" Width="100%" CssClass="AlinearRight" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblVlrMndPpt" Text='<%# Eval("ValorMonedaProp") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% impuesto">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("IVA") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Con Impuesto">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ValorConImpuestoT") %>' runat="server" Width="100%" CssClass="AlinearRight" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblVlrConImpt" Text='<%# Eval("ValorConImpuesto") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ValorTotalT") %>' runat="server" Width="100%" CssClass="AlinearRight" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblVlrTtl" Text='<%# Eval("ValorTotal") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tiempo Entrega Dias">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("TiempoEntregaDias") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtTiempEntD" Text='<%# Eval("TiempoEntregaDias") %>' runat="server" Width="100%" TextMode="Number" step="0" onkeypress="return solonumeros(event);" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Reporte">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Reporte") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OT">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("OT") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serv" HeaderStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("DescricionServicio") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnConsultar" />
                    <asp:PostBackTrigger ControlID="BtnEditCondic" />
                    <asp:PostBackTrigger ControlID="IbtReturnEstado" />
                    <asp:PostBackTrigger ControlID="IbtDesaprobar" />
                    <asp:PostBackTrigger ControlID="BtnNotfPCP" />
                    <asp:PostBackTrigger ControlID="BtnNotfLog" />
                    <asp:PostBackTrigger ControlID="BtnNotfAprob" />
                    <asp:PostBackTrigger ControlID="BtnNotfCumpld" />
                    <asp:PostBackTrigger ControlID="BtnNotfCancel" />
                    <asp:PostBackTrigger ControlID="BtnNotfDevolc" />
                    <asp:PostBackTrigger ControlID="BtnDetalle" />
                    <asp:PostBackTrigger ControlID="BtnEliminar" />
                    <asp:PostBackTrigger ControlID="BtnImprimir" />
                    <asp:PostBackTrigger ControlID="BtnExportPPT" />
                    <asp:PostBackTrigger ControlID="BtnExportDet" />
                    <asp:PostBackTrigger ControlID="BtnAux" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw1Busq" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOpcBusqueda" runat="server" Text="Opciones de búsqueda " />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                    <div class="CentrarBusq DivMarco">
                        <table class="TablaBusqueda">
                            <tr>
                                <td colspan="3">
                                    <asp:RadioButton ID="RdbBusqGnrlPpt" runat="server" CssClass="LblEtiquet" Text="&nbsp propuesta" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqGnrlHk" runat="server" CssClass="LblEtiquet" Text="&nbsp aeronave" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqGnrlSN" runat="server" CssClass="LblEtiquet" Text="&nbsp S/N" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqGnrlPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqGnrlOT" runat="server" CssClass="LblEtiquet" Text="&nbsp O.t." GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqGnrlRte" runat="server" CssClass="LblEtiquet" Text="&nbsp reporte" GroupName="Busq" /></td>
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
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="Codigo,CodCliente,CodTipoPropuesta,IdTercero"
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
                                    <asp:TemplateField HeaderText="Ppt">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPpt" Text='<%# Eval("Codigo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Matricula">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("MatricuaPr") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SnElemento">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("SnElemento") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RazonSocial">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("RazonSocial") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DescripcionEstado">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("DescripcionEstado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarBusq" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw2Condiciones" runat="server">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitCondiciones" runat="server" Text="parametrizacion condiciones de la propuesta " />
                    </h6>
                    <asp:ImageButton ID="IbtClseCondic" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtClseCondic_Click" />
                    <div class="CentrarBusq">
                        <div class="row">
                            <div class="col-sm-6 DivMarco">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCondTiempEntregPpt" runat="server" CssClass="LblEtiquet" Text="condicion tiempo entrega:" />
                                        <asp:TextBox ID="TxtCondTiempEntregPpt" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="100%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCondFormPagoPpt" runat="server" CssClass="LblEtiquet" Text="condicion forma de pago" />
                                        <asp:TextBox ID="TxtCondFormPagoPpt" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="100%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCondDanoOcultPpt" runat="server" CssClass="LblEtiquet" Text="condicion daño oculto" />
                                        <asp:TextBox ID="TxtCondDanoOcultPpt" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="100%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCondGarantPpt" runat="server" CssClass="LblEtiquet" Text="condicion Garantia" />
                                        <asp:TextBox ID="TxtCondGarantPpt" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="100%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row CentrarBoton ">
                                    <div class="col-sm-12">
                                        <asp:Button ID="BtnUpdateCondPpt" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnUpdateCondPpt_Click" Text="actualizar propuesta" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 DivMarco">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCondTiempEntreg" runat="server" CssClass="LblEtiquet" Text="condicion tiempo entrega:" />
                                        <asp:TextBox ID="TxtCondTiempEntreg" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="100%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCondFormPago" runat="server" CssClass="LblEtiquet" Text="condicion forma de pago" />
                                        <asp:TextBox ID="TxtCondFormPago" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="100%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCondDanoOcult" runat="server" CssClass="LblEtiquet" Text="condicion daño oculto" />
                                        <asp:TextBox ID="TxtCondDanoOcult" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="100%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Label ID="LblCondGarant" runat="server" CssClass="LblEtiquet" Text="condicion Garantia" />
                                        <asp:TextBox ID="TxtCondGarant" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Height="100%" MaxLength="250" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row CentrarBoton ">
                                    <div class="col-sm-12">
                                        <asp:Button ID="BtnUpdateCond" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnUpdateCond_Click" Text="actualizar Condiciones" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtClseCondic" />
                    <asp:PostBackTrigger ControlID="BtnUpdateCond" />
                    <asp:PostBackTrigger ControlID="BtnUpdateCondPpt" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw3ElementosNoValorizados" runat="server">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitEleNoValorizado" runat="server" Text="parte no encontradas en la valorización" />
                    </h6>
                    <asp:ImageButton ID="IbtClosePNoValorizado" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtClosePNoValorizado_Click" />
                    <div class="CentrarBusq DivMarco">
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdPnNoValorizado" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="Reporte">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPpt" Text='<%# Eval("Reporte") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OT">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("OT") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CodReferencia">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Pn") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FechaReserva">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaReserva") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fec_crea_PN">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Fec_crea_PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FechaNotificacion">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaNotificacion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FechaValorizado">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FechaValorizado") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtClosePNoValorizado" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw4Det2Elem_HK" runat="server">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitTrabajos" runat="server" Text="propuesta Nro:" /></h6>
                    <div class="CentrarContndSn DivMarco">
                        <asp:ImageButton ID="IbtClosDetElemHK" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtClosDetElemHK_Click" />
                        <div class="row">
                            <div id="Elementos_HK" class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitSNHK" runat="server" Text="elemento" /></h6>
                                <div class="ScrollDet2">
                                    <asp:GridView ID="GrdElementos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdDetPropHk"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%"
                                        OnRowCommand="GrdElementos_RowCommand" OnRowDeleting="GrdElementos_RowDeleting" OnRowDataBound="GrdElementos_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="filtro">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UplFilter" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtFilter" Width="25px" Height="25px" ImageUrl="~/images/FilterIn.png" runat="server" CommandName="Filter" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtFilter" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="IbtPN" Width="25px" Height="25px" ImageUrl="~/images/FindV3.png" runat="server" CommandName="FltrPN" ToolTip="buscar parte" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtPNPP" runat="server" MaxLength="80" Width="100%" Enabled="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("SnElemento") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtSNPP" runat="server" MaxLength="80" Width="100%" Enabled="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("DescripcionPN") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtDescPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CantidadDPHK") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtCantPP" runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" Text="0" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterStyle-Width="5%">
                                                <ItemTemplate>
                                                    <%--<asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />--%>
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
                                    </asp:GridView>
                                    <asp:GridView ID="GrdAeronave" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdDetPropHk,CodAeronave"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%" Visible="false"
                                        OnRowCommand="GrdAeronave_RowCommand" OnRowDeleting="GrdAeronave_RowDeleting" OnRowDataBound="GrdAeronave_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="filtro">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UplFilter" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtFilter3" Width="25px" Height="25px" ImageUrl="~/images/FilterIn.png" runat="server" CommandName="Filter" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtFilter3" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aeronave" HeaderStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblAeronave" Text='<%# Eval("Matricula") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="DdlAeronavePP" runat="server" Width="100%" Height="28px" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modelo">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodModelo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("DescripcionModelo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterStyle-Width="5%">
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
                                    </asp:GridView>
                                </div>
                            </div>
                            <div id="Servicios" class="col-sm-6 heightSvc">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitOt" runat="server" Text="trabajos" /></h6>
                                <div class="ScrollDet2">
                                    <asp:GridView ID="GrdServicios" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                        DataKeyNames="IdDetPropSrv,Ot,IdSvcManto, Pn,CodServicioManto"
                                        CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%"
                                        OnRowCommand="GrdServicios_RowCommand" OnRowEditing="GrdServicios_RowEditing" OnRowUpdating="GrdServicios_RowUpdating"
                                        OnRowCancelingEdit="GrdServicios_RowCancelingEdit" OnRowDeleting="GrdServicios_RowDeleting" OnRowDataBound="GrdServicios_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="filtro" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UplFilter" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtFilter2" Width="25px" Height="25px" ImageUrl="~/images/FilterIn.png" runat="server" CommandName="Filter" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtFilter2" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:UpdatePanel ID="UplSvcMas" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtAddMas" Width="30px" Height="30px" ImageUrl="~/images/AddNewV3.png" runat="server" CommandName="FltrSvcMas" ToolTip="agresar varios servicios" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtAddMas" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Apd" HeaderStyle-Width="1%">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="ChkAll" runat="server" Text="All" OnCheckedChanged="ChkAll_CheckedChanged1" AutoPostBack="true" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbAprobP" Checked='<%# Eval("AprobadoDPSM").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="CkbAprob" Checked='<%# Eval("AprobadoDPSM").ToString()=="1" ? true : false %>' runat="server" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="servicios" HeaderStyle-Width="55%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtDesSvcP" Text='<%# Eval("DescricionServicio") %>' runat="server" Width="100%" TextMode="MultiLine" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtDesSvc" Text='<%# Eval("DescricionServicio") %>' runat="server" Width="100%" TextMode="MultiLine" Enabled="false" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtDesSvcPP" runat="server" MaxLength="200" Width="100%" TextMode="MultiLine" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UplGenOT" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:ImageButton ID="IbtGenOT" Width="25px" Height="25px" ImageUrl="~/images/AddOrder.png" runat="server" CommandName="GenOT" ToolTip="gener" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="IbtGenOT" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OT">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblOTP" Text='<%# Eval("Ot") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblOT" Text='<%# Eval("Ot") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rpt">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblRteP" Text='<%# Eval("IdReporte") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblRte" Text='<%# Eval("IdReporte") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Externa" HeaderStyle-Width="1%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbRExtP" Checked='<%# Eval("ReparacionExterna").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="CkbRExt" Checked='<%# Eval("ReparacionExterna").ToString()=="1" ? true : false %>' runat="server" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="CkbRExtPP" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterStyle-Width="10%">
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
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div id="Pn_Sugerido" class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitPnSugerido" runat="server" Text="Partes sugeridos" /></h6>
                                <div class="CentrarGrid pre-scrollable">
                                    <asp:GridView ID="GrdPnSugerd" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDescSvc" Text='<%# Eval("PN") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cantidad">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Cantidad") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div id="Vlor_MO" class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitDetalleMH" runat="server" Text="mano de obra" /></h6>
                                <div class="CentrarGrid pre-scrollable">
                                    <asp:GridView ID="GrdMO" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Licencia">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDescSvc" Text='<%# Eval("CodLicenciaDMO") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("DescLicencia") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="estimado">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("TiempoEstimado") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="valor">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("ValorHoraTL") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("TotalMO") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtClosDetElemHK" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw5AsigSvcsLote_Elem_HK" runat="server">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigSvcMasivo" runat="server" Text="asignar servicios " />
                    </h6>
                    <div class="CentrarContndSn DivMarco">
                        <div class="row">
                            <div class="col-sm-1">
                                <asp:Button ID="BtnAsigSvcMasivo" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnAsigSvcMasivo_Click" Text="asignar" />
                            </div>
                        </div>
                        <asp:ImageButton ID="IbtClosAsigSvcMasivo" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtClosAsigSvcMasivo_Click" />
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdSvcsMasivo" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                DataKeyNames="IdSrvManto,CodOTPrta, IdReporte,Matricula, CodModeloDPSM,CodReferencia,Descripcion,SubOt"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbCk" Checked='<%# Eval("CK").ToString()=="1" ? true : false %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nro. Reporte">
                                        <ItemTemplate>
                                            <asp:Label ID="LblIdRte" Text='<%# Eval("IdReporte") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OT Principal">
                                        <ItemTemplate>
                                            <asp:Label ID="LblOtPpal" Text='<%# Eval("CodOTPrta") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDescSvc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NroDocumento">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("NroDocumento") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CodServicioManto">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodServicioManto") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ServicioPpal">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("ServicioPpal") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtClosAsigSvcMasivo" />
                    <asp:PostBackTrigger ControlID="BtnAsigSvcMasivo" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw6CargaMasiva" runat="server">
            <asp:UpdatePanel ID="UpPnlCargaMasiva" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitPlntllMasiv" runat="server" Text="Subir plantilla" /></h6>
                    <div class="CentrarContndSn DivMarco">
                        <asp:ImageButton ID="IbtCerrarSubMaxivo" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarSubMaxivo_Click" ImageAlign="Right" />
                        <asp:ImageButton ID="IbtSubirCargaMax" runat="server" ToolTip="Cargar archivo..." ImageUrl="~/images/SubirCarga.png" OnClick="IbtSubirCargaMax_Click" Width="30px" Height="30px" />
                        <asp:ImageButton ID="IbtGuardarCargaMax" runat="server" ToolTip="Guardar" ImageUrl="~/images/Descargar.png" OnClick="IbtGuardarCargaMax_Click" Width="30px" Height="30px" Visible="false" OnClientClick="javascript:return confirm('¿Desea almacenar la información?', 'Mensaje de sistema')" />
                        <div class="row">
                            <div id="DatosCargar" class="col-sm-12  heightPltll">
                                <div class="ScrollDet2">
                                    <asp:GridView ID="GrdCargaMax" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="False"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtPosRF" Text='<%# Eval("Pos") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="9%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtTipo" Text='<%# Eval("Tipo") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Refe" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtRef" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtCantRF" Text='<%# Eval("Qty") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unidad Medida" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtUndDespch" Text='<%# Eval("CodUndMed") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unidad compra" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtUndCompra" Text='<%# Eval("Unit_Purchase") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unidad Sistema" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TxtUndCompraSys" Text='<%# Eval("UndCompra") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="PnNoExiste" class="col-sm-5">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitPnNoExiste" runat="server" Text="p/n nuevos" /></h6>
                                <div class="CentrarGrid pre-scrollable">
                                    <asp:GridView ID="GrdPnNew" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="False"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtPosRF" Text='<%# Eval("Pos") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtCantRF" Text='<%# Eval("Qty") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div id="Inconsistencias" class="col-sm-7">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitIncosistnc" runat="server" Text="inconsistencias" /></h6>
                                <div class="CentrarGrid pre-scrollable">
                                    <asp:GridView ID="GrdInconsist" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="False"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtPosRF" Text='<%# Eval("Pos") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Refe" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtRef" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtCantRF" Text='<%# Eval("Qty") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unidad compra" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtUndCompra" Text='<%# Eval("Unit_Purchase") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unidad Sistema" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="TxtUndCompraSys" Text='<%# Eval("UndCompra") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="GridFooterStyle" />
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarSubMaxivo" />
                    <asp:PostBackTrigger ControlID="IbtSubirCargaMax" />
                    <asp:PostBackTrigger ControlID="IbtGuardarCargaMax" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw7Imprimir" runat="server">
            <h6 class="TextoSuperior">
                <asp:Label ID="LblTitImpresion" runat="server" Text="Impresión" />
            </h6>
            <asp:Button ID="BtnImprPpal" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="5%" OnClick="BtnImprPpal_Click" Text="Principal" />
            <asp:Button ID="BtnImprDet" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="5%" OnClick="BtnImprDet_Click" Text="Detalle" />
            <asp:ImageButton ID="IbtCerrarImpr" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpr_Click" />
            <br />
            <RpVw:ReportViewer ID="RpVwAll" runat="server" Width="98%" />
        </asp:View>
    </asp:MultiView>
</asp:Content>
