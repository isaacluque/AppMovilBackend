using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using WebApplication3.Models;

namespace WebApplication3.Class
{
    public static class SophiaBase64Encoder
    {
        /*
         * Objeto temporal de codificacion BASE64
         */
        public static string Encoder(this string entrada)
        {
            string salida = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(entrada);
            salida = Convert.ToBase64String(encryted);
            return salida;
        }
        public static string Decoder(this string entrada)
        {
            string salida = string.Empty;
            byte[] decryted = Convert.FromBase64String(entrada);
            salida = System.Text.Encoding.Unicode.GetString(decryted);
            return salida;
        }
    }
    public class SophiaSecurityCrypt
    {
        /*
         * Objeto Criptografico Simetrico y Asimetrico 
         * de seguridad Sophia 2.0
         */
        public enum SophiaTypeCrypt { Asimmetric, Simmetric }
        private RSACryptoServiceProvider AsimmetricCrypt;
        private Rijndael SimmetricCrypt;
        private byte[] PublicKey { get; set; }
        private byte[] PrivateKey { get; set; }
        public SophiaSecurityCrypt(SophiaTypeCrypt Type)
        {
            if (Type == SophiaTypeCrypt.Asimmetric)
            {
                this.AsimmetricCrypt = new RSACryptoServiceProvider(2048);
            }
            if (Type == SophiaTypeCrypt.Simmetric)
            {
                this.SimmetricCrypt = Rijndael.Create();
            }
        }
        public byte[] Encriptar(byte[] Encriptar, byte[] PrivateKey)
        {
            byte[] encrypted = null;
            byte[] returnValue = null;
            try
            {
                SimmetricCrypt.Key = PrivateKey;
                SimmetricCrypt.GenerateIV();
                encrypted = (SimmetricCrypt.CreateEncryptor()).TransformFinalBlock(Encriptar, 0, Encriptar.Length);
                returnValue = new byte[SimmetricCrypt.IV.Length + encrypted.Length];
                SimmetricCrypt.IV.CopyTo(returnValue, 0);
                encrypted.CopyTo(returnValue, SimmetricCrypt.IV.Length);
            }
            catch { }
            finally { SimmetricCrypt.Clear(); }
            return returnValue;
        }
        public byte[] Desencriptar(byte[] Desencriptar, byte[] PrivateKey)
        {
            byte[] tempArray = new byte[SimmetricCrypt.IV.Length];
            byte[] encrypted = new byte[Desencriptar.Length - SimmetricCrypt.IV.Length];
            byte[] returnValue = new byte[] { };
            try
            {
                SimmetricCrypt.Key = PrivateKey;
                var decript = SimmetricCrypt.CreateDecryptor();
                Array.Copy(Desencriptar, tempArray, tempArray.Length);
                Array.Copy(Desencriptar, tempArray.Length, encrypted, 0, encrypted.Length);
                SimmetricCrypt.IV = tempArray;
                returnValue = (SimmetricCrypt.CreateDecryptor()).TransformFinalBlock(encrypted, 0, encrypted.Length);
            }
            catch { }
            finally { SimmetricCrypt.Clear(); }
            return returnValue;
        }
    }
    public class EnviarCorreos
    {
        protected MailMessage Correo = new System.Net.Mail.MailMessage();
        protected SmtpClient Cliente = new System.Net.Mail.SmtpClient();
        public Exception Error { get; set; }
        PruebasEntities db = new PruebasEntities();

        protected void Inicializar()
        {
            Correo.SubjectEncoding = System.Text.Encoding.UTF8;
            Correo.BodyEncoding = System.Text.Encoding.UTF8;
            Correo.IsBodyHtml = true;
            Correo.From = new System.Net.Mail.MailAddress("isaacluque3@gmail.com");
            Cliente.Credentials = new System.Net.NetworkCredential("isaacluque3@gmail.com", "bzhhbugbgtzanbce");
            Cliente.Port = 587;
            //Cliente.Port = 52;
            Cliente.EnableSsl = true;
            Cliente.Host = "smtp.gmail.com";
        }
        protected bool Send()
        {
            try
            {
                Cliente.Send(Correo);
                try
                {
                    string para = "";
                    foreach (var p in Correo.To.ToList())
                    {
                        para += p.Address + ";";
                    }
                    db.Pruebas_P_Set_EmailsLog(para, Correo.Subject, Correo.Body);
                }
                catch { }
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                try
                {
                    this.Error = ex;
                    string para = "";
                    foreach (var p in Correo.To)
                    {
                        para += p.Address + "; ";
                    }
                    if (ex.InnerException != null)
                    {
                        db.Pruebas_P_Set_EmailsLog(para, Correo.Subject, ex.Message + " " + ex.InnerException.Message);
                    }
                    else
                    {
                        db.Pruebas_P_Set_EmailsLog(para, Correo.Subject, ex.Message);
                    }
                }
                catch { }
                return false;
            }
        }
    }
    public class ValidacionesPassword
    {
        public string validacionesContrasena(string contasena)
        {
            string respues = "";

            Regex mayusculaRegex = new Regex(@"[A-Z]");
            Regex minusculaRegex = new Regex(@"[a-z]");
            Regex numeroRegex = new Regex(@"[0-9]");

            if (contasena.Length < 8)
                respues += " - La contraseña debe contener al menos 8 carácteres.";
            if (!mayusculaRegex.IsMatch(contasena))
                respues += " - La contraseña debe contener al menos una letra mayúscula.";
            if (!minusculaRegex.IsMatch(contasena))
                respues += " - La contraseña debe contener al menos una letra minúscula.";
            if (!numeroRegex.IsMatch(contasena))
                respues += " - La contraseña debe contener al menos un número.";
            if (!Regex.IsMatch(contasena, @"^[a-zA-Z0-9]+$"))
                respues += " - La contraseña no debe contener caracteres especiales.";

            if (respues == "")
                return "La contraseña es válida.";
            else
                return respues;
        }
    }
}