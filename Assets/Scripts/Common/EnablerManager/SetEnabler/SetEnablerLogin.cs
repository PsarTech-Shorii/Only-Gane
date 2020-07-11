using Insight;
using UnityEngine;

namespace Common.EnablerManager {
	public class SetEnablerLogin : SetEnabler {
		[Header("Module")]
		[SerializeField] private ClientAuthentication clientAuthentication;

		protected override void Start() {
			base.Start();
			
			if(clientAuthentication.IsLogin) Enable();
			else Disable();
		}

		protected override void RegisterHandlers() {
			clientAuthentication.OnLogin += OnLogin;
		}

		protected override void UnregisterHandlers() {
			clientAuthentication.OnLogin -= OnLogin;
		}

		private void OnLogin(bool enable) {
			if(enable) Enable();
			else Disable();
		}
	}
}