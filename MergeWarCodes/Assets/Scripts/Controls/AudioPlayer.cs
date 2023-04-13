using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip mergeSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] AudioClip firstSpawnSound;
    [SerializeField] AudioClip toAttackSound;
    [SerializeField] AudioClip towerHitSound;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMergeSound()
    {
        audioSource.PlayOneShot(mergeSound);
    }

    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }

    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }
    public void PlaySpawnSound()
    {
        audioSource.PlayOneShot(firstSpawnSound);
    }
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(toAttackSound);
    }
    public void PlayTowerSound()
    {
        audioSource.PlayOneShot(towerHitSound);
    }


}
