using pet_login.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace pet_login.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source=DESKTOP-PCMGNFF\\SQLEXPRESS; Initial Catalog=BD_ACCESO;Integrated Security=true";
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario usuario)
        {
            bool registrado;
            string mensaje;

            if (usuario.Clave == usuario.ConfirmarClave) 
            {
                usuario.Clave = CovertirSha256(usuario.Clave);
            }
            else
            {
                ViewData["Mensaje"] = "Las contrasenas no coinciden";
                return View();
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("registrar_usuario", cn);
                cmd.Parameters.AddWithValue("correo", usuario.Correo);
                cmd.Parameters.AddWithValue("clave", usuario.Clave);
                cmd.Parameters.Add("registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();
                registrado = Convert.ToBoolean(cmd.Parameters["registrado"].Value);
                mensaje = cmd.Parameters["mensaje"].Value.ToString();
            }

            ViewData["mensaje"] = mensaje;
            if (registrado)
            {
                return RedirectToAction("Login","Acceso");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            usuario.Clave = CovertirSha256(usuario.Clave);
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("validar_usuario",cn);
                cmd.Parameters.AddWithValue("correo", usuario.Correo);
                cmd.Parameters.AddWithValue("clave", usuario.Clave);
                
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                usuario.IdUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }
            if(usuario.IdUsuario != 0)
            {
                Session["usuario"] = usuario;
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["mensaje"] = "usuario no encontrado";
                return View();
            }
        }
        public static string CovertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (SHA256 hash = SHA256Managed.Create())
                {
                    Encoding enc = Encoding.UTF8;
                    byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                    foreach (byte b in result)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                }
            }
            catch 
            {
                throw;
            }
            finally
            {

            }
            return sb.ToString();
        }
    }
}