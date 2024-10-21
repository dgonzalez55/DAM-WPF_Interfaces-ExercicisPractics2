using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Ex10_AutoImgGallery
{
    public enum GalleryMode { Auto, Manual }
    /// <summary>
    /// Lógica de interacción para AdvImgGallery.xaml
    /// </summary>
    public partial class AdvImgGallery : UserControl
    {
        private int _index = 0;
        private List<string> _imatges = new List<string>();

        public AdvImgGallery()
        {
            InitializeComponent();
            _timer = new Timer(ChangeImage, null, 0, _waitMillis);
        }

        private GalleryMode _mode = GalleryMode.Auto;
        public GalleryMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                if (_mode == GalleryMode.Manual)
                {
                    StopTimer();
                }
                else
                {
                    ReStartTimer();
                }
            }
        }

        private string? _directoriImatges;
        public string? DirectoriImatges
        {
            get => _directoriImatges;
            set
            {
                _directoriImatges = value;
                LoadImagesFromFolder();
            }
        }

        private bool _timerRunning = true;
        private Timer? _timer;

        private int _waitMillis = 2000;
        public int WaitMillis
        {
            get => _waitMillis;
            set
            {
                _waitMillis = value;
                if (_imatges.Count > 0)
                {
                    ReStartTimer();
                }
            }
        }

        // Carrega les imatges del directori especificat
        private void LoadImagesFromFolder()
        {
            if (string.IsNullOrEmpty(_directoriImatges)) return;
            // Resol la ruta relativa des de l'executable actual
            var rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _directoriImatges);

            if (Directory.Exists(rutaCompleta))
            {
                _imatges = new List<string>(Directory.GetFiles(rutaCompleta, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".png", StringComparison.OrdinalIgnoreCase)));

                if (_imatges.Count > 0)
                {
                    _index = 0;
                    LoadImage();
                }
                else
                {
                    MessageBox.Show("No s'han trobat imatges a la carpeta especificada.");
                }
            }
            else
            {
                MessageBox.Show("El directori especificat no existeix.");
            }
        }

        // Carrega la imatge actual amb una animació de dissolució
        private void LoadImage()
        {
            if (_imatges.Count == 0) return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Imatge.Source = new BitmapImage(new Uri(_imatges[_index], UriKind.Absolute));

                // Animació de transició (dissolució)
                var anim = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
                Imatge.BeginAnimation(UIElement.OpacityProperty, anim);

                var zoomInAnimX = new DoubleAnimation(0.8, 1, TimeSpan.FromSeconds(0.5));
                var zoomInAnimY = new DoubleAnimation(0.8, 1, TimeSpan.FromSeconds(0.5));

                ImageScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, zoomInAnimX);
                ImageScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, zoomInAnimY);
            });
        }
        private void ChangeImage(object? state)
        {
            MoveImage(1);
            if (_timerRunning) ReStartTimer();
        }

        private void MoveImage(int direction)
        {
            if (_imatges.Count == 0) return;

            _index = (_index + direction + _imatges.Count) % _imatges.Count;
            LoadImage();
        }

        private void ReStartTimer()
        {
            _timerRunning = true;
            _timer?.Change(_waitMillis, _waitMillis);
        }
        private void StopTimer()
        {
            _timerRunning = false;
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void ToggleTimer()
        {
            if (_timerRunning)
            {
                StopTimer();
            }
            else
            {
                ReStartTimer();
            }
        }

        private void Anterior_Click(object sender, RoutedEventArgs e) => MoveImage(-1);
        private void Següent_Click(object sender, RoutedEventArgs e) => MoveImage(1);

        private void Imatge_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_mode == GalleryMode.Manual)
            {
                MoveImage(1);
            }
            else
            {
                ToggleTimer();
            }
        }
    }

}
