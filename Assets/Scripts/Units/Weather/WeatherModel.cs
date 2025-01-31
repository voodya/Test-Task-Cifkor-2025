using UniRx;
using UnityEngine;

public class WeatherModel
{
    private IReactiveProperty<int> _temperature;
    private IReactiveProperty<Texture2D> _icon;
    private string _apiUrl;

    public IReactiveProperty<int> Temperature => _temperature;
    public IReactiveProperty<Texture2D> Icon => _icon;
    public string ApiUrl => _apiUrl;

    public WeatherModel(string api)
    {
        _temperature = new ReactiveProperty<int>(0);
        _icon = new ReactiveProperty<Texture2D>();
        _apiUrl = api;
    }

    public void SetTemperature(int value)
    {
        _temperature.Value = value;
    }

    public void SetTexture(Texture2D texture)
    {
        _icon.Value = texture;
    }
}


