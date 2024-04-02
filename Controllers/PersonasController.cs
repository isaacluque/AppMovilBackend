using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApplication3.Models;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data.Entity.Core.Objects;
using System.Web;
using WebApplication3.Class;
using System.Security.Cryptography;
using System.Web.Helpers;

namespace WebApplication3.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    [RoutePrefix("api/personas")]
    public class PersonasController : ApiController
    {
        PruebasEntities db = new PruebasEntities();
        ValidacionesPassword validacion = new ValidacionesPassword();

        /////////////////////////Mostrar Usuarios//////////////////////////////
        [HttpGet]
        [Route("listado-personas")]
        public List<Pruebas_FN_TodosEmpleados_Result> GetEmpleados()
        {
            return db.Pruebas_FN_TodosEmpleados().ToList();
        }

        /////////////////////////Detalle del Usuario//////////////////////////////
        [HttpGet]
        [Route("detalle-usuario/{Id}")]
        public IQueryable<Pruebas_SoloUnEmpleado_Result> GetEmpleado(int Id)
        {
            return db.Pruebas_SoloUnEmpleado(Id);
        }

        /////////////////////////Mostrar Puestos//////////////////////////////
        [HttpGet]
        [Route("puestos")]
        public List<Pruebas_TraerPuestos_Result> GetPuestos()
        {
            return db.Pruebas_TraerPuestos().ToList();
        }

        /////////////////////////Mostrar Estados//////////////////////////////
        [HttpGet]
        [Route("estados")]
        public List<Pruebas_FN_TraerEstado_Result> GetEstados()
        {
            return db.Pruebas_FN_TraerEstado().ToList();
        }

        /////////////////////////Mostrar foto de usuario//////////////////////////////
        [HttpGet]
        [Route("imagen/{Id}")]
        public HttpResponseMessage ConvertByteToImage(int Id)
        {
            Respuesta r = new Respuesta();
            try
            {
                var extension = db.Pruebas_FN_ImagenUnEmpleado(Id).Select(a => a.Extension).FirstOrDefault();

                byte[] dd = db.Pruebas_FN_ImagenUnEmpleado(Id).Select(e => e.FotografiaEmpleado).FirstOrDefault();
                if (dd == null)
                {
                    r.status = true;
                    r.message = null;
                }
                else
                {
                    var imreBase64Data = Convert.ToBase64String(dd);
                    string imgDataUrl = string.Format("data:" + extension + ";base64,{0}", imreBase64Data);
                    r.status = true;
                    r.message = imgDataUrl;
                }

                return Request.CreateResponse(HttpStatusCode.OK, r);
            }
            catch
            {
                r.status = false;
                r.message = "Hubo un problema, por favor hablar con sistemas.";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, r);
            }
        }

        /////////////////////////Mostrar horas de usuario//////////////////////////////
        [HttpGet]
        [Route("horas/{Id}")]
        public List<Object> GetCotrolHoras(int Id)
        {
            List<Pruebas_FN_ControlHoras_Result> horas = new List<Pruebas_FN_ControlHoras_Result>();
            horas = db.Pruebas_FN_ControlHoras(Id).ToList();
            List<Object> list = new List<Object>();

            horas.ToList().ForEach(i =>
            {
                list.Add(new
                {
                    Id = i.Id,
                    RefIdEmpleado = i.RefIdEmpleado,
                    HoraEntrada = extraerHora(i.HoraEntrada),
                    HoraSalida = extraerHora((TimeSpan)i.HoraSalida),
                    Fecha = i.Fecha,
                });
            });

            return list;
        }

        public static string extraerHora(TimeSpan extraerFormatoHora)
        {

            var hora = extraerFormatoHora.Hours;
            var minutos = extraerFormatoHora.Minutes;
            var segundos = extraerFormatoHora.Seconds;
            var HoraConcatenada = $"{hora:D2}:{minutos:D2}:{segundos:D2}";

            return HoraConcatenada;
        }

        

        /////////////////////////Editar Usuario y su foto//////////////////////////////
        [HttpPost]
        [Route("editar-usuario/{Id}")]
        public async Task<HttpResponseMessage> PutUsuario(int Id, string nombreEmpleado = "", int edadEmpleado = 0, string sexoEmpleado = "", string Email = "", int idPuesto = 0, int idEstado = 0, string numTelefono = "", string extension = "")
        {
            Respuesta r = new Respuesta();
            ObjectParameter m = new ObjectParameter("Mensaje", typeof(string));
            ObjectParameter s = new ObjectParameter("Status", typeof(byte));
            try
            {

                if (Request.Content.IsMimeMultipartContent() && !Request.Content.IsMimeMultipartContent() || !Request.Content.IsMimeMultipartContent() == null)
                {
                    r.status = false;
                    r.message = "Falta la imagen o se envío un formato incorrecto.";
                    return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                }
                else
                {
                    var correoelectronico = "";
                    if (Email.ToString() == "null")
                    {
                        correoelectronico = "";
                    }
                    else
                    {
                        correoelectronico = Email;
                    }
                    var numT = Convert.ToInt32(numTelefono.Replace("-",""));
                    var nombreExiste = (from n in db.Pruebas_EMP where (n.NombreEmpleado == nombreEmpleado && n.NombreEmpleado != null && n.Id != Id) select n).Count();
                    var emailExiste = (from e in db.Pruebas_EMP where (e.Email == correoelectronico && e.Email != null && e.Id != Id) select e).Count();
                    var telefonoExiste = (from t in db.Pruebas_EMP where (t.NumTelefono == numT && t.NumTelefono != null && t.Id != Id) select t).Count();

                    if (nombreExiste != 0)
                    {
                        r.status = false;
                        r.message = "Ya existe un usuario con estas características. Nombre";
                        return Request.CreateResponse(HttpStatusCode.OK, r);
                    }
                    else if (emailExiste != 0)
                    {
                        r.status = false;
                        r.message = "Ya existe un usuario con estas características. Email";
                        return Request.CreateResponse(HttpStatusCode.OK, r);
                    }
                    else if (telefonoExiste != 0)
                    {
                        r.status = false;
                        r.message = "Ya existe un usuario con estas características. Telefono";
                        return Request.CreateResponse(HttpStatusCode.OK, r);
                    }
                    else
                    {
                        var nombreCapitalizado = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nombreEmpleado);
                        var correoMinuscula = CultureInfo.CurrentCulture.TextInfo.ToLower(correoelectronico);
                        var empleado = (from d in db.Pruebas_EMP where d.Id == Id select d).FirstOrDefault();
                        var nombre = nombreCapitalizado != "" ? nombreCapitalizado : empleado.NombreEmpleado.ToString();
                        var correo = correoMinuscula != "" ? correoMinuscula : empleado.Email.ToString();
                        var edad = edadEmpleado != 0 ? edadEmpleado : empleado.EdadEmpleado;
                        var telefono = numT != 0 ? numT : empleado.NumTelefono;
                        var sexo = sexoEmpleado != "" ? sexoEmpleado : empleado.SexoEmpleado.ToString();
                        var idpuesto = idPuesto != 0 ? idPuesto : empleado.IdPuesto;
                        var idestado = idEstado != 0 ? idEstado : empleado.IdEstado;

                        try
                        {
                            if (Request.Content.Headers.ContentType == null)
                            {
                                var imagenUser = (from imagen in db.Pruebas_EMP where imagen.Id == Id select imagen).FirstOrDefault();
                                db.Pruebas_EditarEmpleado(Id, nombre, edad, telefono, sexo, correo, idpuesto, idestado, imagenUser.FotografiaEmpleado, imagenUser.Extension, m, s);

                                var status = Convert.ToInt32(s.Value);
                                if (status == 1)
                                {
                                    r.status = true;
                                    r.message = m.Value.ToString();
                                }
                            }
                            else
                            {
                                // Read the form data.
                                string root = System.Web.HttpContext.Current.Server.MapPath("~/Imagenes");
                                var provider = new MultipartFormDataStreamProvider(root);
                                byte[] data = null;
                                await Request.Content.ReadAsMultipartAsync(provider);
                                // This illustrates how to get the file names.
                                foreach (MultipartFileData file in provider.FileData)
                                {
                                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);//get FileName
                                    Trace.WriteLine("Server file path: " + file.LocalFileName);//get File Path
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        using (FileStream fs = File.OpenRead(file.LocalFileName))
                                        {
                                            await fs.CopyToAsync(ms);
                                        }
                                        data = ms.ToArray();
                                    }
                                    db.Pruebas_EditarEmpleado(Id, nombre, edad, telefono, sexo, correo, idpuesto, idestado, data, extension, m, s);
                                    File.Delete(file.LocalFileName);
                                }

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
                        catch (Exception e)
                        {
                            r.status = false;
                            r.message = e.Message;
                            return Request.CreateResponse(HttpStatusCode.OK, r);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                r.status = false;
                r.message = "Hubo un problema, por favor hablar con sistemas.";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        /////////////////////////Elimnar Usuario//////////////////////////////
        [HttpDelete]
        [Route("eliminar-usuario/{Id}")]
        public HttpResponseMessage DeleteUsuario(int Id)
        {
            try
            {
                Respuesta r = new Respuesta();
                if (Id == 0)
                {

                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                var pruebas_SoloUnEmpleado_Result = db.Pruebas_SoloUnEmpleado(Id);
                if (pruebas_SoloUnEmpleado_Result == null)
                {
                    r.status = false;
                    r.message = "No existe este usuario.";
                    return Request.CreateResponse(HttpStatusCode.NotFound, r);
                }
                else
                {
                    db.Pruebas_EliminarEmpleado(Id);
                    r.status = false;
                    r.message = "Usuario eliminado.";
                    return Request.CreateResponse(HttpStatusCode.OK, r);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        /////////////////////////Crear Usuario//////////////////////////////
        [HttpPost]
        [Route("crear-usuario")]
        public HttpResponseMessage PostUsuario(string NombreEmpleado, int EdadEmpleado, string SexoEmpleado, int IdPuesto, int IdEstado, string Password, string Confirm_Password, string Numtelefono, string Email)
        {
            try
            {
                Respuesta r = new Respuesta();
                ObjectParameter m = new ObjectParameter("Mensaje", typeof(string));
                ObjectParameter s = new ObjectParameter("Status", typeof(byte));
                if (Confirm_Password != Password)
                {
                    r.status = false;
                    r.message = "Contraseñas no coinciden";
                    return Request.CreateResponse(HttpStatusCode.OK, r);
                }
                else
                {
                    if (NombreEmpleado != null && EdadEmpleado != 0 && SexoEmpleado != null && IdPuesto != 0 && IdEstado != 0 && Password != null && Numtelefono != null && Email != null)
                    {
                        r.message = validacion.validacionesContrasena(Password);
                        if (r.message == "La contraseña es válida.")
                        {
                            var correo = CultureInfo.CurrentCulture.TextInfo.ToLower(Email);
                            var nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NombreEmpleado);
                            
                            SophiaSecurityCrypt Newcrip = new SophiaSecurityCrypt(SophiaSecurityCrypt.SophiaTypeCrypt.Simmetric);
                            var keycu = SophiaBase64Encoder.Encoder(Password + correo.Trim('@', ' ', '.'));
                            var pass = Newcrip.Encriptar(System.Text.Encoding.UTF8.GetBytes(Password), (new PasswordDeriveBytes(keycu, null)).GetBytes(32));
                            db.Pruebas_P_CrearEmpleado(nombre, EdadEmpleado, SexoEmpleado, pass, Convert.ToInt32(Numtelefono.Replace("-", "")), correo, IdPuesto, IdEstado, null, m, s);
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
        }

    }
}