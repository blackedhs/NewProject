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
                    var m = new Mail(dato.Origen, dato.Clave, dato.Asunto, dato.Cuerpo, dato.Adjunto, destino);
                    //Proceso.EnviarMail(m);
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
            var path = @"C:\Users\usuario\Desktop\Mail\EnvioMails\EnvioMails\bin\Debug\Datos";
            var pathArchivo = Path.Combine(path, "Contactos.txt");
            ListaContactos = new List<string>();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!File.Exists(pathArchivo))
                return;
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
                SmtpClient client = ObtenerCliente(m);
                client.Send(m);
                m.Enviado = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Contraseña inválida");
            }
        }

        private static SmtpClient ObtenerCliente(Mail m)
        {
            var EmailOrigen = Mail.Dato.Origen;
            var gmail = EmailOrigen.Contains("gmail");
            var outlook = EmailOrigen.Contains("outlook") ? true : EmailOrigen.Contains("hotmail") ? true : false;
            var cliente = new SmtpClient();
            cliente.Port = 25;
            /*
                Nombre de servidor: smtp.office365.com
                Puerto: 587
                Método de cifrado: STARTTLS
             */
            cliente.Host = gmail ? "smtp.gmail.com" : outlook ? "smtp.office365.com" : throw new Exception("Mail incorrecto");
            cliente.Timeout = 10000;
            cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
            cliente.EnableSsl = true;
            cliente.Credentials = new System.Net.NetworkCredential(m.To.ToString(), Mail.Dato.Clave);
            return cliente;
        }
    }
}
