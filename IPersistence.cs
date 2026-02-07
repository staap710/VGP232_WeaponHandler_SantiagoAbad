using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    public interface IPersistence
    {
        bool Load(string filename);
        bool Save(string filename);
    }
}
