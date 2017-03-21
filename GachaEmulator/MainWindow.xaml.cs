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
using System.Windows.Shapes;

namespace GachaEmulator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public int ssr_sum_ = 0;
        public int sr_sum_ = 0;
        public int gacha_sum_ = 0;
        private int gacha_time = 40;
        public int boom_time_ = 0;
        private int safe_param_ = 0;
        private double ssr_g_ = 0.01;
        private double sr_g_ = 0.18;

        public MainWindow()
        {
            InitializeComponent();
            SSR_G.Text = ssr_g_.ToString();
            SR_G.Text = sr_g_.ToString();

            SafeLable.Visibility = Visibility.Hidden;
            SafeParam.Visibility = Visibility.Hidden;
            safe_param_ = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ssr_g_ = double.Parse(SSR_G.Text);
            sr_g_ = double.Parse(SR_G.Text);
            gacha_time = int.Parse(GachaTime.Text);

            Random rnd = new Random();
            Gacha g = new Gacha(ssr_g_, sr_g_, safe_param_, 10000);
            Result rsl = g.Go(gacha_time, rnd.Next());

            gacha_sum_ += gacha_time;
            ssr_sum_ += rsl.ssr_sum_;
            sr_sum_ += rsl.sr_sum_;
            boom_time_ += rsl.safe_on ? gacha_time : 0;
            SSR_SUM.Content = ssr_sum_.ToString();
            SR_SUM.Content = sr_sum_.ToString();
            SSR_P.Content = (((int)((double)ssr_sum_ / gacha_sum_ * 10000)) / 100f).ToString() + "%";
            SR_P.Content = (((int)((double)sr_sum_ / gacha_sum_ * 10000)) / 100f).ToString() + "%";
            Boom_P.Content = (((int)((double)boom_time_ / gacha_sum_ * 10000)) / 100f).ToString() + "%";

            RslLbl.Text = "Result: \n";
            int i = 0;
            foreach (int c in rsl.card)
            {
                RslLbl.Text += c + "  ";
                ++i;
                if (i % 10 == 0)
                    RslLbl.Text += '\n';
            }
            

            if (rsl.safe_on)
                SAFE_EFF.Visibility = System.Windows.Visibility.Visible;
            else
                SAFE_EFF.Visibility = System.Windows.Visibility.Hidden;

            
        }

        private void SafeSwitch_Checked(object sender, RoutedEventArgs e)
        {
                SafeLable.Visibility = Visibility.Visible;
                SafeParam.Visibility = Visibility.Visible;
                safe_param_ = int.Parse(SafeParam.Text);
        }

        private void SafeSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            SafeLable.Visibility = Visibility.Hidden;
            SafeParam.Visibility = Visibility.Hidden;
            safe_param_ = 0;
        }

        private void SafeParam_TextChanged(object sender, TextChangedEventArgs e)
        {
            safe_param_ = int.Parse(SafeParam.Text);
        }
    }
}
