<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LanManager.BLL.Client>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mostrar Relatório
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Relatórios de <%=Model.FullName %></h2>
    <p>
        <%
            if (Model.ClientSession.Count == 0)
            {
                Response.Write("Não há histórico de relatórios");
            }
            else
            {
                foreach (LanManager.BLL.ClientSession sessao in Model.ClientSession.OrderByDescending(x => x.StartDate))
                {
                    Response.Write(Html.ActionLink(sessao.StartDate.ToString(), "VerSessao",
                                                   new { id = Model.Id, sessao = sessao.Id }) + "<br />");
                }
            }
%>
    </p>
</asp:Content>
