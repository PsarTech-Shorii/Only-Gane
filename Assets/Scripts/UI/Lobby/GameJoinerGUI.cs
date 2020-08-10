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

		public void UpdatePlayerCount(GameContainer _game) {
			game.Update(_game);
			UpdateGUI();
		}

		public bool Is(string _uniqueId) {
			return game.uniqueId == _uniqueId;
		}

		public void JoinGame() {
			joinEvent?.Invoke(game.uniqueId);
		}

		private void UpdateGUI() {
			countText.text = $"{game.currentPlayers} / {game.maxPlayers}";
			gameObject.SetActive(!game.isInMatch && game.currentPlayers < game.maxPlayers);
		}
	}
}