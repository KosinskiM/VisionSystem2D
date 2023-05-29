using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DVisionSystemLibrary.DataConnection
{
    public class MessageDecryption
    {

        /// <summary>
        /// Respond message from controller analyzed 
        /// in case of gripper rotation
        /// </summary>
        /// <param name="pozycja"></param>
        /// <returns></returns>
        public static (bool, string) checkGripperRotation(string pozycja)
        {

            //7 bo bo 7 zmiennych w wiadomosci
            string[] separated = pozycja.Split(',');
            double[] number = new double[separated.Length];
            pozycja = "";
            bool wynik;

            //zmiana na liczby
            for (int i = 0; i < 7; i++)
            {
                //string przechowujacy 
                string temp = "";
                if (separated[i][0] == '+')
                {

                    for (int j = 1; j < separated[i].Length; j++)
                    {
                        temp += separated[i][j];
                    }
                    number[i] = Convert.ToDouble(temp);

                }
                else
                {
                    temp = separated[i];
                    number[i] = Convert.ToDouble(temp);
                }
            }

            //sprawdzenie czy chwytak jest obrocony ?
            if (number[3] == 0)
            {
                wynik = true;

            }
            else
            {
                wynik = false;

                separated[3] = "0.00";

                if (number[2] > 240)
                {
                    pozycja = separated[0] + "," + separated[1] + "," + separated[2] + "," +
                    separated[3] + "," + separated[4] + "," + separated[5] + "," + separated[6] + "," + separated[7] + "," + separated[8];
                }
                else
                {
                    separated[2] = "250.0";
                    pozycja = separated[0] + "," + separated[1] + "," + separated[2] + "," +
                    separated[3] + "," + separated[4] + "," + separated[5] + "," + separated[6] + "," + separated[7] + "," + separated[8];
                }
            }

            return (wynik, pozycja);
        }












        /// <summary>
        /// Respond message from controller analyzed 
        /// in case of gripper rotation
        /// </summary>
        /// <param name="pozycja"></param>
        /// <returns></returns>
        

        // TODO opisac pozosałe metody
        // ddodac biblioteki
        // podlaczyc do kodu glownego



        //sprawdzenie aktualnej wysokosci grippera
        public static (bool, string, double) checkGripperHeight(string message)
        {

            string pozycja = "," + message;
            string[] separated = pozycja.Split(',');
            pozycja = "";
            bool wynik;
            double number = 0;



            if (separated[3][0] == '+')
            {
                string temp = "";
                for (int i = 1; i < separated[3].Length; i++)
                {
                    temp += separated[3][i];
                }
                number = Convert.ToDouble(temp);
            }
            else
            {
                number = Convert.ToDouble(separated[3]);
            }


            if (number > 240)
            {
                wynik = true;
            }
            else
            {
                wynik = false;
                separated[3] = "250.0";
                pozycja = separated[1] + "," + separated[2] + "," + separated[3] + "," +
                    separated[4] + "," + separated[5] + "," + separated[6] + "," + separated[7] + "," + separated[8] + "," + separated[9];
            }

            return (wynik, pozycja, number);
        }


        //sprawdzenie czy bezpieczena pozycja
        public static bool imgPosition(string position)
        {
            bool wynik;
            //sepracja po przecinku
            string[] separated = position.Split(',');
            double[] number = new double[separated.Length];

            //"0        +398.22,   1    0.00,      2  +628.68,      3  0.00,     4  +176.32,       5 +947.41,       6 +0.00,   R,A,C"

            for (int i = 0; i < 7; i++)
            {
                //string przechowujacy 
                string temp = "";
                if (separated[i][0] == '+')
                {

                    for (int j = 1; j < separated[i].Length; j++)
                    {
                        temp += separated[i][j];        //index out of range
                    }
                    number[i] = Convert.ToDouble(temp);

                }
                else
                {
                    temp = separated[i];
                    number[i] = Convert.ToDouble(temp);
                }
            }

            //wlasciwa pozycja:
            //    0    1    2      3     4      5      6   7 8 9
            //+380.11,+0.00,+613.27,+0.00,+176.32,+938.94,+0.00,R,A,C
            //2 wersja +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A
            //warunki dokladnosc pozycji

            if (number[0] == 380.93
                && number[1] == -2.66
                && number[2] == +596.28
                && number[3] == +0.40
                && number[4] == 176.32
                && number[5] == +954.80
                && separated[9][0] == 'C')
            {
                wynik = true;
            }
            else
            {
                wynik = false;
            }

            return wynik;
        }

        //sprawdzenie aktualnej wysokosci grippera
        public static bool safePosition(string position)
        {
            bool wynik;
            //sepracja po przecinku
            string[] separated = position.Split(',');
            double[] number = new double[separated.Length];


            //+455.34,+4.71,+240.00,+0.00,+176.32,+947.41,+0.00,R,A,C


            //sprawdzenie czy znak przed liczba
            //"0        +416.13,   1  +9.33,    2  +496.21,      3  +1.62,    4  +175.58,       5 +1102.50,      6 +0.00,   R,A,O"
            //"0        +398.22,   1    0.00,      2  +628.68,      3  0.00,     4  +176.32,       5 +947.41,       6 +0.00,   R,A,C"
            for (int i = 0; i < 7; i++)
            {
                //string przechowujacy 
                string temp = "";
                if (separated[i][0] == '+')
                {

                    for (int j = 1; j < separated[i].Length; j++)
                    {
                        temp += separated[i][j];        //index out of range
                    }
                    number[i] = Convert.ToDouble(temp);

                }
                else
                {
                    temp = separated[i];
                    number[i] = Convert.ToDouble(temp);
                }
            }

            //wlasciwa pozycja:
            //"+398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C"
            //warunki dokladnosc pozycji
            if (number[2] >= 240 && number[3] == 0.00)
            {

                wynik = true;

            }
            else
            {

                wynik = false;

            }


            return wynik;
        }



        //-------------------------------------------------------------czyszczenie teblicy stringow do wyslania ----
        //wyczyszczenie tablicy wiadomosci do wyslania!
        public static string[] msgTabClear(string[] msgTab)
        {
            for (int i = 0; i < msgTab.Length; i++)
            {
                msgTab[i] = null;
            }

            return msgTab;
        }
        //wyczyszczenie tablicy wiadomosci programu !
        public static string[] programTabClear(string[] programTab)
        {
            for (int i = 0; i < programTab.Length; i++)
            {
                programTab[i] = null;
            }

            return programTab;
        }
    }

}

