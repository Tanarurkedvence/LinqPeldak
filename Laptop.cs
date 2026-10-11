using System;
using System.Collections.Generic;
using System.Text;

namespace LinqPeldak
{
    internal class Laptop
    {
        static int counter = 1;
        public Laptop(string l)
        {
            var r = l.Split(";");
            Category = new Category()
            {
                CategoryCode = int.Parse(r[0]),
                CategoryName = r[1]
            };
            CPU = r[2];
            Manufacturer = new Manufacturer()
            {
                ManufacturerCode = int.Parse(r[3]),
                ManufacturerName = r[4]
            };
            Model = r[5];
            OS = r[6];
            Price = double.Parse(r[7]);
            RAM = int.Parse(r[8]);
            Screen = r[9];
            Storage = r[10];
            ID = counter++;
        }

        public Category Category { get; set; }
        public string CPU { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public string Model { get; set; }
        public string OS { get; set; }
        public double Price { get; set; }
        public int RAM { get; set; }
        public string Screen { get; set; }
        public string Storage { get; set; }
        public int ID { get; set; }


        public override string ToString()
        {
            return $"{ID}. {Category.CategoryName} | {Manufacturer.ManufacturerName} {Model} | {OS} | {Math.Round((Price * 4.12), 0)} HUF";
        }
    }
}
