using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

public class InitializeGame : MonoBehaviour
{
    private HeroManager heroManager
    {
        get {
            return GameObject.FindObjectOfType<HeroManager>( ) as HeroManager;
        }
    }
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
    private ObstacleManager obstacleManager
    {
        get {
            return GameObject.FindObjectOfType<ObstacleManager>( ) as ObstacleManager;
        }
    }
    private TreasureManager treasureManager
    {
        get {
            return GameObject.FindObjectOfType<TreasureManager>( ) as TreasureManager;
        }
    }
    private FloorManager floorManager
    {
        get {
            return GameObject.FindObjectOfType<FloorManager>( ) as FloorManager;
        }
    }
    private AlertManager alertManager
    {
        get {
            return GameObject.FindObjectOfType<AlertManager>( ) as AlertManager;
        }
    }
    private GameManager gameManager
    {
        get {
            return GameObject.FindObjectOfType<GameManager>( ) as GameManager;
        }
    }
    private EquipmentManager equipmentManager
    {
        get {
            return GameObject.FindObjectOfType<EquipmentManager>( ) as EquipmentManager;
        }
    }
    private CameraManager cameraManager
    {
        get {
            return GameObject.FindObjectOfType<CameraManager>( ) as CameraManager;
        }
    }
    void Awake( )
    {
        InitializeIrregularFloors( );
        //InitializeFloors( );
        //InitializeAssociatedFLoors( );
        InitializeNeighborFLoors( );
        InitializeCameraMoveWithFLoors( );
        InitializeHero(0, 40, 8);
        InitializeEnemies( );
        InitializeObstacle( );
        InitializeTreasure( );
        InitializeEquipment( );
        //cameraManager.SetCameraPosition(gameManager.hero.data.currentFloor);
        InitializeCamera( );
    }

