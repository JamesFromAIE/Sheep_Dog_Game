using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    void Awake() => Instance = this;

    [SerializeField] TextMeshProUGUI _countText;

    void Start()
    {
        UpdateUI();
    }

    public int _sheepCount { get; private set; }
    public int _maxSheep;

    void UpdateUI()
    {
        _countText.text = _sheepCount + "/" + _maxSheep;
    }

    public void IncrementCapturedSheep()
    {
        _sheepCount++;
        UpdateUI();
    }
}
