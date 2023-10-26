using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrierManager : MonoBehaviour
{
    private FloorManager floorManager
    {
        get {
            return GameObject.FindObjectOfType<FloorManager>( ) as FloorManager;
        }
    }
    public Barrier barrier;
    public List<Barrier> barriers = new List<Barrier>( );
    public BarrierLine barrierLine;
    public Transform barrierlineParent;

    public void SetBarrier( Point point )
    {
        int temp = 0;
        foreach(Barrier b in barriers) {
            if(b.point.IsIdentical(point)) {
                temp++;
            }
        }
        if(temp == 0) {
            Barrier ba = Instantiate(barrier) as Barrier;
            ba.transform.parent = this.transform;
            ba.transform.position = floorManager.floors[point.x, point.y].transform.position + Vector3.up * 0.5f;
            ba.point = new Point(point.x, point.y);
            ba.currentFloor = floorManager.floors[point.x, point.y];
            floorManager.floors[point.x, point.y].barrier = ba;
            barriers.Add(ba);
        }
        else {
            return;
        }
    }

    public void ShowBarrierLine( List<Point> points )
    {
        Barrier[ ] barrierArray = barriers.ToArray( );
        for(int i = 0; i < barrierArray.Length - 1; i++) {
            bool tmpBool1 = false;
            foreach(Point point in points) {
                if(barrierArray[i].point.IsNeighbor(point)) {
                    tmpBool1 = true;
                }
            }
            for(int j = i + 1; j < barrierArray.Length; j++) {
                bool tmpBool2 = false;
                foreach(Point point in points) {
                    if(barrierArray[j].point.IsNeighbor(point)) {
                        tmpBool2 = true;
                    }
                }
                if(tmpBool1 && tmpBool2 && barrierArray[i].point.IsAround(barrierArray[j].point) && barrierArray[i].gameObject.activeSelf && barrierArray[j].gameObject.activeSelf) {
                    BarrierLine bl = Instantiate(barrierLine) as BarrierLine;
                    bl.Show(barrierArray[i].transform.position, barrierArray[j].transform.position);
                    bl.transform.parent = barrierlineParent;
                }
            }
        }
    }

    public void DestoryBarrierLines( )
    {
        foreach(Transform t in barrierlineParent) {
            t.gameObject.SetActive(false);
        }
    }

}
