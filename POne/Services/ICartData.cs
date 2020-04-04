using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Services
{
    public interface ICartData
    {
        public int Size { get; }

        public void Remove();

        public void Add(int x);

        public int this[int i] { get; set; }
    }
}
