using System;
using CircQueue;

CircularQueue<int> circQueue = new(3);

circQueue.Log(1); 
circQueue.Log(2); 
circQueue.Log(3); 
circQueue.Log(4); 
circQueue.Log(5); 
circQueue.Log(6); 
circQueue.Read();
circQueue.Read();
circQueue.Read();
circQueue.Read();