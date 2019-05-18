using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EnvioMails
{
    public class Mail
    {
        public int Orden { get; set; }
        public string EmailDestino { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public bool Enviado { get; set; }
        public string Motivo { get; set; }
        public static string Pass { get; set; }
        public static string EmailOrigen { get; set; }

        public Mail()
        {

        }
        public Mail(string emailDestino, string asunto, string cuerpo)
        {
            this.EmailDestino = emailDestino;
            this.Asunto = asunto;
            this.Cuerpo = cuerpo;
        }

        public Mail(string pass, string emailOrigen, string emailDestino, string asunto, string cuerpo) : this(emailDestino, asunto, cuerpo)
        {
            Pass = pass;
            EmailOrigen = emailOrigen;
        }

        public static void EnviarMail(Mail m)
        {
            try
            {
                var gmail = EmailOrigen.Contains("gmail");
                var outlook = EmailOrigen.Contains("outlook") ? true : EmailOrigen.Contains("hotmail") ? true : false;
                MailMessage objeto_mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                /*
                    Nombre de servidor: smtp.office365.com
                    Puerto: 587
                    Método de cifrado: STARTTLS
                 */
                client.Host = gmail ? "smtp.gmail.com" : outlook ? "smtp.office365.com" : throw new Exception("Mail incorrecto");
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new System.Net.NetworkCredential(EmailOrigen, Pass);
                objeto_mail.From = new MailAddress(EmailOrigen);
                objeto_mail.To.Add(new MailAddress(m.EmailDestino));
                objeto_mail.Subject = m.Asunto;
                objeto_mail.Body = m.Cuerpo;
                client.Send(objeto_mail);
                m.Enviado = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Contraseña inválida");
            }
        }
        public bool ValidarMail()
        {
            try
            {
                var addr = new MailAddress(this.EmailDestino);
                return addr.Address == this.EmailDestino;
            }
            catch
            {
                return false;
            }
        }
    }
}
