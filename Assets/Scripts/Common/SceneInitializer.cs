using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common {
	public class SceneInitializer : MonoBehaviour {
		[SerializeField] [Scene] private string scene;

		private void Start() {
			SceneManager.LoadSceneAsync(scene);
		}
	}
}