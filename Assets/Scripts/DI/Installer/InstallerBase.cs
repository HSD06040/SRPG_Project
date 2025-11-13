using HSD.DI;
using UnityEngine;

public abstract class InstallerBase : MonoBehaviour, IInstaller
{
    DiContainer _container = new();

    protected DiContainer Container { get { return _container; } }

    public void Binding()
    {
        InstallBindings();
        RegisterBindings();
        Inject();
    }

    public abstract void InstallBindings();

    void RegisterBindings() => _container.RegisterBindings();

    void Inject() => _container.Binding();
}
