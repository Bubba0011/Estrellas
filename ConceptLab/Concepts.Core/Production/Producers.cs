using Concepts.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts.Core
{
	class PopulationProducer : IProducer
	{
		public int Priority => 0;

		public ProductionFlow Produce(Rules rules, Planet planet)
		{
			var populationResource = rules.Resources.PopulationResource;

			int pop = planet.AvailableResources[populationResource];
			double capPct = (double)pop / planet.PopulationCap;
			double penalty = capPct > 0.25 ? Math.Pow(capPct - 0.25, 0.75) / 0.7 : 0;
			decimal effectiveGrowthRate = planet.GrowthRate * (1 - (decimal)penalty);

			int growth = (int)(pop * effectiveGrowthRate);

			ResourceAmountVector output = populationResource.Versor * growth;
			return new ProductionFlow(output);
		}
	}

	// TODO: Replace with scriptable version
	class LaborForceProducer : IProducer
	{
		private int _factor;

		public int Priority => 1;

		public LaborForceProducer(int factor = 1000)
		{
			_factor = factor;
		}

		public ProductionFlow Produce(Rules rules, Planet planet)
		{
			var laborResource = rules.Resources.GetResource("Labor");
			int laborUnits = planet.Population / _factor;

			ResourceAmountVector output = laborResource.Versor * laborUnits;

			return new ProductionFlow(output);
		}
	}
}
