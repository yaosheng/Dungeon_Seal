using UnityEngine;
using System.Collections;
using System;

public class Coin : Treasure
{
    public override void BeEated( Hero hero)
    {
        UseAbility(hero, null);
        Hide( );
    }

    public override void PutInTheInventory( )
    {

    }

    public override void UseAbility( Hero hero, Floor floor )
    {
        UIManager uimanager = GameObject.FindObjectOfType<UIManager>( );
        uimanager.coinNumber++;
        uimanager.ShowCoinUI( );
    }
}
