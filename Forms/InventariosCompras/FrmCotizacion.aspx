<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmCotizacion.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmCotizacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .CentrarContNumCotiza {
            left: 50%;
            /*determinamos una anchura*/
            width: 98%;
            margin-left: 1%;
            height: 8%;
        }

        .CentrarContenedor {
            position: absolute;
            left: 50%;
            width: 98%;
            margin-left: -49%;
            height: 90%;
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
             top: 270px
        }

        .TextR {
            text-align: right;
        }

        .CentrarExportar {
            position: absolute;
            left: 50%;
            width: 40%;
            margin-left: -20%;
            height: 15%;
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
        }
        function myFuncionddl() {
            $('#<%=DdlProvee.ClientID%>').chosen();
            $('#<%=DdlContact.ClientID%>').chosen();
            $('#<%=DdlTipoCot.ClientID%>').chosen();
            $('#<%=DdlMoned.ClientID%>').chosen();
            $('#<%=DdlEstd.ClientID%>').chosen();
            $('#<%=DdlTipoPago.ClientID%>').chosen();
            $('#<%=DdlLugarEntrg.ClientID%>').chosen();
            $('#<%=DdlMedioCot.ClientID%>').chosen();
        }
        function ShowPopup() {
            $('#ModalBusqSP').modal('show');
            $('#ModalBusqSP').on('shown.bs.modal', function () {
                document.getElementById('<%= TxtModalBusq.ClientID %>').focus();
                document.getElementById('<%= TxtModalBusq.ClientID %>').select();
            });
        }
        $(':text').on("focus", function () {
            //here set in localStorage id of the textbox
            localStorage.setItem("focusItem", this.id);
            //console.log(localStorage.getItem("focusItem"));test the focus element id
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="ModalBusqSP" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitModalBusqPN" runat="server" Text="pedidos" /></h4>
                </div>
                <div class="modal-body">
                    <table class="TablaBusqueda">
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="RdbMOdalBusqSP" runat="server" CssClass="LblEtiquet" Text="&nbsp pedido" GroupName="BusqSP" />&nbsp&nbsp&nbsp                               
                                <asp:RadioButton ID="RdbMOdalBusqPPT" runat="server" CssClass="LblEtiquet" Text="&nbsp propuesta" GroupName="BusqSP" />&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbMOdalBusqPet" runat="server" CssClass="LblEtiquet" Text="&nbsp peticion" GroupName="BusqSP" />
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
                        <asp:ImageButton ID="IbtAprDetAll" runat="server" ImageUrl="~/images/Check1.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtAprDetAll_Click" />
                        <asp:GridView ID="GrdModalBusqCot" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
                            DataKeyNames="IdDetPedido,SN, DescPn"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowDataBound="GrdModalBusqCot_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbA" Checked='<%# Eval("CHK").ToString()=="1" ? true : false %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="pedido">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodSped" Text='<%# Eval("CodPedido") %>' runat="server" />
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
                                <asp:TemplateField HeaderText="referencia">
                                    <ItemTemplate>
                                        <asp:Label ID="LblRef" Text='<%# Eval("CodReferencia") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cant">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCant" Text='<%# Eval("CantidadTotal") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="und med">
                                    <ItemTemplate>
                                        <asp:Label ID="LblUndM" Text='<%# Eval("CodUndMed") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="seguimiento">
                                    <ItemTemplate>
                                        <asp:Label ID="LbSeg" Text='<%# Eval("SEGMT") %>' runat="server" />
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
                    <asp:Button ID="BtnAsignarModal" runat="server" class="btn btn-default" Text="asignar" OnClick="BtnAsignarModal_Click" />
                    <asp:Button ID="BtnCloseModalBusqPN" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="CentrarContNumCotiza DivMarco">
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblNumCotiza" runat="server" CssClass="LblEtiquet" Text="cotización Nro.:" />
                                <asp:TextBox ID="TxtNumCotiza" runat="server" CssClass=" heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblNumPetcn" runat="server" CssClass="LblEtiquet" Text="peticion" />
                                <asp:TextBox ID="TxtNumPetcn" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblNumDocum" runat="server" CssClass="LblEtiquet" Text="documento" />
                                <asp:TextBox ID="TxtNumDocum" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblDatosPpt" runat="server" CssClass="LblEtiquet" Text="propuesta" />
                                <asp:TextBox ID="TxtDatosPpt" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblSnRepa" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                <asp:TextBox ID="TxtSnRepa" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                    </div>
                    <div class="CentrarContenedor DivMarco">
                        <div id="Botones" class="row">
                            <div class="col-sm-4">
                                <asp:Button ID="BtnCompra" runat="server" CssClass="btn btn-outline-primary Font_btnCrud" OnClick="BtnCompra_Click" Width="32%" Font-Size="13px" Font-Bold="true" Text="compra" />
                                <asp:Button ID="BtnRepa" runat="server" CssClass="btn btn-outline-primary Font_btnCrud" OnClick="BtnRepa_Click" Width="32%" Font-Size="13px" Font-Bold="true" Text="reparacion" />
                                <asp:Button ID="BtnInterc" runat="server" CssClass="btn btn-outline-primary Font_btnCrud" OnClick="BtnInterc_Click" Width="32%" Font-Size="13px" Font-Bold="true" Text="intercambio" />
                            </div>
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
                                <asp:Button ID="BtnCargaMaxiva" runat="server" CssClass="btn btn-success Font_btnCrud" OnClick="BtnCargaMaxiva_Click" Text="Cargar" Width="100%" Enabled="false" />
                                <asp:FileUpload ID="FileUpCot" runat="server" Font-Size="9px" Visible="false" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnEliminar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnEliminar_Click" OnClientClick="target ='';" Text="eliminar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnExportar" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExportar_Click" OnClientClick="target ='';" Text="exportar" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="BtnOpenSolPed" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnOpenSolPed_Click" OnClientClick="target ='';" Text="s. pedido" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblProvee" runat="server" CssClass="LblEtiquet" Text="proveedor" />
                                <asp:DropDownList ID="DdlProvee" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" OnTextChanged="DdlProvee_TextChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblContact" runat="server" CssClass="LblEtiquet" Text="contacto" />
                                <asp:DropDownList ID="DdlContact" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblMoned" runat="server" CssClass="LblEtiquet" Text="moneda" />
                                <asp:DropDownList ID="DdlMoned" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" OnTextChanged="DdlMoned_TextChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTipoCot" runat="server" CssClass="LblEtiquet" Text="tipo" />
                                <asp:DropDownList ID="DdlTipoCot" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblEstd" runat="server" CssClass="LblEtiquet" Text="estado" />
                                <asp:DropDownList ID="DdlEstd" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblTipoPago" runat="server" CssClass="LblEtiquet" Text="tipo pago" />
                                <asp:DropDownList ID="DdlTipoPago" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblLugarEntrg" runat="server" CssClass="LblEtiquet" Text="lugar entrega" />
                                <asp:DropDownList ID="DdlLugarEntrg" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblMedioCot" runat="server" CssClass="LblEtiquet" Text="medio cotizacion" />
                                <asp:DropDownList ID="DdlMedioCot" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-md-3" style="">
                                <asp:Label ID="LblObsrv" runat="server" CssClass="LblEtiquet" Text="observaciones" />
                                <asp:TextBox ID="TxtObsrv" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="MultiLine" Height="40px" />
                            </div>
                        </div>
                        <div id="Fechas" class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechCot" runat="server" CssClass="LblEtiquet" Text="fecha cotizacion" />
                                <asp:TextBox ID="TxtFechCot" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechPlazRes" runat="server" CssClass="LblEtiquet" Text="fecha plazo respuesta" />
                                <asp:TextBox ID="TxtFechPlazRes" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechRespt" runat="server" CssClass="LblEtiquet" Text="fecha respuesta" />
                                <asp:TextBox ID="TxtFechRespt" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechVigc" runat="server" CssClass="LblEtiquet" Text="fecha vigencia" />
                                <asp:TextBox ID="TxtFechVigc" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                        </div>
                        <div id="valores" class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblSubTtal" runat="server" CssClass="LblEtiquet" Text="sub total" />
                                <asp:TextBox ID="TxtSubTtal" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblIVA" runat="server" CssClass="LblEtiquet" Text="IVA" />
                                <asp:TextBox ID="TxtIVA" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblOtrImpt" runat="server" CssClass="LblEtiquet" Text="otros impuestos" />
                                <asp:TextBox ID="TxtOtrImpt" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" OnTextChanged="TxtOtrImpt_TextChanged" AutoPostBack="true" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTtl" runat="server" CssClass="LblEtiquet" Text="total" />
                                <asp:TextBox ID="TxtTtl" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechTRM" runat="server" CssClass="LblEtiquet" Text="fecha TRM" />
                                <asp:TextBox ID="TxtFechTRM" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Label ID="LblTRM" runat="server" CssClass="LblEtiquet" Text="TRM" />
                                <asp:TextBox ID="TxtTRM" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                            </div>
                        </div>
                        <div class="ScrollDet2">
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="IbtAprPNAll" runat="server" ImageUrl="~/images/Check1.png" ImageAlign="AbsBottom" Height="30px" Width="30px" OnClick="IbtAprPNAll_Click" Enabled="false" />
                                    </td>

                                    <td>
                                        <asp:TextBox ID="TxtBusqPn" runat="server" Width="100%" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                                    <td>
                                        <asp:ImageButton ID="IbtBusqPn" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtBusqPn_Click" /></td>
                                </tr>
                            </table>
                            <asp:GridView ID="GrdDet" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                DataKeyNames="IdDetCotizacion,IdCotizacion, IdDetPedido, CodAeronaveCT,PN,CodUndMed, Cantidad, ValorUnidad, TasaIva, Sn, Ccostos,AccionDet, ObservacionesDC, CodEstdo, TiempoEntrega,
                                                UndMinimaCompra, Alterno"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%" Enabled="false" AllowSorting="true"
                                OnRowCommand="GrdDet_RowCommand"
                                OnRowDeleting="GrdDet_RowDeleting" OnRowDataBound="GrdDet_RowDataBound" OnSorting="GrdDet_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sel." HeaderStyle-Width="1%" SortExpression="Aprobacion">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbAprob" Checked='<%# Eval("Aprobacion").ToString()=="1" ? true : false %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="pedido" HeaderStyle-Width="1%" SortExpression="CodPedido">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPedido" Text='<%# Eval("CodPedido") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pos." HeaderStyle-Width="1%" SortExpression="Posicion">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPosc" Text='<%# Eval("Posicion") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="15%" SortExpression="PN">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DdlPN" runat="server" Width="100%" Height="20px" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="descripc" HeaderStyle-Width="15%" SortExpression="DESPN">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDesc" Text='<%# Eval("DESPN") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="cant" HeaderStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtCant" Text='<%# Eval("Cantidad") %>' runat="server" CssClass="TextR" OnTextChanged="TxtCant_TextChanged" AutoPostBack="true" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="und Medida" HeaderStyle-Width="6%" SortExpression="CodUndMed">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DdlUM" runat="server" Width="100%" Height="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="valor und" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtVlor" Text='<%# Eval("ValorUnidad") %>' runat="server" CssClass="TextR" OnTextChanged="TxtVlor_TextChanged" AutoPostBack="true" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IVA" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtTsIVA" Text='<%# Eval("TasaIva") %>' runat="server" CssClass="TextR" OnTextChanged="TxtTsIVA_TextChanged" AutoPostBack="true" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="valor IVA" HeaderStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblVlrIVA" Text='<%# Eval("ValorIva") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="total" HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="LblVlrTtl" Text='<%# Eval("ValorTotal") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estado" HeaderStyle-Width="10%" SortExpression="CodEstdo">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="DdlEstdElem" runat="server" Width="100%" Height="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="tiempo entrega" HeaderStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtTiempEntr" Text='<%# Eval("TiempoEntrega") %>' runat="server" CssClass="TextR" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="und min compra" HeaderStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtUndMinCompra" Text='<%# Eval("UndMinimaCompra") %>' runat="server" CssClass="TextR" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="parte alterno" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TxtAlterno" Text='<%# Eval("Alterno") %>' runat="server" CssClass="TextR" Width="100%" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="IbtBusqSP" CssClass="BotonNewGrid" ImageUrl="~/images/FindV3.png" runat="server" CommandName="AddNew" ToolTip="asignar solicitud" />
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
                        <asp:Label ID="LblTitOpcBusq" runat="server" Text="opciones de búsq." />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                    <table class="TablaBusqueda">
                        <tr>
                            <td colspan="3">
                                <asp:RadioButton ID="RdbBusqNumCot" runat="server" CssClass="LblEtiquet" Text="&nbsp cotizacion" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqProvee" runat="server" CssClass="LblEtiquet" Text="&nbsp proveedor" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqPN" runat="server" CssClass="LblEtiquet" Text="&nbsp P/N:" GroupName="Busq" />&nbsp&nbsp&nbsp
                                    <asp:RadioButton ID="RdbBusqSN" runat="server" CssClass="LblEtiquet" Text="&nbsp S/N:" GroupName="Busq" />
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
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="IdCotizacion"
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
                                            <asp:Label ID="LblCodCot" Text='<%# Eval("CodCotizacion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("TipoCotiza") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="fecha">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Fecha") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="proveedor">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("RazonSocial") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="P/N">
                                        <ItemTemplate>
                                            <asp:Label ID="LblPnP" Text='<%# Eval("PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="S/N">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("SN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
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
                <asp:View ID="Vw2Exportar" runat="server">
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitExport" runat="server" Text="opciones de búsq." />
                    </h6>
                    <asp:ImageButton ID="IbtCloseExport" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCloseExport_Click" />
                    <div class="CentrarExportar DivMarco">
                        <div id="BtnesExport" class="row">
                            <div class="col-sm-6">
                                <asp:Button ID="BtnExportDetCotiza" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExportDetCotiza_Click" OnClientClick="target ='';" Text="Detalle" />
                            </div>
                            <div class="col-sm-6">
                                <asp:Button ID="BtnExportDetUnidMed" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExportDetUnidMed_Click" OnClientClick="target ='';" Text="Unidad de medida" />
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DdlProvee" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="DdlMoned" EventName="TextChanged" />
            <asp:PostBackTrigger ControlID="IbtModalBusq" />
            <asp:PostBackTrigger ControlID="IbtAprDetAll" />
            <asp:PostBackTrigger ControlID="BtnExportDetCotiza" />
            <asp:PostBackTrigger ControlID="BtnExportDetUnidMed" />
            <asp:PostBackTrigger ControlID="BtnCargaMaxiva" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
