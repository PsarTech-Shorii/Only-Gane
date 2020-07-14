using Insight;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class LoginGUI : MonoBehaviour {
		private ClientAuthentication clientAuthentication;
		
		[Header("Module")]
		[SerializeField] private SO_Object clientAuthenticationRef;

		[Header("Interface")]
		[SerializeField] private TextMeshProUGUI usernameText;
		[SerializeField] private TextMeshProUGUI statusText;

		private void Awake() {
			clientAuthentication = (ClientAuthentication) clientAuthenticationRef.Data;
			Assert.IsNotNull(clientAuthentication);
		}

		private void Start() {
			RegisterHandlers();
		}

		private void OnDestroy() {
			UnregisterHandlers();
		}

		private void RegisterHandlers() {
			clientAuthentication.OnReceiveResponse += OnLogin;
		}
		
		private void UnregisterHandlers() {
			clientAuthentication.OnReceiveResponse -= OnLogin;
		}

		public void SendLoginMsg() {
			clientAuthentication.SendLoginMsg(new LoginMsg{accountName = usernameText.text});
		}
		
		private void OnLogin(InsightMessageBase _message, CallbackStatus _status) {
			if(!(_message is LoginMsg)) return;
			
			if (_status == CallbackStatus.Success) {
				gameObject.SetActive(false);
			}
			else {
				statusText.text = $"Login : {_status}";
			}
		}
	}
}