using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    Inventory playersInventory;

    [SerializeField] GameObject buttonObj;
    [SerializeField] GameObject buttonContainer;

    private void Awake()
    {
        playersInventory = GameObject.Find("OverworldPlayer").GetComponent<Inventory>();

        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        //AddButtons();
    }

    public void ChangeDisplayMode(bool truthValue)
    {
        this.gameObject.SetActive(truthValue);
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
