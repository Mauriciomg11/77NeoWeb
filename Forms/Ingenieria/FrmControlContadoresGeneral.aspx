<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmControlContadoresGeneral.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmControlContadoresGeneral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ProcIng</title>
    <style type="text/css">
        .BotonesPpal {
            width: 110%;
            font-size: 12px;
        }
        .LargoDiv{
            height:60%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">
        function myFuncionddl() {
            $('#<%=DdlCorrContHK.ClientID%>').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplBtnes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-2">
                    <br />
                    <asp:Button ID="BtnProceLibrV" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnProceLibrV_Click" Text="Procesar Libros de Vuelo" ToolTip="Procesar contadores de cada libro de vuelo por día." />
                </div>
                <div class="col-sm-2">
                    <br />
                    <asp:Button ID="BtnAjusExceso" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnAjusExceso_Click" Text="Ajuste Exceso" ToolTip="Eliminar históricos a partir de una fecha." />
                </div>
                <div class="col-sm-2">
                    <br />
                    <asp:Button ID="BtnAjusDefect" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnAjusDefect_Click" Text="Ajuste Defecto" ToolTip="Reproceso de contadores de un elemento a partir de una fecha." />
                </div>
                <div class="col-sm-2">
                    <br />
                    <asp:Button ID="BtnAjusDefectMyr" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnAjusDefectMyr_Click" Text="Ajuste Defecto Mayor" ToolTip="Reproceso de contadores de un Mayor y subcomponentes a partir de una fecha." />
                </div>
                <div class="col-sm-2">
                    <br />
                    <asp:Button ID="BtnAjusConve" runat="server" CssClass="btn btn-outline-primary BotonesPpal" OnClick="BtnAjusConve_Click" Text="Ajuste Conveniencia" ToolTip="Reproceso de contadores de una aeronave y elementos instalados a partir de un rango de fecha." />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
              <asp:PostBackTrigger ControlID="BtnProceLibrV" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:MultiView ID="MlVPI" runat="server">
        <asp:View ID="Vw0CorrerContadores" runat="server">
            <asp:UpdatePanel ID="UplOTDetTec" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitProcCont" runat="server" Text="Procesar Contadores" /></h6>
                    <div class="row">
                        <div class="col-sm-2">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblSubTitCorreContLVSinProc" runat="server" Text="Hojas sin procesar" /></h6>
                              <asp:ListBox ID="LbxLibrosSinProc" runat="server" Font-Size="12px" Width="100%" Height="400px" OnSelectedIndexChanged="LbxLibrosSinProc_SelectedIndexChanged" AutoPostBack="True"/>
                        </div>
                        <div class="col-sm-2">
                            <h6 class="TextoSuperior">
                                <asp:Label ID="LblSubTitCorrContHK" runat="server" Text="Hojas sin procesar" /></h6>
                             <asp:DropDownList ID="DdlCorrContHK" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlCorrContHK_TextChanged" AutoPostBack="true" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
