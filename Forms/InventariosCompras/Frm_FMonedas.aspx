<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="Frm_FMonedas.aspx.cs" Inherits="_77NeoWeb.Forms.InventariosCompras.Frm_FMonedas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarContenedor {
            /*vertical-align: top;*/
            background: #e0e0e0;
            margin: 0 0 1rem;
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 80%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -40%;
            /*determinamos una altura*/
            height: 90%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            /*margin-top: -150px;*/
            border: 1px solid #808080;
            padding: 5px;
        }

        .CentrarBoton {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 80%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -40%;
            /*determinamos una altura*/
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }

        .CentrarGrdHistrc {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 40%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -20%;
            /*determinamos una altura*/
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }

        .Font_btnCrud {
            font-size: 12px;
            font-stretch: condensed;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EncScriptDdl" runat="server">
    <script type="text/javascript">  
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
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
    <h1>
        <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" /></h1>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UpPanel" runat="server">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <br />
                    <br />
                    <div class="CentrarContenedor DivMarco">
                        <div class="CentrarTable">                          
                            <div class="row">
                                <div class="col-sm-12 CentrarBoton ">
                                      <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                                    <td>
                                        <asp:TextBox ID="TxtBusqueda" runat="server" Width="500px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                                    <td>
                                        <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                                    <td>
                                        <asp:Button ID="BtnEditarHistrc" runat="server" CssClass="btn btn-success Font_btnCrud" Width="100%" OnClick="BtnEditarHistrc_Click" OnClientClick="target ='';" Text="Històrico" />
                                    </td>
                                </tr>
                            </table>
                                    <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="true" DataKeyNames="CodTipoMoneda"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="8"
                                        OnRowCommand="GrdDatos_RowCommand" OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating"
                                        OnRowCancelingEdit="GrdDatos_RowCancelingEdit" OnRowDataBound="GrdDatos_RowDataBound" OnPageIndexChanging="GrdDatos_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="moneda" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodTipoMoneda") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtMondPP" runat="server" MaxLength="15" Width="100%" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripc" HeaderStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Descripcion") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtDesc" Text='<%# Eval("Descripcion") %>' runat="server" MaxLength="80" Width="100%" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtDescPP" runat="server" MaxLength="80" Width="100%" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Simbolo">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("simbolo") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtSimbl" Text='<%# Eval("simbolo") %>' runat="server" MaxLength="4" Width="100%" />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="TxtSimblPP" runat="server" MaxLength="4" Width="100%" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TRM actual">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("TasaCambiaria") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtTrmAct" Text='<%# Eval("TasaCambiaria") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" Enabled="false" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ultima fecha registrada" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("UltFecMod") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("UltFecMod") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="TRM nueva" HeaderStyle-Width="15%">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtTrmNew" runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="fecha" HeaderStyle-Width="15%">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtFecha" runat="server" Width="100%" TextMode="Date" MaxLength="10" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="IbtEdit" CssClass="BotonEditGrid" ImageUrl="~/images/Edit.png" runat="server" CommandName="Edit" ToolTip="Editar" />
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
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Historico" runat="server">
                     <br /> <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitHisto" runat="server" Text="Editar Historico" />
                    </h6>
                    <asp:ImageButton ID="IbtCloseHist" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCloseHist_Click" />
                    <div class="CentrarContenedor DivMarco">
                        <div class="CentrarTable">                            
                            <div class="row ">
                                <div class="col-sm-4 CentrarGrdHistrc ">
                                    <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="LblBusquedaH" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                                    <td>
                                        <asp:TextBox ID="TxtBusqMon" runat="server" Width="90px" Height="28px" CssClass="form-control" placeholder="moneda" /></td>
                                    <td>
                                        <asp:TextBox ID="TxtBusqAno" runat="server" Width="80px" Height="28px" CssClass="form-control" placeholder="ano" TextMode="Number" onkeypress="return solonumeros(event);" /></td>
                                    <td>
                                        <asp:TextBox ID="TxtBusqMes" runat="server" Width="90px" Height="28px" CssClass="form-control" placeholder="mes" TextMode="Number" onkeypress="return solonumeros(event);" /></td>
                                    <td>
                                        <asp:ImageButton ID="IbtConsultarH" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultarH_Click" /></td>
                                </tr>
                            </table>
                                    <asp:GridView ID="GrdDatosH" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" DataKeyNames="CodIdTasa, UltFecModSis"
                                        CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true"
                                        OnRowEditing="GrdDatosH_RowEditing" OnRowUpdating="GrdDatosH_RowUpdating" OnRowCancelingEdit="GrdDatosH_RowCancelingEdit"
                                        OnRowDataBound="GrdDatosH_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="TRM actual" HeaderStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("VlrTasa") %>' runat="server" Width="100%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtVrT" Text='<%# Eval("VlrTasa") %>' runat="server" Width="100%" TextMode="Number" step="0.01" onkeypress="return Decimal(event);" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ultima fecha registrada" HeaderStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("UltFecMod") %>' runat="server" Width="100%" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label Text='<%# Eval("UltFecMod") %>' runat="server" Width="100%" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField FooterStyle-Width="30%">
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
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
