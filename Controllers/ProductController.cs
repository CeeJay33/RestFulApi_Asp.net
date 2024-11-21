using CrudApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace CrudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string connectionString;
        public ProductController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"] ?? "";
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO products (name, brand, category, price, description) " +
                       "VALUES (@name, @brand, @category, @price, @description)";

                    using (var cmd = new MySqlCommand(sql, conn)) 
                    {
                        cmd.Parameters.AddWithValue("@name", productDto.Name);
                        cmd.Parameters.AddWithValue("@brand", productDto.Brand);
                        cmd.Parameters.AddWithValue("@category", productDto.Category);
                        cmd.Parameters.AddWithValue("@price", productDto.Price);
                        cmd.Parameters.AddWithValue("@description", productDto.Description);

                        cmd.ExecuteNonQuery();
                    }

                    }
                }
            catch (Exception ex) {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }


        [HttpGet]
        public IActionResult GetProduct()
        {
            List<Product> products = new List<Product>();

            try
            {

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM products";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product();

                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);

                                products.Add(product);
                            }

                        }
                    }
                }

                }
            catch (Exception ex) {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        { 
            Product product = new Product();

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM products WHERE id=@id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);

                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(product);
        }


        [HttpPut]
        public IActionResult UpdateProduct(int id, ProductDto productDto)
        {

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "UPDATE products SET name=@name, brand=@brand, category=@category, price=@price, description=@description WHERE id=@id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", productDto.Name);
                        cmd.Parameters.AddWithValue("@brand", productDto.Brand);
                        cmd.Parameters.AddWithValue("@category", productDto.Category);
                        cmd.Parameters.AddWithValue("@price", productDto.Price);
                        cmd.Parameters.AddWithValue("@description", productDto.Description);
                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok(); 
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id) 
        {



            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "DELETE FROM products WHERE id=@id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Product", "Sorry, but we have an exception");
                return BadRequest(ModelState);
            }
            return Ok();
        }

    }

}
