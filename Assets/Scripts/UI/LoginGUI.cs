using Insight;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI {
	public class LoginGUI : MonoBehaviour {
		private ClientAuthentication _clientAuthentication;
		
		[Header("Module")]
		[SerializeField] private SO_Behaviour clientAuthenticationRef;

		[Header("Interface")]
		[SerializeField] private TextMeshProUGUI usernameText;
		[SerializeField] private TextMeshProUGUI statusText;

		private void Awake() {
			_clientAuthentication = (ClientAuthentication) clientAuthenticationRef.Data;
			Assert.IsNotNull(_clientAuthentication);
		}

		private void Start() {
			RegisterHandlers();
		}

		private void OnDestroy() {
			UnregisterHandlers();
		}

		private void RegisterHandlers() {
			_clientAuthentication.OnReceiveResponse += OnLogin;
		}
		
		private void UnregisterHandlers() {
			_clientAuthentication.OnReceiveResponse -= OnLogin;
		}

		public void SendLoginMsg() {
			_clientAuthentication.SendLoginMsg(new LoginMsg{accountName = usernameText.text});
		}
		
		private void OnLogin(InsightMessageBase messageBase, CallbackStatus status) {
			if(!(messageBase is LoginMsg)) return;
			
			if (status == CallbackStatus.Success) {
				gameObject.SetActive(false);
			}
			else {
				statusText.text = $"Login : {status}";
			}
		}
	}
}