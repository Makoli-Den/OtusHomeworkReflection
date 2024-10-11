namespace ReflexionSerialization
{
    internal class F
    {
        public int i1, i2, i3, i4, i5;

        public static F Get() => new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 };

        public static F GetRandom()
        {
            Random random = new Random();
            return new F()
            {
                i1 = random.Next(-101, 101),
                i2 = random.Next(-101, 101),
                i3 = random.Next(-101, 101),
                i4 = random.Next(-101, 101),
                i5 = random.Next(-101, 101)
            };
        }
    }
}
