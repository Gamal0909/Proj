using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Series.Core
{
    public class AttackTwoPlayerState : PlayerState
    {
       
        public override void OnContact(Player player, Collider other)
        {
            
        }

        protected override void OnEnter(Player player)
        {
            
        }

        protected override void OnExit(Player player)
        {
            
        }

        protected override void OnStep(Player player)
        {
            player.Gravity();
            player.SnapToGround();
            player.AirDive();
            player.StompAttack();
            player.AccelerateToInputDirection();
            player.Attack();
             
        }
        
    }
}

