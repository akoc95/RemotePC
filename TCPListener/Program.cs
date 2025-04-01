using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPListener
{
    class Program
    {
        static void Main()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            Console.WriteLine("Dinleme başlatıldı. Komut bekleniyor...");

            while (true)
            {
                using (TcpClient client = server.AcceptTcpClient())
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string command = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                    Console.WriteLine($"Komut alındı: {command}");
                    ExecuteCommand(command);
                }
            }
        }

        static void ExecuteCommand(string command)
        {
            string? commandToRun = command switch
            {
                "volume_up" => "(New-Object -ComObject WScript.Shell).SendKeys([char]175)", // Ses açma
                "volume_down" => "(New-Object -ComObject WScript.Shell).SendKeys([char]174)", // Ses kısmak
                "shutdown" => "shutdown /s /t 10", // Bilgisayarı kapatma
                var cmd when cmd.StartsWith("url:") => $"Start-Process '{cmd.Substring(4)}'",
                _ => null // Geçersiz komut
            };

            if (commandToRun != null)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = commandToRun,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }
        }
    }
}
