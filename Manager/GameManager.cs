using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public FloorManager fm
    {
        get {
            return GameObject.FindObjectOfType<FloorManager>( ) as FloorManager;
        }
    }
    private BarrierManager barrierManager
    {
        get {
            return GameObject.FindObjectOfType<BarrierManager>( ) as BarrierManager;
        }
    }
    private CameraManager cameraManager
    {
        get {
            return GameObject.FindObjectOfType<CameraManager>( ) as CameraManager;
        }
    }
    public Hero hero;
    private bool isControlled = false;

    void Start( )
    {
        hero.CheckAndShowAlert(hero.data.currentFloor);
    }

    void Update( )
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit( );
    }

    public void DragDirection( int direction )
    {
        if(iTween.Count(hero.gameObject) == 0 && !isControlled) {
            switch(direction) {
                case 0:
                if(hero.data.point.x + 1 <= fm.floors.GetLength(0) - 1) {
                    Floor targetFloor = fm.floors[hero.data.point.x + 1, hero.data.point.y];
                    if(targetFloor != null) {
                        isControlled = true;
                        SwitchAction(90, targetFloor);
                    }
                }
                break;
                case 1:
                if(hero.data.point.x - 1 >= 0) {
                    Floor targetFloor = fm.floors[hero.data.point.x - 1, hero.data.point.y];
                    if(targetFloor != null) {
                        isControlled = true;
                        SwitchAction(-90, targetFloor);
                    }
 
                }
                break;
                case 2:
                if(hero.data.point.y + 1 <= fm.floors.GetLength(1) - 1) {
                    Floor targetFloor = fm.floors[hero.data.point.x, hero.data.point.y + 1];
                    if(targetFloor != null) {
                        isControlled = true;
                        SwitchAction(0, targetFloor);
                    }

                }
                break;
                case 3:
                if(hero.data.point.y - 1 >= 0) {
                    Floor targetFloor = fm.floors[hero.data.point.x, hero.data.point.y - 1];
                    if(targetFloor != null) {
                        isControlled = true;
                        SwitchAction(180, targetFloor);
                    }

                }
                break;
            }
        }
    }

    void SwitchAction( int rotation, Floor targetFloor )
    {
        if(targetFloor.obstacle != null) {
            if(targetFloor.enemy != null) {
                StartCoroutine(AttackObstacleWithEnemy(rotation, targetFloor));
            }
            else if(targetFloor.treasure != null) {
                StartCoroutine(AttackObstacleWithTreasure(rotation, targetFloor));
            }
            else if (targetFloor.equipment != null) {
                StartCoroutine(AttackObstacleWithEquipment(rotation, targetFloor));
            }
            else {
                StartCoroutine(AttackObstacleWithoutAnyThing(rotation, targetFloor));
                fm.RestrictiveCameraMove(rotation, targetFloor);
            }
        }
        else if(targetFloor.enemy != null) {
            StartCoroutine(AttackOpponent(rotation, targetFloor));
        }
        else {
            StartCoroutine(MoveAndDoSomeThing(rotation, targetFloor));
            fm.RestrictiveCameraMove(rotation, targetFloor);
        }
    }

    IEnumerator CheckConnectedEnemies( Floor targetFloor )
    {
        List<List<Enemy>> enemylistList = fm.FindConnectedAndSurroundedEnemies( );

        if(enemylistList.Count > 0) {
            foreach(List<Enemy> enemylist in enemylistList) {
                List<Point> tempPoints = GetPointsByEnemies(enemylist);
                barrierManager.ShowBarrierLine(tempPoints);
            }
            yield return new WaitForSeconds(1.0f);

            foreach(List<Enemy> enemylist in enemylistList) {

                List<Point> tempPoints = GetPointsByEnemies(enemylist);
                fm.DestoryObstaclesByPoints(tempPoints);
                fm.HurtEnemiesByPoints(tempPoints, hero, 10.0f);
                fm.DestoryUselessBarriers(targetFloor);
                barrierManager.DestoryBarrierLines( );
            }
            Debug.Log("1");
            isControlled = false;
        }
        else {
            Debug.Log("2");
            isControlled = false;
        }
        //fm.DestoryUselessBarriers(targetFloor);
    }

    IEnumerator MoveAndDoSomeThing( int rotation, Floor targetFloor )
    {
        hero.Move(rotation, targetFloor);
        yield return new WaitForSeconds(Data.moveSpeed);
        targetFloor.CheckAndGetTreasure(hero);
        targetFloor.CheckAndEquipment( );
        hero.CheckAndShowAlert(targetFloor);
        fm.CheckSurroundedEnemies( );
        StartCoroutine(CheckConnectedEnemies(targetFloor));
    }

    IEnumerator AttackAction( int rotation )
    {
        iTween.RotateTo(hero.gameObject, iTween.Hash("y", rotation, "time", Data.turnSpeed, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(Data.turnSpeed);
        iTween.RotateTo(hero.gameObject, iTween.Hash("x", -20, "time", Data.readyTime, "delay", Data.turnSpeed, "easetype", iTween.EaseType.linear));
        iTween.RotateTo(hero.gameObject, iTween.Hash("x", 40, "time", Data.atttTime, "delay", Data.readyTime + Data.turnSpeed, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
        iTween.RotateTo(hero.gameObject, iTween.Hash("x", 0, "time", Data.atttTime, "easetype", iTween.EaseType.linear));
        yield return new WaitForSeconds(Data.atttTime);
        Debug.Log("AttackAction");
    }

    IEnumerator AttackObstacleWithEnemy( int rotation, Floor targetFloor )
    {
        StartCoroutine(AttackAction(rotation));
        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
        targetFloor.DestoryObstacle( );
        targetFloor.ReadyToFightByEnemy(hero);
        fm.DestoryUselessBarriers(targetFloor);
        StartCoroutine(targetFloor.enemy.AttackOpponent(hero));
        Debug.Log("AttackObstacleWithEnemy");
        isControlled = false;
    }

    IEnumerator AttackObstacleWithTreasure( int rotation, Floor targetFloor )
    {
        StartCoroutine(AttackAction(rotation));
        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
        targetFloor.DestoryObstacle( );
        targetFloor.treasure.ComingOut( );
        Debug.Log("AttackObstacleWithTreasure");
        isControlled = false;
    }

    IEnumerator AttackObstacleWithEquipment( int rotation, Floor targetFloor )
    {
        StartCoroutine(AttackAction(rotation));
        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
        targetFloor.DestoryObstacle( );
        targetFloor.equipment.ComingOut( );
        isControlled = false;
    }

    IEnumerator AttackObstacleWithoutAnyThing( int rotation, Floor targetFloor )
    {
        hero.Move(rotation, targetFloor);
        StartCoroutine(AttackAction(rotation));
        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
        targetFloor.DestoryObstacle( );
        hero.CheckAndShowAlert(targetFloor);
        fm.CheckSurroundedEnemies( );
        StartCoroutine(CheckConnectedEnemies(targetFloor));
    }

    //IEnumerator AttackObstacleWithoutAnyThing( int rotation, Floor targetFloor )
    //{
    //    StartCoroutine(AttackAction(rotation));
    //    yield return new WaitForSeconds(Data.turnSpeed);
    //    targetFloor.DestoryObstacle( );
    //    StartCoroutine(MoveAndDoSomeThing(rotation, targetFloor));
    //}

    IEnumerator AttackOpponent( int rotation, Floor targetFloor )
    {
        StartCoroutine(AttackAction(rotation));
        targetFloor.enemy.FaceTo(hero.data.point);
        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
        targetFloor.enemy.BeAttacked(hero.data.atk);
        if(targetFloor.enemy != null) {
            StartCoroutine(targetFloor.enemy.AttackOpponent(hero));
        }
        fm.DestoryUselessBarriers(targetFloor);
        hero.CheckAndShowAlert(hero.data.currentFloor);
        isControlled = false;
    }

    public List<Floor> GetFloorsByEnemies( List<Enemy> enemies )
    {
        List<Floor> tempFloor = new List<Floor>( );
        foreach(Enemy enemy in enemies) {
            tempFloor.Add(enemy.data.currentFloor);
        }
        return tempFloor;
    }

    public List<Point> GetPointsByEnemies( List<Enemy> enemies )
    {
        List<Point> tempPoints = new List<Point>( );
        foreach(Enemy enemy in enemies) {
            tempPoints.Add(enemy.data.point);
        }
        return tempPoints;
    }
}
