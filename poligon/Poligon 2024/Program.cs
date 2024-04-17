using Poligon_2024;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Poligon_2024
{
    public class Alati
    {

        public static poligon Unesi()
        {
            Console.Write("Unesite broj temena poligona: ");
            int brojTemena = int.Parse(Console.ReadLine());

            poligon noviPoligon = new poligon(brojTemena);

            for (int i = 0; i < brojTemena; i++)
            {
                Console.WriteLine($"Unesite koordinate temena {i + 1}:");

                Console.Write("x = ");
                double x = double.Parse(Console.ReadLine());

                Console.Write("y = ");
                double y = double.Parse(Console.ReadLine());

                noviPoligon.teme[i] = new tacka(x, y);
            }

            return noviPoligon;
        }

        public static void Snimi(poligon poligon, string imeDatoteke)
        {
            using (StreamWriter datoteka = new StreamWriter(imeDatoteke))
            {
                datoteka.WriteLine(poligon.broj_temena);

                for (int i = 0; i < poligon.broj_temena; i++)
                {
                    datoteka.WriteLine($"{poligon.teme[i].x} {poligon.teme[i].y}");
                }
            }

        }

        public static void Ucitaj(poligon poligon, string imeDatoteke)
        {
            try
            {
                using (StreamReader datoteka = new StreamReader(imeDatoteke))
                {
                    poligon.broj_temena = Convert.ToInt32(datoteka.ReadLine());
                    poligon.teme = new tacka[poligon.broj_temena];

                    for (int i = 0; i < poligon.broj_temena; i++)
                    {
                        string red = datoteka.ReadLine();
                        string[] podaci = red.Split();

                        double x, y;

                        if (podaci.Length >= 2 && double.TryParse(podaci[0], out x) && double.TryParse(podaci[1], out y))
                        {
                            poligon.teme[i] = new tacka(x, y);
                        }
                        else
                        {
                            Console.WriteLine($"Greska pri konverziji: {red}");
                        }
                    }
                }

                Console.WriteLine($"Poligon je uspešno učitan iz datoteke {imeDatoteke}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska prilikom citanja fajla: {ex.Message}");
            }
        }

        public static bool Unutra(poligon poligon, tacka p)
        {
            int brojPreseka = 0;

            for (int i = 0; i < poligon.broj_temena; i++)
            {
                int sledeci = (i + 1) % poligon.broj_temena;

                if ((poligon.teme[i].y < p.y && poligon.teme[sledeci].y >= p.y) || (poligon.teme[sledeci].y < p.y && poligon.teme[i].y >= p.y))
                {

                    if (p.y == poligon.teme[i].y)
                    {
                        if ((poligon.teme[i].x <= p.x && p.x <= poligon.teme[sledeci].x) || (poligon.teme[sledeci].x <= p.x && p.x <= poligon.teme[i].x))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        double x_presek = (poligon.teme[i].x + (p.y - poligon.teme[i].y) / (poligon.teme[sledeci].y - poligon.teme[i].y) * (poligon.teme[sledeci].x - poligon.teme[i].x));

                        if (p.x < x_presek)
                        {
                            brojPreseka++;
                        }
                    }
                }
            }

            return brojPreseka % 2 == 1;
        }


    }
    public class vektor
    {
        public tacka pocetak, kraj;
        public vektor()
        {

        }
        public vektor(tacka pocetak, tacka kraj)
        {
            this.pocetak = pocetak;
            this.kraj = kraj;
        }
        public static tacka vektor_c(vektor A)
        {
            tacka nova = new tacka();
            nova.x = A.kraj.x - A.pocetak.x;
            nova.y = A.kraj.y - A.pocetak.y;
            return nova;
        }
        public static double skalarni(vektor A, vektor B)
        {
            tacka A_c = vektor_c(A);
            tacka B_c = vektor_c(B);
            double skalarni = A_c.x * B_c.x + A_c.y * B_c.y;
            return skalarni;
        }
        public static double VP(vektor A, vektor B)
        {
            tacka A_c = vektor_c(A);
            tacka B_c = vektor_c(B);
            return A_c.x * B_c.y - A_c.y * B_c.x;
        }
        public static double ugao(vektor A, vektor B)
        {
            tacka Ac = vektor_c(A);
            tacka Bc = vektor_c(B);
            double ugaoA = Math.Atan2(Ac.y, Ac.x) * 180 / Math.PI;
            double ugaoB = Math.Atan2(Bc.y, Bc.x) * 180 / Math.PI;
            Console.WriteLine("ugao a={0}", ugaoA);
            Console.WriteLine("ugao b={0}", ugaoB);
            if (ugaoB - ugaoA < 0)
            {
                return ugaoB - ugaoA + 360;
            }
            return ugaoB - ugaoA;
        }
    }
}
public class tacka
{
    public double x, y;
    public tacka(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
    public tacka() { }

    public double get_r()
    {
        double r = Math.Sqrt(x * x + y * y);
        return r;
    }

}
public class poligon
{
    public int broj_temena;
    public tacka[] teme;
    public poligon()
    {
        teme = new tacka[broj_temena];
    }
    public poligon(int n)
    {
        broj_temena = n;
        teme = new tacka[broj_temena];
    }

