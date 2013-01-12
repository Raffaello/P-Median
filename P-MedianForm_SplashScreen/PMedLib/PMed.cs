//*********************************************************************
//****   P-Median DLL Library                                      ****
//****   Author : Raffaello Bertini (raffaellobertini@gmail.com)   ****
//****   File: PMed.cs  (Main Class)                               ****
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

namespace PMedLib
{
	/// <summary>
	/// Struttura per passare le coordinate al visualizzatore.
	/// </summary>
	public struct Point
	{
		/// <summary>
		/// coordinata x
		/// </summary>
		public int[] x;
		/// <summary>
		///  coordinata y
		/// </summary>
		public int[] y;
	}

	/// <summary>
	/// 
	/// </summary>
	public class PMed
	{
		#region Private Variables
		//utilizzate per WriteDelegate
		private string methodname;
		private string methodbasename;
		private WriteInfoenum _WriteInto;

		private uint[] x;
		private uint[] y;
		#endregion

		#region Protected Variables
		/// <summary>
		/// 
		/// </summary>
		protected WriteDelegate Write;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="str"></param>
		protected void WriteLine(String str)
		{
			Write(str + "\r\n");
		}

		#endregion

		#region Public Variables
		/// <summary>
		/// 
		/// </summary>
		public uint[] DemandQ { get; private set; }

		/// <summary>
		/// capacità di ogni p, se = 0 allora uncapacitated
		/// </summary>
		public uint Q { get; private set; }
		/// <summary>
		/// Per visualizzare i risultati o su console o dove ti pare...
		/// </summary>
		/// <param name="str">la stringa da visualizzare</param>
		public delegate void WriteDelegate(String str);
		/// <summary>
		/// Visualizza maggiori informazioni se richiesto.
		/// </summary>
		public bool Verbose { get; set; }
		/// <summary>
		/// Enum of type of known write mode
		/// </summary>
		public enum WriteInfoenum
		{
			/// <summary>
			/// 
			/// </summary>
			Unknow,
			/// <summary>
			/// 
			/// </summary>
			Console,
			/// <summary>
			/// 
			/// </summary>
			TextBox,
			/// <summary>
			/// 
			/// </summary>
			File  // not implemented yet! ma se si associa un TextWriter.Write ....
		}
		/// <summary>
		/// Enum of the istance of the Write Method.
		/// </summary>
		public WriteInfoenum WriteInto
		{
			get { return _WriteInto; }
			private set { _WriteInto = value; }
		}

		/// <summary>
		/// numero di vertici, nodi.
		/// </summary>
		public uint nVerteces { get; protected set; }
		/// <summary>
		/// numero di archi... quelli rappresentati nel file di lettura
		/// </summary>
		public uint nEdges { get; protected set; }
		/// <summary>
		///  numero dei nodi classificati p 
		/// </summary>
		public uint p { get; protected set; }
		/// <summary>
		/// matrice dei costi
		/// </summary>
		public uint[,] c { get; protected set; }

		#endregion

		#region Private Method

		private void WriteInfo()
		{
			switch (methodbasename)
			{
				case "TextBoxBase": Write("(Writing into a TextBox. Function : " + methodname + ")\r\n");
					break;
				case "Console": Write("(Writing into a ConsoleWindow. Function : " + methodname + ")\r\n");
					break;
				default: Write(@"Don't know where PMedLib is writing. Is it a delegate? Function : " + methodname + ")\r\n");
					break;
			}
		}

		/// <summary>
		/// Alloca le variabili necessarie per caricare e risolvere il prblema p-median
		/// </summary>
		private void AllocVar()
		{
			c = new uint[nVerteces, nVerteces];
			DemandQ = new uint[nVerteces];
			//settato a 0,
			//se uncapacitated allora Q=0, 
			//ma è ok perchè il vincolo diventerebbe 0<=0
			for(int i = 0;i<nVerteces;i++)
				DemandQ[i]=0;
			
		}

		/// <summary>
		/// algoritmo di Floyd Shortest Path, per sistemare la matrice dei costi letta dal file
		/// </summary>
		private void Floyd()
		{
			int i, j, k;
			uint t;

			for (k = 0; k < nVerteces; k++)
			{
				for (j = 0; j < nVerteces; j++)
				{
					for (i = 0; i < nVerteces; i++)
					{
						t = c[j, k] + c[k, i];
						if (c[j, i] > t)
							c[j, i] = t;
					}
				}
			}
		}

