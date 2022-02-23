using Devart.Data.PostgreSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TaskManagerApp.Controllers
{
    /// <summary>
    /// API controller for the tasks data operations
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private IConfiguration configuration;
        public TaskController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        /// <summary>
        /// Getting all tasks from the Database
        /// </summary>
        /// <returns>JSON array of the tasks</returns>
        // GET: api/<TaskController>
        [HttpGet]
        public List<Task> Get()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                // Getting Connection string
                string dbConnectionString = configuration.GetConnectionString("DBConnectionString");
                // Initializing the Database connection
                using (PgSqlConnection pgSqlConnection = new PgSqlConnection(dbConnectionString))
                {
                    // Initializing the SQL Command
                    using (PgSqlCommand pgSqlCommand = new PgSqlCommand())
                    {
                        // Getting all tasks
                        pgSqlCommand.CommandText = "select * from public.task";
                        pgSqlCommand.Connection = pgSqlConnection;
                        // Checking if connection open
                        if (pgSqlConnection.State != System.Data.ConnectionState.Open)
                            pgSqlConnection.Open();

                        // Read the data
                        using (PgSqlDataReader pgSqlReader = pgSqlCommand.ExecuteReader())
                        {
                            while (pgSqlReader.Read())
                            {
                                Task task = new Task();
                                task.TaskId = int.Parse(pgSqlReader.GetValue(0).ToString());
                                task.CreatedDate = (DateTime)pgSqlReader.GetValue(1);
                                task.RequiredByDate = (DateTime)pgSqlReader.GetValue(2);
                                task.TaskDescription = pgSqlReader.GetValue(3).ToString();
                                task.TaskStatus = pgSqlReader.GetValue(4).ToString();
                                task.TaskType = pgSqlReader.GetValue(5).ToString();
                                task.AssignedTo = pgSqlReader.GetValue(6).ToString();

                                tasks.Add(task);
                            }
                        }
                    }
                }

                return tasks;
            }

            catch
            {
                throw;
            }
        }

        // GET api/<TaskController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Adding new task to the database
        /// </summary>
        /// <param name="task">Task body</param>
        /// <returns>Number of DB rows updated</returns>
        // POST api/<TaskController>
        [HttpPost]
        public int Post([FromBody] Task task)
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

                        cmd.CommandText = "INSERT INTO public.task (taskid, createddate, requiredbydate, taskdescription, taskstatus, tasktype, assignedto)" +
                            " VALUES(@TaskId, @CreatedDate, @RequiredByDate, @TaskDesctiption, @TaskStatus, @TaskType, @AssignedTo)";

                        cmd.Connection = pgSqlConnection;
                        cmd.Parameters.AddWithValue("@taskid", task.TaskId);
                        cmd.Parameters.AddWithValue("@createddate", task.CreatedDate);
                        cmd.Parameters.AddWithValue("@requiredbydate", task.RequiredByDate);
                        cmd.Parameters.AddWithValue("@taskdescription", task.TaskDescription);
                        cmd.Parameters.AddWithValue("@taskstatus", task.TaskStatus);
                        cmd.Parameters.AddWithValue("@tasktype", task.TaskType);
                        cmd.Parameters.AddWithValue("@assignedto", task.AssignedTo);

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

        // PUT api/<TaskController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
