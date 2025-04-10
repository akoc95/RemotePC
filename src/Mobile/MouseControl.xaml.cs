using System.Net.Sockets;
using System.Text;

namespace Mobile
{
    public partial class MouseControl : ContentPage, IDrawable
    {
        private List<TrailPoint> _points = new();
        private Point? _lastTouch = null;
        private System.Timers.Timer _fadeTimer;
        private DateTime lastClearTime = DateTime.MinValue;
        private double _totalDeltaX = 0;
        private double _totalDeltaY = 0;

        public MouseControl()
        {
            InitializeComponent();
            BindingContext = this;

            _fadeTimer = new System.Timers.Timer(50);
            _fadeTimer.Elapsed += (s, e) =>
            {
                var pointsCopy = _points.ToList();

                for (int i = pointsCopy.Count - 1; i >= 0; i--)
                {
                    var p = pointsCopy[i];
                    p.Opacity -= 0.05;

                    if (p.Opacity <= 0)
                    {
                        _points.Remove(p);
                    }
                }

                if (DateTime.Now - lastClearTime > TimeSpan.FromSeconds(2))
                {
                    _points.Clear();
                    lastClearTime = DateTime.Now;
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    DrawingCanvas.Invalidate();
                });
            };

            _fadeTimer.Start();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var pointsCopy = _points.ToList();

            foreach (var point in pointsCopy)
            {
                canvas.StrokeColor = Colors.IndianRed.WithAlpha((float)point.Opacity);
                canvas.StrokeSize = 3;
                canvas.DrawLine((float)point.PreviousX, (float)point.PreviousY, (float)point.X, (float)point.Y);
            }
        }

        private void OnStartInteraction(object sender, TouchEventArgs e)
        {
            _lastTouch = e.Touches.FirstOrDefault();
        }

        private void OnDragInteraction(object sender, TouchEventArgs e)
        {
            if (_lastTouch == null || e.Touches.Count() == 0)
                return;

            var current = e.Touches[0];

            double deltaX = current.X - _lastTouch.Value.X;
            double deltaY = current.Y - _lastTouch.Value.Y;

            _lastTouch = current;

            if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
            {
                _points.Add(new TrailPoint
                {
                    X = current.X,
                    Y = current.Y,
                    PreviousX = current.X - deltaX,
                    PreviousY = current.Y - deltaY,
                    Opacity = 1.0
                });

                DrawingCanvas.Invalidate();
                _totalDeltaX += deltaX;
                _totalDeltaY += deltaY;
            }
        }

        private async void OnEndInteraction(object sender, TouchEventArgs e)
        {
            _lastTouch = null;

            if (Math.Abs(_totalDeltaX) > 1 || Math.Abs(_totalDeltaY) > 1)
            {
                await SendCommand($"mouse_move:{(int)_totalDeltaX},{(int)_totalDeltaY}");
            }

            _totalDeltaX = 0;
            _totalDeltaY = 0;
        }

        private async Task SendCommand(string command)
        {
            string serverIp = Preferences.Get("ServerIp", "192.168.1.100");
            int serverPort = int.Parse(Preferences.Get("ServerPort", "5000"));

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.SendTimeout = 3;
                    await client.ConnectAsync(serverIp, serverPort);
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(command);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bağlantı hatası: {ex.Message}", "Tamam");
            }
        }

        private async void LeftClick_Clicked(object sender, EventArgs e)
        {
            await SendCommand("mouse_click:left");
        }

        private async void RightClick_Clicked(object sender, EventArgs e)
        {
            await SendCommand("mouse_click:right");
        }

        private class TrailPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double PreviousX { get; set; }
            public double PreviousY { get; set; }
            public double Opacity { get; set; } = 1.0;
        }
    }
}