		#endregion

		#region Public Method
		/// <summary>
		/// costruttore base
		/// </summary>
		/// <param name="_w">write function</param>
		/// <param name="verbose">verbose?</param>
		public PMed(WriteDelegate _w, bool verbose = true)
		{
			Q = 0; //settato  per uncapacitated
			Write = _w;
			Verbose = verbose;
			//System.Reflection.MethodInfo methodinfo = _w.Method;
			methodname = _w.Method.Name;
			methodbasename = _w.Method.DeclaringType.Name;
#if DEBUG
			if (Verbose)
				WriteInfo();
#endif
			switch (methodbasename)
			{
				case "TextBoxBase": WriteInto = WriteInfoenum.TextBox;
					break;
				case "Console": WriteInto = WriteInfoenum.Console;
					break;
				default: WriteInto = WriteInfoenum.Unknow;
					break;
			};
		}
	  
		
#if DEBUG
		//[Conditional("DEBUG")]
		/// <summary>
		/// carica i file di test costruiti ad hoc per testare l'algoritmo, esiste solo in debug
		/// </summary>
		/// <param name="FileName"></param>
		/// <returns></returns>
		public bool LoadTest(String FileName)
		{
			bool ret = false;

			try
			{
				uint _uint, _uint1, _uint2;
				TextReader tr = File.OpenText(FileName);
				String str_ln = tr.ReadLine();
				String[] str = str_ln.Trim().Split(' ');

				_uint = _uint1 = _uint2 = 0;
				if (str.Length == 2)
				{
					if (!(uint.TryParse(str[0], out _uint1) &&
					(uint.TryParse(str[1], out _uint2)) ))
					{
						tr.Close();
						return false;
					}
				}
				else //errore
				{
					tr.Close();
					return false;
				}

				nVerteces = _uint1;
				nEdges = 0;
				p = _uint2;

				//Alloco le variabili necessarie al fine di risolvere e caricare il problema.
				AllocVar();

				//_uint1 = nVerteces - 1;
				_uint2 = nVerteces;
				int j=0;
				while (tr.Peek() > -1)
				{
					if(j>=nVerteces) 
					{
						Write("ERRROR");
						return false;
					} 
					str_ln = tr.ReadLine();
					str = str_ln.Trim().Split(' ','\t');
					
					for (int i = 0; i < str.Length; i++)
					{
						if (!(uint.TryParse(str[i], out _uint1)))
						{
							Write("errore parser number");
							return false;
						}

						c[j,i]=_uint1;

					}
					j++;
				}
				tr.Close();

				ret = true;

				if (Verbose)
					Write("P-Median Library Project.\r\nVersion 1.00.\r\nBy Raffaello Bertini.\r\n");
				Write(String.Format(@"
VERTICES : {0}
EDGES    : {1}
p        : {2}
", nVerteces, nEdges, p));

			}
			catch (Exception e)
			{
				//global::System.Windows.Forms.MessageBox.Show();
				Write(e.Message + "\r\n");
			}

			return ret;

		}
#endif
		/// <summary>
		/// Carica un file di tipo UnCapacitaded 
		/// </summary>
		/// <param name="FileName"></param>
		/// <returns></returns>
		private bool LoadUnCap(String FileName)
		{
			bool ret = false;

			try
			{
				uint _uint, _uint1, _uint2;
				TextReader tr = File.OpenText(FileName);
				String str_ln = tr.ReadLine();
				String[] str = str_ln.Trim().Split(' ');

				//identifica dalla prima riga se uncap o cap. problem
				//se solo 1 elemento è cap
				//se sono 3 è uncup.
				_uint = _uint1 = _uint2 = 0;
				if (str.Length == 3)
				{
					bool r1, r2, r3;
					r1 = uint.TryParse(str[0], out _uint1);
					r2 = uint.TryParse(str[1], out _uint2);
					r3 = uint.TryParse(str[2], out _uint);

					if ( !r1 && !r2 && !r3)
					{
						tr.Close();
						return false;
					}
				}
				else //errore
				{
					tr.Close();
					return false;
				}

				nVerteces = _uint1;
				nEdges = _uint2;
				p = _uint;

				//Alloco le variabili necessarie al fine di risolvere e caricare il problema.
				AllocVar();

				_uint1 = nVerteces - 1;
				_uint2 = nVerteces;
				for (int j = 0; j < _uint1; j++)
				{
					c[j, j] = 0;
					for (int i = j + 1; i < _uint2; i++)
						c[j, i] = c[i, j] = int.MaxValue / 2;
				}

				while (tr.Peek() > -1)
				{
					str_ln = tr.ReadLine();
					str = str_ln.Trim().Split(' ');

					bool r1, r2, r3;
					r1 = uint.TryParse(str[0], out _uint1);
					r2 = uint.TryParse(str[1], out _uint2);
					r3 = uint.TryParse(str[2], out _uint);
						
					if (!r1 && !r2 && !r3)
					{
						tr.Close();
						return false;
					}
					_uint1--;
					_uint2--;
					c[_uint1, _uint2] = c[_uint2, _uint1] = _uint;
				}

				tr.Close();
				Floyd();

				ret = true;

				if (Verbose)
					Write("P-Median Library Project.\r\nVersion 1.00.\r\nBy Raffaello Bertini.\r\n");
				Write(String.Format(@"
VERTICES : {0}
EDGES    : {1}
p        : {2}
", nVerteces, nEdges, p));

			}
			catch (Exception e)
			{
				//global::System.Windows.Forms.MessageBox.Show();
				Write(e.Message + "\r\n");
			}

			return ret;

		}

		/// <summary>
		///  carica un file di tipo Capacitated
		/// </summary>
		/// <param name="FileName"></param>
		/// <param name="_M"></param>
		/// <param name="return_m"></param>
		/// <returns></returns>
		private bool LoadCap(String FileName, ref int _M,bool return_m = true)
		{
			//bool ret=false;

			int M;
			uint bestsolM=0;
			bool r1, r2, r3, r4;
		   
			try
			{
				uint _uint, _uint1, _uint2;
				TextReader tr = File.OpenText(FileName);
				String str_ln = tr.ReadLine();
				String[] str = str_ln.Trim().Split(' ');

				if (!int.TryParse(str[0], out M))
				{
					tr.Close();
					return false;
				}
				if (return_m)
				{
					_M = M;
					tr.Close();
					return true;
				}

				int m = 0;
				_uint = 0;
				//seek problem...
				//problem number, best solution found
				do
				{
					try
					{
						str = tr.ReadLine().Trim().Split(' ');
					}
					catch (Exception e)
					{
						Write(e.Message);
						tr.Close();
						return false;
					}

					if (str.Length == 2)
					{
						m++;
						r1=uint.TryParse(str[0], out _uint);
						r2=uint.TryParse(str[1], out bestsolM);
						if ((!r1) &&
							(!r2))
						{
							tr.Close();
							return false;
						}
					}
				}
				while ((_uint != _M)&&(m<M));
					
				//number of customers, number of p, capacity of p
				str = tr.ReadLine().Trim().Split(' ');
				_uint=_uint1=_uint2=0;
				r1 = uint.TryParse(str[0], out _uint);
				r2 = uint.TryParse(str[1], out _uint1);
				r3 = uint.TryParse(str[2], out _uint2);
				if((str.Length != 3)&&
					(!r1)&&
					(!r2)&&
					(!r3))
				{
					tr.Close();
					return false;
				}
				nVerteces = _uint;
				p = _uint1;
				Q = _uint2;

				//ultima parte...
				AllocVar();
				uint _uint3=0;
				x = new uint[nVerteces];
				y = new uint[nVerteces];
				//customer number i, x coordinate of i, y coord of i, demand of i
				for (int i = 0; i < nVerteces; i++)
				{
					str = tr.ReadLine().Trim().Split(' ');
					r1 = uint.TryParse(str[0], out _uint);
					r2 = uint.TryParse(str[1], out _uint1);
					r3 = uint.TryParse(str[2], out _uint2);
					r4 = uint.TryParse(str[3], out _uint3);
					if ((str.Length != 4)&&
						(!r1)&&
						(!r2)&&
						(!r3)&&
						(!r4))
					{
						tr.Close();
						return false;
					}

					x[_uint-1] = _uint1;
					y[_uint-1] = _uint2;
					DemandQ[_uint-1] = _uint3;
				}
				//calcolo le distanze...
				for (int j = 0; j < nVerteces; j++)
				{
					c[j, j] = 0;
					for (int i = j + 1; i < nVerteces; i++)
					{
						int deltax = (int)(x[i] - x[j]);
						int deltay = (int)(y[i] - y[j]);
						double sqrt = Math.Sqrt(deltax * deltax +
												deltay * deltay);

						c[i,j] = c[j,i] = (uint)Math.Floor(sqrt);
					}
				}

				tr.Close();
				if (Verbose)
					Write("P-Median Library Project.\r\nVersion 1.00.\r\nBy Raffaello Bertini.\r\n");
				Write(String.Format(@"
Capacitated problem number {0}
Best Solution Reported in file = {1}
VERTICES : {2}
p        : {3}
Q        : {4}
",_M,bestsolM, nVerteces, p,Q));

			}
			catch (Exception e)
			{
				//global::System.Windows.Forms.MessageBox.Show();
				Write(e.Message + "\r\n");
			}

			return true;
		}

		
		/// <summary>
		/// Carica un file di tipo capacitated e restituisce le coordinate del problema.
		/// Da chiamare specificando quale problema caricare all'interno del file.
		/// (per restituire quanti problemi sono interni al file usare l'altro metodo la prima volta,
		/// la seconda utilizzarlo con ref Pont)
		/// </summary>
		/// <param name="FileName"></param>
		/// <param name="m"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool Load(String FileName, ref int m, ref Point point)
		{
			if(m<=0)
				return false;

			bool ret = LoadCap(FileName,ref m, false);
			if(ret)
			{
				point.x = x.Clone() as int[];
				point.y = y.Clone() as int[];
				x = y = null;
			}

			return ret;
		}

		/// <summary>
		/// Carica un file di tipo uncapacitated/capacitated P-Median Problem
		/// </summary>
		/// <param name="FileName"></param>
		/// <param name="m"></param>
		/// <param name="return_m"></param>
		/// <returns></returns>
		public bool Load(String FileName, ref int m,bool return_m=true)
		{
			bool ret = false;

			try
			{
				TextReader tr = File.OpenText(FileName);
				String str_ln = tr.ReadLine();
				tr.Close();
				String[] str = str_ln.Trim().Split(' ');

				//identifica dalla prima riga se uncap o cap. problem
				//se solo 1 elemento è cap
				//se sono 3 è uncup.
				if (str.Length == 3)
				{
					ret = LoadUnCap(FileName);
					m = -1;
				}
				else if (str.Length == 1)
					ret = LoadCap(FileName, ref m,return_m);
			}
			catch (Exception e)
			{
				//global::System.Windows.Forms.MessageBox.Show();
				Write(e.Message + "\r\n");
			}

			//libero la memoria dalle coordinate...
			x = null;
			y = null;
			
			return ret;
		}

		/// <summary>
		/// Stampa la matrice dei costi.
		/// </summary>
		public void ShowCostMatrix()
		{
			uint i;
			uint j;

			if (this.WriteInto == WriteInfoenum.Console)
				Write("\r\n\r\nWARNING!!! : Not sure if the tableau is displayed correctly, because of column limitation due to Console Window limitation!!!\r\n");

			//intestazione colonna
			Write("\r\n     |");

			for (i = 0; i < nVerteces; i++)
			{
				Write(String.Format("n{0,4}|", i));
			}

			Write("\r\n-----");
			for (i = 0; i < nVerteces; i++)
			{
				Write("------");
			}
			Write("\r\n");

			//stampo le righe...
			for (i = 0; i < nVerteces; i++)
			{

				Write(String.Format("{0,5}|", i));
				for (j = 0; j < nVerteces; j++)
				{
					Write(String.Format("{0,5}|", c[i, j]));
				}
				Write("\r\n");
			}

		}
		#endregion

	}
}

		
 
