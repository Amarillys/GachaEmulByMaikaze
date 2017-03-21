using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GachaEmulator
{
    class Result
    {
        public const int SSR = 5;
        public const int SR = 4;
        public const int R = 3;
        public int ssr_sum_ = 0;
        public int sr_sum_ = 0;
        public int r_sum_ = 0;
        public bool safe_on = false;
        public List<int> card = new List<int>();
    }
    class Gacha
    {
        public double ssr_;
        public double sr_;
        public double r_;
        public int safetymode_;
        public int rndmax_;

        private bool inited_ = false;
        public Gacha(double p_ssr, double p_sr, int p_safetymode, int p_rndmax)
        {
            ssr_ = p_ssr;
            sr_ = p_sr;
            r_ = 1 - ssr_ - sr_;
            safetymode_ = p_safetymode;
            rndmax_ = p_rndmax;
            inited_ = true;
        }

        public Result Go(int p_times, int p_seed)
        {
            if (!inited_)
                return null;
            Random rnd = new Random(p_seed);
            
            Result rsl = new Result();
            int rsl_int = 0;

            int safecard = safetymode_  / 10;

            for (int i = 0; i < p_times; ++i)
            {
                rsl_int = rnd.Next(1, rndmax_);
                //ssr card
                if (rsl_int < rndmax_ * ssr_ )
                {
                    rsl.ssr_sum_++;
                    rsl.card.Add(Result.SSR);
                    continue;
                }
                //rare card
                else if (rsl_int > rndmax_ * (sr_ + ssr_))
                {
                    rsl.r_sum_++;
                    rsl.card.Add(Result.R);
                    continue;
                }
                rsl.sr_sum_++;
                rsl.card.Add(Result.SR);
            }

            int safe_rare = safetymode_ % 10;
            switch (safe_rare)
            {
                case Result.SSR:
                    if (rsl.ssr_sum_ < safecard)
                    {
                        for (int i = rsl.ssr_sum_; i < safecard; ++i)
                            rsl.card.RemoveAt(p_times - i - 1);
                        for (int i = rsl.ssr_sum_; i < safecard; ++i)
                        {
                            rsl.card.Add(Result.SSR);
                        }
                        rsl.ssr_sum_ = safecard;
                        rsl.safe_on = true;
                    }
                    break;
                case Result.SR:
                    if (rsl.sr_sum_ < safecard)
                    {
                        for (int i = rsl.sr_sum_; i < safecard; ++i)
                            rsl.card.RemoveAt(p_times - i - 1);
                        for (int i = rsl.sr_sum_; i < safecard; ++i)
                        {
                            rsl.card.Add(Result.SR);
                        }
                        rsl.sr_sum_ = safecard;
                        rsl.safe_on = true;
                    }
                    break;
                default: 
                    break;
            }
            return rsl;
        }
        
    }
}
