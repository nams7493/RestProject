using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebRole1.Controllers
{
    public class ValuesController : ApiController
    {
        string connstring = "Server=testdb74933.database.windows.net;Database=testdb7493 ;Connect Timeout=60;MultipleActiveResultSets=True; MultiSubnetFailover=True;user id=nams7493;password=master7493@";
        // GET api/values
        public List<Student> Get()
        {
            Stopwatch stopwatch = new Stopwatch();
            //To measure function execcution time
            stopwatch.Start();
            List<Student> Students = new List<Student>();
            try
            {
                
                const string strSql = @"select * from students";
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(strSql, conn))
                    {
                        conn.Open();

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                Students.Add(new Student { id = sdr.GetInt32(0), firstname = sdr.GetString(1),lastname=sdr.GetString(2),score=sdr.GetInt32(3) });
                            }
                        }
                    }
                }

                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                //For now logging on console,can log it on another axure platforms
                Console.WriteLine(ex);
            }
            return Students;
        }

        // GET api/values/5
        [Route("api/getsubject/{id}")]
        public List<string> Get(int id)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<string> Subjects = new List<string>();
            try
            {

                const string strSql = @"SELECT s.name from subjectmapping as sm inner JOIN subjects as s ON sm.subjectid=s.id WHERE sm.studentid=1";
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(strSql, conn))
                    {
                        conn.Open();

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                Subjects.Add(sdr.GetString(0));
                            }
                        }
                    }
                }

                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Subjects;
        }

        // POST api/values
        [Route("api/insert/{firstname=a&lastname=b&score=c}")]
        public string Post( [FromUri]string firstName, [FromUri]string lastname, [FromUri]int score)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string result = "";
            try
            {
                List<Student> Students = new List<Student>();
                
                const string strSql = @"insert into students(firstname,lastname,score)values(@firstname,@lastname,@score)";
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(strSql, conn))
                    {
                        conn.Open();
                        //cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@firstname", firstName);
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@score", score);
                        cmd.ExecuteReader();
                    }
                }
                result = "Student with name:" + firstName + " is inserted";
                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = "Operation not successful!Please try again.";
                return result;
            }
        }

        // PUT api/values/5
        [Route("api/modify/{id=a&firstname=b}")]
        public string Put([FromUri]int id, [FromUri]string firstName)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string result = "";
            try
            {
                List<Student> Students = new List<Student>();
                const string strSql = @"update students set firstname=@firstname where id=@id";
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(strSql, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@firstname", firstName);
                        cmd.ExecuteReader();
                    }
                }
                result = "Student with name:" + firstName + " is modified";
                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = "Modification not successful!";
                return result;
            }
        }

        // DELETE api/products/5
        [Route("api/delete/{name}")]
        public string Delete(string name)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string result = "";
            try
            {
                List<Student> Students = new List<Student>();
                const string strSql = @"delete from students where firstname=@name";
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    using (SqlCommand cmd = new SqlCommand(strSql, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.ExecuteReader();
                    }
                }
                result = "Student with name:" + name + " is deleted";
                stopwatch.Stop();
                Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = "Deletion not successful!";
                return result;
            }
        }
    }
}

