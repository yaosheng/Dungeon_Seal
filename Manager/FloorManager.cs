using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorManager : MonoBehaviour
{
    private CameraManager CM
    {
        get {
            return GameObject.FindObjectOfType<CameraManager>( ) as CameraManager;
        }
    }
    public Floor[ ] floorType;
    public Floor[ , ] floors;
    public Material standardMaterial;
    public Material attackedMaterial;

    public List<Floor> GetCrossFloors( Point point )
    {
        List<Floor> fs = new List<Floor>( );
        if(point.x >= 0 && point.y >= 1) {
            fs.Add(floors[point.x, point.y - 1]);
        }
        if(point.x >= 1 && point.y >= 0) {
            fs.Add(floors[point.x - 1, point.y]);
        }
        if(point.x <= floors.GetLength(0) - 2 && point.y >= 0) {
            fs.Add(floors[point.x + 1, point.y]);
        }
        if(point.x >= 0 && point.y <= floors.GetLength(1) - 2) {
            fs.Add(floors[point.x, point.y + 1]);
        }
        return fs;
    }

    public List<Enemy> GetCrossEnemies( Point point )
    {
        List<Enemy> es = new List<Enemy>( );
        if(point.x >= 0 && point.y >= 1 && floors[point.x, point.y - 1] != null && floors[point.x, point.y - 1].enemy != null) {
            es.Add(floors[point.x, point.y - 1].enemy);
        }
        if(point.x >= 1 && point.y >= 0 && floors[point.x - 1, point.y] != null && floors[point.x - 1, point.y].enemy != null) {
            es.Add(floors[point.x - 1, point.y].enemy);
        }
        if(point.x <= floors.GetLength(0) - 2 && point.y >= 0 && floors[point.x + 1, point.y] != null && floors[point.x + 1, point.y].enemy != null) {
            es.Add(floors[point.x + 1, point.y].enemy);
        }
        if(point.x >= 0 && point.y <= floors.GetLength(1) - 2 && floors[point.x, point.y + 1] != null && floors[point.x, point.y + 1].enemy != null) {
            es.Add(floors[point.x, point.y + 1].enemy);
        }
        return es;
    }

    public List<Barrier> GetCrossBarriers( Point point )
    {
        List<Barrier> bs = new List<Barrier>( );

        if(point.x >= 0 && point.y >= 1 && floors[point.x, point.y - 1].barrier != null) {
            bs.Add(floors[point.x, point.y - 1].barrier);
        }
        if(point.x >= 1 && point.y >= 0 && floors[point.x - 1, point.y].barrier != null) {
            bs.Add(floors[point.x - 1, point.y].barrier);
        }
        if(point.x <= floors.GetLength(0) - 2 && point.y >= 0 && floors[point.x + 1, point.y].barrier != null) {
            bs.Add(floors[point.x + 1, point.y].barrier);
        }
        if(point.x >= 0 && point.y <= floors.GetLength(1) - 2 && floors[point.x, point.y + 1].barrier != null) {
            bs.Add(floors[point.x, point.y + 1].barrier);
        }
        return bs;
    }

    public List<Floor> GetEndFloors( Point point )
    {
        List<Floor> fs = new List<Floor>( );
        if(point.x >= 1 && point.y >= 1) {
            fs.Add(floors[point.x - 1, point.y - 1]);
        }
        if(point.x <= floors.GetLength(0) - 2 && point.y >= 1) {
            fs.Add(floors[point.x + 1, point.y - 1]);
        }
        if(point.x >= 1 && point.y <= floors.GetLength(1) - 2) {
            fs.Add(floors[point.x - 1, point.y + 1]);
        }
        if(point.x <= floors.GetLength(0) - 2 && point.y <= floors.GetLength(1) - 2) {
            fs.Add(floors[point.x + 1, point.y + 1]);
        }
        return fs;
    }

    public void DestoryUselessBarriers( Floor floor )
    {
        for(int y = 0; y < floors.GetLength(1); y++) {
            for(int x = 0; x < floors.GetLength(0); x++) {
                if(floors[x, y] != null && floors[x, y].barrier != null) {
                    bool tempBool = true;
                    List<Enemy> tempEnemies = GetCrossEnemies(floors[x, y].point);
                    foreach(Enemy enemy in tempEnemies) {
                        if(!enemy.isComingout) {
                            tempBool = false;
                        }
                    }
                    if(tempBool) {
                        floors[x, y].barrier.Destory( );
                    }
                }
            }
        }
    }

    public void CheckSurroundedEnemies( )
    {
        for(int y = 0; y < floors.GetLength(1); y++) {
            for(int x = 0; x < floors.GetLength(0); x++) {
                if(floors[x, y] != null && floors[x, y].enemy != null) {
                    int temp = 0;
                    List<Floor> tempfloors = GetCrossFloors(floors[x, y].point);
                    foreach(Floor floor in tempfloors) {
                        if(floor != null) {
                            if(floor.enemy != null && !floor.enemy.isComingout) {
                                temp++;
                            }
                            if(floor.barrier != null) {
                                temp++;
                            }
                        }
                    }
                    if(floors[x, y].point.x == floors.GetLength(0) - 1 || floors[x, y].point.x == 0) {
                        temp++;
                    }
                    if(floors[x, y].point.y == floors.GetLength(1) - 1 || floors[x, y].point.y == 0) {
                        temp++;
                    }
                    if(temp == 4) {
                        floors[x, y].enemy.isSurround = true;
                    }
                    else {
                        floors[x, y].enemy.isSurround = false;
                    }
                }
            }
        }
    }
    public List<List<Enemy>> FindConnectedAndSurroundedEnemies( )
    {
        List<List<Enemy>> enemylistlist = new List<List<Enemy>>( );
        for(int y = 0; y < floors.GetLength(1); y++) {
            for(int x = 0; x < floors.GetLength(0); x++) {
                if(floors[x, y] != null) {
                    if(floors[x, y].enemy != null && floors[x, y].enemy.isSurround) {
                        List<Enemy> enemyList = new List<Enemy>( );
                        enemyList.Add(floors[x, y].enemy);
                        while(true) {
                            int listCount = enemyList.Count;
                            List<Enemy> tempenemyList2 = new List<Enemy>( );
                            foreach(Enemy e1 in enemyList) {
                                List<Enemy> tempenemyList1 = GetCrossEnemies(e1.data.point);
                                foreach(Enemy e in tempenemyList1) {
                                    if(!enemyList.Contains(e)) {
                                        tempenemyList2.Add(e);
                                    }
                                }
                            }
                            foreach(Enemy e in tempenemyList2) {
                                enemyList.Add(e);
                            }
                            if(enemyList.Count != listCount) {
                                continue;
                            }
                            else {
                                break;
                            }
                        }
                        int temp = 0;
                        foreach(Enemy e in enemyList) {
                            if(e.isSurround && !e.isComingout) {
                                temp++;
                            }
                        }
                        if(temp != 0 && temp == enemyList.Count) {
                            if(!IsEnemyListContainedByEnemylistList(enemylistlist, enemyList)) {
                                enemylistlist.Add(enemyList);
                            }
                        }
                    }
                }
            }
        }
        return enemylistlist;
    }


    bool IsEnemyListContainedByEnemylistList( List<List<Enemy>> enemylistList, List<Enemy> enemylist )
    {
        bool tempBool = false;
        foreach(List<Enemy> el in enemylistList) {
            int tempInt = 0;
            foreach(Enemy e in enemylist) {
                if(el.Contains(e)) {
                    tempInt++;
                }
            }
            if(tempInt == el.Count) {
                tempBool = true;
            }
        }
        return tempBool;
    }

    public void DestoryObstaclesByPoints( List<Point> points )
    {
        foreach(Point point in points) {
            floors[point.x, point.y].obstacle.Destory( );
        }
    }

    public void HurtEnemiesByPoints( List<Point> points, Hero hero, float atk )
    {
        foreach(Point p in points) {
            floors[p.x, p.y].enemy.FaceToAndComingOut(hero.data.point, 0.0f);
            floors[p.x, p.y].enemy.BeAttacked(atk);
        }
    }

    public void ResetRenderer( )
    {
        MeshRenderer mr;
        foreach(Floor floor in floors) {
            if(floor != null) {
                if(floor.obstacle != null) {
                    mr = floor.obstacle.GetComponentInChildren<MeshRenderer>( );
                    if(mr.sharedMaterial == attackedMaterial) {
                        mr.sharedMaterial = standardMaterial;
                    }
                }
                else if(floor.enemy != null) {
                    mr = floor.enemy.GetComponentInChildren<MeshRenderer>( );
                    if(mr.sharedMaterial == attackedMaterial) {
                        mr.sharedMaterial = standardMaterial;
                    }
                }
            }
        }
    }

    public void ChangeRenderer( Collider collider )
    {
        Collider co;
        MeshRenderer mr1;
        foreach(Floor floor in floors) {
            if(floor != null) {
                if(floor.obstacle != null) {
                    co = floor.obstacle.GetComponentInChildren<Collider>( );
                    mr1 = floor.obstacle.GetComponentInChildren<MeshRenderer>( );
                    if(collider == co) {
                        if(mr1.sharedMaterial == standardMaterial) {
                            mr1.sharedMaterial = attackedMaterial;
                        }
                    }
                    else {
                        if(mr1.sharedMaterial == attackedMaterial) {
                            mr1.sharedMaterial = standardMaterial;
                        }
                    }
                }
                else if(floor.enemy != null) {
                    co = floor.enemy.GetComponentInChildren<Collider>( );
                    mr1 = floor.enemy.GetComponentInChildren<MeshRenderer>( );
                    if(collider == co) {
                        if(mr1.sharedMaterial == standardMaterial) {
                            mr1.sharedMaterial = attackedMaterial;
                        }
                    }
                    else {
                        if(mr1.sharedMaterial == attackedMaterial) {
                            mr1.sharedMaterial = standardMaterial;
                        }
                    }
                }
            }
        }
    }

    List<Floor> GetEdgeFloors( )
    {
        List<Floor> edgeFloors = new List<Floor>( );
        for(int y = 0; y < floors.GetLength(1); y++) {
            for(int x = 0; x < floors.GetLength(0); x++) {
                if(floors[x, y] != null) {
                    int temp = 0;
                    for(int z = 0; z < floors[x, y].neighborFloors.Length; z++) {
                        if(floors[x, y].neighborFloors[z] == null) {
                            temp++;
                        }
                    }
                    if(temp != 0) {
                        edgeFloors.Add(floors[x, y]);
                    }
                }
            }
        }
        return edgeFloors;
    }

    public List<Floor> GetCanMoveCameraFloors( )
    {
        List<Floor> moveFloors = new List<Floor>( );
        for(int y = 0; y < floors.GetLength(1); y++) {
            for(int x = 0; x < floors.GetLength(0); x++) {
                if(floors[x, y] != null) {
                    if(floors[x, y].isCameraMove) {
                        moveFloors.Add(floors[x, y]);
                    }
                }
            }
        }
        return moveFloors;
    }

    bool IsFarByEdgeFloor( int direction, Floor targetFloor )
    {
        bool tempBool = false;
        Floor tempFloor = targetFloor;
        int temp = 0;
        for(int i = 0; i < 2; i++) {
            if(tempFloor.neighborFloors[direction] != null) {
                tempFloor = targetFloor.neighborFloors[direction];
                temp++;
            }
        }
        if(temp == 2) {
            tempBool = true;
        }
        else {
            tempBool = false;
        }
        return tempBool;
    }

    public void RestrictiveCameraMove( int rotation, Floor targetFloor )
    {
        switch(rotation) {
            case 90:
            if(IsFarByEdgeFloor(2, targetFloor) && IsFarByEdgeFloor(1, targetFloor)) {
                if(IsFarByEdgeFloor(3, targetFloor) && IsFarByEdgeFloor(0, targetFloor)) {
                    CM.MovePosition(targetFloor);
                }
                else {
                    CM.MoveX(targetFloor);
                }
            }
            break;
            case -90:
            if(IsFarByEdgeFloor(1, targetFloor) && IsFarByEdgeFloor(2, targetFloor)) {
                if(IsFarByEdgeFloor(3, targetFloor) && IsFarByEdgeFloor(0, targetFloor)) {
                    CM.MovePosition(targetFloor);
                }
                else {
                    CM.MoveX(targetFloor);
                }
            }
            break;
            case 0:
            if(IsFarByEdgeFloor(3, targetFloor) && IsFarByEdgeFloor(0, targetFloor)) {
                if(IsFarByEdgeFloor(1, targetFloor) && IsFarByEdgeFloor(2, targetFloor)) {
                    CM.MovePosition(targetFloor);
                }
                else {
                    CM.MoveZ(targetFloor);
                }
  
            }
            break;
            case 180:
            if(IsFarByEdgeFloor(0, targetFloor) && IsFarByEdgeFloor(3, targetFloor)) {
                if(IsFarByEdgeFloor(1, targetFloor) && IsFarByEdgeFloor(2, targetFloor)) {
                    CM.MovePosition(targetFloor);
                }
                else {
                    CM.MoveZ(targetFloor);
                }
            }
            break;
        }
    }

    //public void RestrictiveCameraMove(int direction, Floor targetFloor)
    //{
    //    List<Floor> edgeFloors = GetEdgeFloors( );

    //    bool IsmoveX = false;
    //    bool IsmoveZ = false;
    //    int temp = 10000;
    //    Floor rightFloor;

    //    List<Floor> tempFloors = new List<Floor>( );

    //    foreach(Floor floor in edgeFloors) {
    //        if(floor.neighborFloors[0] == null && targetFloor.point.x == floor.point.x) {
    //            tempFloors.Add(floor);
    //            //rightFloor = floor;
    //        }
    //    }

    //    foreach(Floor floor in tempFloors) {
    //        if(Mathf.Abs(targetFloor.point.y - floor.point.y) < temp) {
    //            rightFloor = floor;
    //            temp = Mathf.Abs(targetFloor.point.y - floor.point.y);
    //        }
    //    }
    //}

    //List<Enemy> GetConnectedAndSurroundedEnemies(List<Enemy> enemyList )
    //{
    //    List<Enemy> tempenemyList = new List<Enemy>( );

    //    while(true) {
    //        int listCount = enemyList.Count;
    //        List<Enemy> tempenemyList2 = new List<Enemy>( );
    //        foreach(Enemy e1 in enemyList) {
    //            foreach(Enemy e in GetCrossEnemies(e1.data.point)) {
    //                if(!enemyList.Contains(e)) {
    //                    tempenemyList2.Add(e);
    //                }
    //            }
    //        }
    //        foreach(Enemy e in tempenemyList2) {
    //            enemyList.Add(e);
    //        }
    //        if(enemyList.Count != listCount) {
    //            continue;
    //        }
    //        else {
    //            break;
    //        }
    //    }

    //    return tempenemyList;
    //}

    //List<Barrier> tempBarriers = GetCrossBarriers(floor.point);
    //foreach(Barrier ba in tempBarriers) {
    //    bool tempBool = true;
    //    List<Enemy> tempEnemies = GetCrossEnemies(ba.point);
    //    foreach(Enemy enemy in tempEnemies) {
    //        //if(floor.point.IsIdentical(enemy.data.point) && enemy.isComingout) {
    //        //    tempBool = true;
    //        //}
    //        if(!floor.point.IsIdentical(enemy.data.point) && !enemy.isComingout) {
    //            tempBool = false;
    //        }
    //    }
    //    if(tempBool) {
    //        ba.Destory( );
    //    }
    //}
    //public void DestoryUselessBarriersByLaunch( Floor floor )
    //{
    //    List<Barrier> tempBarriers = GetCrossBarriers(floor.point);
    //    foreach(Barrier ba in tempBarriers) {
    //        bool tempBool = true;
    //        List<Enemy> tempEnemies = GetCrossEnemies(ba.point);
    //        foreach(Enemy enemy in tempEnemies) {
    //            if(!floor.point.IsIdentical(enemy.data.point)) {
    //                tempBool = false;
    //            }
    //        }
    //        if(tempBool) {
    //            ba.Destory( );
    //        }
    //    }
    //}

    //public void CheckConnectedEnemies( )
    //{
    //    List<List<Enemy>> enemylistList = FindConnectedAndSurroundedEnemies( );
    //    //if(enemylistList.Count > 0) {
    //    //    foreach(List<Enemy> enemylist in enemylistList) {
    //    //        StartCoroutine(DestorySomeThingBySurrounded(GetPointsByEnemies(enemylist)));
    //    //    }
    //    //}
    //    //else {
    //    //    inputManager.isControled = false;
    //    //    Debug.Log("iscontrol :" + inputManager.isControled);
    //    //}
    //}

    //List<Floor> tempFloors = GetCrossFloors(point);
    //foreach(Floor floor in tempFloors) {
    //    bool tempBool = false;
    //    if(floor.barrier != null) {
    //        tempBool = true;
    //        List<Floor> tempFloors1 = GetCrossFloors(floor.point);
    //        foreach(Floor floor1 in tempFloors1) {
    //            if(floor1.enemy != null) {
    //                if(!point.IsIdentical(floor1.point) && !floor1.enemy.isComingout) {
    //                    tempBool = false;
    //                }
    //            }
    //        }
    //    }
    //    if(tempBool) {
    //        floor.barrier.Destory( );
    //    }
    //}

    //public Point GetRightPoint(Point point )
    //{
    //    return 
    //}

    //void InitializeAssociatedFLoors( )
    //{
    //    for(int y = 0; y < Data.Height; y++) {
    //        for(int x = 0; x < Data.Width; x++) {
    //            floorManager.floors[x, y].associatedFloors = new Floor[8];
    //            if(x >= 1 && y >= 1) {
    //                floorManager.floors[x, y].associatedFloors[0] = floorManager.floors[x - 1, y - 1];
    //            }
    //            if(x >= 0 && y >= 1) {
    //                floorManager.floors[x, y].associatedFloors[1] = floorManager.floors[x, y - 1];
    //            }
    //            if(x <= Data.Width - 2 && y >= 1) {
    //                floorManager.floors[x, y].associatedFloors[2] = floorManager.floors[x + 1, y - 1];
    //            }
    //            if(x >= 1 && y >= 0) {
    //                floorManager.floors[x, y].associatedFloors[3] = floorManager.floors[x - 1, y];
    //            }
    //            if(x <= Data.Width - 2 && y >= 0) {
    //                floorManager.floors[x, y].associatedFloors[4] = floorManager.floors[x + 1, y];
    //            }
    //            if(x >= 1 && y <= Data.Height - 2) {
    //                floorManager.floors[x, y].associatedFloors[5] = floorManager.floors[x - 1, y + 1];
    //            }
    //            if(x >= 0 && y <= Data.Height - 2) {
    //                floorManager.floors[x, y].associatedFloors[6] = floorManager.floors[x, y + 1];
    //            }
    //            if(x <= Data.Width - 2 && y <= Data.Height - 2) {
    //                floorManager.floors[x, y].associatedFloors[7] = floorManager.floors[x + 1, y + 1];
    //            }
    //        }
    //    }
    //}
}
