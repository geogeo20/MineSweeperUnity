using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombCount : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bombCountText;

    public void UpdateBombCount(int count)
    {
        bombCountText.text = count.ToString();
    }
}
