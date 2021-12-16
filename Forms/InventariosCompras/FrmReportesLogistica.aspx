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
            height: 35px;
            width: 95%;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplRteIngPpl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MlVw" runat="server">
                <asp:View ID="Vw0Principal" runat="server">
                    <div class="CentrarContndr DivMarco">
                        <div class="row">
                            <div class="col-sm-3">
                                <br />
                                <asp:Button ID="BtnReparaciones" runat="server" CssClass="btn btn-primary heightCampo " OnClick="BtnReparaciones_Click" OnClientClick="target ='_blank';" Text="Reparaciones" ToolTip="Informe de reparaciones en un rango de fecha." />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Reparaciones" runat="server">
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
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="IbtExcelRepa" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
