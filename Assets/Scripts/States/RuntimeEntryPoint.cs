using System;
using UniRx;

public class RuntimeEntryPoint
{
    ISceneManager _sceneManager;
    IWebLoadService _webLoadService;

    WeatherPresenter _weatherPresenter;
    FactsPresenter _factsPresenter;

    

    public RuntimeEntryPoint(ISceneManager sceneManager, IWebLoadService webLoadService)
    {
        _sceneManager = sceneManager;
        _webLoadService = webLoadService;
    }

    public async void PrepareScene()
    {
        var loadScene = await _sceneManager.GetScene<LoadingView>();
        var runtimeScene = await _sceneManager.GetScene<RuntimeConnectionView>();
        CreatePresenters(runtimeScene);
        runtimeScene.OnWeather.Subscribe(_ => 
        {
            _weatherPresenter.Show();
            _factsPresenter.Hide();
            runtimeScene.WeatherInteractable = false;
            runtimeScene.FactsInteractable = true;
        });
        runtimeScene.OnFacts.Subscribe(_ =>
        {
            _weatherPresenter.Hide();
            _factsPresenter.Show();
            runtimeScene.WeatherInteractable = true;
            runtimeScene.FactsInteractable = false;
        });
        await loadScene.Hide();

    }

    private void CreatePresenters(RuntimeConnectionView view)
    {
        _weatherPresenter = new WeatherPresenter(new WeatherModel("https://api.weather.gov/gridpoints/TOP/32,81/forecast"), view.WeatherView, _webLoadService);
        _factsPresenter = new FactsPresenter(new FactsModel("https://dogapi.dog/api/v2/breeds", 10), view.FactsView, _webLoadService);
    }
}
