using UnityEngine;
using Zenject;

public class BootstrapInstaller : SceneInstaller
{
    public override void Install(DiContainer builder)
    {
        base.Install(builder);
        builder.Bind<RuntimeEntryPoint>().AsSingle();
        builder.Bind<BootstrapEntryPoint>().AsSingle().NonLazy();
    }
}

public class BootstrapEntryPoint
{
    ISceneManager _sceneManager;
    IWebLoadService _webLoadService;
    RuntimeEntryPoint _runtimeEntryPoint;

    public BootstrapEntryPoint(ISceneManager sceneManager,
        IWebLoadService webLoadService,
        RuntimeEntryPoint runtimeEntryPoint)
    {
        Debug.Log("Application is started");
        _sceneManager = sceneManager;
        _webLoadService = webLoadService;
        _runtimeEntryPoint = runtimeEntryPoint;
        LoadStartScene();
    }

    private async void LoadStartScene()
    {
        var scene = await _sceneManager.GetScene<LoadingView>();

        var result = await _webLoadService.CheckConnection();
        if(result)
        {
            _webLoadService.Initialize();
            _runtimeEntryPoint.PrepareScene();
        }
        else
            Debug.LogError("No connection");
    }

}


