using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using _77NeoWeb.prg;
using System.Data;
using System.Data.SqlClient;
namespace _77NeoWeb.Prg
{
    public class IntervTaHSvc : IHostedService, IDisposable
    {
        private Timer _timer;
        AlertRvaNew Alrt = new AlertRvaNew();
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(SaveFile, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }
        public void SaveFile(object state)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                // Guardar valor en sesión
                HttpContext.Current.Session["MiVariable"] = "Valor a las " + DateTime.Now.ToString();
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}