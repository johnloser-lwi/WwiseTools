using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace WwiseTools.Utils;

public abstract class WaapiListBase<T> : IEnumerable<T>
{
    public abstract string ListContent { get; }
    
    protected List<T> _items;

    public virtual int Count => _items.Count;

    public WaapiListBase()
    {
        _items = new List<T>();
    }
    
    public void Add(T item)
    {
        if (!_items.Contains(item))
            _items.Add(item);
    }

    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    public abstract T Verify(T item);
    
    public virtual void Clear()
    {
        _items.Clear();
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"\nAvailable {ListContent}(s): ");
        foreach (var item in _items)
        {
            builder.AppendLine(item.ToString());
        }

        return builder.ToString();
    }

    public virtual IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}