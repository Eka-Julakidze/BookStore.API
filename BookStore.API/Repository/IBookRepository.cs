using BookStore.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.API.Repository
{
    public interface IBookRepository
    {
        public Task<List<BookModel>> GetAllBooksAsync();
        public Task<BookModel> GetBookByIdAsync(int id);
        public Task<int> AddBookAsync(BookModel bookModel);
        public Task<int> UpdateBookAsync(int id, BookModel bookModel);
        public Task UpdateBookPatchAsync(int id, JsonPatchDocument bookModel);
        public Task<bool> DeleteBookAsync(int id);
    }
}
