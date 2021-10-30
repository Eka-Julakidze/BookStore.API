using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BookStore.API.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<BookModel>> GetAllBooksAsync()
        {
            //var records = await _context.Books.Select(x => new BookModel()
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Description = x.Description
            //}).ToListAsync();

            //return records;

            var records = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookModel>>(records);
        }

        public async Task<BookModel> GetBookByIdAsync(int id)
        {
            //var record = await _context.Books.Where(x => x.Id == id).Select(x =>
            //  new BookModel
            //  {
            //      Id = x.Id,
            //      Title = x.Title,
            //      Description = x.Description
            //  }).FirstOrDefaultAsync();
            //return record;

            var book = await _context.Books.FindAsync(id);
            return _mapper.Map<BookModel>(book);
                                                
        }

        public async Task<int> AddBookAsync(BookModel bookModel)
        {
            var book = new Books()
            {               
                Title = bookModel.Title,
                Description = bookModel.Description
            };

            await _context.Books.AddAsync(book);
            
            await _context.SaveChangesAsync();
            return book.Id;
        }

        public async Task<int> UpdateBookAsync(int id, BookModel bookModel)
        {
            // Version1 
            //var book = await _context.Books.FindAsync(id);
            //if(book != null)
            //{
            //    book.Title = bookModel.Title;
            //    book.Description = bookModel.Description;

            //    await _context.SaveChangesAsync();
            //    return book.Id;
            //}
            //return 0;

            // Version 2
            if(id <= _context.Books.Count())
            {
                var book = new Books()
                {
                    Id = id,
                    Title = bookModel.Title,
                    Description = bookModel.Description
                };
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return book.Id;
            }
            else
            {
                return 0;
            }
            
        }

        public async Task UpdateBookPatchAsync(int id, JsonPatchDocument bookModel)
        {
            var book = await _context.Books.FindAsync(id);
            if(book != null)
            {
                bookModel.ApplyTo(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            //var book = _context.Books.FindAsync(id).Result;
            if(id <= _context.Books.Count())
            {
                var book = new Books() { Id = id };
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }            
        }
    }
}
