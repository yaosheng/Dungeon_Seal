using UnityEngine;
using System.Collections;

public enum Type
{
    DirectToEat,
    ClickToUse,
    DragToObstacle,
    DragToEnemy,
}

public abstract class Treasure : MonoBehaviour
{
    public GameManager gameManager
    {
        get {
            return GameObject.FindObjectOfType<GameManager>( ) as GameManager;
        }
    }
    public SlotManager slotManager
    {
        get {
            return GameObject.FindObjectOfType<SlotManager>( ) as SlotManager;
        }
    }
    public PotionManager potionManager
    {
        get {
            return GameObject.FindObjectOfType<PotionManager>( ) as PotionManager;
        }
    }
    public Point point;
    public Floor currentFloor;
    public Sprite treasureUI;
    public Type usedType;
    public ParticleSystem attackParticle;

    void Start( )
    {
        gameObject.SetActive(false);
    }

    public void ComingOut( )
    {
        gameObject.SetActive(true);
    }

    public void Hide( )
    {
        gameObject.SetActive(false);
    }

    public abstract void PutInTheInventory( );
    public abstract void BeEated( Hero hero );
    public abstract void UseAbility( Hero hero, Floor floor );
}

//foreach(Slot slot in slotManager.slots) {
//    if(slot.currentTreasrue == null) {
//        slot.SetTreasureUI(this);
//        currentFloor.treasure = null;
//        GetComponentInChildren<MeshRenderer>( ).enabled = false;
//        break;
//    }
//}