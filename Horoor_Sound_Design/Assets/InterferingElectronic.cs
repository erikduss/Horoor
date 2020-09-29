using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferingElectronic : MonoBehaviour
{
    [SerializeField] private AudioClip interferingClip;
    [SerializeField] private AudioSource audioPlayer;
    private bool allowInterference = true;

    private void Update()
    {
        if (allowInterference) StartCoroutine(DripWater());
    }

    private IEnumerator DripWater()
    {
        allowInterference = false;
        
        audioPlayer.PlayOneShot(interferingClip);

        int _interferenceDelay = Random.Range(4, 7);
        yield return new WaitForSeconds(_interferenceDelay);
        allowInterference = true;
    }
}
