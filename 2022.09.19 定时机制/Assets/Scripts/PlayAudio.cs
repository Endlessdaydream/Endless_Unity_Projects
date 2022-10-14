using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource source;
    public List<AudioClip> clips;
    int index = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            source.clip = clips[index];
            source.Play();

            index++;
            if(index >= clips.Count) { index = 0; }
        }
    }
}
