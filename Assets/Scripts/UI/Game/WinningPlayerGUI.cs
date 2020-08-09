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
			isMatchWinner.AddListener(_newValue => {
				text.text = _newValue ? "You won !" : "You lost...";
			});
		}
	}
}