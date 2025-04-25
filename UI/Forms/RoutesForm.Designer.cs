namespace BusBuddy.UI.Forms
{
    partial class RoutesForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.routesDataGridView = new System.Windows.Forms.DataGridView();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.defaultBusNumberTextBox = new System.Windows.Forms.TextBox();
            this.defaultDriverNameTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.nameLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.busNumberLabel = new System.Windows.Forms.Label();
            this.driverNameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.routesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // routesDataGridView
            // 
            this.routesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.routesDataGridView.Location = new System.Drawing.Point(12, 12);
            this.routesDataGridView.Name = "routesDataGridView";
            this.routesDataGridView.Size = new System.Drawing.Size(760, 200);
            this.routesDataGridView.TabIndex = 0;
            this.routesDataGridView.SelectionChanged += new System.EventHandler(this.RoutesDataGridView_SelectionChanged);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(150, 230);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(200, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(150, 260);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(200, 20);
            this.descriptionTextBox.TabIndex = 2;
            // 
            // defaultBusNumberTextBox
            // 
            this.defaultBusNumberTextBox.Location = new System.Drawing.Point(150, 290);
            this.defaultBusNumberTextBox.Name = "defaultBusNumberTextBox";
            this.defaultBusNumberTextBox.Size = new System.Drawing.Size(200, 20);
            this.defaultBusNumberTextBox.TabIndex = 3;
            // 
            // defaultDriverNameTextBox
            // 
            this.defaultDriverNameTextBox.Location = new System.Drawing.Point(150, 320);
            this.defaultDriverNameTextBox.Name = "defaultDriverNameTextBox";
            this.defaultDriverNameTextBox.Size = new System.Drawing.Size(200, 20);
            this.defaultDriverNameTextBox.TabIndex = 4;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 400);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 13);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.Text = "Status";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(400, 230);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // editButton
            // 
            this.editButton.Enabled = false;
            this.editButton.Location = new System.Drawing.Point(400, 260);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(75, 23);
            this.editButton.TabIndex = 7;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Location = new System.Drawing.Point(400, 290);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(400, 320);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 9;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(12, 233);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 10;
            this.nameLabel.Text = "Name:";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(12, 263);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(63, 13);
            this.descriptionLabel.TabIndex = 11;
            this.descriptionLabel.Text = "Description:";
            // 
            // busNumberLabel
            // 
            this.busNumberLabel.AutoSize = true;
            this.busNumberLabel.Location = new System.Drawing.Point(12, 293);
            this.busNumberLabel.Name = "busNumberLabel";
            this.busNumberLabel.Size = new System.Drawing.Size(104, 13);
            this.busNumberLabel.TabIndex = 12;
            this.busNumberLabel.Text = "Default Bus Number:";
            // 
            // driverNameLabel
            // 
            this.driverNameLabel.AutoSize = true;
            this.driverNameLabel.Location = new System.Drawing.Point(12, 323);
            this.driverNameLabel.Name = "driverNameLabel";
            this.driverNameLabel.Size = new System.Drawing.Size(115, 13);
            this.driverNameLabel.TabIndex = 13;
            this.driverNameLabel.Text = "Default Driver Name:";
            // 
            // RoutesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.driverNameLabel);
            this.Controls.Add(this.busNumberLabel);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.defaultDriverNameTextBox);
            this.Controls.Add(this.defaultBusNumberTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.routesDataGridView);
            this.Name = "RoutesForm";
            this.Text = "Routes Management";
            ((System.ComponentModel.ISupportInitialize)(this.routesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView routesDataGridView;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox defaultBusNumberTextBox;
        private System.Windows.Forms.TextBox defaultDriverNameTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label busNumberLabel;
        private System.Windows.Forms.Label driverNameLabel;

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            EditRecord();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteRecord();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}