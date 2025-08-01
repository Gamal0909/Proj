using UnityEngine;

namespace Series.Core
{
	[AddComponentMenu("Series/Platformer/Player/States/Idle Player State")]
	public class IdlePlayerState : PlayerState
	{
		protected override void OnEnter(Player player) { }

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			player.Gravity();
			player.SnapToGround();
			player.Jump();
			player.Fall();
			player.Spin();
			player.PickAndThrow();
			player.RegularSlopeFactor();
			player.Friction();
			player.Attack();

			var inputDirection = player.inputs.GetMovementDirection();

				
			if (inputDirection.sqrMagnitude > 0 || player.lateralVelocity.sqrMagnitude > 0)
			{
				player.states.Change<WalkPlayerState>();
			}
			else if (player.inputs.GetCrouchAndCraw())
			{
				player.states.Change<CrouchPlayerState>();
			}
		}

		public override void OnContact(Player player, Collider other) { }
	}
}
