<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmWorkSheet.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmWorkSheet" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Work Sheet</title>
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

        .Scroll {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 400px
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
        function myFuncionddl() {
            $('#<%=DdlWSHK.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:MultiView ID="MlVw" runat="server">
        <asp:View ID="Vw0WorkSheet" runat="server">
            <asp:UpdatePanel ID="UplWS" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                     <br /> <br />
                    <div class="row">
                        <div class="col-sm-2">
                            <asp:Label ID="LblStsHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                            <asp:DropDownList ID="DdlWSHK" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlWSHK_TextChanged" AutoPostBack="true" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnWSNew" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnWSNew_Click" Text="Nuevo" ToolTip="Generar una nueva Work Sheet." Height="23px" />
                        </div>
                        <div class="col-sm-2">
                            <br />
                            <asp:Button ID="BtnWSProces" runat="server" CssClass="btn btn-success heightCampo" OnClick="BtnWSProces_Click" Text="Procesar" ToolTip="Procesar el estatus nuevamente." Height="23px" />
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-4 CentarGridAsig table-responsive">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitWSOpen" runat="server" Text="Work Sheet abiertas" /></h6>
                            <asp:GridView ID="GrdWSAbiertas" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%"
                                OnRowCommand="GrdWSAbiertas_RowCommand" OnRowDeleting="GrdWSAbiertas_RowDeleting" OnRowDataBound="GrdWSAbiertas_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="UplAbrir" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="IbtAbrir" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Abrir" ToolTip="Abrir Work Sheet" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="IbtAbrir" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Work Sheet">
                                        <ItemTemplate>
                                            <asp:Label ID="LblWS" Text='<%# Eval("Numerado") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Creada">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Fechacrea") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vence">
                                        <ItemTemplate>
                                            <asp:Label ID="LblFV" Text='<%# Eval("FechaVence") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Avance">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("AvanceWS") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="UplImpr" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="IbtPrintOT" Width="20" Height="25" ImageUrl="~/images/ManoObraV2.png" runat="server" CommandName="PrintWSTrab" ToolTip="Imprimir trabajos" />
                                                    <asp:ImageButton ID="IbtPrintRecu" Width="20" Height="25" ImageUrl="~/images/InventarioV1.png" runat="server" CommandName="PrintWSRecur" ToolTip="Imprimir planeacion de materiales" />
                                                    <asp:ImageButton ID="IbtDelete" CssClass="BotonDeleteGrid" ImageUrl="~/images/deleteV3.png" runat="server" CommandName="Delete" ToolTip="Eliminar" OnClientClick="javascript:return confirm('¿Está seguro de querer eliminar el registro seleccionado?', 'Mensaje de sistema')" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="IbtPrintOT" />
                                                    <asp:PostBackTrigger ControlID="IbtPrintRecu" />
                                                    <asp:PostBackTrigger ControlID="IbtDelete" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                        <div class="col-sm-6 CentarGridAsig table-responsive">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblTitWsBusq" runat="server" Text="Buscar Work Sheet" /></h6>
                            <div class="row">
                                <div class="col-sm-5">
                                    <asp:TextBox ID="TxtWSBusq" runat="server" Width="100%" Height="28px" CssClass="form-control" placeholder="Ingrese la Work Sheet a consultar" />
                                </div>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="IbtSWConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtSWConsultar_Click" />
                                </div>
                            </div>
                            <asp:GridView ID="GrdWSBusq" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="Estado,CodHKWS"
                                CssClass="GridControl DiseñoGrid table-sm" GridLines="Both"
                                OnRowCommand="GrdWSBusq_RowCommand" OnRowDataBound="GrdWSBusq_RowDataBound">
                                <FooterStyle CssClass="GridFooterStyle" />
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="UplAbrir2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="IbtAbrir2" Width="30px" Height="30px" ImageUrl="~/images/IrV2.png" runat="server" CommandName="Abrir" ToolTip="Abrir Work Sheet" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="IbtAbrir2" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Work Sheet">
                                        <ItemTemplate>
                                            <asp:Label ID="LblWS" Text='<%# Eval("Numerado") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Aeroanve">
                                        <ItemTemplate>
                                            <asp:Label ID="LblHK" Text='<%# Eval("Matricula") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado">
                                        <ItemTemplate>
                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Generada">
                                        <ItemTemplate>
                                            <asp:Label ID="LblFec" Text='<%# Eval("Fecha") %>' runat="server" Width="100%" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnWSNew" />
                    <asp:PostBackTrigger ControlID="BtnWSProces" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw1AsigOTaWS" runat="server">
            <asp:UpdatePanel ID="UplAsigOT" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                     <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitAsigOTaWS" runat="server" Text="Asignar orden de trabajo / Reporte a la Work Sheet" /></h6>
                    <div class="CentrarContenedor DivMarco">
                        <asp:ImageButton ID="IbtCerrarAsigOT" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarAsigOT_Click" ImageAlign="Right" />

                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblAsigOTHK" runat="server" CssClass="LblEtiquet" Text="Aeronave" />
                                <asp:TextBox ID="TxtAsigOTHK" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblAsingOTWS" runat="server" CssClass="LblEtiquet" Text="Work Sheet" />
                                <asp:TextBox ID="TxtAsingOTWS" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-5">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitServicios" runat="server" Text="Servicios / Reportes" /></h6>
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:RadioButton ID="RdbAsigOT" runat="server" CssClass="LblEtiquet" Text="&nbsp O.T." GroupName="AsigOT" OnCheckedChanged="RdbAsigOT_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="RdbAsigRte" runat="server" CssClass="LblEtiquet" Text="&nbsp Reporte" GroupName="AsigOT" OnCheckedChanged="RdbAsigRte_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="TxtAsigOT_RTE" runat="server" Width="100%" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" />
                                    </div>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="IbtAsigOTBusq" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtAsigOTBusq_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class=" table-responsive Scroll">
                                            <asp:GridView ID="GrdServicios" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" 
                                                DataKeyNames="FuenteWS,CodHKRva, Orden" CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" Visible="false"
                                                OnRowCommand="GrdServicios_RowCommand" OnRowDataBound="GrdServicios_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Servicio">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="P/N">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Eval("Pn") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="S/N">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Eval("Sn") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="OT" HeaderStyle-Width ="10%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblOT" Text='<%# Eval("CodigoOT") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Proyección">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblProy" Text='<%# Eval("Proyecc") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="IbtEdit" Width="30px" Height="30px" ImageUrl="~/images/FlechaIr.png" runat="server" CommandName="Asignar" ToolTip="Asignar" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="GridFooterStyle" />
                                                <HeaderStyle CssClass="GridCabecera" />
                                                <RowStyle CssClass="GridRowStyle" />
                                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                            </asp:GridView>
                                            <asp:GridView ID="GrdReportes" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" 
                                                DataKeyNames="FuenteWS,CodHKRva, Orden" CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" Visible="false"
                                                OnRowCommand="GrdReportes_RowCommand" OnRowDataBound="GrdReportes_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Descripción del Reporte">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblDesc" Text='<%# Eval("Descripcion") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PN">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Eval("Pn") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SN / Aeronave">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Eval("Sn") %>' runat="server" Width="100%" Enabled="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reporte">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblRte" Text='<%# Eval("CodigoRTE") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Proyección">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblProy" Text='<%# Eval("Proyecc") %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="IbtEdit" Width="30px" Height="30px" ImageUrl="~/images/FlechaIr.png" runat="server" CommandName="Asignar" ToolTip="Asignar" />
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
                            <div class="col-sm-7">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="lblTitOTWS" runat="server" Text="Ordenes de trabajo / Reportes asignados" /></h6>
                                <div class="CentarGridAsig table-responsive Scroll">
                                    <asp:GridView ID="GrdOTRteWS" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" DataKeyNames="Numerado,FuenteWS, Orden"
                                        CssClass="DiseñoGrid table-sm" GridLines="Both" Width="100%" OnRowDeleting="GrdOTRteWS_RowDeleting"
                                        OnRowEditing="GrdOTRteWS_RowEditing" OnRowUpdating="GrdOTRteWS_RowUpdating" OnRowCancelingEdit="GrdOTRteWS_RowCancelingEdit"
                                        OnRowDataBound="GrdOTRteWS_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Pri">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CkbPplP" Checked='<%# Eval("Eje").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:CheckBox ID="CkbPpl" Checked='<%# Eval("Eje").ToString()=="1" ? true : false %>' runat="server" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Trabajo">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Frec.">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Frec") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("Frec") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dias">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("frecuencia2") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("frecuencia2") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="P/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Pn") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("Pn") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S/N">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Sn") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("Sn") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OT/RTE">
                                                <ItemTemplate>
                                                    <asp:Label ID="LblOTRtP" Text='<%# Eval("CodOTRTE") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="LblOtE" Text='<%# Eval("CodOTRTE") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Estado") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("Estado") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha Vence" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("FechaVenc") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtFecVence" Text='<%# Eval("FechaVencDMY") %>' runat="server" Width="100%" TextMode="Date" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="proyección">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("FechaProyectada") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("FechaProyectada") %>' runat="server" Width="100%" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="8%">
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
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarAsigOT" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
        <asp:View ID="Vw2Informe" runat="server">
            <asp:UpdatePanel ID="UplInforme" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                     <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitImpresion" runat="server" Text="Impresión del reporte" /></h6>
                    <asp:ImageButton ID="IbtCerrarImpresion" runat="server" ToolTip="regresar" CssClass="BtnCerrar" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpresion_Click" ImageAlign="Right" />
                    <rsweb:ReportViewer ID="RpV" runat="server" Width="98%" />
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="IbtCerrarImpresion" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
