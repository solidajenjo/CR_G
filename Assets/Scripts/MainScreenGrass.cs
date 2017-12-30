using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScreenGrass : MonoBehaviour {

    public float speed;
    private Renderer renderer;
    private bool toggler;
    public Text arrow;
    public Canvas menu, credits, loading;
    public GameObject loadingBackground;

    private bool creditsOn;
	// Use this for initialization
	void Start () {
        loadingBackground.SetActive(false);
        loading.gameObject.SetActive(false);
        renderer = GetComponent<Renderer>();
        toggler = false;
        creditsOn = false;
        credits.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        renderer.material.mainTextureOffset = new Vector2(renderer.material.mainTextureOffset.x + 
            Time.deltaTime * speed, renderer.material.mainTextureOffset.x + Time.deltaTime *speed);
        if (Input.GetKeyDown("up") || Input.GetKeyDown("down"))
        {
            toggler = !toggler;
            if (toggler) arrow.rectTransform.anchoredPosition =
                    new Vector2(arrow.rectTransform.anchoredPosition.x, -350);
            else arrow.rectTransform.anchoredPosition =
                    new Vector2(arrow.rectTransform.anchoredPosition.x, -400);
        }
        if (Input.GetKeyDown("return") && !creditsOn)
        {
            if (toggler)
            {
                loadingBackground.SetActive(true);
                menu.gameObject.SetActive(false);
                loading.gameObject.SetActive(true);
                SceneManager.LoadScene("mainGame", LoadSceneMode.Single);
            }            
            else
            {
                menu.gameObject.SetActive(false);
                credits.gameObject.SetActive(true);
                creditsOn = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && creditsOn)
        {
            menu.gameObject.SetActive(true);
            credits.gameObject.SetActive(false);
            creditsOn = false;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !creditsOn)
        {
            Application.Quit();
        }
    }
}
