using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace TCPListener
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        public const uint MOUSEEVENTF_LEFTUP = 0x04;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const uint MOUSEEVENTF_RIGHTUP = 0x10;

        public const uint INPUT_MOUSE = 0;
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
                "volume_up" => "(New-Object -ComObject WScript.Shell).SendKeys([char]175)",
                "volume_down" => "(New-Object -ComObject WScript.Shell).SendKeys([char]174)",
                "shutdown" => "shutdown /s /t 10",
                var cmd when cmd.StartsWith("url:") => $"Start-Process '{cmd.Substring(4)}'",
                var cmd when cmd.StartsWith("mouse_move:") => GetRelativeMouseMoveScript(cmd),
                var cmd when cmd.StartsWith("mouse_click:left") => MouseClickLeft(),
                var cmd when cmd.StartsWith("mouse_click:right") => MouseClickRight(),

                _ => null
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
        public static string MouseClickLeft()
        {
            INPUT inputDown = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dwFlags = MOUSEEVENTF_LEFTDOWN,
                    dx = 0,
                    dy = 0
                }
            };

            INPUT inputUp = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dwFlags = MOUSEEVENTF_LEFTUP,
                    dx = 0,
                    dy = 0
                }
            };

            SendInput(1, new INPUT[] { inputDown }, Marshal.SizeOf(typeof(INPUT)));   // Sol tuş basma
            SendInput(1, new INPUT[] { inputUp }, Marshal.SizeOf(typeof(INPUT)));     // Sol tuş bırakma

            return "";
        }
        public static string MouseClickRight()
        {
            INPUT inputDown = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dwFlags = MOUSEEVENTF_RIGHTDOWN,
                    dx = 0,
                    dy = 0
                }
            };

            INPUT inputUp = new INPUT
            {
                type = INPUT_MOUSE,
                mi = new MOUSEINPUT
                {
                    dwFlags = MOUSEEVENTF_RIGHTUP,
                    dx = 0,
                    dy = 0
                }
            };

            SendInput(1, new INPUT[] { inputDown }, Marshal.SizeOf(typeof(INPUT)));   // Sağ tuş basma
            SendInput(1, new INPUT[] { inputUp }, Marshal.SizeOf(typeof(INPUT)));     // Sağ tuş bırakma

            return "";
        }


        static string GetRelativeMouseMoveScript(string cmd)
        {
            var parts = cmd.Substring("mouse_move:".Length).Split(',');
            if (parts.Length == 2 && int.TryParse(parts[0], out int dx) && int.TryParse(parts[1], out int dy))
            {
                return $@"
                Add-Type -AssemblyName System.Windows.Forms
                $currentPos = [System.Windows.Forms.Cursor]::Position
                $newX = $currentPos.X + {dx}
                $newY = $currentPos.Y + {dy}
                [System.Windows.Forms.Cursor]::Position = New-Object System.Drawing.Point($newX, $newY)";
            }
            return "";
        }
    }
}
