using System;

namespace TicTacToe
{
    public class Player
    {
        public Player(string name, int code, string token)
        {
            if (code != 1 && code != -1 && code != 0)
                throw new InvalidOperationException("This is an invalid code!");

            Name = name;
            Code = code;
            Token = token;
        }

        public string Name { get; private set; }
        public int Code { get; private set; }
        public string Token { get; private set; }

        protected bool Equals(Player other)
        {
            return Code == other.Code;
        }

        public override int GetHashCode()
        {
            return Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Player) obj);
        }

        public override string ToString()
        {
            return Token;
        }
    }
}