﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildQueueLab.Concepts
{
	interface IProducer
	{
		int Priority { get; }

		ProductionFlow Produce(Rules rules, Planet planet);
	}
}
