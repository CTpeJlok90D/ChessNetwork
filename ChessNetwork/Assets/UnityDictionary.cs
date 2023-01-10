using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct UnityDictionarity<Key, Value>
{
    [SerializeField] private List<Key> _keys;
    [SerializeField] private List<Value> _values;

    private Dictionary<Key, Value> _dictionary;

    public UnityDictionarity<Key, Value> Init()
    {
        _dictionary = new();
        if (_keys.Count != _values.Count)
        {
            Debug.LogWarning("Cannot convert to dictiorary: keys and values have not the same count");
            return this;
        }
        for (int i = 0; i < _keys.Count; i++)
        {
            _dictionary.Add(_keys[i], _values[i]);
        }
        return this;
    }

    public Value this[Key index]
    {
        get
        {
            return _dictionary[index];
        }
        set
        {
            _dictionary[index] = value;
        }
    }

    public void Add(Key key, Value value)
    {
        _keys.Add(key);
        _values.Add(value);
        _dictionary.Add(key, value);
    }

    public void Remove(Key key) 
    {
        _values.RemoveAt(_keys.IndexOf(key));
        _keys.Remove(key);
        _dictionary.Remove(key);
    }
}