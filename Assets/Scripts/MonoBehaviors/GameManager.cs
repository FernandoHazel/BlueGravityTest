using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int activeEnemies = 0;
    public static int friendlyCells = 0;
    public static bool canWin = false;
    private EnemyRandomGenerator re;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LoosePanel;

    private void Awake() 
    {
        re = GameObject.Find("EnemyRandomGenerator").GetComponent<EnemyRandomGenerator>();
    }
    private void OnEnable() 
    {
        FriendlyCell.lost += Loose;
        Bacteria.Won += Win;
    }
    private void OnDisable() 
    {
        FriendlyCell.lost -= Loose;
        Bacteria.Won -= Win;
    }

    private void Start() 
    {
        StartCoroutine(LetWin());
        LoosePanel.SetActive(false);
        WinPanel.SetActive(false);
    }

    IEnumerator LetWin()
    {
        //Wait after the first wave is generated
        //To let the player win
        yield return new WaitForSeconds(re.generationSeconds+1);
        canWin = true;
    }

    public void Loose()
    {
        LoosePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Win()
    {
        WinPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
