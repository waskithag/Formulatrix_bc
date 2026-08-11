using System;
using System.Collections.Generic;

namespace VIPQueue;
public class VipQueue<T>
{
    private readonly LinkedList<T> _items = new();

    private LinkedListNode<T>? _lastVip;

    public void Enqueue(T value)
    {
        _items.AddLast(value);
        Console.WriteLine($"Queued {value}");
    }

    public void EnqueueVip(T value)
    {
        LinkedListNode<T> node;

        if (_lastVip == null)
        {
            node = _items.AddFirst(value);
        }
        else
        {
            node = _items.AddAfter(_lastVip, value);
        }

        _lastVip = node;
        Console.WriteLine($"VIP Queued {value}");
    }

    public void Process()
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("Queue is empty");
            return;
        }

        var first = _items.First;
        _items.RemoveFirst();

        if (first == _lastVip)
        {
            _lastVip = null;
        }

        Console.WriteLine($"Processed {first.Value}");
    }
}
