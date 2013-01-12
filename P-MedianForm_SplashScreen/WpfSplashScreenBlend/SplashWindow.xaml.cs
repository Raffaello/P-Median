using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Timers;

namespace WpfSplashScreenBlend
{
	/// <summary>
	/// Interaction logic for SplashWindow.xaml
	/// </summary>
	public partial class SplashWindow : Window
	{
		private Thread loadingThread;
		private delegate void _AppendText(string txt);
		private delegate void _RemoveFirstLineText();
		_AppendText AppendText;
		_RemoveFirstLineText RemoveFirstLineText;

		private delegate void _ChangeOpacity(double qty);
		_ChangeOpacity ChangeOpacity;
		
		private const ushort max_line=5;
		private ushort line_count;

		private const double Opacity_inc = +0.08;
		private const double Opacity_dec = -0.10;
		private double qty;
		private const int timer_interval = 50;
		private System.Timers.Timer timer1;
		public bool IsFadedOut { get; private set; }
		private bool _FadeOut;
		private bool SplashScreen;
		_isFadedIn f;
		public bool IsSplashScreen { get { return SplashScreen; } private set { value = SplashScreen; } }
		
		public SplashWindow(bool FadeIn, bool FadeOut, bool splash)
		{
			_FadeOut = FadeOut;
			IsFadedOut = false;
			SplashScreen = splash;
			f = new _isFadedIn(isFadedIn);
			line_count=0;
			InitializeComponent();
			AppendText = new _AppendText(this.Append);
			RemoveFirstLineText = new _RemoveFirstLineText(this.Remove);

			if ((FadeIn) || (FadeOut))
			{
				ChangeOpacity = new _ChangeOpacity(this.ChgOp);
				if(FadeIn)
					this.Opacity = 0;
				this.qty = Opacity_inc;
				timer1 = new System.Timers.Timer(timer_interval);
				timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
				timer1.Start();
			}
		}

		private void ChgOp(double qty)
		{
			if (qty > 0)
			{
				if (this.Opacity < 1)
					this.Opacity += qty;
				//else
					//Opacity_inc = 0.01;
			}
			else
			{
				if (this.Opacity > 0)
					this.Opacity += qty;
				else
					this.Close();
			}
			
		}

		void timer1_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.Dispatcher.Invoke(this.ChangeOpacity,this.qty);
			//throw new NotImplementedException();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//faccio partire su un thread separato la visualizzazione...
			loadingThread = new Thread(LoadThread);
			loadingThread.Start();
		}

		private void LoadThread()
		{
			//leggo informazioni di sistema.
			//OperatingSystem os = Environment.OSVersion;
			this.Dispatcher.Invoke(AppendText, "\nPlatform : "+ Environment.OSVersion.Platform.ToString());
			//Thread.Sleep(300);
			this.Dispatcher.Invoke(AppendText, "\nService Pack :" + Environment.OSVersion.ServicePack.ToString());
			//this.Dispatcher.Invoke(AppendText,"\nProva2");
			//Thread.Sleep(300);
			this.Dispatcher.Invoke(AppendText, "\nVersion : " + Environment.OSVersion.Version.ToString());
			//this.Dispatcher.Invoke(AppendText,"\nProva3");
			//Thread.Sleep(300);
			this.Dispatcher.Invoke(AppendText, "\nVersionString : " + Environment.OSVersion.VersionString.ToString());
			//this.Dispatcher.Invoke(AppendText,"\nProva4");
			//Thread.Sleep(300);
			//this.Dispatcher.Invoke(AppendText,"\nProva5");
			//Thread.Sleep(300);
			//this.Dispatcher.Invoke(AppendText,"\nProva6");
			//Thread.Sleep(300);

			//close window
			if (SplashScreen)
			{

				bool b;
				do
				{
					object obj = this.Dispatcher.Invoke((Action)delegate() { isFadedIn(); });
					if (obj is bool)
						b = (bool)obj;
					else
						b = true;
					Thread.Sleep(10);
				} while (!b);
				Thread.Sleep(1000);
				this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate() { CloseForm(); });
			}
		}
		delegate bool _isFadedIn();
		private bool isFadedIn()
		{
			if (this.Opacity >= 1)
				return true;
			else
				return false;
		}
		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate() { CloseForm();});
		}

		
		public void CloseForm()
		{
			if (_FadeOut)
			{
				this.qty = Opacity_dec;
				if (this.Opacity<=0)
				{
					IsFadedOut = true;
					//this.Close();
				}
				//else
				//    this.InvalidateVisual();
			}
			else
			{
				IsFadedOut = true;
				this.Close();
			}
			//this.Opacity_inc = 0.0;
			//this.Opacity -= Opacity_dec;

		}


		private void Append(string txt)
		{
			TxtShow.Text += txt;
			line_count++;
			if(line_count>max_line)
			{
				Remove();
				line_count--;
			}
			
		}

		private void Remove()
		{
			string[] str;
			str = TxtShow.Text.Split(new char[] {'\n'},2);
			TxtShow.Text = str[1];
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CloseForm();
			//if (_FadeOut)
				//this.qty = Opacity_dec;
				//while (true) Thread.Sleep(10); ;
			//else
			//    this.Close();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			timer1.Stop();
			IsFadedOut = true;
		}
	}
}
