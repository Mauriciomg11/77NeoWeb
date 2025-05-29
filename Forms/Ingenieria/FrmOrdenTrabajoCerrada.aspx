<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="FrmOrdenTrabajoCerrada.aspx.cs" Inherits="_77NeoWeb.Forms.Ingenieria.FrmOrdenTrabajoCerrada" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarGrid {
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 60%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -30%;
            /*determinamos una altura*/
            /*indicamos que el margen superior, es la mitad de la altura*/
            padding: 5px;
        }

        .CentrarBusq {
            position: absolute;
            left: 50%;
            width: 60%;
            margin-left: -30%;
            height: 85%;
            padding: 5px;
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
        function myFuncionddl() {
            $('#<%=DdlStatus.ClientID%>').chosen();
            $('[id *=DdlCodEstadoP]').chosen();
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
  <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:MultiView ID="MultVw" runat="server">
                <asp:View ID="Vw0Datos" runat="server">
                    <div class="CentrarContenedor DivMarco">
                        <div class="row">
                            <div class="col-sm-3">
                                <asp:Label ID="LblCodOT" runat="server" CssClass="LblEtiquet" Text="orden de trabajo" />
                                <div class="row">
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TxtCodOT" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtCodigoOT" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-5">
                                <asp:Label ID="LblAplica" runat="server" CssClass="LblEtiquet" Text=" aplicabilidad" />
                                <asp:TextBox ID="TxtAplica" runat="server" CssClass="form-control heightCampo" Enabled="false" Width="100%" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="LblStatus" runat="server" CssClass="LblEtiquet" Text=" estado" />
                                <asp:DropDownList ID="DdlStatus" runat="server" CssClass="heightCampo" Width="100%" OnTextChanged="DdlStatus_TextChanged" AutoPostBack="true" />

                            </div>
                            <div class="col-sm-2">
                                <br />
                                <asp:CheckBox ID="CkbCancel" runat="server" CssClass="LblEtiquet" Text="cancelada" Enabled="false" Font-Size="17px" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-2">
                                <asp:Button ID="BtnConsult" runat="server" CssClass="btn btn-primary" Width="100%" OnClick="BtnConsult_Click" Text="consultar" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitPasos" runat="server" Text="pasos" /></h6>
                                <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="IDPasos,CodEstado"
                                    CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true"
                                    OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating"
                                    OnRowCancelingEdit="GrdDatos_RowCancelingEdit" OnRowDataBound="GrdDatos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Paso" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("PASO") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="LblPaso" Text='<%# Eval("PASO") %>' runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="descripción">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("DescripcionPaso") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label Text='<%# Eval("DescripcionPaso") %>' runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="codestado" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="LblCodEstadoP" Text='<%# Eval("CodEstado") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label Text='<%# Eval("CodEstado") %>' runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="estado">
                                            <ItemTemplate>
                                                <asp:Label Text='<%# Eval("NombreESO") %>' runat="server" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DdlCodEstadoP" runat="server" Width="100%" Height="28px" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-Width="15%">
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
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="8" />
                                </asp:GridView>
                            </div>
                            <div class="col-sm-6">
                                <h6 class="TextoSuperior">
                                    <asp:Label ID="LblTitOTPendCerr" runat="server" Text="ot abiertas" /></h6>
                                <div class="pre-scrollable">
                                    <asp:GridView ID="GrdOtPendCerrar" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="Orden"
                                        CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both"
                                        OnSelectedIndexChanged="GrdOtPendCerrar_SelectedIndexChanged">
                                        <Columns>
                                            <asp:CommandField HeaderText="Select" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                            <asp:TemplateField HeaderText="ot">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("CodigoOT") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="aplicabilidad">
                                                <ItemTemplate>
                                                    <asp:Label Text='<%# Eval("Aplicabilidad") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridCabecera" />
                                        <RowStyle CssClass="GridRowStyle" />
                                        <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="Vw1Busq" runat="server">
                    <br />
                    <h6 class="TextoSuperior">
                        <asp:Label ID="LblTitOpcBusqueda" runat="server" Text="Opciones de búsqueda " />
                    </h6>
                    <asp:ImageButton ID="IbtCerrarBusq" runat="server" ToolTip="Cerrar" CssClass="BtnCerrar" ImageAlign="Right" ImageUrl="~/images/CerrarV1.png" OnClick="IbtCerrarBusq_Click" />
                     
                    <div class="CentrarBusq DivMarco">                         
                        <div class="CentrarGrid pre-scrollable">
                            <table>
                            <tr>
                                <td>
                                    <asp:Label ID="LblBusqueda" runat="server" Text="Busqueda: " CssClass="LblTextoBusq" /></td>
                                <td>
                                    <asp:TextBox ID="TxtBusqueda" runat="server" Width="300px" Height="28px" CssClass="form-control" placeholder="Ingrese el dato a consultar" /></td>
                                <td>
                                    <asp:ImageButton ID="IbtConsultar" runat="server" ToolTip="Consultar" CssClass="BtnImagenBusqueda" ImageUrl="~/images/FindV2.png" OnClick="IbtConsultar_Click" /></td>
                            </tr>
                        </table>
                        <br />
                            <asp:GridView ID="GrdBusq" runat="server" EmptyDataText="No existen registros ..!" AutoGenerateColumns="false" DataKeyNames="CodNumOrdenTrab"
                                CssClass="GridControl DiseñoGrid table table-sm" GridLines="Both"
                                OnSelectedIndexChanged="GrdBusq_SelectedIndexChanged">
                                <Columns>
                                    <asp:CommandField HeaderText="Select" SelectText="Select" ShowSelectButton="True" HeaderStyle-Width="33px" />
                                    <asp:TemplateField HeaderText="ot">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Codigo") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="aplicabilidad">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Aplicabilidad") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="estado">
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("CodEstOrdTrab1") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="GridCabecera" />
                                <RowStyle CssClass="GridRowStyle" />
                                <AlternatingRowStyle CssClass="GridFilasIntercaladas" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
