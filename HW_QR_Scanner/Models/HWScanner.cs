using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HW_QR_Scanner.Models
{
    public class HWScanner : IDisposable
    {
        private readonly SerialPort port;
        private readonly CancellationTokenSource _cts = new();

        /* This event should be processed outside of a class */
        public event Action<string>? DataReceived;

        public HWScanner(string port_name = "COM5", int baudrate = 115200)
        {
            /* To be stored in config 
             * port_name
              baudrate */

            port = new SerialPort(port_name, baudrate)
            {
                NewLine = "\r\n",
                ReadTimeout = 100,
            };
        }

        public void StartScan()
        {
            // Open serial port --> dispatch all data left in buffer previosely
            if (!port.IsOpen)
                port.Open();

            port.DiscardInBuffer();

            // Start polling task
            Task.Run(SerialPolling, _cts.Token);
        }

        public void StopScan()
        {
            _cts.Cancel();

            if (port.IsOpen)
                port.Close();
        }

        private async Task SerialPolling()
        {
            byte[] data = new byte[64]; // Do I need this ? 

            CancellationToken token = _cts.Token;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    while (port.BytesToRead > 0)
                    {
                        string id = port.ReadLine();
                        DataReceived?.Invoke(id);
                    }
                }
                catch (TimeoutException)
                {
                    // serial read timed out — ignore and continue polling
                    ;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Serial error: " + ex.Message);
                }

                // Poll every 100 ms.
                await Task.Delay(100, token);
            }
        }
        public void Dispose()
        {
            _cts.Cancel();

            if (port.IsOpen)
                port.Close();

            _cts.Dispose();
            port.Dispose();
        }
    }
}
