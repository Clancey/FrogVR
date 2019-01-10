using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HornScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] audioClips;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;
        var clip = audioClips[Random.Range(0, audioClips.Length)];
        var length = clip.length;
        audioSource.PlayOneShot(clip);
    }
}
