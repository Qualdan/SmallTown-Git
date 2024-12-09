using UnityEngine;
using UnityEngine.SceneManagement;

namespace SmallTown {
public static class Bootstrapper {


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void Bootstrap() {
		#if UNITY_EDITOR
		// Debug.LogWarning("Saved scene");
		var currentlyLoadedEditorScene = SceneManager.GetActiveScene();
		#endif

		if(!SceneManager.GetSceneByName("Bootstrapper").isLoaded) {
			SceneManager.LoadScene("Bootstrapper");
		}

		#if UNITY_EDITOR
		if(currentlyLoadedEditorScene.IsValid()) {
			SceneManager.LoadSceneAsync(currentlyLoadedEditorScene.name, LoadSceneMode.Single);
		}
		#endif
	}
}
}