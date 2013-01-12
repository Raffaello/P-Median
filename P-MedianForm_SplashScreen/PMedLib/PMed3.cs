//*********************************************************************
//****   P-Median DLL Library                                      ****
//****   Author : Raffaello Bertini (raffaellobertini@gmail.com)   ****
//****   File: PMed3.cs  (Class for Solve LP from N Heuristic      ****
//****                    Solution Inerithed from PMed2)           ****
//****-------------------------------------------------------------****
//****                    Version History:                         ****
//****   1.00 September 2011                                       ****
//*********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoinCplexWrapper;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PMedLib
{
    /// <summary>
    /// definisce i tipi di problemi di minimizzazione risolvibili.
    /// </summary>
    public enum eProblemType
    {
        /// <summary>
        /// Set Covering (faster, default)
        /// </summary>
        SetCovering,
        /// <summary>
        /// Set-Partitioning (slower)
        /// </summary>
        SetPartitioning
    }
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class PMed3 : PMed2
    {
 
        #region Private Variables

        /// <summary>
        /// per misurare il tempo....
        /// </summary>
        Stopwatch fase3;

        /// <summary>
        /// perchè altrimenti cambia directory con il caricamento dei file e non trova più la dll dopo...
        /// </summary>
        String CurDir;

        /// <summary>
        /// [Wrapper] indici delle soluzione del set-covering (sono quante è p)
        /// </summary>
        private int[] indexsolvar;

        /// <summary>
        /// [Wrapper] vettore in cui memorizzo i coefficenti della soluzione delle variabili.
        /// </summary>
        private double[] solvars;

        /// <summary>
        /// [Wrapper]  Wrapper.
        /// </summary>
        private Wrapper wrapper;

        /// <summary>
        /// [Wrapper]  nome del problema de Wrapper
        /// </summary>
        private WrapProblem problem;

        /// <summary>
        /// [Wrapper] stringa del nome del problema del wrapper.
        /// </summary>
        private string problem_name;

        /// <summary>
        /// [Wrapper] false = CoinMP ; true = CPLEX
        /// </summary>
        private bool CPLEX;

        /// <summary>
        /// is wrapper inited?
        /// </summary>
        private bool inited;

        /// <summary>
        /// is wrapper loaded?
        /// </summary>
        private bool loaded;

        /// <summary>
        /// is solutions_computed?
        /// </summary>
        private bool solutions_computed;

        /// <summary>
        /// [Wrapper] Oggetto che builda la CCS matrix che, utilizzato nel wrapper c#
        /// </summary>
        private CCS ccs;

        /// <summary>
        /// [Wrapper] Nome della funzione Oggetto.
        /// </summary>
        private const string ObjName = "Obj";

        /// <summary>
        /// [Wrapper] Minimizzare la funzione oggetto. se -1 invece Massimizzare.
        /// </summary>
        private const int ObjSense = 1; //problema di minimo.

        /// <summary>
        /// [Wrapper] vetttore dei coefficenti della funzione oggetto.
        /// </summary>
        private double[] ObjCoeffs;

        private double[] upperBound;
        private double[] lowerbound;
        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_marker"></param>
        /// <returns></returns>
        private bool CheckWrapperClustersSolution(out uint[] node_marker)
        {
            node_marker = null;

            if (!solutions_computed)
                return false;

            if (WrapperSolutionClusters == null)
                return false;

            bool ok = true;
            uint[] node = new uint[nVerteces];

            for (int i = 0; i < nVerteces; i++)
                node[i] = 0;

            for (int j = 0; j < p; j++)
            {
                foreach (uint l in WrapperSolutionClusters[j])
                    node[l]++;
            }

            for (int i = 0; i < nVerteces; i++)
            {
                if (node[i] != 1)
                {
                    if (Verbose)
                        Write(String.Format("\r\nThe node {0} is used  {1} times", i, node[i]));
                    ok = false;
                }
            }

            node_marker = node;
            return ok;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildSolutionFromWrapper()
        {
            if (!solutions_computed)
                return;
#if DEBUG
            int[] intsolvars = new int[solvars.Length];
            int countnz = 0;

            for (int i = 0; i < solvars.Length; i++)
            {
                intsolvars[i] = (int)(solvars[i] + 0.5);
                if (intsolvars[i] > 0)
                    countnz++;
            }

            
            if (countnz != p)
                Write("\r\nERROR! Debug BuildSolutionFromWrapper(), intsolvars");
#endif
            indexsolvar = new int[p];

            for (int i = 0; i < p; i++)
                indexsolvar[i] = -1;

            int k = 0;
            for (int i = 0; i < solvars.Length; i++)
            {
                if (solvars[i] == 1.0)
                    indexsolvar[k++] = i;
                if (k == p)
                    break;
            }
#if DEBUG
            uint wrapsol = 0;
#endif
            if(k<p)
            {
                if (MaxIterSecond == 0.0)
                    Write("\r\n No solution found ERROR!!!");
                else
                    Write("\r\n No solution found. Time Exausted!");
                return;
            }
            WrapperSolutionClusters = new List<uint>[p];
            k = 0;
            for (int i = 0; i < p; i++)
            {
                uint ip1, ip2;
                ip1 = (uint)indexsolvar[i] / p;
                ip2 = (uint)indexsolvar[i] % p;

                Write(String.Format("\r\nWrapper Cluster [{0}] = S{1}C{2} --- Length = {3}", i, ip1, ip2,SolutionsClusters[ip1][ip2].Count));
                WrapperSolutionClusters[k++] = SolutionsClusters[ip1][ip2];
#if DEBUG
                wrapsol += ComputeClusterCost(ip1, ip2);
#endif
            }
#if DEBUG
            if (WrapperSolution != wrapsol)
            {
                Write("\r\n ERROR!!! DEBUG: WrapSol != wrapsol ");
                Write(String.Format("\r\n wrapsol = {0}  ---- Wrappersol = {1}", wrapsol, WrapperSolution));
            }
#endif
            //Write(String.Format("\r\nCosto nuova soluzione = {0}", WrapperSolution));

#if DEBUG
            uint qi = 0;
            for (uint i = 0; i < p; i++)
            {
                qi = ComputeClusterQ(WrapperSolutionClusters, i);
                if (qi > Q)
                    Write(String.Format("\r\n\r\n\r\nDEBUG : qi={0} > Q={1} \r\n\r\n\r\n", qi, Q));
            }
#endif
        }

        /// <summary>
        /// Initializza la DLL. Utilizzato la prima volta dal costruttore. 
        /// e richiamato le volte successive per gestione del wrapper e memoria unsafe
        /// </summary>
        /// <param name="_problem_name"></param>
        /// <param name="_CPLEX"></param>
        private void InitDLL(string _problem_name = "PMedProb", bool _CPLEX = false)
        {
            if (loaded)
            {
                wrapper.unloadProblem(problem);
                loaded = false;
            }
            if (inited)
            {
                wrapper.closeSolver(_CPLEX);
                inited = false;
            }
            wrapper = new Wrapper();
            this.CPLEX = _CPLEX;
            loaded = false;
            inited = false;

            //usato perchè se si utilizza la dll in "directory corrente"
            //l'openfiledilalog dell' os, cambia la "directory corrente"
            //a volte.
            //si può anche rimuovere (forse)
            string dir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(CurDir);
            
            //commentato perchè modificato il wrapper. 
            //da testare se problemi con dll scommentare.
            //if ((!CPLEX) && (File.Exists(CurDir+"\\CoinMP.dll")))
            //    wrapper.initSolver(false);
            //else if ((CPLEX) && (File.Exists("CPLEX101.dll")))
            //else if (CPLEX)
                //wrapper.initSolver(true);
           // else
           //     throw new DllNotFoundException();
            wrapper.initSolver(_CPLEX);
            
            problem = wrapper.createProblem(problem_name = _problem_name, _CPLEX);
            inited = true;

            //ripristino la dir corrente.
            Directory.SetCurrentDirectory(dir);
        }

        /// <summary>
        /// Comtrolla che il tableau sia stato costruito bene.
        /// </summary>
        [Conditional("DEBUG")]
        private void CheckTableau()
        {
            uint nonzero = 0;
            for (uint i = 0; i < this.nVerteces; i++)
            {
                for (uint j = 0; j < this.Nsolution; j++)
                {
                    for (uint k = 0; k < this.p; k++)
                    {
                        foreach (uint l in this.SolutionsClusters[j][k])
                        {
                            if (i == l)
                            {
                                nonzero++;
                                break;
                            }
                        }
                    }
                }
            }
            //se... error
            if (Verbose)
                Write("\r\nCheck tableau non zero elements...");
            if (nonzero != ccs.nonZeroCount)
            {
                Write(String.Format("\r\nDEBUG CHECK: NonZeroCount={0} <> {1}=Check . Error!!!", ccs.nonZeroCount, nonzero));
            }
            if (Verbose)
                Write("\r\nDone!");
        }

        /// <summary>
        /// [Wrapper] costruisce la matrice in formato CCS.
        /// </summary>
        private bool BuildCCS()
        {
            try
            {
                //dalla matrice dei costi a ccs
                ccs = new CCS(this.SolutionsClusters,
                            this.Nsolution,
                            this.p,
                            this.nVerteces,
                            ProblemType);

                ObjCoeffs = new double[ccs.colCount];
 
                for (int i = 0; i < ccs.colCount; i++)
                {
                    uint i1 = (uint)i / this.p;
                    uint i2 = (uint)i % this.p;

                    //costo cluster
                    ObjCoeffs[i] = (double)ComputeClusterCost(i1, i2);//upperBounds[i];
                }
                lowerbound = null;
                upperBound = null;

#if DEBUG
            CheckTableau();
#endif
            }
            catch (Exception e)
            {
                Write(String.Format("\r\nException Build CCS : {0}", e.Message));
                return false;
            }

            return true;
        }

        /// <summary>
        /// [Wrapper] Callback per il MipNode. Usata internamente dal wrapper per CoinOR.
        /// (per CPLEX, non è implementato al momento)
        /// </summary>
        /// <param name="IterCount"></param>
        /// <param name="MipNodeCount"></param>
        /// <param name="BestBound"></param>
        /// <param name="BestInteger"></param>
        /// <param name="IsMipImproved"></param>
        /// <returns></returns>
        private int MipNodeCB(int IterCount,
                int MipNodeCount,
                double BestBound,
                double BestInteger,
                int IsMipImproved)
        {
            WriteLine(String.Format("NODE: iter={0:d}, node={1:d}, bound={2:f}, best={3:f}, {4}",
                IterCount, MipNodeCount, BestBound, BestInteger, IsMipImproved >0 ? "Improved" : ""));
            return 0;
        }


        /// <summary>
        /// Prova a recuperare la memoria non gestita, quando il wrapper "crasha".
        /// E' CONSIGLIATO SE SI VERIFICA DI RE INITIALIZZARE L'INTERO OGGETTO!!!
        /// (Ci sono dei BUGS in Coin-Or.dll, 
        /// indici con tip do dato int,
        /// che ad un certo punto, diventano negativi quando il problema è grande)
        /// bisogna compilare CoinOR con un define di CoinBigIndex=2,
        /// ma c'è un'altro errore nel codice sorgente, altri sottoprogetti utilizzano
        /// direttamente il tipo di dato int, invecde del tipo di dato CoinBig Index
        /// </summary>
        private void RecoveryMemoryFromWrapperProblem()
        {
            try
            {
                Write(String.Format("\r\n *** Trying unloading wrapper problem from unmanaged memory..."));
                //wrapper.unloadProblem(problem);
                InitDLL();
                Write(String.Format(" OK!"));
                //loaded = false;
            }
            catch (Exception ex)
            {
                Write(String.Format("\r\n !!!   Error   !!!! \r\n{0}\r\n{1}\r\n{2}", ex.Message, ex.Source, ex.StackTrace));
            }
        }

        /// <summary>
        /// Initializza il problema.
        /// </summary>
        /// <returns></returns>
        private bool InitProblem()
        {
            if (loaded)
            {
                wrapper.unloadProblem(problem);
                loaded = false;
            }
            if (solutions_computed)
            {
                InitDLL(problem_name, CPLEX);
                solutions_computed = false;
            }
            Write("\r\nInitializing ");
            switch(ProblemType)
            {
                case eProblemType.SetCovering:
                    Write("Set-Covering Problem...\r\n");
                    break;
                case eProblemType.SetPartitioning:
                    Write("Set-Partitioning Problem...\r\n");
                    break;
                default:
                    break;
            }
            
            if ((this.SolutionsClusters == null) || (this.SolutionsClusters[this.Nsolution - 1] == null))
            {
                if (Verbose)
                    Write("\r\nCannot Initialize Problem. No Solutions was found!");

                return false;
            }
            if (Verbose)
                Write("\r\n--- Building tableau...\r\n");

            if (!BuildCCS())
                return false;
#if DEBUG
            if (Verbose)
            {
                //Stampo informazioni del modello matematico.
                Write("\r\nObjCoeff   = ");
                for (uint i = 0; i < this.ObjCoeffs.Length; i++)
                    Write(String.Format("{0}, ", this.ObjCoeffs[i]));
            }
#endif
            try
            {
                //setto quanti secondi max.
                if (MaxIterSecond > 0)
                {
                    if (this.CPLEX)
                    {
                        //da fare per cplex...
                    }
                    else
                    {
                        //wrapper.setRealParam(problem, 16, MaxIterSecond);
                        wrapper.setRealParam(problem, (int) Wrapper.CoinOptions.COIN_REAL_MAXSECONDS,MaxIterSecond);
                        //wrapper.setRealParam(problem, 19, MaxIterSecond);
                        wrapper.setRealParam(problem, (int) Wrapper.CoinOptions.COIN_REAL_MIPMAXSEC, MaxIterSecond);
                    }
                }
                //wrapper.loadProblem(problem, (int)ccs.colCount, (int)ccs.rowCount, (int)ccs.nonZeroCount,
                //                   0, ObjSense, 0, ObjCoeffs, null, null,
                //                   ccs.rowType, ccs.rhsValues, null,
                //                   ccs.matrixBegin, ccs.matrixCount, ccs.matrixIndex, ccs.matrixValues,
                //                   null, null, ObjName);

                wrapper.loadProblem(problem, (int)ccs.colCount, (int)ccs.rowCount, (int)ccs.nonZeroCount,
                                   0, ObjSense, 0, ObjCoeffs, lowerbound, upperBound,
                                   ccs.rowType, ccs.rhsValues, null,
                                   ccs.matrixBegin, ccs.matrixCount, ccs.matrixIndex, ccs.matrixValues,
                                   null, null, ObjName);

                //imposto il vincolo che somma di Xij=p 
                #region Da_usare_con_CCS.ConvertToCCS
                
                
                //double[] xij = new double[ObjCoeffs.Length];
                //for(int i =0; i<ObjCoeffs.Length;i++)
                //    xij[i]= 1;
                //wrapper.addrow(problem, this.p, xij, "E", "Xij=p");
                #endregion

                //imposto i vincoli di binarietà, ovvero le Xij appartengono {0,1} :)
                String colType = new String('B', ObjCoeffs.Length);
                wrapper.loadInteger(problem, colType);
                
                loaded = true;
                
                try
                {
                    //Setto la callback                    
                    //questo if ci vuole perchè la coin dll ha sia la printf che la callback e scrive 2 volte su video.
                    //è da ritenere come bug della libreria coinOR
                    //perchè bastava avere che chiamasse solo la callback e 
                    //di default usa una sua funzione interna con printf! et woilà!
                    if (WriteInto != WriteInfoenum.Console)
                    {
                        wrapper.SetMsgLogCallBack(problem, WriteLine);
                        wrapper.SetMipNodeCallBack(problem, MipNodeCB);
                    }
                    
                    //setto il log level... (in alternativa al 3 c'è 2 o 1)
                    if (this.CPLEX)
                    {
                        // To do... eventualmente...

                    }
                    else
                    {
                        if (Verbose)
                            //wrapper.setIntParam(problem, 7, 4);
                            wrapper.setIntParam(problem, (int)Wrapper.CoinOptions.COIN_INT_LOGLEVEL, (int)Wrapper.CoinLogLevel.Verbose);
                        else
                            //wrapper.setIntParam(problem, 7, 3);
                            wrapper.setIntParam(problem, (int)Wrapper.CoinOptions.COIN_INT_LOGLEVEL, (int)WrapperVerboseLevel);
                    }

                    wrapper.optimizeMipProblem(problem);
                    solutions_computed = true;

                    WrapperSolution = (uint)Math.Round(wrapper.getObjectValue(problem));
                    solvars = new double[ObjCoeffs.Length];
                    wrapper.getSolutionValues(problem, solvars, null, null, null);

                    //dovrebbe essere Nsol * p (ovvero tutti i clusters)
                    if (Verbose)
                    {
                        Write(String.Format("\r\n\r\nSolution costs = {0}", WrapperSolution));
                        Write(String.Format("\r\nNum SolVar = {0}", ObjCoeffs.Length));
                        Write(String.Format("\r\n used {0} cluster; p = {1}", solvars.Sum(), p));
#if DEBUG
                        //Write("\r\nSolVar = ");
                        //for (int i = 0; i < ObjCoeffs.Length; i++)
                        //    Write(String.Format("{0,2}, ", solvars[i]));
#endif
                    }
                }
                //catch( AccessViolationException ave)
                catch (SEHException sehe)
                {
                    Write(String.Format("\r\n{0}\r\n{1}\r\n{2}", sehe.Message, sehe.Source, sehe.StackTrace));
                    Write(String.Format("\r\nError Code = 0x{0:X}", sehe.ErrorCode));
                    Write(String.Format("\r\nHRESULT = {0}", sehe.ToString()));

                    RecoveryMemoryFromWrapperProblem();
                    
                    return false;
                }
                catch (Exception e)
                {
                    Write(String.Format("\r\n{0}\r\n{1}\r\n{2}", e.Message, e.Source, e.StackTrace));
                    
                    RecoveryMemoryFromWrapperProblem();

                    return false;
                }
             }
            catch (Exception e)
            {
                Write(String.Format("\r\n{0}", e.Message));
                return false;
            }
            return true;
        }

        

        /// <summary>
        /// Main Metodo del ShowTableau.
        /// </summary>
        private void ShowTableau()
        {
            uint i;
            uint j;

            if (this.WriteInto == WriteInfoenum.Console)
                Write("\r\n\r\nWARNING!!! : Not sure if the tableau is displayed correctly, because of column limitation due to Console Window limitation!!!\r\n");

            //intestazione colonna
            Write("\r\n    ");
            for (i = 0; i < ccs.colCount; i++)
            {
                uint i1 = i / this.p; //soluzione
                uint i2 = i % this.p; //cluster soluzione

                Write(String.Format("|s{0,-5}c{1}", i1, i2));
            }
            Write("| RHS\r\n----");
            for (i = 0; i < ccs.colCount; i++)
                Write("---------");
            Write("\r\n");

            //stampo le righe...
            for (i = 0; i < ccs.rowCount + 2; i++)
            {
                if (i == ccs.rowCount)
                    Write("--------|");
                else if (i == ccs.rowCount + 1)
                    Write("Cost|");
                else
                    Write(String.Format("{0,4}|", i));

                for (j = 0; j < ccs.colCount; j++)
                {
                    uint j1 = j / this.p; //soluzione
                    uint j2 = j % this.p; //cluster soluzione

                    if (i == ccs.rowCount)
                        Write("--------|");
                    else if (i == ccs.rowCount + 1)
                    {
                        Write(String.Format("{0,8}|", ComputeClusterCost(j1, j2)));
                    }
                    else
                    {
                        bool one = false;
                        if (ccs.rowCount == i + 1)
                        {
                            one = true;
                        }
                        else
                        {
                            
                            foreach (uint l in SolutionsClusters[j1][j2])
                            {
                                if (l == i)
                                {
                                    one = true;
                                    break;
                                }
                            }
                        }

                        if (one)
                            Write(String.Format("{0,8}|", 1));
                        else
                            Write(String.Format("{0,8}|", 0));
                        
                    }
                }
                //Stampo RHS
                if (i < ccs.rowCount)
                    Write(String.Format("{0,3}", ccs.rhsValues[i]));

                Write("\r\n");
            }
        }

        /// <summary>
        /// Destructor. Deinitialize the wrapper.
        /// </summary>
        ~PMed3()
        {
            if (loaded)
            {
                wrapper.unloadProblem(problem);
                loaded = false;
            }

            if (inited)
            {
                wrapper.closeSolver(CPLEX);
                inited = false;
            }
        }

        #endregion

        #region Public Variables

        /// <summary>
        /// 
        /// </summary>
        public Wrapper.CoinLogLevel WrapperVerboseLevel { get; private set; }

        /// <summary>
        /// problema di minimizzazione da risolvere.
        /// </summary>
        public eProblemType ProblemType { get; set; }

        /// <summary>
        /// if set to 0.0 is not set...
        /// Set in seconds 1 minutes = 60 sec and so on...
        /// </summary>
        public double MaxIterSecond { get; set; }
                
        /// <summary>
        /// [Wrapper] valore della soluzione trovata dal wrapper.
        /// </summary>
        public uint WrapperSolution { get; private set; }

        /// <summary>
        /// la soluzione del wrapper, dove vado ad inserire i cluster precedentementi trovati, per praticità e comodità.
        /// </summary>
        public List<uint>[] WrapperSolutionClusters { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Costruttore. throws a DDLNotFoundException if neither CoinOp or CPLEX was founnd.
        /// </summary>
        /// <param name="_w">WriteDelegate</param>
        /// <param name="_problem_name"> nome problema</param>
        /// <param name="N">number of solutions</param>
        /// <param name="verbose">verbose results</param>
        /// <param name="CPLEX">use CPLEX? else CoinOP</param>
        /// <param name="pt">SetCovering or SetPartitioning?</param>
        public PMed3(WriteDelegate _w, uint N, string _problem_name = "PMedProb", bool verbose = false, bool CPLEX = false, eProblemType pt=eProblemType.SetCovering)
            : base(_w, N, verbose)
        {
            fase3 = new Stopwatch();
            MaxIterSecond = 0.0;
            CurDir = Directory.GetCurrentDirectory();
            InitDLL(_problem_name, CPLEX);
            ProblemType = pt;
            WrapperVerboseLevel = Wrapper.CoinLogLevel.JustFactorizationsAndMore;
        }

        /// <summary>
        /// Stampa la matrice in formato CCS. Ovvero i suoi vettori che la compongono
        /// </summary>
        public void ShowCCS(bool onfile=false)
        {
            if (ccs == null)
                return;
            if (ccs.matrixBegin == null)
            {
                Write("\r\nNot Initialized CCS Matrix Format (matrixBegin)!!!");
                return;
            }
            if (ccs.matrixCount == null)
            {
                Write("\r\nNot Initialized CCS Matrix Format (matrixCount)!!!");
                return;

            }
            if (ccs.matrixIndex == null)
            {
                Write("\r\nNot Initialized CCS Matrix Format (matrixIndex)!!!");
                return;

            }
            if (ccs.matrixValues == null)
            {
                Write("\r\nNot Initialized CCS Matrix Format (matrixValues)!!!");
                return;
            }

            uint i;
            TextWriter tw=null;
            WriteDelegate wo = this.Write;

            if (onfile)
            {
                tw = File.CreateText("CCS.DBG");
                this.Write = tw.Write;
            }

            Write("\r\nMatrixBegin  = ");
            for (i = 0; i < ccs.matrixBegin.Length; i++)
                Write(String.Format("{0}, ", ccs.matrixBegin[i]));
            Write("\r\nMatrixCount  = ");
            for (i = 0; i < ccs.matrixCount.Length; i++)
                Write(String.Format("{0}, ", ccs.matrixCount[i]));
            Write("\r\nMatrixIndex  = ");
            for (i = 0; i < ccs.matrixIndex.Length; i++)
                Write(String.Format("{0}, ", ccs.matrixIndex[i]));
            Write("\r\nMatrixValues = ");
            for (i = 0; i < ccs.matrixValues.Length; i++)
                Write(String.Format("{0}, ", ccs.matrixValues[i]));

            Write(String.Format("\r\n MatrixBegin  lenght = {0}", ccs.matrixBegin.Length));
            Write(String.Format("\r\n MatrixCount  lenght = {0}", ccs.matrixCount.Length));
            Write(String.Format("\r\n MatrixIndex  lenght = {0}", ccs.matrixIndex.Length));
            Write(String.Format("\r\n MatrixValues lenght = {0}", ccs.matrixValues.Length));

            if (onfile)
            {
                tw.Close();
                this.Write = wo;
            }
        }

        /// <summary>
        /// Per visualizzare la soluzione in formato Tableau del Wrapper
        /// </summary>
        /// <param name="Write"></param>
        public void ShowWrapperSolution(WriteDelegate Write)
        {
            if (!solutions_computed)
                return;

            if (WrapperSolutionClusters == null)
                return;

            uint i;
            uint j;

            if (this.WriteInto == WriteInfoenum.Console)
                Write("\r\n\r\nWARNING!!! : Not sure if the tableau is displayed correctly, because of column limitation due to Console Window limitation!!!\r\n");

            //intestazione colonna
            Write("\r\n    ");
            for (i = 0; i < p; i++)
                Write(String.Format("|swrap c{0}", i));
 
            Write("| RHS\r\n----");
            for (i = 0; i < p; i++)
                Write("---------");
 
            Write("\r\n");

            //stampo le righe...
            for (i = 0; i < nVerteces+2; i++)
            {
                if (i == nVerteces)
                    Write("----|");
                else if (i == nVerteces + 1)
                    Write("Cost|");
                else
                    Write(String.Format("{0,4}|", i));

                for (j = 0; j < p; j++)
                {
                    if (i == nVerteces)
                        Write("--------|");
                    else if (i == nVerteces + 1)
                    {
                             Write(String.Format("{0,8}|", ComputeWrapperClusterCost(j)));
                    }
                    else
                    {
                        bool one = false;
                        foreach (uint l in WrapperSolutionClusters[j])
                        {
                            if (l == i)
                            {
                                one = true;
                                break;
                            }
                        }
                        if (one)
                            Write(String.Format("{0,8}|", 1));
                        else
                            Write(String.Format("{0,8}|", 0));
                    }
                }
                Write("\r\n");
            }
        }

        /// <summary>
        /// Interfaccia esterna per il tableau
        /// si potrebbe mettere come parametro WriteDelegate, così da poter scegliere
        /// dove poter stampare..
        /// permette di scegliere se stampare come file di debug o su WriteDelegate.
        /// </summary>
        /// <param name="onfile"></param>
        public void ShowTableau(bool onfile=false)
        {
            WriteDelegate WriteOld=null;
            TextWriter tr=null;
            
            if (onfile)
            {
                WriteOld = this.Write;
                tr = File.CreateText("Tableau.dbg");
                this.Write = tr.Write;
            }
            
            ShowTableau();

            if (onfile)
            {
                this.Write = WriteOld;
                tr.Close();
            }
        }

        /// <summary>
        /// setta quante soluzioni genereare.
        /// </summary>
        /// <param name="N">numero soluzioni</param>
        public new void SetSolutions(uint N)
        {
            WrapperSolution = 0;
            WrapperSolutionClusters = null;

            base.SetSolutions(N);
            
        }
        /// <summary>
        /// Evolve il precedente ComputeSolutions. Calcolandolo e poi facendo il Set-Covering.
        /// </summary>
        /// <param name="AsParallel"></param>
        /// <param name="internalParallel"></param>
        /// <returns></returns>
        public new bool ComputeSolutions(bool AsParallel, bool internalParallel)
        {
            bool ret;

            fase3.Restart();
            if (Nsolution == 0)
                return false;
            WrapperSolution = 0;
          
            if (!base.ComputeSolutions(AsParallel, internalParallel))
                return false;
            
            ret = InitProblem();
            
            //controllo se la soluzione è ammissibile...
            FinalizeWrapperSolution();

            if (solutions_computed)
            {
                if ((WrapperSolution < bestsol) && (WrapperSolution > 0))
                {
                    Write(String.Format("\r\n---The solution is improved = {0} --- {1}", WrapperSolution, bestsol));
                    //solution = WrapperSolution;
                }
                else
                    Write(String.Format("\r\n--- The solution is NOT improved = {0} --- {1}", WrapperSolution, bestsol));

                BuildSolutionFromWrapper();
                //Write("\r\nUnloading Problem...");
                //wrapper.unloadProblem(problem);
                //loaded = false;
            }
            else
            {
                Write("\r\n!!! Set Covering Solution not computed! !!!");
            }
            
            fase3.Stop();
            //Write("\r\n End Computation.");

            return ret;
        }


        /// <summary>
        /// Controlla ed eventualmente sistema la wrapper solution.
        /// </summary>
        private void FinalizeWrapperSolution()
        {
            uint[] node_marker;
            if ((!CheckWrapperClustersSolution(out node_marker))&&(node_marker!=null))
            {
                //Se non lo è, devo fare in modo che lo sia.
                Write("\r\nWrapper Solution is not admissible,\r\ntrying to recovery and improve it...");
               
                
                //conto quanti ne ho da cambiare...
                uint count = 0;
                for (uint i = 0; i < node_marker.Length; i++)
                    if (node_marker[i] > 1)
                        count++;

                while (count > 0)
                {
                    //marko l'indice del nodi che sono > 1
                    //fatto nel check...

                    //rimuovo tanti nodi finchè non ne ho 1, 
                    //controllando qual'è più conveniente da rimuovere
                    for (uint i = 0; (i < node_marker.Length) && (count > 0); i++)
                    {
                        if (node_marker[i] > 1)
                        {
                            //devo trovare i nodi corrispondenti nella soluzione
                            int[] index_node;
                            int[] index_p;

                            index_node = new int[node_marker[i]];
                            index_p = new int[node_marker[i]];
                            uint k = 0;
                            for (int j = 0; j < index_node.Length; j++)
                                index_node[j] = -1;

                            for (int j = 0; j < p; j++)
                            {
                                index_node[k] = WrapperSolutionClusters[j].IndexOf(i);

                                if (index_node[k] >= 0)
                                {
                                    //l'ho trovato...
                                    index_p[k++] = j;
                                    if (k == node_marker[i])
                                        break;
                                }
                            }

                            //dovrei aver trovato tutti i nodi di surplus ora li rimuovo "intelligentemente"
                            List<uint>[] clusters_temp = new List<uint>[p];
                            uint[] Weight = new uint[node_marker[i]];
                            k = 0;
                            for (int j = 0; j < index_node.Length; j++)
                            {
                                clusters_temp[k] = new List<uint>(WrapperSolutionClusters[index_p[j]]);
                                Weight[k] = ComputeClusterCost(clusters_temp, k);

                                clusters_temp[k].RemoveAt(index_node[j]);
                                Weight[k] -= ComputeClusterCost(clusters_temp, k);
                                k++;
                            }

                            uint[] weight_index_sorted = new uint[Weight.Length];
                            for (uint j = 0; j < Weight.Length; j++)
                                weight_index_sorted[j] = j;
                                Array.Sort(Weight, weight_index_sorted);
                            //ora rimuovo fino a ... dal maggiore (partendo dal fondo dell'array)
                            for (int j = weight_index_sorted.Length - 1; (j >= 0) && (count > 0); j--)
                            {
                                k = weight_index_sorted[j];
                                uint sumQ = ComputeClusterQ(clusters_temp, k);
                                if (sumQ <= Q)
                                {
                                    WrapperSolutionClusters[index_p[k]] = clusters_temp[k];
                                    count--;
                                }
                            }
                        }
                    }

                    //ricontrollo la soluzione.
                    CheckWrapperClustersSolution(out node_marker);
                    count = 0;
                    for (uint i = 0; i < node_marker.Length; i++)
                        if (node_marker[i] > 1)
                            count++;
                } //while

                //ricalcolo la soluzione...
                Write(String.Format("ok!\r\nWrong Wrapper Solution was {0}", WrapperSolution));
                WrapperSolution = 0;
                for (uint i = 0; i < p; i++)
                    WrapperSolution += ComputeWrapperClusterCost(i);
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalElapsedTime()
        {
            return fase3.Elapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        public new void ShowElapsedTime() 
        {
            base.ShowElapsedTime();
            Write(String.Format("\r\nElapsed Time Wrapper    : {0}", fase3.Elapsed - fase2.Elapsed));
            Write(String.Format("\r\nElapsed Time Total      : {0}", fase3.Elapsed));
        }

        /// <summary>
        /// ritorna il costo del cluster della soluzione del wrapper
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint ComputeWrapperClusterCost(uint index)
        {
            if (!solutions_computed)
                return 0;
            
            //return ComputeClusterCost((uint)indexsolvar[index] / p, (uint)indexsolvar[index] % p);
            return ComputeClusterCost(WrapperSolutionClusters, index);
        }

        /// <summary>
        /// Per controllare se è una soluzione ammissibile
        /// </summary>
        /// <returns></returns>
        public bool CheckWrapperClustersSolution()
        {
            uint[] node;
            return CheckWrapperClustersSolution(out node);
        }

        #endregion
    }
}
