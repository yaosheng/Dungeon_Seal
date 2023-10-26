using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioClip brokenSound;
    public AudioClip coinSound;
    private AudioSource audioSource
    {
        get {
            return GetComponent<AudioSource>( );
        }
    }

    public void PlayBrokenSound( )
    {
        audioSource.PlayOneShot(brokenSound);
    }

    public void PlayCoinSound( )
    {
        audioSource.PlayOneShot(coinSound);
    }
}
