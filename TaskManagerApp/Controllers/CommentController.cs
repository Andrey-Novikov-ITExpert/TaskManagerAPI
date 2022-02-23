using Devart.Data.PostgreSql;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerApp;

namespace CommentManagerApp.Controllers
{
    /// <summary>
    /// API controller for the comments data operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]       
    public class CommentController : Controller
    {
        private IConfiguration configuration;
        public CommentController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }
        /// <summary>
        /// Getting all comments from the Database
        /// </summary>
        /// <returns>JSON array of the comments</returns>
        // GET: api/<CommentController>
        [HttpGet]
        public List<Comment> Get()
        {
            try
            {
                List<Comment> Comments = new List<Comment>();
                // Getting Connection string
                string dbConnectionString = configuration.GetConnectionString("DBConnectionString");
                // Initializing the Database connection
                using (PgSqlConnection pgSqlConnection = new PgSqlConnection(dbConnectionString))
                {
                    // Initializing the SQL Command
                    using (PgSqlCommand pgSqlCommand = new PgSqlCommand())
                    {
                        // Getting all comments
                        pgSqlCommand.CommandText = "select * from public.comment";
                        pgSqlCommand.Connection = pgSqlConnection;

                        // Checking if connection open
                        if (pgSqlConnection.State != System.Data.ConnectionState.Open)
                            pgSqlConnection.Open();
                        
                        // Read the data
                        using (PgSqlDataReader pgSqlReader = pgSqlCommand.ExecuteReader())
                        {
                            while (pgSqlReader.Read())
                            {
                                Comment Comment = new Comment();
                                Comment.commentid = int.Parse(pgSqlReader.GetValue(0).ToString());
                                Comment.dateadded = (DateTime)pgSqlReader.GetValue(1);
                                Comment.commenttext = pgSqlReader.GetValue(2).ToString();                                
                                Comment.reminderdate = (DateTime)pgSqlReader.GetValue(3);
                                Comment.taskid = int.Parse(pgSqlReader.GetValue(4).ToString());
                                Comment.commenttype = pgSqlReader.GetValue(5).ToString();

                                Comments.Add(Comment);
                            }
                        }
                    }
                }

                return Comments;
            }

            catch
            {
                throw;
            }
        }

        // GET api/<CommentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Adding new comment to the database
        /// </summary>
        /// <param name="Comment">Comment body</param>
        /// <returns>Number of DB rows updated</returns>
        // POST api/<CommentController>
        [HttpPost]
        public int Post([FromBody] Comment Comment)
        {
            try
            {   // Getting Connection string
                string dbConnectionString = configuration.GetConnectionString("DBConnectionString");
                // Initializing the Database connection
                using (PgSqlConnection pgSqlConnection = new PgSqlConnection(dbConnectionString))
                {
                    // Initializing the SQL Command
                    using (PgSqlCommand cmd = new PgSqlCommand())
                    {
                        // Setting up the insert query
                        cmd.CommandText = "INSERT INTO public.comment (dateadded, commenttext, commenttype, reminderdate, taskid)" +
                            " VALUES(@dateadded, @commenttext, @commenttype, @reminderdate, @taskid)";

                        cmd.Connection = pgSqlConnection;
                        
                        cmd.Parameters.AddWithValue("@dateadded", Comment.dateadded.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@commenttext", Comment.commenttext);
                        cmd.Parameters.AddWithValue("@commenttype", "{I}");
                        cmd.Parameters.AddWithValue("@reminderdate", Comment.reminderdate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@taskid", Comment.taskid);

                        if (pgSqlConnection.State != System.Data.ConnectionState.Open)
                            pgSqlConnection.Open();

                        return cmd.ExecuteNonQuery();
                    }
                }
            }

            catch
            {
                throw;
            }
        }

        // PUT api/<CommentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// Deleting comment by id
        /// </summary>
        /// <param name="id">Comment ID to be deleted</param>
        // DELETE api/<CommentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
                // Getting Connection string
                string dbConnectionString = configuration.GetConnectionString("DBConnectionString");
                // Initializing the Database connection
                using (PgSqlConnection pgSqlConnection = new PgSqlConnection(dbConnectionString))
                {
                    // Initializing the SQL Command
                    using (PgSqlCommand cmd = new PgSqlCommand())
                    {
                        // Setting up the delete query
                        cmd.CommandText = $"DELETE from public.comment where commentid={id}";
                        cmd.Connection = pgSqlConnection;

                        if (pgSqlConnection.State != System.Data.ConnectionState.Open)
                            pgSqlConnection.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            catch
            {
                throw;
            }
        }
    }
}
