using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POne.Services
{
    public class CartData : ICartData
    {
        private List<int> data;

        public CartData()
        {
            data = new List<int>();
            Size = 0;
        }
        
        public int this[int i] { get => data[i]; set => data[i] = value; }

        public int Size { get; private set; }

        public void Add(int x)
        {
            Size++;
            data.Add(x);
        }

        public void Remove()
        {
            Size--;
            data.RemoveAt(Size-1);
        }
    }
}
