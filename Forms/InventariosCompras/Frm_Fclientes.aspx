<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="Frm_Fclientes.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.Frm_Fclientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarContenedor {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
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

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .AlinearTextoBoton {
            /* text-align: center;*/
            vertical-align: top;
        }

        .Scroll-table2 {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 400px
        }

        .CentarGrid {
            margin-left: auto;
            margin-right: auto;
            width: 50%;
        }

        .CentarGridAsig {
            /*  margin-left: auto;
            margin-right: auto;*/
            width: 48%;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .TextR {
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
            $('#<%=DdlTipoDoc.ClientID%>').chosen();
            $('#<%=DdlClasfJurd.ClientID%>').chosen();
            $('#<%=DdlTipoRegmn.ClientID%>').chosen();
            $('#<%=DdlClase.ClientID%>').chosen();
            $('#<%=DdlFormaPago.ClientID%>').chosen();
            $('#<%=DdlTipTerce.ClientID%>').chosen();
            $('#<%=DdllMoned.ClientID%>').chosen();
            $('#<%=DdlBanco.ClientID%>').chosen();
            $('#<%=DdlPais.ClientID%>').chosen();
            $('#<%=DdlCiudad.ClientID%>').chosen();
            $('#<%=DdlEstado.ClientID%>').chosen();
        }
        $(':text').on("focus", function () {
            //here set in localStorage id of the textbox
            localStorage.setItem("focusItem", this.id);
            //console.log(localStorage.getItem("focusItem"));test the focus element id
        });
        function ShowPopup() {
            $('#ModalBusqTerc').modal('show');
            $('#ModalBusqTerc').on('shown.bs.modal', function () {
                document.getElementById('<%= TxtModalBusq.ClientID %>').focus();
                document.getElementById('<%= TxtModalBusq.ClientID %>').select();
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <div id="ModalBusqTerc" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">
                        <asp:Label ID="LblTitModalBusqTerc" runat="server" Text="opciones de búsqueda" /></h4>
                </div>
                <div class="modal-body">
                    <asp:Table ID="TblMdlOpcBusTerc" runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:RadioButton ID="RdbMdlOpcBusqProv" runat="server" CssClass="LblEtiquet" Text="&nbsp razon social" GroupName="BusqTrc" />&nbsp&nbsp&nbsp 
                                <asp:RadioButton ID="RdbMdlOpcBusqCod" runat="server" CssClass="LblEtiquet" Text="&nbsp código" GroupName="BusqTrc" />

                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <table>
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
                        <asp:GridView ID="GrdModalBusqTercero" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="IdTercero"
                            CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" OnRowCommand="GrdModalBusqTercero_RowCommand" OnRowDataBound="GrdModalBusqTercero_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" ControlStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UplIr" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ImageButton ID="IbtIr" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Ir" ToolTip="Ir" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="IbtIr" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="código">
                                    <ItemTemplate>
                                        <asp:Label ID="LblCodTrcr" Text='<%# Eval("CodTercero") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="razon social">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("RazonSocial") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="moneda">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodMoneda") %>' runat="server" />
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
                    <asp:Button ID="BtnCloseModalBusqCompra" runat="server" CssClass="btn btn-default" Text="cerrar" data-dismiss="modal" />
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <br /><br />
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
                                <asp:Button ID="BtnExport" runat="server" CssClass="btn btn-primary Font_btnCrud" Width="100%" OnClick="BtnExport_Click" OnClientClick="target ='';" Text="exportar" />
                            </div>
                            <div class="col-sm-2">
                                <asp:CheckBox ID="CkbActivo" runat="server" Text="&nbsp activo" Enabled="false" ForeColor="#800000" Font-Bold="true" Font-Size="X-Large" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblNit" runat="server" CssClass="LblEtiquet" Text="NIT" />
                                <asp:Table ID="TblNit" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell Width="150px">
                                            <asp:TextBox ID="TxtNit" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" Enabled="false" />
                                        </asp:TableCell>
                                        <asp:TableCell Width="70px">
                                            <asp:TextBox ID="TxtDV" runat="server" CssClass="form-control-sm heightCampo TextR" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblTipoDoc" runat="server" CssClass="LblEtiquet" Text="tipo documento" />
                                <asp:DropDownList ID="DdlTipoDoc" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblClasfJurd" runat="server" CssClass="LblEtiquet" Text="clasif juridica" />
                                <asp:DropDownList ID="DdlClasfJurd" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblTipoRegmn" runat="server" CssClass="LblEtiquet" Text="tipo regimen" />
                                <asp:DropDownList ID="DdlTipoRegmn" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-md-3">
                                <br />
                                <asp:RadioButton ID="RdbProvdr" runat="server" CssClass="LblEtiquet" Text="&nbsp proveedor" GroupName="TipoTrcr" Enabled="false" />&nbsp&nbsp
                                <asp:RadioButton ID="RdbCliente" runat="server" CssClass="LblEtiquet" Text="&nbsp cliente" GroupName="TipoTrcr" Enabled="false" />&nbsp&nbsp                                
                                <asp:RadioButton ID="RdbAmbos" runat="server" CssClass="LblEtiquet" Text="&nbsp ambos" GroupName="TipoTrcr" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Label ID="LblRazonSoc" runat="server" CssClass="LblEtiquet" Text="razon social" />
                                <asp:TextBox ID="TxtRazonSoc" runat="server" CssClass="form-control-sm heightCampo" MaxLength="100" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-md-4">
                                <asp:Label ID="LblDirecc" runat="server" CssClass="LblEtiquet" Text="direccion" />
                                <asp:TextBox ID="TxtDirecc" runat="server" CssClass="form-control-sm heightCampo" MaxLength="150" Enabled="false" Width="100%" Height="40px" TextMode="MultiLine" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="LblTelef" runat="server" CssClass="LblEtiquet" Text="telefono" />
                                <asp:TextBox ID="TxtTelef" runat="server" CssClass="form-control-sm heightCampo" MaxLength="80" Enabled="false" Width="100%" TextMode="Phone" />
                            </div>
                            <div class="col-md-2">
                                <asp:Label ID="LblFax" runat="server" CssClass="LblEtiquet" Text="Fax" />
                                <asp:TextBox ID="TxtFax" runat="server" CssClass="form-control-sm heightCampo" MaxLength="40" Enabled="false" Width="100%" TextMode="Phone" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <asp:Label ID="LblCorreo" runat="server" CssClass="LblEtiquet" Text="correo" />
                                <asp:TextBox ID="TxtCorreo" runat="server" CssClass="form-control-sm heightCampo" MaxLength="80" Enabled="false" Width="100%" TextMode="Email" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblClase" runat="server" CssClass="LblEtiquet" Text="clase" />
                                <asp:DropDownList ID="DdlClase" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblFormaPago" runat="server" CssClass="LblEtiquet" Text="forma pago" />
                                <asp:DropDownList ID="DdlFormaPago" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblTipTerce" runat="server" CssClass="LblEtiquet" Text="Tipo tercero" />
                                <asp:DropDownList ID="DdlTipTerce" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label ID="LblMoned" runat="server" CssClass="LblEtiquet" Text="moneda" />
                                <asp:DropDownList ID="DdllMoned" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-md-1">
                                <asp:Label ID="LblCodPostal" runat="server" CssClass="LblEtiquet" Text="Codigo postal" />
                                <asp:TextBox ID="TxtCodPostal" runat="server" CssClass="form-control-sm heightCampo" MaxLength="30" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblBanco" runat="server" CssClass="LblEtiquet" Text="Banco" />
                                <asp:DropDownList ID="DdlBanco" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="LblNroCta" runat="server" CssClass="LblEtiquet" Text="No. cuenta" />
                                <asp:TextBox ID="TxtNroCta" runat="server" CssClass="form-control-sm heightCampo" MaxLength="30" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-md-3">
                                <br />
                                <asp:RadioButton ID="RdbCtaAhorr" runat="server" CssClass="LblEtiquet" Text="&nbsp ahorro" GroupName="ClasCta" Enabled="false" />&nbsp&nbsp
                                <asp:RadioButton ID="RdbCtaCte" runat="server" CssClass="LblEtiquet" Text="&nbsp corriente" GroupName="ClasCta" Enabled="false" />&nbsp&nbsp                                
                                <asp:RadioButton ID="RdbCtaNA" runat="server" CssClass="LblEtiquet" Text="&nbsp N/A" GroupName="ClasCta" Enabled="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-5">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:Label ID="LblPais" runat="server" CssClass="LblEtiquet" Text="pais" />
                                        <asp:DropDownList ID="DdlPais" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" OnTextChanged="DdlPais_TextChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:Label ID="LblEstado" runat="server" CssClass="LblEtiquet" Text="estad" />
                                        <asp:DropDownList ID="DdlEstado" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" OnTextChanged="DdlEstado_TextChanged" AutoPostBack="true"/>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-6">
                                        <asp:Label ID="LblCiudad" runat="server" CssClass="LblEtiquet" Text="ciudad" />
                                        <asp:DropDownList ID="DdlCiudad" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="LblSwift" runat="server" CssClass="LblEtiquet" Text="Swift No." />
                                        <asp:TextBox ID="TxtSwift" runat="server" CssClass="form-control-sm heightCampo" MaxLength="30" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="LblAba" runat="server" CssClass="LblEtiquet" Text="ABA" />
                                        <asp:TextBox ID="TxtAba" runat="server" CssClass="form-control-sm heightCampo" MaxLength="30" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="LblIVA" runat="server" CssClass="LblEtiquet" Text="IVA" />
                                        <asp:TextBox ID="TxtIVA" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" step="0.01" onkeypress="return Decimal(event);" TextMode="Number" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="LblObservac" runat="server" CssClass="LblEtiquet" Text="Observaciones" />
                                        <asp:TextBox ID="TxtObservac" runat="server" CssClass="form-control-sm heightCampo" MaxLength="200" Enabled="false" Width="100%" Height="50px" TextMode="MultiLine" />
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-12">
                                        <h6 class="TextoSuperior">
                                            <asp:Label ID="LblTitContactoDefecto" runat="server" Text="contacto por defecto" /></h6>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtNomContactDeft" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="TxtApellContactDeft" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-7">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitContacto" runat="server" Text="contactos" /></h6>

                                <asp:GridView ID="GrdContacto" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdContacto,Ppal"
                                    CssClass="DiseñoGrid table table-sm" GridLines="Both" Width="100%"
                                    OnRowCommand="GrdContacto_RowCommand" OnRowEditing="GrdContacto_RowEditing" OnRowUpdating="GrdContacto_RowUpdating"
                                    OnRowCancelingEdit="GrdContacto_RowCancelingEdit" OnRowDeleting="GrdContacto_RowDeleting" OnRowDataBound="GrdContacto_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="principal">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CkbPpalP" Checked='<%# Eval("Ppal").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CkbPpal" Checked='<%# Eval("Ppal").ToString()=="1" ? true : false %>' runat="server" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:CheckBox ID="CkbPpalPP" runat="server" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="nombre" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblNomP" Text='<%# Eval("Nombre") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtNom" Text='<%# Eval("Nombre") %>' runat="server" MaxLength="80" Width="100%" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNomPP" runat="server" MaxLength="80" Width="100%" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="apellido" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblApellP" Text='<%# Eval("Apellido") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtApell" Text='<%# Eval("Apellido") %>' runat="server" MaxLength="80" Width="100%" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtApellPP" runat="server" MaxLength="80" Width="100%" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="telefono" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblTelP" Text='<%# Eval("Telefono") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtTel" Text='<%# Eval("Telefono") %>' runat="server" MaxLength="40" Width="100%" TextMode="Phone" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtTelPP" runat="server" MaxLength="40" Width="100%" TextMode="Phone" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="correo" HeaderStyle-Width="35%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblMailP" Text='<%# Eval("Correo") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtMail" Text='<%# Eval("Correo") %>' runat="server" MaxLength="40" Width="100%" TextMode="Email" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtMailPP" runat="server" MaxLength="40" Width="100%" TextMode="Email" />
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
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExport" />
           <asp:AsyncPostBackTrigger ControlID ="DdlEstado" EventName ="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
