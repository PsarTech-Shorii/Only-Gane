using Insight;
using UnityEngine;

namespace Common.TrackingLinker.Boolean {
	public class LoginLinker : BooleanLinkerBase {
		[Header("Module")]
		[SerializeField] private ClientAuthentication clientAuthentication;

		protected override void Start() {
			base.Start();
			OnValueChange(clientAuthentication.IsLogin);
		}

		protected override void RegisterHandlers() {
			clientAuthentication.OnLogin += OnValueChange;
		}

		protected override void UnregisterHandlers() {
			clientAuthentication.OnLogin -= OnValueChange;
		}
	}
}