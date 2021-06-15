using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Media;
using System.IO;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using static Tarea1_3erParcial.Class2;

namespace Tarea1_3erParcial
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Variables globales

        int conta = 0;
        int aux = 0;
        int cant = 0;
        int indice = 0;
        int ultCont = 0;
        bool x = true;
        int media = 0;

        MediaPlayer mediaPlayer = new MediaPlayer();
        DispatcherTimer timer = new DispatcherTimer();

        List<string> lstMp3 = new List<string>();
        List<string> lstJpg = new List<string>();
        
        

        bool mediaPlayerIsPlaying = false;
        bool userIsDraggingSlider = false;
     


        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            lstMusic.ItemsSource = lstMp3;


        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                try
                {
                    txtStatus.Text = String.Format("{0} / {1} "
             , mediaPlayer.Position.ToString(@"mm\:ss")
             , mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                    if (userIsDraggingSlider == false)
                    {
                        this.sldProgress.Minimum = 0;
                        this.sldProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                        this.sldProgress.Value = mediaPlayer.Position.TotalSeconds;
                    }
                }
                catch
                {
                }
               

            }
        }


        private void sldProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }
        private void sldProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sldProgress.Value);
        }

        private void ImgCargar_MouseDown(object sender, MouseButtonEventArgs e)
        {      
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "MP3 Files (*.mp3)|*mp3";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        mediaPlayer.Open(new Uri(openFileDialog.FileName));
                        lstMp3.Add(openFileDialog.FileName);
                        lstMusic.ItemsSource = lstMp3;
                        
                        mediaPlayer.Play();
       
                    }

                    OpenFileDialog openFileDialogo5= new OpenFileDialog();
                    openFileDialogo5.Filter = "JPG Files (*.jpg)|*jpg";
            if (openFileDialogo5.ShowDialog() == true)
              {
                     
                        mediaPlayer.Open(new Uri(openFileDialogo5.FileName));
                        lstJpg.Add(System.IO.Path.GetFullPath(openFileDialogo5.FileName));
                        lstImg.Items.Add(System.IO.Path.GetFileNameWithoutExtension(openFileDialogo5.FileName));
               Images.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(openFileDialogo5.FileName)));


            }

        }
        

        private void BtnPlayPause_MouseDown(object sender, MouseButtonEventArgs e)
            {
                conta++;
                if (conta % 2 == 0)
                {
                    mediaPlayer.Pause();
                    timer.Stop();
                }
                else

                {
                    mediaPlayer.Play();
                    timer.Start();

                }
            }

        private void BtnConsultar_Click(object sender, RoutedEventArgs e)
        {
            HttpWebRequest request =
                WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=chart.gettoptracks&api_key=1ca76ca694c848c79150021a0f6aeba8&format=json")
            as HttpWebRequest;

            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            using (StreamReader reader =
                new StreamReader(response.GetResponseStream()))
            {
                string resp = reader.ReadToEnd();

                Cancion song = new Cancion();
                song = JsonConvert.DeserializeObject<Cancion>(resp);
                List<TrackData> list = song.Tracks.tracklist.ToList();
                foreach (TrackData x in list)
                {
                    if (ultCont < 5)
                    {
                        lstBox.Items.Add(x.name);
                    }
                }
            }
        }   
    }
 }
