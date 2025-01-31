using UnityEngine;

public class BootstrapEntryPoint
{
    ISceneManager _sceneManager;
    IWebLoadService _webLoadService;
    RuntimeEntryPoint _runtimeEntryPoint;

    public BootstrapEntryPoint(ISceneManager sceneManager,
        IWebLoadService webLoadService,
        RuntimeEntryPoint runtimeEntryPoint)
    {
        _sceneManager = sceneManager;
        _webLoadService = webLoadService;
        _runtimeEntryPoint = runtimeEntryPoint;
        LoadStartScene();
    }

    private async void LoadStartScene()
    {
        Application.targetFrameRate = 120;
        var scene = await _sceneManager.GetScene<LoadingView>();

        var result = await _webLoadService.CheckConnection();

        if (result)
        {
            _webLoadService.Initialize();
            _runtimeEntryPoint.PrepareScene();
        }
        else
            Debug.LogError("No connection");
    }

}
