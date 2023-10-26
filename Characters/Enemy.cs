using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    private UIManager uiManager
    {
        get {
            return GameObject.FindObjectOfType<UIManager>( ) as UIManager;
        }
    }
    private MeshRenderer meshRenderer
    {
        get {
            return GetComponentInChildren<MeshRenderer>( );
        }
    }
    private HealthLine healthLine;

    public CharacterData data;

    public bool isSurround = false;
    public bool isComingout = false;
    public ParticleSystem deadParticle;

    public Material originalMaterial;
    public Material beattackedMaterial;

    void Start( )
    {
        healthLine = uiManager.GetHelathLine( );
        healthLine.gameObject.SetActive(false);
        meshRenderer.material = originalMaterial;
    }

    void Update( )
    {
        healthLine.FollowTarget(transform.position);
    }

    public void FaceToAndComingOut( Point point , float comingoutTime )
    {
        FaceTo(point);
        ComingOut(comingoutTime);
    }

    public void Destory( )
    {
        data.currentFloor.enemy = null;
        healthLine.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void BeAttacked( float atk )
    {
        Debug.Log("this enemy attacked");
        if(gameObject.activeSelf) {
            data.health -= atk;
            StartCoroutine(BeAttackedEffect( ));
            healthLine.ShowHealthLine(data.health, data.maxHealth);
            if(data.health <= 0) {
                DeadEffect( );
                this.Destory( );
            }
        }
    }

    IEnumerator BeAttackedEffect( )
    {
        if(meshRenderer.sharedMaterial == originalMaterial) {
            meshRenderer.sharedMaterial = beattackedMaterial;
        }
        yield return new WaitForSeconds(0.2f);
        if(meshRenderer.sharedMaterial == beattackedMaterial) {
            meshRenderer.sharedMaterial = originalMaterial;
        }
    }

    void ComingOut(float comingoutTime)
    {
        isComingout = true;
        healthLine.gameObject.SetActive(true);
        iTween.MoveTo(this.gameObject, iTween.Hash("y", 0.5f, "islocal", true, "time", comingoutTime, "easetype", iTween.EaseType.linear));

    }

    public void FaceTo(Point p)
    {
        if(data.point.x + 1 == p.x) {
            Debug.Log("right");
            transform.forward = Vector3.right;
        }
        if(data.point.x - 1 == p.x) {
            Debug.Log("left");
            transform.forward = Vector3.left;
        }
        if(data.point.y + 1 == p.y) {
            Debug.Log("forward");
            transform.forward = Vector3.forward;
        }
        if(data.point.y - 1 == p.y) {
            Debug.Log("back");
            transform.forward = Vector3.back;
        }
    }

    void DeadEffect( )
    {
        if(gameObject.activeSelf) {
            ParticleSystem rb = Instantiate(deadParticle) as ParticleSystem;
            rb.transform.position = transform.position;
        }
    }

    IEnumerator AttackAction( Point targetPoint )
    {
        //iTween.RotateTo(this.gameObject, iTween.Hash("y", rotation, "time", Data.turnSpeed, "easetype", iTween.EaseType.linear));
        //yield return new WaitForSeconds(Data.turnSpeed);
        iTween.RotateTo(this.gameObject, iTween.Hash("x", -20, "time", Data.readyTime, "easetype", iTween.EaseType.linear));
        iTween.RotateTo(this.gameObject, iTween.Hash("x", 40, "time", Data.atttTime, "delay", Data.readyTime, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(Data.readyTime + Data.atttTime);
        iTween.RotateTo(this.gameObject, iTween.Hash("x", 0, "time", Data.atttTime, "easetype", iTween.EaseType.linear));
    }

    public IEnumerator AttackOpponent( Hero hero)
    {
        StartCoroutine(AttackAction(hero.data.point));
        yield return new WaitForSeconds(Data.readyTime + Data.atttTime);
        if(gameObject.activeSelf){
            hero.BeAttacked(data.atk);
        }
        //enemyManager.AttackTheEnemy(targetPoint, atkNumber);
    }
}
