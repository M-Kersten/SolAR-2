using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = System.Random;

public class RandomService
{
    readonly Random random;
    private const string randomCharacters = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public RandomService() => random = new Random();
    public RandomService(int seed)
    {
        random = new Random(seed);
    }

    public virtual int Int() => random.Next();
    public virtual int Int(int maxValue) => random.Next(maxValue);
    public virtual int Int(int minValue, int maxValue) => random.Next(minValue, maxValue);
    public virtual bool Bool(float chance) => Float() < chance;
    public virtual float Float() => (float)random.NextDouble();
    public virtual float Float(float minValue, float maxValue) => minValue + (maxValue - minValue) * Float();
    public virtual T Element<T>(IList<T> elements) => elements[Int(0, elements.Count)];
    public virtual int Sign() => random.Next(0, 2) == 0 ? -1 : 1;
    public virtual Vector2 Vector2(Vector2 min, Vector2 max) => new Vector2(Float(min.x, max.x), Float(min.y, max.y));
    public virtual Vector3 Vector3(Vector3 min, Vector3 max) => new Vector3(Float(min.x, max.x), Float(min.y, max.y), Float(min.z, max.z));
    public virtual string String(int length = 10)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < length; ++i)
            sb.Append(randomCharacters[Int(0, randomCharacters.Length)]);

        return sb.ToString();
    }
}
