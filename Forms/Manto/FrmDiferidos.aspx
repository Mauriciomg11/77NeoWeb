<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmDiferidos.aspx.cs" Inherits="_77NeoWeb.Forms.Manto.FrmDiferidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarDiv {
            position: absolute;
          
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
        ScrollDivGrid {
            vertical-align: top;
            overflow: auto;
            width: 100%;
            height: 63%;
        }
        .heightCampo {
            height: 25px;
            width: 95%;
            font-size: 12px;
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
        function myFuncionddl() {
            $('#<%=DdlAeronave.ClientID%>').chosen();
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
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
             <div class=" CentrarDiv DivMarco">
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Label ID="LblAeronave" runat="server" CssClass="LblEtiquet" Text="aeronave" />
                                <asp:DropDownList ID="DdlAeronave" runat="server" CssClass="heightCampo" Width="100%" />
                            </div>
                            <div class="col-sm-6">
                                <asp:RadioButton ID="RdbTodos" runat="server" CssClass="LblEtiquet" Text="&nbsp Todos" GroupName="D" />
                                <asp:RadioButton ID="RdbAbierto" runat="server" CssClass="LblEtiquet" Text="&nbsp Abiertos" GroupName="D" />
                                <asp:RadioButton ID="RdbCumpl" runat="server" CssClass="LblEtiquet" Text="&nbsp Cumplidos" GroupName="D" />
                            </div>                           
                        </div>
                        <div class="row">                            
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechI" runat="server" CssClass="LblEtiquet" Text="fecha Inicial" />
                                <asp:TextBox ID="TxtFechI" runat="server" CssClass="form-control heightCampo" Width="100%" TextMode="Date" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblFechF" runat="server" CssClass="LblEtiquet" Text="fecha Final" />
                                <asp:TextBox ID="TxtFechF" runat="server" CssClass="form-control heightCampo" Width="100%" TextMode="Date" />
                            </div>
                        </div>
                         <br />
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Button ID="BtnConsult" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnConsult_Click" Text="nuevo" />
                            </div>                           
                            <div class="col-sm-2">
                                <asp:Button ID="BtnAlertaCO" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnAlertaCO_Click" OnClientClick="target ='_blank';"  Text="Alerta C-Over" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="BtnExportar" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnExportar_Click" Text="Exportar" />
                            </div>
                        </div>                     
                        <br />
                        <div class="row ">
                            <div class="col-sm-12">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitReportes" runat="server" Text="reportes de mantenimiento diferidos" /></h6>
                            </div>
                        </div>
                        <div class="ScrollDivGrid">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="num_reporte" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("num_reporte") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Matricula">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Matricula") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="fechareporte">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("fechareporte") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="item_mel" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("item_mel") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="categoria">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("categoria") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="fechavencimiento">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("fechavencimiento")%>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="fechacumplimiento">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("fechacumplimiento")%>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="estado" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("StatusR")%>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="libro_vuelo">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("libro_vuelo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="reportado" HeaderStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("reportado") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="reporte" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("reporte") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="accioncorrectiva" HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("accioncorrectiva") %>' runat="server" Width="100%" />
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
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="BtnExportar" />
            <asp:PostBackTrigger ControlID="BtnAlertaCO" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
