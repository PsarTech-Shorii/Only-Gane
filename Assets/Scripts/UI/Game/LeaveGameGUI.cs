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
		[SerializeField] private SO_Object gameClientManagerRef;

		private void Awake() {
			gameClientManager = (GameClientManager) gameClientManagerRef.Data;
			Assert.IsNotNull(gameClientManager);

			button = GetComponent<Button>();
		}

		public void LeaveGame() {
			gameClientManager.LeaveGame();

			button.interactable = false;
		}
	}
}