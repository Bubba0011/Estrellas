namespace Concepts.Core
{
	internal class ConstructionReport
	{
		public ResourceAmountVector ConsumedResources { get; private set; }

		// ctor
		public ConstructionReport(ResourceAmountVector anyVector)
		{
			ConsumedResources = 0 * anyVector;
		}

		public void AddItem(ResourceAmountVector consumed)
		{
			ConsumedResources += consumed;
			// TODO: What was constructed? Status?
		}
	}
}
