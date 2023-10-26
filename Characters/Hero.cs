using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    private GameObject heroNoumenon
    {
        get {
            return transform.GetChild(0).gameObject;
        }
    }
    private UIManager uiManager
    {
        get {
            return GameObject.FindObjectOfType<UIManager>( ) as UIManager;
        }
    }
    private MeshRenderer meshRenderer {
        get {
            return GetComponentInChildren<MeshRenderer>( );
        }
    }
    private AlertManager alertManager
    {
        get {
            return GetComponentInChildren<AlertManager>( ) as AlertManager;
            //return GameObject.FindObjectOfType<AlertManager>( ) as AlertManager;
        }
    }

    public CharacterData data;
    public Equipment weapon;
    public Equipment armor;
    public HealthLine healthLine;
    public Material originalMaterial;
    public Material beattackedMaterial;
    public Animator animator;

    void Start( )
    {
        healthLine = uiManager.GetHelathLine( );
        healthLine.gameObject.SetActive(true);
        meshRenderer.material = originalMaterial;
        SetEquipment( );
    }

    void Update( )
    {
        healthLine.FollowTarget(transform.position);
        uiManager.bloodText.text = data.health.ToString( );
    }

    public void SetEquipment( )
    {
        if(weapon != null) {
            data.atk = data.originalATK + weapon.value;
        }
        else {
            data.atk = data.originalATK;
        }

        if(armor != null) {
            data.defense = armor.value;
        }
        else {
            data.defense = 0;
        }
        uiManager.ShowEquipUI(this);
    }

    public void Move( int rotation, Floor targetFloor )
    {
        if(Mathf.Abs(rotation) == 90) {
            iTween.MoveTo(this.gameObject, iTween.Hash("x", targetFloor.Position().x, "time", Data.moveSpeed, "easetype", iTween.EaseType.linear));
        }
        else {
            iTween.MoveTo(this.gameObject, iTween.Hash("z", targetFloor.Position( ).z, "time", Data.moveSpeed, "easetype", iTween.EaseType.linear));
        }
        iTween.RotateTo(this.gameObject, iTween.Hash("y", rotation, "time", Data.turnSpeed, "easetype", iTween.EaseType.linear));
        Jump( );
        data.point = targetFloor.point;
        data.currentFloor = targetFloor;
    }

    public void Jump( )
    {
        iTween.MoveTo(heroNoumenon, iTween.Hash("y", heroNoumenon.transform.position.y + 0.5f, "time", Data.moveSpeed / 2, "islocal", true, "easetype", iTween.EaseType.linear));
        iTween.MoveTo(heroNoumenon, iTween.Hash("y", 0, "time", Data.moveSpeed / 2, "delay", Data.moveSpeed / 2, "islocal", true, "easetype", iTween.EaseType.linear));
    }

    public void BeAttacked( float atk )
    {
        float temp = atk;
        if(armor != null) {
            temp = atk - armor.value;
        }
        if(temp >= 0) {
            data.health -= temp;
            StartCoroutine(BeAttackedEffect( ));
            healthLine.ShowHealthLine(data.health, data.maxHealth);
            if(data.health <= 0) {
                DeadEffect( );
                BeWipeOut( );
            }
        }
        else {
            return;
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

    void DeadEffect( )
    {

    }

    void BeWipeOut( )
    {
        uiManager.ShowGameOverUI( );
        gameObject.SetActive(false);
        healthLine.gameObject.SetActive(false);
    }

    public void CheckAndShowAlert(Floor floor)
    {
        alertManager.ShowAlert(floor.point);
    }
}
//IEnumerator SwitchAction( int rotation, Point targetPoint )
//{
//    float timeTotal = moveSpeed + 0.05f;

//    if(obstacleManager.IsThereObstacle(targetPoint)) {
//        Attack(rotation, targetPoint);
//        //MoveReturn(rotation, targetPoint);
//        yield return new WaitForSeconds(0.35f);
//        if(enemyManager.IsThereEnemy(targetPoint)) {
//            yield return new WaitForSeconds(0.75f);
//            MoveReturn(rotation, targetPoint);
//            enemyManager.WipeOutEnemy(targetPoint);
//            Blood -= 8;
//            //enemyManager.enemyCount--;
//            yield return new WaitForSeconds(0.75f);
//            Move(rotation, targetPoint);
//        }
//        else {
//            Move(rotation, targetPoint);
//        }
//    }
//    else {
//        Move(rotation, targetPoint);
//    }
//    yield return new WaitForSeconds(timeTotal);

//    pointString = this.point.ToString( );
//    alertManager.ShowAlert(this.point);
//    //enemyManager.CheckSurroundedByBarrierAndEnemy( );
//    enemyManager.CheckConnectedEnemies( );
//    //isControl = false;
//}

//void MoveReturn( int rotation, Point targetPoint )
//{
//    float average = 0;
//    if(Mathf.Abs(rotation) == 90) {
//        average = (floorManager.floors[targetPoint.x, targetPoint.y].transform.position.x + floorManager.floors[point.x, point.y].transform.position.x) / 2;
//        iTween.MoveTo(this.gameObject, iTween.Hash("x", average, "time", moveSpeed / 2, "easetype", iTween.EaseType.linear));
//        iTween.MoveTo(this.gameObject, iTween.Hash("x", floorManager.floors[point.x, point.y].transform.position.x, "time", moveSpeed / 2, "delay", moveSpeed / 2, "easetype", iTween.EaseType.linear));
//    }
//    else {
//        average = (floorManager.floors[targetPoint.x, targetPoint.y].transform.position.z + floorManager.floors[point.x, point.y].transform.position.z) / 2;
//        iTween.MoveTo(this.gameObject, iTween.Hash("z", average, "time", moveSpeed / 2, "easetype", iTween.EaseType.linear));
//        iTween.MoveTo(this.gameObject, iTween.Hash("z", floorManager.floors[point.x, point.y].transform.position.z, "time", moveSpeed / 2, "delay", moveSpeed / 2, "easetype", iTween.EaseType.linear));
//    }
//    iTween.RotateTo(this.gameObject, iTween.Hash("y", rotation, "time", turnSpeed, "easetype", iTween.EaseType.linear));
//    Jump( );
//}

//bool IsThereObstacle( Point point )
//{
//    bool tempBool = false;
//    foreach(Obstacle ob in obstacleManager.obstacles) {
//        if(ob.point.IsIdentical(point) && ob.gameObject.activeSelf) {
//            tempBool = true;
//            ob.Bebroken( );
//        }
//    }
//    if(tempBool) {
//        List<Obstacle> obs = new List<Obstacle>( );
//        foreach(Obstacle ob in obstacleManager.obstacles) {
//            if(ob.gameObject.activeSelf && ob.point.IsNeighbor(point)) {
//                obs.Add(ob);
//            }
//        }
//        int temp = 0;
//        foreach(Obstacle ob in obs) {
//            foreach(Enemy enemy in enemyManager.enemyGroup) {
//                if(ob.point.IsIdentical(enemy.point)) {
//                    temp++;
//                }
//            }
//        }
//        if(temp == 0) {
//            foreach(Obstacle ob in obs) {
//                ob.Bebroken( );
//            }
//        }
//    }
//    return tempBool;
//}

//void WipeOutEnemy( Point p )
//{
//    foreach(Enemy enemy in enemyManager.enemyGroup) {
//        if(enemy.point.x == p.x && enemy.point.y == p.y) {
//            enemy.BeWipeOut( );
//        }
//    }
//}

//bool IsThereEnemy( Point point )
//{
//    bool tempBool = false;
//    foreach(Enemy enemy in enemyManager.enemyGroup) {
//        if(enemy.point.IsIdentical(point)) {
//            tempBool = true;
//            enemy.FaceTo(this.point);
//            enemy.ComingOut( );
//        }
//        //if(enemy.point.x == point.x && enemy.point.y == point.y) {
//        //    tempBool = true;
//        //    enemy.FaceTo(this.point);
//        //    enemy.ComingOut( );
//        //}
//    }
//    return tempBool;
//}

//if(ground.obstacles[point.x + 1, point.y] != null && ground.obstacles[point.x + 1, point.y].gameObject.activeSelf == true) {
//    MoveReturn(90, new Point(point.x + 1, point.y));
//    IsFindEnemy(new Point(point.x + 1, point.y));
//}
//else {
//    Move(90, new Point(point.x + 1, point.y));
//}

//if(ground.obstacles[point.x - 1, point.y] != null && ground.obstacles[point.x - 1, point.y].gameObject.activeSelf == true) {
//    MoveReturn(-90, new Point(point.x - 1, point.y));
//    IsFindEnemy(new Point(point.x - 1, point.y));
//}
//else {
//    Move(-90, new Point(point.x - 1, point.y));
//}

//if(ground.obstacles[point.x, point.y + 1] != null && ground.obstacles[point.x, point.y + 1].gameObject.activeSelf == true) {
//    MoveReturn(0, new Point(point.x, point.y + 1));
//    IsFindEnemy(new Point(point.x, point.y + 1));
//}
//else {
//    Move(0, new Point(point.x, point.y + 1));
//}

//if(ground.obstacles[point.x, point.y - 1] != null && ground.obstacles[point.x, point.y - 1].gameObject.activeSelf == true) {
//    MoveReturn(180, new Point(point.x, point.y - 1));
//    IsFindEnemy(new Point(point.x, point.y - 1));
//}
//else {
//    Move(180, new Point(point.x, point.y - 1));
//}

//if(ob.point.x == enemy.point.x && ob.point.y == enemy.point.y) {
//    Debug.Log("don't clear" + ob.point.ToString( ));
//}
//else {
//    Debug.Log("clear" + ob.point.ToString( ));
//    ob.Bebroken( );
//}


//foreach(Obstacle ob in ground.obstacles) {
//    foreach(Enemy enemy in enemyManager.enemyGroup) {
//        if(Mathf.Abs(p.x - ob.point.x) <= 1 && Mathf.Abs(p.y - ob.point.y) <= 1) {
//            if(ob.point.x != enemy.point.x || ob.point.y != enemy.point.y) {
//                Debug.Log(ob.point.ToString( ));
//                ob.Bebroken( );
//            }
//        }
//    }
//}



