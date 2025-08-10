namespace ML.Design
{
    partial class FormInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            splitContainer1 = new SplitContainer();
            button_exit = new Button();
            button_csv_reset = new Button();
            uuisu_label_p1 = new Label();
            comboBox_graph_p1 = new ComboBox();
            textBox_validation_p1 = new TextBox();
            dataChar_csv_p1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            dataGrid_csv_p1 = new DataGridView();
            textBox_csv_p1 = new TextBox();
            button_csv_p1 = new Button();
            lblBoxPerformanceMetric_p2 = new Label();
            txtBoxPerformanceMetric_p2 = new RichTextBox();
            txtBoxMetric_p2 = new RichTextBox();
            lblBoxPerformance_p2 = new Label();
            dataChar_csv_p2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            comboBox_graph_p2 = new ComboBox();
            txtBoxPerformanse_p2 = new RichTextBox();
            lblBoxMetric_p2 = new Label();
            train_TextBox_p2 = new RichTextBox();
            train_label_1_p2 = new Label();
            train_Button_p2 = new Button();
            algorithmSelection_p2 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataChar_csv_p1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGrid_csv_p1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataChar_csv_p2).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = SystemColors.Window;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = Color.LightSkyBlue;
            splitContainer1.Panel1.Controls.Add(button_exit);
            splitContainer1.Panel1.Controls.Add(button_csv_reset);
            splitContainer1.Panel1.Controls.Add(uuisu_label_p1);
            splitContainer1.Panel1.Controls.Add(comboBox_graph_p1);
            splitContainer1.Panel1.Controls.Add(textBox_validation_p1);
            splitContainer1.Panel1.Controls.Add(dataChar_csv_p1);
            splitContainer1.Panel1.Controls.Add(dataGrid_csv_p1);
            splitContainer1.Panel1.Controls.Add(textBox_csv_p1);
            splitContainer1.Panel1.Controls.Add(button_csv_p1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = Color.LightSkyBlue;
            splitContainer1.Panel2.Controls.Add(lblBoxPerformanceMetric_p2);
            splitContainer1.Panel2.Controls.Add(txtBoxPerformanceMetric_p2);
            splitContainer1.Panel2.Controls.Add(txtBoxMetric_p2);
            splitContainer1.Panel2.Controls.Add(lblBoxPerformance_p2);
            splitContainer1.Panel2.Controls.Add(dataChar_csv_p2);
            splitContainer1.Panel2.Controls.Add(comboBox_graph_p2);
            splitContainer1.Panel2.Controls.Add(txtBoxPerformanse_p2);
            splitContainer1.Panel2.Controls.Add(lblBoxMetric_p2);
            splitContainer1.Panel2.Controls.Add(train_TextBox_p2);
            splitContainer1.Panel2.Controls.Add(train_label_1_p2);
            splitContainer1.Panel2.Controls.Add(train_Button_p2);
            splitContainer1.Panel2.Controls.Add(algorithmSelection_p2);
            splitContainer1.Size = new Size(1539, 840);
            splitContainer1.SplitterDistance = 691;
            splitContainer1.SplitterWidth = 2;
            splitContainer1.TabIndex = 0;
            // 
            // button_exit
            // 
            button_exit.BackColor = Color.White;
            button_exit.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button_exit.ForeColor = Color.Red;
            button_exit.Location = new Point(513, 21);
            button_exit.Name = "button_exit";
            button_exit.Size = new Size(163, 29);
            button_exit.TabIndex = 10;
            button_exit.Text = "Exit program";
            button_exit.UseVisualStyleBackColor = false;
            // 
            // button_csv_reset
            // 
            button_csv_reset.BackColor = Color.White;
            button_csv_reset.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 238);
            button_csv_reset.ForeColor = Color.Red;
            button_csv_reset.Location = new Point(513, 56);
            button_csv_reset.Name = "button_csv_reset";
            button_csv_reset.Size = new Size(163, 29);
            button_csv_reset.TabIndex = 4;
            button_csv_reset.Text = "Restore Defaults\n";
            button_csv_reset.UseVisualStyleBackColor = false;
            // 
            // uuisu_label_p1
            // 
            uuisu_label_p1.AutoSize = true;
            uuisu_label_p1.BackColor = SystemColors.Window;
            uuisu_label_p1.BorderStyle = BorderStyle.FixedSingle;
            uuisu_label_p1.Font = new Font("Tahoma", 11F, FontStyle.Bold, GraphicsUnit.Point, 238);
            uuisu_label_p1.ForeColor = SystemColors.Highlight;
            uuisu_label_p1.Location = new Point(14, 21);
            uuisu_label_p1.Name = "uuisu_label_p1";
            uuisu_label_p1.Size = new Size(493, 25);
            uuisu_label_p1.TabIndex = 9;
            uuisu_label_p1.Text = "ML.NET.Classifier: Training and Evaluating Models";
            uuisu_label_p1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox_graph_p1
            // 
            comboBox_graph_p1.BackColor = Color.Ivory;
            comboBox_graph_p1.Font = new Font("Tahoma", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 238);
            comboBox_graph_p1.ForeColor = Color.DarkBlue;
            comboBox_graph_p1.FormattingEnabled = true;
            comboBox_graph_p1.Location = new Point(13, 272);
            comboBox_graph_p1.Margin = new Padding(2);
            comboBox_graph_p1.Name = "comboBox_graph_p1";
            comboBox_graph_p1.Size = new Size(238, 29);
            comboBox_graph_p1.TabIndex = 7;
            comboBox_graph_p1.Text = "Select a chart";
            // 
            // textBox_validation_p1
            // 
            textBox_validation_p1.Font = new Font("Tahoma", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 238);
            textBox_validation_p1.Location = new Point(257, 272);
            textBox_validation_p1.Margin = new Padding(2);
            textBox_validation_p1.Name = "textBox_validation_p1";
            textBox_validation_p1.ReadOnly = true;
            textBox_validation_p1.Size = new Size(419, 28);
            textBox_validation_p1.TabIndex = 4;
            // 
            // dataChar_csv_p1
            // 
            chartArea1.Name = "ChartArea1";
            dataChar_csv_p1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            dataChar_csv_p1.Legends.Add(legend1);
            dataChar_csv_p1.Location = new Point(12, 306);
            dataChar_csv_p1.Name = "dataChar_csv_p1";
            dataChar_csv_p1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            dataChar_csv_p1.Series.Add(series1);
            dataChar_csv_p1.Size = new Size(664, 510);
            dataChar_csv_p1.TabIndex = 3;
            dataChar_csv_p1.Text = "dataChar_csv ";
            // 
            // dataGrid_csv_p1
            // 
            dataGrid_csv_p1.AllowUserToOrderColumns = true;
            dataGrid_csv_p1.BackgroundColor = SystemColors.Window;
            dataGrid_csv_p1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid_csv_p1.Location = new Point(14, 89);
            dataGrid_csv_p1.Name = "dataGrid_csv_p1";
            dataGrid_csv_p1.RowHeadersWidth = 51;
            dataGrid_csv_p1.Size = new Size(662, 178);
            dataGrid_csv_p1.TabIndex = 2;
            // 
            // textBox_csv_p1
            // 
            textBox_csv_p1.Font = new Font("Tahoma", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 238);
            textBox_csv_p1.Location = new Point(257, 55);
            textBox_csv_p1.Name = "textBox_csv_p1";
            textBox_csv_p1.ReadOnly = true;
            textBox_csv_p1.Size = new Size(250, 28);
            textBox_csv_p1.TabIndex = 1;
            // 
            // button_csv_p1
            // 
            button_csv_p1.BackColor = Color.Ivory;
            button_csv_p1.Font = new Font("Tahoma", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button_csv_p1.ForeColor = Color.DarkBlue;
            button_csv_p1.Location = new Point(13, 54);
            button_csv_p1.Name = "button_csv_p1";
            button_csv_p1.Size = new Size(238, 29);
            button_csv_p1.TabIndex = 0;
            button_csv_p1.Text = "Select a datased for training";
            button_csv_p1.UseVisualStyleBackColor = false;
            button_csv_p1.Click += button_csv_p1_Click;
            // 
            // lblBoxPerformanceMetric_p2
            // 
            lblBoxPerformanceMetric_p2.AutoSize = true;
            lblBoxPerformanceMetric_p2.Font = new Font("Tahoma", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 238);
            lblBoxPerformanceMetric_p2.Location = new Point(569, 586);
            lblBoxPerformanceMetric_p2.Name = "lblBoxPerformanceMetric_p2";
            lblBoxPerformanceMetric_p2.Size = new Size(187, 21);
            lblBoxPerformanceMetric_p2.TabIndex = 13;
            lblBoxPerformanceMetric_p2.Text = "Performance metrics";
            // 
            // txtBoxPerformanceMetric_p2
            // 
            txtBoxPerformanceMetric_p2.BackColor = SystemColors.Window;
            txtBoxPerformanceMetric_p2.BorderStyle = BorderStyle.FixedSingle;
            txtBoxPerformanceMetric_p2.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            txtBoxPerformanceMetric_p2.ForeColor = Color.Black;
            txtBoxPerformanceMetric_p2.Location = new Point(569, 623);
            txtBoxPerformanceMetric_p2.Name = "txtBoxPerformanceMetric_p2";
            txtBoxPerformanceMetric_p2.ReadOnly = true;
            txtBoxPerformanceMetric_p2.Size = new Size(265, 193);
            txtBoxPerformanceMetric_p2.TabIndex = 12;
            txtBoxPerformanceMetric_p2.Text = "";
            // 
            // txtBoxMetric_p2
            // 
            txtBoxMetric_p2.BackColor = SystemColors.Window;
            txtBoxMetric_p2.BorderStyle = BorderStyle.FixedSingle;
            txtBoxMetric_p2.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            txtBoxMetric_p2.ForeColor = Color.Black;
            txtBoxMetric_p2.Location = new Point(569, 51);
            txtBoxMetric_p2.Name = "txtBoxMetric_p2";
            txtBoxMetric_p2.ReadOnly = true;
            txtBoxMetric_p2.Size = new Size(265, 216);
            txtBoxMetric_p2.TabIndex = 11;
            txtBoxMetric_p2.Text = "";
            // 
            // lblBoxPerformance_p2
            // 
            lblBoxPerformance_p2.AutoSize = true;
            lblBoxPerformance_p2.Font = new Font("Tahoma", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 238);
            lblBoxPerformance_p2.Location = new Point(569, 275);
            lblBoxPerformance_p2.Name = "lblBoxPerformance_p2";
            lblBoxPerformance_p2.Size = new Size(176, 21);
            lblBoxPerformance_p2.TabIndex = 10;
            lblBoxPerformance_p2.Text = "Model performance";
            // 
            // dataChar_csv_p2
            // 
            chartArea2.Name = "ChartArea1";
            dataChar_csv_p2.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            dataChar_csv_p2.Legends.Add(legend2);
            dataChar_csv_p2.Location = new Point(17, 306);
            dataChar_csv_p2.Margin = new Padding(2);
            dataChar_csv_p2.Name = "dataChar_csv_p2";
            dataChar_csv_p2.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            dataChar_csv_p2.Series.Add(series2);
            dataChar_csv_p2.Size = new Size(542, 510);
            dataChar_csv_p2.TabIndex = 9;
            dataChar_csv_p2.Text = "chart1";
            // 
            // comboBox_graph_p2
            // 
            comboBox_graph_p2.BackColor = Color.Ivory;
            comboBox_graph_p2.Font = new Font("Tahoma", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 238);
            comboBox_graph_p2.ForeColor = Color.DarkBlue;
            comboBox_graph_p2.FormattingEnabled = true;
            comboBox_graph_p2.Location = new Point(17, 273);
            comboBox_graph_p2.Margin = new Padding(2);
            comboBox_graph_p2.Name = "comboBox_graph_p2";
            comboBox_graph_p2.Size = new Size(542, 29);
            comboBox_graph_p2.TabIndex = 8;
            comboBox_graph_p2.Text = "Select a model to evaluate performance";
            // 
            // txtBoxPerformanse_p2
            // 
            txtBoxPerformanse_p2.BackColor = SystemColors.Window;
            txtBoxPerformanse_p2.BorderStyle = BorderStyle.FixedSingle;
            txtBoxPerformanse_p2.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            txtBoxPerformanse_p2.ForeColor = Color.Black;
            txtBoxPerformanse_p2.Location = new Point(569, 306);
            txtBoxPerformanse_p2.Name = "txtBoxPerformanse_p2";
            txtBoxPerformanse_p2.ReadOnly = true;
            txtBoxPerformanse_p2.Size = new Size(265, 268);
            txtBoxPerformanse_p2.TabIndex = 6;
            txtBoxPerformanse_p2.Text = "";
            // 
            // lblBoxMetric_p2
            // 
            lblBoxMetric_p2.AutoSize = true;
            lblBoxMetric_p2.Font = new Font("Tahoma", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 238);
            lblBoxMetric_p2.Location = new Point(564, 25);
            lblBoxMetric_p2.Name = "lblBoxMetric_p2";
            lblBoxMetric_p2.Size = new Size(130, 21);
            lblBoxMetric_p2.TabIndex = 5;
            lblBoxMetric_p2.Text = "Model metrics";
            // 
            // train_TextBox_p2
            // 
            train_TextBox_p2.BackColor = SystemColors.Window;
            train_TextBox_p2.BorderStyle = BorderStyle.FixedSingle;
            train_TextBox_p2.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
            train_TextBox_p2.ForeColor = Color.Black;
            train_TextBox_p2.Location = new Point(16, 51);
            train_TextBox_p2.Name = "train_TextBox_p2";
            train_TextBox_p2.ReadOnly = true;
            train_TextBox_p2.Size = new Size(542, 216);
            train_TextBox_p2.TabIndex = 3;
            train_TextBox_p2.Text = "";
            // 
            // train_label_1_p2
            // 
            train_label_1_p2.AutoSize = true;
            train_label_1_p2.Font = new Font("Tahoma", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 238);
            train_label_1_p2.Location = new Point(17, 25);
            train_label_1_p2.Name = "train_label_1_p2";
            train_label_1_p2.Size = new Size(167, 21);
            train_label_1_p2.TabIndex = 2;
            train_label_1_p2.Text = "Supported models";
            train_label_1_p2.TextAlign = ContentAlignment.TopRight;
            // 
            // train_Button_p2
            // 
            train_Button_p2.BackColor = Color.White;
            train_Button_p2.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 238);
            train_Button_p2.ForeColor = Color.Red;
            train_Button_p2.Location = new Point(389, 21);
            train_Button_p2.Name = "train_Button_p2";
            train_Button_p2.Size = new Size(169, 29);
            train_Button_p2.TabIndex = 1;
            train_Button_p2.Text = "Train the model";
            train_Button_p2.UseVisualStyleBackColor = false;
            // 
            // algorithmSelection_p2
            // 
            algorithmSelection_p2.BackColor = Color.Ivory;
            algorithmSelection_p2.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 238);
            algorithmSelection_p2.ForeColor = Color.DarkBlue;
            algorithmSelection_p2.FormattingEnabled = true;
            algorithmSelection_p2.Location = new Point(190, 21);
            algorithmSelection_p2.Name = "algorithmSelection_p2";
            algorithmSelection_p2.Size = new Size(193, 29);
            algorithmSelection_p2.TabIndex = 0;
            // 
            // FormInterface
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkGray;
            ClientSize = new Size(1539, 840);
            Controls.Add(splitContainer1);
            Name = "FormInterface";
            Text = "FormInterface";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataChar_csv_p1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGrid_csv_p1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataChar_csv_p2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Button button_csv_p1;
        private TextBox textBox_csv_p1;
        private System.Windows.Forms.DataVisualization.Charting.Chart dataChar_csv_p1;
        private TextBox textBox_validation_p1;
        private ComboBox comboBox_graph_p1;
        private DataGridView dataGrid_csv_p1;
        private ComboBox algorithmSelection_p2;
        private Button train_Button_p2;
        private Label train_label_1_p2;
        private RichTextBox train_TextBox_p2;
        private Label uuisu_label_p1;
        private Button button_csv_reset;
        private Label lblBoxMetric_p2;
        private RichTextBox txtBoxPerformanse_p2;
        private System.Windows.Forms.DataVisualization.Charting.Chart dataChar_csv_p2;
        private ComboBox comboBox_graph_p2;
        private Label lblBoxPerformance_p2;
        private RichTextBox txtBoxMetric_p2;
        private Button button_exit;
        private Label lblBoxPerformanceMetric_p2;
        private RichTextBox txtBoxPerformanceMetric_p2;
    }
}