using System.Collections.Generic;
using UnityEngine;

public static class CheckpointManager
{
    private static HashSet<string> collectedIds = new HashSet<string>();

    public static bool HasCheckpoint { get; private set; }
    public static Vector3 Position { get; private set; }
    public static int Coins { get; private set; }
    public static IReadOnlyCollection<string> CollectedIds => collectedIds;

    public static void SetCheckpoint(Vector3 position, int coins, IEnumerable<string> ids)
    {
        HasCheckpoint = true;
        Position = position;
        Coins = coins;
        collectedIds = new HashSet<string>(ids);
    }

    public static bool IsCollected(string id)
    {
        return collectedIds.Contains(id);
    }

    public static void Reset()
    {
        HasCheckpoint = false;
        Coins = 0;
        collectedIds.Clear();
    }
}
