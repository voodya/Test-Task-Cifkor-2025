using UnityEngine;
using Zenject;

public class BootstrapInstaller : SceneInstaller
{
    public override void Install(DiContainer builder)
    {
        base.Install(builder);
        builder.Bind<BootstrapEntryPoint>().AsSingle().NonLazy();
    }
}

public class BootstrapEntryPoint
{
    ISceneManager _sceneManager;

    public BootstrapEntryPoint(ISceneManager sceneManager)
    {
        Debug.LogError("Application is started");
        _sceneManager = sceneManager;
        LoadStartScene();
    }

    private async void LoadStartScene()
    {
        await _sceneManager.GetScene<LoadingView>();
    }

}


