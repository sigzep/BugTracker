using System;
using Cars3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cars3.Models
{
    public class Car
    {
        //[DisplayName("The Make")]
        public string Make { get; set; }
        //[DisplayName("The Model")]
        public string Model { get; set; }
        //[DisplayName("The Year")]
        public string Year { get; set; }
        //[DisplayName("The External Color")]
        public string ExternalColor { get; set; }
        //[DisplayName("The Interior Color")]
        public string InteriorColor { get; set; }
    }
}