using System;
//using System.Collections.Generic;
using System.Collections;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace boothialgoritm
{
    class Program
    {                   //teha bitarray peale ymber
        BitArray muutuja1;          
        BitArray muutuja2;          
        BitArray muutuja3;
        BitArray muutuja4;

        int[] bits1;                //reg1
        int[] bits2;                //reg2
        int[] bits3;                //reg4
        int[] bits2neg;             //reg5

         //reg1-3 saavad automaatselt 8biti pikkuseks, kuna andes neile v22rtusi kasutan baidi pikkust, mis on 8...

        int[] bits4;                //reg3 - pikkusega 17bitti.

        bool op1 = false;           //kui true, siis operand on negatiivene
        bool op2 = false;

        static void Main(string[] args)
        {
            Program prog = new Program();

            prog.setreg();
            prog.korrutamine();

            Console.Write("\n");
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
            bits3 = muutuja3.Cast<bool>().Select(bit3 => bit3 ? 1 : 0).ToArray();
            Array.Reverse(bits3);

            //Console.WriteLine("reg3:");
            //for (i = 0; i < 8; i++)
            //    Console.Write(bits3[i]);
            //Console.WriteLine("\n");

            muutuja1 = new BitArray(new byte[] { Convert.ToByte(x) });
            bits1 = muutuja1.Cast<bool>().Select(bit1 => bit1 ? 1 : 0).ToArray();
            Array.Reverse(bits1);

            if (op1)
            {
                //Console.WriteLine("op1 t2iendkoodi:");
                bits1 = taiendkoodi(bits1);
            }

            muutuja2 = new BitArray(new byte[] { Convert.ToByte(y) });
            bits2 = muutuja2.Cast<bool>().Select(bit2 => bit2 ? 1 : 0).ToArray();
            Array.Reverse(bits2);

            muutuja4 = new BitArray(17);
            bits4 = new int[17];

            if (op2)
            {
                Console.WriteLine("op2 t2iendkoodi:");
                bits2neg = taiendkoodi(bits2);
            }


            for (i = 0; i < 8; i++)
                Console.Write(bits1[i]);
            Console.Write("\n");

            for (i = 0; i < 8; i++)
                Console.Write(bits2[i]);
            Console.Write("\n");

            for (i = 0; i < 8; i++)
            {
                if (bits1[i] == 1)
                    muutuja4.Set(8 - i, true);
                else
                    muutuja4.Set(8 - i, false);
            }
               // bits4[8 + i] = bits1[i]; 

            bits3 = bitadd(bits1, bits2);

            Console.Write("\n bitadd tulemus \n");
            for (i = 0; i < 8; i++)
                Console.Write(bits3[i]);
            Console.Write("\n");

            Console.Write("\n reg3: \n");
            for (i = 0; i < 17; i++)
            { 
                if(muutuja4[i])
                    Console.Write("1");
                else
                    Console.Write("0");
            }
            Console.Write("\n");
        }

        int[] bitadd(int[] op1, int[] op2)
        {
            bool carryin = false;
            sbyte i;
            int[] op3 = new int[8];

            //Console.WriteLine("\n adding:");

            //for (i = 0; i < 8; i++)
            //    Console.Write(op1[i]);
            //Console.Write("\n");
            //for (i = 0; i < 8; i++)
            //    Console.Write(op2[i]);
            //Console.WriteLine("\n");
            for (i = 7; i > -1; i--)                    //teha bitarray peale ymber
            {
                if ((op1[i] == 1 && op2[i] == 1 && !carryin) || (op1[i] != op2[i] && carryin))
                {
                    op3[i] = 0;
                    carryin = true;
                }
                else if ((op1[i] == 0 && op2[i] == 0 && !carryin))
                {
                    op3[i] = 0;
                    carryin = false;
                }
                else if (op1[i] == 1 && op2[i] == 1 && carryin)
                {
                    op3[i] = 1;
                    carryin = true;
                }
                else
                {
                    op3[i] = 1;
                    carryin = false;
                }
            }
            //Console.Write("\n bitadd tulemus \n");
            //for (i = 0; i < 8; i++)
            //    Console.Write(op3[i]);
            //Console.Write("\n");

            return op3;
        }

        int getvar()
        {
            int operand = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(operand);
            return operand;
        }

        int[] taiendkoodi(int[] x)
        {
            for (int i = 0; i < 8; i++)
            {
                if (x[i] == 0)
                    x[i] = 1;
                else
                    x[i] = 0;
            }
            //Console.WriteLine("enne +1 taiendkoodi viimisel");
            //for (int i = 0; i < 8; i++)
            //    Console.Write(x[i]);
            //Console.WriteLine("\n----------------");
            x = bitadd(x, bits3);

            //Console.WriteLine("operand t2iendkoodis:");
            //for (int i = 0; i < 8; i++)
            //    Console.Write(x[i]);
            //Console.WriteLine("\n----------------");

            return x;
        }

        void korrutamine()
        {
            sbyte s2ilivbit = (sbyte) bits4[0];
            if (bits4[15] == 1 && bits4[16] == 0)           //lahutamine
                bits4 = bitadd(bits4, bits2neg);

            else if (bits4[15] == 0 && bits4[16] == 1)      //liitmine
                bits4 = bitadd(bits4, bits2);
            shift();
        }
        void shift()
        {
           // muutuja4 = bits4;
            int[] result = new int[1];
            muutuja4.CopyTo(result, 0);
            Console.WriteLine(result[0]);
            Console.WriteLine(result[0]>>1);
        }

    }







    
}
