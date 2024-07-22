using Microsoft.EntityFrameworkCore;
using Profiles.Infrastructure.Data;
using Serilog;

namespace Profiles.API.Extensions;

public static class MigrationManager
{
    private static int _retryForAvailability = 0;

    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            try
            {
                scope.ServiceProvider.GetRequiredService<ProfilesDbContext>().Database.Migrate();
            }
            catch (Exception ex)
            {
                if (_retryForAvailability >= 5)
                {
                    throw;
                }

                _retryForAvailability++;

                Log.Warning(ex.Message);

                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, _retryForAvailability)));

                await MigrateDatabaseAsync(host);
            }
        }

        return host;
    }
}