    public poligon(tacka[] temena)
    {
        broj_temena = temena.Length;
        teme = temena;

    }
    public Boolean konveksan()
    {
        int plusevi = 0;
        for (int i = 0; i < teme.Length - 1; i++)
        {
            vektor prvi = new vektor(teme[i], teme[(i + 1) % broj_temena]);
            vektor drugi = new vektor(teme[(i + 1) % broj_temena], teme[(i + 2) % broj_temena]);
            if (vektor.VP(prvi, drugi) > 0) plusevi++;
        }
        if ((plusevi == 0) || plusevi == broj_temena) return true;
        else return false;
    }

    public bool Prost()
    {
        for (int i = 0; i < broj_temena; i++)
        {
            vektor trenutni = new vektor(teme[i], teme[(i + 1) % broj_temena]);

            for (int j = i + 2; j < broj_temena; j++)
            {
                vektor drugi = new vektor(teme[j], teme[(j + 1) % broj_temena]);
                if (Presek(trenutni, drugi))
                    return false;
            }
        }

        return true;
    }

    private bool Presek(vektor vektor1, vektor vektor2)
    {
        double vp1 = vektor.VP(vektor1, new vektor(vektor1.pocetak, vektor2.pocetak));
        double vp2 = vektor.VP(vektor1, new vektor(vektor1.pocetak, vektor2.kraj));

        double vp3 = vektor.VP(vektor2, new vektor(vektor2.pocetak, vektor1.pocetak));
        double vp4 = vektor.VP(vektor2, new vektor(vektor2.pocetak, vektor1.kraj));

        return (vp1 * vp2 < 0) && (vp3 * vp4 < 0);
    }


    public double Obim()
    {
        double obim = 0;
        for (int i = 0; i < broj_temena; i++)
        {
            int sledeciIndeks = (i + 1) % broj_temena;
            obim += DužinaStranice(teme[i], teme[sledeciIndeks]);
        }
        return obim;
    }

    private double DužinaStranice(tacka tacka1, tacka tacka2)
    {
        return Math.Sqrt(Math.Pow(tacka2.x - tacka1.x, 2) + Math.Pow(tacka2.y - tacka1.y, 2));
    }

    public double Povrsina()
    {
        double povrsina = 0;
        tacka centar = CentarPoligona();

        for (int i = 0; i < broj_temena; i++)
        {
            int sledeciIndeks = (i + 1) % broj_temena;
            tacka trenutna = teme[i];
            tacka sledeca = teme[sledeciIndeks];

            povrsina += PovrsinaTrougla(trenutna, sledeca, centar);
        }

        return Math.Abs(povrsina);
    }

    private tacka CentarPoligona()
    {
        double xSum = 0, ySum = 0;
        foreach (tacka t in teme)
        {
            xSum += t.x;
            ySum += t.y;
        }
        double xCentar = xSum / broj_temena;
        double yCentar = ySum / broj_temena;
        return new tacka(xCentar, yCentar);
    }

    private double PovrsinaTrougla(tacka a, tacka b, tacka c)
    {
        double s = (DužinaStranice(a, b) + DužinaStranice(b, c) + DužinaStranice(c, a)) / 2;
        return Math.Sqrt(s * (s - DužinaStranice(a, b)) * (s - DužinaStranice(b, c)) * (s - DužinaStranice(c, a)));
    }

    public void Ispisi()
    {
        Console.WriteLine($"Broj temena: {broj_temena}");

        for (int i = 0; i < broj_temena; i++)
        {
            Console.WriteLine($"Teme {i + 1}: ({teme[i].x}, {teme[i].y})");
        }
    }

