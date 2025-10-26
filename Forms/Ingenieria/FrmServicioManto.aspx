<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmServicioManto.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmServicioManto" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
    <title>ST</title>
    <style type="text/css">
        .CssTblPpal {
            position: absolute;
            width: 103%;
            top: 150px
        }

        .TablaCampos {
            width: 95%;
        }

        .TablaFase {
            position: absolute;
            text-align: left;
            width: 15%;
            font-size: 100%;
        }

        .TablaCheck {
            position: absolute;
            text-align: left;
            width: 50%;
            font-size: 100%;
        }

        .TablaBotones {
            width: 90%;
            height: 1%;
        }

        .TablaAK {
            width: 100%;
        }

        .TablaHKAsig {
            width: 110%;
            height: 65%;
            margin-top: 0px;
        }

        .TituloHKAsig {
            background-color: cadetblue; /*bg-info text-center*/
            text-align: center;
            color: aliceblue;
            width: 100%;
        }

        .TablaAdj {
            position: absolute;
            width: 27%;
            right: 42px;
        }

        .CsGridHK {
            position: absolute;
            width: 70%;
            height: 30%;
            margin-top: 0px;
        }

        .Campos {
            Height: 27px;
            Width: 100%;
            font-size: 12px;
        }

        .DivRecursoF {
            position: absolute;
            width: 95%;
            height: 45%;
        }

        .DivLicencia {
            width: 50%;
            height: 45%;
        }

        .SubTituloLicencia {
            width: 60%;
        }

        .TextoOTGenrda {
            font-size: 12px;
            font-weight: bold;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }

        .CentrarGrdCntdrInic {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 50%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -25%;
            /*determinamos una altura*/
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Script" ContentPlaceHolderID="EncScriptDdl" runat="server">
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
            $('#<%=DdlTipo.ClientID%>').chosen();
            $('#<%=DdlBusq.ClientID%>').chosen();
            $('[id *=DdlPN], [id *=DdlPNPP], [id *=DdlCont]').chosen();
            $('[id *=DdlMatAsigPP]').chosen();
            $('[id *=DdlLicenRFPP]').chosen();
            $('[id *=DdlHK], [id *=DdlHKPP], [id *=DdlContPP], [id *=DdlMatPP]').chosen();

        }
    </script>
