using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [System.Serializable]
    public class Menu
    {
        public string menuName;
        public GameObject menuRoot;
        public bool canBeClosed;
    }

    public Menu[] menus;
    private Stack<Menu> menuStack = new Stack<Menu>();

    void Start()
    {
        OpenMenu("Main");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuStack.Count > 1)
            {
                CloseMenu();
            }
            else if (menuStack.Count == 1 && menuStack.Peek().canBeClosed)
            {
                CloseMenu();
            }
        }
    }

    public void OpenMenu(string menuName)
    {
        Menu targetMenu = FindMenu(menuName);

        if (targetMenu != null)
        {
            if (menuStack.Count > 0)
                menuStack.Peek().menuRoot.SetActive(false);

            targetMenu.menuRoot.SetActive(true);
            menuStack.Push(targetMenu);
        }
        else
        {
            Debug.LogWarning("Menu introuvable : " + menuName);
        }
    }

    public void CloseMenu()
    {
        if (menuStack.Count > 0)
        {
            Menu currentMenu = menuStack.Pop();
            currentMenu.menuRoot.SetActive(false);

            if (menuStack.Count > 0)
                menuStack.Peek().menuRoot.SetActive(true);
        }
    }

    public void OnStartGame()
    {
        LevelManager.Instance.FadeToNextLevel(); 
    }

    public void OnCredits()
    {
        OpenMenu("Credits");
    }

    public void OnQuitGame()
    {
        // close the game on build version.
        // it's normal if it do nothing in the editor
        // don't worry ;)
        Application.Quit();
    }

    private Menu FindMenu(string menuName)
    {
        foreach (var m in menus)
        {
            if (m.menuName == menuName)
                return m;
        }
        return null;
    }
}
