using UnityEngine;

public class Game : MonoBehaviour
{
	private const string SCENE_ROOT_PREFAB_PATH = "SceneRoot";

	public static ISystemManager Systems { get; private set; }

	private void Awake()
	{
		// Note: We could define other implementation for create subset of games mode for testing purposes
		Systems = new GameSystemManager();

		// Instantiate the scene root prefab which contains gameplay controllers.
		// Note: We still have the flexibility to load different scene roots based on game mode or testing needs.
		GameObject sceneRootPrefab = Systems.Get<IAssetLoadSystem>().LoadAsset<GameObject>(SCENE_ROOT_PREFAB_PATH);
		Instantiate(sceneRootPrefab);

		// Note: Instead of manually instantiating the scene root prefab here, we could also use a "Bootstrapper" pattern where we have a dedicated scene to warm up the game systems and load the main scene asynchronously.
	}

	private void OnDestroy()
	{
		Systems.Dispose();
	}
}
