using PriorityQueue;

PriorityQue queue = new();

queue.AddRule("urgent", 10);
queue.AddRule("normal", 5);

queue.Enqueue("normal");
queue.Enqueue("urgent");

queue.Process();
queue.Process();
queue.Process();