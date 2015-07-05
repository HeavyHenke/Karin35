using System;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace BandDraw
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        // private readonly BandIO _bandIO = new BandIO();

        private Action _nextTapAction;

        public MainPage()
        {
            InitializeComponent();
            //_bandIO.Connect();

            _nextTapAction = () =>
            {
                _typeOfPresentWheel.Spin();
                _nextTapAction = null;
            };

            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Title = "Grattis";
        }

        private void Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _nextTapAction?.Invoke();
        }

        private void TypeOfPresent_OnSpinComplete()
        {
            _nextTapAction = () =>
            {
                HideWheelAndPlaceText(_typeOfPresentWheel, 0);

                _nextTapAction = () =>
                {
                    ShowNextWheel(_destinationWheel, "Vart går resan?" , "Vallentuna, Täby C, Flen, Ukraina, Åland, Klockarboda, Lek&bus, Stockholm, Eksjö, Eslöv", "Paris");
                };
            };
        }

        private void DestinationWheel_OnSpinComplete()
        {
            _nextTapAction = () =>
            {
                HideWheelAndPlaceText(_destinationWheel, 1);

                _nextTapAction = () =>
                {
                    ShowNextWheel(_whenWheel, "När åker vì?", "Idag, nästa år, augusti, december, 2018", "Nu på torsdag");
                };
            };
        }

        private void WhenWheel_OnSpinComplete()
        {
            _nextTapAction = () =>
            {
                HideWheelAndPlaceText(_whenWheel, 1);

                _nextTapAction = () =>
                {
                    ShowNextWheel(_mormorWheel, "Present till mormor", "Skor, Tuggummi, Väska, Ost, Strumpa, Shopping, Utemöbler", "Passa alla barnen");
                };
            };
        }


        private void HideWheelAndPlaceText(FortuneWheel wheel, int textIdx)
        {
            wheel.Visibility = Visibility.Collapsed;

            var tb = new TextBlock
            {
                Text = wheel.CorrectValue,
                Height = 80,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            _leftStack.Children.Insert(textIdx, tb);
        }

        private void ShowNextWheel(FortuneWheel wheel, string title, string values, string correctValue)
        {
            _leftStack.Children.Remove(wheel);
            wheel.SetValue(Grid.RowProperty, 1);
            wheel.SetValue(Grid.ColumnProperty, 1);
            wheel.Width = double.NaN;
            wheel.Height = double.NaN;
            wheel.Values = values;
            wheel.Title = title;
            wheel.CorrectValue = correctValue;
            _grid.Children.Add(wheel);

            _nextTapAction = () =>
            {
                wheel.Spin();
                _nextTapAction = null;
            };
        }

    }

}
