using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlertManager : MonoBehaviour
{
    private EnemyManager enemyManager
    {
        get {
            return GameObject.FindObjectOfType<EnemyManager>( ) as EnemyManager;
        }
    }
    private BarrierManager barrierManager
    {
        get {
            return GameObject.FindObjectOfType<BarrierManager>( ) as BarrierManager;
        }
    }
    public GameObject[ ] alertType;

    void Update( )
    {
        FacePositive( );
    }

    void FacePositive( )
    {
        transform.forward = Vector3.forward;
    }

    int FindNeighborEnemyCount( Point point )
    {
        int tempInt = 0;
        foreach(Enemy enemy in enemyManager.enemies) {
            if(enemy.gameObject.activeSelf && !enemy.isComingout) {
                if(Mathf.Abs(point.x - enemy.data.point.x) == 1 && point.y == enemy.data.point.y) {
                    tempInt++;
                }
                if(Mathf.Abs(point.y - enemy.data.point.y) == 1 && point.x == enemy.data.point.x) {
                    tempInt++;
                }
            }
        }
        return tempInt;
    }

    public void ShowAlert( Point point )
    {
        switch(FindNeighborEnemyCount(point)) {
            case 0:
            alertType[0].SetActive(false);
            alertType[1].SetActive(false);
            alertType[2].SetActive(false);
            break;
            case 1:
            alertType[0].SetActive(true);
            alertType[1].SetActive(false);
            alertType[2].SetActive(false);
            barrierManager.SetBarrier(point);
            break;
            case 2:
            alertType[1].SetActive(true);
            alertType[0].SetActive(false);
            alertType[2].SetActive(false);
            barrierManager.SetBarrier(point);
            break;
            case 3:
            alertType[2].SetActive(true);
            alertType[0].SetActive(false);
            alertType[1].SetActive(false);
            barrierManager.SetBarrier(point);
            break;
        }
    }
}
