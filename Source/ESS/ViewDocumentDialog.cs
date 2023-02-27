using Serilog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Covidien.CGRS.ESS
{
    public partial class ViewDocumentDialog : Form
    {
        private readonly List<KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>>> DocumentsList;

        public ViewDocumentDialog()
        {
            InitializeComponent();
            ModelLabel.Text = Properties.Resources.DEV_INFO_MODEL;
            PackageLabel.Text = Properties.Resources.PACKAGES_AVAILABLE_LABEL;
            DocumentsLabel.Text = Properties.Resources.DOCUMENTS_LABEL;
            this.Text = Properties.Resources.CHOOSE_DOCUMENTS_DIALOG;

            DocumentsList = SoftwarePackageManagement.Instance.PreloadedDocuments();
            InitializeModelSelectionComboBox();
        }

        private void InitializeModelSelectionComboBox()
        {
            foreach( KeyValuePair<string, List<KeyValuePair<string,List<KeyValuePair<string,string>>>>> kvp in DocumentsList )
            {
                ModelSelectionComboBox.Items.Add(kvp.Key);
            }
            ModelSelectionComboBox.SelectedIndex = 0;
        }

        private void InitializePackageSelectionComboBox()
        {
            string model = (string)ModelSelectionComboBox.SelectedItem;

            foreach (KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>> kvp in DocumentsList)
            {
                if (kvp.Key == model)
                {
                    PackageSelectionComboBox.Items.Clear();
                    foreach (KeyValuePair<string, List<KeyValuePair<string, string>>> package in kvp.Value)
                    {
                        PackageSelectionComboBox.Items.Add(package.Key);
                    }
                }
            }
            PackageSelectionComboBox.SelectedIndex = 0;
        }

        private void InitializeDocumentSelectionComboBox()
        {
            string model = (string)ModelSelectionComboBox.SelectedItem;
            string package = (string)PackageSelectionComboBox.SelectedItem;

            foreach (KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>> kvp in DocumentsList)
            {
                if (kvp.Key == model)
                {
                    foreach (KeyValuePair<string, List<KeyValuePair<string, string>>> pkg in kvp.Value)
                    {
                        if (pkg.Key == package)
                        {
                            DocumentSelectionComboBox.Items.Clear();
                            foreach (KeyValuePair<string, string> document in pkg.Value)
                            {
                                DocumentSelectionComboBox.Items.Add(document.Key);
                            }
                        }
                    }
                }
            }
            DocumentSelectionComboBox.SelectedIndex = 0;
        }

        private void ModelSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializePackageSelectionComboBox();
        }

        private void PackageSelectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeDocumentSelectionComboBox();
        }

        private void ViewDocumentButton_Click(object sender, EventArgs e)
        {
            string model = (string)ModelSelectionComboBox.SelectedItem;
            string package = (string)PackageSelectionComboBox.SelectedItem;
            string document = (string)DocumentSelectionComboBox.SelectedItem;
            Log.Information($"ViewDocumentDialog:mViewDocumentButton_Click  Entry model:{model} package:{package} document:{document}");

            foreach (KeyValuePair<string, List<KeyValuePair<string, List<KeyValuePair<string, string>>>>> kvp in DocumentsList)
            {
                if (kvp.Key == model)
                {
                    foreach (KeyValuePair<string, List<KeyValuePair<string, string>>> pkg in kvp.Value)
                    {
                        if (pkg.Key == package)
                        {
                            DocumentSelectionComboBox.Items.Clear();
                            foreach (KeyValuePair<string, string> doc in pkg.Value)
                            {
                                if (doc.Key == document)
                                {
                                    NotesViewer viewer = new NotesViewer();
                                    if (viewer.DisplayFile(doc.Value))
                                        viewer.Show();
                                }
                                else
                                {
                                    // document not available...
                                }
                            }
                        }
                    }
                }
            }
            this.Close();
            Log.Information($"ViewDocumentDialog:mViewDocumentButton_Click  Exit model:{model} package:{package} document:{document}");
        }
     }
}
