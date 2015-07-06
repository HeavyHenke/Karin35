using System;
using System.Diagnostics;
using Microsoft.Band;
using Microsoft.Band.Sensors;
using static System.Math;

namespace BandDraw
{
    public class BandIO
    {
        private IBandClient _bandClient;

        public event Action Pulled;

        public async void Connect()
        {
            if (_bandClient != null)
                return;

            IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (pairedBands.Length == 0)
                return;

            _bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);

            _bandClient.SensorManager.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            await _bandClient.SensorManager.Accelerometer.StartReadingsAsync();
        }

        private void Accelerometer_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> e)
        {
            var s = e.SensorReading;
            if (s.AccelerationX == 0 && s.AccelerationY == 0 && s.AccelerationZ == 0)
                return;

            var totAcc = Sqrt(s.AccelerationX*s.AccelerationX + s.AccelerationY*s.AccelerationY + s.AccelerationZ*s.AccelerationZ);

            if(totAcc > 4.0)
                Pulled?.Invoke();

            Debug.WriteLine(totAcc);
        }
    }
}
