using UnityEngine;
using Zenject;

public abstract class ScriptableInstaller : ScriptableObject
{
    public abstract void Install(DiContainer container);
}
