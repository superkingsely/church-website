

using Microsoft.EntityFrameworkCore;

public static class createMemberEndpoint
{
   public static void MapCreateMember(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/members", async (
            CreateMemberRequest request,
            AppDbContext dbContext,
            CancellationToken cancellationToken) =>
        {
            var member = new Member
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Members.Add(member);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Created(
                $"/api/members/{member.Id}",
                member);
        });
    }
}