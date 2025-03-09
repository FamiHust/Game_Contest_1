using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepClip;

    void PlayFootstep()
    {
        audioSource.PlayOneShot(footstepClip);
    }
}
