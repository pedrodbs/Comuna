// ------------------------------------------
// <copyright file="MainForm.cs" company="Pedro Sequeira">
// 
//     Copyright (c) 2018 Pedro Sequeira
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//  
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// </copyright>
// <summary>
//    Project: CommunitiesVisualizer
//    Last updated: 06/18/2018
//    Author: Pedro Sequeira
//    E-mail: pedrodbs@gmail.com
// </summary>
// ------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Comuna;
using Comuna.D3;
using Comuna.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace CommunitiesVisualizer
{
    public partial class MainForm : Form
    {
        #region Static Fields & Constants

        private const int WAIT_TIMEOUT = 3000000;
        private const GraphvizImageType GRAPHVIZ_IMAGE_TYPE = GraphvizImageType.Png;

        #endregion

        #region Fields

        private CommunityAlgorithm _communityAlg;
        private Network _network;
        private PaletteGenerator _paletteGenerator;

        #endregion

        #region Constructors

        public MainForm()
        {
            this.InitializeComponent();
            this.colorSchemeCBox.SelectedIndex = 0;
            this.colorSchemeCBox.SelectedText = string.Empty;
            this.showLabelsCheckBox.Select();
        }

        #endregion

        #region Private & Protected Methods

        private void AlgorithmDoWork(object sender, DoWorkEventArgs e)
        {
            // retrieves community algorithm and updates
            this._communityAlg.Update();

            // returns dot file location
            e.Result = this._communityAlg?.ToGraphvizFile(
                Path.GetFullPath("."), Path.GetFileNameWithoutExtension(this.fileNameLabel.Text),
                this.showLabelsCheckBox.Checked, GRAPHVIZ_IMAGE_TYPE, this._paletteGenerator, WAIT_TIMEOUT);
        }

        private void AlgorithmWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // checks dot
            var dotFile = (string) e.Result;
            if (!File.Exists(dotFile))
            {
                this.infoLabel.Text = "Error: could not generate file!";
                this.EnableInterface(true);
                return;
            }

            // checks and loads image
            var imageFile = $"{dotFile}.{GRAPHVIZ_IMAGE_TYPE.GetOutputFormatStr()}";
            this.pictureBox.Image?.Dispose();
            if (!File.Exists(imageFile))
            {
                this.infoLabel.Text = "Error: could not generate image file! Check that you have Graphviz installed.";
                this.EnableInterface(true);
                return;
            }

            Image img;
            using (var bmpTemp = new Bitmap(imageFile))
                img = new Bitmap(bmpTemp);
            this.pictureBox.Image = img;
            this.infoLabel.Text = $"Image file saved to: \"{imageFile}\".";

            this.exportBtn.Enabled = true;
            this.EnableInterface(true);
        }

        private void EnableInterface(bool enable)
        {
            this.groupBox1.Enabled = this.groupBox2.Enabled = this.groupBox3.Enabled =
                this.groupBox4.Enabled = this.groupBox5.Enabled = enable;
            this.Cursor = enable ? Cursors.Arrow : Cursors.WaitCursor;
            this.showLabelsCheckBox.Select();
        }

        private void SaveImageCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var outputFile = (string) e.Result;
            this.infoLabel.Text = File.Exists(outputFile)
                ? $"Image file saved to: \"{this.saveImageFileDialog.FileName}\"."
                : $"Error: could not save image file to: \"{this.saveImageFileDialog.FileName}\"!";

            this.EnableInterface(true);
        }

        private void SaveImageDoWork(object sender, DoWorkEventArgs e)
        {
            // gets export extension
            var extension = Path.GetExtension(this.saveImageFileDialog.FileName)?.Remove(0, 1) ??
                            GRAPHVIZ_IMAGE_TYPE.GetOutputFormatStr();

            if (extension == "json")
            {
                // converts to d3 json file
                this._communityAlg.ToD3GraphFile(this.saveImageFileDialog.FileName);
                e.Result = this.saveImageFileDialog.FileName;
                return;
            }

            // converts to image accordingly
            var basePath = Path.GetDirectoryName(this.saveImageFileDialog.FileName);
            var fileName = Path.GetFileNameWithoutExtension(this.saveImageFileDialog.FileName);
            if (!Enum.TryParse(extension, true, out GraphvizImageType imgType)) imgType = GRAPHVIZ_IMAGE_TYPE;
            e.Result = this._communityAlg?.ToGraphvizFile(
                basePath, fileName, this.showLabelsCheckBox.Checked, imgType, this._paletteGenerator, WAIT_TIMEOUT);
        }

        #endregion

        #region UI methods

        private void ColorSchemeCBoxIndexChanged(object sender, EventArgs e)
        {
            // checks selected palette
            switch (this.colorSchemeCBox.SelectedIndex)
            {
                case 0:
                    this._paletteGenerator = TolPalettes.CreateTolDivPalette;
                    break;
                default:
                    this._paletteGenerator = TolPalettes.CreateTolRainbowPalette;
                    break;
            }
        }

        private void GenerateBtnClick(object sender, EventArgs e)
        {
            if (this._network == null) return;

            // creates community algorithm
            this._communityAlg = new CommunityAlgorithm(
                this._network, (int) this.numPassesNumUD.Value, (double) this.minModularityNumUD.Value);

            // runs update algorithm
            this.EnableInterface(false);
            this.infoLabel.Text = "Working...";
            this.algorithmWorker.RunWorkerAsync(this._paletteGenerator);
        }

        private void OpenFileBtnClick(object sender, EventArgs e)
        {
            // opens network file
            if (this.openFileDialog.ShowDialog() != DialogResult.OK) return;

            // loads network
            var network = Network.LoadFromCsv(this.openFileDialog.FileName);
            if (network != null)
            {
                this._network = network;
                this._communityAlg = null;
                this.fileNameLabel.Text = Path.GetFileName(this.openFileDialog.FileName);
                this.infoLabel.Text = $"Successfully loaded network: {this._network}.";
                this.EnableInterface(true);
                this.exportBtn.Enabled = false;
                this.pictureBox.Image = null;
            }
            else
            {
                this.infoLabel.Text = $"Error: could not load network in: {this.openFileDialog.FileName}.";
            }
        }

        private void ExportBtnClick(object sender, EventArgs e)
        {
            if (this._communityAlg == null) return;

            // shows dialog
            if (this.saveImageFileDialog.ShowDialog() != DialogResult.OK) return;

            // converts to image
            this.EnableInterface(false);
            this.infoLabel.Text = "Working...";
            this.saveImageWorker.RunWorkerAsync();
        }

        #endregion
    }
}