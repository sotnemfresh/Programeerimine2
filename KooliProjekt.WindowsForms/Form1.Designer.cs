namespace KooliProjekt.WindowsForms
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
            dataGridView1 = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            idField = new TextBox();
            titleField = new TextBox();
            saveCommand = new Button();
            addCommand = new Button();
            deleteCommand = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(525, 300);
            dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(543, 9);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 1;
            label1.Text = "ID:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(543, 61);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 2;
            label2.Text = "Name:";
            // 
            // idField
            // 
            idField.Location = new Point(543, 27);
            idField.Name = "idField";
            idField.ReadOnly = true;
            idField.Size = new Size(199, 23);
            idField.TabIndex = 3;
            idField.Text = "-1";
            // 
            // titleField
            // 
            titleField.Location = new Point(543, 79);
            titleField.Name = "titleField";
            titleField.Size = new Size(199, 23);
            titleField.TabIndex = 4;
            titleField.TextChanged += titleField_TextChanged;
            // 
            // saveCommand
            // 
            saveCommand.Location = new Point(781, 167);
            saveCommand.Name = "saveCommand";
            saveCommand.Size = new Size(75, 26);
            saveCommand.TabIndex = 5;
            saveCommand.Text = "Salvesta";
            saveCommand.UseVisualStyleBackColor = true;
            // 
            // addCommand
            // 
            addCommand.Location = new Point(781, 135);
            addCommand.Name = "addCommand";
            addCommand.Size = new Size(75, 26);
            addCommand.TabIndex = 6;
            addCommand.Text = "Lisa uus";
            addCommand.UseVisualStyleBackColor = true;
            // 
            // deleteCommand
            // 
            deleteCommand.Location = new Point(781, 103);
            deleteCommand.Name = "deleteCommand";
            deleteCommand.Size = new Size(75, 26);
            deleteCommand.TabIndex = 7;
            deleteCommand.Text = "Kustuta";
            deleteCommand.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 338);
            Controls.Add(deleteCommand);
            Controls.Add(addCommand);
            Controls.Add(saveCommand);
            Controls.Add(titleField);
            Controls.Add(idField);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private TextBox idField;
        private TextBox titleField;
        private Button saveCommand;
        private Button addCommand;
        private Button deleteCommand;
    }
}
