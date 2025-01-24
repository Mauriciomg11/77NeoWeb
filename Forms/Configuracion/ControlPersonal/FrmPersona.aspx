<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmPersona.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.ControlPersonal.FrmPersona" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Status</title>
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

        .MyCalendar {
            border: 1px solid #646464;
            background-color: Gray;
            color: Black;
            font-family: Arial;
            font-size: 14px;
            font-weight: bold;
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
            $('#<%=DdlArea.ClientID%>').chosen();
            $('#<%=DdlCargo.ClientID%>').chosen();
            $('#<%=DdlBusqPers.ClientID%>').chosen();
            $('#<%=DdlEmsa.ClientID%>').chosen();
            $('[id *=DdlLicenRFPP]').chosen();
            $('[id *=DdlNombrePP]').chosen();
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
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <br /><br />
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-1">
                                <asp:Label ID="LblCodUsu" runat="server" CssClass="LblEtiquet" Text="CodUsu" />
                                <asp:TextBox ID="TxtCodUsu" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblBusqPers" runat="server" CssClass="LblEtiquet" Text=" Consultar Persona" />
                                <asp:DropDownList ID="DdlBusqPers" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlBusqPers_TextChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblEmsa" runat="server" CssClass="LblEtiquet" Text=" Empresa" />
                                <asp:DropDownList ID="DdlEmsa" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-2">
                                <br />
                                <asp:CheckBox ID="CkbActivo" runat="server" CssClass="LblEtiquet" Text="Activo" Enabled="false" Font-Size="17px" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblCedul" runat="server" CssClass="LblEtiquet" Text="Ced" />
                                <asp:TextBox ID="TxtCedul" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="50" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblNombr" runat="server" CssClass="LblEtiquet" Text="Nom" />
                                <asp:TextBox ID="TxtNombr" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="80" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblApell" runat="server" CssClass="LblEtiquet" Text="Apell" Width="100%" />
                                <asp:TextBox ID="TxtApell" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="80" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblFechNac" runat="server" CssClass="LblEtiquet" Text="Fecha Nac" />
                                <asp:TextBox ID="TxtFechNac" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="90%" TextMode="Date" MaxLength="10" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblTelef" runat="server" CssClass="LblEtiquet" Text="Tel" />
                                <asp:TextBox ID="TxtTelef" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="40" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblDirec" runat="server" CssClass="LblEtiquet" Text="Dir" />
                                <asp:TextBox ID="TxtDirec" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="100" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblCorreoP" runat="server" CssClass="LblEtiquet" Text="CorrPer" />
                                <asp:TextBox ID="TxtCorreoP" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" TextMode="Email" MaxLength="80" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblCorreoEmsa" runat="server" CssClass="LblEtiquet" Text="CorrEmsa" Width="100%" />
                                <asp:TextBox ID="TxtCorreoEmsa" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" MaxLength="100" TextMode="Email" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblCelu" runat="server" CssClass="LblEtiquet" Text="Cel" />
                                <asp:TextBox ID="TxtCelu" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" MaxLength="40" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblArea" runat="server" CssClass="LblEtiquet" Text="area" />
                                <asp:DropDownList ID="DdlArea" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblCargo" runat="server" CssClass="LblEtiquet" Text="carg" />
                                <asp:DropDownList ID="DdlCargo" runat="server" CssClass="heightCampo" Width="100%" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblUsuario" runat="server" CssClass="LblEtiquet" Text="Usu" />
                                <asp:TextBox ID="TxtUsuario" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="90%" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnIngresar_Click" Text="nuevo" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnModificar_Click" Text="modificar" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnAsigUsu" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnAsigUsu_Click" Text="Asignar usuario" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnExportar" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnExportar_Click" Text="Exportar" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-7">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitLicencias" runat="server" Text="Licencias" /></h6>
                                <asp:GridView ID="GrdLicencias" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdLicenciaxPer"
                                    CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" AllowPaging="true" PageSize="8"
                                    OnRowCommand="GrdLicencias_RowCommand" OnRowEditing="GrdLicencias_RowEditing" OnRowUpdating="GrdLicencias_RowUpdating"
                                    OnRowCancelingEdit="GrdLicencias_RowCancelingEdit" OnRowDeleting="GrdLicencias_RowDeleting" OnRowDataBound="GrdLicencias_RowDataBound"
                                    OnPageIndexChanging="GrdLicencias_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Activo">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CkbActivoP" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CkbActivo" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:CheckBox ID="CkbActivoPP" runat="server" Checked="true" Enabled="false" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Licencia" HeaderStyle-Width="13%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblLicenRF" Text='<%# Eval("CodLicencia") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblLicen" Text='<%# Eval("CodLicencia") %>' runat="server" />
                                                <%-- <asp:TextBox ID="TxtLicenRF" Text='<%# Eval("CodIdLicencia") %>' runat="server" Width="100%" Enabled="false" />--%>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlLicenRFPP" runat="server" Width="100%" Height="28px" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Numero" HeaderStyle-Width="13%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblNum" Text='<%# Eval("Numero") %>' runat="server" Width="100%" TextMode="Number" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtNum" Text='<%# Eval("Numero") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtNumPP" runat="server" Width="100%" Text="0" TextMode="Number" step="0.01" onkeypress="return solonumeros(event);" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Vence" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFecVen" Text='<%# Eval("FechaExp") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtFecVen" Text='<%# Eval("FechaExpedicion") %>' runat="server" Width="100%" onkeypress="return Fecha(event);" TextMode="Date" />                                              
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtFecVenPP" runat="server" Width="100%" onkeypress="return Fecha(event);" TextMode="Date" />                                               
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Modelo" HeaderStyle-Width="18%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("ModeloLP") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtMod" Text='<%# Eval("ModeloLP") %>' runat="server" MaxLength="15" Width="100%" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtModPP" runat="server" MaxLength="15" Width="100%" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Especialidad" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("EspecialidadLP") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtEspec" Text='<%# Eval("EspecialidadLP") %>' runat="server" MaxLength="70" Width="100%" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtEspecPP" runat="server" MaxLength="70" Width="100%" />
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
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                </asp:GridView>
                            </div>
                            <div class="col-sm-5">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitCurso" runat="server" Text="Cursos" /></h6>
                                <asp:GridView ID="GrdCursos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdOperacionCursoXPer"
                                    CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" AllowPaging="true" PageSize="8"
                                    OnRowCommand="GrdCursos_RowCommand" OnRowEditing="GrdCursos_RowEditing" OnRowUpdating="GrdCursos_RowUpdating"
                                    OnRowCancelingEdit="GrdCursos_RowCancelingEdit" OnRowDeleting="GrdCursos_RowDeleting" OnRowDataBound="GrdCursos_RowDataBound"
                                    OnPageIndexChanging="GrdLicencias_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CkbActivoP" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="CkbActivo" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:CheckBox ID="CkbActivoPP" runat="server" Checked="true" Enabled="false" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="55%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblNombreP" Text='<%# Eval("Nombre") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LbNombre" Text='<%# Eval("Nombre") %>' runat="server" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlNombrePP" runat="server" Width="100%" Height="28px" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Vence" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFecVen" Text='<%# Eval("FechaVenc") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtFecVen" Text='<%# Eval("FechaVencDMY") %>' runat="server" Width="100%" onkeypress="return Fecha(event);" TextMode="Date" />                                                
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtFecVenPP" runat="server" Width="100%" onkeypress="return Fecha(event);" TextMode="Date"/>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="15%">
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
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1NuevoUsu" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitCrearusu" runat="server" Text="Asignar Usuario al Grupo de Mantenimiento" /></h6>
                    <asp:ImageButton ID="IbtCerrarCrearUsu" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarCrearusu_Click" ImageAlign="Right" />

                    <br />
                    <asp:Label ID="LblNomUsu" runat="server" CssClass="LblEtiquet" Text="Usuario" />
                    <asp:TextBox ID="TxtNomUsu" runat="server" CssClass="heightCampo" Width="15%" />
                    <asp:Button ID="BtnAsignarUsu" runat="server" CssClass="btn btn-success botones BtnEdicion" OnClick="BtnAsignarUsu_Click" Text="Asignar" />
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExportar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
