using VIPQueue;

var queue = new VipQueue<string>();


queue.Enqueue("A");
queue.Enqueue("B");
queue.EnqueueVip("C"); 
queue.EnqueueVip("D");
queue.Enqueue("E");

Console.WriteLine();

queue.Process();
queue.Process(); 
queue.Process(); 
queue.Process(); 
queue.Process(); 