</asp:Content>
<asp:Content ID="TituloPagina" ContentPlaceHolderID="TituloPagina" runat="server">
    <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:MultiView ID="MultVw" runat="server">
        <asp:View ID="Vw0Datos" runat="server">
            <asp:Panel ID="PnlCampos" runat="server" Width="100%">
                <table class="CssTblPpal">
                    <tr id="Busqueda">
                        <td>
                            <asp:UpdatePanel ID="UpPnlBusq" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="TablaBotones">
                                        <tr>
                                            <td width="50%">
                                                <asp:DropDownList ID="DdlBusq" runat="server" CssClass="Campos" OnTextChanged="DdlBusq_TextChanged" AutoPostBack="true" />
                                            </td>
                                            <td width="5%">
                                                <asp:ImageButton ID="IbtAdd" runat="server" CssClass="BtnImagenAdd" ImageUrl="~/images/AddNew.png" OnClick="IbtAdd_Click" ToolTip="Ingresar" /></td>
                                            <td width="5%">
                                                <asp:ImageButton ID="IbtUpdate" runat="server" CssClass="BtnImagenUpdate" ImageUrl="~/images/Edit.png" OnClick="IbtUpdate_Click" ToolTip="Modificar" AutoPostBack="false" /></td>
                                            <td width="5%">
                                                <asp:ImageButton ID="IbtFind" runat="server" CssClass="BtnImagenFind" ImageUrl="~/images/FindV1.png" OnClick="IbtFind_Click" ToolTip="Otras consultas" /></td>
                                            <td width="5%">
                                                <asp:ImageButton ID="IbtPrint" runat="server" CssClass="BtnImagenPrint" ImageUrl="~/images/PrintV1.png" OnClick="IbtPrint_Click" ToolTip="Imprimir" /></td>
                                            <td width="5%">
                                                <asp:ImageButton ID="IbtDelete" runat="server" CssClass="BtnImagenDelete" ImageUrl="~/images/deleteV1.png" OnClick="IbtDelete_Click" ToolTip="Eliminar" OnClientClick="return confirm('¿Desea eliminar el registro?');" /></td>
                                            <td width="5%">
                                                <asp:ImageButton ID="IbtRecurso" runat="server" CssClass="BtnImagenManoObra" ImageUrl="~/images/ManoObraV1.png" OnClick="IbtRecurso_Click" ToolTip="Programación de recurso y mano de obra" /></td>
                                            <td width="5%">
                                                <asp:ImageButton ID="IbtGenerOT" runat="server" CssClass="BtnImagenGenerarOT" ImageUrl="~/images/WorkOrder.png" OnClick="IbtGenerOT_Click" ToolTip="Generar orden de trabajo" /></td>
                                            <td>
                                                <asp:Button ID="BtnAK" runat="server" CssClass="btn btn-outline-primary" OnClick="BtnAK_Click" Font-Size="14px" Font-Bold="true" Text="Aeronave" /></td>
                                            <td>
                                                <asp:Button ID="BtnPN" runat="server" CssClass="btn btn-outline-primary" OnClick="BtnPN_Click" Font-Size="14px" Font-Bold="true" Text="P/N" /></td>
                                            <td>
                                                <asp:Button ID="BtnSN" runat="server" CssClass="btn btn-outline-primary" OnClick="BtnSN_Click" Font-Size="14px" Font-Bold="true" Text="S/N" /></td>
                                            <td width="15%">
                                                <asp:CheckBox ID="CkbVisuStat" runat="server" CssClass="LblEtiquet" Text="Visualizar en Status" Enabled="false" /></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DdlBusq" EventName="TextChanged" />
                                    <asp:PostBackTrigger ControlID="IbtFind" />
                                    <asp:PostBackTrigger ControlID="IbtPrint" />
                                    <asp:PostBackTrigger ControlID="IbtRecurso" />
                                    <asp:PostBackTrigger ControlID="IbtGenerOT" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="Campos">
                        <td>
                            <asp:UpdatePanel ID="UpPnlCampos" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table class="TablaCampos table table-sm">
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblCod" runat="server" CssClass="LblEtiquet" Text="Código:" /></td>
                                            <td>
                                                <asp:TextBox ID="TxtId" runat="server" CssClass=" form-control-sm Campos" Enabled="false" Width="30%" />
                                                <asp:TextBox ID="TxtCod" runat="server" CssClass="form-control-sm Campos" Enabled="false" Width="65%" /></td>
                                            <td>
                                                <asp:Label ID="LblDescrip" runat="server" CssClass="LblEtiquet" Text="Desripción:" /></td>
                                            <td colspan="3">
                                                <asp:TextBox ID="TxtDesc" runat="server" CssClass="form-control Campos" Enabled="false" TextMode="MultiLine" MaxLength="254"></asp:TextBox></td>
                                            <td rowspan="5" width="27%" align="right">
                                                <table class="TablaHKAsig">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="BtnConfigContdrInic" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnConfigContdrInic_Click" OnClientClick="target ='';" Text="Configurar contador / frecuencia inicial" />
                                                            <h6 class="TextoSuperior">
                                                                <asp:Label ID="LblAkAsing" runat="server" Text="Aeronaves Asingadas" Visible="false" /></h6>
                                                            <asp:GridView ID="GrdHKAsig" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdSrvMantoAeronave,CodAeronave"
                                                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="3" Visible="false"
                                                                OnRowCommand="GrdHKAsig_RowCommand" OnRowDeleting="GrdHKAsig_RowDeleting"
                                                                OnRowDataBound="GrdHKAsig_RowDataBound" OnPageIndexChanging="GrdHKAsig_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Matrícula" HeaderStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                            <asp:Label Text='<%# Eval("Matricula") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:DropDownList ID="DdlMatAsigPP" runat="server" Width="100%" Height="28px" />
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Modelo">
                                                                        <ItemTemplate>
                                                                            <asp:Label Text='<%# Eval("NomModelo") %>' runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField FooterStyle-Width="30px">
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
                                                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="LblHoriz" runat="server" CssClass="LblEtiquet" Text="Horizonte" ToolTip="Horizonte de apertura" /></td>
                                            <td>
                                                <asp:TextBox ID="TxtHoriz" runat="server" CssClass="Campos" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" Width="25%" Font-Size="9px" />
                                                <asp:Label ID="Label2" runat="server" CssClass="LblEtiquet" Text="%" />
                                                <asp:Label ID="LblCumplimi" runat="server" CssClass="LblEtiquet" Text="Cumplimiento" />
                                            </td>
                                            <td>
                                                <asp:Label ID="LblGrupo" runat="server" CssClass="LblEtiquet" Text="Grupo:" /></td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="DdlGrupo" runat="server" CssClass="Campos" Enabled="false" OnTextChanged="DdlGrupo_TextChanged" AutoPostBack="true" /></td>
                                            <td align="left">
                                                <table class="TablaFase table-responsive">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblEtapa" runat="server" CssClass="LblEtiquet" Text="Etapa" /></td>
                                                        <td>
                                                            <asp:TextBox ID="TxtEtapa" runat="server" CssClass=" Campos" Enabled="false" Width="40%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" />
                                                            <asp:TextBox ID="TxtActual" runat="server" CssClass=" Campos" Enabled="false" Width="30%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="LblDoc" runat="server" CssClass="LblEtiquet" Text="Documento:" />
                                                <asp:TextBox ID="TxtDoc" runat="server" CssClass=" form-control Campos" MaxLength="300" Enabled="false" /></td>
                                            <td>
                                                <asp:Label ID="LblRefOT" runat="server" CssClass="LblEtiquet" Text="Refer. OT:" ToolTip="Referencia Orden de Trabajo" />
                                                <asp:TextBox ID="TxtRefOT" runat="server" CssClass=" form-control Campos" MaxLength="60" Enabled="false" />
                                            </td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td width="7%">
                                                <asp:Label ID="LblModel" runat="server" CssClass="LblEtiquet" Text="Modelo:" /></td>
                                            <td width="13%">
                                                <asp:DropDownList ID="DdlModel" runat="server" CssClass="form-control  Campos" Enabled="false" /></td>
                                            <td width="5%">
                                                <asp:Label ID="LblTaller" runat="server" CssClass="LblEtiquet" Text="Taller:" /></td>
                                            <td width="25%">
                                                <asp:DropDownList ID="Ddltaller" runat="server" CssClass="Campos" Enabled="false" /></td>
                                            <td colspan="2">
                                                <table class="TablaCheck table-responsive">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="CkbAD" runat="server" CssClass="LblEtiquet" Text="AD" Enabled="false" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="CkbSB" runat="server" CssClass="LblEtiquet" Text="SB" Enabled="false" /></td>
                                                        <td>
                                                            <asp:CheckBox ID="CkbAplSub" runat="server" CssClass="LblEtiquet" Text="Aplica SubComp." Enabled="false" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LblAta" runat="server" CssClass="LblEtiquet" Text="Capítulo:" /></td>
                                                        <td width="20%">
                                                            <asp:DropDownList ID="DdlAta" runat="server" CssClass="Campos" Enabled="false" /></td>
                                                        <td width="7%">
                                                            <asp:Label ID="LblSubAta" runat="server" CssClass="LblEtiquet" Text="Sub Ata:" /></td>
                                                        <td>
                                                            <asp:TextBox ID="TxtSubAta" runat="server" CssClass="form-control Campos" Enabled="false" onkeypress="return solonumeros(event);" OnTextChanged="TxtSubAta_TextChanged" AutoPostBack="true" /></td>
                                                        <td width="10%">
                                                            <asp:Label ID="LblConsecAta" runat="server" CssClass="LblEtiquet" Text="Consec. Ata:" /></td>
                                                        <td>
                                                            <asp:TextBox ID="TxtConsAta" runat="server" CssClass="form-control Campos" Enabled="false" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" /></td>
                                                        <td>
                                                            <asp:Label ID="LblTipo" runat="server" CssClass="LblEtiquet" Text="Tipo:" /></td>
                                                        <td width="30%">
                                                            <asp:DropDownList ID="DdlTipo" runat="server" CssClass="Campos" Enabled="false" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6"></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DdlGrupo" EventName="TextChanged" />
                                    <asp:PostBackTrigger ControlID="BtnConfigContdrInic" />
                                    <asp:PostBackTrigger ControlID="IbtUpdate" />
                                    <asp:PostBackTrigger ControlID="IbtAdd" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr id="Detalle">
                        <td>
                            <asp:UpdatePanel ID="UpPnlPN" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td width="70%">
                                                <asp:TextBox ID="TxtHistorico" runat="server" Width="35%" Height="20px" CssClass="form-control-sm" MaxLength="200" placeholder="Ingrese el reporte para el histórico" Enabled="false" />
                                                <asp:TextBox ID="TxtEstadoOT" runat="server" Width="30%" Height="20px" CssClass="form-control-sm TextoOTGenrda" placeholder="Estado O.T." Enabled="false" />
                                                <asp:TextBox ID="TxtMatric" runat="server" Width="10%" Height="20px" CssClass="form-control-sm TextoOTGenrda" placeholder="Matrícula" Enabled="false" />
                                                <asp:CheckBox ID="CkbBloqRec" runat="server" CssClass="LblEtiquet" Text="Bloquear Recurso" Enabled="false" ToolTip="Bloquea el recurso físico para que no sea editado" />
                                            </td>
                                            <td width="28%"></td>
                                        </tr>
                                        <tr id="DetallePN-Adjunto">
                                            <td width="70%">
                                                <asp:GridView ID="GrdAeron" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodIdContaSrvManto,Matricula, IdCodElem"
                                                    CssClass=" GridControl DiseñoGrid table CsGridHK table-sm" GridLines="Both" AllowPaging="true" PageSize="2"
                                                    OnRowCommand="GrdAeron_RowCommand" OnSelectedIndexChanged="GrdAeron_SelectedIndexChanged" OnRowEditing="GrdAeron_RowEditing"
                                                    OnRowUpdating="GrdAeron_RowUpdating" OnRowCancelingEdit="GrdAeron_RowCancelingEdit"
                                                    OnRowDeleting="GrdAeron_RowDeleting" OnRowDataBound="GrdAeron_RowDataBound" OnPageIndexChanging="GrdAeron_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Matrícula" HeaderStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblHKP" Text='<%# Eval("Matricula") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="DdlHK" runat="server" Width="100%" Height="28px" Enabled="false" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updHKPP">
                                                                    <ContentTemplate>
                                                                        <asp:DropDownList ID="DdlHKPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlHKPP_TextChanged" AutoPostBack="true" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Contador" HeaderStyle-Width="12%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblCont" Text='<%# Eval("CodContador") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:DropDownList ID="DdlCont" runat="server" Width="100%" Height="28px" Enabled="false" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="DdlContHKPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlContHKPP_TextChanged" AutoPostBack="true" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Frec. Inicial" HeaderStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblFrecIni" Text='<%# Eval("FrecuenciaInicial") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtFrecIni" Text='<%# Eval("FrecuenciaInicial") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="TxtFrecIniPP" runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" />
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
                                                        <asp:TemplateField HeaderText="Extensión">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblExt" Text='<%# Eval("Extension") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtExt" Text='<%# Eval("Extension") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return NumNEgativo(event);" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="TxtExtPP" runat="server" Width="100%" onkeypress="return NumNEgativo(event);" Text="0" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Frec. Actual" HeaderStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Eval("Frec") %>' runat="server" Width="100%" Enabled="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nro Días" HeaderStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblNumDia" Text='<%# Eval("NroDias") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtNumDia" Text='<%# Eval("NroDias") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="TxtNumDiaPP" runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Exten. Días">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblExtDia" Text='<%# Eval("ExtensionDias") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtExtDia" Text='<%# Eval("ExtensionDias") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return NumNEgativo(event);" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="TxtExtDiaPP" runat="server" Width="100%" onkeypress="return NumNEgativo(event);" Text="0" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fecha Inicial" HeaderStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblFecVen" Text='<%# Eval("FVFormat") %>' runat="server" Width="100%" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtFecVen" Text='<%# Eval("FechaVencimiento") %>' runat="server" Width="100%" onkeypress="return Fecha(event);" TextMode="Date" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="TxtFecVenPP" runat="server" Width="100%" onkeypress="return Fecha(event);" TextMode="Date" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reset" HeaderStyle-Width="7%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CkResetP" Checked='<%# Eval("Resetear").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:CheckBox ID="CkbReset" Checked='<%# Eval("Resetear").ToString()=="1" ? true : false %>' runat="server" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:CheckBox ID="CkbResetPP" runat="server" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Histórico" HeaderStyle-Width="7%">
                                                            <EditItemTemplate>
                                                                <asp:CheckBox ID="CkbHist" runat="server" />
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
                                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                                </asp:GridView>
                                                <asp:GridView ID="GrdPN" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodidcodSrvPn,CodIdContadorPn,PN"
                                                    CssClass=" GridControl DiseñoGrid table CsGridHK table-sm" GridLines="Both" AllowPaging="true" PageSize="4" Visible="false"
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
                                                                <asp:DropDownList ID="DdlContPNPP" runat="server" Width="100%" Height="28px" OnTextChanged="DdlContPNPP_TextChanged" AutoPostBack="true" />
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
                                                        <asp:TemplateField HeaderText="Nro Días" HeaderStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblNumDiaPN" Text='<%# Eval("NroDias") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtNumDiaPN" Text='<%# Eval("NroDias") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="TxtNumDiaPNPP" runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reset" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CkResetP" Checked='<%# Eval("Resetear").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:CheckBox ID="CkbReset" Checked='<%# Eval("Resetear").ToString()=="1" ? true : false %>' runat="server" />
                                                            </EditItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:CheckBox ID="CkbResetPP" runat="server" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField FooterStyle-Width="20%">
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
                                                    CssClass=" GridControl DiseñoGrid CsGridHK table-sm" GridLines="Both" AllowPaging="true" PageSize="4" Visible="false"
                                                    OnSelectedIndexChanged="GrdSN_SelectedIndexChanged" OnRowEditing="GrdSN_RowEditing"
                                                    OnRowUpdating="GrdSN_RowUpdating" OnRowCancelingEdit="GrdSN_RowCancelingEdit" OnRowDeleting="GrdSN_RowDeleting"
                                                    OnRowDataBound="GrdSN_RowDataBound" OnPageIndexChanging="GrdSN_PageIndexChanging">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="P/N" HeaderStyle-Width="20%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblPNP" Text='<%# Eval("Pn") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:Label ID="LblPN" Text='<%# Eval("Pn") %>' runat="server" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="S/N" HeaderStyle-Width="12%">
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
                                                        <asp:TemplateField HeaderText="Frec. Inicial" HeaderStyle-Width="6%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblFrecIni" Text='<%# Eval("FrecuenciaInicial") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtFrecIni" Text='<%# Eval("FrecuenciaInicial") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
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
                                                        <asp:TemplateField HeaderText="Ext." HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblExt" Text='<%# Eval("Extension") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtExt" Text='<%# Eval("Extension") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return NumNEgativo(event);" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Frec. Actual" HeaderStyle-Width="6%">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Eval("Frec") %>' runat="server" Width="100%" Enabled="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Nro Días" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblNumDia" Text='<%# Eval("NroDias") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtNumDia" Text='<%# Eval("NroDias") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Exten. Días" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblExtDia" Text='<%# Eval("ExtensionDias") %>' runat="server" Width="100%" TextMode="Number" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtExtDia" Text='<%# Eval("ExtensionDias") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return NumNEgativo(event);" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fecha Inicial" HeaderStyle-Width="13%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LblFecVen" Text='<%# Eval("FVFormat") %>' runat="server" Width="100%" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtFecVenSN" Text='<%# Eval("FechaVencimiento") %>' runat="server" Width="100%" onkeypress="return Fecha(event);" TextMode="Date" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Reset" HeaderStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CkResetP" Checked='<%# Eval("Resetear").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:CheckBox ID="CkbReset" Checked='<%# Eval("Resetear").ToString()=="1" ? true : false %>' runat="server" />
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Hist." HeaderStyle-Width="5%">
                                                            <EditItemTemplate>
                                                                <asp:CheckBox ID="CkbHist" runat="server" />
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
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle CssClass="GridFooterStyle" />
                                                    <HeaderStyle CssClass="GridCabecera" />
                                                    <RowStyle CssClass="GridRowStyle" />
                                                    <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                                                </asp:GridView>
                                            </td>
                                            <td width="25%">
                                                <asp:GridView ID="GrdAdj" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdAdjuntos,Ruta"
                                                    CssClass="GridControl DiseñoGrid TablaAdj table-sm" GridLines="Both" AllowPaging="true" PageSize="3"
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
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlBusq" runat="server" Visible="false">
                <br />
                <br />
                <h6 class="TextoSuperior">
                    <asp:Label ID="LbltitBusq" runat="server" CssClass="LblEtiquet" Text="Opciones de búsqueda" /></h6>
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
                            <asp:RadioButton ID="RdbBusqDesPN" runat="server" GroupName="BusqP" CssClass="LblTextoBusq" Text="Descripción" Checked="true" />
                        </asp:TableCell>
                        <asp:TableCell Width="10%">
                            <asp:RadioButton ID="RdbBusqPnPN" runat="server" GroupName="BusqP" CssClass="LblTextoBusq" Text="&nbsp P/N" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="TblBusqSN" runat="server" class="TablaBusqueda" Visible="false" Width="30%">
                    <asp:TableRow>
                        <asp:TableCell Width="10%">
                            <asp:RadioButton ID="RdbBusqDesSN" runat="server" GroupName="BusqS" CssClass="LblTextoBusq" Text="Descripción" Checked="true" />
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
                        <td>
                            <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" /></td>
                    </tr>
                </table>
                <div class="DivGrid DivContendorGrid">
                    <asp:GridView ID="GrdBusq" runat="server" AutoGenerateColumns="False" EmptyDataText="No existen registros ..!" DataKeyNames="IdSrvManto"
                        CssClass="GridControl DiseñoGrid table" GridLines="Both" AllowPaging="true" PageSize="7"
                        OnSelectedIndexChanged="GrdBusq_SelectedIndexChanged" OnPageIndexChanging="GrdBusq_PageIndexChanging">
                        <FooterStyle CssClass="GridFooterStyle" />
                        <HeaderStyle CssClass="GridCabecera" />
                        <RowStyle CssClass="GridRowStyle" />
                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        <Columns>
                            <asp:CommandField HeaderText="Asignar" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                            <asp:TemplateField HeaderText="Id">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("IdSrvManto") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Codigo">
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
                            <asp:TemplateField HeaderText="Descripcion_PN">
                                <ItemTemplate>
                                    <asp:Label Text='<%# Eval("Descripcion_PN") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" FirstPageText="Primero" LastPageText="Último" />
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID="PnlRecursos" runat="server" Visible="false">
                <asp:UpdatePanel ID="UpPnlRF" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <br />
                        <br />
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitRecursoLice" runat="server" CssClass="LblEtiquet" Text="Recurso Físico y Licencias" /></h6>
                        <asp:ImageButton ID="IbtCerrarRec" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarRec_Click" ImageAlign="Right" />
                        <asp:Table ID="TblRecFis" runat="server" Width="90%">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:GridView ID="GrdRecursoF" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodidDetElemPlanInstrumento"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="4"
                                        OnRowCommand="GrdRecursoF_RowCommand" OnRowEditing="GrdRecursoF_RowEditing"
                                        OnRowUpdating="GrdRecursoF_RowUpdating" OnRowCancelingEdit="GrdRecursoF_RowCancelingEdit"
                                        OnRowDeleting="GrdRecursoF_RowDeleting" OnRowDataBound="GrdRecursoF_RowDataBound" OnPageIndexChanging="GrdRecursoF_PageIndexChanging">
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
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                    </asp:GridView>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow Width="60%">
                                <asp:TableCell Width="60%">
                                    <h6 class="TextoSuperior SubTituloLicencia">
                                        <asp:Label ID="LblTitLicen" runat="server" CssClass="LblEtiquet" Text="Horas estimadas por licencia" /></h6>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:GridView ID="GrdLicen" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="IdSrvLic,CodIdLicencia"
                                        CssClass="DiseñoGrid table table-sm SubTituloLicencia" GridLines="Both" AllowPaging="true" PageSize="4"
                                        OnRowCommand="GrdLicen_RowCommand" OnRowEditing="GrdLicen_RowEditing"
                                        OnRowUpdating="GrdLicen_RowUpdating" OnRowCancelingEdit="GrdLicen_RowCancelingEdit"
                                        OnRowDeleting="GrdLicen_RowDeleting" OnRowDataBound="GrdLicen_RowDataBound" OnPageIndexChanging="GrdLicen_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Licencia" HeaderStyle-Width="20%">
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
                                            <asp:TemplateField HeaderText="Descripción" HeaderStyle-Width="40%">
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
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                    </asp:GridView>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="IbtCerrarRec" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Panel ID="PnlInforme" runat="server" Visible="false">
                <asp:UpdatePanel ID="UpPnlInforme" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <br />
                        <br />
                        <h6 class="TextoSuperior">
                            <asp:Label ID="TitInfSvc" runat="server" Text="Label" />
                        </h6>
                        <asp:ImageButton ID="IbtCerrarInf" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarInf_Click" ImageAlign="Right" />
                        <div class="row">
                            <div class="row-sm-4">
                                <asp:Button ID="BtnSvcAct" runat="server" CssClass="btn btn-primary" Text="Servicios activos" OnClick="BtnSvcAct_Click" />
                                <asp:Button ID="BtnCumplim" runat="server" CssClass="btn btn-primary" Text="Cumplimientos" OnClick="BtnCumplim_Click" />
                            </div>
                            <div class="row-sm-4">
                                <asp:ImageButton ID="IbtExpExcelSvcAplAK" runat="server" ToolTip="Exportar Servicios con aeronave asignadas" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpExcelSvcAplAK_Click" Height="35px" />
                                <asp:ImageButton ID="IbtExpExcelSvcGnrl" runat="server" ToolTip="Exportar todos los servicios  de mantenimiento" CssClass=" BtnExpExcel" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExpExcelSvcGnrl_Click" Height="35px" />
                            </div>
                        </div>
                        <rsweb:ReportViewer ID="RprvSvcActivos" runat="server" Width="95%"></rsweb:ReportViewer>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="IbtExpExcelSvcAplAK" />
                        <asp:PostBackTrigger ControlID="IbtExpExcelSvcGnrl" />
                        <asp:PostBackTrigger ControlID="IbtCerrarInf" />
                    </Triggers>
                </asp:UpdatePanel>

            </asp:Panel>
        </asp:View>
        <asp:View ID="Vw1ConfigIniCntdrHk" runat="server">
            <br />
            <br />
            <h6 class="TextoSuperior">
                <asp:Label ID="LblTitConfgIniCntd" runat="server" Text="configuracion inicial contador / frecuencia." />
            </h6>
            <asp:ImageButton ID="IbtCloseConfIniCF" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCloseConfIniCF_Click" />
            <div class="CentrarContenedor DivMarco">
                <div class="CentrarTable">
                    <div class="row ">
                        <div class="col-sm-8 CentrarGrdCntdrInic">
                            <asp:GridView ID="GrdConfInic" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" DataKeyNames="IdConfIni, IdSrvMantoCntdrSMHK, CodContador"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" ShowFooter="true" OnRowCommand="GrdConfInic_RowCommand"
                                OnRowEditing="GrdConfInic_RowEditing" OnRowUpdating="GrdConfInic_RowUpdating" OnRowCancelingEdit="GrdConfInic_RowCancelingEdit"
                                OnRowDeleting="GrdConfInic_RowDeleting" OnRowDataBound="GrdConfInic_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="contador" HeaderStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodContador") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%# Eval("CodContador") %>' runat="server" Width="100%" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtCodCntdrPP" runat="server" MaxLength="15" Width="100%" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="frecuencia" HeaderStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FrecuenciaSvc") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtFrec" Text='<%# Eval("FrecuenciaSvc") %>' runat="server" Width="100%" TextMode="Number" step="0" onkeypress="return solonumeros(event);" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtFrecPP" runat="server" Width="100%" TextMode="Number" step="0" onkeypress="return solonumeros(event);" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="frecuencia dia" HeaderStyle-Width="25%">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("FrecuenciaDiaSvc") %>' runat="server" Width="100%" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtFrecD" Text='<%# Eval("FrecuenciaDiaSvc") %>' runat="server" Width="100%" TextMode="Number" step="0" onkeypress="return solonumeros(event);" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="TxtFrecDPP" runat="server" Width="100%" TextMode="Number" step="0" onkeypress="return solonumeros(event);" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField FooterStyle-Width="20%">
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
</asp:Content>
