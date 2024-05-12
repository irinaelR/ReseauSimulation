using Dentisterie.Data;
using NetworkSimulation.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;

namespace NetworkSimulation.GUI
{
    public class ServeurDrawer : Form
    {
        private List<Serveur> serveurs;
        private Serveur selectedServeur;
        private ReseauDbContext context;
        private List<Serveur> pathsDijkstra;
        private List<Serveur> pathsBFS;

        public ServeurDrawer(List<Serveur> allServers)
        {
            serveurs = allServers;
            context = new ReseauDbContext();
            // Initialize the form
            this.Text = "NetSim";
            this.Size = new Size(800, 600);
            this.Paint += ServeurDrawer_Paint;
            this.MouseClick += ServeurDrawer_MouseClick;
        }

        private void ServeurDrawer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            // Redraw all serveurs and their connections
            foreach (Serveur serveur in serveurs)
            {
                foreach (var voisin in serveur.voisins)
                {
                    DrawConnection(e.Graphics, serveur, voisin.Key, voisin.Value);
                }
                DrawServeur(g, serveur);
            }
            
            if(pathsDijkstra != null)
            {
                foreach (Serveur sD in pathsDijkstra)
                {
                    g.FillEllipse(Brushes.Red, sD.x - 10, sD.y - 10, 20, 20);
                    if(sD.antecedent != null)
                    {
                        Pen pen = new Pen(Color.Red, 4);
                        g.DrawLine(pen, sD.x, sD.y, sD.antecedent.x, sD.antecedent.y);

                    }
                }
            }

            if (pathsBFS != null)
            {
                foreach (Serveur sD in pathsBFS)
                {
                    g.FillEllipse(Brushes.Green, sD.x - 10, sD.y - 10, 20, 20);
                    if (sD.antecedent != null)
                    {
                        Pen pen = new Pen(Color.Green, 4);
                        g.DrawLine(pen, sD.x, sD.y, sD.antecedent.x, sD.antecedent.y);

                    }
                }
            }
        }

        private void DrawServeur(Graphics g, Serveur serveur)
        {
            Brush b = Brushes.Blue;
            if(!serveur.activite)
            {
                b = Brushes.Gray;
            }
            g.FillEllipse(b, serveur.x - 10, serveur.y - 10, 20, 20);

            // Define the font and brush for the text
            Font font = new Font("Arial", 8);
            Brush brush = Brushes.Black;

            // Draw the text above the circle
            g.DrawString(serveur.ip_adress, font, brush, serveur.x - 25, serveur.y - 25);
        }

        private void DrawConnection(Graphics g, Serveur serveur1, Serveur serveur2, int pingValue)
        {
            int midX = (serveur1.x + serveur2.x) / 2;
            int midY = (serveur1.y + serveur2.y) / 2;

            Pen pen = new Pen(Color.Blue, 2);
            g.DrawLine(pen, serveur1.x, serveur1.y, serveur2.x, serveur2.y);

            Font font = new Font("Arial", 8);
            g.DrawString($"{pingValue}", font, Brushes.Blue, midX - 25, midY - 25);

        }

