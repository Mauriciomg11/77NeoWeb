<%@ Page Title="" Language="C#" MasterPageFile="~/MasterTransac.Master" AutoEventWireup="true" CodeBehind="FrmCierreContable.aspx.cs" Inherits="_77NeoWeb.Forms.Configuracion.FrmCierreContable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .CentrarBoton {
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

        .CentrarContenedor {
            /*vertical-align: top;*/
            /*background: #e0e0e0;*/
            margin: 0 0 1rem;
            position: absolute;
            /*nos posicionamos en el centro del navegador*/
            /*top: 50%;*/
            left: 50%;
            /*determinamos una anchura*/
            width: 60%;
            /*indicamos que el margen izquierdo, es la mitad de la anchura*/
            margin-left: -30%;
            /*determinamos una altura*/
            height: 90%;
            /*indicamos que el margen superior, es la mitad de la altura*/
            /*margin-top: -150px;*/
            /*border: 1px solid #808080;*/
            padding: 5px;
            top: 150px
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
            $('#<%=DdlMes.ClientID%>').chosen();
            $('#<%=DdlAno.ClientID%>').chosen(); 
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="TituloPagina" runat="server">
  <asp:Label ID="TitForm" runat="server" CssClass="CsTitulo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CuerpoPagina" runat="server">
    <asp:UpdatePanel ID="UplDatos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="CentrarContenedor">

                <div class="row">
                    <div class="col-sm-6">
                        <asp:Label ID="LblMes" runat="server" CssClass="LblEtiquet" Text=" mes" />
                        <asp:DropDownList ID="DdlMes" runat="server" CssClass="heightCampo" Width="100%" />
                    </div>
                    <div class="col-sm-6">
                        <asp:Label ID="LblAno" runat="server" CssClass="LblEtiquet" Text=" mes" />
                        <asp:DropDownList ID="DdlAno" runat="server" CssClass="heightCampo" Width="100%" />
                    </div>
                </div>
                <br />
                <div class="row ">
                    <div class="col-sm-6 CentrarBoton ">
                        <asp:Button ID="BtnCierreM" runat="server" CssClass="btn btn-success botones" Width="100%" OnClick="BtnCierreM_Click" Text="cierre" />
                    </div>
                </div>
                <br />
                <br />
                <br />
                <div class="row ">
                    <div class="col-sm-6 CentrarBoton ">
                        <%--  <div class="DivGrid DivContendorGrid">--%>
                        <h6 class="TextoSuperior">
                            <asp:Label ID="LblTitActPer" runat="server" Text="activar / desactivar periodo" /></h6>
                        <asp:GridView ID="GrdDatos" runat="server" AutoGenerateColumns="False" AutoGenerateSelectButton="False" ShowFooter="false" DataKeyNames="idcierre"
                            CssClass="DiseñoGrid table table-sm" GridLines="Both" AllowPaging="true" PageSize="10"
                            OnRowEditing="GrdDatos_RowEditing" OnRowUpdating="GrdDatos_RowUpdating" OnRowCancelingEdit="GrdDatos_RowCancelingEdit" OnRowDataBound="GrdDatos_RowDataBound"
                            OnPageIndexChanging="GrdDatos_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Acti">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CkbActP" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="CkbAct" Checked='<%# Eval("Activo").ToString()=="1" ? true : false %>' runat="server" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="CkbActPP" runat="server" Checked="true" Enabled="false" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="mes">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Mes") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("Mes") %>' runat="server" Width="100%" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ano">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("ano") %>' runat="server" Width="100%" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label Text='<%# Eval("ano") %>' runat="server" Width="100%" />
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
                        <%--</div>--%>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
