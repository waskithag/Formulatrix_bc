using System;
using Classes;

Kucing kucing = new();
Hewan hewan = kucing;
Anjing anjing = new();
Hewan hewan1 = anjing;
Hewan hewan2 = new();

hewan.suara();
kucing.suara();

hewan1.suara();
anjing.suara();

hewan2.suara();