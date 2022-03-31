using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusFloor : MonoBehaviour
{
    public bool isChecked = false;
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;


    private void OnTriggerEnter(Collider other)
    {
        Block block = other.GetComponent<Block>();
        if (block && !block.isBonus && other.CompareTag("Block"))
        {
            isChecked = true;

            block.isBonus = true;

            particles[0].Play(true);
            particles[1].Play(true);

            audioSource.PlayOneShot(clip);
        }
    }
}
