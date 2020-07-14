using Insight;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class ChatGUI : MonoBehaviour {
		private ChatClient chatClient;
		
		[Header("Module")]
		[SerializeField] private SO_Object chatClientRef;
		
		[Header("Interface")]
		[SerializeField] private TextMeshProUGUI chatLogText;
		[SerializeField] private TextMeshProUGUI chatText;

		private void Awake() {
			chatClient = (ChatClient) chatClientRef.Data;
			Assert.IsNotNull(chatClient);
		}

		private void Start() {
			RegisterHandlers();
		}

		private void OnDestroy() {
			UnregisterHandlers();
		}

		private void RegisterHandlers() {
			chatClient.OnReceiveMessage += OnReceiveChat;
		}
		
		private void UnregisterHandlers() {
			chatClient.OnReceiveMessage -= OnReceiveChat;
		}

		private void OnEnable() {
			chatLogText.text = "";
			chatText.text = "";
		}

		public void Chat() {
			chatClient.Chat(chatText.text);
		}

		private void OnReceiveChat(InsightMessageBase _message) {
			if(!(_message is ChatMsg message)) return;
			chatLogText.text += $"{message.username} : {message.data}\n";
		}
	}
}