    void InitializeFloors( )
    {
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if((x + y) % 2 == 0) {
                    floorManager.floors[x, y] = Instantiate(floorManager.floorType[0]) as Floor;
                }
                else {
                    floorManager.floors[x, y] = Instantiate(floorManager.floorType[1]) as Floor;
                }
                floorManager.floors[x, y].transform.parent = floorManager.transform;
                floorManager.floors[x, y].transform.localPosition = new Vector3(-42 + (x * 12), 0, -54 + (y * 12));
                floorManager.floors[x, y].point = new Point(x, y);
            }
        }
    }

    Rectangle tempSumRect;
    Rectangle tempRect1 = new Rectangle(0, 0, 5, 5);
    Rectangle tempRect2 = new Rectangle(3, 3, 6, 6);
    Rectangle tempRect3 = new Rectangle(-3, -3, 6, 6);

    //void OnDrawGizmos( )
    //{
    //    Gizmos.color = UnityEngine.Color.yellow;
    //    Gizmos.DrawCube(new Vector3(tempRect1.X, 0, tempRect1.Y), new Vector3(tempRect1.Width, 0.1f, tempRect1.Height));
    //    Gizmos.color = UnityEngine.Color.red;
    //    Gizmos.DrawCube(new Vector3(tempRect2.X, 0, tempRect2.Y), new Vector3(tempRect2.Width, 0.1f, tempRect2.Height));
    //    Gizmos.color = UnityEngine.Color.blue;
    //    Gizmos.DrawCube(new Vector3(tempRect3.X, 0, tempRect3.Y), new Vector3(tempRect3.Width, 0.1f, tempRect3.Height));
    //}

    void InitializeIrregularFloors( )
    {
        tempSumRect = Rectangle.Union(tempRect1, tempRect2);
        tempSumRect = Rectangle.Union(tempSumRect, tempRect3);

        Debug.Log("top : " + tempSumRect.Top);
        Debug.Log("Bottom : " + tempSumRect.Bottom);
        Debug.Log("right : " + tempSumRect.Right);
        Debug.Log("left : " + tempSumRect.Left);
        //int height = tempSumRect.Bottom - tempSumRect.Top + 1;
        //int width = tempSumRect.Right - tempSumRect.Left + 1;
        //Debug.Log("height :" + height);
        //Debug.Log("width :" + width);
        //tempSumRect.Location = new System.Drawing.Point(tempSumRect.Left, tempSumRect.Bottom);
        //tempSumRect.X = tempSumRect.Left;
        //tempSumRect.Y = tempSumRect.Bottom;
        Debug.Log("tempSumRect.x :" + tempSumRect.X);
        Debug.Log("tempSumRect.y :" + tempSumRect.Y);
        int offsetX = Mathf.Abs(tempSumRect.X);
        int offsetY = Mathf.Abs(tempSumRect.Y);

        tempSumRect = new Rectangle(0, 0, tempSumRect.Width, tempSumRect.Height);
        tempRect1 = new Rectangle(0 + offsetX, 0 + offsetY, 5, 5);
        tempRect2 = new Rectangle(3 + offsetX, 3 + offsetY, 6, 6);
        tempRect3 = new Rectangle(-3 + offsetX, -3 + offsetY, 6, 6);

        floorManager.floors = new Floor[tempSumRect.Width, tempSumRect.Height];
        Debug.Log("tempSumRect.Width :" + tempSumRect.Width);
        Debug.Log("tempSumRect.Height :" + tempSumRect.Height);

        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if(tempRect1.Contains(x, y) || tempRect2.Contains(x, y) || tempRect3.Contains(x, y)) {
                    if((x + y) % 2 == 0) {
                        floorManager.floors[x, y] = Instantiate(floorManager.floorType[0]) as Floor;
                    }
                    else {
                        floorManager.floors[x, y] = Instantiate(floorManager.floorType[1]) as Floor;
                    }
                    floorManager.floors[x, y].transform.parent = floorManager.transform;
                    floorManager.floors[x, y].transform.localPosition = new Vector3(x, 0, y);
                    floorManager.floors[x, y].point = new Point(x, y);
                }
            }
        }

    }

    void InitializeAssociatedFLoors( )
    {
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                floorManager.floors[x, y].associatedFloors = new Floor[8];
                if(x >= 1 && y >= 1) {
                    floorManager.floors[x, y].associatedFloors[0] = floorManager.floors[x - 1, y - 1];
                }
                if(x >= 0 && y >= 1) {
                    floorManager.floors[x, y].associatedFloors[1] = floorManager.floors[x, y - 1];
                }
                if(x <= floorManager.floors.GetLength(0) - 2 && y >= 1) {
                    floorManager.floors[x, y].associatedFloors[2] = floorManager.floors[x + 1, y - 1];
                }
                if(x >= 1 && y >= 0) {
                    floorManager.floors[x, y].associatedFloors[3] = floorManager.floors[x - 1, y];
                }
                if(x <= floorManager.floors.GetLength(0) - 2 && y >= 0) {
                    floorManager.floors[x, y].associatedFloors[4] = floorManager.floors[x + 1, y];
                }
                if(x >= 1 && y <= floorManager.floors.GetLength(1) - 2) {
                    floorManager.floors[x, y].associatedFloors[5] = floorManager.floors[x - 1, y + 1];
                }
                if(x >= 0 && y <= floorManager.floors.GetLength(1) - 2) {
                    floorManager.floors[x, y].associatedFloors[6] = floorManager.floors[x, y + 1];
                }
                if(x <= floorManager.floors.GetLength(0) - 2 && y <= floorManager.floors.GetLength(1) - 2) {
                    floorManager.floors[x, y].associatedFloors[7] = floorManager.floors[x + 1, y + 1];
                }
            }
        }
    }

    void InitializeNeighborFLoors( )
    {
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if(floorManager.floors[x, y] != null) {
                    floorManager.floors[x, y].neighborFloors = new Floor[4];
                    if(x >= 0 && y >= 1) {
                        floorManager.floors[x, y].neighborFloors[0] = floorManager.floors[x, y - 1];
                    }
                    if(x >= 1 && y >= 0) {
                        floorManager.floors[x, y].neighborFloors[1] = floorManager.floors[x - 1, y];
                    }
                    if(x <= floorManager.floors.GetLength(0) - 2 && y >= 0) {
                        floorManager.floors[x, y].neighborFloors[2] = floorManager.floors[x + 1, y];
                    }
                    if(x >= 0 && y <= floorManager.floors.GetLength(1) - 2) {
                        floorManager.floors[x, y].neighborFloors[3] = floorManager.floors[x, y + 1];
                    }
                }
            }
        }
    }

    void InitializeCameraMoveWithFLoors( )
    {
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if(floorManager.floors[x, y] != null) {
                    int temp = 0;
                    for(int z = 0; z < floorManager.floors[x, y].neighborFloors.Length; z++) {
                        if(floorManager.floors[x, y].neighborFloors[z] == null) {
                            temp++;
                        }
                    }
                    if(temp == 0) {
                        floorManager.floors[x, y].isCameraMove = true;
                    }
                    else {
                        floorManager.floors[x, y].isCameraMove = false;
                    }
                }
            }
        }
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if(floorManager.floors[x, y] != null && !floorManager.floors[x, y].isCameraMove) {
                    if(floorManager.floors[x, y].neighborFloors[0] == null) {
                        if(floorManager.floors[x, y].neighborFloors[3] != null) {
                            floorManager.floors[x, y].neighborFloors[3].isCameraMove = false;
                        }
                    }
                    if(floorManager.floors[x, y].neighborFloors[1] == null) {
                        if(floorManager.floors[x, y].neighborFloors[2] != null) {
                            floorManager.floors[x, y].neighborFloors[2].isCameraMove = false;
                        }
                    }
                    if(floorManager.floors[x, y].neighborFloors[2] == null) {
                        if(floorManager.floors[x, y].neighborFloors[1] != null) {
                            floorManager.floors[x, y].neighborFloors[1].isCameraMove = false;
                        }
                    }
                    if(floorManager.floors[x, y].neighborFloors[3] == null) {
                        if(floorManager.floors[x, y].neighborFloors[0] != null) {
                            floorManager.floors[x, y].neighborFloors[0].isCameraMove = false;
                        }
                    }
                }
            }
        }
    }

    void InitializeCamera( )
    {
        List<Floor> tempFloors = floorManager.GetCanMoveCameraFloors( );
        Floor tempFloor;
        float tempFloat = 100000;
        foreach(Floor floor in tempFloors) {
            if((gameManager.hero.data.currentFloor.Position( ) - floor.Position( )).sqrMagnitude < tempFloat) {
                tempFloat = (gameManager.hero.data.currentFloor.Position( ) - floor.Position( )).sqrMagnitude;
                tempFloor = floor;
                cameraManager.SetCameraPosition(tempFloor);
            }
        }
    }
    void InitializeEnemies( )
    {
        List<Floor> tempFloors = new List<Floor>( );
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if(floorManager.floors[x, y] != null) {
                    if(gameManager.hero.data.currentFloor != floorManager.floors[x, y]) {
                        tempFloors.Add(floorManager.floors[x, y]);
                    }
                }
            }
        }
        for(int i = 0; i < enemyManager.enemyCount; i++) {
            int randomInt = Random.Range(0, tempFloors.Count);
            Enemy enemy = Instantiate(enemyManager.enemyType[Random.Range(0, enemyManager.enemyType.Length)]);
            enemy.transform.parent = enemyManager.transform;
            enemy.transform.localPosition = tempFloors[randomInt].transform.position - Vector3.up * 2;

            enemy.data.point = tempFloors[randomInt].point;
            enemy.data.currentFloor = tempFloors[randomInt];
            enemyManager.enemies.Add(enemy);
            tempFloors[randomInt].enemy = enemy;
            tempFloors.Remove(tempFloors[randomInt]);

        }
    }
    void InitializeHero( int type, float health, float atk )
    {
        int x = 0, y = 0;
        Floor startFloor;
        while(true) {
            x = Random.Range(0, floorManager.floors.GetLength(0));
            y = Random.Range(0, floorManager.floors.GetLength(1));
            if(floorManager.floors[x, y] != null) {
                startFloor = floorManager.floors[x, y];
                break;
            }
            else {
                continue;
            }
        }
        gameManager.hero = Instantiate(heroManager.heroType[type]) as Hero;
        gameManager.hero.transform.position = new Vector3(startFloor.transform.position.x, 0.5f, startFloor.transform.position.z);

        //gameManager.hero.data = new CharacterData();
        gameManager.hero.data.point = new Point(x, y);
        gameManager.hero.data.currentFloor = startFloor;
        startFloor.hero = gameManager.hero;
    }
    void InitializeObstacle( )
    {
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if(floorManager.floors[x, y] != null && !gameManager.hero.data.point.IsIdentical(new Point(x, y))) {
                    Obstacle ob = Instantiate(obstacleManager.obstacleType[Random.Range(0, obstacleManager.obstacleType.Length)]) as Obstacle;
                    ob.transform.parent = obstacleManager.transform;
                    ob.transform.localPosition = new Vector3(x, 0.5f, y);
                    ob.point = new Point(x, y);
                    ob.currentFloor = floorManager.floors[x, y];
                    floorManager.floors[x, y].obstacle = ob;
                }
            }
        }
    }
    void InitializeTreasure( )
    {
        List<Floor> tempFloors = new List<Floor>( );
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if(floorManager.floors[x, y] != null && floorManager.floors[x, y].hero == null && floorManager.floors[x, y].enemy == null) {
                    tempFloors.Add(floorManager.floors[x, y]);
                }
            }
        }
        for(int i = 0; i < treasureManager.treasureNumber; i++) {
            Treasure ts = Instantiate(treasureManager.treasureType[Random.Range(0, treasureManager.treasureType.Length)]) as Treasure;
            int randomInt = Random.Range(0, tempFloors.Count);
            ts.transform.parent = treasureManager.transform;
            ts.transform.position = tempFloors[randomInt].transform.position + Vector3.up * 0.5f;
            ts.point = tempFloors[randomInt].point;
            ts.currentFloor = tempFloors[randomInt];
            tempFloors[randomInt].treasure = ts;
            treasureManager.treasures.Add(ts);
            tempFloors.Remove(tempFloors[randomInt]);
        }
    }

    void InitializeEquipment( )
    {
        List<Floor> tempFloors = new List<Floor>( );
        for(int y = 0; y < floorManager.floors.GetLength(1); y++) {
            for(int x = 0; x < floorManager.floors.GetLength(0); x++) {
                if(floorManager.floors[x, y] != null) {
                    if(floorManager.floors[x, y].hero == null && floorManager.floors[x, y].enemy == null && floorManager.floors[x, y].treasure == null) {
                        tempFloors.Add(floorManager.floors[x, y]);
                    }
                }
            }
        }

        for(int i = 0; i < equipmentManager.equipNumber; i++) {
            Equipment e = Instantiate(equipmentManager.equipType[Random.Range(0, equipmentManager.equipType.Length)]) as Equipment;
            int randomInt = Random.Range(0, tempFloors.Count);
            e.transform.parent = equipmentManager.transform;
            e.transform.position = tempFloors[randomInt].Position() + Vector3.up;
            e.point = tempFloors[randomInt].point;
            e.currentFloor = tempFloors[randomInt];
            tempFloors[randomInt].equipment = e;
            equipmentManager.equipments.Add(e);
            tempFloors.Remove(tempFloors[randomInt]);
        }
    }
}
