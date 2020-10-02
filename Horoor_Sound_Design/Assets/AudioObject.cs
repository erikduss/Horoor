using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    [SerializeField] private List<AudioClip> objectAudioClip;
    [SerializeField] private bool useDelay;
    [SerializeField] private int minDelay;
    [SerializeField] private int maxDelay;
    [SerializeField] private float clipVolume;

    private AudioSource objectAudioPlayer;
    private bool allowAudio = true;

    private void Start()
    {
        objectAudioPlayer = GetComponent<AudioSource>();
        objectAudioPlayer.volume = clipVolume;
        if (!useDelay)
        {
            objectAudioPlayer.loop = true;
            objectAudioPlayer.clip = objectAudioClip[0];
            objectAudioPlayer.Play();
        }
    }

    private void Update()
    {
        if (allowAudio && useDelay) StartCoroutine(PlayDelayedSound());
    }

    private IEnumerator PlayDelayedSound()
    {
        allowAudio = false;

        int _randNumber = Random.Range(0, objectAudioClip.Count);
        AudioClip _currentClip = objectAudioClip[_randNumber];

        objectAudioPlayer.PlayOneShot(_currentClip);

        int _interferenceDelay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(_interferenceDelay);
        allowAudio = true;
    }
}
