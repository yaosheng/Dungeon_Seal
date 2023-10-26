using UnityEngine;
using System.Collections;

public class Potion : Treasure
{
    public override void BeEated( Hero hero)
    {
        PutInTheInventory( );
    }

    public override void PutInTheInventory( )
    {
        potionManager.potionNumber++;
        potionManager.ShowPotionNumber( );
        Hide( );
    }

    public override void UseAbility( Hero hero, Floor floor)
    {
        if(hero.data.maxHealth - hero.data.health <= 10) {
            hero.data.health = hero.data.maxHealth;
        }
        else {
            hero.data.health += 10;
        }
        hero.healthLine.ShowHealthLine(hero.data.health, hero.data.maxHealth);
    }
}
