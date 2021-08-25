<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmStatusReportAtas.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmStatusReportAtas" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Status</title>
    <style type="text/css">
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
            $('#<%=DdlStsHK.ClientID%>').chosen();
            $('#<%=DdlStsGrupo.ClientID%>').chosen();
            $('#<%=DdlAsigOTPPT.ClientID%>').chosen();
            $('#<%=DdlLiberarOTNum.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:MultiView ID="MlVwSt" runat="server">
        <asp:View ID="Vw0St" runat="server">
            <asp:UpdatePanel ID="UplPpal" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                            <asp:DropDownList ID="DdlStsHK" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlStsHK_TextChanged" AutoPostBack="true" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsSn" runat="server" CssClass="LblEtiquet" Text="S/N" />
                            <asp:TextBox ID="TxtStsSn" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsModelo" runat="server" CssClass="LblEtiquet" Text="Modelo" />
                            <asp:TextBox ID="TxtStsModelo" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsFecCarga" runat="server" CssClass="LblEtiquet" Text="Ultima Fecha Carga" Width="100%" />
                            <asp:TextBox ID="TxtStsFecCarga" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="60%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblStsTSN" runat="server" CssClass="LblEtiquet" Text="Horas" />
                            <asp:TextBox ID="TxtStsTSN" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblStsCSN" runat="server" CssClass="LblEtiquet" Text="Ciclos" />
                            <asp:TextBox ID="TxtStsCSN" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblStsDiaProy" runat="server" CssClass="LblEtiquet" Text="Dias Proyecc." />
                            <asp:TextBox ID="TxtStsDiaProy" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" Text="0" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblModifDiaProy" runat="server" CssClass="LblEtiquet" Text="Actualizar" /><br />
                            <asp:Button ID="BtnModifDiaProy" runat="server" CssClass="btn btn-success heightCampo" OnClick="BtnModifDiaProy_Click" ToolTip="Modificar dias proyección"/>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsUtilDiaHr" runat="server" CssClass="LblEtiquet" Text="Utilizacion Diaria H" />
                            <asp:TextBox ID="TxtStsUtilDiaHr" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsUtilDiaCc" runat="server" CssClass="LblEtiquet" Text="Utilizacion Diaria C" />
                            <asp:TextBox ID="TxtStsUtilDiaCc" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsUtilDiaAPU" runat="server" CssClass="LblEtiquet" Text="Utilizacion Diaria APU" />
                            <asp:TextBox ID="TxtStsUtilDiaAPU" runat="server" CssClass="form-control heightCampo" Width="70%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="BtnStsImp" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnStsImp_Click" Text="Imprimir" ToolTip="Imprimir status" Height="23px" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="BtnStsExport" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnStsExport_Click" Text="Exportar" Height="23px" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="BtnStsOrdenar" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnStsOrdenar_Click" Text="Organizar" ToolTip="Ordenar grupos de servicios para la impresión" Height="23px" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="BtnStsAsigOT" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnStsAsigOT_Click" Text="Asignar O.T" ToolTip="Asignar O.T. a propuesta" Height="23px" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="BtnStsliberOT" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnStsliberOT_Click" Text="Liberar O.T" ToolTip="Liberar orden de trabajo de una propuesta a todo costo" Height="23px" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="BtnStatusAnt" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnStatusAnt_Click" Text="Pasado" ToolTip="Consultar status de fechas pasadas" Height="23px" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Table ID="TblOpciones" runat="server" Width="100%" GridLines="Horizontal" Visible="false">
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3">
                                        <asp:Label ID="LblStsGrupo" runat="server" CssClass="LblEtiquet" Text="Grupo" />
                                        <asp:DropDownList ID="DdlStsGrupo" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlStsHK_TextChanged" AutoPostBack="true" /><br />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3">
                                        <asp:Label ID="LblStsOrder" runat="server" CssClass="LblEtiquet" Text="Ordenar por" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:RadioButton ID="RdbStsAta" runat="server" CssClass="LblEtiquet" Text="&nbsp ATA" GroupName="BusqSts" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:RadioButton ID="RdbStsProy" runat="server" CssClass="LblEtiquet" Text="&nbsp Proyeción" GroupName="BusqSts" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:RadioButton ID="RdbStsDescrip" runat="server" CssClass="LblEtiquet" Text="&nbsp Descripción" GroupName="BusqSts" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3">
                                        <asp:Button ID="BtnStsConsult" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnStsConsult_Click" Text="Consultar" ToolTip="Consultar Status de la aeronave seleccionada" Height="30px" Width="100%" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </div>
                    </div>
                    <div class="table-responsive Scroll-table2">
                        <asp:GridView ID="GrdStatusReport" runat="server" EmptyDataText="No existen registros ..!"
                            CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                            <FooterStyle CssClass="GridFooterStyle" />
                            <HeaderStyle CssClass="GridCabecera" />
                            <RowStyle CssClass="GridRowStyle" />
                            <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                        </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnStsImp" />
                    <asp:PostBackTrigger ControlID="BtnStsExport" />
                    <asp:PostBackTrigger ControlID="BtnStsOrdenar" />
                    <asp:PostBackTrigger ControlID="BtnStsAsigOT" />
                    <asp:PostBackTrigger ControlID="BtnStsliberOT" />
                    <asp:PostBackTrigger ControlID="BtnStatusAnt" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw1Imprimir" runat="server">
            <asp:UpdatePanel ID="UplPrint" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitImpresion" runat="server" Text="Opciones de Informes" /></h6>
                    <asp:ImageButton ID="IbtCerrarPrint" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarPrint_Click" ImageAlign="Right" />
                    <div class="row">
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnImpStsStdr" runat="server" CssClass="btn btn-primary " OnClick="BtnImpStsStdr_Click" Text="Estandar" ToolTip="Imprimir status Standar" Height="30px" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnImpStsCompr" runat="server" CssClass="btn btn-primary " OnClick="BtnImpStsCompr_Click" Text="Comprimido" ToolTip="Imprimir status Comprimido" Height="30px" Width="100%" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnImpStsGrupos" runat="server" CssClass="btn btn-primary " OnClick="BtnImpStsGrupos_Click" Text="Grupos" ToolTip="Impresión por grupos" Height="30px" Width="100%" />
                        </div>
                    </div>
                    <rsweb:ReportViewer ID="RvwPrint" runat="server" Width="98%" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarPrint" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw2Order" runat="server">
            <asp:UpdatePanel ID="UplOrder" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOrdenarGrupImpr" runat="server" Text="Orden de impresión de grupos" /></h6>
                    <asp:ImageButton ID="IbtCerrarOrder" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarOrder_Click" ImageAlign="Right" />
                    <div class="CentarGrid">
                        <asp:GridView ID="GrdOrderGrup" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="CodPatronManto"
                            CssClass="DiseñoGrid table-sm" GridLines="Both" Width="80%"
                            OnRowEditing="GrdOrderGrup_RowEditing" OnRowUpdating="GrdOrderGrup_RowUpdating" OnRowCancelingEdit="GrdOrderGrup_RowCancelingEdit" OnRowDataBound="GrdOrderGrup_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Posición">
                                    <ItemTemplate>
                                        <asp:Label ID="LblPos" Text='<%# Eval("OrdenImpresion") %>' runat="server" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtPos" Text='<%# Eval("OrdenImpresion") %>' runat="server" Width="100%" TextMode="Number" onkeypress="return solonumeros(event);" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Código">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("CodPatronManto") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("CodPatronManto") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
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
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarOrder" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw3AsignarOTPPT" runat="server">
            <asp:UpdatePanel ID="UplAsigOTPPT" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigOTPPT" runat="server" Text="Órdenes sin asignación de propuesta" /></h6>
                    <asp:ImageButton ID="IbtCerrarAsigOtPPT" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsigOtPPT_Click" ImageAlign="Right" />
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:TextBox ID="TxtOTBusq" runat="server" Width="100%" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" TextMode="Number" onkeypress="return solonumeros(event);" />
                        </div>
                        <div class="col-sm-1">
                            <asp:ImageButton ID="IbtOTConsulAsigOTPPT" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtOTConsulAsigOTPPT_Click" /></td>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 CentarGridAsig table-responsive">
                            <asp:GridView ID="GrdAsigOTPPT" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" DataKeyNames="IdSrvManto"
                                CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="7" Width="100%"
                                OnRowEditing="GrdAsigOTPPT_RowEditing" OnRowUpdating="GrdAsigOTPPT_RowUpdating" OnRowCancelingEdit="GrdAsigOTPPT_RowCancelingEdit"
                                OnRowDataBound="GrdAsigOTPPT_RowDataBound" OnPageIndexChanging="GrdAsigOTPPT_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Asignar" HeaderStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbSelec" Checked='<%# Eval("CK").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CkbSelec" Checked='<%# Eval("CK").ToString()=="1" ? true : false %>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="O.T.">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodNumOrdenTrab") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblOT" Text='<%# Eval("CodNumOrdenTrab") %>' runat="server" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Propuesta">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("IdPropuesta") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblPPT" Text='<%# Eval("IdPropuesta") %>' runat="server" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Matrícula">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Matricula") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%# Eval("Matricula") %>' runat="server" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aplicabilidad">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Aplicabilidad") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%# Eval("Aplicabilidad") %>' runat="server" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Servicio">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Servicio") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%# Eval("Servicio") %>' runat="server" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
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
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" />
                            </asp:GridView>
                        </div>
                        <div class="col-sm-6 CentarGridAsig table-responsive">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitAsigOTPPTRepa" runat="server" Text="Asignar orden de trabajo a propuesta de reparación" /></h6>
                            <div class="row">
                                <div class="col-sm-2">
                                    <asp:Label ID="LblAsigOTPPTRepa" runat="server" CssClass="LblEtiquet" Text="Orden" />
                                    <asp:DropDownList ID="DdlAsigOTPPT" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlAsigOTPPT_TextChanged" AutoPostBack="true" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="LblAsigOTPPTHK" runat="server" CssClass="LblEtiquet" Text="Matrícula" />
                                    <asp:TextBox ID="TxtAsigOTPPTHK" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:Label ID="LblAsigOTPPTPN" runat="server" CssClass="LblEtiquet" Text="P/N" />
                                    <asp:TextBox ID="TxtlAsigOTPPTPN" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                </div>
                                <div class="col-sm-4">
                                    <asp:Label ID="LblAsigOTPPTSN" runat="server" CssClass="LblEtiquet" Text="S/N" />
                                    <asp:TextBox ID="TxtlAsigOTPPTSN" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="lblAsigOTPPTCliente" runat="server" CssClass="LblEtiquet" Text="Cliente" />
                                    <asp:TextBox ID="TxtAsigOTPPTCliente" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="TxtAsigOTPPTSvc" runat="server" CssClass="form-control heightCampo" TextMode="MultiLine" Enabled="false" Width="100%" />
                                </div>
                            </div>
                            <asp:GridView ID="GrdOTPPTRepa" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" DataKeyNames="IdPropuesta,IdDetPropHk"
                                CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%"
                                OnRowEditing="GrdOTPPTRepa_RowEditing" OnRowUpdating="GrdOTPPTRepa_RowUpdating" OnRowCancelingEdit="GrdOTPPTRepa_RowCancelingEdit"
                                OnRowDataBound="GrdOTPPTRepa_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Asignar" HeaderStyle-Width="4%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CkbSelec" Checked='<%# Eval("CK").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CkbSelec" Checked='<%# Eval("CK").ToString()=="1" ? true : false %>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Propuesta">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("IdPropuesta") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="LblPPT" Text='<%# Eval("IdPropuesta") %>' runat="server" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("DescripcionEstado") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%# Eval("DescripcionEstado") %>' runat="server" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MASTER">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("PPTMASTER") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label Text='<%# Eval("PPTMASTER") %>' runat="server" Width="100%" Enabled="false" />
                                        </EditItemTemplate>
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
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarAsigOtPPT" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw4LiberarOTPPT" runat="server">
            <asp:UpdatePanel ID="UplLiberarOT" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitLiberarOT" runat="server" Text="Liberar orden de trabajo de una propuesta a todo costo" /></h6>
                    <asp:ImageButton ID="IbtCerrarLiberarOT" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarLiberarOT_Click" ImageAlign="Right" />
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Label ID="LblLiberarOTNum" runat="server" CssClass="LblEtiquet" Text="Orden" />
                            <asp:DropDownList ID="DdlLiberarOTNum" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlLiberarOTNum_TextChanged" AutoPostBack="true" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Label ID="LblLiberarPPT" runat="server" CssClass="LblEtiquet" Text="Propuesta" />
                            <asp:TextBox ID="TxtLiberarPPT" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                        </div>
                        <div class="col-sm-1">
                            <asp:Label ID="LblBtnLiberar" runat="server" CssClass="LblEtiquet" Text="Ejecutar" />
                            <asp:Button ID="BtnLiberarOTPPT" runat="server" CssClass=" btn btn-success heightCampo" OnClick="BtnLiberarOTPPT_Click" Text="Liberar" ToolTip="Liberar la orden de trabajo seleccionada" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarLiberarOT" />
                    <asp:PostBackTrigger ControlID="BtnLiberarOTPPT" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw5StatusAnterior" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="TitStsAnterior" runat="server" Text="Consultar estatus en fecha anteriores" /></h6>
                    <asp:ImageButton ID="IbtCerrarLStsAnterior" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarLStsAnterior_Click" ImageAlign="Right" />
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Label ID="LblFechaStsAnt" runat="server" CssClass="LblEtiquet" Text="Fecha" />
                            <asp:TextBox ID="TxtFechaStsAnt" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="BtnFechaStsAntEje" runat="server" CssClass=" btn btn-success heightCampo" OnClick="BtnFechaStsAntEje_Click" Text="Consultar" />
                        </div>
                        <div class="col-sm-1">
                            <br />
                            <asp:Button ID="BtnStsAntExportar" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnStsAntExportar_Click" Text="Exportar" ToolTip="Exportar a Excel" Height="23px" />
                        </div>
                        <div class="table-responsive Scroll-table2">
                            <asp:GridView ID="GrdStsAnt" runat="server" EmptyDataText="No existen registros ..!"
                                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both">
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarLStsAnterior" />
                    <asp:PostBackTrigger ControlID="BtnFechaStsAntEje" />
                    <asp:PostBackTrigger ControlID="BtnStsAntExportar" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
