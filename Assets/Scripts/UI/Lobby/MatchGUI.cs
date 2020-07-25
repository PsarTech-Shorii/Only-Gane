using System;
using System.Collections.Generic;
using Insight;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

namespace UI {
	public class MatchGUI : MonoBehaviour {
		private readonly List<GameObject> gameJoiners = new List<GameObject>();

		private GameClientManager gameClientManager;
		private ClientMatchMaker clientMatchMaker;

		[Header("Module")]
		[SerializeField] private SO_Object clientGameManagerRef;
		[SerializeField] private SO_Object clientMatchMakerRef;
		[SerializeField] private SO_Integer maxPlayers;

		[Header("Prefabs")]
		[SerializeField] private GameObject gameJoinerPrefabs;
		[SerializeField] private Transform gameJoinerParent;
		
		[Header("Interface")]
		[SerializeField] private GameObject gameCreationPopup;
		[SerializeField] private Button gameCreationButton;
		
		[SerializeField] private Button matchGameButton;
		
		[SerializeField] private TextMeshProUGUI gameNameText;
		
		[SerializeField] private TextMeshProUGUI minPlayerCountText;
		[SerializeField] private Slider minPlayerCountSlider;
		
		[SerializeField] private TextMeshProUGUI maxPlayerCountText;
		[SerializeField] private Slider maxPlayerCountSlider;

		private void Awake() {
			gameClientManager = (GameClientManager) clientGameManagerRef.Data;
			Assert.IsNotNull(gameClientManager);

			clientMatchMaker = (ClientMatchMaker) clientMatchMakerRef.Data;
			Assert.IsNotNull(clientMatchMaker);
			
			Assert.AreNotEqual(0, maxPlayers.Data);
		}

		private void Start() {
			minPlayerCountSlider.maxValue = maxPlayers.Data;
			maxPlayerCountSlider.maxValue = maxPlayers.Data;
			maxPlayerCountSlider.value = maxPlayers.Data;
			
			RegisterHandlers();

			SetGameList(gameClientManager.gamesList);
		}

		private void OnDestroy() {
			UnregisterHandlers();
		}

		private void RegisterHandlers() {
			gameClientManager.OnReceiveResponse += OnReceiveGameList;
			gameClientManager.OnReceiveMessage += OnUpdateGameList;
			gameClientManager.OnReceiveResponse += OnChangeServer;
			gameClientManager.OnReceiveMessage += OnChangeServer;

			minPlayerCountSlider.onValueChanged.AddListener(OnMinPlayerCountChanged);
			maxPlayerCountSlider.onValueChanged.AddListener(OnMaxPlayerCountChanged);
		}
		
		private void UnregisterHandlers() {
			gameClientManager.OnReceiveResponse -= OnReceiveGameList;
			gameClientManager.OnReceiveMessage -= OnUpdateGameList;
			gameClientManager.OnReceiveResponse -= OnChangeServer;
			gameClientManager.OnReceiveMessage -= OnChangeServer;
		}

		private void OnReceiveGameList(InsightMessageBase _message, CallbackStatus _status) {
			if(!(_message is GameListMsg message)) return;

			switch (_status) {
				case CallbackStatus.Success: {
					SetGameList(message.gamesArray);
				}
					break;
				case CallbackStatus.Error:
				case CallbackStatus.Timeout:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void OnUpdateGameList(InsightMessageBase _message) {
			if(!(_message is GameListStatusMsg message)) return;
			switch (message.operation) {
				case GameListStatusMsg.Operation.Add: {
					var gameJoinerObject = Instantiate(gameJoinerPrefabs, gameJoinerParent);
					var gameJoiner = gameJoinerObject.GetComponent<GameJoinerGUI>();
					gameJoiner.Initialize(message.game);
					gameJoiner.joinEvent.AddListener(gameClientManager.JoinGame);
					gameJoiners.Add(gameJoinerObject);
					break;
				}
				case GameListStatusMsg.Operation.Remove: {
					var gameJoinerObject = gameJoiners.Find(_e =>
						_e.GetComponent<GameJoinerGUI>().Is(message.game.uniqueId));
					Destroy(gameJoinerObject);
					gameJoiners.Remove(gameJoinerObject);
					break;
				}
				case GameListStatusMsg.Operation.Update: {
					var gameJoinerObject = gameJoiners.Find(_e =>
						_e.GetComponent<GameJoinerGUI>().Is(message.game.uniqueId));
					
					gameJoinerObject.GetComponent<GameJoinerGUI>().UpdatePlayerCount(message.game);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void OnChangeServer(InsightMessageBase _callbackMsg, CallbackStatus _status) {
			OnChangeServer(_callbackMsg);
		}
		
		private void OnChangeServer(InsightMessageBase _message) {
			if(!(_message is ChangeServerMsg)) return;

			gameCreationButton.interactable = true;
			matchGameButton.interactable = true;
		}

		private void SetGameList(IEnumerable<GameContainer> _games) {
			foreach (var gameJoiner in gameJoiners) Destroy(gameJoiner);
			gameJoiners.Clear();

			foreach (var game in _games) {
				var gameJoinerObject = Instantiate(gameJoinerPrefabs, gameJoinerParent);
				var gameJoiner = gameJoinerObject.GetComponent<GameJoinerGUI>();
				gameJoiner.Initialize(game);
				gameJoiner.joinEvent.AddListener(gameClientManager.JoinGame);
				gameJoiners.Add(gameJoinerObject);
			}
		}

		private void OnMinPlayerCountChanged(float _playerCount) {
			minPlayerCountText.text = $"Minimum player count : {_playerCount}";
			maxPlayerCountSlider.minValue = _playerCount;
		}
		
		private void OnMaxPlayerCountChanged(float _playerCount) {
			maxPlayerCountText.text = $"Maximum player count : {_playerCount}";
			minPlayerCountSlider.maxValue = _playerCount;

			maxPlayers.Data = (int) _playerCount;
		}

		public void CreateGame() {
			gameCreationButton.interactable = false;
			gameClientManager.CreateGame(new CreateGameMsg {
				gameName = gameNameText.text,
				minPlayers = (int) minPlayerCountSlider.value,
				maxPlayers = (int) maxPlayerCountSlider.value
			});
			gameNameText.text = "";
		}

		public void MatchGame() {
			if (gameJoiners.Exists(_e => _e.activeInHierarchy)) {
				matchGameButton.interactable = false;
				clientMatchMaker.MatchGame();
			}
			else {
				gameCreationPopup.SetActive(true);
			}
		}
	}
}