<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<LanManager.BLL.Client>>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    LanManager - Principal
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Escolha seu dependente</h2>
    <p>
        <%
            foreach (LanManager.BLL.Client client in Model)
            {
                Response.Write(client.FullName + ": " + Html.ActionLink("Editar Aplicativos", "EditarApps", new {id = client.Id}));
                Response.Write(" - " + Html.ActionLink("Editar Horários", "EditarHorarios", new { id = client.Id }));
                Response.Write(" - " + Html.ActionLink("Relatórios", "Relatorio", new { id = client.Id }) + "<br />");
            }
%>
    </p>
</asp:Content>
