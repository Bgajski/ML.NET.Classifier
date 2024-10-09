using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace ML.Graph
{
    public class BinaryChartDisplay : IChartDisplay
    {
        private static readonly List<(Color LightColor, Color DarkColor)> DominantColors = new List<(Color, Color)>
        {
            (Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185)),    
            (Color.FromArgb(231, 76, 60), Color.FromArgb(192, 57, 43)),      
            (Color.FromArgb(46, 204, 113), Color.FromArgb(39, 174, 96)),     
            (Color.FromArgb(155, 89, 182), Color.FromArgb(142, 68, 173)),    
            (Color.FromArgb(241, 196, 15), Color.FromArgb(243, 156, 18))    
        };

        private static readonly List<(Color LightColor, Color DarkColor)> RegularColors = new List<(Color, Color)>
        {
            (Color.FromArgb(149, 165, 166), Color.FromArgb(127, 140, 141)),  
            (Color.FromArgb(230, 126, 34), Color.FromArgb(211, 84, 0)),      
            (Color.FromArgb(26, 188, 156), Color.FromArgb(22, 160, 133)),    
            (Color.FromArgb(52, 73, 94), Color.FromArgb(44, 62, 80)),        
            (Color.FromArgb(241, 196, 15), Color.FromArgb(243, 156, 18)),    
            (Color.FromArgb(155, 89, 182), Color.FromArgb(142, 68, 173)),    
            (Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185)),   
            (Color.FromArgb(46, 204, 113), Color.FromArgb(39, 174, 96)),     
            (Color.FromArgb(231, 76, 60), Color.FromArgb(192, 57, 43)),      
            (Color.FromArgb(26, 188, 156), Color.FromArgb(22, 160, 133))    
        };

        public void DisplayDataOnChart(Chart chart, DataTable dataTable, SeriesChartType chartType, string outcomeColumn)
        {
            var convertedTable = ConvertDataTableToInt(dataTable);

            var outcomes = convertedTable.AsEnumerable()
                                         .Select(row => row[outcomeColumn].ToString())
                                         .Distinct()
                                         .ToArray();

            if (outcomes.Length != 2)
            {
                throw new InvalidOperationException("Two distinct outcomes are expected for binary classification.");
            }

            chart.Series.Clear();
            chart.ChartAreas.Clear();
            var chartArea = new ChartArea
            {
                BackColor = Color.White,
                AxisX = { Title = "Feature", TitleFont = new Font("Tahoma", 10, FontStyle.Regular) },
                AxisY = { Title = "Value", TitleFont = new Font("Tahoma", 10, FontStyle.Regular) }
            };
            chart.ChartAreas.Add(chartArea);

            var featureNames = convertedTable.Columns.Cast<DataColumn>()
                                      .Select(c => c.ColumnName)
                                      .Where(name => name != outcomeColumn)
                                      .ToArray();

            var featureTotals = featureNames.ToDictionary(
                feature => feature,
                feature => convertedTable.AsEnumerable().Sum(row => Convert.ToInt32(row[feature]))
            );

            var sortedFeatures = featureTotals.OrderByDescending(ft => ft.Value)
                                             .Select(ft => ft.Key)
                                             .ToArray();
            var topFeatures = sortedFeatures.Take(5).ToHashSet();

            var colorAssignments = new Dictionary<string, (Color LightColor, Color DarkColor)>();
            int dominantIndex = 0;
            int regularIndex = 0;

            foreach (var feature in featureNames)
            {
                if (topFeatures.Contains(feature))
                {
                    var color = DominantColors[dominantIndex % DominantColors.Count];
                    colorAssignments[feature] = color;
                    dominantIndex++;
                }
                else
                {
                    var color = RegularColors[regularIndex % RegularColors.Count];
                    colorAssignments[feature] = color;
                    regularIndex++;
                }
            }

            var featureOutcomeTotals = featureNames.ToDictionary(
                feature => feature,
                feature => (
                    Value0: convertedTable.AsEnumerable()
                                           .Where(row => row[outcomeColumn].ToString() == outcomes[0])
                                           .Sum(row => Convert.ToInt32(row[feature])),
                    Value1: convertedTable.AsEnumerable()
                                           .Where(row => row[outcomeColumn].ToString() == outcomes[1])
                                           .Sum(row => Convert.ToInt32(row[feature]))
                )
            );

            foreach (var feature in featureNames)
            {
                var (value0, value1) = featureOutcomeTotals[feature];
                var (lightColor, darkColor) = colorAssignments[feature];

                var series0 = new Series
                {
                    Name = $"{feature} ({outcomes[0]})",
                    ChartType = chartType,
                    XValueType = ChartValueType.String,
                    YValueType = ChartValueType.Int32,
                    Color = lightColor,
                    IsValueShownAsLabel = false
                };

                var series1 = new Series
                {
                    Name = $"{feature} ({outcomes[1]})",
                    ChartType = chartType,
                    XValueType = ChartValueType.String,
                    YValueType = ChartValueType.Int32,
                    Color = darkColor,
                    IsValueShownAsLabel = false
                };

                series0.Points.AddXY(feature, value0);
                series1.Points.AddXY(feature, value1);

                chart.Series.Add(series0);
                chart.Series.Add(series1);
            }

            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisY.LabelStyle.Format = "#";
            chartArea.AxisY.Maximum = 60000;
            chartArea.AxisX.LabelStyle.Enabled = false;

            chart.Legends.Clear();
            chart.Legends.Add(new Legend
            {
                Docking = Docking.Right,
                Alignment = StringAlignment.Near,
                Font = new Font("Tahoma", 10, FontStyle.Regular), 
                BackColor = Color.Transparent
            });

            chart.BackColor = Color.White;
            chart.BorderlineColor = Color.LightGray;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineWidth = 1;

            chart.Invalidate();
        }

        private DataTable ConvertDataTableToInt(DataTable originalTable)
        {
            var newTable = new DataTable();

            foreach (DataColumn column in originalTable.Columns)
            {
                newTable.Columns.Add(column.ColumnName, typeof(int));
            }

            foreach (DataRow row in originalTable.Rows)
            {
                var newRow = newTable.NewRow();
                foreach (DataColumn column in originalTable.Columns)
                {
                    if (int.TryParse(row[column].ToString(), out int intValue))
                    {
                        newRow[column.ColumnName] = intValue;
                    }
                    else
                    {
                        newRow[column.ColumnName] = 0;
                    }
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }
    }
}