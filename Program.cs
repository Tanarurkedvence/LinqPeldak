namespace LinqPeldak
{
    // LINQ példatár: minden fontos lekérdezés a laptops.txt valódi adatain.
    // Minden szakasz kiírja a saját számát és címét, hogy a kimenetben könnyű legyen megtalálni.
    internal class Program
    {
        static void Cim(string szoveg)
        {
            Console.WriteLine();
            Console.WriteLine($"===== {szoveg} =====");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;   // ékezetek a konzolon

            List<Laptop> laptops = new List<Laptop>();
            using (StreamReader o = new StreamReader("../../../forras/laptops.txt"))
            {
                o.ReadLine();
                while (!o.EndOfStream)
                {
                    laptops.Add(new Laptop(o.ReadLine()));
                }
            }
            Console.WriteLine($"Beolvasva: {laptops.Count} laptop");

            // ---------------------------------------------------------------- SZŰRÉS
            Cim("01. Where – egy feltétel");
            var gaming = laptops.Where(x => x.Category.CategoryName == "Gaming").ToList();
            Console.WriteLine($"Gaming laptopok: {gaming.Count} db");

            Cim("02. Where – több feltétel (ÉS: &&)");
            var i7ssd = laptops.Where(x => x.CPU.Contains("Intel Core i7") && x.Storage.Contains("SSD")).ToList();
            Console.WriteLine($"i7 + SSD: {i7ssd.Count} db");

            Cim("03. Where – VAGY (||)");
            var appleVagyMsi = laptops.Where(x => x.Manufacturer.ManufacturerName == "Apple" || x.Manufacturer.ManufacturerName == "MSI").ToList();
            Console.WriteLine($"Apple vagy MSI: {appleVagyMsi.Count} db");

            Cim("04. Where – tartomány (min <= x <= max)");
            var kozepes = laptops.Where(x => x.RAM >= 8 && x.RAM <= 16).ToList();
            Console.WriteLine($"8–16 GB RAM: {kozepes.Count} db");

            Cim("05. Where – NEM (!) és nem egyenlő (!=)");
            var nemWindows = laptops.Where(x => !x.OS.Contains("Windows")).ToList();
            Console.WriteLine($"Nem Windows: {nemWindows.Count} db");
            foreach (var l in nemWindows.Take(3)) Console.WriteLine($"\t{l.Manufacturer.ManufacturerName} {l.Model} – {l.OS}");

            Cim("06. Where – kis/nagybetű független keresés");
            string keres = "zenbook";
            var zenbook = laptops.Where(x => x.Model.ToLower().Contains(keres.ToLower())).ToList();
            Console.WriteLine($"'{keres}' a modellnévben: {zenbook.Count} db");

            Cim("07. Where – StartsWith / EndsWith");
            var intelCpu = laptops.Count(x => x.CPU.StartsWith("Intel"));
            var ssdVeg = laptops.Count(x => x.Storage.EndsWith("SSD"));
            Console.WriteLine($"Intel CPU: {intelCpu} db, csak SSD-re végződő háttértár: {ssdVeg} db");

            Cim("08. Where – lista elemei közül (Contains egy listán)");
            var kertGyartok = new List<string> { "Dell", "Lenovo" };
            var dellLenovo = laptops.Count(x => kertGyartok.Contains(x.Manufacturer.ManufacturerName));
            Console.WriteLine($"Dell vagy Lenovo: {dellLenovo} db");

            Cim("09. Where – számolt értékre (ár forintban)");
            var dragaHuf = laptops.Where(x => x.Price * 4.12 > 400000).ToList();
            Console.WriteLine($"400 000 Ft felett: {dragaHuf.Count} db");

            Cim("10. Where – szövegből kivágott számra (képátló a Screen elejéből)");
            // Screen pl. "15.6 col - 1366x768" → "15.6" → 15,6
            var nagyKepernyo = laptops.Where(x => double.Parse(x.Screen.Split(" ")[0].Replace(".", ",")) >= 17).ToList();
            Console.WriteLine($"Legalább 17 colos: {nagyKepernyo.Count} db");

            // ---------------------------------------------------------------- RENDEZÉS
            Cim("11. OrderBy – növekvő");
            foreach (var l in laptops.OrderBy(x => x.Price).Take(3))
                Console.WriteLine($"\t{l.Manufacturer.ManufacturerName} {l.Model}: {l.Price:F0} INR");

            Cim("12. OrderByDescending – csökkenő");
            foreach (var l in laptops.OrderByDescending(x => x.Price).Take(3))
                Console.WriteLine($"\t{l.Manufacturer.ManufacturerName} {l.Model}: {l.Price:F0} INR");

            Cim("13. ThenBy – több szempont (RAM csökkenő, azon belül ár növekvő)");
            foreach (var l in laptops.OrderByDescending(x => x.RAM).ThenBy(x => x.Price).Take(3))
                Console.WriteLine($"\t{l.RAM} GB – {l.Price:F0} INR – {l.Manufacturer.ManufacturerName} {l.Model}");

            Cim("14. Rendezés név szerint (ABC)");
            foreach (var g in laptops.Select(x => x.Manufacturer.ManufacturerName).Distinct().OrderBy(x => x).Take(5))
                Console.WriteLine($"\t{g}");

            // ---------------------------------------------------------------- KIVÁLASZTÁS
            Cim("15. First / FirstOrDefault");
            var elsoApple = laptops.First(x => x.Manufacturer.ManufacturerName == "Apple");
            Console.WriteLine($"Első Apple: {elsoApple.Model}");
            var nincs = laptops.FirstOrDefault(x => x.RAM > 1000);
            Console.WriteLine($"1000 GB RAM feletti: {(nincs == null ? "nincs ilyen (null)" : nincs.Model)}");

            Cim("16. Last, ElementAt");
            Console.WriteLine($"Utolsó: {laptops.Last().Model}, 5. elem (index 4): {laptops.ElementAt(4).Model}");

            Cim("17. A legdrágább laptop (objektum) – 3 módszer");
            var legdragabb1 = laptops.OrderByDescending(x => x.Price).First();
            var legdragabb2 = laptops.MaxBy(x => x.Price);
            var legdragabb3 = laptops.First(x => x.Price == laptops.Max(y => y.Price));
            Console.WriteLine($"\t{legdragabb1.Model} | {legdragabb2.Model} | {legdragabb3.Model}");

            Cim("18. Take / Skip – első N, lapozás");
            var top5olcso = laptops.OrderBy(x => x.Price).Take(5).Select(x => x.Model);
            Console.WriteLine($"5 legolcsóbb: {string.Join(", ", top5olcso)}");
            var masodikOtos = laptops.OrderBy(x => x.Price).Skip(5).Take(5).Select(x => x.Model);
            Console.WriteLine($"6–10.: {string.Join(", ", masodikOtos)}");

            Cim("19. TakeWhile / SkipWhile");
            var amigOlcso = laptops.OrderBy(x => x.Price).TakeWhile(x => x.Price < 15000).Count();
            Console.WriteLine($"Ár szerint rendezve, amíg 15000 INR alatt: {amigOlcso} db");

            // ---------------------------------------------------------------- ÖSSZESÍTÉS
            Cim("20. Count – összes és feltételes");
            Console.WriteLine($"Összes: {laptops.Count()}, Windows 10: {laptops.Count(x => x.OS == "Windows 10")}");

            Cim("21. Sum, Average, Min, Max");
            Console.WriteLine($"RAM összesen: {laptops.Sum(x => x.RAM)} GB");
            Console.WriteLine($"Átlagár: {laptops.Average(x => x.Price):F2} INR");
            Console.WriteLine($"Legkisebb RAM: {laptops.Min(x => x.RAM)} GB, legnagyobb: {laptops.Max(x => x.RAM)} GB");

            Cim("22. Összesítés szűrt listán (és üres lista kezelése!)");
            var appleLista = laptops.Where(x => x.Manufacturer.ManufacturerName == "Apple").ToList();
            if (appleLista.Any())
                Console.WriteLine($"Apple átlagár: {appleLista.Average(x => x.Price):F2} INR");
            var uresLista = laptops.Where(x => x.RAM > 1000).ToList();
            Console.WriteLine(uresLista.Any() ? $"{uresLista.Average(x => x.Price)}" : "Üres lista – Average() hibát dobna, ezért nem hívjuk meg.");

            Cim("23. Any / All");
            Console.WriteLine($"Van 32 GB-os? {laptops.Any(x => x.RAM == 32)} | Mindnek van legalább 2 GB RAM-ja? {laptops.All(x => x.RAM >= 2)}");

            Cim("24. Százalékos arány");
            double arany = (double)laptops.Count(x => x.Storage.Contains("SSD")) / laptops.Count * 100;
            Console.WriteLine($"SSD-s gépek aránya: {arany:F2}%");

            // ---------------------------------------------------------------- ÁTALAKÍTÁS
            Cim("25. Select – egy mező kigyűjtése");
            var modellek = laptops.Select(x => x.Model).Take(3).ToList();
            Console.WriteLine(string.Join(" | ", modellek));

            Cim("26. Select – számolt érték");
            var arakHuf = laptops.Select(x => Math.Round(x.Price * 4.12)).Take(3);
            Console.WriteLine(string.Join(" Ft, ", arakHuf) + " Ft");

            Cim("27. Select – névtelen objektum (több mező)");
            foreach (var x in laptops.Select(x => new { Nev = x.Manufacturer.ManufacturerName + " " + x.Model, Huf = Math.Round(x.Price * 4.12) }).Take(2))
                Console.WriteLine($"\t{x.Nev}: {x.Huf} Ft");

            Cim("28. Select indexszel (sorszám az eredményben)");
            foreach (var x in i7ssd.Select((l, i) => $"[{i + 1}] {l.Category.CategoryName} | {l.Model}").Take(3))
                Console.WriteLine($"\t{x}");

            Cim("29. Distinct – egyedi értékek");
            var gyartok = laptops.Select(x => x.Manufacturer.ManufacturerName).Distinct().ToList();
            Console.WriteLine($"{gyartok.Count} gyártó: {string.Join(", ", gyartok.OrderBy(x => x))}");

            Cim("30. Szövegből kivágott szám maximuma");
            // "Intel Core i7 7500U 2.7GHz" → utolsó szó "2.7GHz" → 2,7
            var maxGhz = laptops.Where(x => x.CPU.EndsWith("GHz"))
                                .Select(x => double.Parse(x.CPU.Split(" ").Last().Replace("GHz", "").Replace(".", ",")))
                                .Max();
            Console.WriteLine($"Legnagyobb órajel: {maxGhz} GHz");

            // ---------------------------------------------------------------- CSOPORTOSÍTÁS
            Cim("31. GroupBy – darabszám csoportonként");
            foreach (var g in laptops.GroupBy(x => x.Category.CategoryName).OrderByDescending(g => g.Count()))
                Console.WriteLine($"\t{g.Key}: {g.Count()} db");

            Cim("32. GroupBy – átlag, max csoportonként");
            var gyartoStat = laptops.GroupBy(x => x.Manufacturer.ManufacturerName)
                                    .Select(g => new { Gyarto = g.Key, Db = g.Count(), Atlag = g.Average(x => x.Price), Max = g.Max(x => x.Price) })
                                    .OrderByDescending(x => x.Atlag)
                                    .Take(3);
            foreach (var s in gyartoStat)
                Console.WriteLine($"\t{s.Gyarto}: {s.Db} db, átlag {s.Atlag:F0}, max {s.Max:F0} INR");

            Cim("33. GroupBy – a csoport legjobb eleme");
            foreach (var g in laptops.GroupBy(x => x.Category.CategoryName).OrderBy(g => g.Key))
            {
                var legolcsobb = g.OrderBy(x => x.Price).First();
                Console.WriteLine($"\t{g.Key}: {legolcsobb.Manufacturer.ManufacturerName} {legolcsobb.Model} ({legolcsobb.Price:F0} INR)");
            }

            Cim("34. GroupBy – csak a nagy csoportok (Where a csoportokra)");
            var nagyGyartok = laptops.GroupBy(x => x.Manufacturer.ManufacturerName).Where(g => g.Count() >= 40).Select(g => g.Key);
            Console.WriteLine($"Legalább 40 laptoppal: {string.Join(", ", nagyGyartok)}");

            Cim("35. GroupBy – két mező szerint");
            foreach (var g in laptops.GroupBy(x => new { x.Manufacturer.ManufacturerName, x.Category.CategoryName })
                                     .OrderByDescending(g => g.Count()).Take(3))
                Console.WriteLine($"\t{g.Key.ManufacturerName} / {g.Key.CategoryName}: {g.Count()} db");

            Cim("36. ToDictionary – kulcs → érték");
            Dictionary<string, int> osDb = laptops.GroupBy(x => x.OS).ToDictionary(g => g.Key, g => g.Count());
            Console.WriteLine($"Windows 10: {osDb["Windows 10"]} db, macOS: {(osDb.ContainsKey("macOS") ? osDb["macOS"] : 0)} db");

            Cim("37. Leggyakoribb érték (pl. leggyakoribb RAM-méret)");
            var leggyakoribb = laptops.GroupBy(x => x.RAM).OrderByDescending(g => g.Count()).First();
            Console.WriteLine($"Leggyakoribb RAM: {leggyakoribb.Key} GB ({leggyakoribb.Count()} db)");

            // ---------------------------------------------------------------- HALMAZOK, EGYÉB
            Cim("38. Átlag feletti elemek (al-lekérdezés)");
            double atlag = laptops.Average(x => x.Price);
            Console.WriteLine($"Átlagnál drágább: {laptops.Count(x => x.Price > atlag)} db");

            Cim("39. Duplikátumok (azonos modellnév többször)");
            var duplak = laptops.GroupBy(x => x.Model).Where(g => g.Count() > 1).OrderByDescending(g => g.Count()).Take(3);
            foreach (var g in duplak) Console.WriteLine($"\t{g.Key}: {g.Count()}x");

            Cim("40. Intersect / Except");
            var gamingGyartok = laptops.Where(x => x.Category.CategoryName == "Gaming").Select(x => x.Manufacturer.ManufacturerName).Distinct();
            var ultraGyartok = laptops.Where(x => x.Category.CategoryName == "Ultrabook").Select(x => x.Manufacturer.ManufacturerName).Distinct();
            Console.WriteLine($"Gaming ÉS Ultrabook gyártó: {string.Join(", ", gamingGyartok.Intersect(ultraGyartok).OrderBy(x => x))}");
            Console.WriteLine($"Csak Gaming gyártó: {string.Join(", ", gamingGyartok.Except(ultraGyartok).OrderBy(x => x))}");

            Cim("41. Query szintaxis (SQL-szerű) – ugyanaz, mint a 02.");
            var query = from l in laptops
                        where l.CPU.Contains("Intel Core i7") && l.Storage.Contains("SSD")
                        orderby l.Price descending
                        select l;
            Console.WriteLine($"i7 + SSD (query szintaxissal): {query.Count()} db, legdrágább: {query.First().Model}");

            Cim("42. Ugyanez LINQ NÉLKÜL (foreach) – ha a feladat tiltja a LINQ-et");
            int db = 0;
            double osszeg = 0;
            Laptop max = null;
            foreach (var l in laptops)
            {
                if (l.CPU.Contains("Intel Core i7") && l.Storage.Contains("SSD"))
                {
                    db++;
                    osszeg += l.Price;
                    if (max == null || l.Price > max.Price) max = l;
                }
            }
            Console.WriteLine($"i7 + SSD: {db} db, átlag {osszeg / db:F2} INR, legdrágább: {max.Model}");

            // ---------------------------------------------------------------- FÁJLBA ÍRÁS
            Cim("43. Szűrt, rendezett lista fájlba");
            var cheap = laptops.Where(x => x.Category.CategoryName == "Gaming").OrderBy(x => x.Price).Take(20);
            using (StreamWriter iro = new StreamWriter("../../../forras/cheap.txt"))
            {
                foreach (var l in cheap)
                {
                    iro.WriteLine($"{l.Manufacturer.ManufacturerName} {l.Model}");
                    iro.WriteLine($"\t\t{l.CPU}");
                    iro.WriteLine($"\t\t{l.Storage}");
                    iro.WriteLine($"\t\t{l.Screen}\n");
                }
            }
            Console.WriteLine("cheap.txt elkészült (20 legolcsóbb Gaming laptop).");

            Cim("44. Csoportosított statisztika fájlba (pontosvesszővel)");
            using (StreamWriter iro = new StreamWriter("../../../forras/statisztika.txt"))
            {
                iro.WriteLine("Kategoria;Darab;Atlagar");
                foreach (var g in laptops.GroupBy(x => x.Category.CategoryName).OrderBy(g => g.Key))
                {
                    iro.WriteLine($"{g.Key};{g.Count()};{g.Average(x => x.Price):F2}");
                }
            }
            Console.WriteLine("statisztika.txt elkészült.");
        }
    }
}
