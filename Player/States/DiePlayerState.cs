using UnityEngine;

namespace Series.Core
{
	[AddComponentMenu("Series/Platformer/Player/States/Die Player State")]
	public class DiePlayerState : PlayerState
	{
		protected override void OnEnter(Player player) { }

		protected override void OnExit(Player player) { }

		protected override void OnStep(Player player)
		{
			player.Gravity();
			player.Friction();
			player.SnapToGround();
		}

		public override void OnContact(Player player, Collider other) { }
	}
}
