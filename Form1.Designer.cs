namespace BPhO__Plotting_Planck_Spectrum_Task_3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            textBox1 = new TextBox();
            buttonAddLine = new Button();
            checkedListBox1 = new CheckedListBox();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(0, 0);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(788, 447);
            formsPlot1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(528, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(246, 27);
            textBox1.TabIndex = 1;
            textBox1.Text = "Click a line to lock in the slider for it";
            // 
            // buttonAddLine
            // 
            buttonAddLine.Location = new Point(680, 36);
            buttonAddLine.Name = "buttonAddLine";
            buttonAddLine.Size = new Size(94, 29);
            buttonAddLine.TabIndex = 2;
            buttonAddLine.Text = "Add Line";
            buttonAddLine.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[] { "Proxima Centauri - 3050K", "Sun - 5800K", "Sirius - 10000K", "Rigel - 12000K" });
            checkedListBox1.Location = new Point(612, 71);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(162, 114);
            checkedListBox1.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(checkedListBox1);
            Controls.Add(buttonAddLine);
            Controls.Add(textBox1);
            Controls.Add(formsPlot1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot1;
        private TextBox textBox1;
        private Button buttonAddLine;
        private CheckedListBox checkedListBox1;
    }
}
