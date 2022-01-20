using System.Collections;

namespace SimuLean
{
    class DoubleMinBinaryHeap
    {
        ArrayList values;
        ArrayList objects;

        public DoubleMinBinaryHeap(int n)
        {
            values = new ArrayList(n);
            objects = new ArrayList(n);
        }

        private void Swap(int i, int j)
        {
            double intermVal = (double)values[i];
            values[i] = values[j];
            values[j] = intermVal;

            object intermObj = objects[i];
            objects[i] = objects[j];
            objects[j] = intermObj;
        }

        public void Reset()
        {
            values.Clear();
            objects.Clear();
        }

        public double GetMinValue()
        {
            if (values.Count == 0)
            {
                return 0;
            }
            return (double)values[0];
        }

        public int Count()
        {
            return values.Count;
        }

        public void Add(double key, object content)
        {
            int parent, child;
            values.Add(key);
            objects.Add(content);

            child = values.Count - 1;
            if (child == 0)
            {
                return;
            }
            parent = (child - 1) / 2;

            while (key < (double)values[parent])
            {
                Swap(child, parent);
                child = parent;
                if (child == 0)
                {
                    return;
                }
                parent = (child - 1) / 2;
            }
        }

        public object First()
        {
            return objects[0];
        }

        public object RetrieveFirst()
        {
            int i = 0, i1, i2, n = objects.Count - 1;
            bool keep = true;
            object first = objects[0];

            objects[0] = objects[n];
            values[0] = values[n];
            objects.RemoveAt(n);
            values.RemoveAt(n);

            while (keep)
            {
                i1 = 2 * i + 1;
                i2 = 2 * i + 2;
                if (i1 >= n)
                {
                    keep = false;
                }
                else if (i2 >= n)
                {
                    if ((double)values[i1] < (double)values[i])
                    {
                        Swap(i1, i);
                    }
                    keep = false;
                }
                else
                {
                    if ((double)values[i1] < (double)values[i2])
                    {
                        if ((double)values[i1] < (double)values[i])
                        {
                            Swap(i1, i);
                            i = i1;
                        }
                        else if ((double)values[i2] < (double)values[i])
                        {
                            Swap(i2, i);
                            i = i2;
                        }
                        else
                        {
                            keep = false;
                        }

                    }
                    else
                    {
                        if ((double)values[i2] < (double)values[i])
                        {
                            Swap(i2, i);
                            i = i2;
                        }
                        else if ((double)values[i1] < (double)values[i])
                        {
                            Swap(i1, i);
                            i = i1;
                        }
                        else
                        {
                            keep = false;
                        }
                    }
                }
            }
            return first;
        }
    }
}
