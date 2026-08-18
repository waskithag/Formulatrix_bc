using PriorityQueue;

PriorityQue queue = new();

queue.Enqueue("A", 1);
queue.Enqueue("C", 5);
queue.Enqueue("B", 5);

queue.Process();
queue.Process();
queue.Process();
queue.Process();