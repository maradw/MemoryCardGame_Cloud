using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

/*public class GameManager143 : MonoBehaviour
{
    public Sprite backcardSprite;
    private bool firstGuess, secondGuess;
    public CardInfoSO[] cardPool;
    public GameObject card;
    public GameObject cardField;
    public GameObject winPanel;
    private List<GameObject> cards = new List<GameObject>();
    private List<Button> buttons = new List<Button>();

    private int index;
    private Card firstchoise;
    private Card secondchoise;
    private bool evaluating;

    private int matches;
    private int totalMatches;
    //added
    private int fallos; // Número de fallos
    private int gameId; // ID único de la partida

    public AudioSource mainAudioSource;
    public AudioClip audioSourceone;
    public AudioClip audioSourcetwo;
    public AudioClip goodAudio;
    public AudioClip wrongAudio;

    void Start()
    {
        // Genera un ID único para la partida
        gameId = UnityEngine.Random.Range(1000, 9999);

        totalMatches = cardPool.Length;
        fallos = 0;

        // Crear las cartas
        for (int i = 0; i < cardPool.Length; i++)
        {
            for (int l = 0; l < 2; l++)
            {
                GameObject go = Instantiate(card, cardField.transform, false);
                go.GetComponent<Card>().Initialize(cardPool[i].index, cardPool[i].sprite, backcardSprite);
                go.gameObject.name = i.ToString();
                cards.Add(go);
            }
        }

        // Mezclar las cartas
        ShuffleCards();

        // Asignar botones
        foreach (var cardObj in cards)
        {
            Button btn = cardObj.gameObject.GetComponent<Button>();
            buttons.Add(btn);
        }

        AddListeners();
    }

    void ShuffleCards()
    {
        List<GameObject> cardscopy = new List<GameObject>(cards);
        List<GameObject> displaycards = new List<GameObject>();

        while (cardscopy.Count > 0)
        {
            int x = UnityEngine.Random.Range(0, cardscopy.Count);
            displaycards.Add(cardscopy[x]);
            cardscopy.RemoveAt(x);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i] = displaycards[i];
            cards[i].transform.SetSiblingIndex(i);
        }
    }

    void AddListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PickACard());
        }
    }

    public void PickACard()
    {
        if (evaluating)
        {
            return;
        }

        if (index < 2)
        {
            index++;
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>().Flip();
            if (!firstGuess)
            {
                firstGuess = true;
                firstchoise = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>();
                mainAudioSource.PlayOneShot(audioSourceone);
            }
            else if (!secondGuess)
            {
                secondGuess = true;
                secondchoise = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>();
                mainAudioSource.PlayOneShot(audioSourcetwo);
            }
        }

        if (index == 2)
        {
            evaluating = true;
            StartCoroutine(EvaluateCards());
        }
    }

    private IEnumerator EvaluateCards()
    {
        yield return new WaitForSeconds(1.5f);
        firstGuess = secondGuess = false;

        if (firstchoise.Index() == secondchoise.Index() && firstchoise.GetInstanceID() != secondchoise.GetInstanceID())
        {
           // if (!firstchoise.IsPaired() && !secondchoise.IsPaired())
            {
                firstchoise.SetPair();
                secondchoise.SetPair();
                matches++;
                firstchoise.btn.interactable = false;
                secondchoise.btn.interactable = false;
                mainAudioSource.PlayOneShot(goodAudio);
            }
        }
        else
        {
            fallos++; // Incrementa el contador de fallos
            mainAudioSource.PlayOneShot(wrongAudio);
            firstchoise.Flip();
            secondchoise.Flip();
        }

        index = 0;
        evaluating = false;

        if (matches == totalMatches)
        {
            print("Win");
            winPanel.gameObject.SetActive(true);

            // Calcular el tiempo total y enviar datos
            float totalTiempo = Time.timeSinceLevelLoad;
            StartCoroutine(SendGameData(gameId, totalTiempo, fallos));
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public IEnumerator SendGameData(int gameId, float tiempo, int fallos)
    {
        string url = "http://localhost/save_card_game_data.php"; // Cambia a la URL de tu servidor PHP

        WWWForm form = new WWWForm();
        form.AddField("game_id", gameId);            // ID de la partida
        form.AddField("tiempo", tiempo.ToString()); // Tiempo total en segundos
        form.AddField("fallos", fallos);            // Número de fallos

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Game data uploaded successfully: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to upload game data: " + www.error);
            }
        }
    }
   

}
*/