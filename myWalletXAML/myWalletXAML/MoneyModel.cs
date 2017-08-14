using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace myWalletXAML
{
    public class MoneyModel
    {
        public int ID { get; set; }
        public float Value { get; set; }
        public string Name { get; set; }
        public string wType { get; set; }
        public Color wColor { get; set; }
    }
}
