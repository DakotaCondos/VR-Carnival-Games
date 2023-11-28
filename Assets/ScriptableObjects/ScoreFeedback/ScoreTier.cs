using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ScoreTier", menuName = "ScoreFeedback/ScoreTier", order = 1)]
public class ScoreTier : ScriptableObject
{
    public int minimumScore;
    public List<string> messages = new List<string>();
}
