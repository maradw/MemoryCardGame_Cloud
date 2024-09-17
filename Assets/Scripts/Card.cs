using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    public Image img;
    public Button btn;
    public Sprite frontS;
    public Sprite backS;

    [SerializeField] private int _index;
    [SerializeField] private bool isFlipped;
    [SerializeField] private bool isPaired;

    public void Initialize(int index, Sprite front, Sprite back)
    {
        _index = index;
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
        frontS = front;
        backS = back;
    }

    public void Flip ()
    {
        isFlipped = !isFlipped;
        img.sprite = isFlipped ? frontS : backS;
    }
    public int Index()
    {
        return _index;
    }
    public void SetPair()
    {
        isPaired = true;
    }
    public bool IsPared()
    {
        return isPaired;
    }

}
