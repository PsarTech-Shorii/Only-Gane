using Insight;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class LeaveGameGUI : MonoBehaviour {
		private ClientGameManager _clientGameManager;
		
		[Header("Module")]
		[SerializeField] private SO_Object clientGameManagerRef;

		private void Awake() {
			_clientGameManager = (ClientGameManager) clientGameManagerRef.Data;
			Assert.IsNotNull(_clientGameManager);
		}

		public void LeaveGame() {
			_clientGameManager.LeaveGame();
		}
	}
}