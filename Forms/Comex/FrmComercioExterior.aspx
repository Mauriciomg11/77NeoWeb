<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmComercioExterior.aspx.cs" Inherits="_77NeoWeb.Forms.Comex.FrmComercioExterior" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .TextR {
            text-align: right;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .TablaBusquedaTipo {
            position: relative;
            text-align: center;
            left: 30%;
            width: 22%;
            height: 5%;
            top: 60px;
        }

        .CentrarBusq {
            position: relative;
            left: 50%;
            width: 60%;
            margin-left: -30%;
            height: 85%;
            padding: 5px;
            top: 70px
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
            $('#<%=DdlProv.ClientID%>').chosen();
            $('#<%=DdlFltPrvdr.ClientID%>').chosen();
            $('#<%=DdlFltOrgPrvdr.ClientID%>').chosen();
            $('#<%=DdlBdgPrvdr.ClientID%>').chosen();
            $('#<%=DdlLbGuPrvdr.ClientID%>').chosen();
            $('#<%=DdlLcIpPrvdr.ClientID%>').chosen();
            $('#<%=DdlAgcmPrvdr.ClientID%>').chosen();
            $('#<%=DdlIVAPrvdr.ClientID%>').chosen();
            $('#<%=DdlArclPrvdr.ClientID%>').chosen();
            $('#<%=DdlFlNlPrvdr.ClientID%>').chosen();
            $('#<%=DdlGst1Prvdr.ClientID%>').chosen();
            $('#<%=DdlGst2Prvdr.ClientID%>').chosen();
            $('#<%=DdlGstEPrvdr.ClientID%>').chosen();
        }
        function ShowPopup() {
             <%--$('#ModalBusqCompraCotiza').modal('show');
             $('#ModalBusqCompraCotiza').on('shown.bs.modal', function () {
                 document.getElementById('<%= TxtModalBusq.ClientID %>').focus();
                document.getElementById('<%= TxtModalBusq.ClientID %>').select();
            });--%>
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <br />
                    <br />
                    <br />
                    <br />
                    <div class="CentrarContenedor DivMarco">
                        <div id="DatosGnrles" class="row">
                            <div id="Botones" class="row">
                                <div class="col-sm-1">
                                    <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnConsultar_Click" OnClientClick="target ='';" Text="Consultar" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnIngresar_Click" OnClientClick="target ='';" Text="Nuevo" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnModificar_Click" OnClientClick="target ='';" Text="Modificar" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:Button ID="BtnEliminar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnEliminar_Click" OnClientClick="target ='';" Text="Eliminar" />
                                </div>
                            </div>
                            <div id="Inform_Basica" class="col-sm-5">
                                <div id="documentoNro" class="row">
                                    <div id="NrDoc" class="col-sm-6">
                                        <asp:Label ID="LblNroEmbq" runat="server" CssClass="LblEtiquet" Text="Embarque Nro.:" />
                                        <asp:TextBox ID="TxtNumDoc" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:Label ID="LblFecha" runat="server" CssClass="LblEtiquet" Text="Fecha" />
                                        <asp:TextBox ID="TxtFecha" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                                    </div>
                                </div>
                                <div id="Opciones" class="row">
                                    <div class="col-sm-7">
                                        <asp:Table ID="TblOpcTipoDoc" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:RadioButton ID="RdbRepa" runat="server" CssClass="LblEtiquet" Text="&nbsp Reparación" GroupName="TipoDoc" Enabled="false"  />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    &nbsp&nbsp
                                                    <asp:RadioButton ID="RdbCompra" runat="server" CssClass="LblEtiquet" Text="&nbsp Compra" GroupName="TipoDoc" Enabled="false"  />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    &nbsp&nbsp
                                                    <asp:RadioButton ID="RdbIntercambio" runat="server" CssClass="LblEtiquet" Text="&nbsp Intercambio" GroupName="TipoDoc" Enabled="false"  />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </div>
                                    <div class="col-sm-5">
                                        <asp:Table ID="TblOpcImpotExpor" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:RadioButton ID="RdbImportar" runat="server" CssClass="LblEtiquet" Text="&nbsp Importar" GroupName="ImporExpor" Enabled="false"  />
                                                </asp:TableCell>
                                                <asp:TableCell>
                                                    &nbsp
                                                    <asp:RadioButton ID="RdbExporar" runat="server" CssClass="LblEtiquet" Text="&nbsp Exportar" GroupName="ImporExpor" Enabled="false" />
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </div>
                                </div>
                            </div>
                            <div id="ProveedorGuia" class="col-sm-7">
                                <div id="Proveedor" class="row">
                                    <div class="col-sm-12">
                                    </div>
                                </div>
                                <div id="Guia-Peso" class="row">
                                    <div id="NrGuia" class="col-md-3" style="">
                                        <asp:Label ID="LblGuia" runat="server" CssClass="LblEtiquet" Text="Guia" />
                                        <asp:TextBox ID="TxtGuia" runat="server" CssClass="form-control-sm heightCampo" MaxLength="20" Enabled="false" Width="100%" />
                                    </div>
                                    <div id="Peso" class="col-sm-2">
                                        <asp:Label ID="LblPeso" runat="server" CssClass="LblEtiquet" Text="Peso" />
                                        <asp:TextBox ID="TxtPeso" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                    </div>
                                    <div id="NrPaq" class="col-sm-2">
                                        <asp:Label ID="LblNrPaq" runat="server" CssClass="LblEtiquet" Text="Paquetes" />
                                        <asp:TextBox ID="TxtNrPaq" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0" onkeypress="return solonumeros(event);" Enabled="false" /><%----%>
                                    </div>
                                    <div id="Observac" class="col-md-5" style="">
                                        <asp:Label ID="LblObsrv" runat="server" CssClass="LblEtiquet" Text="Observaciones" />
                                        <asp:TextBox ID="TxtObsrv" runat="server" CssClass="form-control-sm heightCampo" MaxLength="240" Enabled="false" Width="100%" TextMode="MultiLine" Height="40px" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="ScrollDet2">
                            <asp:GridView ID="GrdDet" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                DataKeyNames=" IdDetCotiza,IdShippin, CodCotizacion"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%" Enabled="false" AllowSorting="true"
                                OnRowCommand="GrdDet_RowCommand"
                                OnRowDeleting="GrdDet_RowDeleting" OnRowDataBound="GrdDet_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Pos." HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPosc" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Documento" HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblNumDoc" Text='<%# Eval("Documento") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Proveedor" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblProv" Text='<%# Eval("RazonSocial") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPN" Text='<%# Eval("PN") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripc" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant Doc" HeaderStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCantDoc" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant Emb" HeaderStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCantEmb" Text='<%# Eval("CantidadSO") %>' runat="server" CssClass="TextR" Width="100%"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Und Medida" HeaderStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblUndMed" Text='<%# Eval("CodUndMed") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valor Und ME" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblVlrUndDoc" Text='<%# Eval("VlrUndDoc") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vlr Und ML" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblVlrUndML" Text='<%# Eval("ValorUndMLText") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total ML" HeaderStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblVlrTtlReg" Text='<%# Eval("ValorTtlMLText") %>' runat="server" Width="100%" CssClass="TextR"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Factura" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtFact" Text='<%# Eval("FacturaOE") %>' runat="server" CssClass="TextR" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="IbtBusqSP" CssClass="BotonNewGrid" ImageUrl="~/images/FindV3.png" runat="server" CommandName="AddNew" ToolTip="Asignar Documento" />
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
                </asp:View>
                <asp:View ID="Vw1Busq" runat="server">
                    <br />
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOpcBusq" runat="server" Text="Opciones de Búsqueda." />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click1" />
                    <%--<table id="TipoSO" class="TablaBusquedaTipo">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RdbBqCompra" runat="server" CssClass="LblEtiquet" Font-Bold="true" Text="&nbsp Compra" GroupName="BqTipo" /></td>
                            <td>
                                <asp:RadioButton ID="RdbBqRepa" runat="server" CssClass="LblEtiquet" Font-Bold="true" Text="&nbsp Proveedor" GroupName="BqTipo" /></td>
                            <td>
                                <asp:RadioButton ID="RdbBqInter" runat="server" CssClass="LblEtiquet" Font-Bold="true" Text="&nbsp Intercambio" GroupName="BqTipo" /></td>
                        </tr>
                    </table>--%>
                    <table class="TablaBusqueda">
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="RdbBusqOrden" runat="server" CssClass="LblEtiquet" Text="&nbsp Nro Embarque" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqGuia" runat="server" CssClass="LblEtiquet" Text="&nbsp Guia" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqDoc" runat="server" CssClass="LblEtiquet" Text="&nbsp Documento:" GroupName="Busq" /></td>
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
                    <div class="CentrarBusq DivMarco">
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodShippingOrder"
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
                                    <asp:TemplateField HeaderText="Embarque">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCodSO" Text='<%# Eval("CodShippingOrder") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Guia">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("NroGuia") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Documento">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Documento") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Tipo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fecha">
                                        <ItemTemplate>
                                            <asp:Label ID="LblFech" Text='<%# Eval("Fecha") %>' runat="server" />
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
                <asp:View ID="Vw2Conceptos" runat="server">
                    <div class="CentrarContenedor DivMarco">
                        <br />
                        <br />
                        <br />
                        <br />
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitConceptos" runat="server" Text="conceptos generales" /></h6>
                        <asp:ImageButton ID="IbtCerrarCnptos" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarCnptos_Click" />
                        <div id="TablaConceptos" class="row">
                            <div id="Tabla" class="col-sm-12">
                                <asp:Table ID="TblConcpts" runat="server" Visible="true" Width="100%">
                                    <asp:TableRow ID="LabelTit" Width="100%">
                                        <asp:TableCell ID="Conceptos" Width="15%">
                                            <asp:Label ID="LblConcpto" runat="server" CssClass="LblEtiquet" Text="Concepto" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="NP" Width="5%">
                                            <asp:Label ID="LblNP" runat="server" CssClass="LblEtiquet" Text="NP" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Proveed" Width="35%">
                                            <asp:Label ID="LblProvdr" runat="server" CssClass="LblEtiquet" Text="Proveedores" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="fecha" Width="1%">
                                            <asp:Label ID="LblFechaConcpt" runat="server" CssClass="LblEtiquet" Text="Fecha" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="MondExtr" Width="14%">
                                            <asp:Label ID="LblME" runat="server" CssClass="LblEtiquet" Text="(me)" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="MondLocl" Width="14%">
                                            <asp:Label ID="LblML" runat="server" CssClass="LblEtiquet" Text="(ml)" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FactServc" Width="10%">
                                            <asp:Label ID="LblFactSvc" runat="server" CssClass="LblEtiquet" Text="Factura" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="Flete">
                                        <asp:TableCell ID="TitFltNP">
                                            <asp:Label ID="LblFltNP" runat="server" CssClass="LblEtiquet" Text="Flete" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltNP">
                                            <asp:CheckBox ID="CKFltNP" runat="server" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltPrvdr">
                                            <asp:DropDownList ID="DdlFltPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="120%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltFech">
                                            <asp:TextBox ID="TxFltFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltME">
                                            <asp:TextBox ID="TxtFltME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltML">
                                            <asp:TextBox ID="TxtFltML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltFac">
                                            <asp:TextBox ID="TxtFltFac" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="FleteOrign">
                                        <asp:TableCell ID="TitFltOrg">
                                            <asp:Label ID="LblFltOrg" runat="server" CssClass="LblEtiquet" Text="Flete Origen" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FletOrgNP">
                                            <asp:CheckBox ID="CkFltOrgNP" runat="server" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltOrgPrvdr">
                                            <asp:DropDownList ID="DdlFltOrgPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltOrgFech">
                                            <asp:TextBox ID="TxtFltOrgFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltOrgME">
                                            <asp:TextBox ID="TxtFltOrgME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltOrgML">
                                            <asp:TextBox ID="TxtFltOrgML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FltOrgFact">
                                            <asp:TextBox ID="TxtFltOrgFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="Bodegaje">
                                        <asp:TableCell ID="TitBdjg">
                                            <asp:Label ID="LblBdjg" runat="server" CssClass="LblEtiquet" Text="Bodegaje" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="BdgNP">
                                            <asp:CheckBox ID="CkBdgNP" runat="server" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="BdgPrvdr">
                                            <asp:DropDownList ID="DdlBdgPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="BdgFech">
                                            <asp:TextBox ID="TxtBdgFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="BdgME">
                                            <asp:TextBox ID="TxtBdgME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="BdgML">
                                            <asp:TextBox ID="TxtBdgML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="BdgFact">
                                            <asp:TextBox ID="TxtBdgFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="RegLiberacGuia">
                                        <asp:TableCell ID="TitLibGu" Width="10%" ColumnSpan="2">
                                            <asp:Label ID="LblLbGu" runat="server" CssClass="LblEtiquet" Text="Liberacion Guia" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LbGuPrvdr">
                                            <asp:DropDownList ID="DdlLbGuPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LbGuFech">
                                            <asp:TextBox ID="TxtLbGuFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LbGuME">
                                            <asp:TextBox ID="TxtLbGuME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LbGuML">
                                            <asp:TextBox ID="TxtLbGuML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LbGuFact">
                                            <asp:TextBox ID="TxtLbGuFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="LicenImportac">
                                        <asp:TableCell ID="TitLcIp">
                                            <asp:Label ID="LblLcIp" runat="server" CssClass="LblEtiquet" Text="Licencia Importacion" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LcIpNP">
                                            <asp:CheckBox ID="CkLcIpNP" runat="server" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LcIpPrvdr">
                                            <asp:DropDownList ID="DdlLcIpPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LcIpFech">
                                            <asp:TextBox ID="TxtLcIpFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LcIpME">
                                            <asp:TextBox ID="TxtLcIpME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LcIpML">
                                            <asp:TextBox ID="TxtLcIpML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="LcIpFact">
                                            <asp:TextBox ID="TxtLcIpFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="Agenciamiento">
                                        <asp:TableCell ID="TitAgcm">
                                            <asp:Label ID="LblAgcm" runat="server" CssClass="LblEtiquet" Text="Agenciamiento" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="AgcmNP">
                                            <asp:CheckBox ID="CkAgcmNP" runat="server" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="AgcmPrvdr">
                                            <asp:DropDownList ID="DdlAgcmPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="AgcmFech">
                                            <asp:TextBox ID="TxtAgcmFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="AgcmME">
                                            <asp:TextBox ID="TxtAgcmME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="AgcmML">
                                            <asp:TextBox ID="TxtAgcmML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="AgcmFact">
                                            <asp:TextBox ID="TxtAgcmFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="IVA">
                                        <asp:TableCell ID="TitIVA" Width="10%" ColumnSpan="2">
                                            <asp:Label ID="LblIVA" runat="server" CssClass="LblEtiquet" Text="IVA" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="IVAPrvdr">
                                            <asp:DropDownList ID="DdlIVAPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="IVAFech">
                                            <asp:TextBox ID="TxtIVAFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="IVAME">
                                            <asp:TextBox ID="TxtIVAME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="IVAML">
                                            <asp:TextBox ID="TxtIVAML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="IVAFact">
                                            <asp:TextBox ID="TxtIVAFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="Arancel">
                                        <asp:TableCell ID="TitArcl">
                                            <asp:Label ID="LblArcl" runat="server" CssClass="LblEtiquet" Text="Arancel" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="ArclNP">
                                            <asp:CheckBox ID="CkArclNP" runat="server" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="ArclPrvdr">
                                            <asp:DropDownList ID="DdlArclPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="ArclFech">
                                            <asp:TextBox ID="TxtArclFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="ArclME">
                                            <asp:TextBox ID="TxtArclME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="ArclML">
                                            <asp:TextBox ID="TxtArclML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="ArclFact">
                                            <asp:TextBox ID="TxtArclFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="FleteNal">
                                        <asp:TableCell ID="TitFlNl" Width="10%" ColumnSpan="2">
                                            <asp:Label ID="LblFlNl" runat="server" CssClass="LblEtiquet" Text="Flete Nacional" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FlNlPrvdr">
                                            <asp:DropDownList ID="DdlFlNlPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FlNlFech">
                                            <asp:TextBox ID="TxtFlNlFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FlNlME">
                                            <asp:TextBox ID="TxtFlNlME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FlNlML">
                                            <asp:TextBox ID="TxtFlNlML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="FlNlFact">
                                            <asp:TextBox ID="TxtFlNlFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="OtroGast1">
                                        <asp:TableCell ID="TitGst1" Width="10%" ColumnSpan="2">
                                            <asp:Label ID="LblGst1" runat="server" CssClass="LblEtiquet" Text="Otros Gastos 1" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst1Prvdr">
                                            <asp:DropDownList ID="DdlGst1Prvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst1Fech">
                                            <asp:TextBox ID="TxtGst1Fech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst1ME">
                                            <asp:TextBox ID="TxtGst1ME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst1ML">
                                            <asp:TextBox ID="TxtGst1ML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst1Fact">
                                            <asp:TextBox ID="TxtGst1Fact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="OtroGast2">
                                        <asp:TableCell ID="TitGst2" Width="10%" ColumnSpan="2">
                                            <asp:Label ID="LblGst2" runat="server" CssClass="LblEtiquet" Text="Otros Gastos 2" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst2Prvdr">
                                            <asp:DropDownList ID="DdlGst2Prvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst2Fech">
                                            <asp:TextBox ID="TxtGst2Fech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst2ME">
                                            <asp:TextBox ID="TxtGst2ME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst2ML">
                                            <asp:TextBox ID="TxtGst2ML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="Gst2Fact">
                                            <asp:TextBox ID="TxtGst2Fact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow ID="GastoExtr">
                                        <asp:TableCell ID="TitGstE">
                                            <asp:Label ID="LblGstE" runat="server" CssClass="LblEtiquet" Text="Gasto Exterior" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="GstENP">
                                            <asp:CheckBox ID="CkGstENP" runat="server" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="GstEPrvdr">
                                            <asp:DropDownList ID="DdlGstEPrvdr" runat="server" CssClass="form-control-sm heightCampo" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="GstEFech">
                                            <asp:TextBox ID="TxtGstEFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="105px" TextMode="Date" MaxLength="10" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="GstEME">
                                            <asp:TextBox ID="TxtGstEME" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="GstEML">
                                            <asp:TextBox ID="TxtGstEML" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell ID="GstEFact">
                                            <asp:TextBox ID="TxtGstEFact" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw3AsignarCompraRepa" runat="server">
                    <br />
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsingar" runat="server" Text="Asignar Compras / Reparaciones." />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarAsignar" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsignar_Click" />
                    <div class="CentrarBusq DivMarco">
                        <div class="CentrarGrid pre-scrollable">
                            <asp:Label ID="Label1" runat="server" CssClass="LblEtiquet" Text="Proveedor" />
                            <asp:DropDownList ID="DdlProv" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlProv_TextChanged" AutoPostBack="true" />
                            <br />
                            <br />
                            <asp:ImageButton ID="IbtAprDetAll" runat="server" ImageUrl="~/images/Check1.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtAprDetAll_Click" ToolTip="Marcar / Desmarcar todos" />
                            <asp:Button ID="BtnAsignar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="120px" OnClick="BtnAsignar_Click" OnClientClick="target ='';" Text="asignar" />
                            <asp:GridView ID="GrdAsignarComRep" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                                DataKeyNames="CodReferencia,SN,CodTipoElem, ValorUnidad,IdDetOrdencompra,IdDetCotiza,CodCotizacion, TipoDocSO,TRM, CantPendF,TtlDoc,Aprobado"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdAsignarComRep_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbA" Checked='<%# Eval("CK").ToString()=="1" ? true : false %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="pedido">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDoc" Text='<%# Eval("Documento") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pos.">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPs" Text='<%# Eval("Posicion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripción">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant Doc">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCantDoc" Text='<%# Eval("Cantidad") %>' runat="server" HeaderStyle-Width="10%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant Pend" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtCantPend" Text='<%# Eval("CantPend") %>' runat="server" CssClass="TextR" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Und Med">
                                        <ItemTemplate>
                                            <asp:Label ID="LblUndM" Text='<%# Eval("CodUndMed") %>' runat="server" />
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
    </asp:UpdatePanel>
</asp:Content>
