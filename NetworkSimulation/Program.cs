using Dentisterie.Data;
using NetworkSimulation.Entities;
using NetworkSimulation.GUI;

namespace NetworkSimulation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            List<Serveur> allServers = new List<Serveur>();
            using (var context = new ReseauDbContext())
            {
                allServers = context.serveurs.ToList();
            }
            foreach (var server in allServers)
            {
                server.SetNonDbAttributes();
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new ServeurDrawer(allServers));
        }

        //static void Main(string[] args)
        //{
        //    List<Serveur> allServers = new List<Serveur>();
        //    using(var context = new ReseauDbContext())
        //    {
        //        allServers = context.serveurs.ToList();
        //    }
        //    foreach (var server in allServers)
        //    {
        //        server.SetNonDbAttributes();
        //    }

        //    Serveur source = allServers.ElementAt(0);
        //    List<Serveur> path = source.SearchByDijkstra("facebook.com", allServers);
        //    foreach (var serveur in path)
        //    {
        //        Console.Write($"- {serveur.id_serveur} - ");
        //    }
        //    Console.WriteLine();

        //    List<Serveur> path2 = source.SearchByBFS("facebook.com", allServers);
        //    foreach (var serveur in path2)
        //    {
        //        Console.Write($"- {serveur.id_serveur} - ");
        //    }
        //    Console.WriteLine();
        //}
    }
}