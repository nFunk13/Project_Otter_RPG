using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueRunner runner;
    [SerializeField] private string node;

    private void Start()
    {
        runner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
        runner.onDialogueStart.AddListener(DisableInput);
        runner.onDialogueComplete.AddListener(EnableInput);
    }

    private void OnDestroy()
    {
        runner.onDialogueStart.RemoveListener(DisableInput);
        runner.onDialogueComplete.RemoveListener(EnableInput);
    }

    private void DisableInput() => InputEventManager.Instance.CanInput = false;
    private void EnableInput() => InputEventManager.Instance.CanInput = true;

    public void Play()
    {
        if (!runner.IsDialogueRunning) runner.StartDialogue(node);
    }
}
