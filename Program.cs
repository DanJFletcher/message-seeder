using System;
using Dapper;
using Dapper.Contrib.Extensions;
using Z.Dapper.Plus;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace voxie_message_seeder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var port = 33061;
            var server = "localhost";
            var database = "homestead";
            var user = "root";
            var password = "secret";

            var connectionString = $"Server={server};Port={port};Database={database};Uid={user};Pwd={password};";

            using (var connection = new MySqlConnection(connectionString)) {
                var count = "SELECT COUNT(*) FROM messages";

                connection.Open();
                Stopwatch stopWatch = new Stopwatch();


                var result = await connection.QueryAsync<Int32>(count);

                Console.WriteLine(result.Sum());

                var recordsToInsert = 1000000;
                var workerCount = 100;

                stopWatch.Start();
                TimeSpan ts;
                string elapsedTime = "";
                int recordsInserted = 0;

                void outputStatus ()
                {
                    Console.Clear();
                    Console.WriteLine("InsertingMessages...\n");
                    Console.WriteLine($"Total records in DB: {result.Sum() + recordsInserted}");
                    Console.WriteLine($"{recordsInserted}/{ recordsToInsert}    {(int)((float)recordsInserted / (float)recordsToInsert * 100)}%");

                    ts = stopWatch.Elapsed;

                    // Format and display the TimeSpan value.
                    elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
                    Console.WriteLine("\nRunTime " + elapsedTime);
                }

                List<Thread> threads = new List<Thread>();


                for (int i = 0; i < workerCount; i++) {
                    //var thread = new Thread(() => {
                        var taskCount = recordsToInsert / workerCount;
                        var messages = Enumerable.Range(0, taskCount)
                            .Select(x => {
                                var message = new Message {
                                    id = (i * taskCount) + x,
                                    contact_id = (i * taskCount) + x
                                };
                                recordsInserted++;
                                outputStatus();

                                return message;
                            });

                        connection.BulkInsert<Message>(messages);
                    //});
                    //thread.Start();
                    //threads.Add(thread);
                }

                //int completedThreads = 0;

                //while (completedThreads < workerCount) {
                //    threads.ForEach(x => {
                //        if (!x.IsAlive)
                //        {
                //            completedThreads++;
                //        }
                //    });
                //}

                outputStatus();

                stopWatch.Stop();

                connection.Dispose();
            }

            Console.WriteLine("\nDone.");
        }
    }
}
