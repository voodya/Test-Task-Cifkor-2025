using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "WebLoadServiceInstaller", menuName = "Game Installers/WebLoadServiceInstaller")]
public class WebLoadServiceInstaller : ScriptableInstaller
{
    public override void Install(DiContainer container)
    {
        container.Bind<IWebLoadService>().To<WebLoadService>().AsSingle();
        container.BindMemoryPool<WebCommand, WebCommand.Pool>().WithInitialSize(10);
    }

}
