using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using PMedLib;
using System.IO;
using System.IO.IsolatedStorage;

namespace PMedLibWebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PMedLibWebService : System.Web.Services.WebService
    {
        //se non va da riga di comando: 
        //aspnet_regiis.exe -iru
        //inoltre digitare se si è su x64 e si compila x86:
        //%windir%\system32\inetsrv\appcmd set config -section:applicationPools -applicationPoolDefaults.enable32BitAppOnWin64:true

        // Todo ....
        private string CurrentDir;
        //private bool UseDefault = true;
        private string ProblemDir;
        private string LogDir;
        //private const string DefaultProblemDir = @"D:\Uni\AAA\Proj_Exam\Problems";
        private const string DefaultProblemDir = @"Problems";
        private const string LogFileName = @"PMedLibWebServer.log";
        //private const string DllDir = @"D:\Uni\GIS\Exam\ASP.NET\PMedLibWcfService\PMedLibWcfService\";
        //private string LogPath;
        private const string ConfigFileName = @"PMedLibWeb.cfg";

        private String[] ListOfProblem;
        private PMed3 pm;
        private bool pm_inited = false;
        private int M;
        private int m;
        private int selectfile_index;
        private FileStream isfs;
        //private IsolatedStorageFileStream isfs;
        private StreamWriter sw;

        private bool isloaded = false;
        
        private bool parsol = false;
        private bool parclu = false;

        [WebMethod]
        public string[] GetListOfCapFile()
        {
            String[] ret = null;
            try
            {
                InitService();
                ListOfProblem = Directory.GetFiles(ProblemDir, "pmedcap*.txt", SearchOption.TopDirectoryOnly);
                ret = ListOfProblem.Clone() as String[];
                //tengo solo il nomefile senza percorso...
                for (int i = 0; i < ListOfProblem.Length; i++)
                    CutPath(ref ret[i]);

            }
            catch (Exception e)
            {
                ListOfProblem = new String[1];
                ListOfProblem[0] = e.Message;
                ret = ListOfProblem;
            }
            finally
            {
                DeInitService();
            }

            return ret;
        }

        private bool ReadConfigFile(String AppDir)
        {
            ProblemDir = LogDir = "";
            LogDir = "";
            parsol = parclu = false;

            if (!File.Exists(AppDir + ConfigFileName))
                return false;

            bool ret = false;
            string line;
            const string ProblemOpt = @"PROBLEM=";
            const string LogOpt = @"LOG=";
            const string ParallelOpt = @"PARALLEL=";
            const string ParClustOpt = @"PARCLUST=";


            FileStream fs = new FileStream(AppDir + ConfigFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader sr = new StreamReader(fs);

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();

                if (line.Length == 0)
                    continue;
                if (line[0] == ';')
                    continue;

                line = line.ToUpper();
                if (line.StartsWith(ProblemOpt))
                {
                    string path = line.Substring(ProblemOpt.Length);
                    if (Directory.Exists(AppDir + path))
                    {
                        //UseDefault = false;
                        ProblemDir = AppDir + path;
                        ret = true;
                        //return true;
                    }
                    //else
                    //    return false;
                    //UseDefault = true;
                }

                else if (line.StartsWith(LogOpt))
                {
                    string path = line.Substring(LogOpt.Length);
                    if (Directory.Exists(AppDir + path))
                    {
                        //LogDir = AppDir + path + "\\" + LogFileName;
                        LogDir = Path.Combine(AppDir, path);
                        LogDir = Path.Combine(LogDir,LogFileName);
                        ret = true;
                    }
                }
                else if (line.StartsWith(ParallelOpt))
                {
                    string value = line.Substring(ParallelOpt.Length);
                    if (!Boolean.TryParse(value, out parsol))
                        parsol = false;
                }
                else if (line.StartsWith(ParClustOpt))
                {
                    string value = line.Substring(ParClustOpt.Length);
                    if (!Boolean.TryParse(value, out parclu))
                        parclu = false;
                }
            }

            sr.Close();
            fs.Close();

            return ret;

        }

        private void InitService()
        {
            CurrentDir = Directory.GetCurrentDirectory();
            String AppDir = AppDomain.CurrentDomain.BaseDirectory;
            selectfile_index = -1;
            M = 0;

            Exception _e = null;
 
            ReadConfigFile(AppDir);
           
            if (ProblemDir.Length == 0)
                ProblemDir = AppDir + DefaultProblemDir;

            if (LogDir.Length == 0)
                LogDir = AppDir + LogFileName;

            //un log al giorno
            LogDir += "_"+DateTime.Today.Year+DateTime.Today.Month+DateTime.Today.Day;
            //un log al mese
            //LogDir += "_" + DateTime.Today.Year + DateTime.Today.Month;

            try
            {
                isfs = new FileStream(LogDir, FileMode.Append);
            }
            catch (Exception e)
            {
                _e = e;
                isfs = new IsolatedStorageFileStream(LogFileName, FileMode.Append);
            }

            sw = new StreamWriter(isfs);
            sw.AutoFlush = true;
            sw.WriteLine("\r\nP - Median Library Web Services LOG file... DATE : {0} - TIME {1}", DateTime.Now.Date, DateTime.Now.TimeOfDay);
            if (_e != null)
                sw.WriteLine("r\nException ERRRO : {0}", _e.Message);

            try
            {
                pm = new PMed3(sw.Write, 1, "PMedWebSerProb");
                pm_inited = true;
            }
            catch (Exception e)
            {
                //...
                pm_inited = false;
                sw.WriteLine("Errore PMedLib!");
                sw.WriteLine("{0}", e.Message);
            }
        }

        #region Old Init Service
        //private void InitService()
        //{
        //    CurrentDir = Directory.GetCurrentDirectory();

        //    String AppDir = AppDomain.CurrentDomain.BaseDirectory;

        //    selectfile_index = -1;
        //    M = 0;
        //    try
        //    {
        //        Exception _e = null;

        //        if (!ReadConfigFile(AppDir))
        //        {
        //            if (ProblemDir.Length == 0)
        //                ProblemDir = AppDir + DefaultProblemDir;
        //        }

        //        if (LogDir.Length == 0)
        //            isfs = new IsolatedStorageFileStream(LogFileName, FileMode.Append);
        //        else
        //        {
        //            try
        //            {
        //                //FileIOPermission fperm = new FileIOPermission(FileIOPermissionAccess.Write, LogDir);
        //                //if(fperm.IsUnrestricted())

        //                if (!File.Exists(LogDir))
        //                    isfs = new FileStream(LogDir, FileMode.Create);
        //                else
        //                    isfs = new FileStream(LogDir, FileMode.Append);
        //            }
        //            catch (Exception e)
        //            {
        //                _e = e;
        //                isfs = new IsolatedStorageFileStream(LogFileName, FileMode.Append);
        //            }
        //        }

        //        sw = new StreamWriter(isfs);
        //        sw.AutoFlush = true;
        //        sw.WriteLine("\r\nP - Median Library Web Services LOG file... DATE : {0} - TIME {1}", DateTime.Now.Date, DateTime.Now.TimeOfDay);
        //        if (LogDir.Length == 0)
        //            sw.WriteLine("\r\n\r\n Error directory for logs doesn't exist!!!");
        //        if (_e != null)
        //            sw.WriteLine("r\nException ERRRO : {0}", _e.Message);

        //        try
        //        {
        //            //codice per fartrovare la dll ....
        //            //var path = Environment.GetEnvironmentVariables();
        //            //System.Environment.SetEnvironmentVariable("Path", path["Path"] + ";" + DllDir);
        //            pm = new PMed3(sw.Write, 1, "PMedWebSerProb");
        //            pm_inited = true;
        //        }
        //        catch (Exception e)
        //        {
        //            //...
        //            pm_inited = false;
        //            sw.WriteLine("Errore PMedLib!");
        //            sw.WriteLine("{0}", e.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //....
        //    }

        //}
        #endregion

        private void DeInitService()
        {
            pm = null;
            pm_inited = false;
            if (sw != null)
                sw.Close();
            if (isfs != null)
                isfs.Close();
        }

        private void CutPath(ref String str)
        {
            int index = str.LastIndexOf("\\");
            if (index == -1)
                return;
            str = str.Remove(0, index + 1);

        }

        private bool CheckSelectedFile()
        {
            if ((ListOfProblem != null) &&
                (selectfile_index >= 0) &&
                (selectfile_index < ListOfProblem.Length))
                return true;
            else
                return false;
        }

        private int SelectFile(int index)
        {
            //if (!pm_inited)
            //    InitService();

            if (!pm_inited)
                return int.MinValue;

            isloaded = false;
            selectfile_index = index;
            if (CheckSelectedFile())
            {
                if (!pm.Load(ListOfProblem[index], ref M))
                    return int.MinValue;
                if (M == -1)
                    isloaded = true;
                return M;
            }
            else
            {
                selectfile_index = -1;
                return int.MinValue;
            }
        }

        private bool GetCoord(ref int[] x, ref int[] y)
        {
            bool ret = false;
            if ((!isloaded) && (M == -1))
                return false;

            Point p = new Point();
            ret = pm.Load(ListOfProblem[selectfile_index], ref m, ref p);
            if (ret)
                isloaded = true;
            else
                isloaded = false;
            x = p.x;
            y = p.y;

            return ret;
        }

        private bool GetSolution(int n, ref uint WrapSol, ref List<uint>[] WrapSolClu)
        {
            bool ret = false;
            WrapSol = 0;
            WrapSolClu = null;

            if (!isloaded)
                return false;
            pm.SetSolutions((uint)n);


            ret = pm.ComputeSolutions(parsol, parclu);
            //tolto il parallelo...
            //ret = pm.ComputeSolutions(false, false);    

            if (ret)
            {
                WrapSol = pm.WrapperSolution;
                WrapSolClu = pm.WrapperSolutionClusters;
            }

            return ret;
        }

        [WebMethod]
        public bool GetSolution(int fileindex, int m, int n, out int[] x, out int[] y, out uint WrapSol, out List<uint>[] WrapSolClu)
        {
            WrapSolClu = null;
            WrapSol = 0;
            x = y = null;

            GetListOfCapFile();
            InitService();
            if (SelectFile(fileindex) == int.MinValue)
            {
                DeInitService();
                return false;
            }

            //if (!SelectCapacitated(m))
            //{
            //    DeInitService();
            //    return false;
            //}
            this.m = m;
            if (!GetCoord(ref x, ref y))
            {
                DeInitService();
                return false;
            }


            //List<uint>[] _WrapSolClu = null;
            if (!GetSolution(n, ref WrapSol, ref WrapSolClu))
            {
                DeInitService();
                return false;
            }

            //uint[][] uWrapSolClu = new uint[_WrapSolClu.Length][];
            //WrapSolClu = new int[_WrapSolClu.Length][];
            //for (int i = 0; i < _WrapSolClu.Length; i++)
            //{
            //    uWrapSolClu[i] = _WrapSolClu[i].ToArray();
            //    WrapSolClu[i] = new int[uWrapSolClu[i].Length];
            //    for (int j = 0; j < uWrapSolClu[i].Length; j++)
            //    {
            //        WrapSolClu[i][j] = (int)uWrapSolClu[i][j];
            //    }
            //}

            DeInitService();
            return true;
        }

 
    }
}