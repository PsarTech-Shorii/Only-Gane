using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common {
	public class ClientSceneManager : NetworkBehaviour {
		[SerializeField] [Scene] private List<string> clientScenes;

		public override void OnStartClient() {
			base.OnStartClient();

			foreach (var clientScene in clientScenes) {
				SceneManager.LoadSceneAsync(clientScene, LoadSceneMode.Additive);
			}
		}
	}
}