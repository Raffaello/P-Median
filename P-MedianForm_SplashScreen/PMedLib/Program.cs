using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using PMedLib;
using CoinCplexWrapper;

namespace SimplyConsoleApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            Console.WriteLine("{0}",Directory.GetCurrentDirectory());

            //TestWrapper();
            //TestCOinMPWrapper();
            //TestPMedWrapper3CCS(); //sembra ok tutto!!!!
            //TestPMedWrapper_Large(); //in un caso del genere da problemi che sia il coinor / wrapper?
            //TestWrapperCapacitated();
            TestWrapperUncapacitated();
            Console.ReadKey();
        }

        static void EsempioMinimale()
        {
            String FileName = "Nome e Percorso del problema da elaborare";
            PMed3 pmed;
            int m=0; //per capacitated problem
            try
            {
                //Punto 1
                pmed = new PMed3(Console.Write, 1);
                
                //Punto 2
                if(pmed.Load(FileName, ref m))
                {
                    if (m == -1) //allora è di tipo uncapacitated (già caricato)
                    {
                        //....
                        //si può non fare niente oppure gestire cio che si deve gestire.
                        //
                    }
                    else
                    {
                        //questo fase è necessaria perchè sono molto diversi i file come strutturazione.
                        //invece che fare 2 metodi load, si è scelto di farne uno che a seconda di cosa ritorna
                        //nel parametro reference m, fa capire di che tipo è il problema,
                        //la seconda volta solo per i capacitated è da richiamare perchè la prima restituisce
                        //quanti sono i problemi in esso contenuti, la seconda carica il problema scelto.
                        int m_problem=1; // dev'essere fra 1 e m. 

                        if(!pmed.Load(FileName,ref m_problem, false))
                        {
                            Console.WriteLine("Impossibile caricare il file...");
                            return;
                        }
                    }
                    //Fine Punto 2
                    //a questo punto ho il file caricato sia uncapacitated che capacitated
                    //Punto 3
                    if(pmed.ComputeSolutions(true,true))
                    {
                        //Punto 4
                        //gestione dati....
                    }
                    else
                        Console.WriteLine("Erorre nella computazione...");
                }   
            }
            catch (DllNotFoundException e)
            {
                Console.WriteLine("DLL NON TROVATA!!! \r\n{0}", e.Message);
            }
        }


        private static void TestWrapperUncapacitated()
        {
            PMed3 pmed3;
            uint nsol = 1;
            int m = 0;


            try
            {
                pmed3 = new PMed3(Console.Write, nsol, "TestWrap", false, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("exception: {0}", e.Message);
                Console.WriteLine("{0}", Directory.GetCurrentDirectory());
                return;
            }


            if (!pmed3.Load(@"..\..\..\pmed4.txt", ref m, false))
            {
                Console.WriteLine("ERROR LOAD FILE!!!");
                return;
            }

            pmed3.ComputeSolutions(false, false);
            //pmed3.InitProblem();
            //#endif

            //pmed3.InitProblem();
            //pmed3.BuildSolutionFromWrapper();

            //Console.ReadKey();
            //pmed3.ShowWrapperSolution(Console.Write);
            pmed3.CheckWrapperClusterSolution();
        }

        private static void TestWrapperCapacitated()
        {
            PMed3 pmed3;
            uint nsol = 1;
            int m = 1;


            try 
            {
                pmed3 = new PMed3(Console.Write, nsol, "TestWrap", false, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("exception: {0}", e.Message);
                Console.WriteLine("{0}", Directory.GetCurrentDirectory());
                return;
            }

            
            if (!pmed3.Load(@"..\..\..\pmedcap1.txt", ref m,false))
            {
                Console.WriteLine("ERROR LOAD FILE!!!");
                return;
            }

            pmed3.ComputeSolutions(false, false);
            //pmed3.InitProblem();
            //#endif

            //pmed3.InitProblem();
            //pmed3.BuildSolutionFromWrapper();

            //Console.ReadKey();
            pmed3.ShowWrapperSolution(Console.Write);
            pmed3.CheckWrapperClusterSolution();

        }

        private static void TestWrapper()
        {
            //Funzione per testare e capire il wrapper...
            Console.WriteLine("TEST wrapper: funzionamento e verifica del suo funzionamento.");
            
            PMed3 pmed3;
            try
            {
                pmed3 = new PMed3(Console.Write, 2, "TestWrap", true, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("exception: {0}",e.Message);
                Console.WriteLine("{0}", Directory.GetCurrentDirectory());
                return;
            }
#if DEBUG
            //if (!pmed3.LoadTest(@"\\polocesena\polocesena\UtentiScienze\STUDENTI\raffaello.bertini\Desktop\AAA\Problems\pmedtest_verysmall.txt"))
            if (!pmed3.LoadTest(@"..\..\..\pmedtest_verysmall.txt"))
            {
                Console.WriteLine("ERROR LOAD FILE!!!");
                return;
            }

            pmed3.ComputeSolutions(false, false);
            //pmed3.InitProblem();
#endif

            //if (!pmed3.Load(@"C:\Users\Raffaello\Documents\Uni\AAA\Proj_Exam\Problems\pmed1.txt"))
            //{
            //    Console.WriteLine("ERROR LOAD FILE!!!");
            //    return;
            //}
            //pmed3.ComputeSolutions(true, false);
            //if (!pmed3.InitProblem())
            //{
            //    Console.WriteLine("init failed!! stop!");
            //    return;
            //}
            //pmed3.ShowTableau();

            // DA FARE TESTARE UN SEMPLICE PROBLEMA pmed3 per il wrapper ed il formato
            // della matrice. 
            // quindi fare un progetto che usa il wrapper e risolve il sistema lineare.

        }

        private static void TestPMedWrapper()
        {
            //Problema da testare : 
            //Sol0 = {4,2} ; {1,0,3}
            //Sol1 = {3,1} ; {4,2,0}

            //C(Sol0) = {0,16}=16 ; {0,15,40}=55  = 71
            //C(Sol1) = {0,40}=40 ; {0,16,24}=40  = 80

            //min z = 0*x000 + 16*x001 + 0*x010 + 15*x011 + 40*x012 + 0*x100 + 40*x101 + 0*x110 + 16*x111 + 24*x112
            //s.t.

            //    x000	x001	x010	x011	x012	x100	x101	x110	x111	x112
            //z	 0		16		0		15		40		0		40		0		16		24		{min}	

            //0	 0		1		0		1	
            //1	 0		1		1		0
            //2	 1		0		0		1
            //3	 0		1		1		0
            //4	 1		0		0		1

            Wrapper w = new Wrapper();
            //da fare in soluzione come calcolerebbe l'algoritmo e poi testare la CCS...
            //dopo che si è riuscito a farlo a "mano"...
            //CCS ccs = new CCS();

            int objectSense = 1;

            int nzc = 10;
            int colnum = 4;
            int rownum = 5;

            //i coefficenti sono il peso delle xi, quindi essendo che ogni variabile a coefficente 1 ...
            double[] objectCoeffs = new double[10] { 0, 16, 0, 15, 40, 0, 40, 0, 16, 24 };
            String rowType = "GGGGG";
            double[] rhsValues = new double[5] { 1, 1, 1,1 , 1 };

            int[] matrixBegin = new int[4 + 1] { 0, 2, 5, 7, 10 };
            int[] matrixCount = new int[4] { 2, 3, 2, 3 };
            int[] matrixindex = new int[10] { 2, 4, 0, 1, 3, 1, 3, 0, 2, 4 };  // notare che sono i clusters ordinati cresenti 
            double[] matrixval = new double[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            w.initSolver(false);

            WrapProblem prob = w.createProblem("prob", false);

            w.loadProblem(prob, colnum, rownum, nzc, 0, objectSense, 0, objectCoeffs, null, null, rowType, rhsValues, null, matrixBegin, matrixCount, matrixindex, matrixval, null, null, "");

            w.optimizeLpProblem(prob);

            double obj = w.getObjectValue(prob);
            double[] solvars = new double[10]; //Vettore deputato a contenere i valori della soluzione
            w.getSolutionValues(prob, solvars, null, null, null);
            for (int i = 0; i < solvars.Length; i++)
                Console.WriteLine("{0} solvar {1}", solvars[i], i);
            Console.WriteLine("VALORE OTTIMO TROVATO :  {0} ", obj);
 
        }
        private static void TestPMedWrapper2()
        {
            //Sol0:
            //p = 4 {2} ; p=1 {0,3}

            //Sol1: 
            //p =3 {1} ; p=4 {2,0}

            //C(Sol0) = {16}=16 ; {15,40}=55  = 71
            //C(Sol1) = {40}=40 ; {16,24}=40  = 80

            //min z = 16*x001 + 15*x011 + 40*x012 + 40*x101 + 16*x111 + 24*x112
            //s.t.

            //        x001	x011	x012	x101	x111	x112
            //z	 	16		15		40		40		16		24		{min}	

            //0	 0		1		0		1	
            //1	 0		1		1		0
            //2	 1		0		0		1
            //3	 0		1		1		0
            //4	 1		0		0		1

            Wrapper w = new Wrapper();
            //da fare in soluzione come calcolerebbe l'algoritmo e poi testare la CCS...
            //dopo che si è riuscito a farlo a "mano"...
            //CCS ccs = new CCS();

            int objectSense = 1;

            int nzc = 10;
            int colnum = 4;
            int rownum = 5;

            //i coefficenti sono il peso delle xi, quindi essendo che ogni variabile a coefficente 1 ...
            double[] objectCoeffs = new double[6] { 16, 15, 40, 40, 16, 24 };
            String rowType = "GGGGG";
            double[] rhsValues = new double[5] { 1, 1, 1, 1, 1 };

            int[] matrixBegin = new int[4 + 1] { 0, 2, 5, 7, 10 };
            int[] matrixCount = new int[4] { 2, 3, 2, 3 };
            int[] matrixindex = new int[10] { 2, 4, 0, 1, 3, 1, 3, 0, 2, 4 };  // notare che sono i clusters ordinati cresenti 
            double[] matrixval = new double[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            w.initSolver(false);

            WrapProblem prob = w.createProblem("prob", false);

            w.loadProblem(prob, colnum, rownum, nzc, 0, objectSense, 0, objectCoeffs, null, null, rowType, rhsValues, null, matrixBegin, matrixCount, matrixindex, matrixval, null, null, "");

            w.optimizeLpProblem(prob);

            double obj = w.getObjectValue(prob);
            double[] solvars = new double[6]; //Vettore deputato a contenere i valori della soluzione
            w.getSolutionValues(prob, solvars, null, null, null);
            for (int i = 0; i < solvars.Length; i++)
                Console.WriteLine("{0} solvar {1}", solvars[i], i);
            Console.WriteLine("VALORE OTTIMO TROVATO :  {0} ", obj);

        }
        private static void TestPMedWrapper3()
        {
            //Problema da testare : 
            //ora proviamo con costo cluster...

            //Sol0 = {4,2} ; {1,0,3}
            //Sol1 = {3,1} ; {4,2,0}

            //C(Sol0) = {0,16}=16 ; {0,15,40}=55  = 71
            //C(Sol1) = {0,40}=40 ; {0,16,24}=40  = 80

            //min z = 16*S0c0 + 55*S0c1+ 40*s1c0 + 40*s1c1
            //s.t.

            //    s0c0	s0c1	s1c0	s1c1
            //z	 16		55		40		40

            //0	 0		1		0		1	
            //1	 0		1		1		0
            //2	 1		0		0		1
            //3	 0		1		1		0
            //4	 1		0		0		1


            Wrapper w = new Wrapper();
            //da fare in soluzione come calcolerebbe l'algoritmo e poi testare la CCS...
            //dopo che si è riuscito a farlo a "mano"...
            //CCS ccs = new CCS();

            int objectSense = 1;

            int nzc = 10;
            int colnum = 4;
            int rownum = 5;

            //i coefficenti sono il peso delle xi, quindi essendo che ogni variabile a coefficente 1 ...
            double[] objectCoeffs = new double[4] { 16, 55, 40, 40,};
            String rowType = "GGGGG";
            double[] rhsValues = new double[5] { 1, 1, 1, 1, 1 };

            int[] matrixBegin = new int[4 + 1] { 0, 2, 5, 7, 10 };
            int[] matrixCount = new int[4] { 2, 3, 2, 3 };
            int[] matrixindex = new int[10] { 2, 4, 0, 1, 3, 1, 3, 0, 2, 4 };  // notare che sono i clusters ordinati cresenti 
            double[] matrixval = new double[10] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            w.initSolver(false);

            WrapProblem prob = w.createProblem("prob", false);

            w.loadProblem(prob, colnum, rownum, nzc, 0, objectSense, 0, objectCoeffs, null, null, rowType, rhsValues, null, matrixBegin, matrixCount, matrixindex, matrixval, null, null, "");

            w.optimizeLpProblem(prob);

            double obj = w.getObjectValue(prob);
            double[] solvars = new double[4]; //Vettore deputato a contenere i valori della soluzione
            w.getSolutionValues(prob, solvars, null, null, null);
            for (int i = 0; i < solvars.Length; i++)
                Console.WriteLine("{0} solvar {1}", solvars[i], i);
            Console.WriteLine("VALORE OTTIMO TROVATO :  {0} ", obj);

        }
        
        private static void TestPMedWrapper_Large()
        {
            //risolviamo il Pmed1.txt in large!!! 10.000

            PMed3 pmed3;
            uint nsol = 2;
            try
            {
                pmed3 = new PMed3(Console.Write, nsol, "TestWrap", false, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("exception: {0}", e.Message);
                Console.WriteLine("{0}", Directory.GetCurrentDirectory());
                return;
            }
//#if DEBUG
            int m=0;
            if (!pmed3.Load(@"..\..\..\pmed1.txt",ref m))
            {
                Console.WriteLine("ERROR LOAD FILE!!!");
                return;
            }

            pmed3.ComputeSolutions(false, false);
            //pmed3.InitProblem();
//#endif

            //pmed3.InitProblem();
            //pmed3.BuildSolutionFromWrapper();

            Console.ReadKey();
            pmed3.ShowWrapperSolution(Console.Write);
            pmed3.CheckWrapperClusterSolution();
        }

        private static void TestPMedWrapper3CCS()
        {
            //Problema da testare : 
            //ora proviamo con costo cluster...

            //Sol0 = {4,2} ; {1,0,3}
            //Sol1 = {3,1} ; {4,2,0}

            //C(Sol0) = {0,16}=16 ; {0,15,40}=55  = 71
            //C(Sol1) = {0,40}=40 ; {0,16,24}=40  = 80

            //min z = 16*S0c0 + 55*S0c1+ 40*s1c0 + 40*s1c1
            //s.t.

            //    s0c0	s0c1	s1c0	s1c1
            //z	 16		55		40		40

            //0	 0		1		0		1	
            //1	 0		1		1		0
            //2	 1		0		0		1
            //3	 0		1		1		0
            //4	 1		0		0		1

            PMed3 pmed3;
            uint nsol = 2;
            try
            {
                pmed3 = new PMed3(Console.Write, nsol, "TestWrap", true, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("exception: {0}", e.Message);
                Console.WriteLine("{0}", Directory.GetCurrentDirectory());
                return;
            }
#if DEBUG
            //if (!pmed3.LoadTest(@"\\polocesena\polocesena\UtentiScienze\STUDENTI\raffaello.bertini\Desktop\AAA\Problems\pmedtest_verysmall.txt"))
            if (!pmed3.LoadTest(@"..\..\..\pmedtest_verysmall.txt"))
            {
                Console.WriteLine("ERROR LOAD FILE!!!");
                return;
            }

            pmed3.ComputeSolutions(false, false);
            //pmed3.InitProblem();
#endif

            //ora modifico le soluzioni a piacimento...
            pmed3.SolutionsClusters[0][0].Clear();
            pmed3.SolutionsClusters[0][0].Add(4);
            pmed3.SolutionsClusters[0][0].Add(2);

            pmed3.SolutionsClusters[0][1].Clear();
            pmed3.SolutionsClusters[0][1].Add(1);
            pmed3.SolutionsClusters[0][1].Add(0);
            pmed3.SolutionsClusters[0][1].Add(3);

            pmed3.SolutionsClusters[1][0].Clear();
            pmed3.SolutionsClusters[1][0].Add(3);
            pmed3.SolutionsClusters[1][0].Add(1);

            pmed3.SolutionsClusters[1][1].Clear();
            pmed3.SolutionsClusters[1][1].Add(4);
            pmed3.SolutionsClusters[1][1].Add(2);
            pmed3.SolutionsClusters[1][1].Add(0);

            //da rendere publici per testare...
            //pmed3.InitProblem();
            //pmed3.BuildSolutionFromWrapper();

            pmed3.ShowWrapperSolution(Console.Write);
            
            //double obj = w.getObjectValue(prob);
            //double[] solvars = new double[4]; //Vettore deputato a contenere i valori della soluzione
            //w.getSolutionValues(prob, solvars, null, null, null);
            //for (int i = 0; i < solvars.Length; i++)
            //    Console.WriteLine("{0} solvar {1}", solvars[i], i);
            //Console.WriteLine("VALORE OTTIMO TROVATO :  {0} ", obj);

        }
        private static void TestCOinMPWrapper()
        {
            //test risoluzione :
            //MAX z=x1+x2+x3+x4
            //s.t
            //x1 + x2 <=2
            //x2 + x3 <=3
            //x3 + x4 <=2

            //La matrice completa dovrebbe essere:
            //      x1	x2	x3	x4 | rhs
            //(1)	1	1	0	0  | 2
            //(2)	0	1	1	0  | 3
            //(3)	0	0	1	1  | 2

            Wrapper w = new Wrapper();
            //CCS ccs = new CCS();

            int objectSense = -1;

            int nzc = 6;
            int colnum = 4;
            int rownum = 3;

            //i coefficenti sono il peso delle xi, quindi essendo che ogni variabile a coefficente 1 ...
            double[] objectCoeffs = new double[4] {1,1,1,1};
            String rowType = "LLL";
            double[] rhsValues = new double[3] {2,3,2};
            
            int [] matrixBegin = new int[4+1] {0,1,3,5,6};
            int [] matrixCount = new int[4] {1,2,2,1};
            int [] matrixindex = new int[6] {0,0,1,1,2,2};
            double[] matrixval = new double[6] {1,1,1,1,1,1};

            w.initSolver(false);

            WrapProblem prob = w.createProblem("prob",false);

            w.loadProblem(prob,colnum,rownum,nzc,0,objectSense,0,objectCoeffs,null,null,rowType,rhsValues,null,matrixBegin,matrixCount,matrixindex,matrixval,null,null,"");

            w.optimizeLpProblem(prob);

            double obj = w.getObjectValue(prob);
            double[] solvars = new double[colnum]; //Vettore deputato a contenere i valori della soluzione
            w.getSolutionValues(prob, solvars, null, null, null);
            for (int i = 0; i < solvars.Length; i++)
                Console.WriteLine("{0} solvar {1}", solvars[i], i);
            Console.WriteLine("VALORE OTTIMO TROVATO :  {0} ", obj);

        }
        private static void TestCOinMPWrapper_DOC()
        {
            Wrapper w = new Wrapper();

            String problemName = "CoinTest";
            int colCount = 8;
            int rowCount = 5;
            int nonZeroCount = 14;
            String objectName = "obj";
            int objectSense = -1;
            double[] objectCoeffs = new double[8] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };
            double[] lowerBounds = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] upperBounds = new double[8] { 1000000.0, 1000000.0, 1000000.0,1000000.0, 1000000.0, 1000000.0,1000000.0, 1000000.0 };
            String rowType = "LLLLL";
            double[] rhsValues = new double[5] { 14.0, 80.0, 50.0, 50.0, 50.0 };
            int[] matrixBegin = new int[8 + 1] { 0, 2, 4, 6, 8, 10, 11, 12, 14 };
            int[] matrixCount = new int[8] { 2, 2, 2, 2, 2, 1, 1, 2 };
            int[] matrixIndex = new int[14] { 0, 4, 0, 1, 1, 2, 0, 3, 0, 4, 2, 3, 0, 4 };
            double[] matrixValues = new double[14] { 3.0, 5.6, 1.0, 2.0, 1.1, 1.0, -2.0,2.8, -1.0, 1.0, 1.0, -1.2, -1.0, 1.9};
            String[] colNames = new String[8] { "c1", "c2", "c3", "c4", "c5", "c6", "c7","c8" };
            String[] rowNames = new String[5] { "r1", "r2", "r3", "r4", "r5" };


            w.initSolver(false);


            WrapProblem prob = w.createProblem(problemName, false);

            w.loadProblem(prob, colCount, rowCount, nonZeroCount, 0, objectSense, 0,objectCoeffs, lowerBounds, upperBounds, rowType, rhsValues,null, matrixBegin, matrixCount, matrixIndex, matrixValues,colNames, rowNames, objectName);

            Console.WriteLine("OPTIMIZZA!!!");
            w.optimizeLpProblem(prob);
            Console.WriteLine("FINE OTTIMIZZA!!!");
            double obj = w.getObjectValue(prob);
            double[] solvars = new double[colCount]; //Vettore deputato a contenere i valori della soluzione
            w.getSolutionValues(prob, solvars, null, null, null);
            for (int i = 0; i < solvars.Length; i++)
                Console.WriteLine("{0} solvar {1}", solvars[i], i);
            Console.WriteLine("VALORE OTTIMO TROVATO :  {0} ", obj);
        }

        private void FirstStep()
        {
            Console.WriteLine("P-Median Project. Console Version.\r\nBy Raffaello Bertini.");

            PMed1 pmed = new PMed1(Console.Write);
            int m = 0;
            if (!pmed.Load(@"C:\Users\Raffaello\Documents\Uni\AAA\Proj_Exam\Problems\pmed1.txt",ref m))
            {
                return;
            }

            Console.WriteLine(@"
VERTICES : {0}
EDGES    : {1}
p        : {2}
", pmed.nVerteces, pmed.nEdges, pmed.p);
            int i, j, k;
            uint sumr, sumc;

            for (j = 0; j < pmed.p; j++)
            {
                k = 0;
                for (i = 0, sumr = sumc = 0; i < pmed.nVerteces; i++)
                {
                    //if (pmed.isp[i])
                    //    continue;
                    sumr += pmed.c[pmed.Clusters[j].First(), i];
                    sumc += pmed.c[i, pmed.Clusters[j].First()];
                    k++;
                }
                Console.WriteLine("{2}. {3} sumr {0} --- sumc {1} --- k = {4}", sumr, sumc, j, pmed.Clusters[j].First(), k);
            }

            Console.WriteLine("Soluzione = {0}", pmed.solution);
 
            //Console.WriteLine(watch1.Elapsed.ToString());
            //for (i = 0; i < pmed.p; i++)
            //    Console.WriteLine("Clusters gap[{0}] = {1}", i, gap[i]);
            Console.WriteLine("Solution* = {0}", pmed.solution);

        }
    }
}
