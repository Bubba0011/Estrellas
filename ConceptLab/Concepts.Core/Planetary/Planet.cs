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

		public readonly List<District> Districts = new List<District>();

		// ctor
		public Planet(Rules rules, int population = 0)
		{
			PopulationResource = rules.Resources.PopulationResource;
			AvailableResources = new ResourceAmountVector(rules.Resources);

			// Producers
			HiddenProducers.Add(new FoodBasedPopulationProducer(this));
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
			Decay(rules);
		}

		/// <summary>
		/// Gets a preview of available resources after the next production phase.
		/// </summary>
		public Projection GetProductionPreview(Rules rules)
		{
			// "Perform" production
			ProductionReport productionReport = CalculateProduction(rules, AvailableResources);
			ResourceAmountVector tempAvailable = AvailableResources + productionReport.TotalFlow.NetFlow;

			// "Perform" construction
			ConstructionReport constructionReport = CalculateConstruction(rules, tempAvailable);
			ResourceAmountVector newAvailable = tempAvailable - constructionReport.ConsumedResources;

			// Handle waste
			ResourceAmountVector waste = CalculateDecay(rules, newAvailable);
			newAvailable -= waste;

			var projection = new Projection()
			{
				Produced = productionReport.TotalFlow.Output,
				Consumed = productionReport.TotalFlow.Input + constructionReport.ConsumedResources,
				Available = newAvailable,
				Wasted = waste,
			};
			
			return projection;
		}
		
		/// <summary>
		/// Existing planetary installations (and such) produce stuff.
		/// </summary>
		private void Produce(Rules rules)
		{
			AvailableResources += CalculateProduction(rules, AvailableResources).TotalFlow.NetFlow;
		}

		private ProductionReport CalculateProduction(Rules rules, ResourceAmountVector availableResources)
		{
			var report = new ProductionReport();
						
			foreach (var producer in Producers.OrderBy(p => p.Priority))
			{
				var reportItem = new ProductionReportItem()
				{
					ProducerName = producer.Name,
					Flow = producer.Produce(rules, availableResources),
				};
				
				report.AddItem(reportItem);
				availableResources += reportItem.Flow.NetFlow;
			}

			return report;
		}

		/// <summary>
		/// Build planetary installations and stuff...
		/// </summary>
		private void Construct(Rules rules)
		{
			BuildQueue.Construct(this);
		}

		private ConstructionReport CalculateConstruction(Rules rules, ResourceAmountVector availableResources)
		{
			return BuildQueue.Preview(availableResources);
		}

		private void Decay(Rules rules)
		{
			AvailableResources -= CalculateDecay(rules, AvailableResources);
		}

		private ResourceAmountVector CalculateDecay(Rules rules, ResourceAmountVector availableResources)
		{ 
			ResourceAmountVector decay = availableResources * 0;

			foreach (var resource in rules.Resources.Items)
			{
				int resourceDecay = (int)(availableResources[resource] * resource.DecayRate);
				decay += resource.Versor * resourceDecay;
			}

			return decay;
		}
	}

	public class Projection
	{
		public ResourceAmountVector Produced { get; set; }

		public ResourceAmountVector Consumed { get; set; }

		public ResourceAmountVector Wasted { get; set; }

		public ResourceAmountVector Available { get; set; }
	}
}
