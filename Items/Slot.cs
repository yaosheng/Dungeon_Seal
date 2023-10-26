using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

    public Treasure currentTreasrue;
    public Image treasureUI;

    public void SetUI(Treasure ts)
    {
        this.currentTreasrue = ts;
        treasureUI.sprite = currentTreasrue.treasureUI;
        treasureUI.color = new Color(1, 1, 1, 1);
    }

    public void UseTreasureAbility(Hero hero, Collider collider)
    {
        Floor fr;
        if(collider.tag == "Enemy") {
            fr = collider.GetComponentInParent<Enemy>( ).data.currentFloor;
            currentTreasrue.UseAbility(hero, fr);
        }
        else if(collider.tag == "Obstacle") {
            fr = collider.GetComponentInParent<Obstacle>( ).currentFloor;
            currentTreasrue.UseAbility(hero, fr);
        }
        //RunOut( );
    }

    public void UseTreasureAbility( Hero hero)
    {
        currentTreasrue.UseAbility(hero, null);
        //RunOut( );
    }

    public void RunOut()
    {
        currentTreasrue.Hide( );
        currentTreasrue = null;
        treasureUI.sprite = null;
        treasureUI.color = new Color(0, 0, 0, 0);
    }

    public void AssignSetting(Transform parent, int slotNumber, float color )
    {
        transform.SetParent(parent);
        transform.SetSiblingIndex(slotNumber);
        GetComponent<Image>( ).color = new Color(255, 255, 255, color);
    }

    public void Setting( Transform parent , float color )
    {
        transform.SetParent(parent);
        GetComponent<Image>( ).color = new Color(0, 0, 0, color);
    }


}
