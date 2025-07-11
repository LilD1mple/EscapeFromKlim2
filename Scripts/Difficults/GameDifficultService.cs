namespace EFK2.Difficult
{
	public sealed class GameDifficultService : IDifficultService
	{
		private DifficultLevelConfiguration _difficultLevel;

		DifficultLevelConfiguration IDifficultService.DifficultConfiguration => _difficultLevel;

		void IDifficultService.SetLevelDifficult(DifficultLevelConfiguration configuration) => _difficultLevel = configuration;
	}
}