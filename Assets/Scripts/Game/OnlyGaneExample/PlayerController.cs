using System.Collections;
using Mirror;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.OnlyGaneExample {
	public class PlayerController : NetworkBehaviour {
		private Rigidbody2D playerRb;
		
		[Header("Input")]
		[SerializeField] private SO_Boolean isInMatch;

		private void Awake() {
			playerRb = GetComponent<Rigidbody2D>();
		}

		#region Server

		private const float JumpForce = 30f;
		private const float JumpCooldown = 1f;

		private Vector2 jumpDirection;
		private bool canJump = true;

		private void FixedUpdate() {
			if(!isServer) return;
			
			JumpPhysic();
		}

		[Server] public override void OnStartServer() {
			base.OnStartServer();
			
			Assert.IsTrue(isInMatch.Data);
			
			isInMatch.AddListener(FinishGame);
		}

		[Server] public override void OnStopServer() {
			base.OnStopServer();
			
			Assert.IsFalse(isInMatch.Data);
			
			isInMatch.RemoveListener(FinishGame);
		}

		[Server] private void FinishGame(bool _isIngame) {
			if(!_isIngame) NetworkServer.Destroy(gameObject);
		}

		[Server] private void JumpPhysic() {
			if(jumpDirection == Vector2.zero) return;
			
			playerRb.AddForce(jumpDirection * JumpForce, ForceMode2D.Impulse);
			jumpDirection = Vector2.zero;
		}

		[Server] private IEnumerator WaitJumpCooldownCor() {
			yield return new WaitForSeconds(JumpCooldown);
			canJump = true;
		}

		[Command] private void CmdJump(Vector2 _jumpDirection) {
			if(!canJump) return;

			jumpDirection = _jumpDirection;
			
			canJump = false;
			StartCoroutine(WaitJumpCooldownCor());
		}

		#endregion

		#region Client

		private Camera mainCamera;

		private void Update() {
			if(!hasAuthority) return;
			
			JumpController();
			MoveBellController();
		}
		
		[Client] public override void OnStartClient() {
			base.OnStartClient();
			playerRb.isKinematic = true;
			enabled = false;
			
			mainCamera = Camera.main;
			Assert.IsNotNull(mainCamera);
		}

		[Client] public override void OnStartAuthority() {
			base.OnStartAuthority();
			enabled = true;
		}

		[Client] private void JumpController() {
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				var direction = GetBalancedMousePosition() - GetBalancedSelfPosition();
				CmdJump(direction.normalized);
			}
		}

		[Client] private void MoveBellController() {
			if (Input.GetKeyDown(KeyCode.Mouse1)) {
				NetworkClient.Send(new MoveBellMsg());
			}
		}

		[Client] private static Vector2 GetBalancedMousePosition() {
			var mousePosition = Input.mousePosition;
			var viewportMousePosition = new Vector2 {
				x = mousePosition.x / Screen.width,
				y = mousePosition.y / Screen.height
			};

			return viewportMousePosition - new Vector2(0.5f, 0.5f);
		}

		[Client] private Vector2 GetBalancedSelfPosition() {
			var viewportPosition = (Vector2) mainCamera.WorldToViewportPoint(transform.position);
			
			return viewportPosition - new Vector2(0.5f, 0.5f);
		}

		#endregion
	}
}