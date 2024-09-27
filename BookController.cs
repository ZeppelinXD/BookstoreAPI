using BookStoreAPI.BookGetSet;
using BookStoreAPI.Repository;

namespace BookStoreAPI.Controllers
{
    public static class BookController
    {
        public static void MapBookEndpoints(this WebApplication app)
        {   //Henter alle bøker eller søk etter bok med ID
            app.MapGet("/books", async (BookRepository repo) => await repo.GetAllBooksAsync());

            app.MapGet("/books/{id}", async (int id, BookRepository repo) =>
            {
                var book = await repo.GetBookByIdAsync(id);
                return book is not null ? Results.Ok(book) : Results.NotFound();
            });
            //Legger til ny bok
            app.MapPost("/books", async (Book book, BookRepository repo) =>
            {
                await repo.AddBookAsync(book);
                return Results.Created($"/books/{book.Id}", book);
            });
            //Endrer allerede eksisterende bok
            app.MapPut("/books/{id}", async (int id, Book book, BookRepository repo) =>
            {
                var existingBook = await repo.GetBookByIdAsync(id);
                if (existingBook is null)
                    return Results.NotFound();

                await repo.UpdateBookAsync(id, book);
                return Results.NoContent();
            });
            //Sletter bok etter ID
            app.MapDelete("/books/{id}", async (int id, BookRepository repo) =>
            {
                var book = await repo.GetBookByIdAsync(id);
                if (book is null)
                    return Results.NotFound();

                await repo.DeleteBookAsync(id);
                return Results.NoContent();
            });

            // Filtrering av bøker
            app.MapGet("/books/filter", async (string? title, int? publicationYear, string? author, BookRepository repo) =>
            {
                var books = await repo.GetAllBooksAsync();

                if (!string.IsNullOrEmpty(title))
                    books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

                if (publicationYear.HasValue)
                    books = books.Where(b => b.PublicationYear == publicationYear.Value);

                if (!string.IsNullOrEmpty(author))
                    books = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));

                return Results.Ok(books);
            });
        }
    }
}
