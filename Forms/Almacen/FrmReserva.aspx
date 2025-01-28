<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmReserva.aspx.cs" Inherits="_77NeoWeb.Forms.Almacen.FrmReserva" %>

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

        .ScrollDet2 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 90%;
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
            $('[id *=DdlPersona]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MlVw" runat="server">
                <asp:View ID="Vw0General" runat="server">
                    <br />
                    <br />
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-1">
                                <asp:Label ID="LblNumRva" runat="server" CssClass="LblEtiquet" Text="Num Rva" />
                                <asp:TextBox ID="TxtIdRva" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" Visible="false" />
                                <asp:TextBox ID="TxtNumRva" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblNumOT" runat="server" CssClass="LblEtiquet" Text="orden trabajo" />
                                <asp:TextBox ID="TxtNumOT" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblNumRTE" runat="server" CssClass="LblEtiquet" Text="reporte no." />
                                <asp:TextBox ID="TxtNumRTE" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblEstado" runat="server" CssClass="LblEtiquet" Text="estad" />
                                <asp:TextBox ID="TxtEstado" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechaRv" runat="server" CssClass="LblEtiquet" Text="Fec" />
                                <asp:TextBox ID="TxtFechaRv" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <br />
                        <div id="botones" class="row">
                            <div class="col-sm-1">
                                <br />
                                <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnConsultar_Click" OnClientClick="target ='';" Text="consultar" />
                            </div>
                            <div class="col-sm-1">
                                <br />
                                <asp:Button ID="BtnExprt" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExprt_Click" OnClientClick="target ='';" Text="exportar" />
                            </div>
                            <div class="col-sm-1">
                                <br />
                                <asp:Button ID="BtnAlerta" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnAlerta_Click" OnClientClick="target ='_blank';" Text="alerta" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblMatr" runat="server" CssClass="LblEtiquet" Text="Hk" />
                                <asp:TextBox ID="TxtMatr" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblPnElem" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                <asp:TextBox ID="TxtPnElem" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblSnElem" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                <asp:TextBox ID="TxtSnElem" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <div id="Grids" class="row">
                            <div class="col-sm-8">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitDetRv" runat="server" Text="Reserv." /></h6>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblConsltPn" runat="server" Text="P/N: " CssClass="LblTextoBusq" /></td>
                                        <td>
                                            <asp:TextBox ID="TxtConsltPN" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                                        <td>
                                            <asp:ImageButton ID="IbtConsltPn" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsltPn_Click" /></td>
                                    </tr>
                                </table>
                                <div class="ScrollDet2">
                                    <asp:GridView ID="GrdReserva" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        DataKeyNames="CodReferencia,Pn,NumeroPosicion"
                                        CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" EmptyDataText="No existen registros ..!"
                                        OnSelectedIndexChanged="GrdReserva_SelectedIndexChanged" OnRowDataBound="GrdReserva_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="pos">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPos" Text='<%# Eval("NumeroPosicion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="referenc">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblRef" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPn" Text='<%# Eval("Pn") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="descripcion">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDescr" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant sol">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantSol" Text='<%# Eval("CantidadSolicitada") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="unid">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUnd" Text='<%# Eval("CODUNIDADMED") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant entreg">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantEntr" Text='<%# Eval("CantidadEntregada") %>' runat="server" Width="100%" />
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
                            <div class="col-sm-4">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitUsuario" runat="server" Text="recibo de la reserva" /></h6>
                                <asp:GridView ID="GrdUsuario" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                    CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" EmptyDataText="No existen registros ..!" DataKeyNames="CodIdDetalleSalida"
                                    OnRowEditing="GrdUsuario_RowEditing" OnRowUpdating="GrdUsuario_RowUpdating" OnRowCancelingEdit="GrdUsuario_RowCancelingEdit" OnRowDataBound="GrdUsuario_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="persona" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPersn" Text='<%# Eval("Persona") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DdlPersona" runat="server" Width="100%" Height="28px" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="fecha despacho">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFecMv" Text='<%# Eval("FechaMovimiento") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha recibo">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFecRc" Text='<%# Eval("FechaReciboReserva") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:ImageButton ID="IbtUpdate" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                                <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="GridFooterStyle" />
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                </asp:GridView>
                                <br />
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitStock" runat="server" Text="stock actual" /></h6>
                                <div class="ScrollDet2">
                                    <asp:GridView ID="GrdStok" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" EmptyDataText="No existen registros ..!">
                                        <Columns>
                                            <asp:TemplateField HeaderText="almacen">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPos" Text='<%# Eval("NomAlmacen") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblRef" Text='<%# Eval("Pn") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPn" Text='<%# Eval("Sn") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="lte">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblDescr" Text='<%# Eval("NumLote") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="cant">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantSol" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Bog">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblUnd" Text='<%# Eval("Bodega") %>' runat="server" Width="100%" />
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
                                    <asp:RadioButton ID="RdbBusqNumRsva" runat="server" CssClass="LblEtiquet" Text="&nbsp reserva" Checked="true" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqNumOT" runat="server" CssClass="LblEtiquet" Text="&nbsp ot" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqNumRte" runat="server" CssClass="LblEtiquet" Text="&nbsp reporte" GroupName="Busq" />&nbsp&nbsp&nbsp
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
                    <br /><br /><br />
                    <div class="CentrarBusq DivMarco">
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodNumOrdenTrab"
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
                                    <asp:TemplateField HeaderText="reserva">
                                        <ItemTemplate>
                                            <asp:Label ID="LblRsva" Text='<%# Eval("CodNumOrdenTrab") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="o.t">
                                        <ItemTemplate>
                                            <asp:Label ID="LblOT" Text='<%# Eval("OT") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="reporte no">
                                        <ItemTemplate>
                                            <asp:Label ID="LblNumRte" Text='<%# Eval("CodigoRTE") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aplica">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Aplicabilidad") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sn">
                                        <ItemTemplate>
                                            <asp:Label ID="LblSnEle" Text='<%# Eval("SN") %>' runat="server" />
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
                                            <asp:Label ID="LblFechOt" Text='<%# Eval("FechaOT") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estado">
                                        <ItemTemplate>
                                            <asp:Label ID="LblEstad" Text='<%# Eval("Estado") %>' runat="server" />
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
            <asp:PostBackTrigger ControlID="BtnExprt" />
            <asp:PostBackTrigger ControlID="BtnAlerta" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
