using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obshepit_form_16.Models
{
    public class Product
    {
        public string Name { get;  }
        public int Code { get; }
        public string NameEi { get; }
        public int CodeEi { get;  }
        public double Price { get; }
      
        public Product(string name, int code, string nameEi, int codeEi, double price)
        {
            Name = name;
            Code = code;
            NameEi = nameEi;
            CodeEi = codeEi;
            Price = price;
        }
    }
}
