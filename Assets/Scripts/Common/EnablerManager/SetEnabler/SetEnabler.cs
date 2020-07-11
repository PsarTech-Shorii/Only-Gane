using ScriptableObjects;
using UnityEngine;

namespace Common.EnablerManager {
	public abstract class SetEnabler : MonoBehaviour {
		[SerializeField] protected SO_Boolean enabler;
		
		protected virtual void Start() {
			RegisterHandlers();
		}

		protected virtual void OnDestroy() {
			UnregisterHandlers();
		}

		protected abstract void RegisterHandlers();
		protected abstract void UnregisterHandlers();
		
		protected void Enable() {
			enabler.Data = true;
		}
		protected void Disable() {
			enabler.Data = false;
		}
	}
}