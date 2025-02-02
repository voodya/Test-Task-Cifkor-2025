using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

[CreateAssetMenu(fileName = "SceneManagerInstaller", menuName = "Bootstrap Installers/SceneManagerInstaller")]
public class SceneManagerInstaller : ScriptableInstaller
{
    [SerializeField] private AssetReference _loadingScene;
    [SerializeField] private AssetReference _runtimeScene;
    [SerializeField] private AssetReference _popupScene;


    public override void Install(DiContainer container)
    {
        ScenesManagerService scenesManagerService = new ScenesManagerService();

        //here register all scenes assetReferences
        scenesManagerService.RegisterScene<LoadingView>(_loadingScene);
        scenesManagerService.RegisterScene<RuntimeConnectionView>(_runtimeScene);
        scenesManagerService.RegisterScene<PopupView>(_popupScene);

        container.Bind<ISceneManager>().To<ScenesManagerService>().FromInstance(scenesManagerService);
    }
}
