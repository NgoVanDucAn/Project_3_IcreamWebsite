using C2108G1_Project_3.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace C2108G1_Project_3.Models

{
    public class Flavors : Base
    {

        public string? Name { get; set; }
        public string? thumbnail { get; set; }

    }
}
