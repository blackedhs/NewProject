using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EnvioMails
{
    public class Proceso
    {
        public static List<string> ListaContactos { get; set; }

        public bool Iniciar(Dato dato)
        {
            try
            {
                CargarContactos();
                foreach (var destino in ListaContactos)
                {
                    var m = new Mail(dato, destino);
                    Proceso.EnviarMail(m);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
        private void CargarContactos()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Datos");
            var pathArchivo = Path.Combine(path, "Contactos.txt");
            ListaContactos = new List<string>();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(pathArchivo))
                throw new Exception("No existe archivo");
            using (StreamReader archivo = new StreamReader(pathArchivo))
            {
                while (!archivo.EndOfStream)
                {
                    var linea = archivo.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                        continue;
                    if (Mail.ValidarMail(linea.Trim()))
                        ListaContactos.Add(linea.Trim());
                }
            }
        }

        public static void EnviarMail(Mail m)
        {
            try
            {
                SmtpClient client = ObtenerCliente(ref m);
                client.Send(m);
                m.Enviado = true;
            }
            catch (Exception ex)
            {
                ex = new Exception("Contraseña inválida");
                throw ex;
            }
        }

        private static SmtpClient ObtenerCliente(ref Mail m)
        {
            var EmailOrigen = Mail.Dato.Origen;
            var gmail = EmailOrigen.Contains("gmail");
            var outlook = EmailOrigen.Contains("outlook") ? true : EmailOrigen.Contains("hotmail") ? true : false;

            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.Host = gmail ? "smtp.gmail.com" : outlook ? "smtp.office365.com" : string.Empty;
            if(string.IsNullOrEmpty(client.Host))
                throw new Exception("Mail incorrecto");
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential(EmailOrigen, Mail.Dato.Clave);
            return client;
        }
    }
}
