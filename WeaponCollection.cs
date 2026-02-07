using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Serialization;

namespace Assignment1
{
    public class WeaponCollection : List<Weapon>, IPersistence
    {
        public bool Load(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return false;
            if (!File.Exists(filename)) return false;

            string ext = Path.GetExtension(filename).ToLower();
            switch (ext)
            {
                case ".csv": return LoadCsv(filename);
                case ".json": return LoadJson(filename);
                case ".xml": return LoadXml(filename);
                default: return false;
            }
        }

        public bool Save(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return false;

            string ext = Path.GetExtension(filename).ToLower();
            switch (ext)
            {
                case ".csv": return SaveCsv(filename);
                case ".json": return SaveJson(filename);
                case ".xml": return SaveXml(filename);
                default: return false;
            }
        }

        private bool LoadCsv(string filename)
        {
            try
            {
                Clear();
                using (StreamReader reader = new StreamReader(filename))
                {
                    string header = reader.ReadLine();
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        Weapon weapon;
                        if (Weapon.TryParse(line, out weapon))
                        {
                            Add(weapon);
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool SaveCsv(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");
                    foreach (var weapon in this)
                    {
                        writer.WriteLine(weapon.ToString());
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool LoadJson(string filename)
        {
            try
            {
                string json = File.ReadAllText(filename);
                List<Weapon> loaded = JsonSerializer.Deserialize<List<Weapon>>(json);
                if (loaded != null)
                {
                    Clear();
                    AddRange(loaded);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool SaveJson(string filename)
        {
            try
            {
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filename, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool LoadXml(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Weapon>));
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    List<Weapon> loaded = (List<Weapon>)serializer.Deserialize(fs);
                    if (loaded != null)
                    {
                        Clear();
                        AddRange(loaded);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool SaveXml(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Weapon>));
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    serializer.Serialize(fs, this.ToList()); // Serialize as List<Weapon>
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SortBy(string columnName)
        {
            switch (columnName)
            {
                case "Name":
                    Sort(Weapon.CompareByName);
                    break;
                case "Type":
                    Sort(Weapon.CompareByType);
                    break;
                case "Rarity":
                    Sort(Weapon.CompareByRarity);
                    break;
                case "BaseAttack":
                    Sort(Weapon.CompareByBaseAttack);
                    break;
                case "Passive":
                     Sort((left, right) => left.Passive.CompareTo(right.Passive));
                    break;
                case "SecondaryStat":
                     Sort((left, right) => left.SecondaryStat.CompareTo(right.SecondaryStat));
                    break;
                default:
                    Sort(Weapon.CompareByName);
                    break;
            }
        }

        public int GetHighestBaseAttack()
        {
            if (Count == 0) return 0;
            return this.Max(w => w.BaseAttack);
        }

        public int GetLowestBaseAttack()
        {
            if (Count == 0) return 0;
            return this.Min(w => w.BaseAttack);
        }

        public List<Weapon> GetAllWeaponsOfType(WeaponType type)
        {
            return this.Where(w => w.Type == type).ToList();
        }

        public List<Weapon> GetAllWeaponsOfRarity(int rarity)
        {
            return this.Where(w => w.Rarity == rarity).ToList();
        }
    }
}
