using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour {

    public Equipment currentEquipment;
    public Image equipmentUI;

    public void SetUI(Equipment e )
    {
        this.currentEquipment = e;
        equipmentUI.sprite = currentEquipment.equipmentUI;
        equipmentUI.color = new Color(1, 1, 1, 1);
    }

    public void TakeOff( )
    {
        currentEquipment.Hide( );
        currentEquipment = null;
        equipmentUI.sprite = null;
        equipmentUI.color = new Color(0, 0, 0, 0);
    }
}
