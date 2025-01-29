using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CompositeScriptableInstaller", menuName = "Bootstrap Installers/CompositeScriptableInstaller")]
public class CompositeScriptableInstaller : ScriptableInstaller
{
    [SerializeField] private List<ScriptableInstaller> _installers;
    public override void Install(DiContainer container)
    {
        foreach (var installer in _installers)
        {
            installer.Install(container);
        }
    }
}
