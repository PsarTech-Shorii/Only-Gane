using Game;
using Mirror;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class StartGameGUI : MonoBehaviour {
		public void StartGame() {
			Assert.IsTrue(NetworkClient.active);
			NetworkClient.Send(new StartGameMsg());
		}
	}
}