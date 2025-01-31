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