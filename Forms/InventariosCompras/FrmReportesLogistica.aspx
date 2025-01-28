<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmReportesLogistica.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.FrmReportesLogistica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarContndr {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 80%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -40%;
            height: 60%;
            padding: 5px;
        }

        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
        }

        .heightBtns {
            height: 35px;
            width: 95%;
            font-size: 12px;
        }

        .CentarGrid {
            text-align: left;
            width: 100%;
            margin: auto;
            border: 1px solid black;
        }

        .wrp {
            width: 100%;
            text-align: center;
        }

        .frm {
            text-align: left;
            width: 80%;
            margin: auto;
            border: 1px solid black;
        }

        .fldLbl {
            white-space: nowrap;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">  

        function myFuncionddl() {
            $('#<%=DdlAlmacenInv.ClientID%>').chosen();
            $('#<%=DdlGrupoInv.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
   <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplRteIngPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MlVw" runat="server">
                <asp:View ID="Vw0Principal" runat="server">
                    <br />
                    <div class="CentrarContndr DivMarco">
                        <div class="row">
                            <div class="col-sm-3">
                                <br />
                                <asp:Button ID="BtnInventario" runat="server" CssClass="btn btn-primary heightBtns" OnClick="BtnInventario_Click" OnClientClick="target ='_blank';" Text="inventario" ToolTip="Inventario por grupo." />
                            </div>
                            <div class="col-sm-3">
                                <br />
                                <asp:Button ID="BtnReparaciones" runat="server" CssClass="btn btn-primary heightBtns" OnClick="BtnReparaciones_Click" OnClientClick="target ='_blank';" Text="Reparaciones" ToolTip="Informe de reparaciones en un rango de fecha." />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Reparaciones" runat="server">
                     <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitReparaciones" runat="server" Text="reparaciones" />
                    </h6>

                    <asp:ImageButton ID="IbtCerrarImpr" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarImpr_Click" />
                    <div class="CentrarContndr DivMarco">
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechI" runat="server" CssClass="LblEtiquet" Text="Fecha Inicial" />
                                <asp:TextBox ID="TxtFechI" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechF" runat="server" CssClass="LblEtiquet" Text="Fecha Final" />
                                <asp:TextBox ID="TxtFechF" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                            <div class="col-sm-0">
                                <br />
                                <asp:ImageButton ID="IbtExcelRepa" runat="server" ToolTip="Exportar reparaciones" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExcelRepa_Click" />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw2Inventario" runat="server">
                     <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitInventario" runat="server" Text="Inventario por grupo a partir de un corte de fecha" />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarInvetr" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarInvetr_Click" />
                    <div class="CentrarContndr DivMarco">
                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label ID="LblAlmacenInv" runat="server" CssClass="LblEtiquet" Text="almacen" />
                                <asp:DropDownList ID="DdlAlmacenInv" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Label ID="LblGrupoInv" runat="server" CssClass="LblEtiquet" Text="grupo" />
                                <asp:DropDownList ID="DdlGrupoInv" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlGrupoInv_TextChanged" AutoPostBack="true" />
                            </div>
                            <div class="col-sm-3">
                                <br />
                                <asp:RadioButton ID="RdbSrlzdInv" runat="server" CssClass="LblEtiquet" GroupName="Grp" Checked="false" Text="serializado &nbsp"  Enabled="false"/>&nbsp&nbsp&nbsp
                                <asp:RadioButton ID="RdbNoSrlzdInv" runat="server" CssClass="LblEtiquet" GroupName="Grp" Checked="false" Text="no serializado &nbsp" Enabled="false"/>&nbsp&nbsp&nbsp
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechCorte" runat="server" CssClass="LblEtiquet" Text="Fecha corte" />
                                <asp:TextBox ID="TxtFechCorte" runat="server" CssClass="form-control-sm heightCampo" Width="100%" TextMode="Date" MaxLength="10" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-0">
                                <br />
                                <asp:ImageButton ID="IbtExprtrInvtr" runat="server" ToolTip="Exportar inventario" CssClass=" BtnExpExcel" Height="38px" Width="40px" ImageUrl="~/images/ExcelV1.png" OnClick="IbtExprtrInvtr_Click" />
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbtExcelRepa" />
            <asp:PostBackTrigger ControlID="IbtExprtrInvtr" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
