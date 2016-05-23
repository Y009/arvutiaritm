/*   Henry Juhanson
 *  143080IASB
 *  Arvutite aritmeetika ja loogika
 *    Boothi algoritmiga korrutamine
 *    Leitav ka internetist : y009.eu/Program.txt  */  
using System;
using System.Collections;

namespace boothialgoritm
{
    class Program
    {
        BitArray muutuja1;          //1operand              8bitti
        BitArray muutuja2;          //2operand              8bitti
        BitArray muutuja2neg;       //2netaiendkoodis       8bitti
        BitArray muutuja3;          //const 1 / miinus 1    8bitti
        BitArray muutuja4;          //tegevus               17bitti
        BitArray vastus;

        bool op1 = false, op2 = false, overflow = false;    
        //op1 ja 2 on pmst ainult 10nd kuju m2rgi jaoks, 2nd kujul on rg1(7) ja rg2(7)
        sbyte counter;

        static void Main(string[] args)
        {
            Program prog = new Program();
            sbyte cont = 1;         //mitmekordseks kasutamiseks

            while(cont == 1)
            {
                prog.counter = 8;
                prog.op1 = false;
                prog.op2 = false;

                prog.setreg();
                while (prog.counter > 0)
                {
                    prog.prindi(false);
                    Console.WriteLine("Counter:" + prog.counter);
                    prog.korrutamine();
                    prog.shift();
                    prog.counter--;
                }
                prog.prindi(true);
                Console.Write("Uuesti? \n 1 = uuesti \n -1 = lopetab \n");
                cont = Convert.ToSByte(Console.ReadLine());
            }
        }

        void setreg()
        {
            sbyte kuju;
            short x = 0;
            short y = 0;
            Console.Write("\nSisestamise kuju? \n 1 = 10nd kuju \n 0 = 2nd kuju \n");
            kuju = Convert.ToSByte(Console.ReadLine());


            Console.Write("Sisetada esimene muutuja: ");
            if (kuju == 1)
            {
                x = (short)getvar();
                if (x < 0)
                {
                    x = Math.Abs(x);
                    op1 = true;
                }
            }
            else
            {
                muutuja1 = get2ndkuju(out op1);
            }

            Console.Write("Sisetada teine muutuja: ");
            if (kuju == 1)
            {
                y = (short)getvar();
                if (y < 0)
                {
                    y = Math.Abs(y);
                    op2 = true;
                }
            }
            else
            {
                muutuja2 = get2ndkuju(out op2);
            }
            sbyte i;

            muutuja3 = new BitArray(new byte[] { Convert.ToByte(1) });
            if (kuju == 1)
                muutuja1 = new BitArray(new byte[] { Convert.ToByte(x) });

            if (op1 && kuju == 1)
                muutuja1 = taiendkoodi(muutuja1);

            if (kuju == 1)
                muutuja2 = new BitArray(new byte[] { Convert.ToByte(y) });
            muutuja4 = new BitArray(17);
            vastus = new BitArray(8);

            if (op2 && kuju == 1)
                muutuja2 = taiendkoodi(muutuja2);
            muutuja2neg = taiendkoodi(muutuja2);

            for (i = 7; i > -1; i--)                 //set reg 3
            {
                if (muutuja1[7 - i] == true)
                    muutuja4[8 - i] = true;
                else
                    muutuja4[8 - i] = false;
            }
        }

        BitArray bitadd(BitArray op1, BitArray op2)
        {
            bool carryin = false;
            sbyte i;
            BitArray op3 = new BitArray(8);
            for (i = 0; i < 8; i++)
            {
                if ((op1[i] == true && op2[i] == true && !carryin) || (op1[i] != op2[i] && carryin))
                {
                    op3[i] = false;
                    carryin = true;
                }
                else if ((op1[i] == false && op2[i] == false && !carryin))
                {
                    op3[i] = false;
                    carryin = false;
                }
                else if (op1[i] == true && op2[i] == true && carryin)
                {
                    op3[i] = true;
                    carryin = true;
                }
                else
                {
                    op3[i] = true;
                    carryin = false;
                }
            }
            BitArray temp = new BitArray(op1);
            for (i = 0; i < 8; i++)
                temp[i] = op3[i];
            return temp;
        }

