using Insight;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class LeaveGameGUI : MonoBehaviour {
		private GameClientManager _gameClientManager;
		
		[Header("Module")]
		[SerializeField] private SO_Object clientGameManagerRef;

		private void Awake() {
			_gameClientManager = (GameClientManager) clientGameManagerRef.Data;
			Assert.IsNotNull(_gameClientManager);
		}

		public void LeaveGame() {
			_gameClientManager.LeaveGame();
		}
	}
}