using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly DateTime FROM_DATE = new DateTime(2015, 1, 1);

        private readonly ApplicationDbContext _dbContext;

 
        public AuthorService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Author> GetAuthor()
        {
            return await _dbContext.Authors
                .OrderByDescending(a => a.Id)
                .Select(a => new
                {
                    Author = a,
                    LongestTitleLength = a.Books.Max(b => b.Title.Length)
                })
                .OrderByDescending(x => x.LongestTitleLength)
                .ThenBy(x => x.Author.Id)
                .Select(x => x.Author)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await _dbContext.Authors
            .Select(a => new
            {
                Author = a,
                actualBooks = a.Books.Where(b => b.PublishDate > FROM_DATE)
            })
            .Where(x => x.actualBooks.Any() && x.actualBooks.Count() % 2 == 0)
            .Select(x => x.Author)
            .ToListAsync();
        }
    }
}
