﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts.Core
{
	public interface IProducer
	{
		string Name { get; }

		int Priority { get; }

		ProductionFlow Produce(Rules rules, ResourceAmountVector availableResources);
	}
}
