using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract.Option_Pattern
{
    public class Config_OptionPattern
    {
        [Required(ErrorMessage = "The symbol can't be blank.")]
        public string? symbol { get; set; }
        [Required(ErrorMessage = "The Token can't be blank.")]
        public string? token { get; set; }
    }
}
