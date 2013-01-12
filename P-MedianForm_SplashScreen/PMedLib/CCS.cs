//*********************************************************************
//****   CCS Utility Class for c# Wrapper & P-Median problem       ****
//****   Author : Raffaello Bertini (raffaellobertini@gmail.com)   ****
//****   File: CCS.cs                                              ****
//****                                                             ****
//****-------------------------------------------------------------****
//****                    Version History:                         ****
//****   1.00 July 2011                                            ****
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMedLib
{
    internal class CCS
    {
        #region Private Methods

        /// <summary>
        /// Converte la matrice dei costi nel formato CCS
        /// </summary>
        /// <param name="Sol"></param>
        /// <param name="NSol"></param>
        /// <param name="p"></param>
        /// <param name="pt"></param>
        private void ConvertToCCS(List<uint>[][] Sol, uint NSol, uint p, eProblemType pt)
        {
            //int count = 0;
            matrixCount = null;
            matrixBegin = matrixIndex = null;
            char _rowtype_char;
            //matrix count 
            //la matrice è simmetrica quindi la sua trasposta è uguale
            //significa che se conto per righe o per colonne è la stessa cosa!
            matrixCount = new int[colCount];
            matrixBegin = new int[colCount + 1];

            //checckare se va bene nel sistema!!!!!! OK
            rhsValues = new double[rowCount];
            switch (pt)
            {
                case eProblemType.SetCovering :
                    _rowtype_char = 'G';
                    break;
                case eProblemType.SetPartitioning :
                    _rowtype_char = 'E';
                    break;
                default:
                    throw new Exception("CCS.ConvertToCCS : Unknown problem Type.");
            }

            for (int i = 0; i < rowCount; i++)
            {
                rhsValues[i] = 1;
                //rowType += 'G';
                rowType += _rowtype_char;
            }

            //per matrixcount è = alla lunghezza del cluster..
            //per il matrixIndex basta ordinare la soluzione del cluster crescente...
            matrixIndex = new int[nonZeroCount];

            uint[] k;
            int _i = 0;
            int mc = 0;
            for (int i = 0; i < NSol; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    matrixCount[mc++] = Sol[i][j].Count;

                    k = Sol[i][j].ToArray();
                    //ordinare k .. un radix sort sarebbe perfetto. ma quicksort va bene lo stesso...
                    Array.Sort(k);

                    for (int k2 = 0; k2 < k.Length; k2++)
                    {
                        matrixIndex[_i++] = (int)k[k2];
                    }

                }
            }

            //matrix values sono gli uni nella matrice
            matrixValues = new double[nonZeroCount];
            for (int i = 0; i < nonZeroCount; i++)
                matrixValues[i] = 1.0;
            //check mindex[0]

            matrixBegin[0] = 0;
            for (int i = 0; i < matrixCount.Length; i++)
            {
                matrixBegin[i + 1] = matrixBegin[i] + matrixCount[i];
            }



#if DEBUG
            //  if (nonZeroCount != count)
            //    throw new Exception("nonZeroCount != count (fine buildCCS)");
#endif
            //nonZeroCount=count;
            //return nonzero;
        }

        /// <summary>
        /// converte in CCS ed in più aggiunge già il vincolo (xij=p)
        /// </summary>
        /// <param name="Sol"></param>
        /// <param name="NSol"></param>
        /// <param name="p"></param>
        /// <param name="pt"></param>
        private void ConvertToCCS_AddRow(List<uint>[][] Sol, uint NSol, uint p, eProblemType pt)
        {
            rowCount++; //perchè c'è il vincolo di AddRows qui
            nonZeroCount += colCount;

            matrixCount = null;
            matrixBegin = matrixIndex = null;
            char _rowtype_char;

            //la matrice è simmetrica quindi la sua trasposta è uguale
            //significa che se conto per righe o per colonne è la stessa cosa!
            matrixCount = new int[colCount];
            matrixBegin = new int[colCount + 1];

            //checckare se va bene nel sistema!!!!!! OK
            rhsValues = new double[rowCount];
            switch (pt)
            {
                case eProblemType.SetCovering:
                    _rowtype_char = 'G';
                    break;
                case eProblemType.SetPartitioning:
                    _rowtype_char = 'E';
                    break;
                default:
                    throw new Exception("CCS.ConvertToCCS : Unknown problem Type.");
            }

            for (int i = 0; i < rowCount-1; i++)
            {
                rhsValues[i] = 1;
                //rowType += 'G';
                rowType += _rowtype_char;
            }
            
            rhsValues[rowCount-1] = p;
            rowType += 'E';

            //per matrixcount è = alla lunghezza del cluster..
            //per il matrixIndex basta ordinare la soluzione del cluster crescente...
            
            matrixIndex = new int[nonZeroCount];

            uint[] k;
            int _i = 0;
            int mc = 0;
            for (int i = 0; i < NSol; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    int count = Sol[i][j].Count+1;
                    matrixCount[mc++] = count;

                    k = new uint[count];
                    Sol[i][j].CopyTo(k);

                    k[--count] = rowCount - 1;
                    
                    //ordinare k .. un radix sort sarebbe perfetto. ma quicksort va bene lo stesso...
                    Array.Sort(k,0,count);

                    for (int k2 = 0; k2 < k.Length; k2++)
                    {
                        matrixIndex[_i++] = (int)k[k2];
                    }

                }
            }
            

            //matrix values sono gli uni nella matrice
           
            matrixValues = new double[nonZeroCount];
            //for (int i = 0; i < nonZeroCount; i++)
            for (int i = 0; i < nonZeroCount; i++)
                matrixValues[i] = 1.0;
            //check mindex[0]

            matrixBegin[0] = 0;
            for (int i = 0; i < matrixCount.Length; i++)
            {
                matrixBegin[i + 1] = matrixBegin[i] + matrixCount[i];
            }
        }
        #endregion

        #region Public Variables

        /// <summary>
        ///  n° colonne
        /// </summary>
        public uint colCount { get; private set; }

        /// <summary>
        ///  n° righe
        /// </summary>
        public uint rowCount { get; private set; }

        /// <summary>
        ///  elementi != 0
        /// </summary>
        public uint nonZeroCount { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int[] matrixBegin { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int[] matrixCount { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public double[] matrixValues { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int[] matrixIndex { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public double[] rhsValues { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public String rowType { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Costruttore
        /// </summary>
        /// <param name="Sol"></param>
        /// <param name="NSol"></param>
        /// <param name="p"></param>
        /// <param name="n"></param>
        /// <param name="pt"></param>
        /// <param name="AddRow"></param>
        public CCS(List<uint>[][] Sol, uint NSol, uint p, uint n, eProblemType pt, bool AddRow=true)
        {
          
            rowCount = n;
            colCount = NSol * p;
            nonZeroCount = NSol * n;
            if(AddRow)
                ConvertToCCS_AddRow(Sol, NSol, p, pt);
            else
                ConvertToCCS(Sol, NSol ,p, pt);
        }

     /*   /// <summary>
        /// conta gli elementi diversi da 0
        /// </summary>
        /// <param name="nvertex"></param>
        /// <param name="p"></param>
        /// <param name="sol"></param>
        /// <param name="nonzerocount"></param>
        /// <param name="mcount"></param>
        /// <param name="mbegin"></param>
        /// <param name="mindex"></param>
        /// <returns></returns>*/
        //public double[] CountNonZeroList(uint nvertex, uint p, List<uint>[][] sol, out uint nonzerocount, out int[] mcount, out int[] mbegin, out int[] mindex)
        //{
        //    uint count = nonzerocount = 0;
        //    double[] nonzero = null;
        //    mcount = null;
        //    mbegin = mindex = null;
        //    if ((sol == null) || (sol[0] == null))
        //        return null;

        //    // questa funzione è Builtableau di Pmed3 quindi non continuare oltre!!!!!!

        //    //....
        //    //le cose qui si complicano la matrice è lunga n 
        //    // e larga quanto le soluzioni
        //    //....         
        //    //quindi nvertex è numero di righe
        //    // e cluster per numero soluzioni sono le colonne (si ricava da sol[][])

        //    //nonzerocount = nvertex * nsoluzioni;
        //    return nonzero;
        //}

        #endregion
     }
}
