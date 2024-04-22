using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private bool isMouseOverPickUp = false;
    private BoxCollider2D mouseCollider;

    private PlayerUI playerUI;
    private void Start()
    {
        mouseCollider = GetComponent<BoxCollider2D>();
        playerUI = FindObjectOfType<PlayerUI>();
    }

    private void Update()
    {
        if (isPlayerInRange && isMouseOverPickUp && Input.GetKeyDown(KeyCode.F))
        {
            EnterPortal();
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        isMouseOverPickUp = mouseCollider.OverlapPoint((Vector2)ray.origin);

        if (isMouseOverPickUp && isPlayerInRange)
        {

            playerUI.ShowPortButton();

        }
        else
        {
            playerUI.HidePortButton();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerUI.HidePortButton();
        }
    }

    private void EnterPortal()
    {
        playerScript player = FindObjectOfType<playerScript>();

        if (player != null)
        {
            NextFloor();
            Destroy(gameObject);
        }

    }
    private void NextFloor()
    { 

        MapGeneration mapGenerator = FindObjectOfType<MapGeneration>();
        if (mapGenerator != null)
        {
            mapGenerator.floorCount++;
            ClearScene();
            mapGenerator.GenerateMap();
        }
        
    }

    private void ClearScene()
    {
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        List<string> excludeNames = new List<string>();

        excludeNames.Add(Camera.main.gameObject.name);
        excludeNames.Add("Player");
        excludeNames.Add(FindObjectOfType<MapGeneration>().gameObject.name);
        excludeNames.Add("Grid");
        excludeNames.Add("ItemList");
        excludeNames.Add("UIDocument");

        foreach (GameObject obj in allObjects)
        {
            if (!excludeNames.Contains(obj.name))
            {
                Destroy(obj);
            }
        }
    }
}
