using pet_login.Permisos;
using System.Web.Mvc;

namespace pet_login
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new UsuarioActionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
