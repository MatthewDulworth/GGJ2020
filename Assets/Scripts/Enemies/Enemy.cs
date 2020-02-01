using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
   // -------------------------------------------------
   // Properties
   // -------------------------------------------------
   public float Health { get; private set; }

   // -------------------------------------------------
   // MonoBehaviour
   // -------------------------------------------------
   void Start() { }
   void Update() { }

   // -------------------------------------------------
   // public methods
   // -------------------------------------------------
   public abstract void handleMovement();
   public abstract void handleAttacks();
}
