using System.Web.Mvc;
using pet_login.Models;

namespace pet_login.Permisos
{
    public class UsuarioActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Usuario usuario = (Usuario)filterContext.HttpContext.Session["usuario"];
            filterContext.Controller.ViewBag.Usuario = usuario;
        }
    }
}
