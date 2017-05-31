using System;

namespace Concepts.Core
{
	class FoodBasedPopulationProducer : IProducer
	{
		private Planet _planet;

		public string Name => "PopulationGrowth";

		public int Priority => 1;

		// ctor
		public FoodBasedPopulationProducer(Planet planet)
		{
			_planet = planet;
		}

		public ProductionFlow Produce(Rules rules, ResourceAmountVector availableResources)
		{
			var populationResource = rules.Resources.PopulationResource;
			var foodResource = rules.Resources.FoodResource;

			int pop = availableResources[populationResource];
			int food = availableResources[foodResource];

			double foodToPopRatio = (1000f * food) / pop;

			if (foodToPopRatio < 1f)
			{
				// Not enough food, population dies...
				double mod = (1 - foodToPopRatio) / 10;
				int popLost = (int)(pop * mod);

				ResourceAmountVector vPop = populationResource.Versor * popLost;
				ResourceAmountVector vFood = foodResource.Versor * food;
				return new ProductionFlow(vPop + vFood, 0 * vPop);
			}
			else
			{
				// Good times...
				double mod = (Math.Min(foodToPopRatio, 1.5) - 1.0) / 0.5f;
				decimal effectiveGrowthRate = _planet.GrowthRate * (decimal)mod;
				int growth = (int)(pop * effectiveGrowthRate);

				ResourceAmountVector input = foodResource.Versor * (Math.Min(foodToPopRatio, 1.5) * pop / 1000);
				ResourceAmountVector output = populationResource.Versor * growth;
				return new ProductionFlow(input, output);
			}
		}
	}
}
