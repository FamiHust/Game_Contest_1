using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;

    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public Button okButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialogueBox.SetActive(false);

        okButton.onClick.AddListener(HideDialogue);
    }

    public void ShowDialogue(string text)
    {
        dialogueBox.SetActive(true);
        dialogueText.text = text;
    }

    public void HideDialogue()
    {
        dialogueBox.SetActive(false);
    }
}
