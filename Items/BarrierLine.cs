using UnityEngine;
using System.Collections;

public class BarrierLine : MonoBehaviour
{
    private new ParticleSystem particleSystem
    {
        get {
            return GetComponent<ParticleSystem>( );
        }
    }

    public Vector3 begin;
    public Vector3 end;
    public float interval = 0.24f;
    public float offset = 0f;
    public float particleSize = 0.08f;
    private ParticleSystem.Particle[ ] particles;

    void Awake( )
    {
        var emission = particleSystem.emission;
        emission.enabled = false;
        var shape = particleSystem.shape;
        shape.enabled = false;

        particleSystem.startLifetime = float.MaxValue;
        particleSystem.startSpeed = 0;
        particleSystem.startSize = particleSize;
        particleSystem.playOnAwake = false;
        particleSystem.maxParticles = 100;
        particleSystem.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    public void Show( Vector3 begin, Vector3 end )
    {
        float distance = Vector3.Distance(begin, end);
        int count = (int)((distance + interval * 0.5f) / interval);

        int particleCount = particleSystem.particleCount;
        if(count > particleCount) {
            particleSystem.Emit(count - particleCount);
        }

        if(particles == null || particles.Length < particleSystem.maxParticles) {
            particles = new ParticleSystem.Particle[particleSystem.maxParticles];
        }

        particleCount = particleSystem.GetParticles(particles);

        Vector3 pos = begin;
        Vector3 dir = (end - begin).normalized;
        Vector3 increment = dir * interval;
        //offset %= interval;
        //if(offset < 0) {
        //    offset += interval;
        //}
        //pos += dir * offset;

        for(int i = 0; i < particleCount; ++i) {
            if(i < count) {
                particles[i].position = pos;
                particles[i].velocity = Vector3.zero;
                particles[i].startSize = particleSize;
            }
            else {
                particles[i].startLifetime = 0;
                particles[i].lifetime = 0;
            }

            pos += increment;
        }

        // Apply the particle changes to the particle system
        particleSystem.SetParticles(particles, particleCount);
    }

    public void Destory( ) {
        gameObject.SetActive(false);
    }


}
