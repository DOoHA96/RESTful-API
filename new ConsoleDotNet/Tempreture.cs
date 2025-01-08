namespace new_ConsoleDotNet
{
    class Tempreture : IComparable
    {
        public int Value { get; private set; }

        public Tempreture(int value)
        {
            Value = value;
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;

            var temp = (Tempreture)obj;

            if (temp is null) throw new ArgumentException("Object is not a Tempreture object");

            return this.Value.CompareTo(temp.Value);


        }
    }

        

}
