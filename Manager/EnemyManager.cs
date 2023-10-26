using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    private UIManager uiManager
    {
        get {
            return GameObject.FindObjectOfType<UIManager>( ) as UIManager;
        }
    }
    public Enemy[ ] enemyType;
    public List<Enemy> enemies = new List<Enemy>( );
    public int enemyCount = 12;

    void Update( )
    {
        CheckEnemiesCount( );
    }

    void CheckEnemiesCount( )
    {
        int temp = 0;
        foreach(Enemy enemy in enemies) {
            if(!enemy.gameObject.activeSelf && enemy.isComingout) {
                temp++;
            }
        }
        if(temp == enemyCount) {
            uiManager.ShowGameOverUI( );
        }
    }
}




