using System;
using System.Net.Mail;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace EmailNotifications
{
    public class SendNotification
    {
        public List<Event> ExecuteEventQuery()
        {
            List <Event> Events = new List<Event>();

            string eventsQuery = "SELECT UserId, EventId, Events.Name, Description, StartDate, EndDate, Categories.Name AS Category," +
                " \r\nFrequencies.Name AS Frequency\r\n  FROM SelectedEvents\r\n " +
                "INNER JOIN Events ON Events.Id = SelectedEvents.EventId\r\n " +
                "INNER JOIN Categories ON Categories.Id = Events.CategoryId\r\n" +
                "INNER JOIN Frequencies ON Frequencies.Id = Events.FrequencyId\r\n " +
                "WHERE CAST(StartDate AS Date) = (SELECT CAST(GETDATE()+1 AS Date))";

            using (SqlConnection sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionStringAsterismWay")))
            {
                using (SqlCommand command = new SqlCommand(eventsQuery, sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Event Event = new Event();
                            Event.Name = reader.GetValue(reader.GetOrdinal("Name")).ToString();
                            Event.Description = reader.GetValue(reader.GetOrdinal("Description")).ToString();
                            Event.Frequency = reader.GetValue(reader.GetOrdinal("Frequency")).ToString();
                            Event.Category = reader.GetValue(reader.GetOrdinal("Category")).ToString();
                            Event.StartDate = reader.GetValue(reader.GetOrdinal("StartDate")).ToString();
                            Event.EndDate = reader.GetValue(reader.GetOrdinal("EndDate")).ToString();
                            Event.UserId = reader.GetValue(reader.GetOrdinal("UserId")).ToString();

                            Events.Add(Event);
                        }
                    }
                }
                
            }

            return Events;
        }

        public List<User> ExecuteUserQuery(List<Event> Events)
        {
            List<User> Users = new List<User>();
            using (SqlConnection sqlConnection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionStringIdentityServer")))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    sqlConnection.Open();
                    foreach (var Event in Events)
                    {
                        bool containsItem = Users.Any(x => x.Id == Event.UserId);
                        if (!containsItem)
                        {
                            User user = new User();
                            command.CommandType = System.Data.CommandType.Text;
                            command.Parameters.AddWithValue("@Id", Event.UserId);
                            user.Id = Event.UserId;
                            command.CommandText = @"SELECT  Id ,FirstName, LastName, Email FROM AspNetUsers WHERE Id= @Id";

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    user.FirstName = reader.GetValue(reader.GetOrdinal("FirstName")).ToString();
                                    user.LastName = reader.GetValue(reader.GetOrdinal("LastName")).ToString();
                                    user.Email = reader.GetValue(reader.GetOrdinal("Email")).ToString();
                                    Users.Add(user);
                                }
                            }
                            command.Parameters.Clear();
                        }
                    }
                }

            }

            return Users;

        }

        [FunctionName("SendNotification")]
        public void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            List<Event> Events = ExecuteEventQuery();
            List<User> Users = ExecuteUserQuery(Events);

            Console.WriteLine(Users);
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("natalia.gerus892@gmail.com", "kzdojfoidoznhooi"),
                EnableSsl = true,
            };

            foreach (var Event in Events)
            {
                User user = Users.FirstOrDefault(x => x.Id == Event.UserId);
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("natalia.gerus892@gmail.com"),
                    Subject = Event.Name,
                    Body = $@"<h2>Привіт, {user.FirstName} {user.LastName}!</h2>" +
                    $@"<h3>Це нагадування про те, що подію «{Event.Name}» з категорії «{Event.Category}», яка відбувається {Event.Frequency.ToLower()} 
                    ви можете побачити вже завтра! <br></br> Початок - {Event.StartDate} <br>Кінець - {Event.EndDate}</br> <br>Читайте детальніший опис нижче:</br></h3>" +
                    $@"<blockquote><h4>{Event.Description}<h4></blockquote>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(user.Email);

                smtpClient.Send(mailMessage);
            }
        }
    }
}
