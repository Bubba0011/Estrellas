using System;

namespace Concepts.Core
{
	public class PlanetaryInstallationStack : IProducer
	{
		public readonly PlanetaryInstallation Installation;

		public int Priority => Installation.Priority;

		public int Count { get; set; }

		// ctor
		internal PlanetaryInstallationStack(PlanetaryInstallation installation, int initialCount = 0)
		{
			Installation = installation;
			Count = initialCount;
		}

		public ProductionFlow Produce(Rules rules, ResourceAmountVector availableResources)
		{
			int opCount = availableResources / Installation.OperationCost;
			int inCount = availableResources / Installation.ProductionInput;
			int count = Math.Min(Math.Min(opCount, inCount), Count);

			ResourceAmountVector input = Installation.ProductionInput * count;
			ResourceAmountVector output = count * Installation.ProductionOutput;

			return new ProductionFlow(input, output);
		}
	}
}
