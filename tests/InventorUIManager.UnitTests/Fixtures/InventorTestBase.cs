using Moq;

namespace InventorUIManager.UnitTests.Fixtures
{
	/// <summary>
	/// Base class for unit tests requiring Inventor application mocks.
	/// </summary>
	public abstract class InventorTestBase
	{
		protected Mock<Inventor.Application> MockApplication { get; }

		protected InventorTestBase()
		{
			MockApplication = MockInventorObjects.CreateApplicationMock();
		}
	}
}
