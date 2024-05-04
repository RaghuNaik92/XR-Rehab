using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorWithActivation : MonoBehaviour
{
    public GameObject gameObject1; // GameObject containing the button (can be a parent object)
    public GameObject gameObject2; // GameObject containing the button
    public GameObject gameObject3; // GameObject containing the button

    public GameObject objectToActivate; // GameObject to activate when the correct button is pressed

    private Coroutine changeColorCoroutine;
    private GameObject currentlyGreenObject; // Track the currently green GameObject

    private void Start()
    {
        // Start the coroutine to change color every 3 seconds
        changeColorCoroutine = StartCoroutine(ChangeColorRoutine());

        // Add click listeners to the buttons in each GameObject
        AddButtonListeners();
    }

    private void AddButtonListeners()
    {
        // Add the click event listener for each GameObject's button (searching in children)
        AddButtonListener(FindButtonInChildren(gameObject1));
        AddButtonListener(FindButtonInChildren(gameObject2));
        AddButtonListener(FindButtonInChildren(gameObject3));
    }

    private Button FindButtonInChildren(GameObject obj)
    {
        // Find the Button component in the GameObject or its children
        Button button = obj.GetComponentInChildren<Button>();

        if (button == null)
        {
            Debug.LogWarning($"GameObject '{obj.name}' does not have a Button component or it's not in the children.");
        }

        return button;
    }

    private void AddButtonListener(Button button)
    {
        if (button != null)
        {
            button.onClick.AddListener(() => OnButtonClick(button.gameObject)); // Add the click event
        }
    }

    private void OnButtonClick(GameObject clickedObject)
    {
        // If the clicked GameObject is the current green object, activate the target GameObject
        if (clickedObject == currentlyGreenObject)
        {
            if (objectToActivate != null)
            {
                objectToActivate.SetActive(true);
            }
        }
    }

    private IEnumerator ChangeColorRoutine()
    {
        while (true) // Infinite loop
        {
            // Wait for 5 seconds
            yield return new WaitForSeconds(3f);

            // Randomly select a new GameObject to turn green
            int randomIndex = Random.Range(0, 3);

            GameObject selectedObject = null;

            switch (randomIndex)
            {
                case 0:
                    selectedObject = gameObject1;
                    break;
                case 1:
                    selectedObject = gameObject2;
                    break;
                case 2:
                    selectedObject = gameObject3;
                    break;
            }

            if (selectedObject != null && selectedObject != currentlyGreenObject)
            {
                // If there's a currently green object, set it back to black
                if (currentlyGreenObject != null)
                {
                    ChangeColor(currentlyGreenObject, Color.black);
                }

                // Change the color of the new object to green
                ChangeColor(selectedObject, Color.green);

                // Update the reference to the current green object
                currentlyGreenObject = selectedObject;
            }
        }
    }

    private void ChangeColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = color;
        }
        else
        {
            Debug.LogWarning($"GameObject '{obj.name}' does not have a Renderer.");
        }
    }

    private void OnDestroy()
    {
        if (changeColorCoroutine != null)
        {
            StopCoroutine(changeColorCoroutine);
        }
    }
}
