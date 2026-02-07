using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assignment1
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection WeaponCollection;
        private string inputPath;
        private string outputPath;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);
            WeaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            if (File.Exists(outputPath)) HelperDelete(outputPath);
            if (File.Exists("output.json")) HelperDelete("output.json");
            if (File.Exists("output.xml")) HelperDelete("output.xml");
        }

        public void HelperDelete(string path)
        {
            try { File.Delete(path); } catch {}
        }

        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            WeaponCollection.Load(inputPath);
            Assert.AreEqual(48, WeaponCollection.GetHighestBaseAttack());
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            WeaponCollection.Load(inputPath);
            Assert.AreEqual(23, WeaponCollection.GetLowestBaseAttack());
        }

        [TestCase(WeaponType.Sword, 21)]
        [TestCase(WeaponType.Polearm, 14)]
        [TestCase(WeaponType.Claymore, 20)]
        [TestCase(WeaponType.Catalyst, 20)]
        [TestCase(WeaponType.Bow, 20)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(WeaponType type, int expectedValue)
        {
            WeaponCollection.Load(inputPath);
            Assert.AreEqual(expectedValue, WeaponCollection.GetAllWeaponsOfType(type).Count);
        }

        [TestCase(5, 10)]
        [TestCase(4, 48)]
        [TestCase(3, 27)]
        [TestCase(2, 5)]
        [TestCase(1, 5)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            WeaponCollection.Load(inputPath);
            Assert.AreEqual(expectedValue, WeaponCollection.GetAllWeaponsOfRarity(stars).Count);
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            Assert.IsTrue(WeaponCollection.Load(inputPath));
            Assert.AreEqual(95, WeaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            Assert.IsFalse(WeaponCollection.Load("fake.csv"));
            Assert.AreEqual(0, WeaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            WeaponCollection.Load(inputPath);
            Assert.IsTrue(WeaponCollection.Save(outputPath));
            Assert.IsTrue(WeaponCollection.Load(outputPath));
            Assert.AreNotEqual(0, WeaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            WeaponCollection.Clear();
            Assert.IsTrue(WeaponCollection.Save(outputPath));
            Assert.IsTrue(WeaponCollection.Load(outputPath));
            Assert.AreEqual(0, WeaponCollection.Count);
        }

        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            Weapon expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            Weapon actual;

            Assert.IsTrue(Weapon.TryParse(line, out actual));
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.BaseAttack, actual.BaseAttack);
            Assert.AreEqual(expected.Image, actual.Image);
            Assert.AreEqual(expected.Rarity, actual.Rarity);
            Assert.AreEqual(expected.SecondaryStat, actual.SecondaryStat);
            Assert.AreEqual(expected.Passive, actual.Passive);
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // Invalid data format
            string line = "1,Bulbasaur,A,B,C,65,65";
            Weapon actual;
            Assert.IsFalse(Weapon.TryParse(line, out actual));
            Assert.IsNull(actual);
        }

        [Test]
        public void WeaponCollection_Load_JSON_Valid_95Items()
        {
            // First save to ensure we have a valid JSON
            WeaponCollection.Load(inputPath);
            string jsonPath = CombineToAppPath("output.json");
            Assert.IsTrue(WeaponCollection.Save(jsonPath));
            
            // Now load
            WeaponCollection.Clear();
            Assert.IsTrue(WeaponCollection.Load(jsonPath));
            Assert.AreEqual(95, WeaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_Load_XML_Valid_95Items()
        {
            // First save to ensure we have a valid XML
            WeaponCollection.Load(inputPath);
            string xmlPath = CombineToAppPath("output.xml");
            Assert.IsTrue(WeaponCollection.Save(xmlPath));
            
            // Now load
            WeaponCollection.Clear();
            Assert.IsTrue(WeaponCollection.Load(xmlPath));
            Assert.AreEqual(95, WeaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_SaveAsJSON_Empty()
        {
            WeaponCollection.Clear();
            string jsonPath = CombineToAppPath("output.json");
            Assert.IsTrue(WeaponCollection.Save(jsonPath));
            Assert.IsTrue(WeaponCollection.Load(jsonPath));
            Assert.AreEqual(0, WeaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_SaveAsXML_Empty()
        {
            WeaponCollection.Clear();
            string xmlPath = CombineToAppPath("output.xml");
            Assert.IsTrue(WeaponCollection.Save(xmlPath));
            Assert.IsTrue(WeaponCollection.Load(xmlPath));
            Assert.AreEqual(0, WeaponCollection.Count);
        }

        [Test]
        public void WeaponCollection_Load_InvalidFormat_0Items()
        {
            // Try loading non-existent or garbage
            Assert.IsFalse(WeaponCollection.Load("garbage.txt"));
            Assert.AreEqual(0, WeaponCollection.Count);
        }
    }
}
