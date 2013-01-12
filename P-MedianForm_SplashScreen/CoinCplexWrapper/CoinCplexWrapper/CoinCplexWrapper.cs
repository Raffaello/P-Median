/*********************************************************************************************/
/*                                    Coin&CplexWrapper                                      */
/*                                                                                           */
/*                                   by Andrea Pierantoni                                    */
/*                                                                                           */
/*-------------------------------------------------------------------------------------------*
 * Added CoinOR Callback Function for Messagge Passing                                       *
 * By Raffaello Bertini on August 2011                                                       *
/*********************************************************************************************///------------------------------ Update By Raffaello Bertini ----------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;



namespace CoinCplexWrapper
{
    /*************************************    Strutture del Wrapper   ******************************************/
    public struct WrapProblem
    {
        private IntPtr problem_p;
        private bool solver;
        public WrapProblem(IntPtr problem, bool solv)
        {
            problem_p = problem;
            solver = solv;
        }
        public IntPtr getProblem()
        {
            return problem_p;
        }
        public bool getSolver()
        {
            return solver;
        }
    };


    /******************************************    Classe Wrapper   ******************************************/
    public class Wrapper
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string filename);


        /*********************************************************************************
         * CoinOr CallBack Function                                                      *
        
         * Sono implementate solo per CoinOR, per CPLEX non fanno nulla.
         * Eventualmente se necessario importare le corrispettive API e inserire
         * le corrispettive chiamate per CPLEX dove opportuno
         * ********************************************************************************/
        #region CoinMP CallBack Function Definitions
        
        public delegate void _MsgLogCallback(String str);
        _MsgLogCallback W_MsgLogCallback;  //la memorizzo per poterla imporre fixed!
        [DllImport("CoinMP.dll")]
        static extern int CoinSetMsgLogCallback(IntPtr hProb, _MsgLogCallback MsgLogCallback);
        public int SetMsgLogCallBack(WrapProblem problem, _MsgLogCallback MsgLogCallBack)
        {
            int status = -1;
            IntPtr prob = new IntPtr();
            
            prob = problem.getProblem();
            bool CPLEX = problem.getSolver();
            if (CPLEX)
            {
                //To do .....
            }
            else
            {
                //setto x CoinOr
                W_MsgLogCallback = MsgLogCallBack;
                status = CoinSetMsgLogCallback(prob, W_MsgLogCallback);
            }

            return status;
        }

        public delegate int _IterCallback(int IterCount, double ObjectValue, int IsFeasible, double InfeasValue);
        [DllImport("CoinMP.dll")]
        static extern int CoinSetIterCallback(IntPtr hProb, _IterCallback IterCallback);
        public int SetIterCallBack(WrapProblem problem, _IterCallback IterCallBack)
        {
            int status = 0;
            IntPtr prob = new IntPtr();
            bool CPLEX = problem.getSolver();
            prob = problem.getProblem();

            if (CPLEX)
            {
                //to do....
            }
            else
            {
                status = CoinSetIterCallback(prob, IterCallBack);
            }

            return status;
        }

        public delegate int _MipNodeCallback(int IterCount, int NodeCount, double BestBound, double BestInteger, int IsMipImproved);
        [DllImport("CoinMP.dll")]
        static extern int CoinSetMipNodeCallback(IntPtr hProb, _MipNodeCallback MipNodeCallback);
        public int SetMipNodeCallBack(WrapProblem problem, _MipNodeCallback MipNodeCallBack)
        {
            int status = 0;
            IntPtr prob = new IntPtr();
            bool CPLEX = problem.getSolver();
            prob = problem.getProblem();

            if (CPLEX)
            {
                //TODO
            }
            else
            {
                status = CoinSetMipNodeCallback(prob, MipNodeCallBack);
            }

            return status;
        }

        #endregion
        /********************************* END Callback Function **************************/

        /************************************ Enum per le opzioni di CoinOr ********************/
        public enum CoinOptions
        {
            COIN_INT_SOLVEMETHOD        = 1,
            COIN_INT_PRESOLVETYPE       = 2,
            COIN_INT_SCALING            = 3,
            COIN_INT_PERTURBATION       = 4,
            COIN_INT_PRIMALPIVOTALG     = 5,
            COIN_INT_DUALPIVOTALG       = 6,
            COIN_INT_LOGLEVEL           = 7,
            COIN_INT_MAXITER            = 8,
            COIN_INT_CRASHIND           = 9,
            COIN_INT_CRASHPIVOT         = 10,
            COIN_REAL_CRASHGAP          = 11,
            COIN_REAL_PRIMALOBJLIM      = 12,
            COIN_REAL_DUALOBJLIM        = 13,
            COIN_REAL_PRIMALOBJTOL      = 14,
            COIN_REAL_DUALOBJTOL        = 15,
            COIN_REAL_MAXSECONDS        = 16,

            COIN_INT_MIPMAXNODES        = 17,
            COIN_INT_MIPMAXSOL          = 18,      
            COIN_REAL_MIPMAXSEC         = 19,  
   
            COIN_INT_MIPFATHOMDISC      = 20,
            COIN_INT_MIPHOTSTART        = 21,
            COIN_INT_MIPMINIMUMDROP     = 22,
            COIN_INT_MIPMAXCUTPASS      = 23,
            COIN_INT_MIPMAXPASSROOT     = 24 ,
            COIN_INT_MIPSTRONGBRANCH,
            COIN_INT_MIPSCANGLOBCUTS,

            COIN_REAL_MIPINTTOL         = 30,
            COIN_REAL_MIPINFWEIGHT      = 31,
            COIN_REAL_MIPCUTOFF         = 32,
            COIN_REAL_MIPABSGAP         = 33,
            COIN_REAL_MIPFRACGAP        = 34,

            COIN_INT_MIPCUT_PROBING     = 110,
            COIN_INT_MIPPROBE_FREQ      = 111,
            COIN_INT_MIPPROBE_MODE      = 112,
            COIN_INT_MIPPROBE_USEOBJ    = 113,
            COIN_INT_MIPPROBE_MAXPASS   = 114,   
            COIN_INT_MIPPROBE_MAXPROBE  = 115,
            COIN_INT_MIPPROBE_MAXLOOK   = 116, 
            COIN_INT_MIPPROBE_ROWCUTS   = 117,

            COIN_INT_MIPCUT_GOMORY      = 120,    
            COIN_INT_MIPGOMORY_FREQ     = 121,     
            COIN_INT_MIPGOMORY_LIMIT    = 122,    
            COIN_REAL_MIPGOMORY_AWAY    = 123,     

            COIN_INT_MIPCUT_KNAPSACK    = 130,      
            COIN_INT_MIPKNAPSACK_FREQ   = 131,    
            COIN_INT_MIPKNAPSACK_MAXIN  = 132,   

            COIN_INT_MIPCUT_ODDHOLE             = 140,        
            COIN_INT_MIPODDHOLE_FREQ            = 141,        
            COIN_REAL_MIPODDHOLE_MINVIOL        = 142,   
            COIN_REAL_MIPODDHOLE_MINVIOLPER     = 143,  
            COIN_INT_MIPODDHOLE_MAXENTRIES      = 144,  

            COIN_INT_MIPCUT_CLIQUE              = 150,          
            COIN_INT_MIPCLIQUE_FREQ             = 151,         
            COIN_INT_MIPCLIQUE_PACKING          = 152,       
            COIN_INT_MIPCLIQUE_STAR             = 153,          
            COIN_INT_MIPCLIQUE_STARMETHOD       = 154,    
            COIN_INT_MIPCLIQUE_STARMAXLEN       = 155,  
            COIN_INT_MIPCLIQUE_STARREPORT       = 156,   
            COIN_INT_MIPCLIQUE_ROW              = 157,        
            COIN_INT_MIPCLIQUE_ROWMAXLEN        = 158,   
            COIN_INT_MIPCLIQUE_ROWREPORT        = 159,   
            COIN_REAL_MIPCLIQUE_MINVIOL         = 160,    

            COIN_INT_MIPCUT_LIFTPROJECT         = 170,    
            COIN_INT_MIPLIFTPRO_FREQ            = 171,     
            COIN_INT_MIPLIFTPRO_BETAONE         = 172,   

            COIN_INT_MIPCUT_SIMPROUND           = 180,      
            COIN_INT_MIPSIMPROUND_FREQ          = 181,    

            COIN_INT_MIPUSECBCMAIN              = 200,       
        }

        public enum CoinLogLevel
        {
            None                        = 0,
            JustFinal                   = 1,
            JustFactorizations          = 2,
            JustFactorizationsAndMore   = 3,
            Verbose                     = 4
        }
        /*********************************** END Enum per Coin or ******************************/



        /******************************************   CoinMP 1.4   *******************************************/
        /**************************************  Modified With AddRows   *************************************/
        #region CoinMP with AddRows Added [DllImport]
        [DllImport("CoinMP.dll")]
        static extern int CoinInitSolver(IntPtr LicenseString);
        [DllImport("CoinMP.dll")]
        static extern int CoinFreeSolver();
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetSolverName();
        [DllImport("CoinMP.dll")]
        static extern int CoinGetSolverNameBuf(IntPtr SolverName, int buflen);
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetVersionStr();
        [DllImport("CoinMP.dll")]
        static extern int CoinGetVersionStrBuf(IntPtr VersionStr, int buflen);
        [DllImport("CoinMP.dll")]
        static extern double CoinGetVersion();
        [DllImport("CoinMP.dll")]
        static extern int CoinGetFeatures();
        [DllImport("CoinMP.dll")]
        static extern int CoinGetMethods();
        [DllImport("CoinMP.dll")]
        static extern double CoinGetInfinity();
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinCreateProblem(IntPtr ProblemName);
        [DllImport("CoinMP.dll", EntryPoint = "CoinLoadProblem", CharSet = CharSet.Ansi)]
        static extern int CoinLoadProblem2(IntPtr hProb, int ColCount, int RowCount, int NZCount,
                                           int RangeCount, int ObjectSense, double ObjectConst, double[] ObjectCoeffs,
                                           double[] LowerBounds, double[] UpperBounds, string RowType, double[] RHSValues,
                                           double[] RangeValues, int[] MatrixBegin, int[] MatrixCount, int[] MatrixIndex,
                                           double[] MatrixValues, IntPtr[] ColNames, IntPtr[] RowNames, string objName);
        [DllImport("CoinMP.dll", CharSet = CharSet.Ansi)]
        static extern int CoinAddRows(IntPtr hProb, int RowCount, int[] RowBegin, int[] ColIndex, double[] RowValues,
                                      double[] RHSValues, double[] RangeValues, string RowType);
        [DllImport("CoinMP.dll")]
        static extern int CoinLoadProblem(IntPtr hProb, int ColCount, int RowCount, int NZCount,
                                           int RangeCount, int ObjectSense, double ObjectConst, IntPtr ObjectCoeffs,
                                           IntPtr LowerBounds, IntPtr UpperBounds, IntPtr RowType, IntPtr RHSValues,
                                           IntPtr RangeValues, IntPtr MatrixBegin, IntPtr MatrixCount, IntPtr MatrixIndex,
                                           IntPtr MatrixValues, IntPtr ColNames, IntPtr RowNames, IntPtr objName);
        [DllImport("CoinMP.dll")]
        static extern int CoinLoadInitValues(IntPtr hProb, IntPtr InitValues);
        [DllImport("CoinMP.dll")]
        static extern int CoinLoadInteger(IntPtr hProb, IntPtr ColumnType);
        [DllImport("CoinMP.dll")]
        static extern int CoinLoadPriority(IntPtr hProb, int PriorCount, IntPtr PriorIndex, IntPtr PriorValues, IntPtr BranchDir);
        [DllImport("CoinMP.dll")]
        static extern int CoinLoadSemiCont(IntPtr hProb, int SemiCount, IntPtr SemiIndex);
        [DllImport("CoinMP.dll")]
        static extern int CoinLoadSos(IntPtr hProb, int SosCount, int SosNZCount, IntPtr SosType, IntPtr SosPrior, IntPtr SosBegin,
                                      IntPtr SosIndex, IntPtr SosRef);
        [DllImport("CoinMP.dll")]
        static extern int CoinLoadQuadratic(IntPtr hProb, IntPtr QuadBegin, IntPtr QuadCount, IntPtr QuadIndex, IntPtr QuadValues);
        [DllImport("CoinMP.dll")]
        static extern int CoinLoadNonLinear(IntPtr hProb, int NlpTreeCount, int NlpLineCount, IntPtr NlpBegin, IntPtr NlpOper,
                                            IntPtr NlpArg1, IntPtr NlpArg2, IntPtr NlpIndex1, IntPtr NlpIndex2, IntPtr NlpValue1,
                                            IntPtr NlpValue2);
        [DllImport("CoinMP.dll")]
        static extern int CoinUnloadProblem(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinCheckProblem(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinSetLoadNamesType(IntPtr hProb, int LoadNamesType);
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetProblemName(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetProblemNameBuf(IntPtr hProb, IntPtr ProblemName, int buflen);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetColCount(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetRowCount(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetColName(IntPtr hProb, int col);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetColNameBuf(IntPtr hProb, int col, IntPtr ColName, int buflen);
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetRowName(IntPtr hProb, int row);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetRowNameBuf(IntPtr hProb, int row, IntPtr RowName, int buflen);
        [DllImport("CoinMP.dll")]
        static extern int CoinOptimizeProblem(IntPtr hProb, int Method);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetSolutionStatus(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetSolutionText(IntPtr hProb, int SolutionStatus);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetSolutionTextBuf(IntPtr hProb, int SolutionStatus, IntPtr SolutionText, int buflen);
        [DllImport("CoinMP.dll")]
        static extern double CoinGetObjectValue(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern double CoinGetMipBestBound(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetIterCount(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetMipNodeCount(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetSolutionValues(IntPtr hProb, IntPtr Activity, IntPtr ReducedCost, IntPtr SlackValues, IntPtr ShadowPrice);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetSolutionRanges(IntPtr hProb, IntPtr ObjLoRange, IntPtr ObjUpRange, IntPtr RhsLoRange, IntPtr RhsUpRange);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetSolutionBasis(IntPtr hProb, IntPtr ColStatus, IntPtr RowStatus);
        [DllImport("CoinMP.dll")]
        static extern int CoinReadFile(IntPtr hProb, int FileType, IntPtr ReadFilename);
        [DllImport("CoinMP.dll")]
        static extern int CoinWriteFile(IntPtr hProb, int FileType, IntPtr WriteFilename);
        [DllImport("CoinMP.dll")]
        static extern int CoinOpenLogFile(IntPtr hProb, IntPtr logFilename);
        [DllImport("CoinMP.dll")]
        static extern int CoinCloseLogFile(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetOptionCount(IntPtr hProb);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetOptionInfo(IntPtr hProb, int OptionNr, IntPtr OptionID, IntPtr GroupType, IntPtr OptionType,
                                            IntPtr OptionName, IntPtr ShortName, int buflen);
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetOptionName(IntPtr hProb, int OptionNr);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetOptionNameBuf(IntPtr hProb, int OptionNr, IntPtr OptionName, int buflen);
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetOptionShortName(IntPtr hProb, int OptionNr);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetOptionShortNameBuf(IntPtr hProb, int OptionNr, IntPtr ShortName, int buflen);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetIntOptionMinMax(IntPtr hProb, int OptionNr, IntPtr MinValue, IntPtr MaxValue);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetRealOptionMinMax(IntPtr hProb, int OptionNr, IntPtr MinValue, IntPtr MaxValue);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetOptionChanged(IntPtr hProb, int OptionID);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetIntOption(IntPtr hProb, int OptionID);
        [DllImport("CoinMP.dll")]
        static extern int CoinSetIntOption(IntPtr hProb, int OptionID, int IntValue);
        [DllImport("CoinMP.dll")]
        static extern double CoinGetRealOption(IntPtr hProb, int OptionID);
        [DllImport("CoinMP.dll")]
        static extern int CoinSetRealOption(IntPtr hProb, int OptionID, double RealValue);
        [DllImport("CoinMP.dll")]
        static extern IntPtr CoinGetStringOption(IntPtr hProb, int OptionID);
        [DllImport("CoinMP.dll")]
        static extern int CoinGetStringOptionBuf(IntPtr hProb, int OptionID, IntPtr StringValue, int buflen);
        [DllImport("CoinMP.dll")]
        static extern int CoinSetStringOption(IntPtr hProb, int OptionID, IntPtr StringValue);
        #endregion
        /******************************************   CPLEX  10.1   *******************************************/
        #region CPLEX 10.1 [DllImport]
        /* Creating/Deleting Problems and Copying Data */
        [DllImport("cplex101.dll")]
        static extern IntPtr CPXcreateprob(IntPtr env, IntPtr status_p, IntPtr probname_str);
        [DllImport("cplex101.dll")]
        static extern int CPXcopylpwnames(IntPtr env, IntPtr lp, int numcols,
                                            int numrows, int objsense, IntPtr objective,
                                            IntPtr rhs, IntPtr sense, IntPtr matbeg,
                                            IntPtr matcnt, IntPtr matind, IntPtr matval,
                                            IntPtr lb, IntPtr ub, IntPtr rngval,
                                            IntPtr colname, IntPtr rowname);
        [DllImport("cplex101.dll")]
        static extern int CPXcopylp(IntPtr env, IntPtr lp, int numcols,
                                     int numrows, int objsense, IntPtr objective,
                                     IntPtr rhs, IntPtr sense, IntPtr matbeg,
                                     IntPtr matcnt, IntPtr matind, IntPtr matval,
                                     IntPtr lb, IntPtr ub, IntPtr rngval);
        [DllImport("cplex101.dll")]
        static extern int CPXcopyobjname(IntPtr env, IntPtr lp, IntPtr objname_str);
        [DllImport("cplex101.dll")]
        static extern int CPXfreeprob(IntPtr env, IntPtr lp_p);

        /* Optimizing Problems */
        [DllImport("cplex101.dll")]
        static extern int CPXlpopt (IntPtr env, IntPtr lp);
        [DllImport("cplex101.dll")]
        static extern int CPXmipopt (IntPtr env, IntPtr lp);

        /* Accessing LP results */
        [DllImport("cplex101.dll")]
        static extern int CPXsolution (IntPtr env, IntPtr lp, IntPtr lpstat_p,
                                       IntPtr objval_p, IntPtr x, IntPtr pi,
                                       IntPtr slack, IntPtr dj);
        [DllImport("cplex101.dll")]
        static extern int CPXgetstat (IntPtr env, IntPtr lp);
        [DllImport("cplex101.dll")]
        static extern IntPtr CPXgetstatstring (IntPtr env, int statind, IntPtr buffer_str);
        [DllImport("cplex101.dll")]
        static extern int CPXgetobjval (IntPtr env, IntPtr lp, IntPtr objval_p);
        [DllImport("cplex101.dll")]
        static extern int CPXgetbase (IntPtr env, IntPtr lp, IntPtr cstat, IntPtr rstat);
        [DllImport("cplex101.dll")]
        static extern int CPXgetitcnt(IntPtr env, IntPtr lp);

        /* Problem Query Routines */
        [DllImport("cplex101.dll")]
        static extern int CPXgetnumcols (IntPtr env, IntPtr lp);
        [DllImport("cplex101.dll")]
        static extern int CPXgetnumrows (IntPtr env, IntPtr lp);
        [DllImport("cplex101.dll")]
        static extern int CPXgetprobname (IntPtr env, IntPtr lp, IntPtr buf_str,
                                          int bufspace, IntPtr surplus_p);
        [DllImport("cplex101.dll")]
        static extern int CPXgetcolname(IntPtr env, IntPtr lp, IntPtr name,
                                        IntPtr namestore, int storespace,
                                        IntPtr surplus_p, int begin, int end);
        [DllImport("cplex101.dll")]
        static extern int CPXgetrowname  (IntPtr env, IntPtr lp, IntPtr name,
                                          IntPtr namestore, int storespace,
                                          IntPtr surplus_p, int begin, int end);

        /* Parameter Setting and Query Routines */
        [DllImport("cplex101.dll")]
        static extern int CPXsetdefaults (IntPtr env);
        [DllImport("cplex101.dll")]
        static extern int CPXsetintparam (IntPtr env, int whichparam, int newvalue);
        [DllImport("cplex101.dll")]
        static extern int CPXsetdblparam (IntPtr env, int whichparam, double newvalue);
        [DllImport("cplex101.dll")]
        static extern int CPXsetstrparam (IntPtr env, int whichparam, IntPtr newvalue_str);
        [DllImport("cplex101.dll")]
        static extern int CPXgetintparam (IntPtr env, int whichparam, IntPtr value_p);
        [DllImport("cplex101.dll")]
        static extern int CPXgetdblparam (IntPtr env, int whichparam, IntPtr value_p);
        [DllImport("cplex101.dll")]
        static extern int CPXgetstrparam (IntPtr env, int whichparam, IntPtr value_str);
        [DllImport("cplex101.dll")]
        static extern int CPXinfointparam (IntPtr env, int whichparam, IntPtr defvalue_p,
                                           IntPtr minvalue_p, IntPtr maxvalue_p);
        [DllImport("cplex101.dll")]
        static extern int CPXinfodblparam (IntPtr env, int whichparam, IntPtr defvalue_p, 
                                           IntPtr minvalue_p, IntPtr maxvalue_p);
        [DllImport("cplex101.dll")]
        static extern int CPXgetparamname (IntPtr env, int whichparam, IntPtr name_str);
        [DllImport("cplex101.dll")]
        static extern int CPXgetparamnum (IntPtr env, IntPtr name_str, IntPtr whichparam_p);

        /* Extra parameter routines */
        [DllImport("cplex101.dll")]
        static extern int CPXgetchgparams (IntPtr env, IntPtr nbparams, IntPtr paramnums);

        /* Utility Routines */
        [DllImport("cplex101.dll")]
        static extern IntPtr CPXversion (IntPtr env);
        [DllImport("cplex101.dll")]
        static extern IntPtr CPXopenCPLEX(IntPtr status_p);
        [DllImport("cplex101.dll")]
        static extern int CPXcloseCPLEX (IntPtr env_p);

        /* Copying Data */
        [DllImport("cplex101.dll")]
        static extern int CPXcopyctype(IntPtr env, IntPtr lp, IntPtr xctype);
        [DllImport("cplex101.dll")]
        static extern int CPXcopyorder (IntPtr env, IntPtr lp, int cnt,
                                        IntPtr indices, IntPtr priority, IntPtr direction);
        [DllImport("cplex101.dll")]
        static extern int CPXcopysos(IntPtr env, IntPtr lp, int numsos,
                                      int numsosnz, IntPtr sostype,
                                      IntPtr sosbeg, IntPtr sosind,
                                      IntPtr soswt, IntPtr sosname);
        [DllImport("cplex101.dll")]
        static extern int CPXcopyquad (IntPtr env, IntPtr lp, IntPtr qmatbeg, 
                                       IntPtr qmatcnt, IntPtr qmatind, IntPtr qmatval);

        /* Accessing MIP Results */
        [DllImport("cplex101.dll")]
        static extern int CPXgetbestobjval (IntPtr env, IntPtr lp, IntPtr objval_p);
        [DllImport("cplex101.dll")]
        static extern int CPXgetnodecnt (IntPtr env, IntPtr lp);

        /* File Reading & Writing Routines */
        [DllImport("cplex101.dll")]
        static extern int CPXreadcopyprob (IntPtr env, IntPtr lp, IntPtr filename_str,
                                           IntPtr filetype_str);
        [DllImport("cplex101.dll")]
        static extern int CPXwriteprob (IntPtr env, IntPtr lp, IntPtr filename_str,
                                        IntPtr filetype_str);

        /* Problem Modification Routines */
        [DllImport("cplex101.dll")]
        static extern int CPXaddrows (IntPtr env, IntPtr lp, int ccnt, int rcnt,
                                      int nzcnt, IntPtr rhs, IntPtr sense,
                                      IntPtr rmatbeg, IntPtr rmatind, IntPtr rmatval, 
                                      IntPtr colname, IntPtr rowname);
        [DllImport("cplex101.dll")]
        static extern int CPXaddcols (IntPtr env, IntPtr lp, int ccnt, int nzcnt,
                                      IntPtr obj, IntPtr cmatbeg, IntPtr cmatind, 
                                      IntPtr cmatval, IntPtr lb, IntPtr ub,
                                      IntPtr colname);
        [DllImport("cplex101.dll")]
        static extern int CPXdelrows (IntPtr env, IntPtr lp, int begin, int end);
        [DllImport("cplex101.dll")]
        static extern int CPXdelcols (IntPtr env, IntPtr lp, int begin, int end);
        #endregion

        /*********************************    Varibili private del Wrapper   ***************************************/
        private IntPtr CPLEXenvironment;
        private bool CoinMP;
        private bool CPLEX;
        private int status;

        /***********************************    Metodi privati del Wrapper   **************************************/
        private void ManageDllNotFound(ref string path, string dllname)
        {
            MessageBox.Show(dllname+" not found!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            FolderBrowserDialog diag = new FolderBrowserDialog();

            diag.Description = "Scegli il percorso del file "+dllname+":";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                path = diag.SelectedPath;
                path += "\\"+dllname;
            }
            else
            {
                Process myProcess = Process.GetCurrentProcess();
                myProcess.Kill();
            }
        }

        private int W_CoinInitSolver(String LicenseString)
        {
            byte[] license;
            IntPtr ls=new IntPtr();
            int x = 0;

            license = ConvertStringToByteArray(LicenseString);
            unsafe
            {
                if (LicenseString != "")
                {
                    fixed (byte* lsp = &license[0])
                    {
                        ls = (IntPtr)lsp;
                    }
                }
            }
            x=CoinInitSolver(ls);

            return x;
        }

        private int W_CoinFreeSolver()
        {
            int x = 0;

            x= CoinFreeSolver();

            return x;
        }

        private string ConvertIntPtrToString(IntPtr p)
        {
            int i=0, size;
            byte[] stringa;
            char[] cstringa;
            string s="";

            unsafe
            {
                byte * str = (byte*)p.ToPointer();

                while (str[i] != 0)
                    i++;
                size = i;

                if (size > 0)
                {
                    i = 0;
                    stringa = new byte[size];
                    cstringa = new char[size];

                    while (str[i] != 0)
                    {
                        stringa[i] = str[i];
                        i++;
                    }

                    for (i = 0; i < size; i++)
                        cstringa[i] = (char)stringa[i];

                    s = new string(cstringa);
                }
            }

            return s;
        }

        private string[] ConvertIntPtrToStringArray(IntPtr p, int nstring)
        {
            string[] s = new string[nstring];
            char[] word;
            int i, j, size;

            unsafe
            {
                byte** str = (byte**)p.ToPointer();

                i = 0;
                while (i < nstring)
                {
                    j = 0;
                    while (str[i][j] != 0)
                        j++;
                    size = j;
                    word = new char[size];
                    j = 0;
                    while (str[i][j] != 0)
                    {
                        word[j] = (char)str[i][j];
                        j++;
                    }
                    s[i] = new string(word);

                    i++;
                }
            }

            return s;
        }

        private int[] ConvertIntPtrToIntArray(IntPtr p)
        {
            int[] v;
            int i = 0, size;

            unsafe
            {
                int* vet = (int*)p.ToPointer();

                while (vet[i] != 0)
                    i++;
                size = i;

                if (size > 0)
                {
                    v = new int[size];

                    i = 0;
                    while (vet[i] != 0)
                    {
                        v[i] = vet[i];
                        i++;
                    }
                }
                else 
                    v = null;
            }

            return v;
        }

        private string W_CoinGetSolverName()
        {
            IntPtr psName;
            string sName="";


            psName = CoinGetSolverName();
            sName = ConvertIntPtrToString(psName);

            return sName;
        }

        private int W_CoinGetSolverNameBuf(out String SolverName, int buflen)
        {
            byte[] arr = new byte[buflen];
            char[] arr2;
            IntPtr p = new IntPtr();
            int x = 0;
            int i;

            if (buflen > 0)
            {
                unsafe
                {
                    fixed (byte* parr = &arr[0])
                    {
                        p = (IntPtr)parr;
                    }
                }
                x = CoinGetSolverNameBuf(p, buflen);
                arr2 = new char[x];
                for (i = 0; i < x; i++)
                    arr2[i] = (char)arr[i];
                SolverName = new String(arr2);
            }
            else
            {
                SolverName = "";
            }
            
            return x;
        }

        private string W_CoinGetVersionStr()
        {
            IntPtr pVersion;
            string sVersion = "";


            pVersion = CoinGetVersionStr();
            sVersion = ConvertIntPtrToString(pVersion);

            return sVersion;
        }

        private int W_CoinGetVersionStrBuf(out String VersionStr, int buflen)
        {
            byte[] arr = new byte[buflen];
            char[] arr2;
            IntPtr p = new IntPtr();
            int x = 0;
            int i;

            if (buflen > 0)
            {
                unsafe
                {
                    fixed (byte* parr = &arr[0])
                    {
                        p = (IntPtr)parr;
                    }
                }
                x = CoinGetVersionStrBuf(p, buflen);
                arr2 = new char[x];
                for (i = 0; i < x; i++)
                    arr2[i] = (char)arr[i];
                VersionStr = new String(arr2);
            }
            else
            {
                VersionStr = "";
            }

            return x;
        }

        private double W_CoinGetVersion()
        {
            double x = 0;

            x = CoinGetVersion();

            return x;
        }

        private int W_CoinGetFeatures()
        {
            int x = 0;

            x = CoinGetFeatures();

            return x;
        }

        private int W_CoinGetMethods()
        {
            int x = 0;

            x = CoinGetMethods();
         
            return x;
        }

        private double W_CoinGetInfinity()
        {
            double x = 0;

            x = CoinGetInfinity();

            return x;
        }

        private IntPtr W_CoinCreateProblem(String ProblemName)
        {
            byte[] probName;
            IntPtr probn=new IntPtr();
            IntPtr hProb=new IntPtr();

            if (ProblemName!="")
                probName = ConvertStringToByteArray(ProblemName);
            else
                probName = ConvertStringToByteArray("noname");
            unsafe
            {
                fixed (byte* pnp = &probName[0])
                {
                    probn = (IntPtr)pnp;
                }
            }
            hProb = CoinCreateProblem(probn);

            return hProb;
        }

        private byte[,] ConvertStringArrayToByte2DArray(String []s, int sizemax)
        {
            byte[,] arr;
            char[] arr2;
            int i,j;

            arr = new byte[s.Length, sizemax+1];

            for (i = 0; i < s.Length; i++)
            {
                arr2 = s[i].ToCharArray();
                for (j = 0; j < s[i].Length; j++)
                    arr[i,j] = (byte)arr2[j];
                while (j < sizemax+1)
                {
                    arr[i, j] = 0;
                    j++;
                }
            }

            return arr;
        }

        private byte[] ConvertStringToByteArray(String s)
        {
            byte[] arr=new byte [s.Length];
            char[] arr2;
            int i;

            arr2 = s.ToCharArray();
            for (i = 0; i < s.Length; i++)
                arr[i] = (byte)arr2[i];

            return arr;
        }

        private int W_CoinAddRows(IntPtr hProb, int RowCount, int[] MatrixBegin, int[] MatrixIndex,
                                  double[] MatrixValues, double[] RHSValues, double[] RangeValues,
                                  string RowType)
        {
            return CoinAddRows(hProb, RowCount, MatrixBegin, MatrixIndex, MatrixValues, RHSValues, RangeValues, RowType);
        }

        private int W_CoinLoadProblem(IntPtr hProb, int ColCount, int RowCount, int NZCount, int RangeCount,
                                      int ObjectSense, double ObjectConst, double[] ObjectCoeffs, double[] LowerBounds,
                                      double[] UpperBounds, String RowType, double[] RHSValues, double[] RangeValues,
                                      int[] MatrixBegin, int[] MatrixCount, int[] MatrixIndex, double[] MatrixValues,
                                      String[] ColNames, String[] RowNames, String ObjectName)
        {
            IntPtr[] cn = null, rn = null;
            if (ColNames != null)
            {
                cn = new IntPtr[ColNames.Length];
                for (int i = 0; i < cn.Length; i++)
                    cn[i] = Marshal.StringToHGlobalAnsi(ColNames[i]);
            }
            if (RowNames != null)
            {
                rn = new IntPtr[RowNames.Length];
                for (int i = 0; i < rn.Length; i++)
                    rn[i] = Marshal.StringToHGlobalAnsi(RowNames[i]);
            }
            //int res = CoinLoadProblem2(hProb, ColCount, RowCount, NZCount, RangeCount, ObjectSense, ObjectConst, ObjectCoeffs,
            //    LowerBounds, UpperBounds, RowType, RHSValues, RangeValues, MatrixBegin, MatrixCount, MatrixIndex, MatrixValues, ColNames == null ? null : cn, RowNames == null ? null : rn, ObjectName);

            int res = -1;
            try
            {
                res = CoinLoadProblem2(hProb,
                    ColCount,
                    RowCount,
                    NZCount,
                    RangeCount,
                    ObjectSense,
                    ObjectConst,
                    ObjectCoeffs,
                      LowerBounds,
                      UpperBounds,
                      RowType,
                      RHSValues,
                      null,
                      MatrixBegin,
                      MatrixCount,
                      MatrixIndex,
                      MatrixValues,
                      null,
                      null,
                      "");
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
                throw e;
            }

            if (cn != null)
                for (int i = 0; i < cn.Length; i++)
                    Marshal.FreeHGlobal(cn[i]);
            if (rn != null)
                for (int i = 0; i < rn.Length; i++)
                    Marshal.FreeHGlobal(rn[i]);

            return res;
        }

        private int W_CoinLoadInitValues(IntPtr hProb, double[] InitValues)
        {
            IntPtr iv = new IntPtr();
            int x = 0;


            unsafe
            {
                if (InitValues != null)
                {
                    fixed (double* ivp = &InitValues[0])
                    {
                        iv = (IntPtr)ivp;
                    }
                }
            }
            x = CoinLoadInitValues(hProb, iv);
            
            return x;
        }

        private int W_CoinLoadInteger(IntPtr hProb, String ColType)
        {
            byte[] col;
            IntPtr ct = new IntPtr();
            int x = 0;

            col = ConvertStringToByteArray(ColType);
            unsafe
            {
                if (ColType != "")
                {
                    fixed (byte* ctp = &col[0])
                    {
                        ct = (IntPtr)ctp;
                    }
                }
            }
            x = CoinLoadInteger(hProb, ct);

            return x;
        }

        private int W_CoinLoadPriority(IntPtr hProb, int PriorCount, int[] PriorIndex, int[] PriorValues,
                                      int[] BranchDir)
        {
            IntPtr pi = new IntPtr();
            IntPtr pv = new IntPtr();
            IntPtr bd = new IntPtr();
            int x = 0;

            unsafe
            {
                if (PriorIndex != null)
                {
                    fixed (int* pip = &PriorIndex[0])
                    {
                        pi = (IntPtr)pip;
                    }
                }
                if (PriorValues != null)
                {
                    fixed (int* pvp = &PriorValues[0])
                    {
                        pv = (IntPtr)pvp;
                    }
                }
                if (BranchDir != null)
                {
                    fixed (int* bdp = &BranchDir[0])
                    {
                        bd = (IntPtr)bdp;
                    }
                }
            }
            x = CoinLoadPriority(hProb, PriorCount, pi, pv, bd);

            return x;
        }

        private int W_CoinLoadSemiCont(IntPtr hProb, int SemiCount, int[] SemiIndex)
        {
            IntPtr si = new IntPtr();
            int x = 0;

            unsafe
            {
                if (SemiIndex != null)
                {
                    fixed (int* sip = &SemiIndex[0])
                    {
                        si = (IntPtr)sip;
                    }
                }
            }
            x = CoinLoadSemiCont(hProb, SemiCount, si);

            return x;
        }

        private int W_CoinLoadSos(IntPtr hProb, int SosCount, int SosNZCount, int[] SosType, int[] SosPrior,
                                 int[] SosBegin, int[] SosIndex, double[] SosRef)
        {
            IntPtr st = new IntPtr();
            IntPtr sp = new IntPtr();
            IntPtr sb = new IntPtr();
            IntPtr si = new IntPtr();
            IntPtr sr = new IntPtr();
            int x = 0;

            unsafe
            {
                if (SosType != null)
                {
                    fixed (int* stp = &SosType[0])
                    {
                        st = (IntPtr)stp;
                    }
                }
                if (SosPrior != null)
                {
                    fixed (int* spp = &SosPrior[0])
                    {
                        sp = (IntPtr)spp;
                    }
                }
                if (SosBegin != null)
                {
                    fixed (int* sbp = &SosBegin[0])
                    {
                        sb = (IntPtr)sbp;
                    }
                }
                if (SosIndex != null)
                {
                    fixed (int* sip = &SosIndex[0])
                    {
                        si = (IntPtr)sip;
                    }
                }
                if (SosRef != null)
                {
                    fixed (double* srp = &SosRef[0])
                    {
                        sr = (IntPtr)srp;
                    }
                }
            }
            x = CoinLoadSos(hProb, SosCount, SosNZCount, st, sp, sb, si, sr);

            return x;
        }

        private int W_CoinLoadQuadratic(IntPtr hProb, int[] QuadBegin, int[] QuadCount, int[] QuadIndex, double[] QuadValues)
        {
            IntPtr qb = new IntPtr();
            IntPtr qc = new IntPtr();
            IntPtr qi = new IntPtr();
            IntPtr qv = new IntPtr();
            int x = 0;

            unsafe
            {
                if (QuadBegin != null)
                {
                    fixed (int* qbp = &QuadBegin[0])
                    {
                        qb = (IntPtr)qbp;
                    }
                }
                if (QuadCount != null)
                {
                    fixed (int* qcp = &QuadCount[0])
                    {
                        qc = (IntPtr)qcp;
                    }
                }
                if (QuadIndex != null)
                {
                    fixed (int* qip = &QuadIndex[0])
                    {
                        qi = (IntPtr)qip;
                    }
                }
                if (QuadValues != null)
                {
                    fixed (double* qvp = &QuadValues[0])
                    {
                        qv = (IntPtr)qvp;
                    }
                }
            }
            x = CoinLoadQuadratic(hProb, qb, qc, qi, qv);

            return x;
        }

        private int W_CoinLoadNonLinear(IntPtr hProb, int NlpTreeCount, int NlpLineCount, int[] NlpBegin,
                                       int[] NlpOper, int[] NlpArg1, int[] NlpArg2, int[] NlpIndex1,
                                       int[] NlpIndex2, double[] NlpValue1, double[] NlpValue2)
        {
            IntPtr nb = new IntPtr();
            IntPtr no = new IntPtr();
            IntPtr na1 = new IntPtr();
            IntPtr na2 = new IntPtr();
            IntPtr ni1 = new IntPtr();
            IntPtr ni2 = new IntPtr();
            IntPtr nv1 = new IntPtr();
            IntPtr nv2 = new IntPtr();
            int x = 0;

            unsafe
            {
                if (NlpBegin != null)
                {
                    fixed (int* nbp = &NlpBegin[0])
                    {
                        nb = (IntPtr)nbp;
                    }
                }
                if (NlpOper != null)
                {
                    fixed (int* nop = &NlpOper[0])
                    {
                        no = (IntPtr)nop;
                    }
                }
                if (NlpArg1 != null)
                {
                    fixed (int* na1p = &NlpArg1[0])
                    {
                        na1 = (IntPtr)na1p;
                    }
                }
                if (NlpArg2 != null)
                {
                    fixed (int* na2p = &NlpArg2[0])
                    {
                        na2 = (IntPtr)na2p;
                    }
                }
                if (NlpIndex1 != null)
                {
                    fixed (int* ni1p = &NlpIndex1[0])
                    {
                        ni1 = (IntPtr)ni1p;
                    }
                }
                if (NlpIndex2 != null)
                {
                    fixed (int* ni2p = &NlpIndex2[0])
                    {
                        ni2 = (IntPtr)ni2p;
                    }
                }
                if (NlpValue1 != null)
                {
                    fixed (double* nv1p = &NlpValue1[0])
                    {
                        nv1 = (IntPtr)nv1p;
                    }
                }
                if (NlpValue2 != null)
                {
                    fixed (double* nv2p = &NlpValue2[0])
                    {
                        nv2 = (IntPtr)nv2p;
                    }
                }
            }

            x = CoinLoadNonLinear(hProb, NlpTreeCount, NlpLineCount, nb, no, na1,
                                     na2, ni1, ni2, nv1, nv2);

            return x;
        }

        private int W_CoinUnloadProblem(IntPtr hProb)
        {
            int x = 0;

            x = CoinUnloadProblem(hProb);

            return x;
        }

        private int W_CoinCheckProblem(IntPtr hProb)
        {
            int x = 0;

            x = CoinCheckProblem(hProb);

            return x;
        }

        private int W_CoinSetLoadNamesType(IntPtr hProb, int LoadNamesType)
        {
            int x = 0;

            x = CoinSetLoadNamesType(hProb, LoadNamesType);

            return x;
        }

        private string W_CoinGetProblemName(IntPtr hProb)
        {
            IntPtr ppName;
            string spName = "";

            ppName = CoinGetProblemName(hProb);
            spName = ConvertIntPtrToString(ppName);

            return spName;
        }

        private int W_CoinGetProblemNameBuf(IntPtr hProb, out String ProbName, int buflen)
        {
            byte[] arr = new byte[buflen];
            char[] arr2;
            IntPtr p = new IntPtr();
            int x = 0;
            int i;

            if (buflen > 0)
            {
                unsafe
                {
                    fixed (byte* parr = &arr[0])
                    {
                        p = (IntPtr)parr;
                    }
                }
                x = CoinGetProblemNameBuf(hProb, p, buflen);
                arr2 = new char[x];
                for (i = 0; i < x; i++)
                    arr2[i] = (char)arr[i];
                ProbName = new String(arr2);
            }
            else
            {
                ProbName = "";
            }

            return x;
        }

        private int W_CoinGetColCount(IntPtr hProb)
        {
            int x = 0;

            x = CoinGetColCount(hProb);

            return x;
        }

        private int W_CoinGetRowCount(IntPtr hProb)
        {
            int x = 0;

            x = CoinGetRowCount(hProb);

            return x;
        }

        private string W_CoinGetColName(IntPtr hProb, int col)
        {
            IntPtr pCol;
            string sCol = "";

            pCol = CoinGetColName(hProb, col);
            sCol = ConvertIntPtrToString(pCol);

            return sCol;
        }

        private int W_CoinGetColNameBuf(IntPtr hProb, int col, out String ColName, int buflen)
        {
            byte[] arr = new byte[buflen];
            char[] arr2;
            IntPtr p = new IntPtr();
            int x = 0;
            int i;

            if (buflen > 0)
            {
                unsafe
                {
                    fixed (byte* parr = &arr[0])
                    {
                        p = (IntPtr)parr;
                    }
                }
                x = CoinGetColNameBuf(hProb, col, p, buflen);
                arr2 = new char[x];
                for (i = 0; i < x; i++)
                    arr2[i] = (char)arr[i];
                ColName = new String(arr2);
            }
            else
            {
                ColName = "";
            }

            return x;
        }

        private string W_CoinGetRowName(IntPtr hProb, int row)
        {
            IntPtr pRow;
            string sRow = "";

            pRow = CoinGetRowName(hProb, row);
            sRow = ConvertIntPtrToString(pRow);

            return sRow;
        }

        private int W_CoinGetRowNameBuf(IntPtr hProb, int row, out String RowName, int buflen)
        {
            byte[] arr = new byte[buflen];
            char[] arr2;
            IntPtr p = new IntPtr();
            int x = 0;
            int i;

            if (buflen > 0)
            {
                unsafe
                {
                    fixed (byte* parr = &arr[0])
                    {
                        p = (IntPtr)parr;
                    }
                }
                x = CoinGetRowNameBuf(hProb, row, p, buflen);
                arr2 = new char[x];
                for (i = 0; i < x; i++)
                    arr2[i] = (char)arr[i];
                RowName = new String(arr2);
            }
            else
            {
                RowName = "";
            }

            return x;
        }

        private int W_CoinOptimizeProblem(IntPtr hProb, int Method)
        {
            int x = 0;
            
            x = CoinOptimizeProblem(hProb, Method);

            return x;
        }

        private int W_CoinGetSolutionStatus(IntPtr hProb)
        {
            int x = 0;

            
               x = CoinGetSolutionStatus(hProb);
            
            return x;
        }

        private string W_CoinGetSolutionText(IntPtr hProb, int SolutionStatus)
        {
            IntPtr psText;
            string ssText = "";

            psText = CoinGetSolutionText(hProb, SolutionStatus);
            ssText = ConvertIntPtrToString(psText);

            return ssText;
        }

        private int W_CoinGetSolutionTextBuf(IntPtr hProb, int SolutionStatus, out String SolutionText, int buflen)
        {
            byte[] arr = new byte[buflen];
            char[] arr2;
            IntPtr p = new IntPtr();
            int x = 0;
            int i;

            if (buflen > 0)
            {
                unsafe
                {
                    fixed (byte* parr = &arr[0])
                    {
                        p = (IntPtr)parr;
                    }
                }
                x = CoinGetSolutionTextBuf(hProb, SolutionStatus, p, buflen);
                arr2 = new char[x];
                for (i = 0; i < x; i++)
                    arr2[i] = (char)arr[i];
                SolutionText = new String(arr2);
            }
            else
            {
                SolutionText = "";
            }

            return x;
        }

        private double W_CoinGetObjectValue(IntPtr hProb)
        {
            double x = 0;

            x = CoinGetObjectValue(hProb);

            return x;
        }

        private double W_CoinGetMipBestBound(IntPtr hProb)
        {
            double x = 0;

            x = CoinGetMipBestBound(hProb);

            return x;
        }

        private int W_CoinGetIterCount(IntPtr hProb)
        {
            int x = 0;

            x = CoinGetIterCount(hProb);

            return x;
        }

        private int W_CoinGetMipNodeCount(IntPtr hProb)
        {
            int x = 0;

            x = CoinGetMipNodeCount(hProb);

            return x;
        }

        private int W_CoinGetSolutionValues(IntPtr hProb, double[] Activity,  double[] ReducedCost,
                                            double[] SlackValues,  double[] ShadowPrice)
        {
            IntPtr a = new IntPtr();
            IntPtr rc = new IntPtr();
            IntPtr sv = new IntPtr();
            IntPtr sp = new IntPtr();
            int x = 0;

            unsafe
            {
                if (Activity != null)
                {
                    fixed (double* ap = &Activity[0])
                    {
                        a = (IntPtr)ap;
                    }
                }
                if (ReducedCost != null)
                {
                    fixed (double* rcp = &ReducedCost[0])
                    {
                        rc = (IntPtr)rcp;
                    }
                }
                if (SlackValues != null)
                {
                    fixed (double* svp = &SlackValues[0])
                    {
                        sv = (IntPtr)svp;
                    }
                }
                if (ShadowPrice != null)
                {
                    fixed (double* spp = &ShadowPrice[0])
                    {
                        sp = (IntPtr)spp;
                    }
                }
            }
            x = CoinGetSolutionValues(hProb, a, rc, sv, sp);

            return x;       
        }

        private int W_CoinGetSolutionRanges(IntPtr hProb, double[] ObjLoRange, double[] ObjUpRange, double[] RhsLoRange, double[] RhsUpRange)
        {
            IntPtr olr = new IntPtr();
            IntPtr our = new IntPtr();
            IntPtr rlr = new IntPtr();
            IntPtr rur = new IntPtr();
            int x = 0;

            unsafe
            {
                if (ObjLoRange != null)
                {
                    fixed (double* olrp = &ObjLoRange[0])
                    {
                        olr = (IntPtr)olrp;
                    }
                }
                if (ObjUpRange != null)
                {
                    fixed (double* ourp = &ObjUpRange[0])
                    {
                        our = (IntPtr)ourp;
                    }
                }
                if (RhsLoRange != null)
                {
                    fixed (double* rlrp = &RhsLoRange[0])
                    {
                        rlr = (IntPtr)rlrp;
                    }
                }
                if (RhsUpRange != null)
                {
                    fixed (double* rurp = &RhsUpRange[0])
                    {
                        rur = (IntPtr)rurp;
                    }
                }
            }

            x = CoinGetSolutionRanges(hProb, olr, our, rlr, rur);

            return x;
        }

        private int W_CoinGetSolutionBasis(IntPtr hProb, int[] ColStatus, double[] RowStatus)
        {
            IntPtr cs = new IntPtr();
            IntPtr rs = new IntPtr();
            int x = 0;

            unsafe
            {
                if (ColStatus != null)
                {
                    fixed (int* csp = &ColStatus[0])
                    {
                        cs = (IntPtr)csp;
                    }
                }
                if (RowStatus != null)
                {
                    fixed (double* rsp = &RowStatus[0])
                    {
                        rs = (IntPtr)rsp;
                    }
                }
            }
            x = CoinGetSolutionBasis(hProb, cs, rs);

            return x;
        }

        private int W_CoinReadFile(IntPtr hProb, int FileType, String ReadFilename)
        {
            byte[] fname;
            IntPtr rf = new IntPtr();
            int x = 0;

            fname = ConvertStringToByteArray(ReadFilename);
            unsafe
            {
                if (ReadFilename != "")
                {
                    fixed (byte* rfp = &fname[0])
                    {
                        rf = (IntPtr)rfp;
                    }
                }
            }
            x = CoinReadFile(hProb, FileType, rf);

            return x;
        }

        private int W_CoinWriteFile(IntPtr hProb, int FileType, String WriteFilename)
        {
            byte[] fname;
            IntPtr wf = new IntPtr();
            int x = 0;

            fname = ConvertStringToByteArray(WriteFilename);
            unsafe
            {
                if (WriteFilename != "")
                {
                    fixed (byte* wfp = &fname[0])
                    {
                        wf = (IntPtr)wfp;
                    }
                }
            }
            x = CoinWriteFile(hProb, FileType, wf);

            return x;
        }

        private int W_CoinOpenLogFile(IntPtr hProb, String logFilename)
        {
            byte[] fname;
            IntPtr f = new IntPtr();
            int x = 0;

            fname = ConvertStringToByteArray(logFilename);
            unsafe
            {
                if (logFilename != "")
                {
                    fixed (byte* fp = &fname[0])
                    {
                        f = (IntPtr)fp;
                    }
                }
            }
            x = CoinOpenLogFile(hProb, f);

            return x;
        }

        private int W_CoinCloseLogFile(IntPtr hProb)
        {
            int x = 0;

            x = CoinCloseLogFile(hProb);

            return x;
        }

        private int W_CoinGetOptionCount(IntPtr hProb)
        {
            int x = 0;

            x = CoinGetOptionCount(hProb);

            return x;
        }

        private int W_CoinGetOptionInfo(IntPtr hProb, int OptionNr, out int OptionID, out int GroupType, out int OptionType,
                                       out String OptionName, out String ShortName, int buflen)
        {
            IntPtr oid = new IntPtr();
            IntPtr gt = new IntPtr();
            IntPtr ot = new IntPtr();
            IntPtr on = new IntPtr();
            IntPtr sn = new IntPtr();
            byte[] arr = new byte[buflen];
            char[] arr2;
            byte[] arr3=new byte[buflen];
            char[] arr4;
            int i, x = 0;
            String s1, s2;

            unsafe
            {
                fixed (int* oidp = &OptionID)
                {
                    oid = (IntPtr)oidp;
                }
                fixed (int* gtp = &GroupType)
                {
                    gt = (IntPtr)gtp;
                }
                fixed (int* otp = &OptionType)
                {
                    ot = (IntPtr)otp;
                }

                if (buflen > 0)
                {
                    fixed (byte* onp = &arr[0])
                    {
                        on = (IntPtr)onp;
                    }
                    fixed (byte* snp = &arr3[0])
                    {
                        sn = (IntPtr)snp;
                    }
                }
            }

            x = CoinGetOptionInfo(hProb, OptionNr, oid, gt, ot, on, sn, buflen);

            i = 0;
            while (arr[i] != 0)
                i++;
            arr2 = new char[i];
            i = 0;
            while (arr3[i] != 0)
                i++;
            arr4 = new char[i];

            i = 0;
            while (arr[i] != 0)
            {
                arr2[i] = (char)arr[i];
                i++;
            }

            i = 0;
            while (arr3[i] != 0)
            {
                arr4[i] = (char)arr3[i];
                i++;
            }

            s1 = new String(arr2);
            s2 = new String(arr4);

            OptionName = (String)s1.Clone();
            ShortName = (String)s2.Clone();

            return x;
        }

        private string W_CoinGetOptionName(IntPtr hProb, int OptionNr)
        {
            IntPtr poName;
            string soName = "";

            poName = CoinGetOptionName(hProb, OptionNr);
            soName = ConvertIntPtrToString(poName);

            return soName;
        }

        private int W_CoinGetOptionNameBuf(IntPtr hProb, int OptionNr, out String OptionName, int buflen)
        {
            IntPtr on = new IntPtr();
            byte[] arr = new byte[buflen];
            char[] arr2;
            int i, x = 0;
            String s1;

            unsafe
            {
                if (buflen > 0)
                {
                    fixed (byte* onp = &arr[0])
                    {
                        on = (IntPtr)onp;
                    }
                }
            }

            x = CoinGetOptionNameBuf(hProb, OptionNr, on, buflen);

            i = 0;
            while (arr[i] != 0)
                i++;
            arr2 = new char[i];

            i = 0;
            while (arr[i] != 0)
            {
                arr2[i] = (char)arr[i];
                i++;
            }

            s1 = new String(arr2);

            OptionName = (String)s1.Clone();

            return x;
        }

        private string W_CoinGetOptionShortName(IntPtr hProb, int OptionNr)
        {
            IntPtr posName;
            string sosName = "";

            posName = CoinGetOptionShortName(hProb, OptionNr);
            sosName = ConvertIntPtrToString(posName);

            return sosName;
        }

        private int W_CoinGetOptionShortNameBuf(IntPtr hProb, int OptionNr, out String ShortName, int buflen)
        {
            IntPtr sn = new IntPtr();
            byte[] arr = new byte[buflen];
            char[] arr2;
            int i, x = 0;
            String s1;

            unsafe
            {
                if (buflen > 0)
                {
                    fixed (byte* snp = &arr[0])
                    {
                        sn = (IntPtr)snp;
                    }
                }
            }

            x = CoinGetOptionShortNameBuf(hProb, OptionNr, sn, buflen);

            i = 0;
            while (arr[i] != 0)
                i++;
            arr2 = new char[i];

            i = 0;
            while (arr[i] != 0)
            {
                arr2[i] = (char)arr[i];
                i++;
            }

            s1 = new String(arr2);

            ShortName = (String)s1.Clone();

            return x;
        }

        private int W_CoinGetIntOptionMinMax(IntPtr hProb, int OptionNr, out int MinValue, out int MaxValue)
        {
            IntPtr minv = new IntPtr();
            IntPtr maxv = new IntPtr();
            int x = 0;

            unsafe
            {
                fixed (int* minvp = &MinValue)
                {
                    minv = (IntPtr)minvp;
                }
                fixed (int* maxvp = &MaxValue)
                {
                    maxv = (IntPtr)maxvp;
                }
            }

            x = CoinGetIntOptionMinMax(hProb, OptionNr, minv, maxv);

            return x;
        }

        private int W_CoinGetRealOptionMinMax(IntPtr hProb, int OptionNr, out double MinValue, out double MaxValue)
        {
            IntPtr minv = new IntPtr();
            IntPtr maxv = new IntPtr();
            int x = 0;

            unsafe
            {
                fixed (double* minvp = &MinValue)
                {
                    minv = (IntPtr)minvp;
                }
                fixed (double* maxvp = &MaxValue)
                {
                    maxv = (IntPtr)maxvp;
                }
            }

            x = CoinGetRealOptionMinMax(hProb, OptionNr, minv, maxv);

            return x;
        }

        private int W_CoinGetOptionChanged(IntPtr hProb, int OptionID)
        {
            int x = 0;

            x = CoinGetOptionChanged(hProb, OptionID);

            return x;
        }

        private int W_CoinGetIntOption(IntPtr hProb, int OptionID)
        {
            int x = 0;

            x = CoinGetIntOption(hProb, OptionID);

            return x;
        }

        private int W_CoinSetIntOption(IntPtr hProb, int OptionID, int IntValue)
        {
            int x = 0;

            x = CoinSetIntOption(hProb, OptionID, IntValue);

            return x;
        }

        private double W_CoinGetRealOption(IntPtr hProb, int OptionID)
        {
            double x = 0;

            x = CoinGetRealOption(hProb, OptionID);

            return x;
        }

        private int W_CoinSetRealOption(IntPtr hProb, int OptionID, double RealValue)
        {
            int x = 0;

            x = CoinSetRealOption(hProb, OptionID, RealValue);

            return x;
        }

        private string W_CoinGetStringOption(IntPtr hProb, int OptionID)
        {
            IntPtr psOption;
            string ssOption = "";

            psOption = CoinGetStringOption(hProb, OptionID);
            ssOption = ConvertIntPtrToString(psOption);

            return ssOption;
        }

        private int W_CoinGetStringOptionBuf(IntPtr hProb, int OptionID, out String StringValue, int buflen)
        {

            byte[] arr = new byte[buflen];
            char[] arr2;
            IntPtr p = new IntPtr();
            int x = 0;
            int i;

            if (buflen > 0)
            {
                unsafe
                {
                    fixed (byte* parr = &arr[0])
                    {
                        p = (IntPtr)parr;
                    }
                }
                x = CoinGetStringOptionBuf(hProb, OptionID, p, buflen);
                arr2 = new char[x];
                for (i = 0; i < x; i++)
                    arr2[i] = (char)arr[i];
                StringValue = new String(arr2);
            }
            else
            {
                StringValue = "";
            }

            return x;
        }

        private int W_CoinSetStringOption(IntPtr hProb, int OptionID, String StringValue)
        {
            byte[] strv;
            IntPtr sv = new IntPtr();
            int x = 0;

            strv = ConvertStringToByteArray(StringValue);
            unsafe
            {
                if (StringValue != "")
                {
                    fixed (byte* svp = &strv[0])
                    {
                        sv = (IntPtr)svp;
                    }
                }
            }
            x = CoinSetStringOption(hProb, OptionID, sv);

            return x;
        }

        private void WriteLogFile(StreamWriter logFile, string s)
        {
            if (logFile != null)
                logFile.Write(s);
        }

        private IntPtr W_RunTestProblem(String problemName, double optimalValue, int colCount, int rowCount,
                                     int nonZeroCount, int rangeCount, int objectSense, double objectConst, double[] objectCoeffs,
                                     double[] lowerBounds, double[] upperBounds, String rowType, double[] rhsValues, double[] rangeValues,
                                     int[] matrixBegin, int[] matrixCount, int[] matrixIndex, double[] matrixValues, String[] colNames,
                                     String[] rowNames, String objectName, double[] initValues, String columnType, int LoadNamesType, StreamWriter logFile, bool writeMPS, out string resultstring)
        {
            IntPtr hProb;
            int result;
            String filename;

            resultstring="";

            resultstring += "Solve problem: " + problemName + "\r\n";
            hProb = W_CoinCreateProblem(problemName);

            if (LoadNamesType > 0)
            {
                result = W_CoinSetLoadNamesType(hProb, LoadNamesType);
            }
            result = W_CoinLoadProblem(hProb, colCount, rowCount, nonZeroCount, rangeCount,
                    objectSense, objectConst, objectCoeffs, lowerBounds, upperBounds,
                    rowType, rhsValues, rangeValues, matrixBegin, matrixCount,
                    matrixIndex, matrixValues, colNames, rowNames, objectName);
            if (columnType != "")
            {
                result = W_CoinLoadInteger(hProb, columnType);
            }
            result = W_CoinCheckProblem(hProb);
            if (result != 0)
            {
                resultstring += "Check Problem failed (result = " + result.ToString() + ")\r\n";
            }

            result = W_CoinOptimizeProblem(hProb, 0);
            if (writeMPS)
            {
                filename = problemName + ".mps";
                result = W_CoinWriteFile(hProb, 3, filename);
            }
            resultstring+=W_GetAndCheckSolution(optimalValue, hProb, logFile);
            W_CoinUnloadProblem(hProb);

            WriteLogFile(logFile, resultstring);
            return hProb;
        }

        private IntPtr W_RunSosTestProblem(String problemName, double optimalValue, int colCount, int rowCount,
                               int nonZeroCount, int rangeCount, int objectSense, double objectConst, double[] objectCoeffs,
                               double[] lowerBounds, double[] upperBounds, String rowType, double[] rhsValues, double[] rangeValues,
                               int[] matrixBegin, int[] matrixCount, int[] matrixIndex, double[] matrixValues, String[] colNames,
                               String[] rowNames, String objectName, double[] initValues, String columnType, int LoadNamesType,
                               int sosCount, int sosNZCount, int[] sosType, int[] sosPrior, int[] sosBegin, int[] sosIndex, double[] sosRef, StreamWriter logFile, bool writeMPS, out string resultstring)
        {
            IntPtr hProb;
            int result;
            String filename;

            resultstring = "";

            resultstring += "Solve problem: " + problemName + "\r\n";
            hProb = W_CoinCreateProblem(problemName);
            result = W_CoinLoadProblem(hProb, colCount, rowCount, nonZeroCount, rangeCount,
                                         objectSense, objectConst, objectCoeffs, lowerBounds, upperBounds,
                                         rowType, rhsValues, rangeValues, matrixBegin, matrixCount,
                                         matrixIndex, matrixValues, colNames, rowNames, objectName);
            if (columnType != "")
            {
                result = W_CoinLoadInteger(hProb, columnType);
            }
            result = W_CoinLoadSos(hProb, sosCount, sosNZCount, sosType, sosPrior, sosBegin, sosIndex, sosRef);
            result = W_CoinCheckProblem(hProb);
            if (result != 0)
            {
                resultstring += "Check Problem failed (result = " + result.ToString() + ")\r\n";
            }

            if (writeMPS)
            {
                filename = problemName + ".mps";
                result = W_CoinWriteFile(hProb, 3, filename);
            }
            result = W_CoinOptimizeProblem(hProb, 0);
            resultstring+=W_GetAndCheckSolution(optimalValue, hProb, logFile);
            W_CoinUnloadProblem(hProb);

            WriteLogFile(logFile, resultstring);
            return hProb;
        }

        private string W_GetAndCheckSolution(double optimalValue, IntPtr hProb, StreamWriter logFile)
        {
            int solutionStatus;
            String solutionText;
            double objectValue;
            int i;
            int colCount;
            double[] xValues;
            String ColName;
            String problemName;
            string resultstring = "";

            W_CoinGetProblemNameBuf(hProb, out problemName, 200);
            //problemName=W_CoinGetProblemName(hProb);
            solutionStatus = W_CoinGetSolutionStatus(hProb);
            W_CoinGetSolutionTextBuf(hProb, solutionStatus, out solutionText, 200);
            //solutionText = W_CoinGetSolutionText(hProb, solutionStatus);
            objectValue = W_CoinGetObjectValue(hProb);

            resultstring += "\r\n---------------------------------------\r\n" +
                "Problem Name:    " + problemName + "\r\nSolution Result: " +
                solutionText + "\r\nSolution Status: " + solutionStatus.ToString() +
                "\r\nOptimal Value:   " + objectValue.ToString() +
                "\r\n---------------------------------------\r\n";

            colCount = W_CoinGetColCount(hProb);
            xValues = new double[colCount];
            W_CoinGetSolutionValues(hProb, xValues, null, null, null);

            for (i = 0; i < colCount; i++)
            {
                if (xValues[i] != 0.0)
                {
                    W_CoinGetColNameBuf(hProb, i, out ColName, 100);
                    //ColName = W_CoinGetColName(hProb, i);
                    resultstring += ColName + " = " + xValues[i].ToString() + "\r\n";
                }
            }

            resultstring += "---------------------------------------\r\n\r\n";
            Debug.Assert(solutionStatus == 0);
            Debug.Assert(solutionText.ToLower() == "optimal solution found");
            if (optimalValue != 0.0)
            {
                Debug.Assert(Math.Abs(objectValue - optimalValue) < 0.001);
            }
            return resultstring;
        }

        private IntPtr W_CPXcreateprob(IntPtr env, out int status_p, string probname_str)
        {
            IntPtr sp = new IntPtr();
            byte[] probName;
            IntPtr probn = new IntPtr();
            IntPtr probptr = new IntPtr();

            if (probname_str != "")
                probName = ConvertStringToByteArray(probname_str);
            else
                probName = ConvertStringToByteArray("noname");

            unsafe
            {
                fixed (int* spp = &status_p)
                {
                    sp = (IntPtr)spp;
                }
                fixed (byte* pnp = &probName[0])
                {
                    probn = (IntPtr)pnp;
                }
            }

            probptr = CPXcreateprob(env, sp, probn);

            return probptr;
        }

        private int W_CPXcopylpwnames(IntPtr env, IntPtr lp, int numcols,
                                     int numrows, int objsense, double[] objective,
                                     double[] rhs, string sense, int[] matbeg,
                                     int[] matcnt, int[] matind, double[] matval,
                                     double[] lb, double[] ub, double[] rngval,
                                     string[] colname, string[] rowname)
        {
            byte[,] Cols;
            byte[,] Rows;
            byte[] rowt;
            IntPtr cn = new IntPtr();
            IntPtr rn = new IntPtr();
            IntPtr ob = new IntPtr();
            IntPtr ilb = new IntPtr();
            IntPtr iub = new IntPtr();
            IntPtr rhsv = new IntPtr();
            IntPtr rv = new IntPtr();
            IntPtr mb = new IntPtr();
            IntPtr mc = new IntPtr();
            IntPtr mi = new IntPtr();
            IntPtr mv = new IntPtr();
            IntPtr rt = new IntPtr();
            int res = 0, i, colmax = -1, rowmax = -1;

            rowt = ConvertStringToByteArray(sense);

            for (i = 0; i < colname.Length; i++)
            {
                if (colname[i].Length > colmax)
                    colmax = colname[i].Length;
            }

            for (i = 0; i < rowname.Length; i++)
            {
                if (rowname[i].Length > rowmax)
                    rowmax = rowname[i].Length;
            }

            Cols = ConvertStringArrayToByte2DArray(colname, colmax);
            Rows = ConvertStringArrayToByte2DArray(rowname, rowmax);

            unsafe
            {
                if (objective != null)
                {
                    fixed (double* obp = &objective[0])
                    {
                        ob = (IntPtr)obp;
                    }
                }

                if (lb != null)
                {
                    fixed (double* lbp = &lb[0])
                    {
                        ilb = (IntPtr)lbp;
                    }
                }

                if (ub != null)
                {
                    fixed (double* ubp = &ub[0])
                    {
                        iub = (IntPtr)ubp;
                    }
                }

                if (rhs != null)
                {
                    fixed (double* rhsvp = &rhs[0])
                    {
                        rhsv = (IntPtr)rhsvp;
                    }
                }

                if (rngval != null)
                {
                    fixed (double* rvp = &rngval[0])
                    {
                        rv = (IntPtr)rvp;
                    }
                }

                if (matbeg != null)
                {
                    fixed (int* mbp = &matbeg[0])
                    {
                        mb = (IntPtr)mbp;
                    }
                }

                if (matcnt != null)
                {
                    fixed (int* mcp = &matcnt[0])
                    {
                        mc = (IntPtr)mcp;
                    }
                }

                if (matind != null)
                {
                    fixed (int* mip = &matind[0])
                    {
                        mi = (IntPtr)mip;
                    }
                }

                if (matval != null)
                {
                    fixed (double* mvp = &matval[0])
                    {
                        mv = (IntPtr)mvp;
                    }
                }

                if (sense != "")
                {
                    fixed (byte* rtp = &rowt[0])
                    {
                        rt = (IntPtr)rtp;
                    }
                }

                if (colname != null)
                {
                    fixed (byte* c = &Cols[0, 0])
                    {
                        byte** vet = (byte**)Marshal.AllocHGlobal(sizeof(byte*) * colname.Length);

                        for (i = 0; i < colname.Length; i++)
                        {
                            vet[i] = &c[i * (colmax + 1)];
                        }
                        cn = (IntPtr)vet;
                    }
                }
                if (rowname != null)
                {
                    fixed (byte* r = &Rows[0, 0])
                    {
                        byte** vet2 = (byte**)Marshal.AllocHGlobal(sizeof(byte*) * rowname.Length);

                        for (i = 0; i < rowname.Length; i++)
                        {
                            vet2[i] = &r[i * (rowmax + 1)];
                        }
                        rn = (IntPtr)vet2;
                    }
                }
            }
            res = CPXcopylpwnames(env, lp, numcols, numrows, objsense, ob, rhsv, rt, mb, mc, mi, mv, ilb, iub,
                                    rv, cn, rn);

            Marshal.FreeHGlobal(cn);
            Marshal.FreeHGlobal(rn);

            return res;
        }


        private int W_CPXcopylp(IntPtr env, IntPtr lp, int numcols,
                                int numrows, int objsense, double[] objective,
                                double[] rhs, string sense, int[] matbeg,
                                int[] matcnt, int[] matind, double[] matval,
                                double[] lb, double[] ub, double[] rngval)
        {
            byte[] rowt;
            IntPtr ob = new IntPtr();
            IntPtr ilb = new IntPtr();
            IntPtr iub = new IntPtr();
            IntPtr rhsv = new IntPtr();
            IntPtr rv = new IntPtr();
            IntPtr mb = new IntPtr();
            IntPtr mc = new IntPtr();
            IntPtr mi = new IntPtr();
            IntPtr mv = new IntPtr();
            IntPtr rt = new IntPtr();
            int res = 0;

            rowt = ConvertStringToByteArray(sense);

            unsafe
            {
                if (objective != null)
                {
                    fixed (double* obp = &objective[0])
                    {
                        ob = (IntPtr)obp;
                    }
                }

                if (lb != null)
                {
                    fixed (double* lbp = &lb[0])
                    {
                        ilb = (IntPtr)lbp;
                    }
                }

                if (ub != null)
                {
                    fixed (double* ubp = &ub[0])
                    {
                        iub = (IntPtr)ubp;
                    }
                }

                if (rhs != null)
                {
                    fixed (double* rhsvp = &rhs[0])
                    {
                        rhsv = (IntPtr)rhsvp;
                    }
                }

                if (rngval != null)
                {
                    fixed (double* rvp = &rngval[0])
                    {
                        rv = (IntPtr)rvp;
                    }
                }

                if (matbeg != null)
                {
                    fixed (int* mbp = &matbeg[0])
                    {
                        mb = (IntPtr)mbp;
                    }
                }

                if (matcnt != null)
                {
                    fixed (int* mcp = &matcnt[0])
                    {
                        mc = (IntPtr)mcp;
                    }
                }

                if (matind != null)
                {
                    fixed (int* mip = &matind[0])
                    {
                        mi = (IntPtr)mip;
                    }
                }

                if (matval != null)
                {
                    fixed (double* mvp = &matval[0])
                    {
                        mv = (IntPtr)mvp;
                    }
                }

                if (sense != "")
                {
                    fixed (byte* rtp = &rowt[0])
                    {
                        rt = (IntPtr)rtp;
                    }
                }
            }
            res = CPXcopylp(env, lp, numcols, numrows, objsense, ob, rhsv, rt, mb, mc, mi, mv, ilb, iub,
                                    rv);

            return res;
        }

        private int W_CPXcopyobjname(IntPtr env, IntPtr lp, string objname_str)
        {
            byte[] objn;
            IntPtr on = new IntPtr();
            int res = 0;

            objn = ConvertStringToByteArray(objname_str);

            unsafe
            {
                if (objname_str != "")
                {
                    fixed (byte* onp = &objn[0])
                    {
                        on = (IntPtr)onp;
                    }
                }
            }

            res = CPXcopyobjname(env, lp, on);

            return res;
        }

        private int W_CPXfreeprob(IntPtr env, ref IntPtr lp)
        {
            int res = 0;
            IntPtr lpp = new IntPtr();

            unsafe
            {
                fixed (void* ilp = &lp)
                {
                    lpp = (IntPtr)ilp;
                }
            }

            res = CPXfreeprob(env, lpp);

            return res;
        }

        private int W_CPXlpopt(IntPtr env, IntPtr lp)
        {
            int res = 0;

            res = CPXlpopt(env, lp);

            return res;
        }

        private int W_CPXmipopt(IntPtr env, IntPtr lp)
        {
            int res = 0;

            res = CPXmipopt(env, lp);

            return res;
        }

        private int W_CPXsolution(IntPtr env, IntPtr lp, out int lpstat_p,
                                  out double objval_p, double [] x, double [] pi,
                                  double [] slack, double [] dj)
        {
            IntPtr lsp = new IntPtr();
            IntPtr ovp = new IntPtr();
            IntPtr xp = new IntPtr();
            IntPtr pip = new IntPtr();
            IntPtr sp = new IntPtr();
            IntPtr dp = new IntPtr();
            int res = 0;

            unsafe
            {
                fixed (int* lspp = &lpstat_p)
                {
                    lsp = (IntPtr)lspp;
                }
                fixed (double* ovpp = &objval_p)
                {
                    ovp = (IntPtr)ovpp;
                }
                if (x != null)
                {
                    fixed (double* xpp = &x[0])
                    {
                        xp = (IntPtr)xpp;
                    }
                }
                if (pi != null)
                {
                    fixed (double* pipp = &pi[0])
                    {
                        pip = (IntPtr)pipp;
                    }
                }
                if (slack != null)
                {
                    fixed (double* spp = &slack[0])
                    {
                        sp = (IntPtr)spp;
                    }
                }
                if (dj != null)
                {
                    fixed (double* dpp = &dj[0])
                    {
                        dp = (IntPtr)dpp;
                    }
                }
            }

            res = CPXsolution(env, lp, lsp, ovp, xp, pip, sp, dp);

            return res;
        }

        private int W_CPXgetstat(IntPtr env, IntPtr lp)
        {
            int res = 0;

            res = CPXgetstat(env, lp);

            return res;
        }

        private string W_CPXgetstatstring(IntPtr env, int statind, out string buffer_str)
        {
            byte[] arr = new byte[510];
            char[] arr2;
            IntPtr p = new IntPtr();
            IntPtr x = new IntPtr();
            int i, length;
            string statstring;

            unsafe
            {
                fixed (byte* parr = &arr[0])
                {
                    p = (IntPtr)parr;
                }
            }

            x = CPXgetstatstring(env, statind, p);

            i = 0;
            while (arr[i] != '\0')
                i++;
            length = i;

            arr2 = new char[length];
            for (i = 0; i < length; i++)
                arr2[i] = (char)arr[i];
            buffer_str = new String(arr2);

            statstring = ConvertIntPtrToString(x);

            return statstring;
        }

        private int W_CPXgetobjval(IntPtr env, IntPtr lp, out double objval_p)
        {
            IntPtr ovp = new IntPtr();
            int res = 0;

            unsafe
            {
                fixed (double* ovpp = &objval_p)
                {
                    ovp = (IntPtr)ovpp;
                }
            }

            res = CPXgetobjval(env, lp, ovp);

            return res;
        }

        private int W_CPXgetbase(IntPtr env, IntPtr lp, int [] cstat, int [] rstat)
        {
            IntPtr csp = new IntPtr();
            IntPtr rsp = new IntPtr();
            int res = 0;

            unsafe
            {
                if (cstat != null)
                {
                    fixed (int* cspp = &cstat[0])
                    {
                        csp = (IntPtr)cspp;
                    }
                }
                if (rstat != null)
                {
                    fixed (int* rspp = &rstat[0])
                    {
                        rsp = (IntPtr)rspp;
                    }
                }
            }

            res = CPXgetbase(env, lp, csp, rsp);

            return res;
        }

        private int W_CPXgetitcnt(IntPtr env, IntPtr lp)
        {
            int res = 0;

            res = CPXgetitcnt(env, lp);

            return res;
        }

        private int W_CPXgetnumcols(IntPtr env, IntPtr lp)
        {
            int res = 0;

            res = CPXgetnumcols(env, lp);

            return res;
        }

        private int W_CPXgetnumrows(IntPtr env, IntPtr lp)
        {
            int res = 0;

            res = CPXgetnumrows(env, lp);

            return res;
        }

        private int W_CPXgetprobname(IntPtr env, IntPtr lp, out string buf_str,
                                     int bufspace, out int surplus_p)
        {
            IntPtr bs = new IntPtr();
            IntPtr sp = new IntPtr();
            int res = 0, i, length;
            byte[] arr = new byte[bufspace];
            char[] arr2;

            unsafe
            {
                fixed (byte* p = &arr[0])
                {
                    bs = (IntPtr)p;
                }
                fixed (int* spp = &surplus_p)
                {
                    sp = (IntPtr)spp;
                }
            }

            res = CPXgetprobname(env, lp, bs, bufspace, sp);
            
            i = 0;
            while (arr[i] != '\0')
                i++;
            length = i;

            arr2 = new char[length];
            for (i = 0; i < length; i++)
                arr2[i] = (char)arr[i];
            buf_str = new String(arr2);

            return res;
        }

        private int W_CPXgetcolname (IntPtr env, IntPtr lp, out string [] name,
                   out string namestore, int storespace,
                   out int surplus_p, int begin, int end)
        {
            IntPtr np = new IntPtr();
            IntPtr ns = new IntPtr();
            IntPtr sp = new IntPtr();
            int res = 0, i, j, k, maxlength = 0, length, totallength;
            byte[] arr;
            byte[,] tr;
            char[,] arr2;
            char[] arr3;

            if (storespace > 0)
            {
                arr = new byte[storespace];
                if (storespace / (end - begin + 1)>0)
                    tr = new byte[end - begin + 1, storespace / (end - begin + 1)];
                else
                    tr = new byte[end - begin + 1, (end - begin + 1) / (end - begin + 1)];
            }
            else
            {
                tr = new byte[end - begin + 1, (end-begin+1) / (end - begin + 1)];
                arr = new byte[1];
            }
            unsafe
            {
                fixed (byte* s = &tr[0, 0])
                {
                    byte** vet = (byte**)Marshal.AllocHGlobal(sizeof(byte*) * (end - begin + 1));

                    for (i = 0; i < (end - begin + 1); i++)
                    {
                        vet[i] = &s[i * (storespace / (end - begin + 1) + 1)];
                    }
                    np = (IntPtr)vet;
                }
                if (storespace > 0)
                {
                    fixed (byte* p = &arr[0])
                    {
                        ns = (IntPtr)p;
                    }
                }
                fixed (int* spp = &surplus_p)
                {
                    sp = (IntPtr)spp;
                }
            }

            res = CPXgetcolname(env, lp, np, ns, storespace, sp, begin, end);

            if (res==0)
            {
                i = 0;
                j = 0;
                totallength = 0;
                while (i < end - begin + 1)
                {
                    k = 0;
                    while (arr[j] != '\0')
                    {
                        j++;
                        k++;
                    }
                    length = k;
                    totallength += length;
                    if (maxlength < length)
                        maxlength = length;

                    while (j < arr.Length && arr[j] == '\0')
                        j++;
                    i++;
                }

                totallength += end - begin + 1;
                i = 0;
                j = 0;
                arr2 = new char[end - begin + 1, maxlength];
                arr3 = new char[totallength];
                while (i < end - begin + 1)
                {
                    k = 0;
                    while (arr[j] != '\0')
                    {
                        arr3[j] = (char)arr[j];
                        k++;
                        j++;
                    }
                    length = k;
                    j++;

                    for (k = 0; k < length; k++)
                        arr2[i, k] = (char)arr[j - 1 - length + k];

                    while (j < arr.Length && arr[j] == '\0')
                        j++;
                    i++;
                }

                namestore = new string(arr3);
                namestore = namestore.Remove(namestore.Length - 1);
                namestore = namestore.Replace("\0", "\r\n");
                name = ConvertIntPtrToStringArray(np, end - begin + 1);
            }
            else
            {
                namestore = "";
                name = null;
            }

            return res;
        }

        private int W_CPXgetrowname(IntPtr env, IntPtr lp, out string [] name,
                                          out string namestore, int storespace,
                                          out int surplus_p, int begin, int end)
        {
            IntPtr np = new IntPtr();
            IntPtr ns = new IntPtr();
            IntPtr sp = new IntPtr();
            int res = 0, i, j, k, maxlength = 0, length, totallength;
            byte[] arr;
            byte[,] tr;
            char[,] arr2;
            char[] arr3;

            if (storespace > 0)
            {
                arr = new byte[storespace];
                if (storespace / (end - begin + 1) > 0)
                    tr = new byte[end - begin + 1, storespace / (end - begin + 1)];
                else
                    tr = new byte[end - begin + 1, (end - begin + 1) / (end - begin + 1)];
            }
            else
            {
                tr = new byte[end - begin + 1, (end - begin + 1) / (end - begin + 1)];
                arr = new byte[1];
            }
            unsafe
            {
                fixed (byte* s = &tr[0, 0])
                {
                    byte** vet = (byte**)Marshal.AllocHGlobal(sizeof(byte*) * (end - begin + 1));

                    for (i = 0; i < (end - begin + 1); i++)
                    {
                        vet[i] = &s[i * (storespace / (end - begin + 1) + 1)];
                    }
                    np = (IntPtr)vet;
                }
                if (storespace > 0)
                {
                    fixed (byte* p = &arr[0])
                    {
                        ns = (IntPtr)p;
                    }
                }
                fixed (int* spp = &surplus_p)
                {
                    sp = (IntPtr)spp;
                }
            }

            res = CPXgetrowname(env, lp, np, ns, storespace, sp, begin, end);

            if (res == 0)
            {
                i = 0;
                j = 0;
                totallength = 0;
                while (i < end - begin + 1)
                {
                    k = 0;
                    while (arr[j] != '\0')
                    {
                        j++;
                        k++;
                    }
                    length = k;
                    totallength += length;
                    if (maxlength < length)
                        maxlength = length;

                    while (j < arr.Length && arr[j] == '\0')
                        j++;
                    i++;
                }

                totallength += end - begin + 1;
                i = 0;
                j = 0;
                arr2 = new char[end - begin + 1, maxlength];
                arr3 = new char[totallength];
                while (i < end - begin + 1)
                {
                    k = 0;
                    while (arr[j] != '\0')
                    {
                        arr3[j] = (char)arr[j];
                        k++;
                        j++;
                    }
                    length = k;
                    j++;

                    for (k = 0; k < length; k++)
                        arr2[i, k] = (char)arr[j - 1 - length + k];

                    while (j < arr.Length && arr[j] == '\0')
                        j++;
                    i++;
                }

                namestore = new string(arr3);
                namestore = namestore.Remove(namestore.Length - 1);
                namestore = namestore.Replace("\0", "\r\n");
                name = ConvertIntPtrToStringArray(np, end - begin + 1);
            }
            else
            {
                namestore = null;
                name = null;
            }

            return res;
        }

        private int W_CPXsetdefaults(IntPtr env)
        {
            int res = 0;

            res = CPXsetdefaults(env);

            return res;
        }

        private int W_CPXsetintparam(IntPtr env, int whichparam, int newvalue)
        {
            int res = 0;

            res = CPXsetintparam(env, whichparam, newvalue);

            return res;
        }

        private int W_CPXsetdblparam(IntPtr env, int whichparam, double newvalue)
        {
            int res = 0;

            res = CPXsetdblparam(env, whichparam, newvalue);

            return res;
        }

        private int W_CPXsetstrparam(IntPtr env, int whichparam, string newvalue_str)
        {
            byte[] strv;
            IntPtr nvs = new IntPtr();
            int res = 0;

            strv = ConvertStringToByteArray(newvalue_str);
            unsafe
            {
                if (newvalue_str!=null)
                {
                    fixed (byte* nvsp = &strv[0])
                    {
                        nvs = (IntPtr)nvsp;
                    }
                }
            }
            res = CPXsetstrparam(env, whichparam, nvs);

            return res;
        }

        private int W_CPXgetintparam(IntPtr env, int whichparam, out int value_p)
        {
            IntPtr vp = new IntPtr();
            int res = 0;

            unsafe
            {
                fixed (int* vpp = &value_p)
                {
                    vp = (IntPtr)vpp;
                }
            }

            res = CPXgetintparam(env, whichparam, vp);

            return res;
        }

        private int W_CPXgetdblparam(IntPtr env, int whichparam, out double value_p)
        {
            IntPtr vp = new IntPtr();
            int res = 0;

            unsafe
            {
                fixed (double* vpp = &value_p)
                {
                    vp = (IntPtr)vpp;
                }
            }

            res = CPXgetdblparam(env, whichparam, vp);

            return res;
        }

        private int W_CPXgetstrparam(IntPtr env, int whichparam, out string value_str)
        {
            byte[] arr = new byte[510];
            char[] arr2;
            IntPtr p = new IntPtr();
            int i, length, res = 0;

            unsafe
            {
                fixed (byte* parr = &arr[0])
                {
                    p = (IntPtr)parr;
                }
            }

            res = CPXgetstrparam(env, whichparam, p);

            i = 0;
            while (arr[i] != '\0')
                i++;
            length = i;

            arr2 = new char[length];
            for (i = 0; i < length; i++)
                arr2[i] = (char)arr[i];
            value_str = new String(arr2);

            return res;
        }

        private int W_CPXinfointparam(IntPtr env, int whichparam, out int defvalue_p,
                                      out int minvalue_p, out int maxvalue_p)
        {
            IntPtr dvp = new IntPtr();
            IntPtr mnvp = new IntPtr();
            IntPtr mxvp = new IntPtr();
            int res = 0;

            unsafe
            {
                fixed (int* dvpp = &defvalue_p)
                {
                    dvp = (IntPtr)dvpp;
                }
                fixed (int* mnvpp = &minvalue_p)
                {
                    mnvp = (IntPtr)mnvpp;
                }
                fixed (int* mxvpp = &maxvalue_p)
                {
                    mxvp = (IntPtr)mxvpp;
                }
            }

            res = CPXinfointparam(env, whichparam, dvp, mnvp, mxvp);

            return res;
        }

        private int W_CPXinfodblparam(IntPtr env, int whichparam, out double defvalue_p,
                                      out double minvalue_p, out double maxvalue_p)
        {
            IntPtr dvp = new IntPtr();
            IntPtr mnvp = new IntPtr();
            IntPtr mxvp = new IntPtr();
            int res = 0;

            unsafe
            {
                fixed (double* dvpp = &defvalue_p)
                {
                    dvp = (IntPtr)dvpp;
                }
                fixed (double* mnvpp = &minvalue_p)
                {
                    mnvp = (IntPtr)mnvpp;
                }
                fixed (double* mxvpp = &maxvalue_p)
                {
                    mxvp = (IntPtr)mxvpp;
                }
            }

            res = CPXinfodblparam(env, whichparam, dvp, mnvp, mxvp);

            return res;
        }

        private int W_CPXgetparamname(IntPtr env, int whichparam, out string name_str)
        {
            byte[] arr = new byte[510];
            char[] arr2;
            IntPtr p = new IntPtr();
            int i, length, res = 0;

            unsafe
            {
                fixed (byte* parr = &arr[0])
                {
                    p = (IntPtr)parr;
                }
            }

            res = CPXgetstrparam(env, whichparam, p);

            i = 0;
            while (arr[i] != '\0')
                i++;
            length = i;

            arr2 = new char[length];
            for (i = 0; i < length; i++)
                arr2[i] = (char)arr[i];
            name_str = new String(arr2);

            return res;
        }

        private int W_CPXgetparamnum(IntPtr env, string name_str, out int whichparam_p)
        {
            byte[] strv;
            IntPtr wpp = new IntPtr();
            IntPtr ns = new IntPtr();

            strv = ConvertStringToByteArray(name_str);
            int res = 0;

            unsafe
            {
                fixed (byte* nsp = &strv[0])
                {
                    ns = (IntPtr) nsp;
                }
                fixed (int* wppp = &whichparam_p)
                {
                    wpp = (IntPtr)wppp;
                }
            }

            res = CPXgetparamnum(env, ns, wpp);

            return res;
        }

        private int W_CPXgetchgparams(IntPtr env, out int[] nbparams, out int[] paramnums)
        {
            IntPtr np = new IntPtr();
            IntPtr pn = new IntPtr();
            int res = 0;
            int[] v1 = new int[1000];
            int[] v2 = new int[1000];

            unsafe
            {
                fixed (int* npp = &v1[0])
                {
                    np = (IntPtr)npp;
                }
                fixed (int* pnp = &v2[0])
                {
                    pn = (IntPtr)pnp;
                }
            }

            res = CPXgetchgparams(env, np, pn);

            nbparams = ConvertIntPtrToIntArray(np);
            paramnums = ConvertIntPtrToIntArray(pn);

            return res;
        }

        private string W_CPXversion(IntPtr env)
        {
            IntPtr pVersion;
            string sVersion = "";


            pVersion = CPXversion(env);
            sVersion = ConvertIntPtrToString(pVersion);

            return sVersion;
        }

        private IntPtr W_CPXopenCPLEX(out int status_p)
        {
            IntPtr sp = new IntPtr();
            IntPtr envptr = new IntPtr();

            unsafe
            {
                fixed (int* spp = &status_p)
                {
                    sp = (IntPtr)spp;
                }
            }

            envptr = CPXopenCPLEX(sp);

            return envptr;
        }

        private int W_CPXcloseCPLEX(ref IntPtr env_p)
        {
            int res = 0;
            IntPtr ep = new IntPtr();

            unsafe
            {
                fixed (void* iep = &env_p)
                {
                    ep = (IntPtr)iep;
                }
            }

            res = CPXcloseCPLEX(ep);

            return res;
        }

        private int W_CPXcopyctype(IntPtr env, IntPtr lp, string xctype)
        {
            byte[] columnt;
            IntPtr ct = new IntPtr();
            int res = 0;

            columnt = ConvertStringToByteArray(xctype);

            unsafe
            {
                if (xctype != "")
                {
                    fixed (byte* ctp = &columnt[0])
                    {
                        ct = (IntPtr)ctp;
                    }
                }
            }
            res = CPXcopyctype(env, lp, ct);

            return res;
        }

        private int W_CPXcopyorder(IntPtr env, IntPtr lp, int cnt,
                                  int [] indices, int [] priority, int [] direction)
        {
            IntPtr ind = new IntPtr();
            IntPtr pri = new IntPtr();
            IntPtr dir = new IntPtr();
            int res = 0;

            unsafe
            {
                if (indices != null)
                {
                    fixed (int* indp = &indices[0])
                    {
                        ind = (IntPtr)indp;
                    }
                }
                if (priority != null)
                {
                    fixed (int* prip = &priority[0])
                    {
                        pri = (IntPtr)prip;
                    }
                }
                if (direction != null)
                {
                    fixed (int* dirp = &direction[0])
                    {
                        dir = (IntPtr)dirp;
                    }
                }
            }

            res = CPXcopyorder(env, lp, cnt, ind, pri, dir);

            return res;
        }

        private int W_CPXcopysos(IntPtr env, IntPtr lp, int numsos,
                                      int numsosnz, string sostype,
                                      int[] sosbeg, int[] sosind,
                                      double[] soswt, string[] sosname)
        {
            byte[] sost;
            byte[,] sosn;
            IntPtr st = new IntPtr();
            IntPtr sb = new IntPtr();
            IntPtr si = new IntPtr();
            IntPtr sw = new IntPtr();
            IntPtr sn = new IntPtr();
            int res = 0, i, sosmax = 0;

            sost = ConvertStringToByteArray(sostype);

            for (i = 0; i < sosname.Length; i++)
            {
                if (sosname[i].Length > sosmax)
                    sosmax = sosname[i].Length;
            }

            sosn = ConvertStringArrayToByte2DArray(sosname, sosmax);

            unsafe
            {
                if (sostype != "")
                {
                    fixed (byte* stp = &sost[0])
                    {
                        st = (IntPtr)stp;
                    }
                }
                if (sosbeg != null)
                {
                    fixed (int* sbp = &sosbeg[0])
                    {
                        sb = (IntPtr)sbp;
                    }
                }
                if (sosind != null)
                {
                    fixed (int* sip = &sosind[0])
                    {
                        si = (IntPtr)sip;
                    }
                }
                if (soswt != null)
                {
                    fixed (double* swp = &soswt[0])
                    {
                        sw = (IntPtr)swp;
                    }
                }
                if (sosname != null)
                {
                    fixed (byte* s = &sosn[0, 0])
                    {
                        byte** vet = (byte**)Marshal.AllocHGlobal(sizeof(byte*) * sosname.Length);

                        for (i = 0; i < sosname.Length; i++)
                        {
                            vet[i] = &s[i * (sosmax + 1)];
                        }
                        sn = (IntPtr)vet;
                    }
                }
            }

            res = CPXcopysos(env, lp, numsos, numsosnz, st, sb, si, sw, sn);

            Marshal.FreeHGlobal(sn);

            return res;
        }

        private int W_CPXcopyquad(IntPtr env, IntPtr lp, int [] qmatbeg,
                                 int [] qmatcnt, int [] qmatind, double [] qmatval)
        {
            IntPtr qmb = new IntPtr();
            IntPtr qmc = new IntPtr();
            IntPtr qmi = new IntPtr();
            IntPtr qmv = new IntPtr();
            int res = 0;

            unsafe
            {
                if (qmatbeg != null)
                {
                    fixed (int* qmbp = &qmatbeg[0])
                    {
                        qmb = (IntPtr)qmbp;
                    }
                }
                if (qmatcnt != null)
                {
                    fixed (int* qmcp = &qmatcnt[0])
                    {
                        qmc = (IntPtr)qmcp;
                    }
                }
                if (qmatind != null)
                {
                    fixed (int* qmip = &qmatind[0])
                    {
                        qmi = (IntPtr)qmip;
                    }
                }
                if (qmatval != null)
                {
                    fixed (double* qmvp = &qmatval[0])
                    {
                        qmv = (IntPtr)qmvp;
                    }
                }
            }

            res = CPXcopyquad(env, lp, qmb, qmc, qmi, qmv);

            return res;
        }

        private int W_CPXgetbestobjval(IntPtr env, IntPtr lp, out double objval_p)
        {
            IntPtr ovp = new IntPtr();
            int res = 0;

            unsafe
            {
                fixed (double* ovpp = &objval_p)
                {
                    ovp = (IntPtr)ovpp;
                }
            }

            res = CPXgetbestobjval(env, lp, ovp);

            return res;
        }

        private int W_CPXgetnodecnt(IntPtr env, IntPtr lp)
        {
            int res = 0;

            res = CPXgetnodecnt(env, lp);

            return res;
        }

        private int W_CPXreadcopyprob(IntPtr env, IntPtr lp, string filename_str,
                                     string filetype_str)
        {
            byte[] fsarr;
            byte[] ftsarr;
            IntPtr fs = new IntPtr();
            IntPtr fts = new IntPtr();
            int res = 0;

            fsarr = ConvertStringToByteArray(filename_str);
            ftsarr = ConvertStringToByteArray (filetype_str);

            unsafe
            {
                if (filename_str != "")
                {
                    fixed (byte* fsp = &fsarr[0])
                    {
                        fs = (IntPtr)fsp;
                    }
                }
                if (filetype_str != "")
                {
                    fixed (byte* ftsp = &ftsarr[0])
                    {
                        fts = (IntPtr)ftsp;
                    }
                }
            }

            res = CPXreadcopyprob(env, lp, fs, fts);

            return res;
        }

        private int W_CPXwriteprob(IntPtr env, IntPtr lp, string filename_str,
                                   string filetype_str)
        {
            byte[] fsarr;
            byte[] ftsarr;
            IntPtr fs = new IntPtr();
            IntPtr fts = new IntPtr();
            int res = 0;

            fsarr = ConvertStringToByteArray(filename_str);
            ftsarr = ConvertStringToByteArray(filetype_str);

            unsafe
            {
                if (filename_str != "")
                {
                    fixed (byte* fsp = &fsarr[0])
                    {
                        fs = (IntPtr)fsp;
                    }
                }
                if (filetype_str != "")
                {
                    fixed (byte* ftsp = &ftsarr[0])
                    {
                        fts = (IntPtr)ftsp;
                    }
                }
            }

            res = CPXwriteprob(env, lp, fs, fts);

            return res;
        }

        private int W_CPXaddrows (IntPtr env, IntPtr lp, int ccnt, int rcnt,
                                 int nzcnt, double [] rhs, string sense,
                                 int [] rmatbeg, int [] rmatind, double [] rmatval, 
                                 string [] colname, string [] rowname)
        {
            byte[,] Cols;
            byte[,] Rows;
            byte[] rowt;
            IntPtr cn = new IntPtr();
            IntPtr rn = new IntPtr();
            IntPtr rhsv = new IntPtr();
            IntPtr mb = new IntPtr();
            IntPtr mi = new IntPtr();
            IntPtr mv = new IntPtr();
            IntPtr rt = new IntPtr();
            int res = 0, i, colmax = -1, rowmax = -1;

            rowt = ConvertStringToByteArray(sense);

            for (i = 0; i < colname.Length; i++)
            {
                if (colname[i].Length > colmax)
                    colmax = colname[i].Length;
            }

            for (i = 0; i < rowname.Length; i++)
            {
                if (rowname[i].Length > rowmax)
                    rowmax = rowname[i].Length;
            }

            Cols = ConvertStringArrayToByte2DArray(colname, colmax);
            Rows = ConvertStringArrayToByte2DArray(rowname, rowmax);

            unsafe
            {
                if (rhs != null)
                {
                    fixed (double* rhsvp = &rhs[0])
                    {
                        rhsv = (IntPtr)rhsvp;
                    }
                }

                if (rmatbeg != null)
                {
                    fixed (int* mbp = &rmatbeg[0])
                    {
                        mb = (IntPtr)mbp;
                    }
                }

                if (rmatind != null)
                {
                    fixed (int* mip = &rmatind[0])
                    {
                        mi = (IntPtr)mip;
                    }
                }

                if (rmatval != null)
                {
                    fixed (double* mvp = &rmatval[0])
                    {
                        mv = (IntPtr)mvp;
                    }
                }

                if (sense != "")
                {
                    fixed (byte* rtp = &rowt[0])
                    {
                        rt = (IntPtr)rtp;
                    }
                }

                if (colname != null)
                {
                    fixed (byte* c = &Cols[0, 0])
                    {
                        byte** vet = (byte**)Marshal.AllocHGlobal(sizeof(byte*) * colname.Length);

                        for (i = 0; i < colname.Length; i++)
                        {
                            vet[i] = &c[i * (colmax + 1)];
                        }
                        cn = (IntPtr)vet;
                    }
                }
                if (rowname != null)
                {
                    fixed (byte* r = &Rows[0, 0])
                    {
                        byte** vet2 = (byte**)Marshal.AllocHGlobal(sizeof(byte*) * rowname.Length);

                        for (i = 0; i < rowname.Length; i++)
                        {
                            vet2[i] = &r[i * (rowmax + 1)];
                        }
                        rn = (IntPtr)vet2;
                    }
                }
            }
            res = CPXaddrows(env, lp, ccnt, rcnt, nzcnt, rhsv, rt, mb, mi, mv, cn, rn);

            Marshal.FreeHGlobal(cn);
            Marshal.FreeHGlobal(rn);

            return res;
        }

        private int W_CPXaddcols(IntPtr env, IntPtr lp, int ccnt, int nzcnt,
                                      double [] obj, int [] cmatbeg, int [] cmatind,
                                      double [] cmatval, double [] lb, double [] ub,
                                      string [] colname)
        {
            byte[,] Cols;
            IntPtr cn = new IntPtr();
            IntPtr ob = new IntPtr();
            IntPtr ilb = new IntPtr();
            IntPtr iub = new IntPtr();
            IntPtr mb = new IntPtr();
            IntPtr mi = new IntPtr();
            IntPtr mv = new IntPtr();
            int res = 0, i, colmax = -1;

            for (i = 0; i < colname.Length; i++)
            {
                if (colname[i].Length > colmax)
                    colmax = colname[i].Length;
            }

            Cols = ConvertStringArrayToByte2DArray(colname, colmax);

            unsafe
            {
                if (obj != null)
                {
                    fixed (double* obp = &obj[0])
                    {
                        ob = (IntPtr)obp;
                    }
                }

                if (lb != null)
                {
                    fixed (double* lbp = &lb[0])
                    {
                        ilb = (IntPtr)lbp;
                    }
                }

                if (ub != null)
                {
                    fixed (double* ubp = &ub[0])
                    {
                        iub = (IntPtr)ubp;
                    }
                }

                if (cmatbeg != null)
                {
                    fixed (int* mbp = &cmatbeg[0])
                    {
                        mb = (IntPtr)mbp;
                    }
                }

                if (cmatind != null)
                {
                    fixed (int* mip = &cmatind[0])
                    {
                        mi = (IntPtr)mip;
                    }
                }

                if (cmatval != null)
                {
                    fixed (double* mvp = &cmatval[0])
                    {
                        mv = (IntPtr)mvp;
                    }
                }

                if (colname != null)
                {
                    fixed (byte* c = &Cols[0, 0])
                    {
                        byte** vet = (byte**)Marshal.AllocHGlobal(sizeof(byte*) * colname.Length);

                        for (i = 0; i < colname.Length; i++)
                        {
                            vet[i] = &c[i * (colmax + 1)];
                        }
                        cn = (IntPtr)vet;
                    }
                }
            }
            res = CPXaddcols(env, lp, ccnt, nzcnt, ob, mb, mi, mv, ilb, iub, cn);

            Marshal.FreeHGlobal(cn);

            return res;
        }

        private int W_CPXdelrows(IntPtr env, IntPtr lp, int begin, int end)
        {
            int res = 0;

            res = CPXdelrows(env, lp, begin, end);

            return res;
        }

        private int W_CPXdelcols(IntPtr env, IntPtr lp, int begin, int end)
        {
            int res = 0;

            res = CPXdelcols(env, lp, begin, end);

            return res;
        }

        /***********************************    Metodi pubblici del Wrapper   **************************************/
        public Wrapper()
        {
            CPLEXenvironment = new IntPtr();
            CoinMP = false;
            CPLEX = true;
        }

        public bool getCoinMPSolver()
        {
            return CoinMP;
        }

        public bool getCPLEXSolver()
        {
            return CPLEX;
        }

        //Modificato perchè gestione dll esterne non concettualmente corretta.
        //significa che se la si utilizza in modo diverso dalla semplice "WebForm" si hanno problemi (si possono avere).
        public int initSolver(bool solver)
        {
            double ver;
            //string path;
            int res = 0;

            if (!solver)
            {
                //Commentato perchè il load library cerca in directory più ampie dellla sola corrente...
                //path = "CoinMP.dll";
                //è di concetto sbagliato fare il file.Exists solo in un percorso. le dll windows le cerca 
                //in + percorsi.
                //while (!File.Exists(path))
                //    ManageDllNotFound(ref path, "CoinMP.dll");

                //IntPtr resptr = LoadLibrary(path);

                res = W_CoinInitSolver("");
                ver = W_CoinGetVersion();
                if (ver < 1.4)
                {
                    MessageBox.Show("Si sta tentando di caricare una versione di CoinMP precedente alla 1.4!", "Errore!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process myProcess = Process.GetCurrentProcess();
                    myProcess.Kill();
                }
            }
            // è da modificare anche qui sotto.... analogo a sopra.
            else
            {
                //if (IntPtr.Size == 4)
                //    path = @"C:\ILOG\CPLEX101\bin\x86_win32\cplex10.dll";
                //else
                //{
                //    path = @"C:\ILOG\CPLEX101\bin\x64_win64\cplex10.dll";
                //}

                //while (!File.Exists(path))
                //    ManageDllNotFound(ref path, "cplex101.dll");

                
                //LoadLibrary(path);

                CPLEXenvironment = W_CPXopenCPLEX(out res);
            }

            return res;
        }

        public int closeSolver(bool solver)
        {
            int res = 0;

            if (!solver)
                res = W_CoinFreeSolver();
            else
                res = W_CPXcloseCPLEX(ref CPLEXenvironment);

            return res;
        }

        public string getSolverName (bool solver)
        {
            string sName = "";

            if (!solver)
                sName = W_CoinGetSolverName();
            else
                sName = "CPLEX";

            return sName;
        }

        public string getVersion(bool solver)
        {
            string sVersion = "";

            if (!solver)
                sVersion = W_CoinGetVersionStr();
            else
                sVersion = W_CPXversion(CPLEXenvironment);

            return sVersion;
        }

        public WrapProblem createProblem(string problemName, bool solver)
        {
            IntPtr prob = new IntPtr();
            WrapProblem problem;

            if (!solver)
                prob = W_CoinCreateProblem(problemName);
            else
                prob = W_CPXcreateprob(CPLEXenvironment, out status, problemName);

            problem = new WrapProblem(prob, solver);

            return problem;
        }

        public void loadProblem(WrapProblem problem, int colCount, int rowCount, int nzCount, int rangeCount, int objSense,
                                 double objConst, double[] objCoeffs, double[] lowerBounds, double[] upperBounds,
                                 string rowType, double[] rhsValues, double[] rangeValues, int[] matrixBegin,
                                 int[] matrixCount, int[] matrixIndex, double[] matrixValues, string[] colNames,
                                 string[] rowNames, string objName)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
            {
                W_CoinLoadProblem(prob, colCount, rowCount, nzCount, rangeCount, objSense, objConst, objCoeffs,
                                          lowerBounds, upperBounds, rowType, rhsValues, rangeValues, matrixBegin,
                                          matrixCount, matrixIndex, matrixValues, colNames, rowNames, objName);

                status = W_CoinCheckProblem(prob);
            }
            else
            {
                if (colNames == null && rowNames == null)
                    status = W_CPXcopylp(CPLEXenvironment, prob, colCount, rowCount, objSense, objCoeffs, rhsValues,
                                        rowType, matrixBegin, matrixCount, matrixIndex, matrixValues, lowerBounds,
                                        upperBounds, rangeValues);
                else
                    status = W_CPXcopylpwnames(CPLEXenvironment, prob, colCount, rowCount, objSense, objCoeffs, rhsValues,
                                              rowType, matrixBegin, matrixCount, matrixIndex, matrixValues, lowerBounds,
                                              upperBounds, rangeValues, colNames, rowNames);

                if (objName != "")
                    status = W_CPXcopyobjname(CPLEXenvironment, prob, objName);
            }
        }

        public void loadInteger(WrapProblem problem, string colType)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinLoadInteger(prob, colType);
            else
                status = W_CPXcopyctype(CPLEXenvironment, prob, colType);
        }

        public void loadPriority(WrapProblem problem, int priorCount, int[] priorIndex, int[] priorValues,
                                  int[] branchDirection)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinLoadPriority(prob, priorCount, priorIndex, priorValues, branchDirection);
            else
                status = W_CPXcopyorder(CPLEXenvironment, prob, priorCount, priorIndex, priorValues, branchDirection);
        }

        public void loadSos(WrapProblem problem, int sosCount, int sosNzCount, int[] coinSosType, int[] sosPrior,
                             int[] sosBegin, int[] sosIndex, double[] sosRef, string cplexSosType, string[] sosNames)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinLoadSos(prob, sosCount, sosNzCount, coinSosType, sosPrior, sosBegin, sosIndex, sosRef);
            else
                status = W_CPXcopysos(CPLEXenvironment, prob, sosCount, sosNzCount, cplexSosType, sosBegin, sosIndex, sosRef,
                                     sosNames);
        }

        public void loadQuadratic(WrapProblem problem, int[] quadBegin, int[] quadCount, int[] quadIndex,
                                   double[] quadValues)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinLoadQuadratic(prob, quadBegin, quadCount, quadIndex, quadValues);
            else
                status = W_CPXcopyquad(CPLEXenvironment, prob, quadBegin, quadCount, quadIndex, quadValues);
        }

        public void unloadProblem(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinUnloadProblem(prob);
            else
                status = W_CPXfreeprob(CPLEXenvironment, ref prob);
        }

        public string getProblemName(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            int surp;
            string pName="";

            prob = problem.getProblem();

            if (!problem.getSolver())
                pName = W_CoinGetProblemName(prob);
            else
                status = W_CPXgetprobname(CPLEXenvironment, prob, out pName, 510, out surp);

            return pName;
        }

        public int getColCount(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            int ncol = 0;

            prob = problem.getProblem();

            if (!problem.getSolver())
                ncol = W_CoinGetColCount(prob);
            else
                ncol = W_CPXgetnumcols(CPLEXenvironment, prob);

            return ncol;
        }

        public int getRowCount(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            int nrow = 0;

            prob = problem.getProblem();

            if (!problem.getSolver())
                nrow = W_CoinGetRowCount(prob);
            else
                nrow = W_CPXgetnumrows(CPLEXenvironment, prob);

            return nrow;
        }

        public string getColName(WrapProblem problem, int colIndex)
        {
            IntPtr prob = new IntPtr();
            int surp;
            string cName = "";
            string[] cNames = null;

            prob = problem.getProblem();

            if (!problem.getSolver())
                cName = W_CoinGetColName(prob, colIndex);
            else
                status = W_CPXgetcolname(CPLEXenvironment, prob, out cNames, out cName, 1000, out surp, colIndex, colIndex);

            return cName;
        }

        public string getRowName(WrapProblem problem, int rowIndex)
        {
            IntPtr prob = new IntPtr();
            int surp;
            string rName = "";
            string[] rNames;

            prob = problem.getProblem();

            if (!problem.getSolver())
                rName = W_CoinGetRowName(prob, rowIndex);
            else
                status = W_CPXgetrowname(CPLEXenvironment, prob, out rNames, out rName, 510, out surp, rowIndex, rowIndex);

            return rName;
        }

        public void optimizeLpProblem(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinOptimizeProblem(prob, 0);
            else
                status = W_CPXlpopt(CPLEXenvironment, prob);
        }

        public void optimizeMipProblem(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinOptimizeProblem(prob, 0);
            else
                status = W_CPXmipopt(CPLEXenvironment, prob);
        }

        public int getSolutionStatus(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinGetSolutionStatus(prob);
            else
                status = W_CPXgetstat(CPLEXenvironment, prob);

            return status;
        }

        public string getSolutionText(WrapProblem problem, int solutionStatus)
        {
            IntPtr prob = new IntPtr();
            string solstat = "";

            prob = problem.getProblem();

            if (!problem.getSolver())
                solstat = W_CoinGetSolutionText(prob, solutionStatus);
            else
                W_CPXgetstatstring(CPLEXenvironment, solutionStatus, out solstat);

            return solstat;
        }

        public double getObjectValue(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            double obj = 0.0;

            prob = problem.getProblem();

            if (!problem.getSolver())
                obj = W_CoinGetObjectValue(prob);
            else
                status = W_CPXgetobjval(CPLEXenvironment, prob, out obj);

            return obj;
        }

        public double getMipBestBound(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            double bound = 0.0;

            prob = problem.getProblem();

            if (!problem.getSolver())
                bound = W_CoinGetMipBestBound(prob);
            else
                status = W_CPXgetbestobjval(CPLEXenvironment, prob, out bound);

            return bound;
        }

        public int getIterCount(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            int itcnt = 0;

            prob = problem.getProblem();

            if (!problem.getSolver())
                itcnt = W_CoinGetIterCount(prob);
            else
                itcnt = W_CPXgetitcnt(CPLEXenvironment, prob);

            return itcnt;
        }

        public int getMipNodeCount(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            int nodecnt = 0;

            prob = problem.getProblem();

            if (!problem.getSolver())
                nodecnt = W_CoinGetMipNodeCount(prob);
            else
                nodecnt = W_CPXgetnodecnt(CPLEXenvironment, prob);

            return nodecnt;
        }

        public void getSolutionValues(WrapProblem problem, double[] activity, double[] reducedCost, double[] slackValues,
                                       double[] shadowPrice)
        {
            IntPtr prob = new IntPtr();
            int lpstat;
            double obj;

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinGetSolutionValues(prob, activity, reducedCost, slackValues, shadowPrice);
            else
                status = W_CPXsolution(CPLEXenvironment, prob, out lpstat, out obj, activity, reducedCost, slackValues, shadowPrice);
        }

        public int getSolverStatus(WrapProblem problem)
        {
            return status;
        }

        public void getSolutionBasis(WrapProblem problem, int[] colStatus, double[] rowStatus)
        {
            IntPtr prob = new IntPtr();
            int[] rintstat = new int[rowStatus.Length];
            int i;

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinGetSolutionBasis(prob, colStatus, rowStatus);
            else
            {
                status = W_CPXgetbase(CPLEXenvironment, prob, colStatus, rintstat);
                unsafe
                {
                    //Conversione da int a double (In CPLEX rowStatus è di tipo int)
                    fixed (double* rp = &rowStatus[0])
                    {
                        for (i = 0; i < rowStatus.Length; i++)
                            rp[i] = rintstat[i];
                    }
                }
            }
        }

        public void readFile(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            OpenFileDialog ofp = new OpenFileDialog();
            string ext = "";

            prob = problem.getProblem();

            if (!problem.getSolver())
                ofp.Filter = "MPS files (*.mps)|*.mps";
            else
                ofp.Filter = "MPS format (*.mps)|*.mps|Binary matrix and basis file (*.sav)|*.sav|" +
                             "CPLEX LP format with names modified(*.lp)|*.lp|" +
                             "MPS format, with all names changed(*.rew)|*.rew|" +
                             "MPS format, with all names changed(*.rmp)|*.rmp|" +
                             "LP format, with all names changed(*.rlp)|*.rlp";

            if (ofp.ShowDialog() == DialogResult.OK)
            {
                if (!problem.getSolver())
                {
                    ext = ofp.FileName.Substring(ofp.FileName.Length - 4);
                    if (ext != ".mps")
                        ofp.FileName += ".mps";
                    status = W_CoinReadFile(prob, 3, ofp.FileName);
                }
                else
                {
                    ext = ofp.FileName.Substring(ofp.FileName.Length - 4);
                    if (ext == ".sav")
                        status = W_CPXreadcopyprob(CPLEXenvironment, prob, ofp.FileName, "SAV");
                    if (ext == ".mps")
                        status = W_CPXreadcopyprob(CPLEXenvironment, prob, ofp.FileName, "");
                    if (ext == ".rew" || ext == ".rmp")
                        status = W_CPXwriteprob(CPLEXenvironment, prob, ofp.FileName, "MPS");
                    if (ext == ".lp" || ext == ".rlp")
                        status = W_CPXwriteprob(CPLEXenvironment, prob, ofp.FileName, "LP");
                }
            }
            else
                status = -1;
        }

        public void writeFile(WrapProblem problem)
        {
            IntPtr prob = new IntPtr();
            SaveFileDialog sfp = new SaveFileDialog();
            string ext="";

            prob = problem.getProblem();
            sfp.RestoreDirectory = true;
            sfp.OverwritePrompt = true;

            if (!problem.getSolver())
                sfp.Filter = "MPS files (*.mps)|*.mps";
            else
                sfp.Filter = "MPS format (*.mps)|*.mps|Binary matrix and basis file (*.sav)|*.sav|" +
                             "CPLEX LP format with names modified(*.lp)|*.lp|"+
                             "MPS format, with all names changed(*.rew)|*.rew|"+
                             "MPS format, with all names changed(*.rmp)|*.rmp|"+
                             "LP format, with all names changed(*.rlp)|*.rlp";

            if (sfp.ShowDialog() == DialogResult.OK)
            {
                if (!problem.getSolver())
                {
                    ext = sfp.FileName.Substring(sfp.FileName.Length - 4);
                    if (ext != ".mps")
                        sfp.FileName += ".mps";
                    status = W_CoinWriteFile(prob, 3, sfp.FileName);
                }
                else
                {
                    ext = sfp.FileName.Substring(sfp.FileName.Length - 4);
                    if (ext == ".sav")
                        status = W_CPXwriteprob(CPLEXenvironment, prob, sfp.FileName, "SAV");
                    if (ext == ".mps")
                        status = W_CPXwriteprob(CPLEXenvironment, prob, sfp.FileName, "MPS");
                    if (ext == ".lp")
                        status = W_CPXwriteprob(CPLEXenvironment, prob, sfp.FileName, "LP");
                    if (ext == ".rew")
                        status = W_CPXwriteprob(CPLEXenvironment, prob, sfp.FileName, "REW");
                    if (ext == ".rmp")
                        status = W_CPXwriteprob(CPLEXenvironment, prob, sfp.FileName, "RMP");
                    if (ext == ".rlp")
                        status = W_CPXwriteprob(CPLEXenvironment, prob, sfp.FileName, "RLP");
                }
            }
            else
                status = -1;
        }

        public string getParamName(WrapProblem problem, int paramID)
        {
            IntPtr prob = new IntPtr();
            string pName = "";
            string spname = "";
            int v1,v2,v3;

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinGetOptionInfo(prob, paramID, out v1, out v2, out v3, out pName, out spname, 100);
            else
                status = W_CPXgetparamname(CPLEXenvironment, paramID, out pName);

            return pName;
        }

        public void getIntParamMinMax(WrapProblem problem, int paramID, out int minValue, out int maxValue)
        {
            IntPtr prob = new IntPtr();
            int def;

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinGetIntOptionMinMax(prob, paramID, out minValue, out maxValue);
            else
                status = W_CPXinfointparam(CPLEXenvironment, paramID, out def, out minValue, out maxValue);
        }

        public void getRealParamMinMax(WrapProblem problem, int paramID, out double minValue, out double maxValue)
        {
            IntPtr prob = new IntPtr();
            double def;

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinGetRealOptionMinMax(prob, paramID, out minValue, out maxValue);
            else
                status = W_CPXinfodblparam(CPLEXenvironment, paramID, out def, out minValue, out maxValue);
        }

        public int getIntParam(WrapProblem problem, int paramID)
        {
            IntPtr prob = new IntPtr();
            int param;

            prob = problem.getProblem();

            if (!problem.getSolver())
                param = W_CoinGetIntOption(prob, paramID);
            else
                status = W_CPXgetintparam(CPLEXenvironment, paramID, out param);

            return param;
        }

        public void setIntParam(WrapProblem problem, int paramID, int intValue)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinSetIntOption(prob, paramID, intValue);
            else
                status = W_CPXsetintparam(CPLEXenvironment, paramID, intValue);
        }

        public double getRealParam(WrapProblem problem, int paramID)
        {
            IntPtr prob = new IntPtr();
            double param;

            prob = problem.getProblem();

            if (!problem.getSolver())
                param = W_CoinGetRealOption(prob, paramID);
            else
                status = W_CPXgetdblparam(CPLEXenvironment, paramID, out param);

            return param;
        }

        public void setRealParam(WrapProblem problem, int paramID, double realValue)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinSetRealOption(prob, paramID, realValue);
            else
                status = W_CPXsetdblparam(CPLEXenvironment, paramID, realValue);
        }

        public string getStringParam(WrapProblem problem, int paramID)
        {
            IntPtr prob = new IntPtr();
            string param;

            prob = problem.getProblem();

            if (!problem.getSolver())
                param = W_CoinGetStringOption(prob, paramID);
            else
                status = W_CPXgetstrparam(CPLEXenvironment, paramID, out param);

            return param;
        }

        public void setStringParam(WrapProblem problem, int paramID, string strValue)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinSetStringOption(prob, paramID, strValue);
            else
                status = W_CPXsetstrparam(CPLEXenvironment, paramID, strValue);
        }

        public void addrows(WrapProblem problem, int ccnt, int rcnt,
                            int nzcnt, double[] rhs, string sense,
                            int[] rmatbeg, int[] rmatind, double[] rmatval,
                            string[] colname, string[] rowname)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = W_CoinAddRows(prob, rcnt, rmatbeg, rmatind, rmatval, rhs, null, sense);
            else
                status = W_CPXaddrows(CPLEXenvironment, prob, ccnt, rcnt, nzcnt, rhs, sense, rmatbeg, rmatind,
                                       rmatval, colname, rowname);
        }

        public void addrow(WrapProblem problem, double rhsvalue, double[] values, string sense, string rowname)
        {
            if (!problem.getSolver())
            {
                int nnzero = 0;
                for (int i = 0; i < values.Length; i++)
                    if (values[i] != 0.0)
                        nnzero++;
                int[] matbeg = new int[2] { 0, nnzero };
                int[] matind = new int[nnzero];
                double[] matval = new double[nnzero];
                int i2=0;
                for (int i = 0; i < values.Length; i++)
                    if (values[i] != 0.0)
                    {
                        matind[i2] = i;
                        matval[i2++] = values[i];
                    }

                addrows(problem, -1, 1, -1, new double[] { rhsvalue }, sense, matbeg, matind, matval, null, (rowname == null ? null : new string[] { rowname }));
            }
            else
                status = -1;
        }

        public void addcols(WrapProblem problem, int ccnt, int nzcnt,
                           double[] obj, int[] cmatbeg, int[] cmatind,
                           double[] cmatval, double[] lb, double[] ub,
                           string[] colname)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = -1;
            else
                status = W_CPXaddcols(CPLEXenvironment, prob, ccnt, nzcnt, obj, cmatbeg, cmatind, cmatval, lb, ub,
                                       colname);
        }

        public void delrows(WrapProblem problem, int begin, int end)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = -1;
            else
                status = W_CPXdelrows(CPLEXenvironment, prob, begin, end);
        }

        public void delcols(WrapProblem problem, int begin, int end)
        {
            IntPtr prob = new IntPtr();

            prob = problem.getProblem();

            if (!problem.getSolver())
                status = -1;
            else
                status = W_CPXdelcols(CPLEXenvironment, prob, begin, end);
        }
    }
}
