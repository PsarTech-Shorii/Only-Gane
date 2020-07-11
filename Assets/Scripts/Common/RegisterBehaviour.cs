using ScriptableObjects;
using UnityEngine;

namespace Common {
	public class RegisterBehaviour : MonoBehaviour {
		[SerializeField] private SO_Behaviour behaviourRef;
		[SerializeField] private Behaviour behaviour;

		private void Awake() {
			behaviourRef.Data = behaviour;
		}

		private void OnDestroy() {
			behaviourRef.Data = null;
		}
	}
}