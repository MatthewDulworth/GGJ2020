using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMachine
{
   // -------------------------------------------------
   // Variables
   // -------------------------------------------------
   public Attack CurrentAttack { get; private set; }
   public Attack LastAttack { get; private set; }
   public bool Attacking { get; private set; }
   private ControlScheme controlScheme;
   private Attack[] attacks;


   // -------------------------------------------------
   // Constructor
   // -------------------------------------------------
   public AttackMachine(ControlScheme cs, Attack[] attacks)
   {
      this.attacks = attacks;
      this.controlScheme = cs;

      this.CurrentAttack = null;
      this.LastAttack = null;
   }

   // -------------------------------------------------
   // Update
   // -------------------------------------------------
   public void Update(bool grounded)
   {
      this.Attacking = false;
      bool attackPressed = Input.GetAxis(controlScheme.AttackAxis) > 0;
      float vertical = Input.GetAxis(controlScheme.VerticalAxis);

      if (grounded && attackPressed)
      {
         HandleTilts(vertical);
         this.Attacking = true;
      }
      else if (!grounded && attackPressed)
      {
         HandleAerials(vertical);
         this.Attacking = true;
      }
   }

   // -------------------------------------------------
   // Handle Tilts
   // -------------------------------------------------
   private void HandleTilts(float vertical)
   {
      if (vertical > 0)
      {
         UpdateAttack("UpTilt");
      }
      else if (vertical < 0)
      {
         UpdateAttack("DownTilt");
      }
      else
      {
         UpdateAttack("Jab");
      }
   }

   // -------------------------------------------------
   // Handle Aerials
   // -------------------------------------------------
   private void HandleAerials(float vertical)
   {
      if (vertical > 0)
      {
         UpdateAttack("UpAir");
      }
      else if (vertical < 0)
      {
         UpdateAttack("DownAir");
      }
      else
      {
         UpdateAttack("Jab");
      }
   }

   // -------------------------------------------------
   // UpdateAttack
   // -------------------------------------------------
   private void UpdateAttack(string attack)
   {
      LastAttack = CurrentAttack;
      CurrentAttack = FindAttack(attack);
   }

   // -------------------------------------------------
   // Find Attacks
   // -------------------------------------------------
   private Attack FindAttack(string name)
   {
      foreach (Attack attack in attacks)
      {
         if (attack.name.Equals(name))
         {
            return attack;
         }
      }
      return null;
   }
}
