﻿using System.Threading.Tasks;

namespace JackHenry.Console.Reddit.Interfaces;

internal interface ISubRedditMonitor
{
	Task MonitorAsync();
}