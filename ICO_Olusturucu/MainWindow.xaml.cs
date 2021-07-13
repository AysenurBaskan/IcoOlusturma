using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace ICO_Olusturucu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Color secim; 
        Point mevcutKonum = new Point(); 
        /// <summary>
        /// Seçilen renk bilgisi ve fare pozizyonu tutulur
        /// </summary>

        private void cp_SelectedColorChanged_1(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp.SelectedColor.HasValue)
            {
                secim = cp.SelectedColor.Value; 
            }
        }
        /// <summary>
        /// Eğer renk seçimi yapılmışsa, seçilen rengi değişkene aktarır
        /// </summary>

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) 
                mevcutKonum = e.GetPosition(this);
        }
        /// <summary>
        /// Fare ile tıklandığında, farenin başlangıç konumu belirlenir
        /// </summary>


        private void ico_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) 
            {
                Line cizim = new Line(); 
                if (radioKalem.IsChecked==true) 
                {
                    cizim.Stroke = new SolidColorBrush(secim); 
                }
               
                else
                {
                    cizim.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255)); 
                }
                cizim.StrokeThickness = double.Parse(cbBoyut.SelectedItem.ToString()); 
                cizim.X1 = mevcutKonum.X; 
                cizim.Y1 = mevcutKonum.Y;
                cizim.X2 = e.GetPosition(this).X;
                cizim.Y2 = e.GetPosition(this).Y;
                mevcutKonum = e.GetPosition(this); 
                ico.Children.Add(cizim); 
            }
        }
        /// <summary>
        /// Çizilecek çizgi nesnesi oluşturuldu
        /// Eğer fare ile sol tıklanıyorsa
        /// Eğer kalem seçilmişse rengi seçimden aldırıyor değilse otomatik olarak beyaz renge boyatıyor
        /// Kalınlığını belirliyor
        /// Fare hareket ettikçe farenin X ve Y değerlerini, ekranda çizdiriyor
        /// Mevcut fare konumunu güncelliyor
        /// Çizimi canvas'a ekliyor. Bu metot sayesinde kullanıcının dilediği işlemler gerçekleştirilmiş oluyor
        /// </summary>

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string dosyaYolu; 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.ico | *.ico"; 
            if (saveFileDialog.ShowDialog() == true) 
            {
                dosyaYolu = saveFileDialog.FileName; 
                FileStream fs = new FileStream(dosyaYolu, FileMode.Create); 
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)ico.ActualWidth,
                    (int)ico.ActualHeight, (int)ico.ActualWidth, (int)ico.ActualHeight, PixelFormats.Pbgra32); 
                bmp.Render(ico);
                BitmapEncoder bmpEncoder = new PngBitmapEncoder(); 
                bmpEncoder.Frames.Add(BitmapFrame.Create(bmp));
                bmpEncoder.Save(fs);
                fs.Close();
                MessageBox.Show("Dosya başarıyla kaydedildi.");
            }
            else
            {
                MessageBox.Show("Yol seçilmedi.");
            }
        }
        /// <summary>
        /// Bu metot kayıt işlemi için
        /// İlk nesne: kaydedilecek konumun seçilmesi için SaveFileDialog nesnesi
        /// Sadece ico formatında kayıt edilebilir olmasını sağlıyor
        /// Eğer dosya kayıt yeri seçilmişse seçilen yol alınır seçilen konumda dosya oluşturulur 
        /// Çizilen resim, Bitmap olarak render edilir
        /// Ardından Encoder kullanılarak bitmap'i ico'ya dönüştürme işlemi gerçekleştirilir ve ico dosyası kaydedilir
        /// Kaydedilince dosya başarıyla kaydedildi messageBox ı gelir
        /// Kaydedilecek konum seçilmemiş ise de yol seçilmesi messageBox ı gelir.
        /// </summary>

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            cbBoyut.Items.Add(1);
            cbBoyut.Items.Add(2);
            cbBoyut.Items.Add(3);
            cbBoyut.Items.Add(4);
            cbBoyut.Items.Add(5);
            cbBoyut.SelectedIndex = 0;
        }
        ///<summary>
        /// Kalınlık değerlerini oluşturan metot
        ///</summary>
    }

}
