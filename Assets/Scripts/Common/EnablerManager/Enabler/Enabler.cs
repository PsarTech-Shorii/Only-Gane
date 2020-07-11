using ScriptableObjects;
using UnityEngine;

namespace Common.EnablerManager {
	public abstract class Enabler : MonoBehaviour {
		[SerializeField] protected SO_Boolean enabler;
		[SerializeField] protected bool enable = true;

		private void Start() {
			enabler.AddListener(CheckEnable);
			CheckEnable(enabler.Data);
		}

		private void OnDestroy() {
			enabler.RemoveListener(CheckEnable);
		}

		protected abstract void CheckEnable(bool newEnable);
	}
}