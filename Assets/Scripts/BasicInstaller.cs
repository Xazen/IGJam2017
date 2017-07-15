using Zenject;

public class BasicInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
//        Container.Bind<string>().FromInstance("Hello World!");
//        Container.Bind<Greeter>().AsSingle().NonLazy();
          Container.Bind<UnitSpawnController>().AsSingle();
          Container.Bind<GridController>().AsSingle().NonLazy();
          Container.Bind<IInitializable>().To<GridController>().AsSingle().NonLazy();
    }
}

//public class Greeter
//{
//    public Greeter(string message)
//    {
//        Debug.Log(message);
//    }
//}