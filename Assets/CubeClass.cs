using UnityEngine;
using Zenject;

public class CubeClass : MonoBehaviour {

	private Bar _bar;
	private Foo _foo;
	private PathFindingService _pathFindingService;

	[Inject]
	public void Inject(Foo foo, Bar bar, PathFindingService pathFindingService, List<IUnit>)
	{
		_pathFindingService = pathFindingService;
		_foo = foo;
		_bar = bar;
	}

	public void Update()
	{
		_pathFindingService.FindPath();
		_foo.FooLog();
		_bar.BarLog();
	}
}
