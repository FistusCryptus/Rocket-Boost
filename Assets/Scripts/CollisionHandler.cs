using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float crashResetDelay = 0;
    [SerializeField] float nextLevelDelay = 0;
    [SerializeField] AudioClip bumpSFX;
    [SerializeField] AudioClip finishSFX;
    [SerializeField] ParticleSystem finishReachParticles;
    [SerializeField] ParticleSystem BumpParticles;
    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;
    bool isContollable = true;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {

        if (!isContollable) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("friend");
                break;
            case "Finish":
                StartFinishSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void removeControll()
    {
        mainBoosterParticles.Stop();
        leftBoosterParticles.Stop();
        rightBoosterParticles.Stop();
        audioSource.Stop();
        isContollable = false;
        GetComponent<Movement>().enabled = false;
    }

    private void StartFinishSequence()
    {
        removeControll();
        finishReachParticles.Play();
        audioSource.PlayOneShot(finishSFX);
        Invoke("NextLevel", nextLevelDelay);
    }

    private void StartCrashSequence()
    {
        removeControll();
        BumpParticles.Play();
        audioSource.PlayOneShot(bumpSFX);
        Invoke("ReloadLevel", crashResetDelay);
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void NextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }
}
