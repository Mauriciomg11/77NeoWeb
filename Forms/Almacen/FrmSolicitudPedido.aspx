<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmSolicitudPedido.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmSolicitudPedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .CentrarContenedor {
            position: relative;
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

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .ScrollDet2 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 90%;
        }

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 60%;
            margin-left: -30%;
            height: 85%;
            padding: 5px;
            top: 290px;
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
            $('#<%=DdlPriord.ClientID%>').chosen();
            $('#<%=DdlTipo.ClientID%>').chosen();
            $('#<%=DdlPpt.ClientID%>').chosen();
            $('#<%=DdlCcosto.ClientID%>').chosen();
            $('#<%=DdlRespsbl.ClientID%>').chosen();
        }
        $(':text').on("focus", function () {
            //here set in localStorage id of the textbox
            localStorage.setItem("focusItem", this.id);
            //console.log(localStorage.getItem("focusItem"));test the focus element id
        });
        function ShowPopup() {
            $('#ModalBusqPN').modal('show');
            $('#ModalBusqPN').on('shown.bs.modal', function () {
                   document.getElementById('<%= TxtModalBusq.ClientID %>').focus();
                    document.getElementById('<%= TxtModalBusq.ClientID %>').select();<%-- --%>
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
  <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="ModalBusqPN" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitModalBusqPN" runat="server" Text="P/N" /></h4>
                </div>
                <div class="modal-body">
                    <table>
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="RdbMOdalBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N" GroupName="BusqPn" />&nbsp&nbsp&nbsp                               
                                <asp:RadioButton ID="RdbMOdalBusqDesc" runat="server" CssClass="LblEtiquet" Text="&nbsp descripcion" GroupName="BusqPn" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp   
                                 <asp:CheckBox ID="CkbIngrPNNuevo" runat="server" CssClass="LblEtiquet" Text="solicitar p/n nuevo" OnCheckedChanged="CkbIngrPNNuevo_CheckedChanged" AutoPostBack="true" />
                            </td>
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
                            DataKeyNames="CodReferencia,CodEstadoPn"
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
                                <asp:TemplateField HeaderText="descripcion">
                                    <ItemTemplate>
                                        <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="stock">
                                    <ItemTemplate>
                                        <asp:Label ID="LblStock" Text='<%# Eval("Cant") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="und med">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUndMed" Text='<%# Eval("CodUndMed") %>' runat="server" />
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

    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <br />
                    <br />
                    <div class="CentrarContenedor">
                        <div id="Botones" class="row">
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
                                <asp:Button ID="BtnCargaMaxiva" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnCargaMaxiva_Click" Text="Carga masiva" Width="100%" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnEliminar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnEliminar_Click" OnClientClick="target ='';" Text="eliminar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnAlert" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnAlert_Click" OnClientClick="target ='';" Text="alertas" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnOpenCotiza" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnOpenCotiza_Click" OnClientClick="target ='';" Text="alertas" />
                            </div>
                            <%--<div class="col-sm-1">
                                <asp:Button ID="BtnExportar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExportar_Click" OnClientClick="target ='';" Text="exportar" />
                            </div>--%>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-1">
                                <asp:Label ID="LblCodPedd" runat="server" CssClass="LblEtiquet" Text="pedido Nro.:" />
                                <asp:TextBox ID="TxtCodPedd" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFech" runat="server" CssClass="LblEtiquet" Text="fecha pedido" />
                                <asp:TextBox ID="TxtFech" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblPriord" runat="server" CssClass="LblEtiquet" Text="prioridad" />
                                <asp:DropDownList ID="DdlPriord" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="tipo" />
                                <asp:DropDownList ID="DdlTipo" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblPpt" runat="server" CssClass="LblEtiquet" Text="propuesta" />
                                <asp:DropDownList ID="DdlPpt" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblCotiza" runat="server" CssClass="LblEtiquet" Text="Cotización" />
                                <asp:TextBox ID="TxtCotiza" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblEstd" runat="server" CssClass="LblEtiquet" Text="estado" />
                                <asp:DropDownList ID="DdlEstd" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblCcosto" runat="server" CssClass="LblEtiquet" Text="c. costo" />
                                <asp:DropDownList ID="DdlCcosto" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-6">
                                <asp:Label ID="LblRespsbl" runat="server" CssClass="LblEtiquet" Text="responsable" />
                                <asp:DropDownList ID="DdlRespsbl" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:Label ID="LblObsrvcn" runat="server" CssClass="LblEtiquet" Text="observacion" />
                                <asp:TextBox ID="TxtObsrvcn" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" TextMode="MultiLine" />
                            </div>

                        </div>
                        <div class="ScrollDet2">
                            <asp:GridView ID="GrdDetSP" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdDetPedido,IdPedido,RefPN,Descripcion"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%"
                                OnRowCommand="GrdDetSP_RowCommand" OnRowEditing="GrdDetSP_RowEditing" OnRowUpdating="GrdDetSP_RowUpdating" OnRowCancelingEdit="GrdDetSP_RowCancelingEdit"
                                OnRowDeleting="GrdDetSP_RowDeleting" OnRowDataBound="GrdDetSP_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="pos">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPoscP" Text='<%# Eval("posicion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPnP" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="IbtBusqPn" CssClass="BotonNewGrid" ImageUrl="~/images/FindV3.png" runat="server" CommandName="BusqPN" ToolTip="Buscar p/n" />
                                            <asp:TextBox ID="TxtPNPP" runat="server" MaxLength="80" Width="80%" Enabled="false" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="descripc" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDescP" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtDescPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="referenc" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblRefP" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblRef" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtRefPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cant" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCantP" Text='<%# Eval("CantidadTotal") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtCant" Text='<%# Eval("CantidadTotal") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtCantPP" runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Text="0" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="unid med" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="UndMedP" Text='<%# Eval("CodUndMedida") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="UndMed" Text='<%# Eval("CodUndMedida") %>' runat="server" Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtUndMPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Notas") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtSNPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estad" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCodSegP" Text='<%# Eval("CodSeguimiento") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtEstdPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="matricula" HeaderStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Matricula") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtHkPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
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
                </asp:View>
                <asp:View ID="Vw1Busq" runat="server">
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOpcBusq" runat="server" Text="Opciones de búsq." />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                    <table class="TablaBusqueda">
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="RdbBusqNumSlPd" runat="server" CssClass="LblEtiquet" Text="&nbsp solicitud" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N:" GroupName="Busq" />
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
                    <div class="CentrarBusq DivMarco">

                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="IdPedido"
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
                                    <asp:TemplateField HeaderText="pedido">
                                        <ItemTemplate>
                                            <asp:Label ID="LblCodPedP" Text='<%# Eval("CodPedido") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="referencia">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPnP" Text='<%# Eval("PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="fecha">
                                        <ItemTemplate>
                                            <asp:Label ID="LblFechPedP" Text='<%# Eval("FechaPedido") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estado">
                                        <ItemTemplate>
                                            <asp:Label ID="LblEstado" Text='<%# Eval("Estado") %>' runat="server" />
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
                <asp:View ID="Vw2CargaMasiva" runat="server">
                    <br />
                    <asp:Label ID="LblCargaMasvNumPed" runat="server" CssClass="LblEtiquet" Text="pedido:"></asp:Label>
                    <asp:TextBox ID="TxtCargaMasvNumPed" runat="server" CssClass="Form-control-sm heightCampo" Width="7%" Enabled="false" />
                    <asp:ImageButton ID="IbtCerrarSubMaxivo" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarSubMaxivo_Click" ImageAlign="Right" />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOTCargMasiv" runat="server" Text="Subir Evaluación"></asp:Label></h6>
                    <asp:ImageButton ID="IbtSubirCargaMax" runat="server" ToolTip="Cargar archivo..." ImageUrl="~/images/SubirCarga.png" OnClick="IbtSubirCargaMax_Click" Width="30px" Height="30px" />
                    <asp:ImageButton ID="IbtGuardarCargaMax" runat="server" ToolTip="Guardar" ImageUrl="~/images/Descargar.png" OnClick="IbtGuardarCargaMax_Click" Width="30px" Height="30px" Enabled="false" OnClientClick="javascript:return confirm('¿Desea almacenar la información?', 'Mensaje de sistema')" />
                    <br />
                    <asp:FileUpload ID="FileUpRva" runat="server" Font-Size="9px" Visible="false" />
                    <asp:GridView ID="GrdCargaMax" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="False"
                        CssClass="DiseñoGrid table table-sm" GridLines="Both">
                        <Columns>
                            <asp:TemplateField HeaderText="Pos" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtPosRF" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cant" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtCantRF" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad Medida" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtUMRF" Text='<%# Eval("UndDespacho") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unidad Sistema" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtUMSysRF" Text='<%# Eval("UndSistema") %>' runat="server" Width="100%" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IPC - FIG - ITEM" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:TextBox ID="TxtIPCRF" Text='<%# Eval("IPC") %>' runat="server" Width="100%" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                    </asp:GridView>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbtBusqueda" />
            <asp:PostBackTrigger ControlID="IbtCerrarBusq" />
            <asp:PostBackTrigger ControlID="IbtModalBusq" />
            <asp:PostBackTrigger ControlID="BtnAlert" />
            <asp:PostBackTrigger ControlID="IbtSubirCargaMax" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
