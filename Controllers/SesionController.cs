using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication3.Models;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using WebApplication3.Class;
using System.Text.RegularExpressions;
using System.Security.Cryptography;


namespace WebApplication3.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AllowAnonymous]
    [RoutePrefix("api/sesion")]

    public class SesionController : ApiController
    {
       PruebasEntities db = new PruebasEntities();
       ValidacionesPassword validacion = new ValidacionesPassword();

        /////////////////////////Obtener Correo//////////////////////////////
        [HttpGet]
        [Route("extraer_datos")]
        public HttpResponseMessage ObtenerCorreoOTelefono(string dispositivo = null, string email = null, string phone = null)
        {
            Respuesta r = new Respuesta();
            SendEmail enviarEmail = new SendEmail();
            ObjectParameter m = new ObjectParameter("Mensaje", typeof(string));
            ObjectParameter s = new ObjectParameter("Status", typeof(byte));
            try
            {
                var correo_electronico = "";
                if (email != null)
                {
                    correo_electronico = CultureInfo.CurrentCulture.TextInfo.ToLower(email);
                }
                else
                {
                    correo_electronico = email;
                }
                var num = 0;
                if (phone != null)
                {
                    num = Convert.ToInt32(phone.Replace("-",""));
                }
                else
                {
                    num = 0;
                }
                var correo = (from d in db.Pruebas_EMP where (d.Email == correo_electronico && d.Email != null) select d);
                var telefono = (from t in db.Pruebas_EMP where (t.NumTelefono == num && t.NumTelefono != null) select t);

                if (correo.Count() != 0)
                {
                    var solicitudes = (from solicitud in db.Pruebas_SolicitarToken_CambioPassword where solicitud.FechaSolicitud > DateTime.Today select solicitud).ToList();
                    if(solicitudes.Count >= 1)
                    {
                        switch (solicitudes.First().Estado) 
                        {
                            case 0:
                                r.status = true;
                                r.message = "Usted ya ha realizado una solicitud de recuperacion de contraseña el día de hoy.";
                                break;
                            case 1:
                                r.status = false;
                                r.message = "Usted ya solicito el cambio de contraseña el día de hoy, por favor espere 24 horas antes de volver a intentar";
                                break;
                        }
                    }
                    else
                    {
                        Random tokenRandom = new Random();
                        var token = tokenRandom.Next(1, 999999999);
                        var Key = token.ToString("000000000");
                        var Token = SophiaBase64Encoder.Encoder(token.ToString("000000000"));
                        var usuario = (from u in db.Pruebas_EMP where u.Email == correo_electronico select u).FirstOrDefault();
                        db.Pruebas_P_SolicitarToken_CambioPassword(email, dispositivo, Token, Key, DateTime.Now, DateTime.Now.AddMinutes(30), 1,m,s);
                        enviarEmail.enviarToken(correo_electronico, Key, usuario.NombreEmpleado);
                        var status = Convert.ToInt32(s.Value);
                        if (status == 1)
                        {
                            r.status = true;
                            r.message = m.Value.ToString();
                        }
                        else
                        {
                            r.status = false;
                            r.message = m.Value.ToString();
                        }
                    }
                    
                    return Request.CreateResponse(HttpStatusCode.OK, r);
                }
                else if (telefono.Count() != 0)
                {
                    r.status = true;
                    r.message = "";
                    return Request.CreateResponse(HttpStatusCode.OK, r);
                }
                else
                {
                    r.status = false;
                    r.message = "No existe o se ha equivocado";
                    return Request.CreateResponse(HttpStatusCode.OK, r);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /////////////////////////Iniciar Sesión//////////////////////////////
        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login(string email, string password)
        {
            Respuesta r = new Respuesta();
      
            try
            {
                var existe = (from d in db.Pruebas_EMP where d.Email == email select d).FirstOrDefault();
                if (existe == null)
                {
                    r.status = false;
                    r.message = "No existe este usuario.";
                    r.token = null;
                    return Request.CreateResponse(HttpStatusCode.OK, r);
                }
                
                if(existe.IdEstado == 2)
                {
                    r.status = false;
                    r.message = "Su usuario se encuentra bloqueado";
                    r.token = null;
                    return Request.CreateResponse(HttpStatusCode.OK, r);
                }
                SophiaSecurityCrypt Newcrip = new SophiaSecurityCrypt(SophiaSecurityCrypt.SophiaTypeCrypt.Simmetric);
                string keyl = SophiaBase64Encoder.Encoder(password + email.Trim('@', ' ', '.'));
                
                var Desencriptar = Newcrip.Desencriptar(existe.Password, (new PasswordDeriveBytes(keyl, null)).GetBytes(32));
                var PassDesencriptada = System.Text.Encoding.UTF8.GetString(Desencriptar);
                if (PassDesencriptada != password)
                {
                    var intentos = existe.Intentos;

                    if (existe.Intentos == 3)
                    {
                        r.status = false;
                        r.message = "Su usuario ha sido bloqueado";
                        db.Pruebas_P_Intentos_Usuario(intentos, email, 2);
                        return Request.CreateResponse(HttpStatusCode.OK, r);
                    }
                    r.status = false;
                    r.message = "Usuario o contraseña incorrecta.";
                    intentos++;
                    db.Pruebas_P_Intentos_Usuario(intentos, email, 1);
                }
                else
                {

                    db.Pruebas_P_Intentos_Usuario(0, email, 1);
                    var token = TokenGenerator.GenerateTokenJwt(email);
                    r.status = true;
                    r.message = "Bienvenido " + existe.NombreEmpleado;
                    r.token = token;
                }

                return Request.CreateResponse(HttpStatusCode.OK, r);

            }
            catch (Exception ex) 
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /////////////////////////Recuperar Cuenta//////////////////////////////
        [HttpPost]
        [Route("recuperar-cuenta")]
        public HttpResponseMessage RecuperarCuenta(string password, string conf_password, string key = null ,string phone = null, string email = null)
        {
            Respuesta r = new Respuesta();
            ObjectParameter m = new ObjectParameter("Mensaje", typeof(string));
            ObjectParameter s = new ObjectParameter("Status", typeof(byte));
            try                                                                
            {
                if (password != conf_password)
                {
                    r.status = false;
                    r.message = "No coinciden las contraseñas";
                    return Request.CreateResponse(HttpStatusCode.OK, r);
                }
                else
                {
                    if (email != null && phone == null)
                    {
                        string tokenEncoder = SophiaBase64Encoder.Encoder(key);
                        var solicitud = (from token in db.Pruebas_SolicitarToken_CambioPassword where (token.Token == tokenEncoder && token.Estado == 0) select token).ToList();
                        if(solicitud.Count == 1)
                        {
                            r.message = validacion.validacionesContrasena(password);
                            if (r.message == "La contraseña es válida.")
                            {
                                SophiaSecurityCrypt Newcrip = new SophiaSecurityCrypt(SophiaSecurityCrypt.SophiaTypeCrypt.Simmetric);
                                var keyc = SophiaBase64Encoder.Encoder(password + email.Trim('@', ' ', '.'));
                                var pass = Newcrip.Encriptar(System.Text.Encoding.UTF8.GetBytes(password), (new PasswordDeriveBytes(keyc, null)).GetBytes(32));
                                db.Pruebas_P_Recuperar_Cuenta(email, pass, null, m, s);
                                db.Pruebas_P_Actualizar_Estado_Token(email);
                                var status = Convert.ToInt32(s.Value);
                                if (status == 1)
                                {
                                    r.status = true;
                                    r.message = m.Value.ToString();
                                }
                                else
                                {
                                    r.status = false;
                                    r.message = m.Value.ToString();
                                }
                            }
                        }       
                    }
                    else
                    {
                        r.message = validacion.validacionesContrasena(password);
                        if (r.message == "La contraseña es válida.")
                        {
                            var num = Convert.ToInt32(phone.Replace("-", ""));
                            var correousuario = (from cor in db.Pruebas_EMP where cor.NumTelefono == num select cor).FirstOrDefault();
                            SophiaSecurityCrypt Newcrip = new SophiaSecurityCrypt(SophiaSecurityCrypt.SophiaTypeCrypt.Simmetric);
                            var keyt = SophiaBase64Encoder.Encoder(password + correousuario.Email.Trim('@', ' ', '.'));
                            var pass = Newcrip.Encriptar(System.Text.Encoding.UTF8.GetBytes(password), (new PasswordDeriveBytes(keyt, null)).GetBytes(32));
                            db.Pruebas_P_Recuperar_Cuenta(null, pass, num, m, s);
                            var status = Convert.ToInt32(s.Value);
                            if (status == 1)
                            {
                                r.status = true;
                                r.message = m.Value.ToString();
                            }
                            else
                            {
                                r.status = false;
                                r.message = m.Value.ToString();
                            }
                        }
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /////////////////////////Registrar Usuario//////////////////////////////
        [HttpPost]
        [Route("registrarse")]
        public HttpResponseMessage Registrarse(string email, string password, string NombreEmpleado, int EdadEmpleado, string phone, string SexoEmpleado)
        {
            try
            {
                Respuesta r = new Respuesta();
                
                var correo = CultureInfo.CurrentCulture.TextInfo.ToLower(email);
                var nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NombreEmpleado);
                ObjectParameter m = new ObjectParameter("Mensaje", typeof(string));
                ObjectParameter s = new ObjectParameter("Status", typeof(byte));

                if (correo != null && password != null && nombre != null && EdadEmpleado != 0 && phone != null && SexoEmpleado != null)
                {
                    r.message = validacion.validacionesContrasena(password);
                    if(r.message == "La contraseña es válida.")
                    {
                        SophiaSecurityCrypt Newcrip = new SophiaSecurityCrypt(SophiaSecurityCrypt.SophiaTypeCrypt.Simmetric);
                        var key = SophiaBase64Encoder.Encoder(password + email.Trim('@', ' ', '.'));
                        var pass = Newcrip.Encriptar(System.Text.Encoding.UTF8.GetBytes(password), (new PasswordDeriveBytes(key,null)).GetBytes(32));
                        db.Pruebas_P_Regristrar_Usuario_Login(nombre, EdadEmpleado, SexoEmpleado, pass, Convert.ToInt32(phone.Replace("-", "")), correo, m, s);
                        var status = Convert.ToInt32(s.Value);
                        if (status == 1)
                        {
                            r.status = true;
                            r.message = m.Value.ToString();
                        }
                        else
                        {
                            r.status = false;
                            r.message = m.Value.ToString();
                        }
                    } 
                }
                else
                {
                    r.status = false;
                    r.message = "Todos los campos son obligatorios.";
                }
                return Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        

        /////////////////////////Clase donde se guarda el status y el message de respuesta//////////////////////////////
        public class Respuesta
        {
            public bool status { get; set; }
            public string message { get; set; }

            public string token { get; set; }
        }

    }
}
