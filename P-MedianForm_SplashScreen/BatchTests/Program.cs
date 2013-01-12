using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PMedLib;
using System.Diagnostics;

namespace BatchTests
{
    class Program
    {
        static FileStream fsBatchIn;
        static FileStream fsBatchOut;
        static StreamReader srBatchIn;
        static StreamWriter swBatchOut;
        static PMed3 pmed;
        static Stopwatch time;


        static void Main(string[] args)
        {   
            Console.WriteLine("BATCH TESTS");
            time = new Stopwatch();
            time.Start();
            if (args.Length==0)
                return;
            BatchTests(args[0]);
        }

        private static bool LoadPmedProb(string filename, ref int m, bool return_m)
        {
            if (!pmed.Load(filename, ref m, return_m))
                return false;
            
            //if(return_m)
            //    return true;

            //if (m == -1)
            //{
                //uncap
            //    return true;
            //}
            //else if(!pmed.Load(filename, ref m, return_m))
            //    return false;

            return true;
            
        }

        private static bool ComputeBestof3Sol(string nomep)
        {
            //int i_sol = 3;
            int i_sol = 1;
            int i,j;
            TimeSpan timeelapsed= new TimeSpan();
            //uint[] setN = {1000, 3000, 5000};
            uint[] setN = { 1000 };
            //uint[] setN = { 100, 500, 1000, 3000, 5000, 10000, 15000, 20000 };
            uint setpofsol;
            //double MaxIterSec = 60 * 5; // 5 min;
            double MaxIterSec = 0.0;

            pmed.Seed = 1657;
            pmed.MaxIterSecond = MaxIterSec;

            swBatchOut.WriteLine("\r\nSeed = {0}", pmed.Seed);
            uint bestsol=int.MaxValue;
            bool isbestwrap=false;
            for (j = 0; j < setN.Length; j++)
            {
                Console.WriteLine("preparing computation {0} ...", setN[j]);
                pmed.SetSolutions(setN[j]);
                setpofsol = 0;
                
                //swBatchOut.WriteLine("Computing with {0} sols", setN[j]);
                bestsol = int.MaxValue;
                isbestwrap = false;
                timeelapsed = new TimeSpan();
                for (i = 0; i < i_sol; i++)
                {
                    Console.WriteLine("Computing SOl..");
                    if (!pmed.ComputeSolutions(true, true))
                    {
                        swBatchOut.WriteLine("Cannot Compute Wrapper Solution!!! error!!!");
                    }
                    /*else*/ if (bestsol > pmed.WrapperSolution)
                    {
                        if (pmed.WrapperSolution > 0)
                        {
                            if (pmed.WrapperSolution < pmed.Solutions[pmed.BestSolutionIndex])
                                isbestwrap = true;
                            else
                                isbestwrap = false;

                            bestsol = pmed.WrapperSolution;
                            
                            //Console.WriteLine("\r\nChecking Solutions Set of p ...");
                            //setpofsol = pmed.CheckPofSolutions(false);
                            timeelapsed = pmed.GetTotalElapsedTime();
                        }
                        else
                        {
                            swBatchOut.WriteLine("{0} : No Wrapper Computed!!!!", nomep);
                            if (bestsol > pmed.Solutions[pmed.BestSolutionIndex])
                            {
                                Console.WriteLine("\r\nChecking Solutions Set of p ...");
                                //setpofsol = pmed.CheckPofSolutions(false);
                                timeelapsed = pmed.GetTotalElapsedTime();
                                bestsol = pmed.Solutions[pmed.BestSolutionIndex];
                            }
                        }
                        
                        //timeelapsed = pmed.GetTotalElapsedTime();
                        
                        //setpofsol = pmed.CheckPofSolutions(false);
                    }
                }
                //salvo i risultati del meglio
                Console.Write("\r\nSalvo i risultati...");
                swBatchOut.WriteLine("{4} : Solution = {0} --- Wrapper = {1} --- TotTime = {2} --- N = {3} CheckSetP = {5}", bestsol, isbestwrap, timeelapsed, setN[j],nomep,setpofsol);
                swBatchOut.Flush();
                Console.WriteLine("ok");
            }
            return true;
        }

        private static void BatchTests(String args0)
        {
            int m=0;
            int M=0;
            //int sol_i=3;
            //int i;
            //int N = 100;

            String BatchOut = "BatchOut-" + DateTime.Now.Date + "_" + DateTime.Now.TimeOfDay + ".txt";
            BatchOut = BatchOut.Replace("/", "-");
            BatchOut = BatchOut.Replace(":", "_");

            //fsBatchIn = new FileStream("Batch.txt", FileMode.Open);
            fsBatchIn = new FileStream(args0, FileMode.Open);
            srBatchIn = new StreamReader(fsBatchIn);
            //fsBatchOut = new FileStream("BatchOut.txt", FileMode.Create);
            fsBatchOut = new FileStream(BatchOut, FileMode.Create);
            swBatchOut = new StreamWriter(fsBatchOut);
            swBatchOut.AutoFlush = true;

            pmed = new PMed3(Console.Write, 1);

            swBatchOut.WriteLine("Batch Output Date/Time : {0} - {1}", DateTime.Now.Date, DateTime.Now.TimeOfDay);
            while (!srBatchIn.EndOfStream)
            {
                string filein = srBatchIn.ReadLine().Trim();
                if (filein == "")
                    break;
                swBatchOut.Write("loading {0} --- nodes ", filein);
                //aggiunto il costruttore, reinitializzo l'oggetto ad ogni nuovo file,
                //perchè coinor ha dei problemi nella gestione della memoria quando si hanno
                //matrici molto grandi.
                //dovuto al fatto di bugs a coinor? 
                //+ precisamente sta in CoinBigIndex che non è implementato in tutti i sotto progetti dell
                //libreria.
                //e coin.dll in x64 non funziona correttamente...
                pmed = new PMed3(Console.Write, 1);
                pmed.ProblemType = eProblemType.SetPartitioning;
                if (!LoadPmedProb(filein, ref m, true))
                {
                    swBatchOut.WriteLine("\r\nCannot load {0}  --- m = {1}\r\n", filein, m);

                }
                else
                {
                    
                    M = m;
                    if ((m > -1))
                        m = 1;

                    for (; m <= M; m++)
                    {
                        //per Capacitated.
                        LoadPmedProb(filein, ref m, false);
                        swBatchOut.WriteLine("{0} - p {1} - m {2}", pmed.nVerteces, pmed.p,m);
                        ComputeBestof3Sol(filein);
                    }
                    if (m == -1) //uncap
                    {
                        swBatchOut.WriteLine("{0} - p {1}", pmed.nVerteces, pmed.p);
                        ComputeBestof3Sol(filein);
                    }
                }
                swBatchOut.Flush();
            }

            time.Stop();
            swBatchOut.WriteLine("\r\nFinish All batch, time elapsed {0}", time.Elapsed);
            swBatchOut.Close();
            fsBatchOut.Close();
            srBatchIn.Close();
            fsBatchIn.Close();
            

        }
    }
}
