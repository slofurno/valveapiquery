using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace valveapiquery
{
    

    public class MatchModel
    {

        public int match_id { get; set; }
        public int match_seq_num;

        public MatchModel()
        {


        }




    }

    public class MyMatchPlayer
    {


            


    }

    public class MatchPlayer
    {

        public long account_id {get; set;}
        public int hero_id {get; set;}
        public int kills {get; set;}
        public int deaths {get; set;}
        public int assists {get; set;}
        public int last_hits {get; set;}
        public int denies {get; set;}

        public int item_0 { get { return items[0]; } set { items[0] = value; } }
        public int item_1 { get { return items[1]; } set { items[1] = value; } }
        public int item_2 { get { return items[2]; } set { items[2] = value; } }
        public int item_3 { get { return items[3]; } set { items[3] = value; } }
        public int item_4 { get { return items[4]; } set { items[4] = value; } }
        public int item_5 { get { return items[5]; } set { items[5] = value; } }

        public int[] items { get; set; }
        public double itemhash { get; set; }

        public MatchPlayer()
        {
            this.itemhash = -1;
            this.items = new int[6];

        }

        public static void buildItem(IEnumerable<MatchPlayer>players)
        {

            // int[] items = new int[6];

            foreach (var player in players)
            {
                /*
                for (int i = 0; i < 6; i++)
                {
                    items[i] = Convert.ToInt32(Console.ReadLine());

                }*/
                /*
                items[0] = player.item_0;
                items[1] = player.item_1;
                items[2] = player.item_2;
                items[3] = player.item_3;
                items[4] = player.item_4;
                items[5] = player.item_5;
                */

                double somenumber = 0;

                Array.Sort(player.items);



                for (int i = 0; i < 6; i++)
                {
                    somenumber += (player.items[i] * Math.Pow(120, i));

                }

                player.itemhash = somenumber;

            }

        }



    }

        


    
}
