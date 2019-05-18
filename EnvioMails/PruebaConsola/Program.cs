using EnvioMails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Ingrese cuenta: ");
                var origen = Console.ReadLine();
                Console.Write("Password: ");
                var pass = Console.ReadLine();
                Console.Write("Ingrese asunto: ");
                var asunto = Console.ReadLine();
                Console.Write("Ingrese: ");
                var cuerpo = Console.ReadLine();

                var listaContactos = new List<string>();
                //listaContactos.Add("rodrigoebravo@hotmail.com");
                //listaContactos.Add("rodrigoebravo@hotmail.com");
                //listaContactos.Add("rodrigoebravo07@gmail.com");
                listaContactos.Add("eliorodrigobravo@gmail.com");

                foreach (var mail in listaContactos)
                {
                    var m = new Mail(pass, origen, mail, asunto, cuerpo);
                    m.ValidarMail();
                    Mail.EnviarMail(m);
                    Console.WriteLine("OK");
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
