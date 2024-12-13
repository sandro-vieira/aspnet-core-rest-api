using Microsoft.AspNetCore.Mvc.RazorPages;
using Movies.Application.Model;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping
{
    public static class ContractMapping
    {
        public static Movie MapToMovie(this CreateMovieRequest request)
        {
            return new Movie
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
        }

        public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
        {
            return new Movie
            {
                Id = id,
                Title = request.Title,
                YearOfRelease = request.YearOfRelease,
                Genres = request.Genres.ToList()
            };
        }

        public static MovieResponse MapToResponse(this Movie movie)
        {
            return new MovieResponse
            {
                Id = movie.Id,
                Title = movie.Title,
                Slug = movie.Slug,
                Rating = movie.Rating,
                UserRating = movie.UserRating,
                YearOfRelease = movie.YearOfRelease,
                Genres = movie.Genres
            };
        }

        public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies,
            int page, int pageSize, int totalCount)
        {
            return new MoviesResponse
            {
                Items = movies.Select(MapToResponse),
                Page = page,
                PageSize = pageSize,
                Total = totalCount
            };
        }

        public static IEnumerable<MovieRatingResponse> MapToResponse(this IEnumerable<MovieRating> ratings)
        {
            return ratings.Select(x => new MovieRatingResponse
            {
                MovieId = x.MovieId,
                Slug = x.Slug,
                Rating = x.Rating
            });
        }

        public static GetAllMoviesOptions MapToOptions(this GetAllMoviesRequest request)
        {
            var options = new GetAllMoviesOptions
            {
                Title = request.Title,
                YearOfRelease = request.Year,
                Page = request.Page,
                PageSize = request.PageSize
            };

            if (request.SortBy is null)
            {
                options.SortOrder = SortOrder.Unsorted;
            }
            else
            {
                if (request.SortBy.StartsWith('-'))
                {
                    options.SortOrder = SortOrder.Descending;
                }
                else
                {
                    options.SortOrder = SortOrder.Ascending;
                }

                var sortBy = request.SortBy.Trim('+', '-');

                options.SortField = sortBy.ToLower() switch
                {
                    "title" => sortBy,
                    "year" => "yearOfRelease",
                    _ => "invalid",
                };
            }

            return options;
        }

        public static GetAllMoviesOptions WithUser(this GetAllMoviesOptions options, Guid? userId)
        {
            options.UserId = userId;
            return options;
        }
    }
}
