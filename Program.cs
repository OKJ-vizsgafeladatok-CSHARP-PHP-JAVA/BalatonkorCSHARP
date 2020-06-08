using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BalatonkorCSHARP
{
    class Telepules
    {
        public string telepules { get; set; }
        public int elso { get; set; }
        public int masodik { get; set; }
        public int harmadik { get; set; }

        public Telepules(string telepules, int elso, int masodik, int harmadik)
        {
            this.telepules = telepules;
            this.elso = elso;
            this.masodik = masodik;
            this.harmadik = harmadik;
        }
    }

    class Program
    {
        static public List<Telepules> lista = beolvasTelep();

        static public List<Telepules> beolvasTelep()
        {
            List<Telepules> list = new List<Telepules>();
            try
            {
                using (StreamReader sr = new StreamReader(new FileStream("kerekpar.csv", FileMode.Open), Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var split = sr.ReadLine().Split(';');
                        Telepules o = new Telepules(split[0], Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3]));
                        list.Add(o);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a beolvasáskor. " + e.Message);
            }
            return list;
        }

        static public int beolvasVers()
        {
            int vers = 0;
            try
            {
                using (StreamReader sr = new StreamReader(new FileStream("kerekpar.csv", FileMode.Open), Encoding.UTF8))
                {
                    vers = Convert.ToInt32(sr.ReadLine());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a beolvasásnál. " + ex.Message);
            }
            return vers;
        }

        static void Main(string[] args)
        {
            #region 1. feladat
            Console.WriteLine("1. feladat: Teljesítve, a fájl beolvasva.");
            #endregion

            #region 2. feladat
            Console.WriteLine("2. feladat:\n\tA helyszínek száma: {0} db", lista.Count());
            #endregion

            #region 3. feladat
            Console.WriteLine("3. feladat: A versenysorozat teljes hossza: {0} km.",
                lista.Sum(x => x.elso + x.masodik + x.harmadik));
            #endregion

            #region 4. feladat
            Console.WriteLine("4. feladat:");
            Console.Write("\tAdjon meg egy (balatoni) városnevet: ");
            var beker = Console.ReadLine();
            var varosom = lista.SingleOrDefault(x => x.telepules.ToUpper().Equals(beker.ToUpper()));
            if (varosom != null)
            {
                Console.WriteLine($"\tAz adott város versenyszakaszai: {varosom.elso} km, {varosom.masodik} km, {varosom.harmadik} km");
            }
            else
            {
                Console.WriteLine("\tEz a város nem szerepel a verseny állomásai között!");
            }
            #endregion

            #region 5. feladat
            Console.WriteLine("5. feladat: ");
            var leghossz = lista
                .Select(x => new
                {
                    telep = x.telepules,
                    sum = x.elso + x.masodik + x.harmadik
                })
                .ToList()
                .OrderByDescending(y => y.sum)
                .First();

            Console.WriteLine("\tA leghosszabb versenytávot adó település: {0}", leghossz.telep);
            #endregion

            #region 6. feladat
            var atl = lista.Average(x => x.elso);
            Console.WriteLine("6. feladat Az első szakaszok átlagos hossza: {0} km", Math.Round(atl, 1));
            #endregion

            #region 7. feladat
            var versenyOsszHossz = lista.Sum(x => x.elso + x.masodik + x.harmadik);
            var telepListaHosszal = lista
                .Select(x => new
                {
                    telep = x.telepules,
                    sum = x.elso + x.masodik + x.harmadik
                })
                .ToList();
            using (StreamWriter sw = new StreamWriter(new FileStream("statisztika.txt", FileMode.Create), Encoding.UTF8))
            {
                telepListaHosszal.ForEach(x =>
                {
                    var szazalek = x.sum / (double)versenyOsszHossz * 100;
                    sw.WriteLine(x.telep + "-" + Math.Round(szazalek) + "%");
                });
            }
            Console.WriteLine("7. feladat: A fájl létrehozva.");
            #endregion
            #region 8. feladat
            var atlv = beolvasVers() / (double)lista.Count();
            Console.WriteLine("8. feladat: Az átlagos versenyzőszám: " + Math.Round(atlv));
            #endregion
            Console.ReadKey();
        }
    }
}
