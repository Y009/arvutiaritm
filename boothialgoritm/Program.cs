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

        bool op1 = false;           //kui true, siis operand on negatiivene
        bool op2 = false;

        sbyte counter;

        static void Main(string[] args)
        {
            Program prog = new Program();
            byte cont = 1;

            while(cont >= 0)
            {
                prog.counter = 7;
                prog.op1 = false;
                prog.op2 = false;

                prog.setreg();
                while (prog.counter >= 0)
                {
                    Console.WriteLine("\nCounter:" + prog.counter);
                    prog.korrutamine();
                }
                prog.shift();
                Console.Write("Uuesti? \n 1 = uuesti \n 0 = lopetab. \n");
                cont = Convert.ToByte(Console.ReadLine());
            }
        }

        void setreg()
        {
            Console.Write("Sisetada esimene muutuja: ");
            short x = (short)getvar();
            if (x < 0)
            { 
                x = Math.Abs(x);
                op1 = true;
            }

            Console.Write("Sisetada teine muutuja: ");
            short y = (short)getvar();
            if (y < 0)
            { 
                y = Math.Abs(x);
                op2 = true;
            }
            sbyte i;

            muutuja3 = new BitArray(new byte[] { Convert.ToByte(1) });
            muutuja1 = new BitArray(new byte[] { Convert.ToByte(x) });

            if (op1)
                muutuja1 = taiendkoodi(muutuja1);

            muutuja2 = new BitArray(new byte[] { Convert.ToByte(y) });
            muutuja4 = new BitArray(17);

            if (op2)
                muutuja2 = taiendkoodi(muutuja2);
            muutuja2neg = taiendkoodi(muutuja2);

            for (i = 7; i > -1; i--)
            {
                if (muutuja1[i] == true)
                    Console.Write("1");
                else
                    Console.Write("0");
            }

            Console.Write("\n");
            for (i = 7; i > -1; i--)
            {
                if (muutuja2[i] == true)
                    Console.Write("1");
                else
                    Console.Write("0");
            }

            for (i = 7; i > -1; i--)                 //set reg 3
            {
                if (muutuja1[7 - i] == true)
                    muutuja4[8 - i] = true;
                else
                    muutuja4[8 - i] = false;
            }

            Console.Write("\n reg3: \n");
            for (i = 16; i > -1 ; i--)
            { 
                if(muutuja4[i])
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");
        }

        BitArray bitadd(BitArray op1, BitArray op2)
        {
            bool carryin = false;
            sbyte i;
            BitArray op3 = new BitArray(8);
            Console.Write("\n");
            Console.WriteLine("1.liidetav");
            for (i = 0; i < 8; i++)
                Console.WriteLine(op1[i]);
            Console.Write("\n");
            Console.WriteLine("2.liidetav");
            for (i = 0; i < 8; i++)
                Console.WriteLine(op2[i]);

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
                //Console.WriteLine("op3 kohal " + i + " on " + op3[i]);
            }

            Console.Write("\n bitadd tulemus \n");
            for (i = 7; i > -1; i--)
            {
                if (op3[i] == true)
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");

            return op3;
        }

        int getvar()
        {
            int operand = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(operand);
            return operand;
        }

        BitArray taiendkoodi(BitArray x)
        {
            BitArray y = new BitArray(x);
            for (int i = 0; i < 8; i++)
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
            Console.WriteLine("\n reg3");
            for (i = muutuja4.Length-1; i > -1; i--)
            {
                if (muutuja4[i])
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");

            BitArray temp = new BitArray(8);
            for (i = 0; i <= 7; i++ )
                temp[i]=muutuja4[9 + i];

            if (muutuja4[1] == true && muutuja4[0] == false)           //lahutamine
            {
                Console.WriteLine("lahutamine");
                temp = bitadd(temp, muutuja2neg);
            }

            else if (muutuja4[1] == false && muutuja4[0] == true)      //liitmine
            {
                Console.WriteLine("liitmine");
                temp = bitadd(temp, muutuja2);
            }

            for (i = 0; i <= 7; i++)
                muutuja4[9  + i] = temp[i];
                
            shift();
        }
        void shift()
        {
            Console.WriteLine("---------------------------------------------");
            Console.Write("\n reg3 enne shifti:\n");
            for (int i = muutuja4.Length - 1; i > -1; i--)
            {
                if (muutuja4[i])
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");
  
            for (int i = 0; i < 15; i++)
                muutuja4[i] = muutuja4[i+1];

            Console.WriteLine("reg3 peale shifti:");
            for (int i = muutuja4.Length - 1; i > -1; i--)
            {
                if (muutuja4[i])
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");
            counter--;
        }
    } 
}
