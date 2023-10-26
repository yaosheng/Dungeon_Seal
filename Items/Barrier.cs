using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour {

    public Floor currentFloor;
    public Point point;
    public ParticleSystem brokenParticle;

    public void Destory( )
    {
        DeadParticle( );
        currentFloor.barrier = null;
        gameObject.SetActive(false);
    }

    public void DeadParticle( )
    {
        ParticleSystem pc = Instantiate(brokenParticle) as ParticleSystem;
        pc.transform.position = transform.position;
    }
}
