using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concepts.Core
{
	public class Planet
	{
		private readonly Resource PopulationResource;

		public string Name { get; set; }

		public int Population
		{
			get { return AvailableResources[PopulationResource]; }
			internal set { AvailableResources[PopulationResource] = value; }
		}

		public int PopulationCap { get; set; }

		public decimal GrowthRate { get; set; }

		public ResourceAmountVector AvailableResources { get; internal set; }

		public readonly ConstructionQueue BuildQueue = new ConstructionQueue();

		private readonly List<IProducer> HiddenProducers = new List<IProducer>();

		public readonly PlanetaryInstallationCollection Installations = new PlanetaryInstallationCollection();

		public IEnumerable<IProducer> Producers => HiddenProducers.Concat(Installations.Items);

		// ctor
		public Planet(Rules rules, int population = 0)
		{
			PopulationResource = rules.Resources.PopulationResource;
			AvailableResources = new ResourceAmountVector(rules.Resources);

			// Producers			
			HiddenProducers.Add(new PopulationProducer());
			HiddenProducers.Add(new LaborForceProducer(1000));

			Population = population;
		}

		/// <summary>
		/// Performs production and construction.
		/// </summary>		
		public void Update(Rules rules)
		{
			Produce(rules);
			Construct(rules);
		}

		/// <summary>
		/// Gets a preview of available resources after the next production phase.
		/// </summary>
		public ResourceAmountVector GetProductionPreview(Rules rules)
		{
			return AvailableResources + CalculateProduction(rules);
		}
		
		/// <summary>
		/// Existing planetary installations (and such) produce stuff.
		/// </summary>
		private void Produce(Rules rules)
		{
			AvailableResources += CalculateProduction(rules);
		}

		private ResourceAmountVector CalculateProduction(Rules rules)
		{
			var result = new ResourceAmountVector(rules.Resources);

			foreach (var producer in Producers.OrderBy(p => p.Priority))
			{
				var flow = producer.Produce(rules, this);
				result += flow.NetFlow;
			}

			return result;
		}

		/// <summary>
		/// Build planetary installations and stuff...
		/// </summary>
		private void Construct(Rules rules)
		{
			BuildQueue.Construct(this);
		}
	}
}
