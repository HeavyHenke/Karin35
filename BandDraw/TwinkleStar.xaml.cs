using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace BandDraw
{
    public sealed partial class TwinkleStar : UserControl
    {
        private readonly Random _rnd;
        private DispatcherTimer _timer;

        public TwinkleStar(int seed)
        {
            InitializeComponent();

            _rnd = new Random(seed);
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(_rnd.Next(2, 5)) };
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void TimerTick(object sender, object e)
        {
            _sb.Begin();
            _timer.Interval = TimeSpan.FromSeconds(_rnd.Next(4, 7));
            _timer.Start();
        }

        private void _sb_OnCompleted(object sender, object e)
        {
        }

    }
}
