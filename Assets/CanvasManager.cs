using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum GameState { MAINMENU, INGAME, GAMEOVER }
public class CanvasManager : MonoBehaviour
{
    private GameState gameState;    

    [Header("MainMenu")]
    [SerializeField] private GameObject uiInGame;
    [SerializeField] private GameObject keyTutorial;
    [SerializeField] private GameObject gameTitle;

    [Header("UIInGame")]
    [SerializeField] private Image heart1;
    [SerializeField] private Image heart2;
    [SerializeField] private Image heart3;

    [SerializeField] private Sprite heartFull;
    [SerializeField] private Sprite heartHalf;
    [SerializeField] private Sprite heartEmpty;

    [SerializeField] private Image damageFeedbackLeft;
    [SerializeField] private Image damageFeedbackRight;
    [SerializeField] private float fadeDuration; // Duración del desvanecimiento

    [SerializeField] private TextMeshProUGUI coinsText; // Duración del desvanecimiento



    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.MAINMENU;
        gameTitle.SetActive(true);
        uiInGame.SetActive(false);
        keyTutorial.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.MAINMENU:
                {
                    if (Input.anyKeyDown)
                    {
                        gameTitle.SetActive(false);
                        uiInGame.SetActive(true);
                        keyTutorial.SetActive(true);

                        gameState = GameState.INGAME;
                    }
                }
                break;
            case GameState.INGAME:
                break;
            case GameState.GAMEOVER:
                break;
            default:
                break;
        }
        
    }

    public void UpdateUICoins(int coins)
    {
        coinsText.text = coins.ToString();
    }
    public void UpdateUILives(float life)
    {
        // Actualiza el primer corazón
        if (life >= 1)
        {
            heart1.sprite = heartFull;
        }
        else if (life >= 0.5)
        {
            heart1.sprite = heartHalf;
        }
        else
        {
            heart1.sprite = heartEmpty;
        }

        // Actualiza el segundo corazón
        if (life >= 2)
        {
            heart2.sprite = heartFull;
        }
        else if (life >= 1.5)
        {
            heart2.sprite = heartHalf;
        }
        else
        {
            heart2.sprite = heartEmpty;
        }

        // Actualiza el tercer corazón
        if (life >= 3)
        {
            heart3.sprite = heartFull;
        }
        else if (life >= 2.5)
        {
            heart3.sprite = heartHalf;
        }
        else
        {
            heart3.sprite = heartEmpty;
        }
    }
    public void HitFeedback()
    {
        StartCoroutine(FadeOutDamageFeedback());
    }

    // Coroutine para desvanecer la imagen de feedback
    private IEnumerator FadeOutDamageFeedback()
    {
        // Asegurarse de que la imagen es completamente visible al inicio
        damageFeedbackLeft.color = new Color(damageFeedbackLeft.color.r, damageFeedbackLeft.color.g, damageFeedbackLeft.color.b, 0.5f);
        damageFeedbackRight.color = new Color(damageFeedbackRight.color.r, damageFeedbackRight.color.g, damageFeedbackRight.color.b, 0.5f);

        // Tiempo total transcurrido
        float timeElapsed = 0f;

        // Mientras el tiempo transcurrido sea menor que la duración del desvanecimiento
        while (timeElapsed < fadeDuration)
        {
            // Aumentar el tiempo transcurrido
            timeElapsed += Time.deltaTime;
            // Calcular la nueva opacidad
            float alpha = Mathf.Clamp01(0.5f - (timeElapsed / fadeDuration));
            // Actualizar la opacidad de la imagen
            damageFeedbackLeft.color = new Color(damageFeedbackLeft.color.r, damageFeedbackLeft.color.g, damageFeedbackLeft .color.b, alpha);
            damageFeedbackRight.color = new Color(damageFeedbackRight.color.r, damageFeedbackRight.color.g, damageFeedbackRight.color.b, alpha);
            // Esperar hasta el próximo frame
            yield return null;
        }

        // Asegurarse de que la imagen es completamente transparente al final
        damageFeedbackLeft.color = new Color(damageFeedbackLeft.color.r, damageFeedbackLeft.color.g, damageFeedbackLeft.color.b, 0f);
        damageFeedbackRight.color = new Color(damageFeedbackRight.color.r, damageFeedbackRight.color.g, damageFeedbackRight.color.b, 0f);
    }
}
