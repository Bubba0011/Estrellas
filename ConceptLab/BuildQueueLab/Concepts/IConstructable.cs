namespace BuildQueueLab.Concepts
{
	interface IConstructable
	{
		string Name { get; }

		ResourceAmountVector ConstructionCost { get; }
	}
}