        private void ServeurDrawer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Serveur clickedServeur = GetClickedServeur(e.Location);
                if (clickedServeur == null)
                {
                    // Spawn a new serveur
                    string ipAddress = GetIpAddressFromUser();
                    if(ipAddress != null && ipAddress.Length > 0)
                    {
                        Serveur newServeur = new Serveur(0, ipAddress, e.X, e.Y, true);
                        context.serveurs.Add(newServeur);
                        context.SaveChanges();

                        newServeur.SetNonDbAttributes();
                        serveurs.Add(newServeur);

                    }
                }
                else
                {
                    // Show menu for existing serveur
                    selectedServeur = clickedServeur;
                    ContextMenuStrip menu = new ContextMenuStrip();
                    menu.Items.Add("Add URL Mapping", null, AddUrlMapping_Click);
                    menu.Items.Add("Search for a URL from here", null, SearchURL_Click);
                    menu.Items.Add("Connect to Another Serveur", null, ConnectServeur_Click);
                    menu.Items.Add("Disable/Enable", null, DisableServeur_Click);
                    menu.Show(this, e.Location);
                }
            }
            Invalidate();

        }

        private Serveur GetClickedServeur(Point location)
        {
            foreach (Serveur serveur in serveurs)
            {
                if (Math.Pow(location.X - serveur.x, 2) + Math.Pow(location.Y - serveur.y, 2) <= 100)
                {
                    return serveur;
                }
            }
            return null;
        }

        private string GetIpAddressFromUser()
        {
            return Microsoft.VisualBasic.Interaction.InputBox("Enter IP Address for the new Serveur:", "New Serveur IP Address", "127.0.0.1");
        }

        private string NewUrlForClickedServeur()
        {
            return Microsoft.VisualBasic.Interaction.InputBox("Enter new URL:", $"New URL for {this.selectedServeur.ip_adress}", "google.com");

        }

        private string URLToLookFor()
        {
            return Microsoft.VisualBasic.Interaction.InputBox("Enter new URL:", $"New URL for {this.selectedServeur.ip_adress}", "google.com");

        }

        private void SearchURL_Click(object sender, EventArgs e)
        {
            //Invalidate();
            string url = URLToLookFor();
            if(url != null &&  url.Length > 0)
            {
                this.pathsDijkstra = selectedServeur.SearchByDijkstra(url, serveurs);
                this.pathsBFS = selectedServeur.SearchByBFS(url, serveurs);
            }
            Invalidate();

        }

        private void AddUrlMapping_Click(object sender, EventArgs e)
        {
            // Implement adding URL mapping for selected serveur
            string url = NewUrlForClickedServeur();
            this.selectedServeur.AddMapping(url);
        }

        private void ConnectServeur_Click(object sender, EventArgs e)
        {
            // Get available serveurs for connection
            List<Serveur> availableServeurs = serveurs.Except(new List<Serveur> { selectedServeur }).ToList();
            foreach (var voisin in selectedServeur.voisins)
            {
                Serveur actualVoisin = serveurs.Where(s => s.id_serveur == voisin.Key.id_serveur).First();
                availableServeurs.Remove(actualVoisin);
            }

            // Show the connection form
            ConnectionForm connectionForm = new ConnectionForm(availableServeurs);
            if (connectionForm.ShowDialog() == DialogResult.OK)
            {
                var selectedServeurAndNumber = connectionForm.GetSelectedServeurAndNumber();
                Serveur selectedServeurToConnect = selectedServeurAndNumber.Item1;
                int number = selectedServeurAndNumber.Item2;

                // Connect selected serveur to another serveur with the given number (implementation required)
                selectedServeur.AddVoisins(selectedServeurToConnect, number);
                Invalidate();

            }
        }

        private void DisableServeur_Click(object sender, EventArgs e)
        {
            // Implement disabling selected serveur
            this.selectedServeur.activite = !this.selectedServeur.activite;

            context.serveurs.Find(this.selectedServeur.id_serveur).activite = this.selectedServeur.activite;
            context.SaveChanges();
            Invalidate();

        }

        //private Serveur draggingServeur;
        //private Point initialMousePosition;

        //private void ServeurDrawer_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        MessageBox.Show($"Initial mouse position = {initialMousePosition}");
        //        draggingServeur = GetClickedServeur(e.Location);
        //        if (draggingServeur != null)
        //        {
        //            initialMousePosition = e.Location;
        //            //this.Close();
        //        }
        //    }
        //}

        //private void ServeurDrawer_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (draggingServeur != null && e.Button == MouseButtons.Left)
        //    {
        //        // Calculate the difference between current mouse position and initial mouse position
        //        int deltaX = e.X - initialMousePosition.X;
        //        int deltaY = e.Y - initialMousePosition.Y;

        //        // Update the position of the dragging serveur
        //        draggingServeur.x += deltaX;
        //        draggingServeur.y += deltaY;

        //        // Update the initial mouse position
        //        initialMousePosition = e.Location;

        //        // Redraw the canvas
        //        Invalidate();
        //    }
        //}

        //private void ServeurDrawer_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        draggingServeur = null;
        //    }
        //}

    }
}
