using System;
using System.Collections.Generic;
using Insight;
using Mirror;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

namespace UI {
	public class MatchInterface : MonoBehaviour {
		private readonly List<GameObject> _gameJoiners = new List<GameObject>();

		private ClientGameManager _clientGameManager;
		private ClientMatchMaker _clientMatchMaker;

		[Header("Module")]
		[SerializeField] private SO_Behaviour clientGameManagerRef;
		[SerializeField] private SO_Behaviour clientMatchMakerRef;

		[Header("Prefabs")]
		[SerializeField] private GameObject gameJoinerPrefabs;
		[SerializeField] private Transform gameJoinerParent;
		
		[Header("Interface")]
		[SerializeField] private GameObject gameCreationPopup;
		[SerializeField] private Button gameCreationButton;
		[SerializeField] private Button matchGameButton;
		[SerializeField] private TextMeshProUGUI gameNameText;
		[SerializeField] private TextMeshProUGUI playerCountText;
		[SerializeField] private Slider playerCountSlider;

		private void Awake() {
			_clientGameManager = (ClientGameManager) clientGameManagerRef.Data;
			Assert.IsNotNull(_clientGameManager);

			_clientMatchMaker = (ClientMatchMaker) clientMatchMakerRef.Data;
			Assert.IsNotNull(_clientMatchMaker);
		}

		private void Start() {
			playerCountSlider.maxValue = NetworkManager.singleton.maxConnections;
			
			RegisterHandlers();

			SetGameList(_clientGameManager.gamesList);
		}

		private void OnDestroy() {
			UnregisterHandlers();
		}

		private void RegisterHandlers() {
			_clientGameManager.OnReceiveResponse += OnReceiveGameList;
			_clientGameManager.OnReceiveMessage += OnUpdateGameList;
			_clientGameManager.OnReceiveResponse += OnChangeServer;
			_clientGameManager.OnReceiveMessage += OnChangeServer;

			playerCountSlider.onValueChanged.AddListener(OnPlayerCountChanged);
		}
		
		private void UnregisterHandlers() {
			_clientGameManager.OnReceiveResponse -= OnReceiveGameList;
			_clientGameManager.OnReceiveMessage -= OnUpdateGameList;
			_clientGameManager.OnReceiveResponse -= OnChangeServer;
			_clientGameManager.OnReceiveMessage -= OnChangeServer;

			playerCountSlider.onValueChanged.RemoveListener(OnPlayerCountChanged);
		}

		private void OnReceiveGameList(InsightMessageBase messageBase, CallbackStatus status) {
			if(!(messageBase is GameListMsg message)) return;

			switch (status) {
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

		private void OnUpdateGameList(InsightMessageBase messageBase) {
			if(!(messageBase is GameListStatusMsg message)) return;
			switch (message.operation) {
				case GameListStatusMsg.Operation.Add: {
					var gameJoinerObject = Instantiate(gameJoinerPrefabs, gameJoinerParent);
					var gameJoiner = gameJoinerObject.GetComponent<GameJoinerInterface>();
					gameJoiner.Initialize(message.game);
					gameJoiner.joinEvent.AddListener(_clientGameManager.JoinGame);
					_gameJoiners.Add(gameJoinerObject);
					break;
				}
				case GameListStatusMsg.Operation.Remove: {
					var gameJoinerObject = _gameJoiners.Find(e =>
						e.GetComponent<GameJoinerInterface>().Is(message.game.uniqueId));
					Destroy(gameJoinerObject);
					_gameJoiners.Remove(gameJoinerObject);
					break;
				}
				case GameListStatusMsg.Operation.Update: {
					var gameJoinerObject = _gameJoiners.Find(e =>
						e.GetComponent<GameJoinerInterface>().Is(message.game.uniqueId));
					
					gameJoinerObject.GetComponent<GameJoinerInterface>().UpdatePlayerCount(new GameStatusMsg {
							currentPlayers = message.game.currentPlayers,
							hasStarted = message.game.hasStarted
						}
					);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void OnChangeServer(InsightMessageBase callbackMsg, CallbackStatus status) {
			OnChangeServer(callbackMsg);
		}
		
		private void OnChangeServer(InsightMessageBase messageBase) {
			if(!(messageBase is ChangeServerMsg)) return;

			gameCreationButton.interactable = true;
			matchGameButton.interactable = true;
		}

		private void SetGameList(IEnumerable<GameContainer> games) {
			foreach (var gameJoiner in _gameJoiners) Destroy(gameJoiner);
			_gameJoiners.Clear();

			foreach (var game in games) {
				var gameJoinerObject = Instantiate(gameJoinerPrefabs, gameJoinerParent);
				var gameJoiner = gameJoinerObject.GetComponent<GameJoinerInterface>();
				gameJoiner.Initialize(game);
				gameJoiner.joinEvent.AddListener(_clientGameManager.JoinGame);
				_gameJoiners.Add(gameJoinerObject);
			}
		}

		private void OnPlayerCountChanged(float playerCount) {
			playerCountText.text = $"Minimum player count : {playerCount}";
		}

		public void CreateGame() {
			gameCreationButton.interactable = false;
			_clientGameManager.CreateGame(new CreateGameMsg {
				gameName = gameNameText.text,
				minPlayers = (int) playerCountSlider.value
			});
			gameNameText.text = "";
		}

		public void MatchGame() {
			if (_gameJoiners.Exists(e => e.activeInHierarchy)) {
				matchGameButton.interactable = false;
				_clientMatchMaker.MatchGame();
			}
			else {
				gameCreationPopup.SetActive(true);
			}
		}
	}
}