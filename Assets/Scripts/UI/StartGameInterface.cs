using ScriptableObjects;
using UnityEngine;

namespace UI {
	public class StartGameInterface : MonoBehaviour {
		[SerializeField] private SO_Boolean hasStartedGame;

		public void StartGame() {
			hasStartedGame.Data = true;
		}
	}
}