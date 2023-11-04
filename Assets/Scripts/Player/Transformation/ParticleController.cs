using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    
    private void Start()
    {
        _particleSystem = gameObject.GetComponent<ParticleSystem>();
        _particleSystem.Pause();
    }

    public void Begin(int count)
    {
        _particleSystem.Emit(count);
    }
}
