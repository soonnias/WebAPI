using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class SizeType
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } // Наприклад: "Одяг", "Взуття"

        // Навігаційна властивість
        public ICollection<Size> Sizes { get; set; } = new List<Size>();
    }

}
