using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

    Rigidbody rb;
    private AudioSource[] audioSources;
    int mainEngineSource = 0;
    int sideEnginesSource = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSources = GetComponents<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            ThrustUp();
        }
        else
        {
            TurnOfMainEngine();
        }

    }

    private void ProcessRotation()
    {
        if (rotation.IsPressed())
        {
            ThrustToSide();
        }
        else
        {
            TurnOffSideEngines();
        }
    }

    private void TurnOfMainEngine()
    {
        audioSources[mainEngineSource].Stop();
        mainBoosterParticles.Stop();
    }

    private void TurnOffSideEngines()
    {
        audioSources[sideEnginesSource].Stop();
        leftBoosterParticles.Stop();
        rightBoosterParticles.Stop();
    }

    private void ThrustUp()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        EnableAudio(mainEngineSource);
        EnableParticles(mainBoosterParticles);
    }

    private void ThrustToSide()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput > 0)
        {
            RotateRight();
        }

        else if (rotationInput < 0)
        {
            RotateLeft();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(rotationSpeed);
        EnableAudio(sideEnginesSource);
        EnableParticles(leftBoosterParticles);
    }

    private void RotateLeft()
    {
        ApplyRotation(-rotationSpeed);
        EnableAudio(sideEnginesSource);
        EnableParticles(rightBoosterParticles);
    }

    private void ApplyRotation(float rotationStrength) {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationStrength * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }

    private void EnableParticles(ParticleSystem particleSystem)
    {
        if(!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }
    }

    private void EnableAudio(int sourceNumber)
    {
        if(!audioSources[sourceNumber].isPlaying)
        {
            audioSources[sourceNumber].PlayOneShot(mainEngine);
        }
    }
}