        int getvar()
        {
            int operand = Convert.ToInt32(Console.ReadLine());
            return operand;
        }

        BitArray get2ndkuju(out bool op)
        {
            op = false;
            Console.WriteLine("\nSisestada madalaim bitt esimesena ja enter peale igat bitti");
            Console.WriteLine("(Numbreid peale \"1\" tolgendatakse kui nulli.)");
            int temp = 0;
            BitArray operand = new BitArray(8);
            for (int i = 0; i < 8; i++)
            {
                temp = Convert.ToByte(Console.ReadLine());
                if(temp ==1)
                    operand[i] = true;
                else
                    operand[i] = false;
            }
            if (operand[7])
                op = true;
            return operand;
        }

        BitArray taiendkoodi(BitArray x)
        {
            BitArray y = new BitArray(x);

            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] == false)
                    y[i] = true;
                else
                    y[i] = false;
            }
            y = bitadd(y, muutuja3);
            return y;
        }

        void korrutamine()
        {
            int i;
            BitArray temp = new BitArray(8);
            for (i = 0; i <= 7; i++ )
                temp[i]=muutuja4[9 + i];

            if (muutuja4[1] == true && muutuja4[0] == false)
            {
                temp = bitadd(temp, muutuja2neg);
                Console.WriteLine("\nToimus lahutamine\n");
            }
            else if (muutuja4[1] == false && muutuja4[0] == true)
            {
                temp = bitadd(temp, muutuja2);
                Console.WriteLine("\nToimus liitmine\n");
            }

            for (i = 0; i <= 7; i++)
                muutuja4[9  + i] = temp[i];
        }
        void shift()
        {
            for (int i = 0; i < 15; i++)
                muutuja4[i] = muutuja4[i+1];
        }

        void prindi(bool end)
        {   //reaalsuses v6iks dynaamiliseks teha, bitarray muutujaks ja i on length
            bool temp = false;
            int i;
            Console.Write("\nReg1:"); 
            for (i = 7; i > -1; i--)
            {
                if (muutuja1[i] == true)
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");

            Console.Write("Reg2:");
            for (i = 7; i > -1; i--)
            {
                if (muutuja2[i] == true)
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");

            Console.Write("Reg3:");
            for (i = 7; i > -1; i--)
            {
                if (muutuja2neg[i] == true)
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");

            Console.Write("Reg4:");
            for (i = muutuja4.Length - 1; i > -1; i--)
            {
                if (muutuja4[i])
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");

            Console.Write("Reg5:");
            for (i = 7; i > -1; i--)
            {
                if (muutuja3[i] == true)
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.WriteLine("\n---------------------------------------------");
            if (end)
            {
                shift();   //viimane shift on registri lykkamiseks 6igesse j2rku.
                for (i = 7; i > -1; i--)
                    vastus[i] = muutuja4[i];

                Console.Write("Vastus:");
                for (i = vastus.Length - 1; i > -1; i--)
                {
                    if (vastus[i])
                        Console.Write("1");
                    else
                        Console.Write("0");
                }
                Console.Write("\n");

                int[] result = new int[1];

                if (vastus[7])
                { 
                    vastus = taiendkoodi(vastus);
                    temp = true;
                }

                vastus.CopyTo(result, 0);
                if (temp)
                    result[0] = -result[0];
                Console.WriteLine("Vastus 10nd kujul:" + result[0] + "\n");

                for (i = muutuja4.Length - 1; i > 6; i--)
                    if (muutuja4[i] != (op1 ^ op2)) overflow = true;

                if (overflow)
                {
                    temp = false;
                    Console.Write("Saadud vastus on yletaitunud.\nV6imalik reaalne vastus: ");

                    for (i = muutuja4.Length - 1; i > -1; i--)
                    {
                        if (muutuja4[i])
                            Console.Write("1");
                        else
                            Console.Write("0");
                    }
                    Console.Write("\n");

                    if (muutuja4[16])
                       temp = true;

                    if (op1 ^ op2)
                        muutuja4 = taiendkoodi(muutuja4);

                    muutuja4.CopyTo(result, 0);
                    if (temp) result[0] = -result[0];
                    Console.WriteLine("V6imalik vastus 10nd kujul: " + result[0] + "\n");
                }
            }
        }
    } 
}