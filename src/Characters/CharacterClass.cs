using System;
using System.Collections.Generic;

namespace PoolOfRadiance.Characters
{
    /// <summary>
    /// Represents a character class in the game.  Converted from the previous
    /// enum so that additional metadata and behaviors may be added later.
    /// Instances are exposed as static readonly fields for the core classes.
    /// </summary>
    public sealed class CharacterClass : IEquatable<CharacterClass>
    {
        public string Name { get; }

        private CharacterClass(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;

        public override bool Equals(object? obj) => Equals(obj as CharacterClass);
        public bool Equals(CharacterClass? other) => other != null && Name == other.Name;
        public override int GetHashCode() => Name.GetHashCode(StringComparison.Ordinal);

        public static bool operator ==(CharacterClass? left, CharacterClass? right) =>
            ReferenceEquals(left, right) || (left is not null && left.Equals(right));

        public static bool operator !=(CharacterClass? left, CharacterClass? right) => !(left == right);

        // Core classes
        public static readonly CharacterClass Fighter = new("Fighter");
        public static readonly CharacterClass Ranger = new("Ranger");
        public static readonly CharacterClass Paladin = new("Paladin");
        public static readonly CharacterClass Cleric = new("Cleric");
        public static readonly CharacterClass MagicUser = new("MagicUser");
        public static readonly CharacterClass Thief = new("Thief");

        public static IEnumerable<CharacterClass> Values
            => [Fighter, Ranger, Paladin, Cleric, MagicUser, Thief];
    }
}
