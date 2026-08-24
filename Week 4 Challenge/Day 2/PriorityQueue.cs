using System;
using System.Collections.Generic;

namespace PriorityQueue;

class QueueNode(string value, int priority)
{
    public string Value { get; } = value;
    public int PriorityValue { get; } = priority;
}

class PriorityQue
{
    private readonly Dictionary<string, int> _rules = new();
    private readonly List<QueueNode> _priorityQueueNodes = new();


    public void AddRule(string keyword, int priority)
    {
        rules[keyword] = priority;
    }
    public void Enqueue(string value)
    {

        int priority = 0;

        if (rules.ContainsKey("val"))
        {
            priority = _rules[val];
        }

        QueueNode newNode = QueueNode(value, priority);

        int i = 0;

        while (i < _priorityQueueNodes.Count && _priorityQueueNodes[index].Priority >= newNode.Priority)
        {
            i++;
        }

        _priorityQueueNodes.Insert(index, newItem);

        Console.WriteLine($"Queued {value} with priority {priority}");
    }

    public void Process()
    {
        if (_priorityQueueNodes.Count == 0)
        {
            Console.WriteLine("No queue to process");
            return;
        }

        QueueNode nodeToProcess = _priorityQueueNodes.First.Value;

        Console.WriteLine($"Processed {nodeToProcess.Value}");

        _priorityQueueNodes.RemoveAt(0);
    }
}