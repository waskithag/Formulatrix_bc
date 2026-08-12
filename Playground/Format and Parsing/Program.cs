using System;

bool result = TimeSpan.TryParse("invalid", out TimeSpan outTimespan);
Console.WriteLine($"bool value: {result} | value : {outTimespan}");

bool result1 = DateTime.TryParse("invalid", out DateTime outDate);
Console.WriteLine($"bool value: {result1} | value : {outDate}");

bool result2 = Guid.TryParse("invalid", out Guid outGuid);
Console.WriteLine($"bool value: {result2} | value : {outGuid}");

