using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Karin35
{
    public sealed partial class FloatingBallon
    {
        private readonly Random _rnd;
        private readonly DispatcherTimer _timer;

        public FloatingBallon(int seed)
        {
            InitializeComponent();

            _image.Visibility = Visibility.Collapsed;

            _rnd = new Random(seed);
            _timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1 + _rnd.NextDouble()*4)};
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void TimerTick(object sender, object e)
        {
            _image.Visibility = Visibility.Visible;

            var canvasParent = Parent as Canvas;
            if (canvasParent == null) return;

            SetValue(Canvas.LeftProperty, (double) _rnd.Next(0, (int)canvasParent.ActualWidth));
            SetValue(Canvas.TopProperty, canvasParent.ActualHeight);

            _timer.Stop();
            _sb.Begin();
        }

        private void _sb_OnCompleted(object sender, object e)
        {
            _image.Visibility = Visibility.Collapsed;
            _timer.Interval = TimeSpan.FromSeconds(1 + _rnd.NextDouble()*3);
            _timer.Start();
        }

    }
}
