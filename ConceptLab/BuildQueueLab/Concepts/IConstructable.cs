namespace BuildQueueLab.Concepts
{
	interface IConstructable
	{
		string Name { get; }

		ResourceAmountVector ConstructionCost { get; }

		void Deploy(Planet planet, int count);
	}
}
