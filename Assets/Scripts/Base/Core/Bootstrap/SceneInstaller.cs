using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class SceneInstaller : MonoInstaller
{
    [SerializeField] private CompositeScriptableInstaller _installer;

    public override void InstallBindings()
    {
        Install(Container);
    }

    public virtual void Install(DiContainer builder)
    {
        _installer?.Install(builder);
    }
}
