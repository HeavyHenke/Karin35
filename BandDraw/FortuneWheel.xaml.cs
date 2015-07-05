using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

using static System.Math;

namespace BandDraw
{
    public sealed partial class FortuneWheel
    {
        private readonly List<Line> _lines = new List<Line>();
        private readonly List<TextBlock> _textBlocks = new List<TextBlock>();

        private int _numPossibilities = 10;

        public event Action SpinComplete;

        public string Values
        {
            get { return String.Join(", ", _textBlocks.Select(t => t.Text)); }
            set
            {
                var vals = value.Split(',').Select(a => a.Trim()).Where(b => !string.IsNullOrWhiteSpace(b)).ToList();
                for (int i = 0; i < vals.Count && i < 10; i++)
                {
                    _textBlocks[i].Text = vals[i];
                }

                SetNumPossibilities(Min(vals.Count, 10));
            }
        }

        public string CorrectValue { get; set; }

        public string Title
        {
            get { return _title.Text; }
            set { _title.Text = value; }
        }

        public FortuneWheel()
        {
            InitializeComponent();

            for (int i = 0; i < 10; i++)
            {
                var line = new Line
                {
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 5,
                };
                _lines.Add(line);
                _wheelCanvas.Children.Add(line);

                var text = new TextBlock
                {
                    RenderTransform = new RotateTransform(),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20,
                };
                _textBlocks.Add(text);
                _wheelCanvas.Children.Add(text);
            }

#if DEBUG
            //_sbDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
#endif
        }

        public void Spin()
        {
            _sb.Begin();

            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1.5)};
            timer.Tick += (sender, o) =>
            {
                _textBlocks[0].Text = CorrectValue ?? "";
                RecalculateLayout();
            };
            timer.Start();
        }

        private void SetNumPossibilities(int num)
        {
            _numPossibilities = num;
            for (int i = _numPossibilities; i < 10; i++)
            {
                _lines[i].Visibility = Visibility.Collapsed;
                _textBlocks[i].Visibility = Visibility.Collapsed;
            }

            RecalculateLayout();
            _sbDoubleAnimation.To = 360*4 - (360.0/_numPossibilities)/2;
        }

        private void _canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecalculateLayout();
        }

        private void RecalculateLayout()
        {
            _wheelCanvas.Width = _mainCanvas.ActualWidth;
            _wheelCanvas.Height = _mainCanvas.ActualHeight;

            var elipseSize = Min(_wheelCanvas.Width, _wheelCanvas.Height) - 20;
            if (elipseSize <= 0) return;

            _ellipse.Width = elipseSize;
            _ellipse.Height = elipseSize;

            _canvasRotTrans.CenterX = elipseSize / 2 + 10;
            _canvasRotTrans.CenterY = elipseSize / 2 + 10;

            _winPointTransform.X = elipseSize / 2 - 10;
            _winPointTransform.Y = -20;

            _title.Width = _mainCanvas.ActualWidth;

            var angleAddPerLine = (360.0 / _numPossibilities) * PI / 180.0;
            var angle = -90.0*PI/180.0;
            var lineLength = elipseSize / 2.0;
            for (int i = 0; i < _numPossibilities; i++)
            {
                _lines[i].X1 = lineLength + 10;
                _lines[i].Y1 = lineLength + 10;

                _lines[i].X2 = lineLength + Cos(angle) * lineLength + 10;
                _lines[i].Y2 = lineLength + Sin(angle) * lineLength + 10;

                var x = lineLength + Cos(angle + angleAddPerLine / 2.0) * lineLength * 0.8 + 10 - _textBlocks[i].ActualWidth / 2;
                var y = lineLength + Sin(angle + angleAddPerLine / 2.0) * lineLength * 0.8 + 10 - _textBlocks[i].ActualHeight / 2;
                _textBlocks[i].SetValue(Canvas.LeftProperty, x);
                _textBlocks[i].SetValue(Canvas.TopProperty, y);

                ((RotateTransform)_textBlocks[i].RenderTransform).CenterX = _textBlocks[i].ActualWidth / 2;
                ((RotateTransform)_textBlocks[i].RenderTransform).CenterY = _textBlocks[i].ActualHeight / 2;
                ((RotateTransform)_textBlocks[i].RenderTransform).Angle = (angle + angleAddPerLine / 2) * 180 / PI + 90;

                angle += angleAddPerLine;
            }
        }


        private void _sb_OnCompleted(object sender, object e)
        {
            SpinComplete?.Invoke();
        }
    }
}
