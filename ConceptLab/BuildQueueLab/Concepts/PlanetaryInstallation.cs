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

		public void Deploy(Planet planet, int count)
		{
			planet.Installations.Add(this, count);
		}
	}
}
