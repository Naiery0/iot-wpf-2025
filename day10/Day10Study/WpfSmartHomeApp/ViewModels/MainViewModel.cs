using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using MQTTnet;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfSmartHomeApp.Helpers;
using WpfSmartHomeApp.Models;

namespace WpfSmartHomeApp.ViewModels
{
    public partial class MainViewModel : ObservableObject, IDisposable
    {
        private double _homeTemp;
        private double _homeHumid;

        private bool _isDetectOn;
        private bool _isRainOn;
        private bool _isAirConOn;
        private bool _isLightOn;
        private string _detectResult;
        private string _rainResult;
        private string _airConResult;
        private string _lightResult;
        private string? _currDateTime;

        // readonly는 생성자에서만 값 할당 가능
        private readonly DispatcherTimer _timer;
        // MQTT용 변수
        private string TOPIC;
        private IMqttClient mqttClient;
        private string BROKERHOST;

        public MainViewModel()
        {
            CurrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, e) =>
            {
                CurrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            };
            _timer.Start();
        }
        public string? CurrDateTime
        {
            get => _currDateTime;
            set => SetProperty(ref _currDateTime, value);
        }

        // 온도 속성
        public double HomeTemp
        {
            get => _homeTemp;
            set => SetProperty(ref _homeTemp, value);
        }

        // 습도 속성
        public double HomeHumid
        {
            get => _homeHumid;
            set => SetProperty(ref _homeHumid, value);
        }

        // 사람인지
        public string DetectResult
        {
            get => _detectResult;
            set => SetProperty(ref _detectResult, value);
        }

        // 사람인지여부
        public bool IsDetectOn
        {
            get => _isDetectOn;
            set => SetProperty(ref _isDetectOn, value);
        }

        // IsAirConOn, IsLightOn
        public bool IsRainOn
        {
            get => _isRainOn;
            set => SetProperty(ref _isRainOn, value);
        }

        public bool IsAirConOn
        {
            get => _isAirConOn;
            set => SetProperty(ref _isAirConOn, value);
        }

        public bool IsLightOn
        {
            get => _isLightOn;
            set => SetProperty(ref _isLightOn, value);
        }

        // AirConResult, LightResult
        public string RainResult
        {
            get => _rainResult;
            set => SetProperty(ref _rainResult, value);
        }

        public string AirConResult
        {
            get => _airConResult;
            set => SetProperty(ref _airConResult, value);
        }

        public string LightResult
        {
            get => _lightResult;
            set => SetProperty(ref _lightResult, value);
        }

        // LoadedCommand 에서 앞에 On이 붙고 Command는 삭제
        [RelayCommand]
        public async Task OnLoaded()
        {
             //테스트로 집어넣은 가짜 데이터
            //HomeTemp = 30;
            //HomeHumid = 43.2;

            //DetectResult = "Detected Human!";
            //IsDetectOn = true;
            //RainResult = "Raining";
            //IsRainOn = true;
            //AirConResult = "Aircon On!";
            //IsAirConOn = true;
            //LightResult = "Light On~";
            //IsLightOn = true;

            // MQTT 접속부터 실행까지
            TOPIC = "pknu/sh01/data";
            BROKERHOST = "210.119.12.52";
            var mqttFactory = new MqttClientFactory();
            mqttClient = mqttFactory.CreateMqttClient();

            var mqttClientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(BROKERHOST)
                .WithCleanSession(true)
                .Build();
            mqttClient.ConnectedAsync += MqttClient_ConnectedAsync;
            mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;

            await mqttClient.ConnectAsync(mqttClientOptions);
        }

        private Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var topic = arg.ApplicationMessage.Topic;
            var payload = arg.ApplicationMessage.ConvertPayloadToString(); // byte -> utf-8

            var data = JsonConvert.DeserializeObject<SensingInfo>(payload);
            //Common.LOGGER.Info($@"Light: {data.L} / Rain: {data.R} / Temp: {data.T} / Humid: {data.H}
            //                      Fan: {data.F} / Vulernability: {data.V} / RealLight: {data.RL} / ChaimBell: {data.CB}");

            HomeHumid = data.H;
            HomeTemp = data.T;

            DetectResult = data.V == "ON" ? "Detection Human!" : "Normal State";
            IsDetectOn = data.V == "ON" ? true : false;

            LightResult = data.RL == "ON" ? "Light On~" : "Light Off";
            IsLightOn = data.RL == "ON" ? true : false;

            IsAirConOn = data.F == "ON" ? true : false;
            AirConResult = data.F == "ON" ? "Aircon On" : "Aircon Off";

            IsRainOn = data.R <= 350 ? true : false;
            RainResult = data.R <= 350 ? "Raining" : "No Rain"; 

            return Task.CompletedTask; // 구독 종료
        }

        private async Task MqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            Common.LOGGER.Info($"{arg}");
            Common.LOGGER.Info("MQTT Broker 접속 성공");

            await mqttClient.SubscribeAsync(TOPIC);
        }

        public void Dispose()
        {

        }
    }
}