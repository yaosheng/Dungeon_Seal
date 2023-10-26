using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    public Point point;
    public Floor currentFloor;
    public string pointString;
    public ParticleSystem brokenParticle;
    public SoundManager soundManager
    {
        get {
            return GameObject.FindObjectOfType<SoundManager>( ) as SoundManager;
        }
    }

    void Start( )
    {
        pointString = point.ToString( );
    }

    public void Destory( )
    {
        currentFloor.obstacle = null;
        BreakParticle( );
        soundManager.PlayBrokenSound( );
        this.gameObject.SetActive(false);
    }

    void BreakParticle( )
    {
        ParticleSystem rb = Instantiate(brokenParticle) as ParticleSystem;
        rb.transform.position = transform.position;
    }
}
