using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class ShuffleHandler
{
    private Dictionary<Vector3Int, bool> _availableShufflingPosDic = new();


    public Dictionary<Vector3Int, bool> AvailableShufflingPositions => _availableShufflingPosDic;

    public void Add(Vector3Int pos) => _availableShufflingPosDic[pos] = true;

    public void Remove(Vector3Int pos) => _availableShufflingPosDic.Remove(pos);


    public List<Vector3Int> GetShuffledPositions() => _availableShufflingPosDic.Keys.ToList().Shuffle();

    public void Clear() => _availableShufflingPosDic.Clear();
}