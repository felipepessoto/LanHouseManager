<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LanManager.BLL.Client>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Editar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Aplicativos permitidos</h2>

<p><strong><%=Model.FullName %></strong></p>

<%using (Html.BeginForm())
  { %>
  
<%
    if (!Model.CanAccessAnyApplication)
    {
        Response.Write("Aplicativos:<br />");
        foreach (var application in ((List<LanManager.BLL.Application>) ViewData["AllApps"]))
        {
            Response.Write(application.Name + " ");
            if (Model.ApplicationsAllowed.Any(x => x.Id == application.Id))
            {
                Response.Write(Html.ActionLink("Remover Permissão", "RemoverApp",
                                               new {id = Model.Id, app = application.Id}, new {style = "color:#FF0000"}));
            }
            else
            {
                Response.Write(Html.ActionLink("Adicionar Permissão", "AdicionarApp",
                                               new {id = Model.Id, app = application.Id}, new {style = "color:#0000FF"}));
            }
            Response.Write("<br />");
        }
%>

<br />
<br />
    Grupos:<br />
    <%
        foreach (var group in ((List<LanManager.BLL.ApplicationGroup>) ViewData["AllGroups"]))
        {
            Response.Write(group.Name + " ");
            if (Model.ApplicationGroupsAllowed.Any(x => x.Id == group.Id))
            {
                Response.Write(Html.ActionLink("Remover Permissão", "RemoverGroup", new {id = Model.Id, app = group.Id},
                                               new {style = "color:#FF0000"}));
            }
            else
            {
                Response.Write(Html.ActionLink("Adicionar Permissão", "AdicionarGroup",
                                               new {id = Model.Id, app = group.Id}, new {style = "color:#0000FF"}));
            }
            Response.Write("<br />");
        }
        Response.Write("<br /><br />");
    }
%>

Pode acessar todos aplicativos: <%=Html.CheckBox("canAccessAllApps", Model.CanAccessAnyApplication) %>

<input type="submit" value="Salvar" />

<%} %>
    <p>
        <%=Html.ActionLink("Voltar", "Index") %>
    </p>

</asp:Content>

