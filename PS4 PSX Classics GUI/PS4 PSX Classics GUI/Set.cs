using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helpers
{
    /*
    Ideas:
    public SetOf Alphabet = {'A'..'z'};
    public SetOf SmallNums = {0..9};
    public SetOf ValidStrings = {"Test", "Test2", "Test3"};
    public enum CardName { Ace, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
    public SetOf<CardName> Cards;


    Good ideas: (Will be implemented)
    public SetOf<Char> Alphabet = {'A'..'z'};
    public SetOf<int> SmallNums = {0..9};
    public SetOf<string> ValidStrings = {"Test", "Test2", "Test3"};
    public enum CardName { Ace, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
    public SetOf<CardName> Cardset;
    public SetOf<CardName> Cards = { Ace, Three, Six, Seven, King };


    Bad ideas:
    public SetOf<'A', 'z'> Alphabet;
    public SetOf<0, 9> SmallNums;
    public SetOf<"Test", "Test2", "Test3"> ValidStrings;
    public enum CardName { Ace, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King };
    public SetOf<CardName> Cards;
    */
    public struct Set<T>
    {
        private static Array _enumValues = Enum.GetValues(typeof(T));
        private List<T> _values;

        public Set(T startingvalue)
        {
            _values = new List<T>();
            _values.Add(startingvalue);
        }

        public Set(params T[] startingvalues)
        {
            _values = new List<T>();
            _values.AddRange(startingvalues);
        }

        private List<T> Values
        {
            get
            {
                if (_values == null)
                {
                    _values = new List<T>();
                }

                return _values;
            }
        }

        private static Set<T> GetMatchingSet(int x)
        {
            Set<T> result = new Set<T>();

            for (int i = 0; i < _enumValues.Length; i++)
            {
                object eo = _enumValues.GetValue(i);
                int ei = (int)Convert.ChangeType(eo, typeof(int));

                if ((x & ei) == ei)
                {
                    // T item = (T)Enum.ToObject(typeof(T), i);
                    result.Values.Add((T)eo);
                }
                // T item = (T)i;
                // result += (int)Convert.ChangeType(val, typeof(int));
            }

            return result;
        }

        public static implicit operator Set<T>(T x)
        {
            return GetMatchingSet((int)Convert.ChangeType(x, typeof(int)));
        }

        // Implicit conversion from int to Set<T>
        public static implicit operator Set<T>(int x)
        {
            return GetMatchingSet(x);
        }

        // Equality operator. Returns true if both operands match.
        public static bool operator ==(Set<T> x, Set<T> y)
        {
            try
            {
                Set<T> sety = (Set<T>)y;

                if (x.Values.Count != sety.Values.Count)
                    return false;

                foreach (T val in x.Values)
                {
                    if (!sety.Values.Contains(val))
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        // Inequality operator. Returns true if operands doesn't match.
        public static bool operator !=(Set<T> x, Set<T> y)
        {
            if (x == null && y == null)
                return true;
            return !(x == y);
        }

        // Logical negation operator. Pretty much returns a flipped version of Set<T>.
        public static Set<T> operator !(Set<T> x)
        {
            Set<T> result = new Set<T>();

            for (int i = 0; i < _enumValues.Length; i++)
            {
                object eo = _enumValues.GetValue(i);
                int ei = (int)Convert.ChangeType(eo, typeof(int));

                if ((x == null) || (!x._values.Contains((T)eo)))
                {
                    result.Values.Add((T)eo);
                }
            }

            return result;
        }

        // Logical AND operator. Returns a new set containing the union between the current set and the value.
        public static Set<T> operator &(Set<T> x, T y)
        {
            if (x != null)
            {
                Set<T> sety = GetMatchingSet((int)Convert.ChangeType(y, typeof(int)));
                return x & sety;
            }

            return new Set<T>();
        }

        // Logical AND operator. Returns a new set containing the union between the current set and the value.
        public static Set<T> operator &(Set<T> x, Set<T> y)
        {
            Set<T> result = new Set<T>();

            if (x != null)
            {
                foreach (T val in y.Values) // TODO: Change to just y not y.Values
                {
                    if (x.Values.Contains(val))
                    {
                        result.Values.Add(val);
                    }
                }
            }

            return result;
        }

        // Logical OR operator. Returns a new set containing the intersect between the current set and the value.
        public static Set<T> operator |(Set<T> x, T y)
        {
            /*
            Set<T> result = new Set<T>();
            if (x != null)
            {
                result.Values.AddRange(x.Values);
            }
            */
            Set<T> sety = GetMatchingSet((int)Convert.ChangeType(y, typeof(int)));
            return x | sety;
        }

        // Logical OR operator. Returns a new set containing the intersect between the current set and the value.
        public static Set<T> operator |(Set<T> x, Set<T> y)
        {
            Set<T> result = new Set<T>();
            if (x != null)
            {
                result.Values.AddRange(x.Values);
            }

            foreach (T val in y.Values) // TODO: Change to just y not y.Values
            {
                if (!result.Values.Contains(val))
                {
                    result.Values.Add(val);
                }
            }

            return result;
        }

        /*
        // Definitely true operator. Returns true if the operand is 
        // dbTrue, false otherwise:
        public static bool operator true(DBBool x)
        {
            return x.value > 0;
        }

        // Definitely false operator. Returns true if the operand is 
        // dbFalse, false otherwise:
        public static bool operator false(DBBool x)
        {
            return x.value < 0;
        }

        // Overload the conversion from DBBool to string:
        public static implicit operator string(DBBool x)
        {
            return x.value > 0 ? "dbTrue"
                 : x.value < 0 ? "dbFalse"
                 : "dbNull";
        }
        */

        // Overload the conversion from Set to int:
        // (?) public static explicit operator int(Set<T> x)
        public static implicit operator int(Set<T> x)
        {
            int result = 0;
            if (x != null)
            {
                foreach (T val in x.Values)
                {
                    int intval = (int)Convert.ChangeType(val, typeof(int));
                    result = result | intval;
                }
            }

            return result;
        }

        // Override the Object.Equals(object o) method:
        public override bool Equals(object o)
        {
            return this == (Set<T>)o;
        }

        // Override the Object.GetHashCode() method:
        public override int GetHashCode()
        {
            return (int)this;
        }

        /*
        // Override the ToString method to convert DBBool to a string:
        public override string ToString()
        {
            switch (value)
            {
                case -1:
                    return "DBBool.False";
                case 0:
                    return "DBBool.Null";
                case 1:
                    return "DBBool.True";
                default:
                    throw new InvalidOperationException();
            }
        }
        */
    }
}
