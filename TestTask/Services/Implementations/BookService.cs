using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private const string GOAL_NAME = "Red";
        private readonly DateTime FORM_DATE = new(2012, 5, 25);


        private readonly ApplicationDbContext _dbContext;


        public BookService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Book> GetBook()
        {
            return await _dbContext.Books
                .OrderByDescending(b => b.Price)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetBooks()
        {
            return await _dbContext.Books
               .Where(b => b.Title.Contains(GOAL_NAME) && b.PublishDate > FORM_DATE)
               .ToListAsync();
        }
    }
}
