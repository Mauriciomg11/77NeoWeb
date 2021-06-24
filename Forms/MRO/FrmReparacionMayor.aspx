<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmReparacionMayor.aspx.cs" Inherits="_77NeoWeb.Forms.MRO.FrmReparacionMayor" %>

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

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 90%;
            margin-left: -45%;
            height: 85%;
            padding: 5px;
        }

        .CentrarRecurso {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 94%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -47%;
            /*determinamos una altura*/
            height: 85%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
            font-weight: bold;
            width: 8%;
            height: 27px;
        }

        .Font_btnSelect {
            font-size: 12px;
            font-stretch: condensed;
            width: 14%;
            height: 27px;
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
        function NumNEgativo(e) {

            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }
            if (key < 45 || key > 57) {
                return false;
            }
            else if (key == 46 || key == 47) {
                return false
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
        function Fecha(e) {

            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }
            if (key < 47 || key > 57) {
                return false;
            }
            return true;
        }
        function myFuncionddl() {
            $('#<%=DdlGrupo.ClientID%>').chosen();
            $('#<%=Ddltaller.ClientID%>').chosen();
            $('#<%=DdlModel.ClientID%>').chosen();
            $('#<%=DdlAta.ClientID%>').chosen();
            /*$('[id *=DdlContPNPP]').chosen();*/
            $('[id *=DdlPNPP]').chosen();
            $('[id *=DdlHK], [id *=DdlHKPP]').chosen();
            $('[id *=DdlCont],[id *=DdlContPP]').chosen();
            $('[id *=DdlHK], [id *=DdlHKPP]').chosen();
            $('[id *=DdlLicenRFPP]').chosen();
            $('[id *=DdlPNRFPP]').chosen();
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
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-7">
                                <asp:Button ID="BtnConsultar" runat="server" CssClass="btn btn-primary Font_btnSelect" Width="13%" OnClick="BtnConsultar_Click" OnClientClick="target ='';" Text="consultar" />
                                <asp:Button ID="BtnIngresar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="13%" OnClick="BtnIngresar_Click" OnClientClick="target ='';" Text="nuevo" />
                                <asp:Button ID="BtnModificar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="13%" OnClick="BtnModificar_Click" OnClientClick="target ='';" Text="modificar" />
                                <asp:Button ID="BtnImprimir" runat="server" CssClass="btn btn-primary Font_btnSelect" Width="13%" OnClick="BtnImprimir_Click" OnClientClick="target ='';" Text="imprimir" Visible="false" />
                                <asp:Button ID="BtnEliminar" runat="server" CssClass="btn btn-success Font_btnCrud" Width="13%" OnClick="BtnEliminar_Click" OnClientClick="target ='';" Text="eliminar" />
                                <asp:Button ID="BtnRecurso" runat="server" CssClass="btn btn-primary Font_btnSelect" Width="13%" OnClick="BtnRecurso_Click" OnClientClick="target ='';" Text="consultar" />
                                <asp:Button ID="BtnGenerarOT" runat="server" CssClass="btn btn-success Font_btnCrud" Width="13%" OnClick="BtnGenerarOT_Click" OnClientClick="target ='';" Text="nuevo" />
                            </div>
                            <div class="col-sm-5">
                                <asp:Button ID="BtnAK" runat="server" CssClass="btn btn-outline-primary" OnClick="BtnAK_Click" Width="30%" Font-Size="14px" Font-Bold="true" Text="Aeronave" OnClientClick="target ='';" />
                                <asp:Button ID="BtnPN" runat="server" CssClass="btn btn-outline-primary" OnClick="BtnPN_Click" Width="30%" Font-Size="14px" Font-Bold="true" Text="P/N" OnClientClick="target ='';" />
                                <asp:Button ID="BtnSN" runat="server" CssClass="btn btn-outline-primary" OnClick="BtnSN_Click" Width="30%" Font-Size="14px" Font-Bold="true" Text="S/N" OnClientClick="target ='';" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblCod" runat="server" CssClass="LblEtiquet" Text="Código:" Width="100%" />
                                <asp:TextBox ID="TxtId" runat="server" CssClass=" form-control-sm heightCampo" Enabled="false" Width="20%" />
                                <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control-sm heightCampo" Enabled="false" Width="35%" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblGrupo" runat="server" CssClass="LblEtiquet" Text="Grupo:" />
                                <asp:DropDownList ID="DdlGrupo" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" OnTextChanged="DdlGrupo_TextChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-5">
                                <asp:Label ID="LblDescrip" runat="server" CssClass="LblEtiquet" Text="Desripción:" />
                                <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" TextMode="MultiLine" MaxLength="254" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblAta" runat="server" CssClass="LblEtiquet" Text="Capítulo:" />
                                <asp:DropDownList ID="DdlAta" runat="server" CssClass="heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Label ID="LblDoc" runat="server" CssClass="LblEtiquet" Text="Documento:" />
                                <asp:TextBox ID="TxtDoc" runat="server" CssClass="form-control heightCampo" MaxLength="60" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblModel" runat="server" CssClass="LblEtiquet" Text="Modelo:" />
                                <asp:DropDownList ID="DdlModel" runat="server" CssClass="form-control heightCampo" Enabled="false" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblTaller" runat="server" CssClass="LblEtiquet" Text="Taller:" />
                                <asp:DropDownList ID="Ddltaller" runat="server" CssClass="heightCampo" Enabled="false" />
                            </div>
                        </div>
                        <table width="100%">
                            <tr>
                                <td width="70%">
                                    <asp:TextBox ID="TxtHistorico" runat="server" Width="35%" Height="20px" CssClass="form-control-sm" MaxLength="200" placeholder="Ingrese el reporte para el histórico" Enabled="false" />
                                    <asp:TextBox ID="TxtEstadoOT" runat="server" Width="30%" Height="20px" CssClass="form-control-sm TextoOTGenrda" placeholder="Estado O.T." Enabled="false" />
                                    <asp:TextBox ID="TxtMatric" runat="server" Width="10%" Height="20px" CssClass="form-control-sm TextoOTGenrda" placeholder="Matrícula" Enabled="false" />
                                    <asp:CheckBox ID="CkbBloqRec" runat="server" CssClass="LblEtiquet" Text="Bloquear Recurso" Enabled="false" ToolTip="Bloquea el recurso físico para que no sea editado" /></td>
                                <td width="28%"></td>
                            </tr>
                        </table>
                        <div id="Grids" class="row">
                            <div class="col-sm-8">
                                <asp:GridView ID="GrdAeron" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdContaSrvManto,Matricula, IdCodElem"
                                    CssClass=" GridControl DiseñoGrid table CsGridHK table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                                    OnRowCommand="GrdAeron_RowCommand" OnSelectedIndexChanged="GrdAeron_SelectedIndexChanged" OnRowEditing="GrdAeron_RowEditing"
                                    OnRowUpdating="GrdAeron_RowUpdating" OnRowCancelingEdit="GrdAeron_RowCancelingEdit"
                                    OnRowDeleting="GrdAeron_RowDeleting" OnRowDataBound="GrdAeron_RowDataBound" OnPageIndexChanging="GrdAeron_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Matrícula" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblHKP" Text='<%# Eval("Matricula") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DdlHK" runat="server" Width="100%" Height="28px" Enabled="false" CssClass="heightCampo" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updHKPP">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="DdlHKPP" runat="server" Width="100%" Height="28px" CssClass="heightCampo" OnTextChanged="DdlHKPP_TextChanged" AutoPostBack="true" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCont" Text='<%# Eval("CodContador") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DdlCont" runat="server" Width="100%" Height="28px" Enabled="false" CssClass="heightCampo" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlContHKPP" runat="server" Width="100%" Height="28px" CssClass="heightCampo" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frecuen." HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFrec" Text='<%# Eval("Frecuencia") %>' runat="server" Width="100%" TextMode="Number" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtFrec" Text='<%# Eval("Frecuencia") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtFrecPP" runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="5%">
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
                                <asp:GridView ID="GrdPN" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodidcodSrvPn,CodIdContadorPn,PN"
                                    CssClass=" GridControl DiseñoGrid table CsGridHK table-sm" GridLines="Both" AllowPaging="true" PageSize="8" Visible="false"
                                    OnRowCommand="GrdPN_RowCommand" OnSelectedIndexChanged="GrdPN_SelectedIndexChanged" OnRowEditing="GrdPN_RowEditing"
                                    OnRowUpdating="GrdPN_RowUpdating" OnRowCancelingEdit="GrdPN_RowCancelingEdit"
                                    OnRowDeleting="GrdPN_RowDeleting" OnRowDataBound="GrdPN_RowDataBound" OnPageIndexChanging="GrdPN_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPNP" Text='<%# Eval("Pn") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblPN" Text='<%# Eval("Pn") %>' runat="server" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpDPNPP">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="DdlPNPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlPNPP_TextChanged" AutoPostBack="true" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="DdlPNPP" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblDescPn" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtDescPn" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtDescPnPP" runat="server" Width="100%" Enabled="false" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblContPNP" Text='<%# Eval("CodContador") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblContPN" Text='<%# Eval("CodContador") %>' runat="server" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:DropDownList ID="DdlContPNPP" runat="server" Width="100%" Height="28px" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frecuen." HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFrecPN" Text='<%# Eval("Frecuencia") %>' runat="server" Width="100%" TextMode="Number" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtFrecPN" Text='<%# Eval("Frecuencia") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtFrecPNPP" runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="5%">
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
                                <asp:GridView ID="GrdSN" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" DataKeyNames="CodIdContaSrvManto,CodElem,Matricula,IdCodElem,Pn,Sn"
                                    CssClass=" GridControl DiseñoGrid CsGridHK table-sm" GridLines="Both" AllowPaging="true" PageSize="8" Visible="false"
                                    OnSelectedIndexChanged="GrdSN_SelectedIndexChanged" OnRowEditing="GrdSN_RowEditing"
                                    OnRowUpdating="GrdSN_RowUpdating" OnRowCancelingEdit="GrdSN_RowCancelingEdit" OnRowDeleting="GrdSN_RowDeleting"
                                    OnRowDataBound="GrdSN_RowDataBound" OnPageIndexChanging="GrdSN_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="40%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblPNP" Text='<%# Eval("Pn") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblPN" Text='<%# Eval("Pn") %>' runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="40%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblSNP" Text='<%# Eval("SN") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblSN" Text='<%# Eval("SN") %>' runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cont." HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblContP" Text='<%# Eval("CodContador") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblCont" Text='<%# Eval("CodContador") %>' runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frec." HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblFrec" Text='<%# Eval("Frecuencia") %>' runat="server" Width="100%" TextMode="Number" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtFrec" Text='<%# Eval("Frecuencia") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                                <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
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
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                </asp:GridView>
                            </div>
                            <div class="col-sm-4">
                                <asp:GridView ID="GrdAdj" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdAdjuntos,Ruta"
                                    CssClass="GridControl DiseñoGrid TablaAdj table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                                    OnRowCommand="GrdAdj_RowCommand" OnRowEditing="GrdAdj_RowEditing"
                                    OnRowUpdating="GrdAdj_RowUpdating" OnRowCancelingEdit="GrdAdj_RowCancelingEdit"
                                    OnRowDeleting="GrdAdj_RowDeleting" OnRowDataBound="GrdAdj_RowDataBound" OnPageIndexChanging="GrdAdj_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TxtDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="TxtDescPP" runat="server" Width="100%" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nombre del archivo" HeaderStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updDown">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="lnkDownload" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Ruta") %>'
                                                            CommandName="Download" Text='<%# Eval("Ruta") %>' />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="lnkDownload" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:FileUpload ID="FileUp" runat="server" Width="100%" Font-Size="7px" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:FileUpload ID="FileUpPP" runat="server" Width="100%" Font-Size="7px" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
                                                <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updFUMod">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="IbtUpdateAdj" CssClass="BotonUpdateGrid" ImageUrl="~/images/Save.png" runat="server" CommandName="Update" ToolTip="Actualizar" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="IbtUpdateAdj" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                <asp:ImageButton ID="IbtCancel" CssClass="BotonCancelGrid" ImageUrl="~/images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancelar" />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updFU">
                                                    <ContentTemplate>
                                                        <asp:ImageButton ID="IbtAddNew" CssClass="BotonNewGrid" ImageUrl="~/images/AddNew.png" runat="server" CommandName="AddNew" ToolTip="Nuevo" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="IbtAddNew" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle CssClass="GridFooterStyle" />
                                    <HeaderStyle CssClass="GridCabecera" />
                                    <RowStyle CssClass="GridRowStyle" />
                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Busq" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LbltitBusq" runat="server" Text="Opciones de búsqueda " />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                    <div class="CentrarBusq DivMarco">
                        <asp:Table ID="TblBusqHK" runat="server" class="TablaBusqueda" Visible="false" Width="10%">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:RadioButton ID="RdbBusqDes" runat="server" GroupName="BusqA" CssClass="LblTextoBusq" Text="Descripción" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <asp:Table ID="TblBusqPN" runat="server" class="TablaBusqueda" Visible="false" Width="20%">
                            <asp:TableRow>
                                <asp:TableCell Width="10%">
                                    <asp:RadioButton ID="RdbBusqDesPN" runat="server" GroupName="BusqP" CssClass="LblTextoBusq" Text="Descripción" />
                                </asp:TableCell>
                                <asp:TableCell Width="10%">
                                    <asp:RadioButton ID="RdbBusqPnPN" runat="server" GroupName="BusqP" CssClass="LblTextoBusq" Text="&nbsp P/N" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <asp:Table ID="TblBusqSN" runat="server" class="TablaBusqueda" Visible="false" Width="30%">
                            <asp:TableRow>
                                <asp:TableCell Width="10%">
                                    <asp:RadioButton ID="RdbBusqDesSN" runat="server" GroupName="BusqS" CssClass="LblTextoBusq" Text="Descripción" />
                                </asp:TableCell>
                                <asp:TableCell Width="10%">
                                    <asp:RadioButton ID="RdbBusqPnSN" runat="server" GroupName="BusqS" CssClass="LblTextoBusq" Text="&nbsp P/N" />
                                </asp:TableCell>
                                <asp:TableCell Width="10%">
                                    <asp:RadioButton ID="RdbBusqSnSN" runat="server" GroupName="BusqS" CssClass="LblTextoBusq" Text="&nbsp S/N" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <table class="TablaBusqueda">
                            <tr>
                                <td>
                                    <asp:Label ID="LblBusq" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                                <td>
                                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="550px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar"></asp:TextBox></td>
                                <td>
                                    <asp:ImageButton ID="IbtConsultar" runat="server" ImageUrl="~/images/FindV2.png" ToolTip="Consultar" CssClass="BtnImagenBusqueda" OnClick="IbtConsultar_Click" /></td>
                            </tr>
                        </table>
                        <br />
                        <div class="CentrarGrid pre-scrollable">
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false"
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
                                    <asp:TemplateField HeaderText="Id">
                                        <ItemTemplate>
                                            <asp:Label ID="LblId" Text='<%# Eval("IdSrvManto") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="codigo">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodServicioManto") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripcion">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NroDocumento">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("NroDocumento") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PN">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("PN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SN">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("SN") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Desc elemento">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Descripcion_PN") %>' runat="server" />
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
                <asp:View ID="Vw2Recurso" runat="server">
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitRecursoLice" runat="server" Text="Recurso Físico y Licencias" /></h6>
                    <asp:ImageButton ID="IbtCloseRecurso" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCloseRecurso_Click" />
                    <div class="CentrarRecurso DivMarco">
                        <div id="Partes" class="row">
                            <div class="col-sm-12">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitRecPartes" runat="server" Text="Partes " /></h6>
                                <div class="CentrarGrid pre-scrollable">
                                    <asp:GridView ID="GrdRecursoF" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true"
                                        DataKeyNames="CodidDetElemPlanInstrumento"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both"
                                        OnRowCommand="GrdRecursoF_RowCommand" OnRowEditing="GrdRecursoF_RowEditing"
                                        OnRowUpdating="GrdRecursoF_RowUpdating" OnRowCancelingEdit="GrdRecursoF_RowCancelingEdit"
                                        OnRowDeleting="GrdRecursoF_RowDeleting" OnRowDataBound="GrdRecursoF_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Parte número" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblPn" Text='<%# Eval("PN") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtPNRF" Text='<%# Eval("PN") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="DdlPNRFPP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlPNRFPP_TextChanged" />
                                                    <asp:TextBox ID="TxtPNRFPP" runat="server" MaxLength="80" Width="100%" Enabled="false" Visible="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Referencia" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtRefRF" Text='<%# Eval("CodReferencia") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtDesRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtDesRFPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cantidad" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblCantRF" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtCantRF" Text='<%# Eval("Cantidad") %>' runat="server" Width="100%" step="0.01" onkeypress="return Decimal(event);" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtCantRFPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fase" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblFaseRF" Text='<%# Eval("NumFase") %>' runat="server" Width="100%" TextMode="Number" step="0" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtFaseRF" Text='<%# Eval("NumFase") %>' runat="server" Width="100%" step="0" onkeypress="return solonumeros(event);" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtFaseRFPP" runat="server" Width="100%" Text="0" TextMode="Number" step="0" onkeypress="return solonumeros(event);" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Condicional" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbCondicP" Checked='<%# Eval("Condicional").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="CkbCondic" Checked='<%# Eval("Condicional").ToString()=="1" ? true : false %>' runat="server" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:CheckBox ID="CkbCondicPP" runat="server" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Unidad Medida" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodUndMedR") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtUMRF" Text='<%# Eval("CodUndMedR") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Tipo") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtTipoRF" Text='<%# Eval("Tipo") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
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
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div id="Licencia" class="row">
                            <div class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitLicen" runat="server" Text="horas estimadas por licencia " /></h6>
                                <div class="CentrarGrid pre-scrollable">
                                    <asp:GridView ID="GrdLicen" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdSrvLic,CodIdLicencia"
                                        CssClass="DiseñoGrid table table-sm SubTituloLicencia" GridLines="Both"
                                        OnRowCommand="GrdLicen_RowCommand" OnRowEditing="GrdLicen_RowEditing"
                                        OnRowUpdating="GrdLicen_RowUpdating" OnRowCancelingEdit="GrdLicen_RowCancelingEdit"
                                        OnRowDeleting="GrdLicen_RowDeleting" OnRowDataBound="GrdLicen_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Licencia" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblLicenRF" Text='<%# Eval("CodLicencia") %>' runat="server" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtLicenRF" Text='<%# Eval("CodLicencia") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="DdlLicenRFPP" runat="server" Width="100%" Height="28px" AutoPostBack="true" OnTextChanged="DdlLicenRFPP_TextChanged"></asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="50%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtDesLiRF" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtDesLiRFPP" runat="server" MaxLength="240" Width="100%" Enabled="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tiempo Estimado" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblTieEstRF" Text='<%# Eval("TiempoEstimado") %>' runat="server" Width="100%" TextMode="Number" step="0.01" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtTieEstRF" Text='<%# Eval("TiempoEstimado") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtTieEstRFPP" runat="server" Width="100%" Text="1" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterStyle-Width="7%">
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
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
