using UnityEngine;
using System.Collections;

public class Spell : Treasure
{
    public override void BeEated( Hero hero)
    {
        PutInTheInventory( );
    }

    public override void PutInTheInventory( )
    {
        foreach(Transform t in slotManager.transform) {
            Slot slot = t.GetComponent<Slot>( );
            if(slot.currentTreasrue == null) {
                slot.SetUI(this);
                currentFloor.treasure = null;
                GetComponentInChildren<MeshRenderer>( ).enabled = false;
                break;
            }
        }
    }

    public override void UseAbility( Hero hero , Floor floor)
    {
        ParticleSystem ps = Instantiate(attackParticle) as ParticleSystem;
        ps.transform.position = floor.Position( ) + Vector3.up * 15;
        if(floor.obstacle != null) {
            floor.DestoryObstacle( );
        }
        if(floor.enemy != null) {
            floor.ReadyToFightByEnemy(hero);
            floor.enemy.BeAttacked(10.0f);
            gameManager.fm.DestoryUselessBarriers(floor);
        }
    }
}
