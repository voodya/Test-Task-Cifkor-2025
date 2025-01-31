using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using UniRx;
using UnityEngine;

public class WeatherPresenter : ABaseWebPanelPresenter<WeatherView>, IDisposable
{
    private WeatherModel _weatherModel;
    private WeatherView _weatherView;

    public WeatherPresenter(
        WeatherModel weatherModel,
        WeatherView weatherView,
        IWebLoadService webLoadService) : base(webLoadService)
    {
        SetView(weatherView);
        Hide();

        _weatherModel = weatherModel;
        _weatherView = weatherView;

        _weatherModel.Temperature.Subscribe(SetTemperatureView).AddTo(_weatherView);
        _weatherModel.Icon.Subscribe(SetWeatherTexture).AddTo(_weatherView);
    }


    #region View Update
    public void SetTemperatureView(int temperature)
    {
        _weatherView.TemperatureText.text = "Сегодня: " + temperature.ToString() + "F";
        _view.LoadIndicator.SetActive(false);
    }

    public void SetWeatherTexture(Texture2D texture)
    {
        _weatherView.TargetImage.texture = texture;
        _view.LoadIndicator.SetActive(false);
    }

    #endregion

    #region Override
    public override void Show()
    {
        base.Show();
        RequestIterator();
    }

    #endregion

    #region Web

    private void RequestIterator()
    {
        SendRequest(_weatherModel.ApiUrl, RequestCallback);
        Observable
            .Timer(TimeSpan.FromSeconds(5))
            .Repeat()
            .Subscribe(_ => SendRequest(_weatherModel.ApiUrl, RequestCallback))
            .AddTo(_compositeDisposable);
    }

    #endregion

    #region Model Update
    private void RequestCallback(ExecuteResult result)
    {
        WeatherData data = JsonConvert.DeserializeObject<WeatherData>(result.ResultData);
        _weatherModel.SetTemperature(data.Properties.Periods[0].Temperature);
        SendRequest(data.Properties.Periods[0].Icon, TextureLoadCallback);
    }

    private void TextureLoadCallback(ExecuteResult result)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(result.RawData);
        _weatherModel.SetTexture(texture);
    }

    #endregion
}
