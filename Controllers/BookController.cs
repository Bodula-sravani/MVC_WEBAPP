using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MVC_web_app.Models;

namespace MVC_web_app.Controllers
{
    public class BookController : Controller
    {
        List<Book> booksList = new List<Book>();

        IConfiguration configuration;
        public BookController(IConfiguration configuration) {
            this.configuration = configuration;
        }
        public IActionResult BookList()
        {
            List<Book> booksList = new List<Book>();
            try
            {
                string connectionString = configuration.GetConnectionString("BooksDB");
                SqlConnection connection = new SqlConnection(connectionString);

                connection.Open();

                string query = "select BOOK_CODE,BOOK_TITLE,AUTHOR,PRICE,PUBLICATION from LMS_BOOK_DETAILS";
                SqlCommand command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();
                Console.WriteLine("reader");
                while (reader.Read())
                {
                    Book book = new Book();

                    book.Id = reader.GetString(0);
                    book.BookTitle = reader.GetString(1);
                    book.Author = reader.GetString(2);
                    book.Price = reader.GetDouble(3);
                    book.Publication = (string)reader["PUBLICATION"];
                    
                    booksList.Add(book);
                    Console.WriteLine(book.Id,booksList.Count);
                }
                ViewBag.booksList = booksList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View();
        }
    }
}
