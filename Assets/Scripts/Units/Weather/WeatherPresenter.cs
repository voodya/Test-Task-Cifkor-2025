using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WeatherPresenter : ABasePanelPresenter<WeatherView>, IDisposable
{
    private WeatherModel _weatherModel;
    private WeatherView _weatherView;
    private IWebLoadService _webLoadService;
    private List<WebCommand> _currentCommands = new();


    public WeatherPresenter(WeatherModel weatherModel, WeatherView weatherView, IWebLoadService webLoadService)
    {
        SetView(weatherView);
        Hide();

        _weatherModel = weatherModel;
        _weatherView = weatherView;

        _weatherModel.Temperature.Subscribe(SetTemperatureView);

        _webLoadService = webLoadService;
    }

    public void SetTemperatureView(int temperature)
    {
        _weatherView.TemperatureText.text = "Temperature: " + temperature.ToString() + "F";
    }

    public override void Show()
    {
        base.Show();
        StartSendRequests();
    }

    private void StartSendRequests()
    {
        SendRequest();
        Observable
            .Timer(TimeSpan.FromSeconds(5)).Repeat().Subscribe(_ => SendRequest())
            .AddTo(_compositeDisposable);
    }

    public override void Hide()
    {
        for (int i = 0; i < _currentCommands.Count; i++)
        {
            _currentCommands[i]?.Cancell();
        }
        base.Hide();
    }

    private void SendRequest()
    {
        var result = _webLoadService.SendCommand(_weatherModel.ApiUrl, out WebCommand command);
        if (result.SendSuccess)
        {
            _currentCommands.Add(command);
            command.OnRelease
                .Subscribe(com => _currentCommands.Remove(com))
                .AddTo(command.DisposeToken);
            command.ExecuteResult
                .Subscribe(ExecuteResult => RequestCallback(ExecuteResult))
                .AddTo(command.DisposeToken);
        }
        else
            Debug.LogError(result.Message);
    }

    private void RequestCallback(ExecuteResult result)
    {
        if (!result.Success)
        {
            Debug.LogError(result.ResultDescription);
            result.Command.Cancell();
            return;
        }
        WeatherData data = JsonConvert.DeserializeObject<WeatherData>(result.ResultData);
        _weatherModel.SetTemperature(data.Properties.Periods[0].Temperature);
  
        result.Command.Cancell();
    }
}
