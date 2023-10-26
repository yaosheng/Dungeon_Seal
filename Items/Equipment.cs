using UnityEngine;
using System.Collections;

public enum EquipType
{
    Weapon,
    Armor,
}

public abstract class Equipment : MonoBehaviour {

    public GameManager gameManager
    {
        get {
            return GameObject.FindObjectOfType<GameManager>( ) as GameManager;
        }
    }
    public EquipSlotManager equipSlotManager
    {
        get {
            return GameObject.FindObjectOfType<EquipSlotManager>( ) as EquipSlotManager;
        }
    }
    public float value;
    public float durability;
    public EquipType equipType;
    public Point point;
    public Floor currentFloor;
    public Sprite equipmentUI;

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

    public void PutInTheEquipSlot( )
    {
        if(equipType == EquipType.Armor) {
            if(equipSlotManager.armorSlot.currentEquipment == null) {
                equipSlotManager.armorSlot.currentEquipment = this;
                gameManager.hero.armor = this;
                equipSlotManager.armorSlot.SetUI(this);
            }
            else {
                Equipment tempEquip = equipSlotManager.armorSlot.currentEquipment;
                tempEquip.currentFloor = this.currentFloor;
                currentFloor.equipment = tempEquip;
                tempEquip.transform.localPosition = tempEquip.currentFloor.Position( ) + Vector3.up;
                tempEquip.ComingOut( );

                equipSlotManager.armorSlot.currentEquipment = this;
                gameManager.hero.armor = this;
                equipSlotManager.armorSlot.SetUI(this);
            }
        }
        if(equipType == EquipType.Weapon) {
            if(equipSlotManager.weaponSlot.currentEquipment == null) {
                equipSlotManager.weaponSlot.currentEquipment = this;
                gameManager.hero.weapon = this;
                equipSlotManager.weaponSlot.SetUI(this);
            }
            else {
                Equipment tempEquip = equipSlotManager.weaponSlot.currentEquipment;
                tempEquip.currentFloor = this.currentFloor;
                currentFloor.equipment = tempEquip;
                tempEquip.transform.localPosition = tempEquip.currentFloor.Position( ) + Vector3.up;
                tempEquip.ComingOut( );

                equipSlotManager.weaponSlot.currentEquipment = this;
                gameManager.hero.weapon = this;
                equipSlotManager.weaponSlot.SetUI(this);
            }
        }

        gameManager.hero.SetEquipment( );
        Hide( );
    }
}
