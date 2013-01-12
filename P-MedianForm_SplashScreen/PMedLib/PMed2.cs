//*********************************************************************
//****   P-Median DLL Library                                      ****
//****   Author : Raffaello Bertini (raffaellobertini@gmail.com)   ****
//****   File: PMed2.cs  (Class for Solve N Heuristic Solutions    ****
//****                    Inerithed from PMed1)                    ****
//****-------------------------------------------------------------****
//****                    Version History:                         ****
//****   1.00 September 2011                                       ****
//*********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;


namespace PMedLib
{
    /// <summary>
    /// 
    /// </summary>
    public class PMed2 : PMed1
    {
    
        #region Private Methods

        /// <summary>
        /// usato internamente nel ciclo for,  per calcolare ogni singola soluzione e memorizzarne i risultati ottenuti.
        /// </summary>
        /// <param name="i">indice della soluzione</param>
        /// <param name="AsParClu">in parallelo?</param>
        private void inComputeSolutions(long i, bool AsParClu)
        {
            if (Verbose)
                Write((String.Format("Computing solution : {0,3}\r\n", i)));
            
            ushort count = 0;
            bool first;
            do
            {
                first = FirstStep(ref SolutionsClusters[i], ref Solutions[i]);
                count++;
                if (!first)
                {
                    if (count == MaxReSetSolution)
                    {
                        Write(String.Format("\r\nCannot Compute Solution {0} due to Q variable.", i));
                        break;
                    }
                    SolutionsClusters[i] = null;
                    SetUpP(ref SolutionsClusters[i]);
                }
            } while (!first);
            
            
            GapClusters[i] = RefineClusterSolution(AsParClu, ref SolutionsClusters[i], ref Solutions[i]);
            //SolutionsClusters[i] = Clusters;
            //Solutions[i] = solution;
            
            lock(this)
            {
                if (bestsol > Solutions[i])
                {
                    bestsol = Solutions[i];
                    BestSolutionIndex = (uint)i;
                }
            }
            //lancio l'evento di aggiornamento progressi. Se non esiste è di default settato OnProgressIdle, così da non crashare        
            Progress((uint)i);
        }

        /// <summary>
        /// così non ho bisogno di controllare se è stato associato un evento.
        /// di default a questa funzione vuota che è associata all'evento.
        /// </summary>
        /// <param name="index"></param>
        private void OnProgressIdle(uint index)
        {

        }

        #endregion

        #region Protected Variables
        /// <summary>
        /// 
        /// </summary>
        protected Stopwatch fase2;
        /// <summary>
        /// valore della soluzione migliore trovata, come valore di riferimento durante la computazione.
        /// (si ereda solution da PMed1 per la soluzione migliore trovata!!!)
        /// </summary>
        protected uint bestsol;
        #endregion

        #region Public Variables

        /// <summary>
        /// Numero di soluzioni da generare
        /// </summary>
        public uint Nsolution { get; private set; }

        /// <summary>
        /// memorizzo i vari cluster e le soluzioni.
        /// </summary>
        public List<uint>[][] SolutionsClusters { get; private set; }

        /// <summary>
        /// memorizzo il gap per ogni soluzioni di ogni cluster dopo il refine.
        /// </summary>
        public uint[][] GapClusters { get; private set; }

        /// <summary>
        /// Le N soluzioni
        /// </summary>
        public uint[] Solutions { get; private set; }

        /// <summary>
        /// indice della migliore soluzione
        /// </summary>
        public uint BestSolutionIndex { get; private set; }

        /// <summary>
        ///  l'evento è creato ogni singola soluzione computata
        /// </summary>
        public event ProgressReporter Progress;
        
        #endregion

        #region Public Methods 
        
