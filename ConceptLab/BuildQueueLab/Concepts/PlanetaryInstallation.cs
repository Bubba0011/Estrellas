using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BuildQueueLab.Concepts
{
	class PlanetaryInstallation : IConstructable
	{
		public string Name { get; private set; }

		public int Priority { get; private set; }

		public ResourceAmountVector ConstructionCost { get; private set; }
	
		public readonly ResourceAmountVector OperationCost;

		public readonly ResourceAmountVector ProductionInput;

		public readonly ResourceAmountVector ProductionOutput;

		// ctor
		internal PlanetaryInstallation(XElement xPI, Rules rules)
		{
			Name = (string)xPI.Attribute("name");
			Priority = (int?)xPI.Attribute("prio") ?? 1;

			ConstructionCost = ResourceAmountVector.Initialize(rules.Resources, xPI.XPathSelectElements("Construction/Resource"));
			OperationCost = ResourceAmountVector.Initialize(rules.Resources, xPI.XPathSelectElements("Production/Operation/Resource"));
			ProductionInput = ResourceAmountVector.Initialize(rules.Resources, xPI.XPathSelectElements("Production/Input/Resource"));
			ProductionOutput = ResourceAmountVector.Initialize(rules.Resources, xPI.XPathSelectElements("Production/Output/Resource"));
		}
	}

	class PlanetaryInstallationInstanceStack : IProducer
	{
		public readonly PlanetaryInstallation Installation;

		public int Priority => Installation.Priority;

		public int Count { get; set; }

		// ctor
		internal PlanetaryInstallationInstanceStack(PlanetaryInstallation installation, int initialCount = 0)
		{
			Installation = installation;
			Count = initialCount;
		}

		public ProductionFlow Produce(Rules rules, Planet planet)
		{
			int opCount = planet.AvailableResources / Installation.OperationCost;
			int inCount = planet.AvailableResources / Installation.ProductionInput;
			int count = Math.Min(Math.Min(opCount, inCount), Count);

			ResourceAmountVector input = Installation.ProductionInput * count;
			ResourceAmountVector output = count * Installation.ProductionOutput;

			return new ProductionFlow(input, output);
		}
	}
}
