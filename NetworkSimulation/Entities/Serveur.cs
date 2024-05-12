using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dentisterie.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace NetworkSimulation.Entities
{
    public class Serveur
    {
        [Key]
        public int id_serveur { get; set; }

        [StringLength(15)]
        public string ip_adress { get; set; }

        public int x { get; set; }
        public int y { get; set; }

        public bool activite { get; set; }
        public List<UrlMapping> urls { get; set; }

        [NotMapped]
        public float distance { get; set; }

        [NotMapped]
        public Serveur antecedent { get; set; }

        [NotMapped]
        public Dictionary<Serveur, int> voisins { get; set; }

        public Serveur()
        {
            this.urls = new List<UrlMapping>();
            this.voisins = new Dictionary<Serveur, int>();
            this.antecedent = null;
            this.distance = float.MaxValue;
        }

        public Serveur(int id_serveur, string ip_adress, int x, int y, bool activite)
        {
            this.id_serveur = id_serveur;
            this.ip_adress = ip_adress;
            this.x = x;
            this.y = y;
            this.activite = activite;
        }

        void GetVoisins(string serverToGet, string serverThisIs)
        {
            using (var context = new ReseauDbContext())
            {
                //string connectionString = context.Database.GetDbConnection().ConnectionString;
                string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=olafienby7;Database=reseausim;";
                //Console.WriteLine(connectionString);
                string query = $"select {serverToGet}, pingvalue from chemins where {serverThisIs} = {this.id_serveur}";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    connection.Open();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idserveur1 = reader.GetInt32(0);
                            int pingvalue = reader.GetInt32(1);
                            Serveur s = context.serveurs.Find(idserveur1);
                            if (s != null)
                            {
                                this.voisins.Add(s, pingvalue);
                            }
                        }
                    }
                }

            }
        }

        public void AddVoisins(Serveur serveur2, int pingValue)
        {
            using (var context = new ReseauDbContext())
            {
                //string connectionString = context.Database.GetDbConnection().ConnectionString;
                string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=olafienby7;Database=reseausim;";
                //Console.WriteLine(connectionString);
                string query = $"insert into chemins values ({this.id_serveur}, {serveur2.id_serveur}, {pingValue})";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                }
            }

            this.voisins.Add(serveur2, pingValue);
            serveur2.voisins.Add(this, pingValue);
        }

        void SetVoisins()
        {
            this.voisins = new Dictionary<Serveur, int>();
            this.GetVoisins("idserveur1", "idserveur2");
            this.GetVoisins("idserveur2", "idserveur1");
        }

        public void SetNonDbAttributes()
        {
            this.SetVoisins();
            this.FetchUrls();
        }

        public static Serveur GetById(int id)
        {
            Serveur s = null;

            using (var context = new ReseauDbContext())
            {
                s = context.serveurs.Find(id);
                if (s != null)
                {
                    s.SetNonDbAttributes();
                }
            }

            return s;
        }

        public static void Dijkstra(Serveur source, List<Serveur> serveurs)
        {
            foreach (var serveur in serveurs)
            {
                serveur.distance = float.MaxValue;
                serveur.antecedent = null;
            }

            source.distance = 0;
            var nonVisitedServeurs = new HashSet<Serveur>(serveurs.Where(s => s.activite));

            while (nonVisitedServeurs.Count != 0)
            {
                var currentServeur = nonVisitedServeurs.OrderBy(s => s.distance).First();
                //Console.WriteLine($"On va traiter {currentServeur.id_serveur}, il a {currentServeur.voisins.Count} voisins et une distance de {currentServeur.distance} jusque là");

                nonVisitedServeurs.Remove(currentServeur);

                foreach (var voisin in currentServeur.voisins)
                {
                    Serveur actualVoisin = serveurs.Where(s => s.id_serveur == voisin.Key.id_serveur).First();
                    if (!actualVoisin.activite)
                    {
                        //Console.WriteLine($"On passe {actualVoisin.id_serveur}");
                        continue;
                    }
                    else
                    {
                        var altDistance = currentServeur.distance + voisin.Value;
                        if (altDistance < actualVoisin.distance)
                        {
                            actualVoisin.distance = altDistance;
                            actualVoisin.antecedent = currentServeur;
                            //Console.WriteLine($"ça prend {actualVoisin.distance} pour aller vers {actualVoisin.id_serveur}");
                        }
                       
                    }

                }
            }
        }

        public List<Serveur> FindPathTo(Serveur objectif, List<Serveur> serveurs)
        {
            List<Serveur> path = new List<Serveur>();

            Serveur current = objectif;
            while(current != null)
            {
                path.Add(current);
                current = current.antecedent;
            }

            // sending the list from the source to the objectif
            path.Reverse();

            return path;
        }

        public void AddMapping(string url)
        {
            using(var context = new ReseauDbContext())
            {
                UrlMapping um = new UrlMapping(0, this.id_serveur, url);
                context.dns.Add(um);
                context.SaveChanges();

                this.urls.Add(um);
            }
        }
        
        public void FetchUrls()
        {
            this.urls = new List<UrlMapping>();

            using (var context = new ReseauDbContext())
            {
                List<UrlMapping> urls = context.dns.ToList().Where(d => d.id_serveur == this.id_serveur).ToList();
                this.urls = urls;
            }
        }

        public static List<Serveur> FindUrlHosts(string url, List<Serveur> serveurs) 
        { 
            List<Serveur> hosts = new List<Serveur>();

            foreach(Serveur s in serveurs)
            {
                foreach (var u in s.urls)
                {
                    if (string.Equals(u.url, url.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        hosts.Add(s);
                        break;
                    }
                }
            }

            return hosts;
        }

        public static List<Serveur> BFS(Serveur source, Serveur objectif, List<Serveur> serveurs)
        {
            
            foreach (var serveur in serveurs)
            {
                serveur.distance = float.MaxValue;
                serveur.antecedent = null;
            }

            source.distance = 0;

            Queue<Serveur> queue = new Queue<Serveur>();

            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                Serveur currentServeur = queue.Dequeue();
                //Console.WriteLine($"Visited Serveur {currentServeur.id_serveur}");

                if(currentServeur == objectif)
                {
                    break;
                }

                if(!currentServeur.activite)
                {
                    continue;
                }

                foreach (var voisin in currentServeur.voisins)
                {
                    Serveur actualVoisin = serveurs.Where(s => s.id_serveur == voisin.Key.id_serveur).First();
                    if (!actualVoisin.activite)
                    {
                        continue;
                    }
                    else if(actualVoisin.distance == float.MaxValue)
                    {
                        actualVoisin.distance = currentServeur.distance + 1;
                        actualVoisin.antecedent = currentServeur;
                        queue.Enqueue(actualVoisin);
                    }
                }
            }
            
            List<Serveur> path = new List<Serveur>();
            Serveur serveurNow = objectif;
            while (serveurNow != null)
            {
                path.Add(serveurNow);
                serveurNow = serveurNow.antecedent;
            }

            path.Reverse();
            return path;
        }

        public List<Serveur> SearchByDijkstra(string url, List<Serveur> allServers)
        {
            // this calling object must be a member of allServers or else the algorithm won't function

            List<Serveur> path = new List<Serveur>();

            Dijkstra(this, allServers);
            //foreach (var item in allServers)
            //{
            //    Console.WriteLine($"{item.id_serveur}'s antecedent is {item.antecedent.id_serveur}");
            //}

            List<Serveur> hosts = FindUrlHosts(url, allServers);

            Serveur currentServeur = hosts.OrderBy(s => s.distance).First();
            while (currentServeur != null)
            {
                path.Add(currentServeur);
                currentServeur = currentServeur.antecedent;
            }

            path.Reverse();

            return path;
        }

        public List<Serveur> SearchByBFS(string url, List<Serveur> allServers)
        {
            List<Serveur> path = new List<Serveur>(allServers);

            List<Serveur> hosts = FindUrlHosts(url, allServers);
            foreach (var host in hosts)
            {
                List<Serveur> pathToHost = BFS(this, host, allServers);
                if(pathToHost.Count < path.Count)
                {
                    path = pathToHost;
                }
            }

            return path;
        }
    }
}
