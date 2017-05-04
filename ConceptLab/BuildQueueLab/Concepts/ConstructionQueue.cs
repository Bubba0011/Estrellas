using BuildQueueLab.Concepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildQueueLab.Concepts
{
	class ConstructionQueue
	{
		public void Construct(Planet planet)
		{
			// TODO...
		}
	}

	class ConstructionOrder
	{
		public readonly IConstructable Construction;

		public readonly int Count;

		// ctor
		public ConstructionOrder(IConstructable what, int howMany)
		{
			Construction = what;
			Count = howMany;
		}
	}
}
