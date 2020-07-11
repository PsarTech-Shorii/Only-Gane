namespace Common.Enabler {
	public class GameObjectEnabler : EnablerBase {
		protected override void CheckEnable(bool newEnable) {
			gameObject.SetActive(enable ? newEnable : !newEnable);
		}
	}
}