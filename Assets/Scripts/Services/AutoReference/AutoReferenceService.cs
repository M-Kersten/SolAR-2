using System.Reflection;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class AutoReferenceService : MonoBehaviour
{
    void Awake()
    {
        var components = FindObjectsOfType<MonoBehaviour>();
        foreach (var component in components)
        {
            var fields = component.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(AutoReference), true);
                if (attributes.Length > 0)
                {
                    var fieldType = field.FieldType;
                    var foundObject = FindObjectOfType(fieldType);
                    if (foundObject != null)
                    {
                        field.SetValue(component, foundObject);
                    }
                }
            }
        }
    }
}