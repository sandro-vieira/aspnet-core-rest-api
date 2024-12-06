
using Dapper;
using Movies.Application.Database;

namespace Movies.Application.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RatingRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition("""
            select round(avg(rating), 1)
              from ratings
             where movieId = @movieId
            """, new { movieId }, cancellationToken: token));
    }

    public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition("""
            select round(avg(rating), 1),
                   (select rating
                      from ratings
                     where movieId = @movieId
                       and userId = @userId
                     limit 1)
              from ratings
             where movieId = @movieId
            """, new { movieId, userId }, cancellationToken: token));
    }
}