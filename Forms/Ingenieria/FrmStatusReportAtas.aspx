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
                            <asp:Button ID="BtnModifDiaProy" runat="server" CssClass=" btn btn-success heightCampo" OnClick="BtnModifDiaProy_Click" ToolTip="Modificar dias proyección" Enabled="false" />
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
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <%----%>
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
                    <%----%>
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
                    <div>
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
    </asp:MultiView>
</asp:Content>
