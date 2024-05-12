using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSimulation.Entities
{
    public class UrlMapping
    {
        public UrlMapping(int id_mapping, int id_serveur, string url)
        {
            this.id_mapping = id_mapping;
            this.id_serveur = id_serveur;
            this.url = url;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_mapping { get; set; }

        public int id_serveur { get; set; }

        public string url { get; set; }

        [ForeignKey("id_serveur")]
        public Serveur serveur { get; set; }
    }
}
