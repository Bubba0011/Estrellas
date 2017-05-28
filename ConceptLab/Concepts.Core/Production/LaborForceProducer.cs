using Concepts.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts.Core
{
	// TODO: Replace with scriptable version
	class LaborForceProducer : IProducer
	{
		private int _factor;

		public string Name => "Labor Force";

		public int Priority => 1;

		public LaborForceProducer(int factor = 1000)
		{
			_factor = factor;
		}

		public ProductionFlow Produce(Rules rules, ResourceAmountVector availableResources)
		{
			var populationResource = rules.Resources.PopulationResource;
			var laborResource = rules.Resources.GetResource("Labor");

			int laborUnits = availableResources[populationResource] / _factor;

			ResourceAmountVector output = laborResource.Versor * laborUnits;

			return new ProductionFlow(output);
		}
	}
}
