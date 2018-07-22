using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace Algo_Spring2016_TermProject_i13_1821_i13_1822
{
    public partial class PakistanMap : Form
    {
        public List<string> Cities = new List<string>();
        private List<string> CitiesTemp = new List<string>();

        private int[] NumOfVisits = new int[39];

        public List<List<int>> AdjMatrix = new List<List<int>>();
        public int[,] FloydPath;

        public List<List<int>> AdjMatrixAlter = new List<List<int>>();
        public int[,] FloydPathAlter;

        public List<List<List<string>>> EveryPairShortestpath = new List<List<List<string>>>();
        public List<List<List<string>>> EveryPairAlternatepath = new List<List<List<string>>>();

        private ComboBox c1 = new ComboBox();
        private ComboBox c2 = new ComboBox();

        private Label selectLabel = new Label();
        private Label sp = new Label();
        private Label ap = new Label();

        CheckedListBox checkList = new CheckedListBox();

        Label FROM = new Label();
        Label TO = new Label();


        public PakistanMap()
        {
            for (int i = 0; i < 39; i++)
            {
                NumOfVisits[i] = 0;
            }

            LoadAdj();
            AdjMatrixAlter = AdjMatrix.ToList();

            FloydPath = new int[AdjMatrix.Count, AdjMatrix.Count];
            FloydPathAlter = new int[AdjMatrixAlter.Count, AdjMatrixAlter.Count];

            Floyd FloydAlgorithm = new Floyd();
            FloydAlgorithm.Run(ref AdjMatrix, ref FloydPath, true);
            FloydAlgorithm.Run(ref AdjMatrixAlter, ref FloydPathAlter, false);

            PreCompute();

            c1.DataSource = Cities;
            c1.Top = 40;

            c2.DataSource = CitiesTemp;
            c2.Left = 150;
            c2.Top = 40;


            FROM.Text = "FROM";
            TO.Text = "TO";

            selectLabel.Text = "Select City";
            selectLabel.Top = 10;
            selectLabel.Left = 30;

            sp.Text = "Shortest Path";
            sp.Left = 220; sp.Top = 10;

            ap.Text = "Alternate Path";
            ap.Left = 720; ap.Top = 10;

            InitializeComponent();
        }

        public void LoadAdj()
        {

            string path = @"D:\study\SMS-6\ALGO\Assignments\project.txt";
            using (StreamReader readFile = new StreamReader(path))
            {
                string line;

                line = readFile.ReadLine();
                Cities.AddRange(line.Split('\t').Skip(1).ToList<string>());
                CitiesTemp.AddRange(line.Split('\t').Skip(1).ToList<string>());
                while ((line = readFile.ReadLine()) != null)
                {
                    List<int> row;
                    row = line.Split('\t').Skip(1).ToList<string>().Select(s => int.Parse(s)).ToList();
                    AdjMatrix.Add(new List<int>());
                    AdjMatrix[AdjMatrix.Count - 1].AddRange(row);

                }
            }
        }

        public void getPathRec(int i, int j, ref List<string> connectedCities)
        {
            int k = FloydPath[i, j];
            if (k != 0)
            {
                getPathRec(i, k - 1, ref connectedCities);
                connectedCities.Add(Cities[k - 1]);
                NumOfVisits[k - 1]++;
                getPathRec(k - 1, j, ref connectedCities);
            }
        }

        public void getPathRecAlter(int i, int j, ref List<string> connectedCities)
        {
            int k = FloydPathAlter[i, j];
            if (k != 0)
            {
                getPathRecAlter(i, k - 1, ref connectedCities);
                connectedCities.Add(Cities[k - 1]);
                getPathRecAlter(k - 1, j, ref connectedCities);
            }
        }

        public void PreCompute()
        {
            for (int i = 0; i < Cities.Count; i++)
            {
                EveryPairShortestpath.Add(new List<List<string>>());
                EveryPairAlternatepath.Add(new List<List<string>>());

                List<List<string>> targetCities = new List<List<string>>();
                List<List<string>> targetCitiesAlter = new List<List<string>>();

                for (int j = 0; j < Cities.Count; j++)
                {
                    List<string> connectedCities = new List<string>();
                    targetCities.Add(new List<string>());

                    List<string> connectedCitiesAlter = new List<string>();
                    targetCitiesAlter.Add(new List<string>());

                    getPathRec(i, j, ref connectedCities);
                    targetCities[j] = connectedCities;

                    getPathRecAlter(i, j, ref connectedCitiesAlter);
                    targetCitiesAlter[j] = connectedCitiesAlter;
                }

                EveryPairShortestpath[i] = targetCities;
                EveryPairAlternatepath[i] = targetCitiesAlter;
            }
        }

        public void setTreeView(List<List<string>> targetCities, int city, int left, int top)
        {
            TreeView ShortestPathTreeView = new TreeView();
            TreeView AlternatePathTreeView = new TreeView();

            ShortestPathTreeView.Size = new Size(250, 600);
            AlternatePathTreeView.Size = new Size(250, 600);

            ShortestPathTreeView.Left = left;
            ShortestPathTreeView.Top = top;

            AlternatePathTreeView.Left = left + 500;
            AlternatePathTreeView.Top = top;

            panel1.Controls.Add(sp);
            panel1.Controls.Add(ap);

            panel1.Controls.Add(ShortestPathTreeView);
            panel1.Controls.Add(AlternatePathTreeView);

            panel1.Refresh();

            for (int i = 0; i < targetCities.Count; i++)
            {
                if (AdjMatrix[city][i] != 0)
                {

                    TreeNode n = new TreeNode(AdjMatrix[city][i].ToString() + "----------" + Cities[i]);
                    for (int j = 0; j < targetCities[i].Count; j++)
                    {
                        TreeNode nc = new TreeNode(targetCities[i][j]);
                        n.Nodes.Add(nc);
                    }

                    ShortestPathTreeView.Nodes.Add(n);
                }
            }


            for (int i = 0; i < targetCities.Count; i++)
            {
                if (AdjMatrixAlter[city][i] != 0)
                {

                    TreeNode n = new TreeNode(AdjMatrixAlter[city][i].ToString() + "----------" + Cities[i]);
                    for (int j = 0; j < targetCities[i].Count; j++)
                    {
                        TreeNode nc = new TreeNode(targetCities[i][j]);
                        n.Nodes.Add(nc);
                    }

                    AlternatePathTreeView.Nodes.Add(n);
                }
            }
        }

        public void setSingleTreeView(List<string> targetCities, int left, int top)
        {
            TreeView ShortestPathTreeView = new TreeView();
            TreeView AlternatePathTreeView = new TreeView();

            ShortestPathTreeView.Size = new Size(150, 400);
            AlternatePathTreeView.Size = new Size(150, 400);

            ShortestPathTreeView.Left = left;
            ShortestPathTreeView.Top = top;

            AlternatePathTreeView.Left = left + 100;
            AlternatePathTreeView.Top = top;

            Label newSP = new Label();
            newSP.Text = "Shortest Path";
            newSP.Top = 10;
            newSP.Left = left + 30;

            panel1.Controls.Add(newSP);
            panel1.Controls.Add(ap);

            panel1.Controls.Add(ShortestPathTreeView);
            //panel1.Controls.Add(AlternatePathTreeView);

            panel1.Refresh();
            ShortestPathTreeView.Nodes.Add(new TreeNode("Total Distance: " + AdjMatrix[cityFrom][cityTo].ToString()));

            for (int i = 0; i < targetCities.Count; i++)
            {
                ShortestPathTreeView.Nodes.Add(new TreeNode(targetCities[i]));
                if (i + 1 < targetCities.Count)
                {
                    TreeNode n = new TreeNode(AdjMatrix[Cities.IndexOf(targetCities[i])][Cities.IndexOf(targetCities[i + 1])].ToString());
                    ShortestPathTreeView.Nodes.Add(n);
                }
            }
        }

        private void RemoveHandlers(ref ComboBox c)
        {
            c1.SelectedIndexChanged -= new EventHandler(CityChoosed);
            c1.SelectedIndexChanged -= new EventHandler(Question3CityChoosed);
            c1.SelectedIndexChanged -= new EventHandler(Q4From);
        }

        private void Question2Button_Click(object sender, EventArgs e)
        {
            RemoveHandlers(ref c1);
            panel1.Controls.Clear();
            c1.SelectedIndexChanged += new EventHandler(CityChoosed);

            panel1.Controls.Add(selectLabel);
            panel1.Controls.Add(c1);
        }

        private void Question3Button_Click(object sender, EventArgs e)
        {
            RemoveHandlers(ref c1);
            panel1.Controls.Clear();

            panel1.Controls.Add(selectLabel);
            panel1.Controls.Add(c1);

            c1.SelectedIndexChanged += new EventHandler(Question3CityChoosed);

        }

        private void Question4Button_Click(object sender, EventArgs e)
        {
            RemoveHandlers(ref c1);
            panel1.Controls.Clear();

            FROM.Top = 10;
            FROM.Left = 40;

            TO.Top = 10;
            TO.Left = 190;

            panel1.Controls.Add(c1);
            panel1.Controls.Add(FROM);

            c1.SelectedIndexChanged += new EventHandler(Q4From);
        }

        private void Question5Button_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            TreeView tv = new TreeView();
            for (int i = 0; i < Cities.Count; i++)
            {
                TreeNode n1 = new TreeNode(Cities[i]);
                for (int j = 0; j < Cities.Count; j++)
                {
                    string n3s = "";

                    List<string> temp = new List<string>();
                    if (EveryPairShortestpath[i][j].Count != 0)
                    {
                        temp.Add(Cities[i]);
                        temp.AddRange(EveryPairShortestpath[i][j]);
                        temp.Add(Cities[j]);
                        n3s = temp[0] + "--------";

                    }
                    TreeNode n2 = new TreeNode(AdjMatrix[i][j] + "---------------" + Cities[j]);
                    for (int k = 0; k < temp.Count; k++)
                    {
                        if (k < temp.Count - 1)
                        {
                            n3s += AdjMatrix[Cities.IndexOf(temp[k])][Cities.IndexOf(temp[k + 1])] + "--------" + temp[k + 1] + "--------";
                        }

                    }

                    if (EveryPairShortestpath[i][j].Count != 0)
                    {
                        TreeNode n3 = new TreeNode(n3s);
                        n2.Nodes.Add(n3);
                    }

                    n1.Nodes.Add(n2);
                }
                tv.Nodes.Add(n1);
            }

            panel1.Controls.Add(sp);
            panel1.Controls.Add(ap);
            tv.Size = new Size(300, 600);
            tv.Left = 150;
            tv.Top = 40;
            panel1.Controls.Add(tv);
            panel1.Refresh();
        }

        private void Question6Button_Click(object sender, EventArgs e)
        {
            RemoveHandlers(ref c1);
            panel1.Controls.Clear();
            Label Production = new Label();
            Production.Text = "Select Production Unit";
            Production.Top = 10;
            Production.AutoSize = true;

            Label Distribution = new Label();
            Distribution.Text = "Choose Distrbution Points";
            Distribution.Top = 10;
            Distribution.Left = 180;
            Distribution.AutoSize = true;


            checkList.Items.AddRange(Cities.ToArray());
            checkList.Left = 150;
            checkList.Top = 40;
            checkList.Size = new Size(200, 600);

            Button Calculate = new Button();
            Calculate.Text = "Show Path";
            Calculate.AutoSize = true;
            Calculate.Left = 400;
            Calculate.Top = 40;
            Calculate.Click += new EventHandler(CalculatePath);

            panel1.Controls.Add(Calculate);
            panel1.Controls.Add(checkList);
            panel1.Controls.Add(Production);
            panel1.Controls.Add(Distribution);
            panel1.Controls.Add(c1);

        }

        private void Question7Button_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            Label l1 = new Label();
            Label l2 = new Label();
            l1.BackColor = Color.Black;
            l2.BackColor = Color.Black;
            l1.ForeColor = Color.White;
            l2.ForeColor = Color.White;
            l1.Font = new Font("Arial", Font.Size, FontStyle.Bold);
            l1.Left = 150;
            l2.Left = 150;
            l1.Top = 20;
            l2.Top = 40;
            l1.AutoSize = true;
            l2.AutoSize = true;
            l1.Text = "Most Visited City: " + Cities[NumOfVisits.ToList().IndexOf(NumOfVisits.Max())];
            l2.Text = "Num of Visits:  " + NumOfVisits.Max().ToString();
            panel1.Controls.Add(l1);
            panel1.Controls.Add(l2);
        }

        private void CityChoosed(object sender, EventArgs e)
        {
            int city = c1.SelectedIndex;
            setTreeView(EveryPairShortestpath[city], city, 150, 40);

        }

        private void Question3CityChoosed(object sender, EventArgs e)
        {
            int city = c1.SelectedIndex;
            List<List<string>> temp = new List<List<string>>();

            for (int i = 0; i < EveryPairShortestpath[city].Count; i++)
            {
                temp.Add(new List<string>());
                temp[i] = EveryPairShortestpath[i][city];
            }

            setTreeView(temp, city, 150, 40);
        }

        int cityFrom = -1;
        int cityTo = -1;
        private void Q4From(object sender, EventArgs e)
        {
            c2.SelectedIndexChanged -= new EventHandler(Q4To);
            RemoveHandlers(ref c2);
            cityFrom = c1.SelectedIndex;

            panel1.Controls.Add(c2);
            panel1.Controls.Add(TO);
            c2.SelectedIndexChanged += new EventHandler(Q4To);
        }

        private void Q4To(object sender, EventArgs e)
        {
            cityTo = c2.SelectedIndex;

            List<string> temp = new List<string>();
            temp.Add(Cities[cityFrom]);
            temp.AddRange(EveryPairShortestpath[cityFrom][cityTo]);
            temp.Add(Cities[cityTo]);
            setSingleTreeView(temp, 300, 40);
        }

        private void CalculatePath(object sender, EventArgs e)
        {
            int startPoint = c1.SelectedIndex;
            List<string> distributionPoints = new List<string>();

            foreach (String indexChecked in checkList.CheckedItems)
            {
                distributionPoints.Add(indexChecked);
            }

            string path = Cities[startPoint] + "----";
            for (int i = 0; distributionPoints.Count != 0; i++)
            {
                List<int> cost = new List<int>();

                for (int j = 0; j < distributionPoints.Count; j++)
                {
                    cost.Add(AdjMatrix[startPoint][Cities.IndexOf(distributionPoints[j])]);

                }

                int minCostElement = cost.IndexOf(cost.Min());

                if (Cities[startPoint] != distributionPoints[minCostElement])
                {
                    if (distributionPoints.Count > 1)
                    {
                        path += AdjMatrix[Cities.IndexOf(Cities[startPoint])][Cities.IndexOf(distributionPoints[minCostElement])] + "----" + distributionPoints[minCostElement] + "----";
                    }
                    else
                    {
                        path += AdjMatrix[Cities.IndexOf(Cities[startPoint])][Cities.IndexOf(distributionPoints[minCostElement])] + "----" + distributionPoints[minCostElement];
                    }

                }
                startPoint = Cities.IndexOf(distributionPoints[minCostElement]);
                distributionPoints.RemoveAt(minCostElement);
                cost.RemoveAt(minCostElement);
            }

            Label spath = new Label();
            spath.Text = path;
            spath.Top = 100;
            spath.Left = 350;
            spath.BackColor = Color.White;
            spath.ForeColor = Color.DarkGreen;
            spath.AutoSize = true;

            panel1.Controls.Add(spath);
            panel1.Refresh();
        }

        private void ShowMap_Click(object sender, EventArgs e)
        {
            /*Chart chart = new Chart();
            chart.Size = new Size(900, 600);
            chart.Visible = true;
            chart.TabIndex = 1;
            chart.Text = "PakistanMap";
            chart.Location = new Point(10, 10);
            ChartArea chartArea = new ChartArea("draw");
            chartArea.AxisX.Title = "Cities";
            chartArea.AxisY.Title = "Cities";
            chart.ChartAreas.Add(chartArea);
            chart.ChartAreas["draw"].AxisX.Minimum = 1;
            chart.ChartAreas["draw"].AxisX.Maximum = 40;
            chart.ChartAreas["draw"].AxisX.Interval = 1;
            chart.ChartAreas["draw"].AxisX.MajorGrid.LineColor = Color.White;
            chart.ChartAreas["draw"].AxisY.Minimum = 1;
            chart.ChartAreas["draw"].AxisY.Maximum = 40;
            chart.ChartAreas["draw"].AxisY.Interval = 1;
            chart.ChartAreas["draw"].AxisY.MajorGrid.LineColor = Color.White;
            chart.ChartAreas["draw"].BackColor = Color.Aqua;
            chart.ChartAreas["draw"].AxisX.Title = "Cities";
            chart.ChartAreas["draw"].AxisX.Title = "Cities";

            chart.Series.Add("test1");
            chart.Series.Add("test2");
            


            for (int i = 0; i < Cities.Count; i++)
            
            {
                chart.ChartAreas[0].AxisY.CustomLabels.Add(i+1, i+1, Cities[i]);
            }

            for (int i = 0; i < 50; i++)
            {
                chart.Series["test1"].Points.AddXY
                                (rdn.Next(0, 10), rdn.Next(0, 10));
                chart.Series["test2"].Points.AddXY
                                (rdn.Next(0, 10), rdn.Next(0, 10));
            }

            chart.Series["test1"].ChartType =
                                SeriesChartType.FastLine;
            chart.Series["test1"].Color = Color.Red;

            chart.Series["test2"].ChartType =
                                SeriesChartType.FastLine;
            chart.Series["test2"].Color = Color.Blue;
            panel1.Controls.Add(chart);
            panel1.Refresh();*/
        } 
    }
}