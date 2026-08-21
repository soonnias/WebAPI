using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Size
    {
        private string _value;

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Value
        {
            get => _value;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("Size value cannot be empty or null.");
                if (value.Length > 20)
                    throw new InvalidOperationException("Size value cannot exceed 20 characters.");
                _value = value;
            }
        }

        public bool IsAvailable { get; set; } = true;

        public string SizeTypeId { get; set; }
        public SizeType SizeType { get; set; }

        // Навігаційна властивість для зв'язку з продуктами
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
