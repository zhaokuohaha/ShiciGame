using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Shici
{
    public class ShiciAns
    {
		[Key]
		public int sa_id { get; set; }
		public string sa_head { get; set; }
		public string sa_tail { get; set; }
		public string sa_title { get; set; }
		public string sa_author { get; set; }
    }
}
