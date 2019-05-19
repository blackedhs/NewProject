using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EnvioMails
{
    public class Mail : MailMessage
    {
        public static Dato Dato { get; set; }
        public string Destino { get; set; }
        public bool Enviado { get; set; }
        public string Motivo { get; set; }
        private static int CambioMail { get; set; }
        public Mail(string origen, string clave, string asunto, string cuerpo, Object adjunto, string destino, bool cambioMail) : this(destino, cambioMail)
        {
            if (CambioMail == 0)
            {
                Dato = new Dato(origen, clave, asunto, cuerpo, adjunto);
                this.From = new MailAddress(origen);
                this.Subject = asunto;
                this.Body = cuerpo;
                CambioMail = 1;
                return;
            }
            this.Motivo = string.Empty;
        }
        private Mail(string destino, bool cambioMail)
        {
            if (cambioMail)
                CambioMail = 0;


            this.To.Add(new MailAddress(destino));
            this.Enviado = false;
        }

        public static bool ValidarMail(string mail)
        {
            try
            {
                var addr = new MailAddress(mail);
                return addr.Address == mail;
            }
            catch
            {
                return false;
            }
        }
    }
}
