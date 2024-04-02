using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Security.Policy;
using System.Web;
using static System.Net.Mime.MediaTypeNames;
using System.Web.UI.WebControls;

namespace WebApplication3.Class 
{
    public class SendEmail : EnviarCorreos
    {
        protected string StyleEmail { get { return "<style type='text/css'>@media only screen and (min-width: 620px){.u-row{width: 600px !important;}.u-row .u-col{vertical-align: top;}.u-row .u-col-100{width: 600px !important;}}.u-row .u-col{min-width: 320px !important;max-width: 100% !important;display: block !important;}.u-row{width: 100% !important;}.u-col{width: 100% !important;}.u-col>div{margin: 0 auto;}body{margin: 0;padding: 0;}table,tr,td{vertical-align: top;border-collapse: collapse;}p{margin: 0;}.ie-container table,.mso-container table {table-layout: fixed;}*{line-height: inherit;}a[x-apple-data-detectors='true'] {color: inherit !important;text-decoration: none !important;}table,td{color: #000000;}@media (max-width: 480px){#u_content_image_2 .v-container-padding-padding {padding: 50px 10px 10px !important;}#u_content_image_2 .v-src-width{width: auto !important;}#u_content_image_2 .v-src-max-width{max-width: 45% !important;}#u_content_heading_1 .v-font-size{font-size: 26px !important;}#u_content_text_1 .v-container-padding-padding{padding: 5px 10px 10px !important;}}</style><link href='https://fonts.googleapis.com/css?family=Raleway:400,700&display=swap' rel='stylesheet' type='text/css'>"; } }
        public SendEmail() {
            this.Inicializar();
        }

        public bool enviarToken(string correo, string token, string usuario)
        {
            Correo.To.Add(correo);
            Correo.Subject = "Recuperación de cuenta";
            Correo.Body = ("<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional //EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>");
            Correo.Body += ("<html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'>");
            Correo.Body += ("<head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'><meta name='x-apple-disable-message-reformatting'><meta http-equiv='X-UA-Compatible' content='IE=edge'><title></title>");
            Correo.Body += (this.StyleEmail);
            Correo.Body += ("</head>");
            Correo.Body += ("<body class='clean-body u_body' style='margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #ffffff;color: #000000'>");
            Correo.Body += ("<table style='border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #ffffff;width:100%' cellpadding='0' cellspacing='0'><tbody><tr style='vertical-align: top'><td style='word-break: break-word;border-collapse: collapse !important;vertical-align: top'>");
            Correo.Body += ("<div class='u-row-container' style='padding: 0px;background-color: transparent'>");
            Correo.Body += ("<div class='u-row' style='margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;''>");
            Correo.Body += ("<div style='border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;'>");
            Correo.Body += ("<div class='u-col u-col-100' style='max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;'>");
            Correo.Body += ("<div style='background-color: #4264f0;height: 100%;width: 100% !important;'>");
            Correo.Body += ("<div style='box-sizing: border-box; height: 100%; padding: 10px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;'>");
            Correo.Body += ("<table id='u_content_image_2' style='font-family:'Raleway',sans-serif;' role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr>");
            Correo.Body += ("<td class='v-container-padding-padding' style='overflow-wrap:break-word;word-break:break-word;padding:100px 10px 10px;font-family:'Raleway',sans-serif;' align='left'>");
            Correo.Body += ("<table width='100%' cellpadding='0' cellspacing='0' border='0'><tr><td style='padding-right: 0px;padding-left: 0px;' align='center'>");
            Correo.Body += ("<img align='center' border='0' src='https://cdn.templates.unlayer.com/assets/1701416668612-Group%202.png' alt='image' title='image' style='outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 42%;max-width: 243.6px;' width='243.6' class='v-src-width v-src-max-width' />");
            Correo.Body += ("</td></tr></table></td></tr></tbody></table>");
            Correo.Body += ("</div></div></div></div></div></div>");
            Correo.Body += ("<div class='u-row-container' style='padding: 0px;background-color: transparent'>");
            Correo.Body += ("<div class='u-row' style='margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;''>");
            Correo.Body += ("<div style='border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;'>");
            Correo.Body += ("<div class='u-col u-col-100' style='max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;'>");
            Correo.Body += ("<div style='background-color: #4264f0;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;'>");
            Correo.Body += ("<div style='box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;'>");
            Correo.Body += ("<table style='font-family:'Raleway',sans-serif;' role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr>");
            Correo.Body += ("<td class='v-container-padding-padding' style='overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:'Raleway',sans-serif;' align='left'>");
            Correo.Body += ("<div class='v-font-size' style='font-size: 14px; color: #ffffff; line-height: 140%; text-align: center; word-wrap: break-word;'>");
            Correo.Body += ("<p style='line-height: 140%;'>¡Hola usuario! " + usuario + ", para poder cambiar su contraseña debe de ingresar el siguiente token.</p>");
            Correo.Body += ("</div></td></tr></tbody></table>");
            Correo.Body += ("</div></div></div></div></div></div>");
            Correo.Body += ("<div class='u-row-container' style='padding: 0px;background-color: transparent'>");
            Correo.Body += ("<div class='u-row' style='margin: 0 auto;min-width: 320px;max-width: 600px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;''>");
            Correo.Body += ("<div style='border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;'>");
            Correo.Body += ("<div class='u-col u-col-100' style='max-width: 320px;min-width: 600px;display: table-cell;vertical-align: top;'>");
            Correo.Body += ("<div style='background-color: #4264f0;height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;'>");
            Correo.Body += ("<div style='box-sizing: border-box; height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;'>");
            Correo.Body += ("<table id='u_content_text_1' style='font-family:'Raleway',sans-serif;' role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr>");
            Correo.Body += ("<td class='v-container-padding-padding' style='overflow-wrap:break-word;word-break:break-word;padding:5px 50px 10px;font-family:'Raleway',sans-serif;' align='left'>");
            Correo.Body += ("<div class='v-font-size' style='font-size: 14px; color: #ced4d9; line-height: 140%; text-align: center; word-wrap: break-word;'>");
            Correo.Body += ("<p style='line-height: 140%;'><span data-metadata='&lt;!--(figmeta)eyJmaWxlS2V5IjoiNjVKak1FdmhsNGVRclpybThWU3JnaSIsInBhc3RlSUQiOjQ0MjAzNDA0NywiZGF0YVR5cGUiOiJzY2VuZSJ9Cg==(/figmeta)--&gt;' style='line-height: 19.6px;'>");
            Correo.Body += ("</span>Token: "+token);
            Correo.Body += ("</p></div></td></tr></tbody></table>");
            Correo.Body += ("<table style='font-family:'Raleway',sans-serif;' role='presentation' cellpadding='0' cellspacing='0' width='100%' border='0'><tbody><tr>");
            Correo.Body += ("<td class='v-container-padding-padding' style='overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:'Raleway',sans-serif;' align='left'>");
            Correo.Body += ("<div class='v-font-size' style='font-size: 14px; color: #ffffff; line-height: 140%; text-align: center; word-wrap: break-word;'>");
            Correo.Body += ("<p style='line-height: 140%;'>El token tiene un tiempo límite de 30 minutos.</p>");
            Correo.Body += ("</div></td></tr></tbody></table>");
            Correo.Body += ("</div></div></div></div></div></div>");
            Correo.Body += ("</td></tr></tbody></table></body></html>");

            return this.Send();
        }
    }
}

