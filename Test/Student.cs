using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public class Student
    {
        public string name { get; set; }

        public Address address { get; set; }

        public Subjects[] subjects { get; set; }
    }

    public class Address
    {
        public string country { get; set; }

        public string city { get; set; }
    }

    public class Subjects
    {
        public string name { get; set; }
    }
}
