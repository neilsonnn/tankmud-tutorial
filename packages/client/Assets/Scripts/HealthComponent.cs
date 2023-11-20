using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class HealthComponent : MUDComponent
{

    public int health;

    protected override void UpdateComponent(MUDTable table, UpdateInfo updateInfo) {

        HealthTable update = table as HealthTable;
        health = (int)update.Value;

        if(Loaded && health <= 0) {
            //do some death animation

        }
    }
}
