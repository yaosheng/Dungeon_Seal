//using UnityEngine;
//using System.Collections;

//public class InPutManager : MonoBehaviour
//{
//    //    private Hero hero
//    //    {
//    //        get {
//    //            return GameObject.FindObjectOfType<Hero>( ) as Hero;
//    //        }
//    //    }
//    //    private ObstacleManager obstacleManager
//    //    {
//    //        get {
//    //            return GameObject.FindObjectOfType<ObstacleManager>( ) as ObstacleManager;
//    //        }
//    //    }
//    //    private TreasureManager treasureManager
//    //    {
//    //        get {
//    //            return GameObject.FindObjectOfType<TreasureManager>( ) as TreasureManager;
//    //        }
//    //    }
//    //    private EnemyManager enemyManager
//    //    {
//    //        get {
//    //            return GameObject.FindObjectOfType<EnemyManager>( ) as EnemyManager;
//    //        }
//    //    }
//    //    private AlertManager alertManager
//    //    {
//    //        get {
//    //            return GameObject.FindObjectOfType<AlertManager>( ) as AlertManager;
//    //        }
//    //    }
//    //    public bool isControled = false;

//    //    void Update( )
//    //    {

//    //    }

//    //    public void DragAction( int direction )
//    //    {
//    //        if(iTween.Count(hero.gameObject) == 0 && !isControled) {
//    //            isControled = true;
//    //            switch(direction) {
//    //                case 0:
//    //                if(hero.data.point.x + 1 <= Data.Width - 1) {
//    //                    SwitchAction(90, new Point(hero.data.point.x + 1, hero.data.point.y));
//    //                }
//    //                break;
//    //                case 1:
//    //                if(hero.data.point.x - 1 >= 0) {
//    //                    SwitchAction(-90, new Point(hero.data.point.x - 1, hero.data.point.y));
//    //                }
//    //                break;
//    //                case 2:
//    //                if(hero.data.point.y + 1 <= Data.Height - 1) {
//    //                    SwitchAction(0, new Point(hero.data.point.x, hero.data.point.y + 1));
//    //                }
//    //                break;
//    //                case 3:
//    //                if(hero.data.point.y - 1 >= 0) {
//    //                    SwitchAction(180, new Point(hero.data.point.x, hero.data.point.y - 1));
//    //                }
//    //                break;
//    //            }
//    //        }
//    //    }

//    //    void SwitchAction( int rotation, Point targetPoint )
//    //    {
//    //        if(obstacleManager.IsThereObstacle(targetPoint)) {
//    //            if(enemyManager.IsThereEnemy(targetPoint)) {
//    //                StartCoroutine(AttackObstacleWithEnemy(rotation, targetPoint));
//    //            }
//    //            else if(treasureManager.IsThereTreasure(targetPoint)) {
//    //                StartCoroutine(AttackObstacleWithTreasure(rotation, targetPoint));
//    //            }
//    //            else {
//    //                StartCoroutine(AttackObstacleWithoutEnemy(rotation, targetPoint));
//    //            }
//    //        }
//    //        else if(enemyManager.IsThereEnemy(targetPoint)) {
//    //            StartCoroutine(AttackOpponent(rotation, targetPoint));
//    //        }
//    //        else {
//    //            StartCoroutine(MoveAndDoSomeThing(rotation, targetPoint));
//    //        }
//    //    }

//    //    IEnumerator MoveAndDoSomeThing( int rotation, Point targetPoint )
//    //    {
//    //        hero.Move(rotation, targetPoint);
//    //        //float timeTotal = Data.moveSpeed + 0.05f;
//    //        yield return new WaitForSeconds(Data.moveSpeed);
//    //        alertManager.ShowAlert(hero.data.point);
//    //        treasureManager.CheckTreasureMatchByPoint(hero.data.point);
//    //        enemyManager.CheckConnectedEnemies( );
//    //    }

//    //    IEnumerator AttackAction( int rotation, Point targetPoint )
//    //    {
//    //        iTween.RotateTo(hero.gameObject, iTween.Hash("y", rotation, "time", Data.turnSpeed, "easetype", iTween.EaseType.linear));
//    //        yield return new WaitForSeconds(Data.turnSpeed);
//    //        iTween.RotateTo(hero.gameObject, iTween.Hash("x", -20, "time", Data.readyTime, "delay", Data.turnSpeed, "easetype", iTween.EaseType.linear));
//    //        iTween.RotateTo(hero.gameObject, iTween.Hash("x", 40, "time", Data.atttTime, "delay", Data.readyTime + Data.turnSpeed, "easetype", iTween.EaseType.linear));
//    //        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
//    //        iTween.RotateTo(hero.gameObject, iTween.Hash("x", 0, "time", Data.atttTime, "easetype", iTween.EaseType.linear));
//    //        yield return new WaitForSeconds(Data.atttTime);
//    //    }

//    //    IEnumerator AttackObstacleWithEnemy( int rotation, Point targetPoint )
//    //    {
//    //        StartCoroutine(AttackAction(rotation, targetPoint));
//    //        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
//    //        obstacleManager.DestoryObstacleByPoint(targetPoint);
//    //        if(enemyManager.IsThereEnemy(targetPoint)) {
//    //            enemyManager.ReadyToFight(targetPoint);
//    //            enemyManager.AttackOpponent(targetPoint);
//    //        }
//    //        isControled = false;
//    //    }

//    //    IEnumerator AttackObstacleWithTreasure( int rotation, Point targetPoint )
//    //    {
//    //        StartCoroutine(AttackAction(rotation, targetPoint));
//    //        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
//    //        obstacleManager.DestoryObstacleByPoint(targetPoint);
//    //        if(treasureManager.IsThereTreasure(targetPoint)) {
//    //            treasureManager.TreasureComingOut(targetPoint);
//    //        }
//    //        isControled = false;
//    //    }

//    //    IEnumerator AttackObstacleWithoutEnemy( int rotation, Point targetPoint )
//    //    {
//    //        StartCoroutine(AttackAction(rotation, targetPoint));
//    //        yield return new WaitForSeconds(Data.turnSpeed);
//    //        //StartCoroutine(hero.Move(rotation, targetPoint));
//    //        StartCoroutine(MoveAndDoSomeThing(rotation, targetPoint));
//    //        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
//    //        obstacleManager.DestoryObstacleByPoint(targetPoint);
//    //        if(enemyManager.IsThereEnemy(targetPoint)) {
//    //            enemyManager.ReadyToFight(targetPoint);
//    //            enemyManager.AttackOpponent(targetPoint);
//    //        }
//    //        isControled = false;
//    //    }

//    //    IEnumerator AttackOpponent( int rotation, Point targetPoint )
//    //    {
//    //        StartCoroutine(AttackAction(rotation, targetPoint));
//    //        enemyManager.FaceToHero(targetPoint);
//    //        yield return new WaitForSeconds(Data.turnSpeed + Data.readyTime + Data.atttTime);
//    //        enemyManager.BeAttackByOpponent(targetPoint, hero.data.atk);
//    //        enemyManager.AttackOpponent(targetPoint);
//    //        alertManager.ShowAlert(hero.data.point);
//    //        isControled = false;
//    //    }
//}
