using static DAGS.Constants;

namespace DAGS;

public partial class Dags
{
    /// <summary>
    /// Gets a value from the dictionary, or "" if not found.
    /// </summary>
    private string Get(string key)
    {
        if (data.TryGetValue(key, out string? value))
        {
            if (value == null || value == NULL_VALUE)
            {
                value = "";
            }
            return value;
        }
        return "";
    }

    /// <summary>
    /// Sets a value into the dictionary.
    /// </summary>
    private void Set(string key, string value)
    {
        if (value == null || value == NULL_VALUE)
        {
            value = "";
        }
        if (data.ContainsKey(key))
        {
            data[key] = value;
        }
        else
        {
            data.Add(key, value);
        }
    }

    /// <summary>
    /// Gets a value from the dictionary and converts it to an integer.
    /// </summary>
    private int GetInt(string key)
    {
        var value = Get(key);
        try
        {
            return ConvertToInt(value);
        }
        catch (Exception)
        {
            throw new SystemException($"Value is not numeric: [{key}] {value}");
        }
    }

    /// <summary>
    /// Gets a subset of the dictionary where key begins with the prefix. Case sensitive except for "@..." prefixes.
    /// </summary>
    private Dictionary<string, string?> GetByPrefix(string prefix)
    {
        Dictionary<string, string?> result = [];
        List<string> keys;
        keys = data.Keys.Where(x => x.StartsWith(prefix, OIC)).ToList();
        foreach (string k in keys)
        {
            result.Add(k, Get(k));
        }
        return result;
    }
}