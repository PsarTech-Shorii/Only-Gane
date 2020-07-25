using Insight;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI {
	public class LeaveGameGUI : MonoBehaviour {
		private GameClientManager gameClientManager;
		private Button button;
		
		[Header("Module")]
		[SerializeField] private SO_Object clientGameManagerRef;

		private void Awake() {
			gameClientManager = (GameClientManager) clientGameManagerRef.Data;
			Assert.IsNotNull(gameClientManager);

			button = GetComponent<Button>();
		}

		public void LeaveGame() {
			gameClientManager.LeaveGame();

			button.interactable = false;
		}
	}
}