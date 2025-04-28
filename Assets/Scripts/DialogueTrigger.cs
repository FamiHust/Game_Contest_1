using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string dialogueText; // Nội dung hội thoại

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueSystem.Instance.ShowDialogue(dialogueText);
        }
    }
}