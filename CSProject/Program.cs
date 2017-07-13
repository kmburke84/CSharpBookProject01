using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSProject {

    class Staff {
        private float hourlyRate;
        private int hWorked;

        public float TotalPay {

            get; protected set;

        }

        public float BasicPay {

            get; private set;

        }

        public String NameOfStaff {

            get; private set;

        }

        public int HoursWorked {
            get {
                return hWorked;
            }
            set {
                if (value > 0) {
                    hWorked = value;
                }
                else {
                    hWorked = 0;
                }
            }

        }

        public Staff(String name, float rate) {
            this.NameOfStaff = name;
            this.hourlyRate = rate;
        }

        public virtual void CalculatePay() {
            Console.WriteLine("Calculating Pay...");
            BasicPay = hWorked * hourlyRate;
            TotalPay = BasicPay;
        }

        public override string ToString() {
            return "Name of Staff: " + NameOfStaff + "hourly rate: " + hourlyRate + "hours worked: " + HoursWorked +
                "Basic Pay: " + BasicPay + "TotalPay: " + TotalPay;
        }

    }

    class Manager : Staff {
        private const float managerHourlyRate = 50;

        public Manager(string name) : base(name, managerHourlyRate) {

        }



        public int Allowance {
            get; private set;
        }

        public override void CalculatePay() {
            base.CalculatePay();
            Allowance = 1000;

            if (HoursWorked > 160) {
                TotalPay = BasicPay + Allowance;
            }

        }

        public override string ToString() {
            return "Name of Staff: "+ NameOfStaff + "Manager hourly rate: " + managerHourlyRate + "Hours Worked: "
                + HoursWorked + "Basic Pay: " + BasicPay + "Allowance: " + Allowance + "Total Pay: "+ TotalPay;
        }

    }

    class Admin : Staff {
        private const float overtimeRate = 15;
        private const float adminHourlyRate = 30;
        public float Overtime {
            get; private set;
        }

        public Admin(String name) : base(name, adminHourlyRate) {

        }

        public override void CalculatePay() {
            base.CalculatePay();
            

            if (HoursWorked > 160) {
                Overtime = overtimeRate * (HoursWorked - 160);
            }
        }

        public override string ToString() {
            return "Name of Staff: " + NameOfStaff + "Admin hourly rate: " + adminHourlyRate + "Hours Worked: "
                + HoursWorked + "Basic Pay: " + BasicPay +  "Total Pay: " + TotalPay;
        }

    }

    class FileReader {
        public List<Staff> ReadFile() {
            List<Staff> myStaff = new List<Staff>();
            string[] result = new string[2];
            string path = "staff.txt";
            string[] seperator = { ", " };

            if (File.Exists(path)) {
                using (StreamReader sr = new StreamReader(path)) {
                    while (sr.EndOfStream != true) {
                        result = (sr.ReadLine().Split(','));

                        if (result[1] == "Manager") {
                            myStaff.Add(new Manager(result[0]));
                        }
                        else if (result[1] == "Admin") {
                           
                            myStaff.Add(new Admin(result[0]));
                        }
                        else {
                        }
                    }

                    sr.Close();
                }
            }
            else {
                Console.WriteLine("Error has occured");
            }
            return myStaff;


        }
    }

    class PaySlip {

        private int month;
        private int year;



        enum MonthsOfYear {
            JAN = 1, FEB = 2, MAR = 3, APR = 4, MAY = 5, JUN = 6,
            JUL = 7, AUG = 8, SEP = 9, OCT = 10, NOV = 11, DEC = 12
        }

        public PaySlip(int payMonth, int payYear) {

            this.month = payMonth;
            this.year = payYear;

        }

        public void GeneratePaySlip(List<Staff> myStaff) {
            string path;

            foreach (Staff f in myStaff) {
                path = f.NameOfStaff + ".txt";

                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine("PAYSLIP FOR " + (MonthsOfYear)month, year);
                sw.WriteLine("========================================");
                sw.WriteLine("Name of Staff: " + f.NameOfStaff);
                sw.WriteLine("Hours Worked: " + f.HoursWorked);
                sw.WriteLine("");
                sw.WriteLine("Basic Pay: " + f.BasicPay.ToString("C"));
                if (f.GetType() == typeof(Manager)) {
                    sw.WriteLine("Allowance: " + ((Manager)f).Allowance);
                }
                else if (f.GetType() == typeof(Admin)) {
                    sw.WriteLine("Overtime: " + ((Admin)f).Overtime);
                }
                sw.WriteLine("");
                sw.WriteLine("=========================================");
                sw.WriteLine("Total Pay: " + f.TotalPay);
                sw.WriteLine("=========================================");
                sw.Close();
            }
        }

        public void GenerateSummary(List<Staff> myStaff) {
            var result =
                from s in myStaff
                where s.HoursWorked < 10
                orderby s.NameOfStaff ascending
                select new {
                    s.NameOfStaff,
                    s.HoursWorked
                };


            var path = "summary.txt";

            using (StreamWriter sw = new StreamWriter(path)) {

                sw.WriteLine("Staff with less than 10 working hours");
                sw.WriteLine("");
                foreach (var s in result) {
                    sw.WriteLine("Name of Staff: " + s.NameOfStaff + ", Hours Worked: " + s.HoursWorked);
                }
                sw.Close();
            }
        }
        public override string ToString() {
            return "month: " + month + "year: " + year ;     3
        }




    }

    class Program {

        static void Main(string[] args) {
            List<Staff> myStaff = new List<Staff>();

            FileReader fr = new FileReader();
            int month = 0;
            int year = 0;
            
            while (year == 0) {
                Console.Write("\nPlease enter the year: ");

                try {
                    //Code to convert the input to an integer
                    
                    year = Convert.ToInt32(Console.ReadLine());

                }
                catch (Exception e) {
                    //Code to handle the exception
                    Console.Write(e.Message + "Incorrect Input");

                }



            }
            while (month == 0) {
                Console.Write("\nPlease enter a month: ");

                try {
                    //Code to convert the input to an integer
                    
                    month = Convert.ToInt32(Console.ReadLine());

                    if (month < 1 || month > 12) {
                        Console.Write("Month is invalid. Try again");
                        month = 0;

                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message + "Error, try again");
                }
            }

            myStaff = fr.ReadFile();

            for (int i = 0; i < myStaff.Count; i++) {
                try {
                    Console.WriteLine("Enter hours worked for " + myStaff[i].NameOfStaff + ":");
                    
                    myStaff[i].HoursWorked = Convert.ToInt32(Console.ReadLine());
                    myStaff[i].CalculatePay();
                    Console.WriteLine(myStaff.ToString());
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                    i--;
                }
            }
            PaySlip ps = new PaySlip(month, year);

            ps.GeneratePaySlip(myStaff);
            ps.GenerateSummary(myStaff);
            Console.Read();
        }

        
    }
}
