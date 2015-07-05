﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BandDraw
{
    public sealed partial class FortuneWheel : UserControl
    {
        private readonly List<Line> _lines = new List<Line>();
        private readonly List<TextBlock> _textBlocks = new List<TextBlock>();

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
            }
        }

        public string FoolValue
        {
            get { return _textBlocks[3].Text; }
            set { _textBlocks[3].Text = value; }
        }

        public string CorrectValue { get; set; }

        public FortuneWheel()
        {
            this.InitializeComponent();

            //var presents = new[]{"Skor", "Tuggummi", "Väska", "Resa", "Ost", "Trosor", "Strumpa", "Shopping", "Middag", "Utemöbler"};

            for (int i = 0; i < 10; i++)
            {
                var line = new Line();
                line.Stroke = new SolidColorBrush(Colors.Black);
                _lines.Add(line);
                _wheelCanvas.Children.Add(line);

                var text = new TextBlock();
                //text.Text = presents[i];
                text.RenderTransform = new RotateTransform();
                _textBlocks.Add(text);
                _wheelCanvas.Children.Add(text);
            }
        }

        public void Spin()
        {
            _sb.Begin();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.5);
            timer.Tick += (sender, o) => _textBlocks[3].Text = CorrectValue;
            timer.Start();
        }

        private void _canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _wheelCanvas.Width = _mainCanvas.ActualWidth;
            _wheelCanvas.Height = _mainCanvas.ActualHeight;

            var elipseSize = Math.Min(_wheelCanvas.Width, _wheelCanvas.Height) - 20;
            if (elipseSize <= 0) return;

            _ellipse.Width = elipseSize;
            _ellipse.Height = elipseSize;

            _canvasRotTrans.CenterX = elipseSize / 2 + 10;
            _canvasRotTrans.CenterY = elipseSize / 2 + 10;

            _winPointTransform.X = elipseSize / 2 - 10;
            _winPointTransform.Y = -20;

            var angleAddPerLine = (360.0 / (_lines.Count)) * Math.PI / 180.0;
            var angle = 0.0;
            var lineLength = elipseSize / 2.0;
            for (int i = 0; i < _lines.Count; i++)
            {
                _lines[i].X1 = lineLength + 10;
                _lines[i].Y1 = lineLength + 10;

                _lines[i].X2 = lineLength + Math.Sin(angle) * lineLength + 10;
                _lines[i].Y2 = lineLength + Math.Cos(angle) * lineLength + 10;

                var x = lineLength + Math.Sin(angle + angleAddPerLine / 2.0) * lineLength * 0.8 + 10 - _textBlocks[i].ActualWidth / 2;
                var y = lineLength + Math.Cos(angle + angleAddPerLine / 2.0) * lineLength * 0.8 + 10 - _textBlocks[i].ActualHeight / 2;
                _textBlocks[i].SetValue(Canvas.LeftProperty, x);
                _textBlocks[i].SetValue(Canvas.TopProperty, y);

                ((RotateTransform)_textBlocks[i].RenderTransform).CenterX = _textBlocks[i].ActualWidth / 2;
                ((RotateTransform)_textBlocks[i].RenderTransform).CenterY = _textBlocks[i].ActualHeight / 2;
                ((RotateTransform)_textBlocks[i].RenderTransform).Angle = -(angle + angleAddPerLine / 2) * 180 / Math.PI + 180;

                angle += angleAddPerLine;
            }
        }

        private void _canvasAnim_OnCompleted(object sender, object e)
        {
        }

    }
}
