using System.Collections.Generic;
using UnityEngine;

namespace Series.Core
{
	[RequireComponent(typeof(Player))]
	[AddComponentMenu("Series/Platformer/Player/Player State Manager")]
	public class PlayerStateManager : EntityStateManager<Player>
	{
		[ClassTypeName(typeof(PlayerState))]
		public string[] states;

		protected override List<EntityState<Player>> GetStateList()
		{
			return PlayerState.CreateListFromStringArray(states);
		}
	}
}
