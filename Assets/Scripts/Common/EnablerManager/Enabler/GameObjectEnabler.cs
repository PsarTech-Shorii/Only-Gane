namespace Common.EnablerManager {
	public class GameObjectEnabler : Enabler {
		protected override void CheckEnable(bool newEnable) {
			gameObject.SetActive(enable ? newEnable : !newEnable);
		}
	}
}