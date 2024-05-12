using NetworkSimulation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkSimulation.GUI
{
    public class ConnectionForm : Form
    {
        private ComboBox serveurComboBox;
        private NumericUpDown numberUpDown;
        private Button okButton;

        public ConnectionForm(List<Serveur> availableServeurs)
        {
            InitializeComponent();
            PopulateComboBox(availableServeurs);
        }

        private void InitializeComponent()
        {
            this.serveurComboBox = new ComboBox();
            this.numberUpDown = new NumericUpDown();
            this.okButton = new Button();
            this.SuspendLayout();
            // 
            // serveurComboBox
            // 
            this.serveurComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.serveurComboBox.FormattingEnabled = true;
            this.serveurComboBox.Location = new Point(12, 12);
            this.serveurComboBox.Name = "serveurComboBox";
            this.serveurComboBox.Size = new Size(200, 24);
            this.serveurComboBox.TabIndex = 0;
            // 
            // numberUpDown
            // 
            this.numberUpDown.Location = new Point(12, 42);
            this.numberUpDown.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numberUpDown.Name = "numberUpDown";
            this.numberUpDown.Size = new Size(200, 22);
            this.numberUpDown.TabIndex = 1;
            // 
            // okButton
            // 
            this.okButton.Location = new Point(137, 72);
            this.okButton.Name = "okButton";
            this.okButton.Size = new Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += OkButton_Click;
            // 
            // ConnectionForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(224, 107);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.numberUpDown);
            this.Controls.Add(this.serveurComboBox);
            this.Name = "ConnectionForm";
            this.Text = "Connect Serveur";
            this.ResumeLayout(false);
        }

        private void PopulateComboBox(List<Serveur> serveurs)
        {
            serveurComboBox.DataSource = serveurs;
            serveurComboBox.DisplayMember = "ip_adress";
        }

        public Tuple<Serveur, int> GetSelectedServeurAndNumber()
        {
            Serveur selectedServeur = serveurComboBox.SelectedItem as Serveur;
            int selectedNumber = (int)numberUpDown.Value;
            return new Tuple<Serveur, int>(selectedServeur, selectedNumber);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

}
