namespace Common.Enabler {
	public class GameObjectEnabler : EnablerBase {
		protected override void CheckEnable(bool _newEnable) {
			gameObject.SetActive(enable ? _newEnable : !_newEnable);
		}
	}
}