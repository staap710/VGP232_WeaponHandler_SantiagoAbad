using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1
{
    public enum WeaponType
    {
        Sword,
        Polearm,
        Claymore,
        Catalyst,
        Bow,
        None
    }

    public class Weapon
    {
        // Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive
        public string Name { get; set; }
        public WeaponType Type { get; set; }
        public string Image { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }
        public string SecondaryStat { get; set; }
        public string Passive { get; set; }

        public static int CompareByName(Weapon left, Weapon right)
        {
            return left.Name.CompareTo(right.Name);
        }

        public static int CompareByType(Weapon left, Weapon right)
        {
            return left.Type.CompareTo(right.Type);
        }

        public static int CompareByRarity(Weapon left, Weapon right)
        {
            return left.Rarity.CompareTo(right.Rarity);
        }

        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            return left.BaseAttack.CompareTo(right.BaseAttack);
        }

        public override string ToString()
        {
            // Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive
            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        public static bool TryParse(string line, out Weapon weapon)
        {
            weapon = null;
            if (string.IsNullOrWhiteSpace(line)) return false;

            string[] values = line.Split(',');
            // We expect 7 columns for data2.csv
            if (values.Length != 7) return false;

            try 
            {
                weapon = new Weapon();
                weapon.Name = values[0];
                
                if (!Enum.TryParse(values[1], true, out WeaponType type))
                {
                    weapon = null;
                    return false;
                }
                weapon.Type = type;

                weapon.Image = values[2];
                
                if (!int.TryParse(values[3], out int rarity))
                {
                    weapon = null;
                    return false;
                }
                weapon.Rarity = rarity;

                if (!int.TryParse(values[4], out int baseAttack))
                {
                    weapon = null;
                    return false;
                }
                weapon.BaseAttack = baseAttack;

                weapon.SecondaryStat = values[5];
                weapon.Passive = values[6];

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
