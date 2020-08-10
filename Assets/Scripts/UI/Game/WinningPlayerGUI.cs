using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI {
	public class WinningPlayerGUI : MonoBehaviour {
		private TextMeshProUGUI text;
		
		[SerializeField] private SO_Boolean isMatchWinner;

		private void Awake() {
			text = GetComponent<TextMeshProUGUI>();
		}

		private void Start() {
			isMatchWinner.AddListener(OnFinishGame);
		}

		private void OnDestroy() {
			isMatchWinner.RemoveListener(OnFinishGame);
		}

		private void OnFinishGame(bool _winValue) {
			text.text = _winValue ? "You won !" : "You lost...";
		}
	}
}