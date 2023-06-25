using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Core.Models
{
    public class Category : BaseEntity
    { // ? nullable C#8 ve üzeri için kodlarken ref tipli proplarda null olabilir bilgisini kodlarken görebilmek için
        public string? Name { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
