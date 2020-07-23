using Insight;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class LeaveGameGUI : MonoBehaviour {
		private GameClientManager gameClientManager;
		
		[Header("Module")]
		[SerializeField] private SO_Object clientGameManagerRef;

		private void Awake() {
			gameClientManager = (GameClientManager) clientGameManagerRef.Data;
			Assert.IsNotNull(gameClientManager);
		}

		public void LeaveGame() {
			gameClientManager.LeaveGame();
		}
	}
}