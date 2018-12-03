using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : BaseAttack {

    public Fireball()
    {
        attackName = "Fireball";
        attackDescription = "This is fireball magic attack";
        attackDamage = 25f;
        attackCost = 20f;
    }
}
