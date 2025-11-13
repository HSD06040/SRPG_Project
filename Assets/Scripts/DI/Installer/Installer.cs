using HSD.DI;

public class Installer : InstallerBase
{   
    public override void InstallBindings()
    {
        Container.Bind<Context>().AsSingle().WithArguments(10);
    }
}
