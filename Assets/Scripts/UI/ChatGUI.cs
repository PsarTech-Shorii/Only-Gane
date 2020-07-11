using Insight;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class ChatGUI : MonoBehaviour {
		private ChatClient _chatClient;
		
		[Header("Module")]
		[SerializeField] private SO_Behaviour chatClientRef;
		
		[Header("Interface")]
		[SerializeField] private TextMeshProUGUI chatLogText;
		[SerializeField] private TextMeshProUGUI chatText;

		private void Awake() {
			_chatClient = (ChatClient) chatClientRef.Data;
			Assert.IsNotNull(_chatClient);
		}

		private void Start() {
			RegisterHandlers();
		}

		private void OnDestroy() {
			UnregisterHandlers();
		}

		private void RegisterHandlers() {
			_chatClient.OnReceiveMessage += OnReceiveChat;
		}
		
		private void UnregisterHandlers() {
			_chatClient.OnReceiveMessage -= OnReceiveChat;
		}

		private void OnEnable() {
			chatLogText.text = "";
			chatText.text = "";
		}

		public void Chat() {
			_chatClient.Chat(chatText.text);
		}

		private void OnReceiveChat(InsightMessageBase messageBase) {
			if(!(messageBase is ChatMsg message)) return;
			chatLogText.text += $"{message.username} : {message.data}\n";
		}
	}
}