    public poligon GrahamovSkeniranje()
    {

        if (broj_temena < 3) return null;


        tacka pocetnaTacka = teme.OrderBy(t => t.y).ThenBy(t => t.x).First();

        List<tacka> sortiranaTemena = teme.ToList();
        sortiranaTemena.Remove(pocetnaTacka);
        sortiranaTemena.Sort((t1, t2) =>
        {
            double ugao1 = Math.Atan2(t1.y - pocetnaTacka.y, t1.x - pocetnaTacka.x);
            double ugao2 = Math.Atan2(t2.y - pocetnaTacka.y, t2.x - pocetnaTacka.x);
            if (ugao1 < ugao2) return -1;
            if (ugao1 > ugao2) return 1;
            return 0;
        });
        sortiranaTemena.Insert(0, pocetnaTacka);

        Stack<tacka> konveksniOmotac = new Stack<tacka>();
        konveksniOmotac.Push(sortiranaTemena[0]);
        konveksniOmotac.Push(sortiranaTemena[1]);

        for (int i = 2; i < sortiranaTemena.Count; i++)
        {
            tacka vrh = konveksniOmotac.Pop();
            while (Orientation(konveksniOmotac.Peek(), vrh, sortiranaTemena[i]) != 2)
            {
                vrh = konveksniOmotac.Pop();
            }
            konveksniOmotac.Push(vrh);
            konveksniOmotac.Push(sortiranaTemena[i]);
        }

        tacka[] tackeOmotača = konveksniOmotac.ToArray();

        return new poligon(tackeOmotača);
    }

    private static int Orientation(tacka p, tacka q, tacka r)
    {
        double val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
        if (Math.Abs(val) < 0.000001) return 0;
        return (val > 0) ? 1 : 2;
    }

}

internal class Program
    {
        static void Main(string[] args)
        {
            poligon poligon = null;
            bool stop = true;
        bool prst = false;
        bool konv = false;
            while (stop)
            {
                Console.WriteLine("Izaberite opciju:");
                Console.WriteLine("1. Unos poligona");
                Console.WriteLine("2. Snimi");
                Console.WriteLine("3. Ucitaj");
                Console.WriteLine("4. Provera da li je poligon prost");
                Console.WriteLine("5. Provera da li je poligon konveksan");
                Console.WriteLine("6. Izracunavanje obima poligona");
                Console.WriteLine("7. Izracunavanje povrsine poligona");
                Console.WriteLine("8. Izracunavanje konveksnog omotača");
                Console.WriteLine("9. Provera da li je tačka unutar poligona");
                Console.WriteLine("0. Izlaz");

                string opcija = Console.ReadLine();

                switch (opcija)
                {
                    case "0":
                        Console.WriteLine("Izlaz...");
                        stop = false;
                        break;

                    case "1":
                        poligon = Alati.Unesi();
                        break;

                    case "2":
                        if (poligon != null)
                        {
                            Alati.Snimi(poligon, @"C:\Users\admin\Desktop\Pg.txt");
                            Console.WriteLine("uspesno snimljen");
                        }
                        else
                        {
                            Console.WriteLine("Poligon nije unet.");
                        }
                        break;

                    case "3":
                        poligon = new poligon();
                        Alati.Ucitaj(poligon, @"C:\Users\admin\Desktop\Pg.txt");
                        break;

                    case "4":
                        if (poligon != null)
                        {
                        prst = poligon.Prost();
                        if (prst = true)
                        {
                            Console.WriteLine("Prost je");
                        }
                        }
                        else
                        {
                            Console.WriteLine("Poligon nije unet.");
                        }
                        break;

                    case "5":
                        if (poligon != null)
                        {
                        konv = poligon.konveksan();
                        if (konv = true)
                        {
                            Console.WriteLine("Konveksan je");
                        }
                        }
                        else
                        {
                            Console.WriteLine("Poligon nije unet.");
                        }
                        break;

                    case "6":
                        if (poligon != null)
                        {
                            Console.WriteLine(poligon.Obim());
                        }
                        else
                        {
                            Console.WriteLine("Poligon nije unet.");
                        }
                        break;

                    case "7":
                        if (poligon != null)
                        {
                            Console.WriteLine(poligon.Povrsina());
                        }
                        else
                        {
                            Console.WriteLine("Poligon nije unet.");
                        }
                        break;

                    case "8":
                        if (poligon != null)
                        {
                            poligon.Ispisi();
                            poligon.GrahamovSkeniranje().Ispisi();

                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Poligon nije unet.");
                        }
                        break;

                    case "9":
                        if (poligon != null)
                        {
                            tacka tacka = new tacka();
                            Console.Write("x koordinatu tacke: ");
                            tacka.x = double.Parse(Console.ReadLine());
                            Console.Write("y koordinaty tacke: ");
                            tacka.y = double.Parse(Console.ReadLine());
                            Console.WriteLine(Alati.Unutra(poligon, tacka));
                        }
                        else
                        {
                            Console.WriteLine("Poligon nije unet.");
                        }
                        break;

                    default:
                        Console.WriteLine("Nepoznata opcija");
                        break;
                }
            }
        }
    }


