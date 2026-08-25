using System;
using System.Collections.Generic;

namespace PriorityQueue;

public class QueueNode(string value, int priority)
{
    public string Value { get; } = value;
    public int Priority { get; } = priority;
}

public class PriorityQue
{
    private readonly Dictionary<string, int> _rules = new();
    private readonly List<QueueNode> _priorityQueueNodes = new();

    public void AddRule(string keyword, int priority)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(keyword);

        _rules[keyword] = priority;
    }

    public void Enqueue(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        int priority = GetPriority(value);
        QueueNode newNode = new(value, priority);

        // Insert after existing nodes with the same priority.
        // This preserves FIFO ordering for equal-priority items.
        int index = 0;

        while (index < _priorityQueueNodes.Count &&
               _priorityQueueNodes[index].Priority >= newNode.Priority)
        {
            index++;
        }

        _priorityQueueNodes.Insert(index, newNode);

        Console.WriteLine($"Queued {value} with priority {priority}");
    }

    public void Process()
    {
        if (_priorityQueueNodes.Count == 0)
        {
            Console.WriteLine("No queue to process");
            return;
        }

        QueueNode nodeToProcess = _priorityQueueNodes[0];

        Console.WriteLine($"Processed {nodeToProcess.Value}");

        _priorityQueueNodes.RemoveAt(0);
    }

    private int GetPriority(string value)
    {
        int priority = 0;

        foreach (KeyValuePair<string, int> rule in _rules)
        {
            if (value.Contains(rule.Key, StringComparison.OrdinalIgnoreCase))
            {
                priority = Math.Max(priority, rule.Value);
            }
        }

        return priority;
    }
}