using UnityEngine;
using System.Collections.Generic;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/Player/States/Attack Player State")]
    public class AttackOnePlayerState : PlayerState
    {
        

        
        protected override void OnEnter(Player player)
        {
            
        }

        protected override void OnExit(Player player)
        {
            
        }

        protected override void OnStep(Player player)
        {
            //if(player.inputs.GetAttack() == false)
            //{
            //    player.states.Change<IdlePlayerState>();    
            //}


            player.Gravity();
            player.SnapToGround();
            player.AirDive();
            player.StompAttack();
            player.AccelerateToInputDirection();
            player.Attack();

        }


        public override void OnContact(Player player, Collider other)
        {
        }
    }
}