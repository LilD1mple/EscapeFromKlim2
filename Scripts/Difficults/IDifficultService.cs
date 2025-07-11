namespace EFK2.Difficult
{
	public interface IDifficultService
	{
		DifficultLevelConfiguration DifficultConfiguration { get; }

		void SetLevelDifficult(DifficultLevelConfiguration configuration);
	}
}
