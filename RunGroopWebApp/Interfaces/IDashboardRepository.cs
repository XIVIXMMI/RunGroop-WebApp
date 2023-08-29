namespace RunGroopWebApp.Models;

public interface IDashboardRepository
{
	Task<List<Race>> GetAllUserRaces();
	Task<List<Club>> GetAllUserClubs();
}