        /// <summary>
        /// Controlla i p delle soluzioni. Utile quando si vuol verificare la bontà della randomness nella scelta dei p nodi.
        /// </summary>
        //[Conditional("DEBUG")]
        public uint CheckPofSolutions(bool verbose=true)
        {
            List<uint>[] p_tmp, p2_tmp;
            uint pcount;
            uint ret = 0;
            //List<uint>[] tmp = new List<uint>[2];
            if(verbose)
                Write(String.Format("\r\n*** Check P Sets...***"));
            for (uint i = 0; i < this.Nsolution; i++)
            {
                p_tmp = this.SolutionsClusters[i];
                if (p_tmp == null)
                    break;
                for (uint j = i + 1; j < this.Nsolution; j++)
                {
                    p2_tmp = this.SolutionsClusters[j];
                    if (p2_tmp == null)
                        break;
                    pcount = 0;
                    foreach (List<uint> p in p_tmp)
                    {
                        foreach (List<uint> p2 in p2_tmp)
                        {
                            if (p[0] == p2[0])
                            {
                                pcount++;
                                break;
                            }
                        }
                    }
                    if (pcount == this.p)
                    {
                        //TROVATO!!! il set dei p è il medesimo.
                        if(verbose)
                            Write(String.Format("\r\nSolution {0} has the same Set P of solution {1}", i, j));
                        ret++;
                        break;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// delegate per notificare l'avanzamento della computazione.
        /// </summary>
        /// <param name="index"></param>
        public delegate void ProgressReporter(uint index);
   
        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="_w">Funzione che scrive su qualche oggetto, che sia console o textbox (ex:Console.Write)</param>
        /// <param name="N">quante soluzioni generare.</param>
        /// <param name="verbose"></param>
        public PMed2(WriteDelegate _w, uint N, bool verbose = true)
            : base(_w, verbose)
        {
            fase2 = new Stopwatch();
            SetSolutions(N);
        }

        /// <summary>
        /// Setta il numero di soluzioni da calcolare
        /// </summary>
        /// <param name="N">maggiore uguale a 1</param>
        public void SetSolutions(uint N)
        {
            Nsolution = N;
            SolutionsClusters = new List<uint>[N][];
            GapClusters = new uint[N][];
            Solutions = new uint[N];
            GC.Collect();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowElapsedTime()
        {
            Write(String.Format("\r\nElapsed Time step 1 & 2 : {0}",fase2.Elapsed));
        }

        /// <summary>
        /// Calcola le N soluzioni.
        /// </summary>
        /// <param name="AsParallel">in parallelo le n soluzioni</param>
        /// <param name="internalParallel">in parallelo il miglioramento soluzione trovata sui Clusters</param>
        public bool ComputeSolutions(bool AsParallel, bool internalParallel)
        {
            try
            {
                fase2.Restart();
                
                r = null;
                GC.Collect();

                ReinitRandom();

                bestsol = uint.MaxValue;
                if (Progress == null)
                    Progress += OnProgressIdle;
                Write("\r\nComputing solutions (Seed = ");
                Write(String.Format("{0})...",(Seed<0)?"Time": Seed.ToString()));
                
                //genero prima tutto il set di nodi per forza in seriale...
                for (uint i = 0; i < Nsolution; i++)
                    SetUpP(ref SolutionsClusters[i]);

                try
                {
                    if (AsParallel)
                    {
                        Parallel.For(0, Nsolution, i =>
                        {
                            inComputeSolutions(i, internalParallel);
                        });
                    }
                    else
                    {
                        for (long i = 0; i < Nsolution; i++)
                            inComputeSolutions(i, internalParallel);
                    }
                }
                catch (OutOfMemoryException ome)
                {
                    Write(String.Format("\r\n {0}", ome.Message));
                    return false;
                }
                catch (Exception e)
                {
                    Write(String.Format("\r\n {0}", e.Message));
                    return false;
                }
                   
#if DEBUG
            for (uint i = 0; i < Nsolution; i++)
            {
                if (Solutions[BestSolutionIndex] > Solutions[i])
                    Write(string.Format("Solution {0} = {1} --- Best Solution = {2}\r\n", i, Solutions[i], Solutions[BestSolutionIndex]));
            }
#endif
                solution = bestsol;
                fase2.Stop();
            }
            catch (Exception e)
            {
                Write(String.Format("\r\n {0}", e.Message));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calcola il valore del cluster specificato della soluzione specificata
        /// </summary>
        /// <param name="Solution_index"></param>
        /// <param name="Cluster_index"></param>
        /// <returns>il costo</returns>
        public uint ComputeClusterCost(uint Solution_index, uint Cluster_index)
        {
            this.Clusters = SolutionsClusters[Solution_index];
            return ComputeClusterCost(Cluster_index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Solution_index"></param>
        /// <param name="Cluster_index"></param>
        /// <returns></returns>
        public uint ComputeClusterQ(uint Solution_index, uint Cluster_index)
        {
            this.Clusters = SolutionsClusters[Solution_index];
            return ComputeClusterQ(Clusters, Cluster_index);
        }

        /// <summary>
        /// Stampa a video i nodi p dei vari cluster delle varie soluzioni
        /// </summary>
        public void ShowPofSolutions()
        {
            if (this.Solutions == null)
                return;
            Write("\r\nP-Set :");
            for (uint i = 0; i < this.Nsolution; i++)
            {
                Write(string.Format("\r\nS{0} = ", i));
                foreach (List<uint> j in this.SolutionsClusters[i])
                {
                    Write(String.Format("{0}, ", j[0]));
                }
            }

            CheckPofSolutions();
        }

        #endregion

    }
}