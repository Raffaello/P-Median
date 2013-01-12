//*********************************************************************
//****   P-Median DLL Library                                      ****
//****   Author : Raffaello Bertini (raffaellobertini@gmail.com)   ****
//****   File: PMed1.cs  (Class for Solve one Heuristic Solution   ****
//****                    Inerithed from Main)                     ****
//****-------------------------------------------------------------****
//****                    Version History:                         ****
//****   1.00 September 2011                                       ****
//*********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PMedLib
{
    /// <summary>
    /// 
    /// </summary>
    public class PMed1 : PMed
    {
        #region Private Variables

        /// <summary>
        /// default del MaxReSetSolution
        /// </summary>
        private const ushort _MaxReSetSolution = 1000;
        /// <summary>
        /// variabile che conta gli zeri consecutivi del random...
        /// </summary>
        private ushort dc; 
        /// <summary>
        /// se si dovesse bloccare il random... 
        /// </summary>
        private const ushort _MaxZeroRandomCount = 1000;
       
        /// <summary>
        /// memorizzo il valore della soluzione
        /// </summary>
        private uint _solution;

        #endregion

        #region Private Methods

        /// <summary>
        /// ridefinisce la soluzione del cluster ruotando il nodo p per verificare che possa essere migliorabile
        /// vedi il suo overload
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private uint ComputeBestClusterCost(uint index)
        {
            List<uint>[] c = Clusters;
            return ComputeBestClusterCost(ref c, index);
        }
        
        /// <summary>
        /// Shifta la soluzione del cluster e trova la migliore.
        /// </summary>
        /// <param name="Clusters"></param>
        /// <param name="index">indice cluster</param>
        /// <returns>ritorna il guadagno rispetto alla soluzione precedente</returns>
        private uint ComputeBestClusterCost(ref List<uint>[] Clusters, uint index)
        {
            uint best_sol = ComputeClusterCost(Clusters, index);
            uint sol = best_sol;
            uint current_sol = sol;
            uint first = Clusters[index].First();
            List<uint> cluster = Clusters[index];
            int p_i = 0;
            int n = Clusters[index].Count;

            for (int i = 1; i < n; i++)
            {
                Clusters[index] = new List<uint>(cluster);
                Clusters[index].RemoveAt(0);
                Clusters[index].Insert(i, first);
                sol = ComputeClusterCost(Clusters, index);
                //guardo se è ammissibile...
                uint cq = ComputeClusterQ(Clusters, index);

                if ((sol < best_sol) &&
                   (cq<=Q))
                {
                    best_sol = sol;
                    p_i = i;
                }

            }

            //setto il cluster corretto
            Clusters[index] = cluster;
            Clusters[index].RemoveAt(0);
            Clusters[index].Insert(p_i, first);
#if DEBUG
            sol = ComputeClusterCost(Clusters, index);
            if (sol != best_sol)
            {
                Write(String.Format("\r\nDEBUG BestClusteCost!!!"));
            }
#endif

            //if (Verbose)
            //    Write(String.Format("ComputedBestClusterCost {0,3}\r\n", index));

            return current_sol - best_sol;
        }

        /// <summary>
        /// Calcola la soluzione del problema, attraverso i cluster.
        /// </summary>
        /// <returns></returns>
        private uint ComputeSolutionCost(List<uint>[] Clusters)
        {
#if DEBUG
            Debug.Assert(Clusters != null);
#endif
            uint i, sumtot;
            for (i = 0, sumtot = 0; i < p; i++)
                sumtot += ComputeClusterCost(Clusters, i);

            return sumtot;
        }

        /// <summary>
        /// Funzione di controllo della soluzione generata
        /// Esiste solo se esiste #define DEBUG
        /// </summary>
        [Conditional("DEBUG")]
        private void CheckCluster(List<uint>[] Clusters)
        {
            int i, j;

            Write("*** CheckCluster Debug Function Started ***\r\n");
            for (i = 0; i < p; i++)
                if (Clusters[i] == null)
                    Write(String.Format("*** Clusters {0,3} non allocato! Error!!!  ***\r\n", i));

            uint[] count = new uint[nVerteces];
            for (i = 0; i < nVerteces; i++)
                count[i] = 0;

            for (j = 0; j < p; j++)
            {
                foreach (uint l in Clusters[j])
                    count[l]++;
            }

            for (i = 0; i < nVerteces; i++)
            {
                if (count[i] == 0)
                    Write(String.Format("*** Nodo {0,3} non usato!! ERROR!         ***\r\n", i));
                else if (count[i] > 1)
                    Write(String.Format("*** Nodo {0,3} usato {1,3} volte. ERROR!!!! ***\r\n", i, count[i]));
            }

            Write("*** CheckCluster Debug Function Ended.  ***\r\n");

        }

        #endregion

        #region Protected Variables
        /// <summary>
        /// per generare i nodo p casuali.
        /// </summary>
        protected Random r;
        #endregion

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Clusters"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected uint ComputeClusterQ(List<uint>[] Clusters, uint index)
        {
            Debug.Assert(Clusters[index] != null);

            uint sum = 0;
            uint first = Clusters[index].First();

            foreach (uint l in Clusters[index])
                sum += DemandQ[l];

            return sum;
        }


        /// <summary>
        /// Calcola il costo di un singolo cluster, ovvero di un singolo p nodo
        /// </summary>
        /// <param name="Clusters"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected uint ComputeClusterCost(List<uint>[] Clusters, uint index)
        {
            Debug.Assert(Clusters[index] != null);

            uint sum = 0;
            uint first = Clusters[index].First();

            foreach (uint l in Clusters[index])
                sum += c[first, l];

            return sum;
        }


        /// <summary>
        /// inizializza p nodi come i nodi p della soluzione
        /// </summary>
        /// <param name="Clusters"> utilizzato in maniera esplicita i clusters della soluzione</param>
        protected void SetUpP(ref List<uint>[] Clusters)
        {
            int i;
            uint index;
            bool[] bn = new bool[nVerteces];

            //preparo i p cluster della soluzione ovviamente vuoti
            // o con il nodo p come primo nella lista!
            Clusters = new List<uint>[p];
            for (i = 0; i < p; i++)
                Clusters[i] = new List<uint>();

            for (i = 0; i < nVerteces; i++)
                bn[i] = false;

            int n = (int)nVerteces;
            i = 0;
            dc = 0;
            do
            {
                do
                {
                    lock (this)
                    {
                        index = (uint)r.Next(n);
                    }
                    //qui sotto controlla per il random se si dovesse bloccare...
                    //scommentare se accade...

                    if (index == 0)
                        dc++;
                    else
                        dc = 0;
                    lock (this) //questo rallenta il parallelo indispensabile però per rigenerare il random. 
                    {
                        if (dc == MaxZeroRandomCount)
                        {
                            ReinitRandom();
                            dc = 0;
                        }
                    }

                    //**** fine snippets codice per il "check random" ... *****
                } while (bn[index]);
                bn[index] = true;
                Clusters[i++].Add(index);
            } while (i < p);
        }


        /// <summary>
        /// 
        /// </summary>
        protected void ReinitRandom()
        {
            if (Seed >= 0)
                r = new Random(Seed);
            else
                r = new Random();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected uint ComputeClusterCost(uint index)
        {
            return ComputeClusterCost(this.Clusters, index);
        }

        /// <summary>
        /// primo passo dell'algoritmo e calcolo soluzione euristica base
        /// </summary>
        protected bool FirstStep(ref List<uint>[] Clusters, ref uint solution)
        {
            //setto i punti p random
            //SetUpP(ref Clusters); //commentato per via della concorrenza

            //sistemo le cose...
            bool[,] isfree = new bool[p, nVerteces];
            uint i, j, k;

            for (i = 0; i < p; i++)
            {
                for (j = 0; j < nVerteces; j++)
                     isfree[i, j] = true;
            }

            //ora sistemo gli archi disponibili
            for (j = 0; j < p; j++)
            {
                for (i = 0; i < p; i++)
                    //isfree[j, p_index[i]] = false;
                    isfree[j, Clusters[i].First()] = false;
            }
#if DEBUG
            if (Verbose)
            {
                //visualizzo come sono i cluster ora.. (con solo il nodo p)
                for (i = 0; i < p; i++)
                {

                    Write(String.Format("Clusters[{0}] =", i));
                    foreach (uint l in Clusters[i])
                        Write(String.Format(" {0}\r\n", l));
                }
            }
#endif
            //variabili locali per risolvere il primo step.
            uint[][] index_c_sorted = new uint[p][];
            uint[][] c_clone = new uint[p][];

            for (j = 0; j < p; j++)
            {
                index_c_sorted[j] = new uint[nVerteces];
                c_clone[j] = new uint[nVerteces];
                //creo la matrice di indici per ordinare rispetto ai costi dai nodi p
                for (i = 0; i < nVerteces; i++)
                {
                    index_c_sorted[j][i] = i;
                    c_clone[j][i] = c[Clusters[j].First(), i];
                }
                //ordino rispetto a p i nodi che costano meno
                Array.Sort(c_clone[j], index_c_sorted[j]);
            }

            //ora costruiamo la soluzione... 
            // i cluster è meglio se sono un' array di liste di nodi (di uint)!!!!
            //perchè possono avere lunghezze diverse fra loro.
            uint n = nVerteces - p;
            k = 0;
            uint[] qxi = new uint[p]; //per memorizzare la capaità del cluster
            for (j = 0; j < p; j++)
                qxi[j] = DemandQ[Clusters[j].First()];
               
            while (k < n)
                {
                    bool foundQ = false;
                    j = 0;
                    for (j = 0; j < p; j++)
                    {
                        //si parte da 1 pechè il primo nodo e il nodo stesso sempre false
                        for (i = 1; i < nVerteces; i++)
                        {
                            uint q = qxi[j] + DemandQ[index_c_sorted[j][i]];
                            if ((isfree[j, index_c_sorted[j][i]]) &&
                                (q<=Q))
                            {
                                foundQ = true;
                                //ora controllo che non sia meglio in un altra posizione...
                                uint current = c_clone[j][i];
                                uint icurrent = j;
                                for (uint j2 = 0; j2 < p; j2++)
                                {
                                    uint q2 = qxi[j2] + DemandQ[index_c_sorted[j2][i]];
                                    if ((isfree[j2, index_c_sorted[j2][i]]) &&
                                       (current > c_clone[j2][i]) &&
                                        (q2<=Q))
                                    {
                                        //se da un'altra parte è meglio memorizzo attualmente
                                        current = c_clone[j2][i];
                                        icurrent = j2;
                                    }
                                }
                                //setto il meglio trovato.
#if DEBUG
                                Debug.Assert(isfree[icurrent, index_c_sorted[j][i]]);
#endif
                                j = icurrent;

                                //lo toglie dalla disponibilità per gli altri p.
                                for (int i2 = 0; i2 < p; i2++)
                                    isfree[i2, index_c_sorted[j][i]] = false;

                                Clusters[j].Add(index_c_sorted[j][i]);
                                qxi[j] += DemandQ[index_c_sorted[j][i]];
                                k++;
                                break;
                            }
                        }
                    }

                    //se non ha trovato un Q entra in loop...
                    //quindi bisogna fare qualcosa....
                    if (foundQ == false)
                    {
                        if(Verbose)
                            Write("\r\nCan\'t find an appropriated node. Re-init Solution.");
                        return false;

                    }
                }

#if DEBUG
            if (Verbose)
            {
                uint sum, sumtot;
                for (i = 0, sumtot = 0; i < p; i++)
                {
                    Write(String.Format("Clusters[{0}] =", i));
                    sum = 0;
                    foreach (uint l in Clusters[i])
                    {
                        Write(String.Format(" {0}", l));
                        sum += c[Clusters[i].First(), l];
                    }
                    Write(String.Format("\r\n Length = {0} --- Sum = {1}\r\n", Clusters[i].Count, sum));
                    sumtot += sum;
                }
                Write(String.Format("SumTot = {0}\r\n", sumtot));
            }

            //controllo che sia rispettato il vincolo di capacita...DEBUG
            for (i = 0; i < p; i++)
            {
                if (qxi[i] > Q)
                {
                    Write(String.Format("\r\nDEBUG: vincolo di capacità del cluster {2} non rispettato q={0} <= Q={1}", qxi[i], Q, i));
                }
                if(qxi[i]!= ComputeClusterQ(Clusters,i))
                {
                    Write(String.Format("\r\nDEBUG: ComputeClusterQ = {0} --- qxi = {1}",ComputeClusterQ(Clusters,i),qxi[i]));
                }
            }
#endif
                CheckCluster(Clusters);

            solution = ComputeSolutionCost(Clusters);
            return true;
        }

        /// <summary>
        /// Affina la soluzione del cluster trovata utilizzando ComputeBestClusterCost.
        /// </summary>
        /// <param name="AsParallel"></param>
        /// <param name="solution"></param>
        /// <returns></returns>
        protected uint[] RefineClusterSolution(bool AsParallel, ref uint solution)
        {
            List<uint>[] c = Clusters;
            return RefineClusterSolution(AsParallel, ref c, ref solution);

        }

        /// <summary>
        /// Affina la soluzione del cluster trovata utilizzando ComputeBestClusterCost.
        /// </summary>
        /// <param name="AsParallel"></param>
        /// <param name="Clusters"></param>
        /// <param name="solution"></param>
        /// <returns>ritorna il guadagno</returns>
        protected uint[] RefineClusterSolution(bool AsParallel, ref List<uint>[] Clusters, ref uint solution)
        {
            uint[] gap = new uint[p];

            if (AsParallel)
            {
                List<uint>[] c = Clusters;
                Parallel.For(0, p, i => gap[i] = ComputeBestClusterCost(ref c, (uint)i));
                Clusters = c;
            }
            else
                for (uint i = 0; i < p; i++)
                    gap[i] = ComputeBestClusterCost(ref Clusters, i);


            CheckCluster(Clusters);
            solution = ComputeSolutionCost(Clusters);
            return gap;
        }

        #endregion
        
        #region Public Variables

        /// <summary>
        /// quanti random consecutivi uguali a zero prima di reiterare il processo.
        /// default = _MaxZeroRandomCount
        /// (sola lettura)
        /// </summary>
        public ushort MaxZeroRandomCount { get; private set; }
       
        /// <summary>
        /// quante volte riprova a ricalcolare una soluzione se non c'è riuscito 
        /// (default=_MaxReSetSolution) 
        /// Utilizzato in pratica solo nei capacitated proprio per il controllo sulle capacità
        /// </summary>
        public ushort MaxReSetSolution { get; set; }

        /// <summary>
        /// Se negativo usa Time come seed altrimenti il seed corrispondete
        /// </summary>
        public int Seed { get; set; }

        /// <summary>
        /// cluster della soluzione
        /// </summary>
        public List<uint>[] Clusters { get; protected set; }

        /// <summary>
        /// valore della soluzione. come proprietà
        /// </summary>
        public uint solution { get { return _solution; } protected set { _solution = value; } } 

       #endregion

        #region Public Methods

        /// <summary>
        /// costruttore
        /// </summary>
        /// <param name="_w"></param>
        /// <param name="verbose"></param>
        public PMed1(WriteDelegate _w, bool verbose = true)
            : base(_w, verbose)
        {
            MaxReSetSolution = _MaxReSetSolution;
            MaxZeroRandomCount = _MaxZeroRandomCount;
            Seed = -1;
            solution = uint.MaxValue;
            ReinitRandom();
        }

//#if DEBUG
        /// <summary>
        /// per utilizzare nei test ad hoc o testare la libreria
        /// </summary>
        [Conditional("DEBUG")]
        public void FirstStep()
        {
            List<uint>[] c = Clusters;
            uint s=0;
            SetUpP(ref c);
            FirstStep(ref c, ref s);
            solution = s;
            Clusters = c;
        }
//#endif

        #endregion
    }
}