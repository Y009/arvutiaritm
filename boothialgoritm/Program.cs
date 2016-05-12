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
        BitArray muutuja4;          //tegevus + vastus      17bitti

        bool op1 = false, op2 = false;           //kui true, siis operand on negatiivene
        sbyte siskuju;               //2nd v6i 10nd kuju sisestamiseks

        sbyte counter;

        static void Main(string[] args)
        {
            Program prog = new Program();
            sbyte cont = 1;

            while(cont == 1)
            {
                prog.counter = 7;
                prog.op1 = false;
                prog.op2 = false;

                prog.setreg();
                while (prog.counter >= 0)
                {
                    prog.prindi(false);
                    Console.WriteLine("Counter:" + prog.counter);
                    prog.korrutamine();
                }
                prog.shift();
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
            return op3;
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
                
            shift();
            counter--;
        }
        void shift()
        {
            for (int i = 0; i < 15; i++)
                muutuja4[i] = muutuja4[i+1];
        }

        void prindi(bool end)
        {   //reaalsuses v6iks dynaamiliseks teha, bitarray muutujaks ja i on length
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
                    Console.
                        Write("0");
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
                if (op1 ^ op2)
                    muutuja4 = taiendkoodi(muutuja4);

                Console.Write("Reg4:");
                for (i = muutuja4.Length - 1; i > -1; i--)
                {
                    if (muutuja4[i])
                        Console.Write("1");
                    else
                        Console.Write("0");
                }
                Console.Write("\n");

                int[] result = new int[1];
                muutuja4.CopyTo(result, 0);
                if (op1 ^ op2)
                    result[0] = -result[0];
                Console.WriteLine("Tulemus 10nd kujul:" + result[0] + "\n");
            }
        }
    } 
}
