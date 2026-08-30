using Microsoft.EntityFrameworkCore;

public static class getMembersEndpoint
{
    public static void MapGetMembers(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/members", async (
            AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var members = await dbContext.Members
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Results.Ok(members);
        });
    }
}