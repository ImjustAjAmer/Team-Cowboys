using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private Coroutine checkForButtons;

    private EventSystem eventSYS;

    private GameObject lastSelected;
    
    private void Awake()
    {
        instance = this;

        eventSYS = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
    }

    void Update()
    {
        if(Gamepad.current != null)
        {
            checkForButtons = StartCoroutine(checkForCurrentlySelected());
        }
        
        if(Gamepad.current == null)
        {
            eventSYS.SetSelectedGameObject(null);
            tryStopCheck();
        }

    }
    
    private void tryStopCheck()
    {
        if(checkForButtons != null)
        {
            StopCoroutine(checkForButtons);
        }
        checkForButtons = null;
    }

    IEnumerator checkForCurrentlySelected()
    {
        WaitForSecondsRealtime wait  = new WaitForSecondsRealtime(0.1f);
        GameObject lastSelected = EventSystem.current.currentSelectedGameObject;
        while(true)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
            else if (lastSelected != EventSystem.current.currentSelectedGameObject && lastSelected != null)
            {
                lastSelected = EventSystem.current.currentSelectedGameObject;
            }
            yield return null;
        }
    }



}
