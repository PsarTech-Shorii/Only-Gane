using Insight;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI {
	public class JoinEvent : UnityEvent<string> {}
	
	public class GameJoinerGUI : MonoBehaviour {
		[SerializeField] private TextMeshProUGUI gameNameText;
		[SerializeField] private TextMeshProUGUI countText;
		
		private GameContainer game;

		public JoinEvent joinEvent = new JoinEvent();

		public void Initialize(GameContainer _game) {
			game = _game;
			gameNameText.text = game.gameName;
			UpdateGUI();
		}

		public void UpdatePlayerCount(GameStatusMsg _gameStatus) {
			game.currentPlayers = _gameStatus.currentPlayers;
			game.hasStarted = _gameStatus.hasStarted;
			UpdateGUI();
		}

		public bool Is(string _uniqueId) {
			return game.uniqueId == _uniqueId;
		}

		public void JoinGame() {
			joinEvent?.Invoke(game.uniqueId);
		}

		private void UpdateGUI() {
			countText.text = $"{game.currentPlayers} / {game.minPlayers}";
			gameObject.SetActive(!game.hasStarted && game.currentPlayers < game.maxPlayers);
		}
	}
}