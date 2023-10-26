using UnityEngine;
using System.Collections;

[System.Serializable]
public class Floor : MonoBehaviour
{
    public Point point;
    public Enemy enemy;
    public Hero hero;
    public Equipment equipment;
    public Obstacle obstacle;
    public Treasure treasure;
    public Barrier barrier;
    public bool isCameraMove;
    public Floor[ ] associatedFloors;
    public Floor[ ] neighborFloors;

    public Vector3 Position( )
    {
        return new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    public void CheckAndGetTreasure( Hero hero )
    {
        if(treasure != null && treasure.gameObject.activeSelf) {
            treasure.BeEated(hero);
        }
    }

    public void CheckAndEquipment( )
    {
        if(equipment != null && equipment.gameObject.activeSelf) {
            equipment.PutInTheEquipSlot( );
        }
    }

    public void DestoryObstacle( )
    {
        if(obstacle != null && obstacle.gameObject.activeSelf) {
            obstacle.Destory( );
            obstacle = null;
        }
    }

    public void ReadyToFightByEnemy( Hero hero )
    {
        if(!enemy.isComingout) {
            enemy.FaceToAndComingOut(hero.data.point, 0.25f);
        }
    }

    //public void DestoryUselessBarrier( )
    //{
    //    bool tempBool = false;
    //    for(int i = 0; i < associatedFloors.Length; i++) {
    //        if(i == 1 || i == 3 || i == 4 || i == 6) {
    //            if(associatedFloors[i].barrier != null) {
    //            }
    //        }
    //    }
    //    foreach(Point point in points) {
    //        foreach(Barrier ba in barriers) {
    //            bool tempBool = false;
    //            if(point.IsNeighbor(ba.point)) {
    //                tempBool = true;
    //            }
    //            foreach(Enemy enemy in enemyManager.enemies) {
    //                if(enemy.gameObject.activeSelf && enemy.point.IsNeighbor(ba.point) && !point.IsIdentical(enemy.point)) {
    //                    tempBool = false;
    //                }
    //            }
    //            if(tempBool) {
    //                ba.Destory( );
    //            }
    //        }
    //    }
    //}
}
