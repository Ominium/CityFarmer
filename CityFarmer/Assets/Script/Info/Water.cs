using UnityEngine;
using System;
public class Water : MonoBehaviour
{
    public int UserSeq { get; set; }
    public int CurrentWater { get; set; }
    public int MaxWater { get; set; }

    public DateTime LastDateWater { get; set; }
}
