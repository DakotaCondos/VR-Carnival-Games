using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoreFeedback : MonoBehaviour
{
    [SerializeField] private List<ScoreTier> scoreTiers;
    private List<ScoreTier> sortedTiers;

    void Start()
    {
        sortedTiers = scoreTiers.OrderByDescending(t => t.minimumScore).ToList();
    }

    public string ProvideFeedback(int score)
    {
        var tier = sortedTiers.FirstOrDefault(t => score >= t.minimumScore);

        if (tier != null && tier.messages.Count > 0)
        {
            return tier.messages[Random.Range(0, tier.messages.Count)];
        }

        return "Try again to score some points!";
    }
}
