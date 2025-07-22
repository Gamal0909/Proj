using UnityEngine;

namespace Series.Core
{
	[AddComponentMenu("Series/Platformer/Player/States/Walk Player State")]
	public class WalkPlayerState : PlayerState
	{
        PlayerEquipmentSystem swordSystem;
        protected override void OnEnter(Player player) 
		{
            swordSystem = player.GetComponent<PlayerEquipmentSystem>();
            swordSystem.SheathSword();
        }

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			player.Gravity();
			player.SnapToGround();
			player.Jump();
			player.Fall();
			player.Spin();
			player.PickAndThrow();
			player.Dash();
			player.RegularSlopeFactor();
			player.Attack();

			var inputDirection = player.inputs.GetMovementCameraDirection();

			// Get raw input direction first
			var rawInput = player.inputs.GetMovement();

			// Modify inputDirection to only use X axis
			inputDirection = new Vector3(rawInput.x, 0, 0);

			if (inputDirection.sqrMagnitude > 0)
			{
				var dot = Vector3.Dot(inputDirection, player.lateralVelocity);

				if (dot >= player.stats.current.brakeThreshold)
				{
					player.Accelerate(inputDirection);
					player.FaceDirectionSmooth(player.lateralVelocity);
				}
				else
				{
					player.states.Change<BrakePlayerState>();
				}
			}
			else
			{
				player.Friction();

				if (player.lateralVelocity.sqrMagnitude <= 0)
				{
					player.states.Change<IdlePlayerState>();
				}
			}

			if (player.inputs.GetCrouchAndCraw())
			{
				player.states.Change<CrouchPlayerState>();
			}
		}

		public override void OnContact(Player player, Collider other)
		{
			player.PushRigidbody(other);
		}
	}
}
