namespace ReflectionSerialization.TestData
{
    internal class Person
    {
        public string FirstName;
        public string LastName;
        public int Age;
        public double Height;
        public DateTime DateOfBirth;

        public string FullName => $"{FirstName} {LastName}";
        public bool IsAdult => Age >= 18;
        public decimal Salary { get; set; }
        public bool IsEmployed { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public int YearsExperience { get; set; }

        public Person()
        {
            FirstName = "John";
            LastName = "Doe";
            Age = 30;
            Height = 180.5;
            DateOfBirth = new DateTime(1993, 5, 15);
            Salary = 50000;
            IsEmployed = true;
            Street = "123 Main St";
            City = "Metropolis";
            ZipCode = "12345";
            Title = "Software Developer";
            Company = "Tech Corp";
            YearsExperience = 5;
        }
    }
}
