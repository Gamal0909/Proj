using UnityEngine;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/Player/States/Draw Sword Player State")]
    public class IdleSwordPlayerState : PlayerState
    {
        protected override void OnEnter(Player player)
        {
            
        }

        protected override void OnExit(Player player) { }

        protected override void OnStep(Player player)
        {
            player.Gravity();
            player.SnapToGround();
            player.FaceDirectionSmooth(player.lateralVelocity);
            player.AccelerateToInputDirection();
            player.PickAndThrow();
            player.AirDive();
            player.StompAttack();
            player.LedgeGrab();
            player.Dash();
            player.Glide();
            
            
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

