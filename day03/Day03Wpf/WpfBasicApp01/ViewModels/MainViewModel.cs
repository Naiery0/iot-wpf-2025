using CommunityToolkit.Mvvm.ComponentModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfBasicApp01.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // NLog 객체 생성
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region 속성영역
        private string _greeting;
        public string Greeting
        {
            get => _greeting;
            set => SetProperty(ref _greeting, value);
        }

        private string _currentTime;
        public string CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }

        private readonly DispatcherTimer _timer; 

        #endregion

        public MainViewModel()
        {
            _logger.Info("뷰모델 시작");
            Greeting = "Hello WPF MVVM!";

            CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // 초반렉

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, e) =>
            {
                CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Debug.WriteLine($"[DEBUG] {CurrentTime}");
                _logger.Info($"[DEBUG] {CurrentTime}");
            }; // 람다식
            _timer.Start();

        }

        //private void _timer_Tick(object? sender, EventArgs e)
        //{
        //    CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    Debug.WriteLine($"[DEBUG] {CurrentTime}");
        //}
    }
}
