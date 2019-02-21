using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace RobotArm.Entidades
{
    public class RobotArmSender
    {
        public HttpClient client;
        public string IpRobotArm { get; set; }

        public RobotArmSender(string ip)
        {
            IpRobotArm = ip;
            client = new HttpClient();
            client.BaseAddress = new Uri("http://" + ip + "//");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.MaxResponseContentBufferSize = 256000;
        }

        private async void EnviarMovimiento(Angulo movimiento)
        {
            try
            {
                char[] charsToTrim = { '*', ' ', '\'' };
                string result = (movimiento.Motor + "=" + movimiento.Valor).Trim(charsToTrim);
                HttpResponseMessage response = await client.GetAsync(result).ConfigureAwait(false);
                //return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
            }
        }

        private void Movimiento(Angulo movimiento)
        {
            EnviarMovimiento(movimiento);
            //   return EnviarMovimiento(movimiento).Result;
        }

        public void EnviarMovimientos(List<Angulo> movimientos)
        {
            //string resultado = "";
            foreach (var item in movimientos)
            {
                //resultado =
                Movimiento(item);

                Thread.Sleep(500);
            }
            //return resultado;
        }
    }
}
