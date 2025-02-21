using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3console.Models.EntityFramework
{
    public partial class Avi
    {
        public override string? ToString()
        {
            return "Note : " + this.Note;
        }
    }
}
