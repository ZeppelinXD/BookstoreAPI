using MySql.Data.MySqlClient;
using BookStoreAPI.BookGetSet;

namespace BookStoreAPI.Repository
{
    public class BookRepository
    {
        private readonly string _connectionString;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var books = new List<Book>();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Books";
            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                books.Add(new Book
                {
                    Id = reader.GetInt32("Id"), 
                    Title = reader.GetString("Title"), 
                    Author = reader.GetString("Author"),
                    PublicationYear = reader.GetInt32("PublicationYear"), 
                    ISBN = reader.GetString("ISBN"),
                    InStock = reader.GetInt32("InStock") 
                });
            }

            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Books WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Book
                {
                    Id = reader.GetInt32("Id"), 
                    Title = reader.GetString("Title"),
                    Author = reader.GetString("Author"),
                    PublicationYear = reader.GetInt32("PublicationYear"), 
                    ISBN = reader.GetString("ISBN"),
                    InStock = reader.GetInt32("InStock") 
                };
            }

            return null;
        }

        public async Task AddBookAsync(Book book)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "INSERT INTO Books (Title, Author, PublicationYear, ISBN, InStock) VALUES (@Title, @Author, @PublicationYear, @ISBN, @InStock)";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear); // Pass integer value
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@InStock", book.InStock); // Pass integer value

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateBookAsync(int id, Book book)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "UPDATE Books SET Title = @Title, Author = @Author, PublicationYear = @PublicationYear, ISBN = @ISBN, InStock = @InStock WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear); // Pass integer value
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@InStock", book.InStock); // Pass integer value

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteBookAsync(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "DELETE FROM Books WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}
