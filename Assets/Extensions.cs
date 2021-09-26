namespace Imbalance
{
    public static class Extensions
    {
        public static string ToImbalance(this int c)
        {
            c &= 255;
            string rep = ".«";
            if((c & 1) == 1)
                rep = "1" + rep;
            else
            {
                rep = "2" + rep;
                c -= 2;
            }
            c >>= 1;
            for(int i = 1; i < 15; i += 2)
            {
                int code = (c & 1) + (c & 2);
                switch(code)
                {
                    case 0:
                        rep = "12" + rep;
                        break;
                    case 1:
                        rep = "11" + rep;
                        break;
                    case 2:
                        rep = "22" + rep;
                        break;
                    case 3:
                        rep = "21" + rep;
                        break;
                }
                c >>= 2;
            }

            string rep1 = rep;
            string rep2 = rep;
            if(rep1.StartsWith("2"))
                rep1 = rep1.Substring(1);
            if(rep2.StartsWith("1"))
                rep2 = rep2.Substring(1);

            while(rep1.StartsWith("12"))
                rep1 = rep1.Substring(2);
            while(rep2.StartsWith("21"))
                rep2 = rep2.Substring(2);

            rep1 = "«" + rep1;
            rep2 = "»" + rep2;

            return rep1.Length < rep2.Length ? rep1 : rep2;
        }
    }
}