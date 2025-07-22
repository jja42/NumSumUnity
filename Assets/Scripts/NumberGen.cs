using System.Collections.Generic;
using UnityEngine;

public class NumberGen : MonoBehaviour
{

    public static NumberGen instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public class Grid
    {
        public int size;
        public List<List<Num>> grid;
        public List<int> column_sums;
        public List<int> row_sums;
    }

    public class Num
    {
        public int value;
        public bool is_marked;
        public bool is_valid;
    }

    public Grid CreateGrid(int size)
    {
        //Init Grid Params
        Grid g = new();
        g.size = size;
        g.column_sums = new List<int>();
        g.row_sums = new List<int>();
        List<List<Num>> rows = new List<List<Num>>();

        //Generate Nums for Our Grid
        for (int i = 0; i < size; i++)
        {

            int max_size = size * 2;
            //Odd Nums become Even and round up. Even Nums become odd and round down.
            int min_size = (size + 1) / 2 + 1;
            //generate random number. modulo our max gives 0 through max - 1. Add 1 for 0 through max. 
            //minus min for the modulo and add it back after to give us the correct lower bound
            int rand_num = Random.Range(min_size, max_size);

            //generate twice to provide plausible invalid nums
            List<Num> section_1 = Create_Nums(rand_num, (size + 1) / 2); //Allows us to handle odd nums
            List<Num> section_2 = Create_Nums(rand_num, size / 2);

            List<Num> nums = new List<Num>(section_1);
            nums.AddRange(section_2);

            rows.Add(nums);
        }

        //Set valid numbers randomly
        for (int row = 0; row < size; row++)
        {
            //init index list
            List<int> indices = new List<int>();
            for (int i = 0; i < size; i++) indices.Add(i);
            Shuffle(indices);

            int max_size = ((size + 1) / 2) + 1;
            int min_size = size / 2 - 1;
            if (min_size < 1)
            {
                min_size = 1;
            }

            int valid_count = Random.Range(min_size,max_size+1);

            //Mark shuffled list as valid up to count
            for (int i = 0; i < valid_count; i++)
            {
                List<Num> nums = rows[row];
                Num n = nums[indices[i]];
                n.is_valid = true;
            }
        }

        //finally calculate our sums

        //start with rows
        for (int row = 0; row < size; row++)
        {
            int sum = 0;
            List<Num> row_nums = rows[row];
            for (int c = 0; c < size; c++)
            {
                Num n = row_nums[c];
                if (n.is_valid)
                {
                    sum += n.value;
                }
            }
            g.row_sums.Add(sum);
        }


        //then columns
        for (int col = 0; col < size; col++)
        {
            int sum = 0;
            //loop through our rows
            for (int row = 0; row < size; row++)
            {
                List<Num> row_nums = rows[row];
                Num n = row_nums[col];
                if (n.is_valid)
                {
                    sum += n.value;
                }
            }

            g.column_sums.Add(sum);
        }

        g.grid = rows;

        return g;
    }

    List<Num> Create_Nums(int val, int num_count)
    {
        List<Num> nums = new List<Num>();

        //to split into num_count parts we must do that many splits minus 1
        int splits = num_count - 1;

        //declare our arrays for splitting
        List<int> split_list = new List<int>();
        List<int> parts_list = new List<int>();

        int i = 0;

        while (i < splits)
        {
            //Generate a number between 1 and val minus 1
            int rand_num = Random.Range(1, val);

            //Check for dupe and if so retry
            if (Check_Dupe(rand_num, split_list, i))
            {
                continue;
            }
            //Otherwise continue

            //add rand_num to our array
            split_list.Add(rand_num);

            //rand_num will become an effective index into our sum as an array.

            i++;
        }

        //with our splits done, it's now time to generate our actual parts

        //first we'll sort our splits
        split_list.Sort();

        //set up a reference to our last split point
        //start at 0
        int last_split = 0;

        //iterate through our parts array except the last index
        for (int j = 0; j < num_count - 1; j++)
        {

            //our value becomes the difference between the last split and our split point
            //for our first split point, this is just the value of the split point
            //ie: if our next split is 18 and our last split was 12, our part is 6
            //since our splits break into sections that total to our sum
            //this ensures that we get a perfect range of numbers that add to our sum
            parts_list.Add(split_list[j] - last_split);
            last_split = split_list[j];
        }

        //our last part is from our last split to our total value
        //ie if our total is 20 and our last split is 17, our last part is 3
        parts_list.Add(val - last_split);

        for (int p = 0; p < num_count; p++)
        {
            Num n = Create_Num(parts_list[p]);
            nums.Add(n);
        }

        return nums;
    }

    Num Create_Num(int val)
    {
        Num n = new();
        n.value = val;
        n.is_marked = false;
        n.is_valid = false;
        return n;
    }

    bool Check_Dupe(int num, List<int> split_list, int size)
    {
        //loop through our list and check for a dupe
        for (int n = 0; n < size; n++)
        {
            if (split_list[n] == num)
            {
                return true;
            }
        }
        return false;
    }

    void Shuffle(List<int> list)
    {
        int size = list.Count;
        //loop through, pick a random index and shuffle
        for (int i = size - 1; i > 0; i--)
        {
            int j = Random.Range(0,i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
