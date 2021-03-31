using System;
using System.Collections.Generic;
using System.Text;

namespace ModelService
{
    public class Captch
    {
        public string Id { get; set; }
        public string Capchid { get; set; }
        public int IsActive { get; set; } //1-active 0-deactive
    }
}
