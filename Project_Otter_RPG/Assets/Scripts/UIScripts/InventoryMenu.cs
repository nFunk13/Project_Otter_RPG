using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    Inventory playersInventory;

    [SerializeField] GameObject buttonObj;
    [SerializeField] GameObject buttonContainer;
    private bool canSee = false;

    private void Awake()
    {
        playersInventory = GameObject.Find("OverworldPlayer").GetComponent<Inventory>();

        this.gameObject.SetActive(canSee);
    }

    public void ChangeDisplayMode()
    {
        canSee = !canSee;
        this.gameObject.SetActive(canSee);
    }

    private void AddButtons()
    {
        foreach (var item in playersInventory.GetInventory().Values)
        {
            GameObject newButton = Instantiate(buttonObj, buttonContainer.transform);
            newButton.transform.SetParent(buttonContainer.transform);
        }
    }

    private void OnEnable()
    {
        AddButtons();
    }

    private void OnDisable()
    {
        foreach (Transform child in buttonContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
