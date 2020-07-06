using Microsoft.Data.SqlClient;
using System;
using System.Threading;
using static System.Console;

namespace Parkeringhus
{
    class Program
    {
        // Vad är machinen som databasen hanteraren kör på eller vad ligger machinen?
        // vi bhevör en typ av address när vi väll known av machinen med anropp.
        // Addressen till server som hostar databasen motorn som sen hanterar databasen.
        // vi behöver komma till machinen först och det behöver vi kallas för
        // connection string.
        // då behöver vi tre andra grejer. 
        // 1. Vilken databasen vi kommer åt? eller var ligger databasen?
        // 2. Vad heter databasen eller vilken databasen kommer vi koppla till?
        // 3. Vem är vi? Hur vi identifiera oss? Databasen hanterar vet att vi 
        // får åt komma till databasen och det fallet använder vi Integrated Security=true
        // och Det innebär att local användaren på machinen kan logga in.

        static string connectionString = "Data Source=.; Initial Catalog=Parkeringshus; Integrated Security=true";
        static void Main(string[] args)
        {           

            bool shouldNotExit = true;

            while (shouldNotExit)
            {
                WriteLine("1. Register arrival");
                WriteLine("2. Register departure");
                WriteLine("3. Show parking registry");
                WriteLine("4. Exit");

                ConsoleKeyInfo keyPressed = ReadKey();

                Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        {
                            Write("Customer: ");
                            string customer = ReadLine();

                            Write("Contact details: ");
                            string contactDetails = ReadLine();

                            Write("Registration number: ");
                            string registrationNumber = ReadLine();

                            Write("Description: ");
                            string description = ReadLine();

                            DateTime arrival = DateTime.Now;

                            // Vi sätter in @ symbol---> placeholder like  @Customer för att den kommer
                            // vi skjuta in själva med värde och sen kommer skicka till servern. 
                            string query = @"INSERT INTO Parking (Customer,
                                                              ContactDetails,
                                                              RegistrationNumber,
                                                              Description,
                                                              Arrival)
                                       VALUES (@Customer,
                                               @ContactDetails,
                                               @RegistrationNumber,
                                               @Description,
                                               @Arrival)";
                            // Vi behöver skappa upp ett objekt som representerar anslutning till databasen.
                            // Det objektet känner till vad är connectionString som vi har skapat tidigare och
                            // lägga in inne.

                            SqlConnection connection = new SqlConnection(connectionString);

                            // Nu har vi skappat nånting like connection som anslutar till server. Nu skappar vi
                            // nånting som skickar sql commando till server. Det heter SqlCommand.
                            // Då vill vi ha två grejer. 
                            // 1. Det vill vi veta, vilken kommand ska vi köra och
                            // 2. Vilken connection objekt och query string ska sätta in like query, connection?

                            SqlCommand command = new SqlCommand(query, connection);

                            // I nästa steg, vi behöver tilldela @Customer värde till variable customer och smma till andra variablar. Det gör vi genom command.

                            command.Parameters.AddWithValue("@Customer", customer);

                            command.Parameters.AddWithValue("@ContactDetails", contactDetails);

                            command.Parameters.AddWithValue("@RegistrationNumber", registrationNumber);

                            command.Parameters.AddWithValue("@Description", description);

                            command.Parameters.AddWithValue("@Arrival", arrival);

                            connection.Open();

                            command.ExecuteNonQuery();

                            connection.Close();

                            // Vi behöver koden som lägger in den Tabellen i Databasen
                            // TODO: Add method to insert record into database

                            Clear();

                            WriteLine("Parking registered");

                            Thread.Sleep(2000);

                            Clear();

                            break;

                        }
                       

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        

                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        { 
                        WriteLine("ID  Registration Number     Arrival     Departure");
                        WriteLine("-----------------------------------------------------");

                            string query = @"SELECT Id
                                              ,Customer
                                              ,ContactDetails
                                              ,RegistrationNumber
                                              ,Description
                                              ,Arrival
                                              ,Departure
                                              FROM Parking";

                            // SqlConnection är det vi använder för att upprätta (establish) anslutning till servern och databasen hanteraren.  
                            SqlConnection connection = new SqlConnection(connectionString);

                            // SqlCommand är det ett Objekt som represeterar det kommandot vi köra. 
                            SqlCommand command = new SqlCommand(query, connection);

                            connection.Open();

                            SqlDataReader dataReader = command.ExecuteReader();

                            // den Read() inne i While loopen kommer att retunera en boolen --> true eller false
                            // den  Read()--> försöker läsa en rad från databasen och om det gick och läsa, Om det fanns
                            // en rad att läsa då retunera den true och då kan vi komma åt värden i dataReader.  
                            while (dataReader.Read())
                            {
                                string id = dataReader["Id"].ToString();

                                string registrationNumber = dataReader["RegistrationNumber"].ToString();

                                string arrival = dataReader["Arrival"].ToString();

                                string departure = dataReader["Departure"].ToString();

                                Write(id.PadRight(5, ' '));
                                Write(registrationNumber.PadRight(20, ' '));
                                Write(arrival.PadRight(20, ' '));
                                WriteLine(departure);

                            }


                            connection.Close();

                            WriteLine();
                            WriteLine("Press key to continue");

                            ReadKey(true);

                            Clear();

                        break;

                        }
                       

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:

                        shouldNotExit = false;

                        break;
                }
            }
        }
    }
}
