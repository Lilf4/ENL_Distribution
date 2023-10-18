using Bogus;
using Fare;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ENF_Dist_Test.Validators {

    public class fakerGenerateInfo {
        public int minProducts { get; set; } = 1;
        public int maxProducts { get; set; } = 500;
        public int maxProdQuantity { get; set; } = 1000;

        public int minEmployees { get; set; } = 5;
        public int maxEmployees { get; set; } = 300;

        public int minOrdPerEmployee { get; set; } = 0;
        public int maxOrdPerEmployee { get; set; } = 20;
        public int maxOrdQuantity { get; set; } = 400;
    }
}
