<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="Frm_InfIngenieria.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.Frm_InfIngenieria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>OT</title>
    <style type="text/css">
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
    <script type="text/javascript">       
        function myFuncionddl() {
            <%--$('#<%=DdlMroTaller.ClientID%>').chosen();--%>          
        }
        function targetMeBlank() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:MultiView ID="MlVw" runat="server">
        <asp:View ID="Vw0Principal" runat="server">
            <asp:UpdatePanel ID="UplRteIngPpl" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="wrp">
                        <div class="frm">
                            <div class="row">
                                <div class="col-sm-3">
                                    <br />
                                    <asp:Button ID="BtnAdvice" runat="server" CssClass="btn btn-primary heightCampo " OnClick="BtnAdvice_Click" Text="Advice" ToolTip="Imprimir valores actuales de los contadores de un elemento." />
                                </div>
                                <div class="col-sm-3">
                                    <br />
                                    <asp:Button ID="BtnInsRemElem" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnInsRemElem_Click" Text="Instalación / Remoción" ToolTip="Histórico de Istalaciones y remociones / Eliminación de histórico." />
                                </div>
                                <div class="col-sm-3">
                                    <br />
                                    <asp:Button ID="BtnInsRemSubC" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnInsRemSubC_Click" Text="Histórico Subcomponente" ToolTip="Historico de instalaciones y remociones de Subcomponentes." />
                                </div>
                                <div class="col-sm-3">
                                    <br />
                                    <asp:Button ID="BtnPnPlanti" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnPnPlanti_Click" Text="P/N en plantilla maestra" ToolTip="P/N configurados en el último nivel." />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3">
                                    <br />
                                    <asp:Button ID="BtnHistCont" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnHistCont_Click" OnClientClick="target ='_blank';" Text="Histórico de contadores" ToolTip="Histórico de contadores de aeronave y elementos." />
                                </div>
                                <div class="col-sm-3">
                                    <br />
                                    <asp:Button ID="BtnProcIngeni" runat="server" CssClass="btn btn-primary heightCampo" OnClick="BtnProcIngeni_Click" OnClientClick="target ='_blank';" Text="Procesos de Ingenieria" ToolTip="Procesos de los contadores." />
                                </div>
                            </div>

                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="BtnHistCont" />
                    <asp:PostBackTrigger ControlID="BtnProcIngeni" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
