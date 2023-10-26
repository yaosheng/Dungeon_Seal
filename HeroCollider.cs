using UnityEngine;
using System.Collections;

public class HeroCollider : MonoBehaviour {

    void OnTriggerEnter( Collider other )
    {
        Debug.Log("hit");
        if(other.tag == "Obstacles") {
            other.gameObject.SetActive(false);
            other.transform.parent.gameObject.SetActive(false);
        }
    }
}
