using Insight;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class LeaveGameInterface : MonoBehaviour {
		private ClientGameManager _clientGameManager;
		
		[Header("Module")]
		[SerializeField] private SO_Behaviour clientGameManagerRef;

		private void Awake() {
			_clientGameManager = (ClientGameManager) clientGameManagerRef.Data;
			Assert.IsNotNull(_clientGameManager);
		}

		public void LeaveGame() {
			_clientGameManager.LeaveGame();
		}
	}
}